using PoeRankingTracker.Models;
using System;
using System.Windows.Forms;

namespace PoeRankingTracker
{
    class RankingTrackerContext : ApplicationContext
    {
        public static RankingTrackerContext currentContext;

        private readonly ConfigurationForm configurationForm;
        private readonly TrackerForm trackerForm;

        public RankingTrackerContext()
        {
            currentContext = this;

            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            configurationForm = new ConfigurationForm();
            trackerForm = new TrackerForm();

            ShowConfigurationForm();
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
            }
            base.Dispose(disposing);
        }
    }
}
