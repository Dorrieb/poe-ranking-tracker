using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PoeApiClient.Models;
using PoeApiClient.Services;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoeApiClientTests.Services
{
    [TestClass]
    public class HttpClientServiceTest : IDisposable
    {
        private HttpClient client;
        private HttpClientService service;

        [TestMethod]
        public void SetSessionIdEmpty()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.SetSessionId("");
            Assert.IsFalse(service.SessionIdCorrect());
        }

        [TestMethod]
        public void SetSessionIdInvalid()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.SetSessionId("invalid");
            Assert.IsFalse(service.SessionIdCorrect());
        }

        [TestMethod]
        public void GetLeaguesAsync()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            List<ILeague> leagues = service.GetLeaguesAsync().Result;
            Assert.AreEqual(4, leagues.Count);
        }

        [TestMethod]
        public void GetLeagueAsync()
        {
            var leagueJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.League);
            string leagueId = "Hardcore";
            var url = $"https://api.pathofexile.com/leagues/{leagueId}";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leagueJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            ILeague league = service.GetLeagueAsync(leagueId).Result;
            Assert.AreEqual(leagueId, league.Id);
        }

        [TestMethod]
        public void GetLadderAsyncDummyEntries()
        {
            var ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Ladder);
            string leagueId = "Hardcore";
            string accountName = "morinfa";
            var url = $"https://api.pathofexile.com/ladders/{leagueId}?accountName={accountName}";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = ladderJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            ILadder ladder = service.GetLadderAsync(leagueId, accountName).Result;
            Assert.IsNull(ladder);
        }

        [TestMethod]
        public void GetLadderAsync()
        {
            var ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.LadderMorinfa);
            string leagueId = "Hardcore";
            string accountName = "morinfa";
            var url = $"https://api.pathofexile.com/ladders/{leagueId}?accountName={accountName}";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = ladderJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            ILadder ladder = service.GetLadderAsync(leagueId, accountName).Result;
            Assert.AreEqual(12, ladder.Entries.Count);
        }

        [TestMethod]
        public void GetLadderAsyncOffsetAndLimit()
        {
            var ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.LadderOffset1Limit2);
            string leagueId = "Hardcore";
            var url = $"https://api.pathofexile.com/ladders/{leagueId}?offset=1&limit=2";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = ladderJson,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            ILadder ladder = service.GetLadderAsync(leagueId, 1, 2).Result;
            Assert.AreEqual(2, ladder.Entries.Count);
        }

        [TestMethod]
        public void GetEntries()
        {
            string leagueId = "Standard";
            string accountName = "morinfa";
            string characterName = "Lawyne";
            int rank = 591;
            var url1 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=0&limit=200";
            var url2 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=200&limit=200";
            var url3 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=400&limit=200";
            var ladder1Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset0Limit200);
            var ladder2Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset200Limit200);
            var ladder3Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset400Limit200);
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url1,
                    Response = ladder1Json,
                    StatusCode = HttpStatusCode.OK,
                },
                new UrlWithResponse()
                {
                    Url = url2,
                    Response = ladder2Json,
                    StatusCode = HttpStatusCode.OK,
                },
                new UrlWithResponse()
                {
                    Url = url3,
                    Response = ladder3Json,
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            var entries = service.GetEntries(leagueId, accountName, characterName, rank).Result;
            Assert.AreEqual(600, entries.Count);
        }

        [TestMethod]
        public void CancelPendingRequestsEvent()
        {
            var ok = false;
            string leagueId = "Standard";
            string accountName = "morinfa";
            string characterName = "Lawyne";
            int rank = 591;
            var url1 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=0&limit=200";
            var url2 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=200&limit=200";
            var url3 = $"https://api.pathofexile.com/ladders/{leagueId}?offset=400&limit=200";
            var ladder1Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset0Limit200);
            var ladder2Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset200Limit200);
            var ladder3Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.StandardOffset400Limit200);
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url1,
                    Response = ladder1Json,
                    StatusCode = HttpStatusCode.OK,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 0),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                },
                new UrlWithResponse()
                {
                    Url = url2,
                    Response = ladder2Json,
                    StatusCode = HttpStatusCode.OK,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(11, 5, 10),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                },
                new UrlWithResponse()
                {
                    Url = url3,
                    Response = ladder3Json,
                    StatusCode = HttpStatusCode.OK,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(11, 6, 10),
                        new RuleApi(8, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                }
            };
            service = GetService(list);
            var entries = service.GetEntries(leagueId, accountName, characterName, rank).Result;
            service.CancelRequested += delegate (object sender, EventArgs args)
            {
                ok = true;
            };
            service.CancelPendingRequests();
            Assert.IsTrue(ok);
            // TODO
        }

        [TestMethod]
        public void MaxRequestLimitInitialAtZero()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                }
            };
            service = GetService(list);
            int max = service.GetMaxRequestLimit();
            Assert.AreEqual(0, max);
        }

        [TestMethod]
        public void MaxRequestLimitDefault()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(1, 5, 0),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.GetLeaguesAsync().Result;
            int max = service.GetMaxRequestLimit();
            Assert.AreEqual(10, max);
        }

        [TestMethod]
        public void CurrentRequestLimitInitialAtZero()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                }
            };
            service = GetService(list);
            int current = service.GetCurrentRequestLimit();
            Assert.AreEqual(0, current);
        }

        [TestMethod]
        public void CurrentRequestLimitDefault()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(1, 5, 0),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.GetLeaguesAsync().Result;
            int current = service.GetCurrentRequestLimit();
            Assert.AreEqual(1, current);
        }

        [TestMethod]
        public void GetIntervalInitialAtZero()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                }
            };
            service = GetService(list);
            int interval = service.GetInterval();
            Assert.AreEqual(0, interval);
        }

        [TestMethod]
        public void GetIntervalDefault()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(1, 5, 0),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.GetLeaguesAsync().Result;
            int interval = service.GetInterval();
            Assert.AreEqual(10, interval);
        }

        [TestMethod]
        public void GetTimeoutInitialAtZero()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                }
            };
            service = GetService(list);
            int interval = service.GetTimeout();
            Assert.AreEqual(0, interval);
        }

        [TestMethod]
        public void GetTimeoutDefault()
        {
            var leaguesJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Leagues);
            var url = "https://api.pathofexile.com/leagues";
            var list = new List<UrlWithResponse>()
            {
                new UrlWithResponse()
                {
                    Url = url,
                    Response = leaguesJson,
                    Rule = new List<RuleApi>()
                    {
                        new RuleApi(10, 5, 10),
                        new RuleApi(20, 1, 30),
                        new RuleApi(30, 10, 300),
                    },
                    RuleState = new List<RuleApi>()
                    {
                        new RuleApi(1, 5, 10),
                        new RuleApi(1, 10, 0),
                        new RuleApi(1, 10, 0),
                    },
                    StatusCode = HttpStatusCode.OK,
                }
            };
            service = GetService(list);
            _ = service.GetLeaguesAsync().Result;
            int interval = service.GetTimeout();
            Assert.AreEqual(10, interval);
        }

        private HttpClientService GetService(List<UrlWithResponse> list)
        {
            using (var mockHttp = new MockHttpMessageHandler())
            {
                foreach (var data in list)
                {
                    var mock = mockHttp.When(data.Url).Respond(async request =>
                    {
                        var response = new HttpResponseMessage(data.StatusCode)
                        {
                            Content = new StringContent(data.Response, Encoding.UTF8, "application/json")
                        };

                        // Adjust your response headers here
                        if (data.Rule != null)
                        {
                            var key = "x-rate-limit-account";
                            var value = String.Join(",", data.Rule);
                            response.Headers.Add(key, value);
                        }

                        if (data.RuleState != null)
                        {
                            var key = "x-rate-limit-account-state";
                            var value = String.Join(",", data.RuleState);
                            response.Headers.Add(key, value);
                        }

                        if (data.Delay.HasValue)
                        {
                            await Task.Delay(data.Delay.Value * 1000).ConfigureAwait(true);
                        }

                        return response;
                    });
                }

                client = new HttpClient(mockHttp);
                service = new HttpClientService(client);
                return service;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            client?.Dispose();
            service?.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client?.Dispose();
                service?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    class UrlWithResponse
    {
        public string Url { get; set; }
        public string Response { get; set; }
        public List<RuleApi> Rule { get; set; }
        public List<RuleApi> RuleState { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int? Delay { get; set; }
    }
}
