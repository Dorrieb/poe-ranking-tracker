namespace PoeRankingTracker.Forms
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.okButton = new System.Windows.Forms.Button();
            this.leagueRaceCombo = new System.Windows.Forms.ComboBox();
            this.leagueRaceLabel = new System.Windows.Forms.Label();
            this.characterNameLabel = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.sessionIdTextBox = new System.Windows.Forms.TextBox();
            this.sessionIdLabel = new System.Windows.Forms.Label();
            this.leagueUrlLink = new System.Windows.Forms.LinkLabel();
            this.charactersComboBox = new System.Windows.Forms.ComboBox();
            this.accountNameTextBox = new System.Windows.Forms.TextBox();
            this.accountNameLabel = new System.Windows.Forms.Label();
            this.refreshTemplatesButton = new System.Windows.Forms.Button();
            this.infoKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.warningKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.templatesComboBox = new System.Windows.Forms.ComboBox();
            this.samplePanel = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.sampleLabel = new System.Windows.Forms.Label();
            this.infoProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.okProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.refreshTemplatesButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoKeyProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningKeyProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorKeyProvider)).BeginInit();
            this.samplePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.okProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(165, 482);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // leagueRaceCombo
            // 
            this.leagueRaceCombo.FormattingEnabled = true;
            this.leagueRaceCombo.Location = new System.Drawing.Point(96, 41);
            this.leagueRaceCombo.Name = "leagueRaceCombo";
            this.leagueRaceCombo.Size = new System.Drawing.Size(234, 21);
            this.leagueRaceCombo.TabIndex = 1;
            this.leagueRaceCombo.SelectedIndexChanged += new System.EventHandler(this.LeagueRaceCombo_SelectedIndexChanged);
            this.leagueRaceCombo.TextUpdate += new System.EventHandler(this.LeagueRaceCombo_TextUpdate);
            // 
            // leagueRaceLabel
            // 
            this.leagueRaceLabel.AutoSize = true;
            this.leagueRaceLabel.Location = new System.Drawing.Point(9, 44);
            this.leagueRaceLabel.Name = "leagueRaceLabel";
            this.leagueRaceLabel.Size = new System.Drawing.Size(74, 13);
            this.leagueRaceLabel.TabIndex = 2;
            this.leagueRaceLabel.Text = "League/Race";
            // 
            // characterNameLabel
            // 
            this.characterNameLabel.AutoSize = true;
            this.characterNameLabel.Location = new System.Drawing.Point(9, 103);
            this.characterNameLabel.Name = "characterNameLabel";
            this.characterNameLabel.Size = new System.Drawing.Size(82, 13);
            this.characterNameLabel.TabIndex = 3;
            this.characterNameLabel.Text = "Character name";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.languageComboBox);
            this.mainPanel.Controls.Add(this.languageLabel);
            this.mainPanel.Controls.Add(this.sessionIdTextBox);
            this.mainPanel.Controls.Add(this.sessionIdLabel);
            this.mainPanel.Controls.Add(this.leagueUrlLink);
            this.mainPanel.Controls.Add(this.charactersComboBox);
            this.mainPanel.Controls.Add(this.accountNameTextBox);
            this.mainPanel.Controls.Add(this.accountNameLabel);
            this.mainPanel.Controls.Add(this.leagueRaceLabel);
            this.mainPanel.Controls.Add(this.leagueRaceCombo);
            this.mainPanel.Controls.Add(this.characterNameLabel);
            this.mainPanel.Location = new System.Drawing.Point(12, 17);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(380, 163);
            this.mainPanel.TabIndex = 11;
            // 
            // languageComboBox
            // 
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Location = new System.Drawing.Point(96, 11);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(234, 21);
            this.languageComboBox.TabIndex = 16;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.Location = new System.Drawing.Point(9, 14);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(55, 13);
            this.languageLabel.TabIndex = 15;
            this.languageLabel.Text = "Language";
            // 
            // sessionIdTextBox
            // 
            this.sessionIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PoeRankingTracker.Properties.Settings.Default, "SessionId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.sessionIdTextBox.Location = new System.Drawing.Point(96, 130);
            this.sessionIdTextBox.Name = "sessionIdTextBox";
            this.sessionIdTextBox.Size = new System.Drawing.Size(234, 20);
            this.sessionIdTextBox.TabIndex = 10;
            this.sessionIdTextBox.Text = global::PoeRankingTracker.Properties.Settings.Default.SessionId;
            this.sessionIdTextBox.UseSystemPasswordChar = true;
            this.sessionIdTextBox.TextChanged += new System.EventHandler(this.SessionIdTextBox_TextChanged);
            // 
            // sessionIdLabel
            // 
            this.sessionIdLabel.AutoSize = true;
            this.sessionIdLabel.Location = new System.Drawing.Point(9, 133);
            this.sessionIdLabel.Name = "sessionIdLabel";
            this.sessionIdLabel.Size = new System.Drawing.Size(56, 13);
            this.sessionIdLabel.TabIndex = 9;
            this.sessionIdLabel.Text = "Session Id";
            // 
            // leagueUrlLink
            // 
            this.leagueUrlLink.AutoSize = true;
            this.leagueUrlLink.Enabled = false;
            this.leagueUrlLink.Location = new System.Drawing.Point(348, 44);
            this.leagueUrlLink.Name = "leagueUrlLink";
            this.leagueUrlLink.Size = new System.Drawing.Size(24, 13);
            this.leagueUrlLink.TabIndex = 8;
            this.leagueUrlLink.TabStop = true;
            this.leagueUrlLink.Text = "info";
            this.leagueUrlLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LeagueUrlLink_LinkClicked);
            // 
            // charactersComboBox
            // 
            this.charactersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.charactersComboBox.FormattingEnabled = true;
            this.charactersComboBox.Location = new System.Drawing.Point(96, 100);
            this.charactersComboBox.Name = "charactersComboBox";
            this.charactersComboBox.Size = new System.Drawing.Size(234, 21);
            this.charactersComboBox.TabIndex = 7;
            this.charactersComboBox.SelectedIndexChanged += new System.EventHandler(this.CharactersComboBox_SelectedIndexChanged);
            // 
            // accountNameTextBox
            // 
            this.accountNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PoeRankingTracker.Properties.Settings.Default, "accountName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.accountNameTextBox.Location = new System.Drawing.Point(96, 71);
            this.accountNameTextBox.Name = "accountNameTextBox";
            this.accountNameTextBox.Size = new System.Drawing.Size(234, 20);
            this.accountNameTextBox.TabIndex = 6;
            this.accountNameTextBox.Text = global::PoeRankingTracker.Properties.Settings.Default.AccountName;
            this.accountNameTextBox.TextChanged += new System.EventHandler(this.AccountNameTextBox_TextChanged);
            // 
            // accountNameLabel
            // 
            this.accountNameLabel.AutoSize = true;
            this.accountNameLabel.Location = new System.Drawing.Point(9, 74);
            this.accountNameLabel.Name = "accountNameLabel";
            this.accountNameLabel.Size = new System.Drawing.Size(76, 13);
            this.accountNameLabel.TabIndex = 5;
            this.accountNameLabel.Text = "Account name";
            // 
            // refreshTemplatesButton
            // 
            this.refreshTemplatesButton.Image = global::PoeRankingTracker.Properties.Resources.Refresh_16x;
            this.refreshTemplatesButton.Location = new System.Drawing.Point(344, 9);
            this.refreshTemplatesButton.Name = "refreshTemplatesButton";
            this.refreshTemplatesButton.Size = new System.Drawing.Size(28, 23);
            this.refreshTemplatesButton.TabIndex = 17;
            this.refreshTemplatesButton.UseVisualStyleBackColor = true;
            this.refreshTemplatesButton.Click += new System.EventHandler(this.RefreshTemplatesButton_Click);
            // 
            // infoKeyProvider
            // 
            this.infoKeyProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.infoKeyProvider.ContainerControl = this;
            // 
            // warningKeyProvider
            // 
            this.warningKeyProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.warningKeyProvider.ContainerControl = this;
            // 
            // errorKeyProvider
            // 
            this.errorKeyProvider.ContainerControl = this;
            // 
            // templatesComboBox
            // 
            this.templatesComboBox.FormattingEnabled = true;
            this.templatesComboBox.Location = new System.Drawing.Point(12, 11);
            this.templatesComboBox.Name = "templatesComboBox";
            this.templatesComboBox.Size = new System.Drawing.Size(318, 21);
            this.templatesComboBox.TabIndex = 1;
            this.templatesComboBox.Text = "Choose a template";
            this.templatesComboBox.SelectedIndexChanged += new System.EventHandler(this.TemplatesComboBox_SelectedIndexChanged);
            // 
            // samplePanel
            // 
            this.samplePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.samplePanel.Controls.Add(this.refreshTemplatesButton);
            this.samplePanel.Controls.Add(this.templatesComboBox);
            this.samplePanel.Controls.Add(this.webBrowser);
            this.samplePanel.Controls.Add(this.sampleLabel);
            this.samplePanel.Location = new System.Drawing.Point(12, 189);
            this.samplePanel.Name = "samplePanel";
            this.samplePanel.Size = new System.Drawing.Size(380, 287);
            this.samplePanel.TabIndex = 14;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(12, 68);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(349, 204);
            this.webBrowser.TabIndex = 15;
            this.webBrowser.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webBrowser.Visible = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
            // 
            // sampleLabel
            // 
            this.sampleLabel.AutoSize = true;
            this.sampleLabel.Location = new System.Drawing.Point(9, 40);
            this.sampleLabel.Name = "sampleLabel";
            this.sampleLabel.Size = new System.Drawing.Size(121, 13);
            this.sampleLabel.TabIndex = 13;
            this.sampleLabel.Text = "Preview (dummy values)";
            // 
            // infoProvider
            // 
            this.infoProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.infoProvider.ContainerControl = this;
            // 
            // okProvider
            // 
            this.okProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.okProvider.ContainerControl = this;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(404, 514);
            this.Controls.Add(this.samplePanel);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POE Ranking tracker - Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigurationForm_FormClosed);
            this.Move += new System.EventHandler(this.ConfigurationForm_Move);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoKeyProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningKeyProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorKeyProvider)).EndInit();
            this.samplePanel.ResumeLayout(false);
            this.samplePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.okProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox leagueRaceCombo;
        private System.Windows.Forms.Label leagueRaceLabel;
        private System.Windows.Forms.Label characterNameLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ComboBox charactersComboBox;
        private System.Windows.Forms.TextBox accountNameTextBox;
        private System.Windows.Forms.Label accountNameLabel;
        private System.Windows.Forms.LinkLabel leagueUrlLink;
        private System.Windows.Forms.TextBox sessionIdTextBox;
        private System.Windows.Forms.Label sessionIdLabel;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.ErrorProvider infoKeyProvider;
        private System.Windows.Forms.ErrorProvider warningKeyProvider;
        private System.Windows.Forms.ErrorProvider errorKeyProvider;
        private System.Windows.Forms.Panel samplePanel;
        private System.Windows.Forms.Label sampleLabel;
        private System.Windows.Forms.ErrorProvider infoProvider;
        private System.Windows.Forms.ErrorProvider okProvider;
        private System.Windows.Forms.ComboBox templatesComboBox;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Button refreshTemplatesButton;
        private System.Windows.Forms.ToolTip refreshTemplatesButtonToolTip;
    }
}

