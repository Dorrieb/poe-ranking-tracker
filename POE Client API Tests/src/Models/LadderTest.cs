using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeApiClient.Models;
using POEToolsTestsBase;
using System.Collections.Generic;
using System.Text;

namespace PoeApiClientTests.Models
{
    [TestClass]
    public class LadderTest : BaseUnitTest
    {
        private ILadder ladder;

        [TestInitialize]
        public void TestSetup()
        {
            string ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Ladder); ;
            ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson, GetJsonSettings());
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(20, ladder.Entries.Count);
        }

        [TestMethod]
        public void Set()
        {
            ladder.Entries = new List<IEntry>();
            Assert.AreEqual(0, ladder.Entries.Count);
        }
    }
}