using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Models;
using System;

namespace PoeApiClientTests.Converters
{
    [TestClass]
    public class AccountConverterTest
    {
        private AccountConverter converter;

        [TestInitialize]
        public void TestSetup()
        {
            converter = new AccountConverter();
        }

        [TestMethod]
        public void CanConvert()
        {
            Assert.IsTrue(converter.CanConvert(typeof(IAccount)));
            Assert.IsFalse(converter.CanConvert(typeof(Account)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException), "WriteJson not implemented.")]
        public void WriteJson()
        {
            converter.WriteJson(null, null, null);
        }
    }
}
