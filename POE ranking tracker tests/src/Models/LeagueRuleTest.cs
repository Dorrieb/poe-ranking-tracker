using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Models;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class LeagueRuleTest
    {
        private LeagueRule rule;
        private readonly string id = "Hardcore";
        private readonly string name = " Hardcore";
        private readonly string description = "A character killed in Hardcore is moved to its parent league.";

        [TestInitialize]
        public void TestSetup()
        {
            rule = new LeagueRule
            {
                Id = id,
                Name = name,
                Description = description,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(id, rule.Id);
            Assert.AreEqual(name, rule.Name);
            Assert.AreEqual(description, rule.Description);
        }

        [TestMethod]
        public void Set()
        {
            string id2 = "NoParties";
            string name2 = "Solo";
            string description2 = "You may not party in this league.";
            rule.Id = id2;
            rule.Name = name2;
            rule.Description = description2;
            Assert.AreEqual(id2, rule.Id);
            Assert.AreEqual(name2, rule.Name);
            Assert.AreEqual(description2, rule.Description);
        }
    }
}
