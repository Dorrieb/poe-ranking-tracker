using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeApiClient.Services;
using PoeRankingTracker.Installers;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class ApiInstallerTest : BaseUnitTest, IDisposable
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(new ApiInstaller());
#pragma warning restore CA2000
        }

        [TestMethod]
        public void AllApiImplementsIService()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllServicesAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(
                c => c.Is<IHttpClientService>() ||
                     c.Is<ISemaphoreService>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyApiHaveServicesSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Name.EndsWith("Service", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyApiLiveInServicesNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<HttpClientService>(c => c.Namespace.Contains("Services"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllApiAreSingleton()
        {
            var nonSingleton = GetHandlers()
                .Where(c => c.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingleton.Length);
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[2];

            handlers[0] = GetHandlersFor(typeof(IHttpClientService), container)[0];
            handlers[1] = GetHandlersFor(typeof(ISemaphoreService), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[2];

            registered[0] = GetImplementationTypesFor(typeof(IHttpClientService), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(ISemaphoreService), container)[0];

            return registered;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                container?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
