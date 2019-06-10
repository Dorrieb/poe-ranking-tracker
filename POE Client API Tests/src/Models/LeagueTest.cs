using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeApiClient.Models;
using POEToolsTestsBase;
using System;
using System.Text;

namespace PoeApiClientTests.Models
{
    [TestClass]
    public class LeagueTest : BaseUnitTest
    {
        private League league;

        [TestInitialize]
        public void TestSetup()
        {
            string leagueJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.League); ;
            league = JsonConvert.DeserializeObject<League>(leagueJson, GetJsonSettings());
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual("Hardcore", league.Id);
            Assert.AreEqual(Realm.pc, league.Realm);
            Assert.AreEqual("#LeagueHardcore", league.Description);
            Assert.AreEqual("http://pathofexile.com/forum/view-thread/71276", league.Url.AbsoluteUri);
            Assert.AreEqual(new DateTime(2013, 1, 23, 21, 0, 0), league.StartAt);
            Assert.IsNull(league.EndAt);
            Assert.IsNull(league.RegisterAt);
            Assert.IsTrue(league.DelveEvent);
            Assert.IsNull(league.Ladder);
            Assert.IsFalse(league.LeagueEvent);
            Assert.AreEqual(1, league.Rules.Count);
            Assert.AreEqual("Hardcore", league.Rules[0].Id);
            Assert.AreEqual("Hardcore", league.Rules[0].Name);
            Assert.AreEqual("A character killed in Hardcore is moved to its parent league.", league.Rules[0].Description);
        }

        [TestMethod]
        public void Set()
        {
            league.Id = "SSF Hardcore";
            league.Realm = Realm.sony;
            league.Description = "#LeagueHardcoreSSF";
            league.Url = new Uri("http://pathofexile.com/forum/view-thread/1841353");
            league.StartAt = new DateTime(2016, 2, 13, 22, 30, 55);
            league.EndAt = new DateTime(2020, 5, 12, 13, 50, 0);
            league.RegisterAt = new DateTime(2016, 2, 13, 22, 31, 30);
            league.DelveEvent = false;
            league.LeagueEvent = true;
            var rule = new LeagueRule
            {
                Id = "NoParties",
                Name = "Solo",
                Description = "You may not party in this league.",
            };
            league.Rules.Add(rule);

            Assert.AreEqual("SSF Hardcore", league.Id);
            Assert.AreEqual(Realm.sony, league.Realm);
            Assert.AreEqual("#LeagueHardcoreSSF", league.Description);
            Assert.AreEqual("http://pathofexile.com/forum/view-thread/1841353", league.Url.AbsoluteUri);
            Assert.AreEqual(new DateTime(2016, 2, 13, 22, 30, 55), league.StartAt);
            Assert.AreEqual(new DateTime(2020, 5, 12, 13, 50, 0), league.EndAt);
            Assert.AreEqual(new DateTime(2016, 2, 13, 22, 31, 30), league.RegisterAt);
            Assert.IsFalse(league.DelveEvent);
            Assert.IsTrue(league.LeagueEvent);
            Assert.IsNull(league.Ladder);
            Assert.AreEqual(2, league.Rules.Count);
            Assert.AreEqual("Hardcore", league.Rules[0].Id);
            Assert.AreEqual("Hardcore", league.Rules[0].Name);
            Assert.AreEqual("A character killed in Hardcore is moved to its parent league.", league.Rules[0].Description);
            Assert.AreEqual("NoParties", league.Rules[1].Id);
            Assert.AreEqual("Solo", league.Rules[1].Name);
            Assert.AreEqual("You may not party in this league.", league.Rules[1].Description);
        }

        [TestMethod]
        public void StringCheck()
        {
            Assert.AreEqual($"League : {league.Id}", league.ToString());
        }
    }
}
