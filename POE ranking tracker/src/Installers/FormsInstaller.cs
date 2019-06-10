using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace PoeRankingTracker.Installers
{
    public class FormsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .InNamespace("PoeRankingTracker.Forms")
                .LifestyleSingleton()
            );
        }
    }
}
