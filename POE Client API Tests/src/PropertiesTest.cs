using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PoeApiClientTests
{
    [TestClass]
    public class PropertiesTest
    {
        [TestMethod]
        public void TestBaseUri()
        {
            Assert.AreEqual("https://api.pathofexile.com", PoeApiClient.Properties.Settings.Default.BaseUri);
        }

        [TestMethod]
        public void TestLaddersAllPath()
        {
            Assert.AreEqual("ladders/{0}?offset={1}&limit={2}", PoeApiClient.Properties.Settings.Default.LaddersAllPath);
        }

        [TestMethod]
        public void TestLaddersPath()
        {
            Assert.AreEqual("ladders/{0}?accountName={1}", PoeApiClient.Properties.Settings.Default.LaddersPath);
        }

        [TestMethod]
        public void TestLeaguesPath()
        {
            Assert.AreEqual("leagues", PoeApiClient.Properties.Settings.Default.LeaguesPath);
        }
    }
}
