using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Models;

namespace PoeApiClientTests.Models
{
    [TestClass]
    public class ChallengesTest
    {
        private IChallenges challenges;
        private readonly int total = 12;

        [TestInitialize]
        public void TestSetup()
        {
            challenges = new Challenges
            {
                Total = total,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(total, challenges.Total);
        }

        [TestMethod]
        public void Set()
        {
            int total2 = total + 2;
            challenges.Total = total2;
            Assert.AreEqual(total2, challenges.Total);
        }
    }
}
