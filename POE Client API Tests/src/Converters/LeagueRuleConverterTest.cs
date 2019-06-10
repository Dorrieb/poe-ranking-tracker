using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Models;
using System;

namespace PoeApiClientTests.Converters
{
    [TestClass]
    public class LeagueRuleConverterTest
    {
        private LeagueRuleConverter converter;

        [TestInitialize]
        public void TestSetup()
        {
            converter = new LeagueRuleConverter();
        }

        [TestMethod]
        public void CanConvert()
        {
            Assert.IsTrue(converter.CanConvert(typeof(ILeagueRule)));
            Assert.IsFalse(converter.CanConvert(typeof(LeagueRule)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "WriteJson not implemented.")]
        public void WriteJson()
        {
            converter.WriteJson(null, null, null);
        }
    }
}
