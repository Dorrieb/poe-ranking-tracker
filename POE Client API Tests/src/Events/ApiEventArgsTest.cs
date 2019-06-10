using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Events;
using System;

namespace PoeApiClientTests.Events
{
    [TestClass]
    public class ApiEventArgsTest
    {
        private ApiEventArgs eventArgs;

        [TestInitialize]
        public void TestSetup()
        {
            eventArgs = new ApiEventArgs();
        }

        [TestMethod]
        public void IsInstanceOfEventArgs()
        {
            Assert.IsTrue(eventArgs is EventArgs);
        }

        [TestMethod]
        public void GetSetValue()
        {
            Assert.AreEqual(0, eventArgs.Value);
            eventArgs.Value = 5;
            Assert.AreEqual(5, eventArgs.Value);
        }
    }
}
