using PoeApiClient.Models;
using PoeApiClient.Services;
using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using PoeRankingTracker.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoeRankingTracker.Forms
{
    public partial class ConfigurationForm : Form
    {
        private ILeague selectedLeague = null;
        private string accountName = Properties.Settings.Default.AccountName;
        private IEntry selectedEntry = null;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IHttpClientService httpClientService;
        private ISemaphoreService semaphoreService;
        private IFormService formService;
        private IFormatterService formatterService;
        private IHtmlService htmlService;

        public ConfigurationForm(IFormService formService, IHttpClientService httpClientService, ISemaphoreService semaphoreService, IFormatterService formatterService, IHtmlService htmlService)
        {
            this.formService = formService;
            this.httpClientService = httpClientService;
            this.semaphoreService = semaphoreService;
            this.formatterService = formatterService;
            this.htmlService = htmlService;
            InitializeComponent();
            InitializeTranslations();
            InitializePosition();
            InitializeFontAndOptions();
            InitializeLanguagesCombo();
            InitializeLeagueRaceCombo();
            InitializeTemplates();
        }

        private void InitializeTranslations()
        {
            if (Properties.Settings.Default.Language.Length > 0)
            {
                formService.SetCulture(Properties.Settings.Default.Language);
            }

            Text = Strings.Configuration;
            leagueRaceLabel.Text = Strings.League;
            accountNameLabel.Text = Strings.AccountName;
            characterNameLabel.Text = Strings.CharacterName;
            sessionIdLabel.Text = Strings.SessionId;
            templatesComboBox.Text = Strings.ChooseTemplate;
            sampleLabel.Text = Strings.PreviewLabel;
            okButton.Text = Strings.OK;
            languageLabel.Text = Strings.Language;
            refreshTemplatesButtonToolTip.SetToolTip(refreshTemplatesButton, Strings.RefreshTemplates);
        }

        private void InitializePosition()
        {
            if (Properties.Settings.Default.ConfigurationMoved)
            {
                StartPosition = FormStartPosition.Manual;
                Location = Properties.Settings.Default.ConfigurationLocation;
            }
        }

        private void InitializeLanguagesCombo()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>
            {
                new ComboBoxItem
                {
                    Text = Strings.LanguageEnglish,
                    Value = "en",
                },
                new ComboBoxItem
                {
                    Text = Strings.LanguageFrench,
                    Value = "fr",
                }
            };

            languageComboBox.Items.Clear();
            foreach (var item in items)
            {
                languageComboBox.Items.Add(item);
                if ((string)item.Value == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    languageComboBox.SelectedIndexChanged -= LanguageComboBox_SelectedIndexChanged;
                    languageComboBox.SelectedItem = item;
                    languageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
                }
            }
        }

        private async void InitializeFontAndOptions()
        {
            infoKeyProvider.Icon = Icon.FromHandle(Properties.Resources.Key_16x.GetHicon());
            warningKeyProvider.Icon = Icon.FromHandle(Properties.Resources.KeyWarning_16x.GetHicon());
            errorKeyProvider.Icon = Icon.FromHandle(Properties.Resources.KeyError_16x.GetHicon());
            infoProvider.Icon = formService.ResizeIcon(SystemIcons.Information, SystemInformation.SmallIconSize);
            okProvider.Icon = Icon.FromHandle(Properties.Resources.StatusOK_16x.GetHicon());
            await httpClientService.SetSessionId(Properties.Settings.Default.SessionId).ConfigureAwait(true);
            CheckSessionId();
        }

        private async void InitializeLeagueRaceCombo()
        {
            try
            {
                logger.Debug("Get leagues");
                List<ILeague> leagues = await httpClientService.GetLeaguesAsync().ConfigureAwait(true);
                CheckSessionId();

                errorProvider.SetError(leagueRaceCombo, "");
                leagueRaceCombo.Items.Clear();
                foreach (League league in leagues)
                {
                    var item = new ComboBoxItem
                    {
                        Text = league.Id,
                        Value = league,
                    };
                    leagueRaceCombo.Items.Add(item);
                }

                SetSelectedLeague();
            }
            catch (HttpRequestException e)
            {
                logger.Error(e, "Failed to retrieve leagues");
                errorProvider.SetError(leagueRaceCombo, Strings.FailedToRetrieveLeagues);
            }
        }

        private void InitializeTemplates()
        {
            templatesComboBox.Items.Clear();

            foreach (string file in Directory.EnumerateFiles(Properties.Settings.Default.TemplatesPath, "*.html"))
            {
                var item = new ComboBoxItem()
                {
                    Text = file.Substring(Properties.Settings.Default.TemplatesPath.Length + 1),
                    Value = file,
                };
                templatesComboBox.Items.Add(item);
                if (Properties.Settings.Default.Template == file)
                {
                    templatesComboBox.SelectedItem = item;
                }
            }
        }

        private async void SetSelectedLeague()
        {
            selectedLeague = null;
            foreach (var item in leagueRaceCombo.Items)
            {
                var league = (item as ComboBoxItem).Value as League;
                if (league.Id == Properties.Settings.Default.LeagueId)
                {
                    leagueRaceCombo.SelectedIndexChanged -= LeagueRaceCombo_SelectedIndexChanged;
                    leagueRaceCombo.SelectedItem = item;
                    leagueRaceCombo.SelectedIndexChanged += LeagueRaceCombo_SelectedIndexChanged;
                    selectedLeague = league;
                    leagueUrlLink.Enabled = true;
                }
            }

            if (selectedLeague == null && Properties.Settings.Default.LeagueId.Length > 0)
            {
                logger.Debug($"Get league {Properties.Settings.Default.LeagueId}");
                var league = await httpClientService.GetLeagueAsync(Properties.Settings.Default.LeagueId).ConfigureAwait(true);
                if (league != null)
                {
                    var item = new ComboBoxItem
                    {
                        Text = league.Id,
                        Value = league,
                    };
                    leagueRaceCombo.Items.Add(item);
                    leagueRaceCombo.SelectedIndexChanged -= LeagueRaceCombo_SelectedIndexChanged;
                    leagueRaceCombo.SelectedItem = item;
                    leagueRaceCombo.SelectedIndexChanged += LeagueRaceCombo_SelectedIndexChanged;
                    selectedLeague = league;
                    leagueUrlLink.Enabled = false;
                }
            }

            DisplayLeagueInfo();
            InitializeCharactersList();
        }

        private void LeagueRaceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            selectedLeague = item.Value as League;

            Properties.Settings.Default.LeagueId = selectedLeague.Id;

            leagueUrlLink.Enabled = true;

            InitializeCharactersList();
            DisplayLeagueInfo();
        }

        private async void LeagueRaceCombo_TextUpdate(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int startLength = cb.Text.Length;

            await Task.Delay(Properties.Settings.Default.TextChangedDelay).ConfigureAwait(true);
            if (startLength == cb.Text.Length)
            {
                var leagueId = cb.Text;
                logger.Debug($"Get league {leagueId}");
                var league = await httpClientService.GetLeagueAsync(leagueId).ConfigureAwait(true);
                if (league == null)
                {
                    errorProvider.SetError(leagueRaceCombo, Strings.FailedToRetrieveLeague);
                }
                else
                {
                    selectedLeague = league;

                    leagueUrlLink.Enabled = false;

                    errorProvider.SetError(leagueRaceCombo, "");

                    Properties.Settings.Default.LeagueId = selectedLeague.Id;
                }

                InitializeCharactersList();
            }
        }

        private void CheckIfOkButtonCanBeEnabled()
        {
            okButton.Enabled = selectedEntry != null && Properties.Settings.Default.CharacterName.Length > 0 && Properties.Settings.Default.Template.Length > 0;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Hide();

            semaphoreService.CreateSemaphore();

            var configuration = new TrackerConfiguration
            {
                League = selectedLeague,
                Entry = selectedEntry,
                AccountName = accountNameTextBox.Text,
                Culture = CultureInfo.CurrentCulture,
                Template = Properties.Settings.Default.Template,
            };

            RankingTrackerContext.CurrentContext.ShowTrackerForm(configuration);
        }

        private async void AccountNameTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int startLength = tb.Text.Length;

            await Task.Delay(Properties.Settings.Default.TextChangedDelay).ConfigureAwait(true);
            if (startLength == tb.Text.Length)
            {
                accountName = tb.Text;

                Properties.Settings.Default.AccountName = accountName;

                InitializeCharactersList();
            }
        }

        private async void InitializeCharactersList()
        {
            charactersComboBox.Items.Clear();
            if (selectedLeague != null && accountName != null && accountName.Length > 0)
            {
                logger.Debug($"Get ladder {selectedLeague.Id} - {accountName}");
                ILadder ladder = await httpClientService.GetLadderAsync(selectedLeague.Id, accountName).ConfigureAwait(true);
                try
                {
                    selectedEntry = null;
                    CheckIfOkButtonCanBeEnabled();
                    if (ladder == null || (ladder.Entries != null && ladder.Entries.Count == 0))
                    {
                        SetAccountNameError();
                    }
                    else
                    {
                        foreach (Entry entry in ladder.Entries)
                        {
                            if (!entry.Dead && !entry.Retired)
                            {
                                var item = new ComboBoxItem
                                {
                                    Text = $"{entry.Character.Name} ({entry.Character.Level})",
                                    Value = entry,
                                };
                                charactersComboBox.Items.Add(item);
                                if (entry.Character.Name == Properties.Settings.Default.CharacterName)
                                {
                                    charactersComboBox.SelectedItem = item;
                                }
                            }
                        }
                        SetAccountNameError();
                    }
                }
                catch (HttpRequestException e)
                {
                    logger.Error(e, "Unable to retrieve characters");
                    errorProvider.SetError(accountNameTextBox, Strings.UnableToRetrieveCharacters);
                }
            }
        }

        private void SetAccountNameError()
        {
            if (charactersComboBox.Items.Count > 0)
            {
                okProvider.SetError(accountNameTextBox, string.Format(CultureInfo.CurrentCulture, Strings.CharactersFoundForAccount, accountName));
                errorProvider.SetError(accountNameTextBox, "");
            }
            else
            {
                okProvider.SetError(accountNameTextBox, "");
                errorProvider.SetError(accountNameTextBox, string.Format(CultureInfo.CurrentCulture, Strings.NoCharactersFoundForAccount, accountName));
            }
        }

        private void CharactersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            leagueUrlLink.LinkVisited = false;

            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            selectedEntry = item.Value as Entry;

            Properties.Settings.Default.CharacterName = selectedEntry.Character.Name;

            CheckIfOkButtonCanBeEnabled();
        }

        private void TemplatesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            var item = combo.SelectedItem as ComboBoxItem;
            
            Properties.Settings.Default.Template = item.Value as string;

            LoadTemplate();
        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.ConfigurationLocation = Location;
        }

        private void LeagueUrlLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                logger.Error(ex, "Unable to open league URL");
                MessageBox.Show(Strings.UnableToOpenUrl);
            }
        }

        private void VisitLink()
        {
            leagueUrlLink.LinkVisited = true;
            System.Diagnostics.Process.Start(selectedLeague.Url.AbsoluteUri);
        }

        private void ConfigurationForm_Move(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConfigurationMoved = true;
        }

        private async void SessionIdTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int startLength = tb.Text.Length;

            await Task.Delay(Properties.Settings.Default.TextChangedDelay).ConfigureAwait(true);
            if (startLength == tb.Text.Length)
            {
                Properties.Settings.Default.SessionId = tb.Text;
                await httpClientService.SetSessionId(tb.Text).ConfigureAwait(true);
                CheckSessionId();
            }
        }

        private void ConfigurationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LoadTemplate()
        {
            try
            {
                if (Properties.Settings.Default.Template != null)
                {
                    var configuration = new HtmlConfiguration()
                    {
                        CharacterClass = CharacterClass.Trickster,
                        DeadsAhead = 7,
                        ExperienceAhead = 45527983,
                        ExperienceBehind = 199532,
                        Level = 85,
                        Rank = 27,
                        RankByClass = 6,
                    };
                    var content = htmlService.GetTemplate(Properties.Settings.Default.Template);
                    content = htmlService.UpdateContent(content, configuration);

                    webBrowser.Document.Write(content);
                    webBrowser.Refresh();

                    CheckIfOkButtonCanBeEnabled();
                }
            }
            catch (IOException e)
            {
                logger.Error(e, "Failed to load template");
                Properties.Settings.Default.Template = null;
                InitializeTemplates();
            }
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            var selectedLanguage = item.Value as string;
            Properties.Settings.Default.Language = selectedLanguage;
            formService.SetCulture(selectedLanguage);
            InitializeTranslations();
            InitializeLanguagesCombo();
            DisplayLeagueInfo();
            SetAccountNameError();
            CheckSessionId();
            LoadTemplate();
        }

        private void CheckSessionId()
        {
            if (httpClientService.SessionIdCorrect() && sessionIdTextBox.Text.Length > 0)
            {
                errorKeyProvider.SetError(sessionIdTextBox, "");
                warningKeyProvider.SetError(sessionIdTextBox, "");
                infoKeyProvider.SetError(sessionIdTextBox, Strings.SessionIdCorrect);
            }
            else
            {
                if (sessionIdTextBox.Text.Length == 0)
                {
                    errorKeyProvider.SetError(sessionIdTextBox, "");
                    warningKeyProvider.SetError(sessionIdTextBox, Strings.SessionIdMissing);
                    infoKeyProvider.SetError(sessionIdTextBox, "");
                }
                else
                {
                    errorKeyProvider.SetError(sessionIdTextBox, Strings.SessionIdIncorrect);
                    warningKeyProvider.SetError(sessionIdTextBox, "");
                    infoKeyProvider.SetError(sessionIdTextBox, "");
                }
            }
        }

        private void DisplayLeagueInfo()
        {
            if (selectedLeague == null)
            {
                infoProvider.SetError(leagueRaceCombo, "");
                errorProvider.SetError(leagueRaceCombo, Strings.CannotFindLeague);
            }
            else
            {
                errorProvider.SetError(leagueRaceCombo, "");
                var info = string.Format(CultureInfo.CurrentCulture, Strings.StartDate, selectedLeague.StartAt.ToString("r", CultureInfo.CurrentCulture));
                if (selectedLeague.EndAt != null)
                {
                    info += string.Format(CultureInfo.CurrentCulture, $"\n{Strings.EndDate}", selectedLeague.EndAt?.ToString("r", CultureInfo.CurrentCulture));
                }
                infoProvider.SetError(leagueRaceCombo, info);
            }
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var progress = webBrowser.Document.GetElementsByTagName("progress");
            if (progress != null && progress.Count > 0)
            {
                progress[0].SetAttribute("value", "30");
                progress[0].SetAttribute("max", "100");
            }

            var container = webBrowser.Document.GetElementById("container");
            webBrowser.Visible = container != null;
            if (container != null)
            {
                webBrowser.Size = container.OffsetRectangle.Size;
                webBrowser.Location = new Point(
                    samplePanel.Width / 2 - webBrowser.Size.Width / 2,
                    samplePanel.Height / 2 - webBrowser.Size.Height / 2);
            }
        }

        private void RefreshTemplatesButton_Click(object sender, EventArgs e)
        {
            InitializeTemplates();
            LoadTemplate();
        }
    }
}
