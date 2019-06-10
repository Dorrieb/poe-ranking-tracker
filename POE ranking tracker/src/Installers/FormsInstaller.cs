using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Diagnostics.Contracts;

namespace PoeRankingTracker.Installers
{
    public class FormsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires(container != null);

            container.Register(Classes.FromThisAssembly()
                .InNamespace("PoeRankingTracker.Forms")
                .LifestyleSingleton()
            );
        }
    }
}
