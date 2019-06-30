using PoeApiClient.Events;
using PoeApiClient.Models;
using PoeApiClient.Services;
using PoeRankingTracker.Exceptions;
using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using PoeRankingTracker.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace PoeRankingTracker.Forms
{
    public partial class TrackerForm : Form
    {
        private Point lastPoint;
        private System.Timers.Timer timer = new System.Timers.Timer(Properties.Settings.Default.TimerInterval);
        private System.Timers.Timer timerSecondsPlayed = new System.Timers.Timer(Properties.Settings.Default.TimerInterval);
        private TrackerConfiguration configuration;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IHttpClientService httpClientService;
        private ICharacterService characterService;
        private IHtmlService htmlService;
        private string templateContent;
        private bool initialLoading = false;
        private int currentProgress = 0;
        private int maxProgress = 0;
        private long initialExperience;
        private long secondsPlayed = 0;

        public TrackerForm(IHttpClientService httpClientService, ICharacterService characterService, IHtmlService htmlService)
        {
            this.httpClientService = httpClientService;
            this.characterService = characterService;
            this.htmlService = htmlService;
            InitializeComponent();
            InitializePosition();
            InitializeProgressEvents();
            SetTimer();
        }

        private void InitializeTranslations()
        {
            Text = Strings.Configuration;
        }

        private void InitializePosition()
        {
            if (Properties.Settings.Default.TrackerMoved)
            {
                StartPosition = FormStartPosition.Manual;
                Location = Properties.Settings.Default.TrackerLocation;
            }
        }

        private void InitializeProgressEvents()
        {
            httpClientService.GetEntriesStarted += ProgressStarted;
            httpClientService.GetEntriesIncremented += ProgressIncremented;
        }

        private void ProgressStarted(object sender, ApiEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                currentProgress = 0;
                maxProgress = args.Value;

                var progress = webBrowser.Document.GetElementsByTagName("progress");
                if (progress != null && progress.Count > 0)
                {
                    progress[0].SetAttribute("value", "0");
                    progress[0].SetAttribute("max", $"{maxProgress}");
                }
            }));
        }

        private void ProgressIncremented(object sender, ApiEventArgs args)
        {
            currentProgress += args.Value;

            Invoke(new MethodInvoker(delegate
            {
                var progress = webBrowser.Document.GetElementsByTagName("progress");
                if (progress != null && progress.Count > 0)
                {
                    progress[0].SetAttribute("value", $"{currentProgress}");
                }
            }));
        }

        public void SetConfiguration(TrackerConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            this.configuration = configuration;
            InitializeTranslations();
            secondsPlayed = 0;
            initialExperience = configuration.Entry.Character.Experience;
            templateContent = htmlService.GetTemplate(configuration.Template);
            initialLoading = true;
            webBrowser.Navigate(new Uri("about:blank"));
        }

        private async void RetrieveData()
        {
            try
            {
                logger.Debug("RetrieveData");
                timer?.Stop();

                logger.Debug($"Get ladder {configuration.League.Id} - {configuration.AccountName}");
                List<IEntry> entries = await httpClientService.GetEntries(configuration.League.Id, configuration.AccountName, configuration.Entry.Character.Name).ConfigureAwait(true);
                var entryRefreshed = characterService.GetEntry(entries, configuration.Entry.Character.Name);
                if (entryRefreshed != null)
                {
                    configuration.Entry = entryRefreshed;
                    RefreshDisplay(entries, entryRefreshed);
                }

                timer?.Start();
            }
            catch (OperationCanceledException e)
            {
                logger.Debug(e, "Operation cancelled");
            }
            catch (CharacterNotFoundException e)
            {
                logger.Debug(e, "Character not found");
            }
            catch (ObjectDisposedException e)
            {
                logger.Debug(e, "Object disposed after tasks cancel");
            }
            catch (InvalidOperationException e)
            {
                logger.Debug(e, "Object disposed with invalid operation");
            }
        }

        private void RefreshDisplay(List<IEntry> entries, IEntry entry)
        {
            Invoke(new MethodInvoker(delegate
            {
                var htmlConfiguration = htmlService.BuildHtmlConfiguration(entries, configuration.Entry);
                if (secondsPlayed > 0)
                {
                    htmlConfiguration.ExperiencePerHour = (entry.Character.Experience - initialExperience) * 3600 / secondsPlayed;
                }
                else
                {
                    htmlConfiguration.ExperiencePerHour = 0;
                }
                var content = htmlService.UpdateContent(templateContent, htmlConfiguration, true);
                webBrowser.Document.Write(content);
                webBrowser.Refresh();
            }));
        }

        private void SetTimer()
        {
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timerSecondsPlayed.Elapsed += OnTimedSecondsPlayedEvent;
            timerSecondsPlayed.AutoReset = true;
            timerSecondsPlayed.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RetrieveData();
        }

        private void OnTimedSecondsPlayedEvent(object source, ElapsedEventArgs e)
        {
            secondsPlayed += Properties.Settings.Default.TimerInterval / 1000;
        }

        private void TrackerForm_MouseMove(object sender, HtmlElementEventArgs  e)
        {
            if (e.MouseButtonsPressed == MouseButtons.Left)
            {
                Left += e.MousePosition.X - lastPoint.X;
                Top += e.MousePosition.Y - lastPoint.Y;
            }
        }

        private void TrackerForm_MouseDown(object sender, HtmlElementEventArgs e)
        {
            lastPoint = new Point(e.MousePosition.X, e.MousePosition.Y);
        }

        private void TrackerForm_MouseDoubleClick(object sender, System.EventArgs e)
        {
            timer.Stop();
            timerSecondsPlayed.Stop();
            RankingTrackerContext.CurrentContext.ShowConfigurationForm();
        }

        private void TrackerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timerSecondsPlayed.Stop();
            Properties.Settings.Default.TrackerLocation = Location;
        }

        private void TrackerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void TrackerForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                timer.Stop();
                timerSecondsPlayed.Stop();
                httpClientService.CancelPendingRequests();
                initialLoading = true;
                webBrowser.Document.MouseDown -= new HtmlElementEventHandler(TrackerForm_MouseDown);
                webBrowser.Document.MouseMove -= new HtmlElementEventHandler(TrackerForm_MouseMove);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                webBrowser.Dispose();
                timer.Dispose();
                timerSecondsPlayed.Dispose();
            }

            base.Dispose(disposing);
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            logger.Debug("WebBrowser_DocumentCompleted");
            if (initialLoading)
            {
                initialLoading = false;
                var htmlConfiguration = htmlService.BuildHtmlConfiguration(null, configuration.Entry);
                var content = htmlService.UpdateContent(templateContent, htmlConfiguration, false);
                webBrowser.Document.Write(content);
                webBrowser.Document.MouseDown += new HtmlElementEventHandler(TrackerForm_MouseDown);
                webBrowser.Document.MouseMove += new HtmlElementEventHandler(TrackerForm_MouseMove);
                webBrowser.Refresh();
                RetrieveData();
            }

            var container = webBrowser.Document.GetElementById("container");
            if (container != null)
            {
                container.DoubleClick += new HtmlElementEventHandler(TrackerForm_MouseDoubleClick);
                webBrowser.Size = container.OffsetRectangle.Size;
                Size = container.OffsetRectangle.Size;
            }
        }

        private void WebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                Close();
            }
        }
    }
}
