using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeApiClient.Converters;
using PoeApiClient.Formatters;
using PoeApiClient.Installers;
using PoeApiClient.Models;
using POEToolsTestsBase;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Text;

namespace PoeApiClientTests.Formatters
{
    [TestClass]
    public class LadderFormatterTest : BaseUnitTest, IDisposable
    {
        private IWindsorContainer container;
        private LadderFormatter formatter;

        [TestInitialize]
        public void TestSetup()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(new ConvertersInstaller());
#pragma warning restore CA2000
            formatter = new LadderFormatter();
            
            List<JsonConverter> converters = new List<JsonConverter>
            {
                container.Resolve<LeagueConverter>(),
                container.Resolve<LeagueRuleConverter>(),
                container.Resolve<LadderConverter>(),
                container.Resolve<EntryConverter>(),
                container.Resolve<CharacterConverter>(),
                container.Resolve<AccountConverter>(),
                container.Resolve<ChallengesConverter>(),
            };

            foreach (var converter in converters)
            {
                formatter.SerializerSettings.Converters.Add(converter);
            }
        }

        [TestMethod]
        public void IsInstanceOfJsonMediaTypeFormatter()
        {
            Assert.IsTrue(formatter is JsonMediaTypeFormatter);
        }

        [TestMethod]
        public void ReadFromStream()
        {
            ILadder ladder;
            string ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Ladder);
            using (var stream = GenerateStreamFromString(ladderJson))
            {
                ladder = formatter.ReadFromStream(typeof(Ladder), stream, Encoding.UTF8, new Logger()) as ILadder;
                Assert.AreEqual(17, ladder.Entries.Count);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            container.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                container.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    class Logger : IFormatterLogger
    {
        public void LogError(string errorPath, string errorMessage)
        {
            Assert.Fail(errorPath, errorMessage);
        }

        public void LogError(string errorPath, Exception exception)
        {
            Assert.Fail(errorPath, new object[1] { exception });
        }
    }
}
