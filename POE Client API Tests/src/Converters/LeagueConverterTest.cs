using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Models;
using System;

namespace PoeApiClientTests.Converters
{
    [TestClass]
    public class LeagueConverterTest
    {
        private LeagueConverter converter;

        [TestInitialize]
        public void TestSetup()
        {
            converter = new LeagueConverter();
        }

        [TestMethod]
        public void CanConvert()
        {
            Assert.IsTrue(converter.CanConvert(typeof(ILeague)));
            Assert.IsFalse(converter.CanConvert(typeof(League)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "WriteJson not implemented.")]
        public void WriteJson()
        {
            converter.WriteJson(null, null, null);
        }
    }
}
