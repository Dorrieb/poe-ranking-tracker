using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using PoeRankingTracker.Events;
using PoeRankingTracker.Models;

namespace PoeRankingTracker.Services
{
    public interface IHttpClientService
    {
        Task SetSessionId(string sessionId);
        bool SessionIdCorrect();
        void CancelPendingRequests();
        Task<List<League>> GetLeaguesAsync();
        Task<League> GetLeagueAsync(string leagueId);
        Task<Ladder> GetLadderAsync(string leagueId, string accountName);
        Task<Ladder> GetLadderAsync(string leagueId, int offset, int limit);
        Task<List<Entry>> GetEntries(string leagueId, string accountName, string characterName);
        int GetMaxRequestLimit();
        int GetCurrentRequestLimit();
        int GetInterval();
        int GetTimeout();

        event EventHandler<ApiEventArgs> GetEntriesStarted;
        event EventHandler<ApiEventArgs> GetEntriesIncremented;
        event EventHandler<ApiEventArgs> GetEntriesEnded;
        event EventHandler<HttpRequestEventArgs> ProcessGetRequestStarted;
        event EventHandler<HttpRequestEventArgs> ProcessGetRequestEnded;
        event EventHandler<RulesEventArgs> UpdateRulesStarted;
        event EventHandler<RulesEventArgs> UpdateRulesEnded;
        event EventHandler<EventArgs> CancelRequested;
    }

    public class HttpClientService : IHttpClientService, IDisposable
    {
        private readonly CookieContainer cookieContainer = new CookieContainer();
        private HttpClient client;
        private readonly Uri baseUri = new Uri(Properties.Settings.Default.BaseUri);
        private List<RuleApi> rules;
        private List<RuleApi> rulesState;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool sessionIdCorrect = false;
        private readonly ILadderService ladderService;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public HttpClientService(ILadderService ladderService)
        {
            this.ladderService = ladderService;
            CreateClient();
        }

        private void CreateClient(string mediaType = "application/json")
        {
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            client = new HttpClient(handler) { BaseAddress = baseUri };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }

        public async Task SetSessionId(string sessionId)
        {
            if (sessionId.Length > 0)
            {
                try
                {
                    cookieContainer.SetCookies(baseUri, $"POESESSID={sessionId}");

                    // Dummy call to validate session Id
                    await GetLeaguesAsync().ConfigureAwait(true);
                }
                catch (CookieException e)
                {
                    logger.Error(e, "Failed to set cookie");
                    sessionIdCorrect = false;
                }
            }
        }

        public void CancelPendingRequests()
        {
            OnCancelRequested();
            client.CancelPendingRequests();
        }

        public async Task<List<Entry>> GetEntries(string leagueId, string accountName, string characterName)
        {
            logger.Debug("GetEntries");
            List<Entry> entries = new List<Entry>();
            try
            {
                Ladder ladder = await GetLadderAsync(leagueId, accountName).ConfigureAwait(true);
                int rank = ladderService.GetRank(ladder, characterName);
                int limit = 200;
                int offset = -1;
                int tasksNumber = (int)Math.Ceiling((double)rank / limit);
                OnGetEntriesStarted(tasksNumber);

                tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                Task[] tasks = new Task[tasksNumber];
                for (int i = 0; i < tasksNumber; i++)
                {
                    tasks[i] = Task.Run(async () =>
                    {
                        var offsetNow = Interlocked.Increment(ref offset);
                        var nextLadder = await GetLadderAsync(leagueId, limit * offsetNow, limit).ConfigureAwait(true);
                        if (nextLadder != null)
                        {
                            entries.AddRange(nextLadder.Entries);
                        }
                        OnGetEntriesIncremented();
                    }, token);
                }
                await Task.WhenAll(tasks).ConfigureAwait(true);

                OnGetEntriesEnded();

                entries.Sort((Entry x, Entry y) =>
                {
                    return x.Rank - y.Rank;
                });
            }
            catch (TaskCanceledException e)
            {
                logger.Debug(e, "Task cancelled");
            }

            return entries;
        }

