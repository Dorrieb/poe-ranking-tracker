using PoeRankingTracker.Events;
using PoeRankingTracker.Models;
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
using System.Timers;

namespace PoeRankingTracker.Services
{
    public class ApiService : IApiService, IDisposable
    {
        private readonly CookieContainer cookieContainer = new CookieContainer();
        private HttpClient client;
        private readonly Uri baseUri = new Uri(Properties.Settings.Default.BaseUri);
        private List<RuleApi> rules;
        private List<RuleApi> rulesState;
        private System.Timers.Timer timer;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private SemaphoreSlim semaphore;
        private int semaphoreNumber;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private bool sessionIdCorrect = false;
        private readonly ILadderService ladderService;

        public ApiService(ILadderService ladderService)
        {
            this.ladderService = ladderService;
            CreateClient();
        }

        private void CreateClient()
        {
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            client = new HttpClient(handler) { BaseAddress = baseUri };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void InitializeTimer()
        {
            if (timer == null)
            {
                OnTimer(null, null);
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimer;
                timer.AutoReset = true;
                timer.Interval = rulesState[0].Interval * 1000;
                timer.Enabled = true;
            }
        }

        private void OnTimer(Object source, ElapsedEventArgs e)
        {
            int timeout = GetTimeout();
            if (timeout == 0)
            {
                ReleaseSemaphore();
            }
            else
            {
                timer.Stop();
                Thread.Sleep(timeout * 1000);
                ResetRequestLimit();
                ReleaseSemaphore();
                timer.Start();
            }
        }

        private int GetTimeout()
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

        private void ResetRequestLimit()
        {
            if (rulesState != null)
            {
                foreach (var rule in rulesState)
                {
                    rule.RequestLimit = 0;
                }
            }
        }

        public async Task SetSessionId(string sessionId)
        {
            if (sessionId.Length > 0)
            {
                try
                {
                    cookieContainer.SetCookies(baseUri, $"POESESSID={sessionId}");
                }
                catch (CookieException e)
                {
                    logger.Error(e, "Failed to set cookie");
                    sessionIdCorrect = false;
                }
            }
            // Dummy call to validate session Id
            await GetLeaguesAsync().ConfigureAwait(true);
        }

        public async Task<List<League>> GetLeaguesAsync()
        {
            List<League> leagues = null;

            var uri = new Uri(baseUri, Properties.Settings.Default.LeaguesPath);
            HttpResponseMessage response = await ProcessGetRequest(uri, false).ConfigureAwait(false);
            if (response != null)
            {
                leagues = await response.Content.ReadAsAsync<List<League>>().ConfigureAwait(false);
            }

            return leagues;
        }

        public async Task<League> GetLeagueAsync(string leagueId)
        {
            League league = null;

            if (leagueId.Length > 0)
            {
                var uri = new Uri(baseUri, $"{Properties.Settings.Default.LeaguesPath}/{leagueId}");
                HttpResponseMessage response = await ProcessGetRequest(uri, false).ConfigureAwait(false);
                if (response != null)
                {
                    league = await response.Content.ReadAsAsync<League>().ConfigureAwait(false);
                }
            }

            return league;
        }

        public async Task<Ladder> GetLadderAsync(string leagueId, string accountName)
        {
            Ladder ladder = null;
            var uri = new Uri(baseUri, string.Format(CultureInfo.CurrentCulture, Properties.Settings.Default.LaddersPath, leagueId, accountName));
            HttpResponseMessage response = await ProcessGetRequest(uri, false).ConfigureAwait(false);
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

                if (semaphore == null)
                {
                    CreateSemaphore();
                }
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

        public void CancelTasks()
        {
            logger.Debug("Cancel tasks");
            timer.Stop();
            tokenSource.Cancel();
            client.CancelPendingRequests();
            if (semaphore != null)
            {
                semaphore.Dispose();
                semaphore = null;
            }
        }

        private void ReleaseSemaphore()
        {
            if (semaphore != null && !tokenSource.IsCancellationRequested)
            {
                int i = semaphoreNumber - semaphore.CurrentCount - rulesState[0].RequestLimit;
                if (i > 0)
                {
                    semaphore.Release(i);
                }
                else if (semaphore.CurrentCount == 0)
                {
                    rulesState[0].RequestLimit = 0;
                    OnTimer(null, null);
                }
            }
        }

        private async Task<HttpResponseMessage> ProcessGetRequest(Uri uri, bool withSemaphore = true)
        {
            HttpResponseMessage response;
            if (withSemaphore && semaphore != null)
            {
                semaphore.Wait();
            }

            logger.Debug($"Process GET request uri={uri}");
            response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            int timeout = GetTimeout();
            if (timeout > 0)
            {
                await Task.Delay(timeout * 1000).ConfigureAwait(true);
            }
            UpdateRules(response.Headers);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else if (429 == (int)response.StatusCode)
            {
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
                logger.Error($"Request error for uri {uri.AbsoluteUri} failed with code {response.StatusCode}");
                return null;
            }
        }

        private void UpdateRules(HttpResponseHeaders headers)
        {
            timer?.Stop();
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
                logger.Debug("Rules : {0} - Rules state : {1}", rulesStr, rulesStateStr);
            }
            else
            {
                logger.Debug("No rules returned in response");
            }
            timer?.Start();
        }

        private void CreateSemaphore()
        {
            if (semaphore != null)
            {
                logger.Debug("Destroy semaphore");
                semaphore.Dispose();
            }
            semaphoreNumber = rules[0].RequestLimit;
            logger.Debug($"Create semaphore with max = {semaphoreNumber}");
            semaphore = new SemaphoreSlim(0, semaphoreNumber);
            InitializeTimer();
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
                timer?.Dispose();
                semaphore?.Dispose();
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
    }
}
