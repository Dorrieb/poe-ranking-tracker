using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Converters;
using PoeApiClient.Formatters;
using PoeApiClient.Installers;
using PoeApiClient.Services;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class FormattersInstallerTest : BaseUnitTest
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(new FormattersInstaller());
#pragma warning restore CA2000
        }

        [TestMethod]
        public void AllFormattersExtendsJsonMediaTypeFormatter()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllFormattersAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(
                c => c.Is<LadderFormatter>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyFormattersHaveFormatterSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Name.EndsWith("Formatter", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyFormattersLiveInFormattersNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Namespace.Contains("Formatters"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllFormattersAreSingleton()
        {
            var nonSingleton = GetHandlers()
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingleton.Length);
        }

        [TestCleanup]
        public void DisposeWindsor()
        {
            container.Dispose();
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[1];

            handlers[0] = GetHandlersFor(typeof(LadderFormatter), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[1];

            registered[0] = GetImplementationTypesFor(typeof(LadderFormatter), container)[0];

            return registered;
        }
    }
}
