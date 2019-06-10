using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker;
using PoeRankingTracker.Forms;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class FormsInstallerTest : BaseUnitTest
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
            container = new WindsorContainer()
                .Install(new FormsInstaller());
        }

        [TestMethod]
        public void AllFormsExtendsForm()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllFormssAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(
                c => c.Is<ConfigurationForm>() ||
                     c.Is<TrackerForm>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyFormsHaveFormSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Name.EndsWith("Form", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyFormsLiveInFormsNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Namespace.Contains("Forms"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllFormsAreSingleton()
        {
            var nonSingleton = GetHandlers()
                .Where(f => f.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingleton.Length);
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[2];

            handlers[0] = GetHandlersFor(typeof(ConfigurationForm), container)[0];
            handlers[1] = GetHandlersFor(typeof(TrackerForm), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[2];

            registered[0] = GetImplementationTypesFor(typeof(ConfigurationForm), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(TrackerForm), container)[0];

            return registered;
        }
    }
}
