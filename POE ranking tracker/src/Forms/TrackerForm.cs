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
using System.Runtime.InteropServices;
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
        private int hotkeyId = 0;
        private bool formInteractionsDisabled = false;

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

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
            formInteractionsDisabled = configuration.InteractionsDisabled;
            templateContent = htmlService.GetTemplate(configuration.Template);
            initialLoading = true;
            webBrowser.Navigate(new Uri("about:blank"));
            SetHotkey();
            SetCursor();
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

        private void SetHotkey()
        {
            if (!NativeMethods.RegisterHotKey(Handle, hotkeyId, (int)KeyModifier.Control | (int)KeyModifier.Shift, Keys.O.GetHashCode()))
            {
                logger.Error("Failed to register hotkey");
            }
        }

        private void SetCursor()
        {
            if (formInteractionsDisabled)
            {
                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.SizeAll;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RetrieveData();
        }

        private void OnTimedSecondsPlayedEvent(object source, ElapsedEventArgs e)
        {
            secondsPlayed += Properties.Settings.Default.TimerInterval / 1000;
        }

        private void TrackerForm_MouseMove(object sender, HtmlElementEventArgs e)
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
            Properties.Settings.Default.TrackerMoved = true;
            Properties.Settings.Default.TrackerLocation = Location;
            Properties.Settings.Default.InteractionsDisabled = formInteractionsDisabled;
            NativeMethods.UnregisterHotKey(Handle, hotkeyId);
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                formInteractionsDisabled = !formInteractionsDisabled;
                if (formInteractionsDisabled)
                {
                    int wl = NativeMethods.GetWindowLong(this.Handle, NativeMethods.GWL.ExStyle);
                    wl = wl | (int)NativeMethods.WS_EX.Layered | (int)NativeMethods.WS_EX.Transparent;
                    _ = NativeMethods.SetWindowLong(this.Handle, NativeMethods.GWL.ExStyle, wl);
                    //NativeMethods.SetLayeredWindowAttributes(this.Handle, 0, 215, NativeMethods.LWA.Alpha);
                }
                else
                {
                    int wl = NativeMethods.GetWindowLong(this.Handle, NativeMethods.GWL.ExStyle);
                    wl = wl & (int)NativeMethods.WS_EX.Layered & (int)NativeMethods.WS_EX.Transparent;
                    _ = NativeMethods.SetWindowLong(this.Handle, NativeMethods.GWL.ExStyle, wl);
                    //NativeMethods.SetLayeredWindowAttributes(this.Handle, 0, 255, NativeMethods.LWA.Alpha);
                }
                SetCursor();
            }
        }
    }

    internal static class NativeMethods
    {
        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);
    }
}
