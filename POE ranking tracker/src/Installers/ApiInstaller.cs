using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace PoeRankingTracker.Installers
{
    public class ApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed("POE API client")
                .InNamespace("PoeApiClient.Services")
                .WithService.DefaultInterfaces()
                .LifestyleSingleton()
            );
        }
    }
}
