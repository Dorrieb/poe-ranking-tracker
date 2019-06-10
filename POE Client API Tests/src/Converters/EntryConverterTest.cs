using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Models;
using System;

namespace PoeApiClientTests.Converters
{
    [TestClass]
    public class EntryConverterTest
    {
        private EntryConverter converter;

        [TestInitialize]
        public void TestSetup()
        {
            converter = new EntryConverter();
        }

        [TestMethod]
        public void CanConvert()
        {
            Assert.IsTrue(converter.CanConvert(typeof(IEntry)));
            Assert.IsFalse(converter.CanConvert(typeof(Entry)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "WriteJson not implemented.")]
        public void WriteJson()
        {
            converter.WriteJson(null, null, null);
        }
    }
}
