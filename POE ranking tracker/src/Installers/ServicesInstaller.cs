using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PoeRankingTracker.Services;

namespace PoeRankingTracker.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .InNamespace("PoeRankingTracker.Services")
                .WithService.DefaultInterfaces()
                .LifestyleSingleton()
            );
        }
    }
}
