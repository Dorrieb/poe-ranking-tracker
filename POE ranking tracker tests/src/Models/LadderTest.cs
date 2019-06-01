using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeRankingTracker.Models;
using System.Collections.Generic;
using System.Text;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class LadderTest
    {
        private Ladder ladder;

        [TestInitialize]
        public void TestSetup()
        {
            string ladderJson = Encoding.UTF8.GetString(Properties.Resources.Ladder); ;
            ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson);
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(20, ladder.Entries.Count);
        }

        [TestMethod]
        public void Set()
        {
            ladder.Entries = new List<Entry>();
            Assert.AreEqual(0, ladder.Entries.Count);
        }
    }
}