using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Diagnostics.Contracts;

namespace PoeApiClient.Installers
{
    public class ConvertersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires(container != null);

            container.Register(Classes.FromThisAssembly()
                .InNamespace("PoeApiClient.Converters")
                .LifestyleSingleton()
            );
        }
    }
}
