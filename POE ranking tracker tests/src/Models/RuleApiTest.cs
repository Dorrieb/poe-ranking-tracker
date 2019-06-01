using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Models;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class RuleApiTest
    {
        private RuleApi rule;
        private readonly int requestLimit = 5;
        private readonly int interval = 10;
        private readonly int timeout = 30;

        [TestInitialize]
        public void TestSetup()
        {
            rule = new RuleApi(requestLimit, interval, timeout);
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(requestLimit, rule.RequestLimit);
            Assert.AreEqual(interval, rule.Interval);
            Assert.AreEqual(timeout, rule.Timeout);
        }

        [TestMethod]
        public void Set()
        {
            int requestLimit2 = 10;
            int interval2 = 20;
            int timeout2 = 60;
            rule.RequestLimit = requestLimit2;
            rule.Interval = interval2;
            rule.Timeout = timeout2;
            Assert.AreEqual(requestLimit2, rule.RequestLimit);
            Assert.AreEqual(interval2, rule.Interval);
            Assert.AreEqual(timeout2, rule.Timeout);
        }

        [TestMethod]
        public void StringCheck()
        {
            Assert.AreEqual($"{requestLimit}:{interval}:{timeout}", rule.ToString());
        }
    }
}
