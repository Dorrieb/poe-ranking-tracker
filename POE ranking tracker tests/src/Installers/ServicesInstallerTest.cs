using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class ServicesInstallerTest : BaseUnitTest
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
            container = new WindsorContainer()
                .Install(new ServicesInstaller());
        }

        [TestMethod]
        public void AllServicesImplementsIService()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllServicesAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(
                c => c.Is<ICharacterService>() ||
                     c.Is<IFormService>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesHaveServicesSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Name.EndsWith("Service", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesLiveInServicesNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Namespace.Contains("Services"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllServicesAreSingleton()
        {
            var nonSingletonServices = GetHandlers()
                .Where(service => service.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingletonServices.Length);
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[2];

            handlers[0] = GetHandlersFor(typeof(ICharacterService), container)[0];
            handlers[1] = GetHandlersFor(typeof(IFormService), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[2];

            registered[0] = GetImplementationTypesFor(typeof(ICharacterService), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(IFormService), container)[0];

            return registered;
        }
    }
}