        public async Task<Ladder> GetLadderAsync(string leagueId, string accountName)
        {
            Ladder ladder = null;
            var uri = new Uri(baseUri, string.Format(CultureInfo.CurrentCulture, Properties.Settings.Default.LaddersPath, leagueId, accountName));
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                ladder = await response.Content.ReadAsAsync<Ladder>(new List<MediaTypeFormatter>() { new LadderFormatter() }).ConfigureAwait(false);
                // When requesting with an invalid account name, dummy entries are returned with multiple account names
                if (ladder.Entries.Count > 0 && ladder.Entries[0].Account != null)
                {
                    ladder = null;
                }
            }

            return ladder;
        }

        public async Task<Ladder> GetLadderAsync(string leagueId, int offset, int limit)
        {
            Ladder ladder = null;
            var uri = new Uri(baseUri, string.Format(CultureInfo.CurrentCulture, Properties.Settings.Default.LaddersAllPath, leagueId, offset, limit));
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                ladder = await response.Content.ReadAsAsync<Ladder>().ConfigureAwait(false);
            }
            return ladder;
        }

        public async Task<League> GetLeagueAsync(string leagueId)
        {
            League league = null;

            if (leagueId.Length > 0)
            {
                var uri = new Uri(baseUri, $"{Properties.Settings.Default.LeaguesPath}/{leagueId}");
                HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
                if (response != null)
                {
                    league = await response.Content.ReadAsAsync<League>().ConfigureAwait(false);
                }
            }

            return league;
        }

        public async Task<List<League>> GetLeaguesAsync()
        {
            List<League> leagues = null;

            var uri = new Uri(baseUri, Properties.Settings.Default.LeaguesPath);
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                leagues = await response.Content.ReadAsAsync<List<League>>().ConfigureAwait(false);
            }

            return leagues;
        }

        private async Task<HttpResponseMessage> ProcessGetRequest(Uri uri)
        {
            logger.Debug($"Process GET request uri={uri} - start");
            OnProcessGetRequestStarted();
            HttpResponseMessage response;

            logger.Debug($"Process GET request uri={uri} - begin");

            response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            int timeout = GetTimeout();
            if (timeout > 0)
            {
                await Task.Delay(timeout * 1000).ConfigureAwait(true);
            }
            UpdateRules(response.Headers);

            if (response.IsSuccessStatusCode)
            {
                OnProcessGetRequestEnded(true);
                return response;
            }
            else if (429 == (int)response.StatusCode)
            {
                OnProcessGetRequestEnded(false);
                // Retry after waiting the appropriate time
                int delay;
                if (response.Headers.RetryAfter != null)
                {
                    delay = (int)response.Headers.RetryAfter.Delta.Value.TotalMilliseconds;
                }
                else
                {
                    delay = GetTimeout() * 1000;
                }
                logger.Debug($"Sleep for {delay}");
                Thread.Sleep(delay);

                return await ProcessGetRequest(uri).ConfigureAwait(false);
            }
            else
            {
                OnProcessGetRequestEnded(false);
                logger.Error($"Request error for uri {uri.AbsoluteUri} failed with code {response.StatusCode}");
                return null;
            }
        }

        private void UpdateRules(HttpResponseHeaders headers)
        {
            OnUpdateRulesStarted(rules, rulesState);
            rules = GetRules(headers, "x-rate-limit-account");

            if (rules != null)
            {
                rulesState = GetRules(headers, "x-rate-limit-account-state");
                sessionIdCorrect = true;
            }
            else
            {
                rules = GetRules(headers, "x-rate-limit-ip");
                rulesState = GetRules(headers, "x-rate-limit-ip-state");
                sessionIdCorrect = false;
            }

            if (rules != null)
            {
                var rulesStr = string.Join(",", rules);
                var rulesStateStr = string.Join(",", rulesState);
                OnUpdateRulesEnded(rules, rulesState);
                logger.Debug("Rules : {0} - Rules state : {1}", rulesStr, rulesStateStr);
            }
            else
            {
                logger.Debug("No rules returned in response");
            }
        }

        private List<RuleApi> GetRules(HttpResponseHeaders headers, string value)
        {
            if (!headers.Contains(value))
            {
                return null;
            }

            var headersValue = headers.GetValues(value);
            var rulesStr = headersValue.FirstOrDefault();
            var rulesArray = rulesStr.Split(',');
            var rules = new List<RuleApi>();
            foreach (var ruleStr in rulesArray)
            {
                var data = ruleStr.Split(':');
                int requestLimit = Convert.ToInt32(data[0], CultureInfo.CurrentCulture);
                int interval = Convert.ToInt32(data[1], CultureInfo.CurrentCulture);
                int timeout = Convert.ToInt32(data[2], CultureInfo.CurrentCulture);
                RuleApi ruleApi = new RuleApi(requestLimit, interval, timeout);
                rules.Add(ruleApi);
            }

            return rules;
        }

        public bool SessionIdCorrect()
        {
            return sessionIdCorrect;
        }

        public int GetCurrentRequestLimit()
        {
            int requestLimit = int.MaxValue;

            if (rulesState != null)
            {
                foreach (var rule in rulesState)
                {
                    if (rule.RequestLimit < requestLimit)
                    {
                        requestLimit = rule.RequestLimit;
                    }
                }
            }

            return requestLimit;
        }

        public int GetMaxRequestLimit()
        {
            int requestLimit = int.MaxValue;

            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    if (rule.RequestLimit < requestLimit)
                    {
                        requestLimit = rule.RequestLimit;
                    }
                }
            }

            return requestLimit;
        }

        public int GetInterval()
        {
            int interval = 0;

            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    if (rule.Interval > interval)
                    {
                        interval = rule.Interval;
                    }
                }
            }

            return interval;
        }

        public int GetTimeout()
        {
            int timeout = 0;

            if (rulesState != null)
            {
                foreach (var rule in rulesState)
                {
                    if (rule.Timeout > timeout)
                    {
                        timeout = rule.Timeout;
                    }
                }
            }

            return timeout;
        }

        private void OnGetEntriesStarted(int value)
        {
            ApiEventArgs args = new ApiEventArgs
            {
                Value = value
            };
            GetEntriesStarted?.Invoke(this, args);
        }

        private void OnGetEntriesIncremented()
        {
            ApiEventArgs args = new ApiEventArgs
            {
                Value = 1
            };
            GetEntriesIncremented?.Invoke(this, args);
        }

        private void OnGetEntriesEnded()
        {
            ApiEventArgs args = new ApiEventArgs();
            GetEntriesEnded?.Invoke(this, args);
        }

        private void OnProcessGetRequestStarted()
        {
            HttpRequestEventArgs args = new HttpRequestEventArgs();
            ProcessGetRequestStarted?.Invoke(this, args);
        }

        private void OnProcessGetRequestEnded(bool success)
        {
            HttpRequestEventArgs args = new HttpRequestEventArgs {
                Success = success,
                Rules = rules,
                RulesState = rulesState,
            };
            ProcessGetRequestEnded?.Invoke(this, args);
        }

        private void OnUpdateRulesStarted(List<RuleApi> rules, List<RuleApi> rulesStates)
        {
            RulesEventArgs args = new RulesEventArgs
            {
                Rules = rules,
                RulesState = rulesStates,
            };
            UpdateRulesStarted?.Invoke(this, args);
        }

        private void OnUpdateRulesEnded(List<RuleApi> rules, List<RuleApi> rulesStates)
        {
            RulesEventArgs args = new RulesEventArgs
            {
                Rules = rules,
                RulesState = rulesStates,
            };
            UpdateRulesEnded?.Invoke(this, args);
        }

        private void OnCancelRequested()
        {
            EventArgs args = new EventArgs();
            CancelRequested?.Invoke(this, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
                tokenSource.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event EventHandler<ApiEventArgs> GetEntriesStarted;
        public event EventHandler<ApiEventArgs> GetEntriesIncremented;
        public event EventHandler<ApiEventArgs> GetEntriesEnded;
        public event EventHandler<HttpRequestEventArgs> ProcessGetRequestStarted;
        public event EventHandler<HttpRequestEventArgs> ProcessGetRequestEnded;
        public event EventHandler<RulesEventArgs> UpdateRulesStarted;
        public event EventHandler<RulesEventArgs> UpdateRulesEnded;
        public event EventHandler<EventArgs> CancelRequested;
    }
}
