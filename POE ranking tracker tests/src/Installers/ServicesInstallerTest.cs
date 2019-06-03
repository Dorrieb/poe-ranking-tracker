using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
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
            var handlers = GetServicesHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllServicesAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly(
                c => c.Is<ICharacterService>() ||
                     c.Is<IFormService>() ||
                     c.Is<IHttpClientService>() ||
                     c.Is<ILadderService>() ||
                     c.Is<ISemaphoreService>()
            );
            var registered = GetImplementationServicesTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesHaveServicesSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Service"));
            var registered = GetImplementationServicesTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesLiveInControllersNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Services"));
            var registered = GetImplementationServicesTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllServicesAreSingleton()
        {
            var nonSingletonServices = GetServicesHandlers()
                .Where(service => service.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingletonServices.Length);
        }

        private IHandler[] GetServicesHandlers()
        {
            var handlers = new IHandler[5];

            handlers[0] = GetHandlersFor(typeof(ICharacterService), container)[0];
            handlers[1] = GetHandlersFor(typeof(IFormService), container)[0];
            handlers[2] = GetHandlersFor(typeof(IHttpClientService), container)[0];
            handlers[3] = GetHandlersFor(typeof(ILadderService), container)[0];
            handlers[4] = GetHandlersFor(typeof(ISemaphoreService), container)[0];

            return handlers;
        }

        private Type[] GetImplementationServicesTypes()
        {
            var registered = new Type[5];

            registered[0] = GetImplementationTypesFor(typeof(ICharacterService), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(IFormService), container)[0];
            registered[2] = GetImplementationTypesFor(typeof(IHttpClientService), container)[0];
            registered[3] = GetImplementationTypesFor(typeof(ILadderService), container)[0];
            registered[4] = GetImplementationTypesFor(typeof(ISemaphoreService), container)[0];

            return registered;
        }
    }
}
