using Castle.Windsor;
using Castle.Windsor.Installer;
using PoeRankingTracker.Forms;
using PoeRankingTracker.Models;
using System;
using System.Windows.Forms;

namespace PoeRankingTracker
{
    public class RankingTrackerContext : ApplicationContext
    {
        private readonly ConfigurationForm configurationForm;
        private readonly TrackerForm trackerForm;
        private IWindsorContainer container;

        private static RankingTrackerContext currContext;

        public RankingTrackerContext()
        {
            if (currContext == null)
            {
                currContext = this;
            }
            CreateWindsorContainer();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            configurationForm = container.Resolve<ConfigurationForm>();
            trackerForm = container.Resolve<TrackerForm>();

            ShowConfigurationForm();
        }

        public static RankingTrackerContext CurrentContext
        {
            get
            {
                return currContext;
            }
        }

        public void ShowConfigurationForm()
        {
            configurationForm.Show();
            trackerForm.Hide();
        }

        public void ShowTrackerForm(TrackerConfiguration configuration)
        {
            trackerForm.SetConfiguration(configuration);
            configurationForm.Hide();
            trackerForm.Show();
        }

        private void CreateWindsorContainer()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(
                    FromAssembly.This()
                 );
#pragma warning restore CA2000
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                configurationForm.Dispose();
                trackerForm.Dispose();
                container.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
