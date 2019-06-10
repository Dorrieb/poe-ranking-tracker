using Castle.Windsor;
using Castle.Windsor.Installer;
using Newtonsoft.Json;
using PoeApiClient.Converters;
using PoeApiClient.Events;
using PoeApiClient.Formatters;
using PoeApiClient.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace PoeApiClient.Services
{
    public interface IHttpClientService
    {
        Task SetSessionId(string sessionId);
        bool SessionIdCorrect();
        void CancelPendingRequests();
        Task<List<ILeague>> GetLeaguesAsync();
        Task<ILeague> GetLeagueAsync(string leagueId);
        Task<ILadder> GetLadderAsync(string leagueId, string accountName);
        Task<ILadder> GetLadderAsync(string leagueId, int offset, int limit);
        Task<List<IEntry>> GetEntries(string leagueId, string accountName, string characterName, int rank);
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
        private HttpClient client;
        private readonly Uri baseUri = new Uri(Properties.Settings.Default.BaseUri);
        private List<IRuleApi> rules;
        private List<IRuleApi> rulesState;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool sessionIdCorrect = false;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private MediaTypeFormatterCollection formatters;
        private IWindsorContainer container;
        private string sessionId;
        private HttpClientHandler handler;

        public HttpClientService(HttpClient httpClient = null)
        {
            CreateWindsorContainer();
            InitializeConverters();
            CreateClient(httpClient);
        }

        private void InitializeConverters()
        {
            List<JsonConverter> converters = new List<JsonConverter>
            {
                container.Resolve<LeagueConverter>(),
                container.Resolve<LeagueRuleConverter>(),
                container.Resolve<LadderConverter>(),
                container.Resolve<EntryConverter>(),
                container.Resolve<CharacterConverter>(),
                container.Resolve<AccountConverter>(),
                container.Resolve<ChallengesConverter>(),
            };

            formatters = new MediaTypeFormatterCollection();
            foreach (var converter in converters)
            {
                formatters.JsonFormatter.SerializerSettings.Converters.Add(converter);
            }
        }

        private void CreateClient(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                handler = new HttpClientHandler()
                {
                    UseCookies = false
                };
                client = new HttpClient(handler)
                {
                    BaseAddress = baseUri
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                client = httpClient;
            }
        }

        private void CreateWindsorContainer()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(
                    FromAssembly.This()
                 );
#pragma warning disable CA2000
        }

        public async Task SetSessionId(string sessionId)
        {
            Contract.Requires(sessionId != null);

            if (sessionId.Length > 0)
            {
                try
                {
                    this.sessionId = sessionId;

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

        public async Task<List<IEntry>> GetEntries(string leagueId, string accountName, string characterName, int rank)
        {
            logger.Debug("GetEntries");
            List<IEntry> entries = new List<IEntry>();
            ILadder ladder = await GetLadderAsync(leagueId, accountName).ConfigureAwait(true);
            int limit = 200;
            int offset = -1;
            int tasksNumber = (int)Math.Ceiling((double)rank / limit);
            OnGetEntriesStarted(tasksNumber);
            ConcurrentBag<List<IEntry>> bag = new ConcurrentBag<List<IEntry>>();

            try
            {
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
                            bag.Add(nextLadder.Entries);
                        }
                        OnGetEntriesIncremented();
                    }, token);
                }
                await Task.WhenAll(tasks).ConfigureAwait(true);

                OnGetEntriesEnded();

                foreach(var nextEntries in bag)
                {
                    entries.AddRange(nextEntries);
                }

                entries.Sort((IEntry x, IEntry y) =>
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

        public async Task<ILadder> GetLadderAsync(string leagueId, string accountName)
        {
            Ladder ladder = null;
            var uri = new Uri(baseUri, string.Format(CultureInfo.CurrentCulture, Properties.Settings.Default.LaddersPath, leagueId, accountName));
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                var formatter = container.Resolve<LadderFormatter>();
                formatters.Add(formatter);
                ladder = await response.Content.ReadAsAsync<Ladder>(formatters).ConfigureAwait(false);
                formatters.Remove(formatter);
                // When requesting with an invalid account name, dummy entries are returned with multiple account names
                if (ladder.Entries.Count > 0 && ladder.Entries[0].Account != null)
                {
                    ladder = null;
                }
            }

            return ladder;
        }

        public async Task<ILadder> GetLadderAsync(string leagueId, int offset, int limit)
        {
            Ladder ladder = null;
            var uri = new Uri(baseUri, string.Format(CultureInfo.CurrentCulture, Properties.Settings.Default.LaddersAllPath, leagueId, offset, limit));
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                ladder = await response.Content.ReadAsAsync<Ladder>(formatters).ConfigureAwait(false);
            }
            return ladder;
        }

        public async Task<ILeague> GetLeagueAsync(string leagueId)
        {
            Contract.Requires(leagueId != null);

            League league = null;

            if (leagueId.Length > 0)
            {
                var uri = new Uri(baseUri, $"{Properties.Settings.Default.LeaguesPath}/{leagueId}");
                HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
                if (response != null)
                {
                    league = await response.Content.ReadAsAsync<League>(formatters).ConfigureAwait(false);
                }
            }

            return league;
        }

        public async Task<List<ILeague>> GetLeaguesAsync()
        {
            List<ILeague> leagues = null;

            var uri = new Uri(baseUri, Properties.Settings.Default.LeaguesPath);
            HttpResponseMessage response = await ProcessGetRequest(uri).ConfigureAwait(false);
            if (response != null)
            {
                leagues = await response.Content.ReadAsAsync<List<ILeague>>(formatters).ConfigureAwait(false);
            }

            return leagues;
        }

        private async Task<HttpResponseMessage> ProcessGetRequest(Uri uri)
        {
            logger.Debug($"Process GET request uri={uri} - start");
            OnProcessGetRequestStarted();
            HttpResponseMessage response;

            logger.Debug($"Process GET request uri={uri} - begin");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            message.Headers.Add("Cookie", $"POESESSID={sessionId}");
            response = await client.SendAsync(message, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

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
                CancelPendingRequests();
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
                if (delay > 0)
                {
                    logger.Debug($"Sleep for {delay}");
                    Thread.Sleep(delay);
                }

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

        private static List<IRuleApi> GetRules(HttpResponseHeaders headers, string value)
        {
            if (!headers.Contains(value))
            {
                return null;
            }

            var headersValue = headers.GetValues(value);
            var rulesStr = headersValue.FirstOrDefault();
            var rulesArray = rulesStr.Split(',');
            var rules = new List<IRuleApi>();
            foreach (var ruleStr in rulesArray)
            {
                var data = ruleStr.Split(':');
                int requestLimit = Convert.ToInt32(data[0], CultureInfo.CurrentCulture);
                int interval = Convert.ToInt32(data[1], CultureInfo.CurrentCulture);
                int timeout = Convert.ToInt32(data[2], CultureInfo.CurrentCulture);
                IRuleApi ruleApi = new RuleApi(requestLimit, interval, timeout);
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
            else
            {
                requestLimit = 0;
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
            else
            {
                requestLimit = 0;
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
            HttpRequestEventArgs args = new HttpRequestEventArgs
            {
                Success = success,
                Rules = rules,
                RulesState = rulesState,
            };
            ProcessGetRequestEnded?.Invoke(this, args);
        }

        private void OnUpdateRulesStarted(List<IRuleApi> rules, List<IRuleApi> rulesStates)
        {
            RulesEventArgs args = new RulesEventArgs
            {
                Rules = rules,
                RulesState = rulesStates,
            };
            UpdateRulesStarted?.Invoke(this, args);
        }

        private void OnUpdateRulesEnded(List<IRuleApi> rules, List<IRuleApi> rulesStates)
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
                client?.Dispose();
                tokenSource?.Dispose();
                container?.Dispose();
                handler?.Dispose();
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
