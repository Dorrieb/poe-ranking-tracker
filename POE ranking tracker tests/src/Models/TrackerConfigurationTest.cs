using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeRankingTracker.Models;
using System.Drawing;
using System.Text;
using System.Globalization;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class TrackerConfigurationTest
    {
        private TrackerConfiguration config;
        private League league;
        private readonly Font font = new Font("Arial", 8);
        private readonly Color fontColor = Color.Azure;
        private readonly Color backgroundColor = Color.Black;
        private readonly string accountName = "morinfa";

        [TestInitialize]
        public void TestSetup()
        {
            string leagueJson = Encoding.UTF8.GetString(Properties.Resources.LeagueWithEntries); ;
            league = JsonConvert.DeserializeObject<League>(leagueJson);

            config = new TrackerConfiguration
            {
                League = league,
                Entry = league.Ladder.Entries[0],
                Font = font,
                FontColor = fontColor,
                BackgroundColor = backgroundColor,
                ShowDeadsAhead = true,
                ShowExperienceAhead = true,
                ShowExperienceBehind = true,
                ShowProgressBar = true,
                AccountName = accountName,
                Culture = CultureInfo.CurrentCulture,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(league, config.League);
            Assert.AreEqual(league.Ladder.Entries[0], config.Entry);
            Assert.AreEqual(font, config.Font);
            Assert.AreEqual(fontColor, config.FontColor);
            Assert.AreEqual(backgroundColor, config.BackgroundColor);
            Assert.IsTrue(config.ShowDeadsAhead);
            Assert.IsTrue(config.ShowExperienceAhead);
            Assert.IsTrue(config.ShowExperienceBehind);
            Assert.IsTrue(config.ShowProgressBar);
            Assert.AreEqual(accountName, config.AccountName);
            Assert.AreEqual(CultureInfo.CurrentCulture, config.Culture);
        }

        [TestMethod]
        public void Set()
        {
            string leagueJson = Encoding.UTF8.GetString(Properties.Resources.League); ;
            League league2 = JsonConvert.DeserializeObject<League>(leagueJson);
            Entry entry2 = league.Ladder.Entries[1];
            Font font2 = new Font("Courrier", 10);
            Color fontColor2 = Color.Beige;
            Color backgroundColor2 = Color.Blue;
            string accountName2 = "morinfa2";

            config.League = league2;
            config.Entry = entry2;
            config.Font = font2;
            config.FontColor = fontColor2;
            config.BackgroundColor = backgroundColor2;
            config.ShowDeadsAhead = false;
            config.ShowExperienceAhead = false;
            config.ShowExperienceBehind = false;
            config.ShowProgressBar = false;
            config.AccountName = accountName2;
            config.Culture = CultureInfo.InvariantCulture;

            Assert.AreEqual(league2, config.League);
            Assert.AreEqual(entry2, config.Entry);
            Assert.AreEqual(font2, config.Font);
            Assert.AreEqual(fontColor2, config.FontColor);
            Assert.AreEqual(backgroundColor2, config.BackgroundColor);
            Assert.IsFalse(config.ShowDeadsAhead);
            Assert.IsFalse(config.ShowExperienceAhead);
            Assert.IsFalse(config.ShowExperienceBehind);
            Assert.IsFalse(config.ShowProgressBar);
            Assert.AreEqual(accountName2, config.AccountName);
            Assert.AreEqual(CultureInfo.InvariantCulture, config.Culture);
        }
    }
}
