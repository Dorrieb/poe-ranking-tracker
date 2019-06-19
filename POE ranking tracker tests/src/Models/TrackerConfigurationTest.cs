using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeApiClient.Models;
using PoeRankingTracker.Models;
using POEToolsTestsBase;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class TrackerConfigurationTest : BaseUnitTest
    {
        private TrackerConfiguration config;
        private League league;
        private readonly string accountName = "morinfa";

        [TestInitialize]
        public void TestSetup()
        {
            string leagueJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.LeagueWithEntries);
            league = JsonConvert.DeserializeObject<League>(leagueJson, GetJsonSettings());

            config = new TrackerConfiguration
            {
                League = league,
                Entry = league.Ladder.Entries[0],
                AccountName = accountName,
                Culture = CultureInfo.CurrentCulture,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(league, config.League);
            Assert.AreEqual(league.Ladder.Entries[0], config.Entry);
            Assert.AreEqual(accountName, config.AccountName);
            Assert.AreEqual(CultureInfo.CurrentCulture, config.Culture);
        }

        [TestMethod]
        public void Set()
        {
            string leagueJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.League); ;
            ILeague league2 = JsonConvert.DeserializeObject<ILeague>(leagueJson, GetJsonSettings());
            IEntry entry2 = league.Ladder.Entries[1];
            string accountName2 = "morinfa2";

            config.League = league2;
            config.Entry = entry2;
            config.AccountName = accountName2;
            config.Culture = CultureInfo.InvariantCulture;

            Assert.AreEqual(league2, config.League);
            Assert.AreEqual(entry2, config.Entry);
            Assert.AreEqual(accountName2, config.AccountName);
            Assert.AreEqual(CultureInfo.InvariantCulture, config.Culture);
        }
    }
}
