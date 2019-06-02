using PoeRankingTracker.Models;
using PoeRankingTracker.Resources.Translations;
using PoeRankingTracker.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoeRankingTracker.Forms
{
    public partial class ConfigurationForm : Form
    {
        private League selectedLeague = null;
        private string accountName = Properties.Settings.Default.AccountName;
        private Entry selectedEntry = null;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IHttpClientService httpClientService;
        private ISemaphoreService semaphoreService;

        public ConfigurationForm(IHttpClientService httpClientService, ISemaphoreService semaphoreService)
        {
            this.httpClientService = httpClientService;
            this.semaphoreService = semaphoreService;
            InitializeComponent();
            InitializeTranslations();
            InitializePosition();
            InitializeFontAndOptions();
            InitializeLanguagesCombo();
            InitializeLeagueRaceCombo();
        }
        
        private void InitializeTranslations()
        {
            if (Properties.Settings.Default.Language.Length > 0)
            {
                SetCulture(Properties.Settings.Default.Language);
            }

            Text = Strings.Configuration;
            leagueRaceLabel.Text = Strings.League;
            accountNameLabel.Text = Strings.AccountName;
            characterNameLabel.Text = Strings.CharacterName;
            seesionIdLabel.Text = Strings.SessionId;
            showRankByClassCheckbox.Text = Strings.ShowRankByClass;
            showDeadsAheadCheckBox.Text = Strings.ShowDeadsAhead;
            showExperienceAheadCheckBox.Text = Strings.ShowExperienceAhead;
            showExperienceBehindCheckBox.Text = Strings.ShowExperienceBehind;
            showProgressBarCheckBox.Text = Strings.ShowProgressBar;
            fontButton.Text = Strings.ChooseFont;
            backgroundColor.Text = Strings.BackgroundColor;
            sampleLabel.Text = Strings.PreviewLabel;
            globalRankValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", 27);
            classRankValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", 2);
            showExperienceAheadValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", 527983);
            showExperienceBehindValue.Text = string.Format(CultureInfo.CurrentCulture, "{0:#,0}", 199532);
            globalRankLabel.Text = Strings.GlobalRank;
            classRankLabel.Text = Strings.ClassRank;
            deadsAheadlabel.Text = Strings.DeadsAhead;
            showExperienceAheadLabel.Text = Strings.ExperienceAhead;
            showExperienceBehindLabel.Text = Strings.ExperienceBehind;
            okButton.Text = Strings.OK;
            languageLabel.Text = Strings.Language;
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
            fontDialog.Font = Properties.Settings.Default.Font;
            fontDialog.Color = Properties.Settings.Default.FontColor;
            classRankLabel.Visible = Properties.Settings.Default.ShowRankByClass;
            classRankValue.Visible = Properties.Settings.Default.ShowRankByClass;
            deadsAheadlabel.Visible = Properties.Settings.Default.ShowDeadsAhead;
            deadsAheadValue.Visible = Properties.Settings.Default.ShowDeadsAhead;
            showExperienceAheadLabel.Visible = Properties.Settings.Default.ShowExperienceAhead;
            showExperienceAheadValue.Visible = Properties.Settings.Default.ShowExperienceAhead;
            showExperienceBehindLabel.Visible = Properties.Settings.Default.ShowExperienceBehind;
            showExperienceBehindValue.Visible = Properties.Settings.Default.ShowExperienceBehind;
            progressBar.Visible = Properties.Settings.Default.ShowProgressBar;
            progressBar.SetColor(Properties.Settings.Default.FontColor);
            infoKeyProvider.Icon = Icon.FromHandle(Properties.Resources.Key_16x.GetHicon());
            warningKeyProvider.Icon = Icon.FromHandle(Properties.Resources.KeyWarning_16x.GetHicon());
            errorKeyProvider.Icon = Icon.FromHandle(Properties.Resources.KeyError_16x.GetHicon());
            await httpClientService.SetSessionId(Properties.Settings.Default.SessionId).ConfigureAwait(true);
            CheckSessionId();
            CenterPanel();
        }

        private async void InitializeLeagueRaceCombo()
        {
            try
            {
                List<League> leagues = await httpClientService.GetLeaguesAsync().ConfigureAwait(true);
                CheckSessionId();

                semaphoreService.CreateSemaphore();

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

            InitializeCharactersList();
        }

        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog() == DialogResult.OK)
            {
                backgroundColorButton.BackColor = backgroundColorDialog.Color;
                Properties.Settings.Default.BackgroundColor = backgroundColorDialog.Color;
            }
        }

        private void LeagueRaceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            selectedLeague = item.Value as League;

            Properties.Settings.Default.LeagueId = selectedLeague.Id;

            leagueUrlLink.Enabled = true;

            InitializeCharactersList();
        }

        private async void LeagueRaceCombo_TextUpdate(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int startLength = cb.Text.Length;

            await Task.Delay(Properties.Settings.Default.TextChangedDelay).ConfigureAwait(true);
            if (startLength == cb.Text.Length)
            {
                var leagueId = cb.Text;
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
            okButton.Enabled = selectedEntry != null;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Hide();

            var configuration = new TrackerConfiguration
            {
                League = selectedLeague,
                Entry = selectedEntry,
                Font = fontDialog.Font,
                FontColor = fontDialog.Color,
                BackgroundColor = backgroundColorButton.BackColor,
                ShowDeadsAhead = showDeadsAheadCheckBox.Checked,
                ShowExperienceAhead = showExperienceAheadCheckBox.Checked,
                ShowExperienceBehind = showExperienceBehindCheckBox.Checked,
                ShowProgressBar = showProgressBarCheckBox.Checked,
                AccountName = accountNameTextBox.Text,
                Culture = CultureInfo.CurrentCulture,
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
                Ladder ladder = await httpClientService.GetLadderAsync(selectedLeague.Id, accountName).ConfigureAwait(true);
                try
                {
                    selectedEntry = null;
                    CheckIfOkButtonCanBeEnabled();
                    if (ladder == null || (ladder.Entries != null && ladder.Entries.Count == 0))
                    {
                        errorProvider.SetError(accountNameTextBox, string.Format(CultureInfo.CurrentCulture, Strings.NoCharactersFoundForAccount, accountName));
                    }
                    else
                    {
                        errorProvider.SetError(accountNameTextBox, "");
                        foreach (Entry entry in ladder.Entries)
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
                        if (charactersComboBox.Items.Count == 0)
                        {
                            errorProvider.SetError(accountNameTextBox, string.Format(CultureInfo.CurrentCulture, Strings.NoCharactersFoundForAccount, accountName));
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    logger.Error(e, "Unable to retrieve characters");
                    errorProvider.SetError(accountNameTextBox, Strings.UnableToRetrieveCharacters);
                }
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

        private void ShowRankByClassCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var b = (sender as CheckBox).Checked;
            Properties.Settings.Default.ShowRankByClass = b;
            classRankLabel.Visible = b;
            classRankValue.Visible = b;
            CenterPanel();
        }

        private void ShowDeadsAheadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var b = (sender as CheckBox).Checked;
            Properties.Settings.Default.ShowDeadsAhead = b;
            deadsAheadlabel.Visible = b;
            deadsAheadValue.Visible = b;
            CenterPanel();
        }

        private void ShowExperienceAheadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var b = (sender as CheckBox).Checked;
            Properties.Settings.Default.ShowExperienceAhead = b;
            showExperienceAheadLabel.Visible = b;
            showExperienceAheadValue.Visible = b;
            CenterPanel();
        }

        private void ShowExperienceBehindCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var b = (sender as CheckBox).Checked;
            Properties.Settings.Default.ShowExperienceBehind = b;
            showExperienceBehindLabel.Visible = b;
            showExperienceBehindValue.Visible = b;
            CenterPanel();
        }

        private void ShowProgressBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var b = (sender as CheckBox).Checked;
            Properties.Settings.Default.ShowProgressBar = b;
            progressBar.Visible = b;
            CenterPanel();
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
                semaphoreService.CreateSemaphore();
            }
        }

        private void ConfigurationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            fontDialog.Font = Properties.Settings.Default.Font;

            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                tableLayoutPanel.Font = fontDialog.Font;
                tableLayoutPanel.ForeColor = fontDialog.Color;
                progressBar.SetColor(fontDialog.Color);
                Properties.Settings.Default.Font = fontDialog.Font;
                Properties.Settings.Default.FontColor = fontDialog.Color;
                CenterPanel();
            }
        }
        private void CenterPanel()
        {
            tableLayoutPanel.Location = new Point(
                this.samplePanel.Width / 2 - tableLayoutPanel.Size.Width / 2,
                this.samplePanel.Height / 2 - tableLayoutPanel.Size.Height / 2);
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            var selectedLanguage = item.Value as string;
            Properties.Settings.Default.Language = selectedLanguage;
            SetCulture(selectedLanguage);
            InitializeTranslations();
            InitializeLanguagesCombo();
            CheckSessionId();
            CenterPanel();
        }

        private static void SetCulture(string language)
        {
            var culture = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
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

        private void AdvancedOptionsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            advancedOptionsPanel.Visible = !advancedOptionsPanel.Visible;
        }
    }
}
