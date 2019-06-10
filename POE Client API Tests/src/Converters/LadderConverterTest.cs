using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Models;
using System;

namespace PoeApiClientTests.Converters
{
    [TestClass]
    public class LadderConverterTest
    {
        private LadderConverter converter;

        [TestInitialize]
        public void TestSetup()
        {
            converter = new LadderConverter();
        }

        [TestMethod]
        public void CanConvert()
        {
            Assert.IsTrue(converter.CanConvert(typeof(ILadder)));
            Assert.IsFalse(converter.CanConvert(typeof(Ladder)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "WriteJson not implemented.")]
        public void WriteJson()
        {
            converter.WriteJson(null, null, null);
        }
    }
}
