using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Events;
using PoeApiClient.Models;
using System;
using System.Collections.Generic;

namespace PoeApiClientTests.Events
{
    [TestClass]
    public class RulesEventArgsTest
    {
        private RulesEventArgs eventArgs;

        [TestInitialize]
        public void TestSetup()
        {
            eventArgs = new RulesEventArgs();
        }

        [TestMethod]
        public void IsInstanceOfEventArgs()
        {
            Assert.IsTrue(eventArgs is EventArgs);
        }

        [TestMethod]
        public void GetSetRules()
        {
            Assert.IsNull(eventArgs.Rules);
            var rules1 = new RuleApi(5, 5, 10);
            var rules2 = new RuleApi(10, 15, 30);
            var rules3 = new RuleApi(15, 30, 60);
            var rules = new List<IRuleApi>()
            {
                rules1,
                rules2,
                rules3,
            };
            eventArgs.Rules = rules;
            Assert.AreEqual(rules, eventArgs.Rules);
        }

        [TestMethod]
        public void GetSetRulesStates()
        {
            Assert.IsNull(eventArgs.Rules);
            var rules1 = new RuleApi(5, 5, 10);
            var rules2 = new RuleApi(10, 15, 30);
            var rules3 = new RuleApi(15, 30, 60);
            var rules = new List<IRuleApi>()
            {
                rules1,
                rules2,
                rules3,
            };
            eventArgs.RulesState = rules;
            Assert.AreEqual(rules, eventArgs.RulesState);
        }
    }
}
