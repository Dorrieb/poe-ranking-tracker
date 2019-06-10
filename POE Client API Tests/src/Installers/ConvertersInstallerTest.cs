using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Installers;
using PoeApiClient.Services;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class ConvertersInstallerTest : BaseUnitTest
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(new ConvertersInstaller());
#pragma warning restore CA2000
        }

        [TestMethod]
        public void AllConvertersExtendsJsonConverter()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllConvertersAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(
                c => c.Is<AccountConverter>() ||
                     c.Is<ChallengesConverter>() ||
                     c.Is<CharacterConverter>() ||
                     c.Is<EntryConverter>() ||
                     c.Is<LadderConverter>() ||
                     c.Is<LeagueConverter>() ||
                     c.Is<LeagueRuleConverter>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyConvertersHaveConverterSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Name.EndsWith("Converter", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyConvertersLiveInConvertersNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Namespace.Contains("Converters"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllConvertersAreSingleton()
        {
            var nonSingleton = GetHandlers()
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingleton.Length);
        }

        [TestCleanup]
        public void DisposeContainer()
        {
            container.Dispose();
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[7];

            handlers[0] = GetHandlersFor(typeof(AccountConverter), container)[0];
            handlers[1] = GetHandlersFor(typeof(ChallengesConverter), container)[0];
            handlers[2] = GetHandlersFor(typeof(CharacterConverter), container)[0];
            handlers[3] = GetHandlersFor(typeof(EntryConverter), container)[0];
            handlers[4] = GetHandlersFor(typeof(LadderConverter), container)[0];
            handlers[5] = GetHandlersFor(typeof(LeagueConverter), container)[0];
            handlers[6] = GetHandlersFor(typeof(LeagueRuleConverter), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[7];

            registered[0] = GetImplementationTypesFor(typeof(AccountConverter), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(ChallengesConverter), container)[0];
            registered[2] = GetImplementationTypesFor(typeof(CharacterConverter), container)[0];
            registered[3] = GetImplementationTypesFor(typeof(EntryConverter), container)[0];
            registered[4] = GetImplementationTypesFor(typeof(LadderConverter), container)[0];
            registered[5] = GetImplementationTypesFor(typeof(LeagueConverter), container)[0];
            registered[6] = GetImplementationTypesFor(typeof(LeagueRuleConverter), container)[0];

            return registered;
        }
    }
}
