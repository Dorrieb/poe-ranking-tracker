using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Diagnostics.Contracts;

namespace PoeRankingTracker.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires(container != null);

            container.Register(Classes.FromThisAssembly()
                .InNamespace("PoeRankingTracker.Services")
                .WithService.DefaultInterfaces()
                .LifestyleSingleton()
            );
        }
    }
}
