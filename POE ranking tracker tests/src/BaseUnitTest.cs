using Castle.MicroKernel;
using Castle.Windsor;
using PoeRankingTracker;
using System;
using System.Linq;

namespace PoeRankingTrackerTests
{
    public abstract class BaseUnitTest
    {
        protected static IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        protected static IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        protected static Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        protected static Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof(RankingTrackerContext).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }
    }
}
