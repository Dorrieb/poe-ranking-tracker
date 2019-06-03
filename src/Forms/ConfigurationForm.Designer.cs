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
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.sessionIdTextBox = new System.Windows.Forms.TextBox();
            this.seesionIdLabel = new System.Windows.Forms.Label();
            this.leagueUrlLink = new System.Windows.Forms.LinkLabel();
            this.charactersComboBox = new System.Windows.Forms.ComboBox();
            this.accountNameTextBox = new System.Windows.Forms.TextBox();
            this.accountNameLabel = new System.Windows.Forms.Label();
            this.infoKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.warningKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorKeyProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.stylePanel = new System.Windows.Forms.Panel();
            this.fontButton = new System.Windows.Forms.Button();
            this.backgroundColor = new System.Windows.Forms.Label();
            this.backgroundColorButton = new System.Windows.Forms.Button();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.showProgressBarCheckBox = new System.Windows.Forms.CheckBox();
            this.showRankByClassCheckbox = new System.Windows.Forms.CheckBox();
            this.showDeadsAheadCheckBox = new System.Windows.Forms.CheckBox();
            this.showExperienceAheadCheckBox = new System.Windows.Forms.CheckBox();
            this.showExperienceBehindCheckBox = new System.Windows.Forms.CheckBox();
            this.samplePanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.globalRankLabel = new System.Windows.Forms.Label();
            this.showExperienceBehindLabel = new System.Windows.Forms.Label();
            this.progressBar = new PoeRankingTracker.Components.TrackerProgressBar();
            this.globalRankValue = new System.Windows.Forms.Label();
            this.showExperienceAheadLabel = new System.Windows.Forms.Label();
            this.showExperienceAheadValue = new System.Windows.Forms.Label();
            this.classRankLabel = new System.Windows.Forms.Label();
            this.classRankValue = new System.Windows.Forms.Label();
            this.deadsAheadValue = new System.Windows.Forms.Label();
            this.deadsAheadlabel = new System.Windows.Forms.Label();
            this.showExperienceBehindValue = new System.Windows.Forms.Label();
            this.sampleOptionsPanel = new System.Windows.Forms.Panel();
            this.sampleLabel = new System.Windows.Forms.Label();
            this.advancedOptionsPanel = new System.Windows.Forms.Panel();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.infoProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.okProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoKeyProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningKeyProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorKeyProvider)).BeginInit();
            this.stylePanel.SuspendLayout();
            this.optionsPanel.SuspendLayout();
            this.samplePanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.okProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(165, 602);
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
            // backgroundColorDialog
            // 
            this.backgroundColorDialog.Color = System.Drawing.Color.White;
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
            this.mainPanel.Controls.Add(this.seesionIdLabel);
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
            // seesionIdLabel
            // 
            this.seesionIdLabel.AutoSize = true;
            this.seesionIdLabel.Location = new System.Drawing.Point(9, 133);
            this.seesionIdLabel.Name = "seesionIdLabel";
            this.seesionIdLabel.Size = new System.Drawing.Size(56, 13);
            this.seesionIdLabel.TabIndex = 9;
            this.seesionIdLabel.Text = "Session Id";
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
            // stylePanel
            // 
            this.stylePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stylePanel.Controls.Add(this.fontButton);
            this.stylePanel.Controls.Add(this.backgroundColor);
            this.stylePanel.Controls.Add(this.backgroundColorButton);
            this.stylePanel.Location = new System.Drawing.Point(12, 319);
            this.stylePanel.Name = "stylePanel";
            this.stylePanel.Size = new System.Drawing.Size(380, 39);
            this.stylePanel.TabIndex = 13;
            // 
            // fontButton
            // 
            this.fontButton.AutoSize = true;
            this.fontButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fontButton.Location = new System.Drawing.Point(22, 6);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(74, 23);
            this.fontButton.TabIndex = 12;
            this.fontButton.Text = "Choose font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new System.EventHandler(this.FontButton_Click);
            // 
            // backgroundColor
            // 
            this.backgroundColor.AutoSize = true;
            this.backgroundColor.Location = new System.Drawing.Point(146, 11);
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.Size = new System.Drawing.Size(91, 13);
            this.backgroundColor.TabIndex = 8;
            this.backgroundColor.Text = "Background color";
            // 
            // backgroundColorButton
            // 
            this.backgroundColorButton.BackColor = global::PoeRankingTracker.Properties.Settings.Default.BackgroundColor;
            this.backgroundColorButton.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::PoeRankingTracker.Properties.Settings.Default, "backgroundColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.backgroundColorButton.Location = new System.Drawing.Point(243, 6);
            this.backgroundColorButton.Name = "backgroundColorButton";
            this.backgroundColorButton.Size = new System.Drawing.Size(75, 23);
            this.backgroundColorButton.TabIndex = 10;
            this.backgroundColorButton.UseVisualStyleBackColor = true;
            this.backgroundColorButton.Click += new System.EventHandler(this.BackgroundColorButton_Click);
            // 
            // optionsPanel
            // 
            this.optionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.optionsPanel.Controls.Add(this.showProgressBarCheckBox);
            this.optionsPanel.Controls.Add(this.showRankByClassCheckbox);
            this.optionsPanel.Controls.Add(this.showDeadsAheadCheckBox);
            this.optionsPanel.Controls.Add(this.showExperienceAheadCheckBox);
            this.optionsPanel.Controls.Add(this.showExperienceBehindCheckBox);
            this.optionsPanel.Location = new System.Drawing.Point(12, 189);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Size = new System.Drawing.Size(380, 121);
            this.optionsPanel.TabIndex = 15;
            // 
            // showProgressBarCheckBox
            // 
            this.showProgressBarCheckBox.AutoSize = true;
            this.showProgressBarCheckBox.Checked = global::PoeRankingTracker.Properties.Settings.Default.ShowProgressBar;
            this.showProgressBarCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PoeRankingTracker.Properties.Settings.Default, "ShowProgressBar", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showProgressBarCheckBox.Location = new System.Drawing.Point(12, 99);
            this.showProgressBarCheckBox.Name = "showProgressBarCheckBox";
            this.showProgressBarCheckBox.Size = new System.Drawing.Size(114, 17);
            this.showProgressBarCheckBox.TabIndex = 15;
            this.showProgressBarCheckBox.Text = "Show progress bar";
            this.showProgressBarCheckBox.UseVisualStyleBackColor = true;
            this.showProgressBarCheckBox.CheckedChanged += new System.EventHandler(this.ShowProgressBarCheckBox_CheckedChanged);
            // 
            // showRankByClassCheckbox
            // 
            this.showRankByClassCheckbox.AutoSize = true;
            this.showRankByClassCheckbox.Checked = global::PoeRankingTracker.Properties.Settings.Default.ShowRankByClass;
            this.showRankByClassCheckbox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PoeRankingTracker.Properties.Settings.Default, "ShowRankByClass", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showRankByClassCheckbox.Location = new System.Drawing.Point(12, 11);
            this.showRankByClassCheckbox.Name = "showRankByClassCheckbox";
            this.showRankByClassCheckbox.Size = new System.Drawing.Size(118, 17);
            this.showRankByClassCheckbox.TabIndex = 14;
            this.showRankByClassCheckbox.Text = "Show rank by class";
            this.showRankByClassCheckbox.UseVisualStyleBackColor = true;
            this.showRankByClassCheckbox.CheckedChanged += new System.EventHandler(this.ShowRankByClassCheckbox_CheckedChanged);
            // 
            // showDeadsAheadCheckBox
            // 
            this.showDeadsAheadCheckBox.AutoSize = true;
            this.showDeadsAheadCheckBox.Checked = global::PoeRankingTracker.Properties.Settings.Default.ShowDeadsAhead;
            this.showDeadsAheadCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PoeRankingTracker.Properties.Settings.Default, "showDeadsAhead", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showDeadsAheadCheckBox.Location = new System.Drawing.Point(12, 33);
            this.showDeadsAheadCheckBox.Name = "showDeadsAheadCheckBox";
            this.showDeadsAheadCheckBox.Size = new System.Drawing.Size(323, 17);
            this.showDeadsAheadCheckBox.TabIndex = 11;
            this.showDeadsAheadCheckBox.Text = "Show number of deads/retired ahead, until next alive character";
            this.showDeadsAheadCheckBox.UseVisualStyleBackColor = true;
            this.showDeadsAheadCheckBox.CheckedChanged += new System.EventHandler(this.ShowDeadsAheadCheckBox_CheckedChanged);
            // 
            // showExperienceAheadCheckBox
            // 
            this.showExperienceAheadCheckBox.AutoSize = true;
            this.showExperienceAheadCheckBox.Checked = global::PoeRankingTracker.Properties.Settings.Default.ShowExperienceAhead;
            this.showExperienceAheadCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PoeRankingTracker.Properties.Settings.Default, "showExperienceAhead", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showExperienceAheadCheckBox.Location = new System.Drawing.Point(12, 56);
            this.showExperienceAheadCheckBox.Name = "showExperienceAheadCheckBox";
            this.showExperienceAheadCheckBox.Size = new System.Drawing.Size(256, 17);
            this.showExperienceAheadCheckBox.TabIndex = 12;
            this.showExperienceAheadCheckBox.Text = "Show missing experience points to gain one rank";
            this.showExperienceAheadCheckBox.UseVisualStyleBackColor = true;
            this.showExperienceAheadCheckBox.CheckedChanged += new System.EventHandler(this.ShowExperienceAheadCheckBox_CheckedChanged);
            // 
            // showExperienceBehindCheckBox
            // 
            this.showExperienceBehindCheckBox.AutoSize = true;
            this.showExperienceBehindCheckBox.Checked = global::PoeRankingTracker.Properties.Settings.Default.ShowExperienceBehind;
            this.showExperienceBehindCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PoeRankingTracker.Properties.Settings.Default, "showExperienceBehind", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showExperienceBehindCheckBox.Location = new System.Drawing.Point(12, 79);
            this.showExperienceBehindCheckBox.Name = "showExperienceBehindCheckBox";
            this.showExperienceBehindCheckBox.Size = new System.Drawing.Size(256, 17);
            this.showExperienceBehindCheckBox.TabIndex = 13;
            this.showExperienceBehindCheckBox.Text = "Show present experience points to lose one rank";
            this.showExperienceBehindCheckBox.UseVisualStyleBackColor = true;
            this.showExperienceBehindCheckBox.CheckedChanged += new System.EventHandler(this.ShowExperienceBehindCheckBox_CheckedChanged);
            // 
            // samplePanel
            // 
            this.samplePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.samplePanel.Controls.Add(this.tableLayoutPanel);
            this.samplePanel.Controls.Add(this.sampleOptionsPanel);
            this.samplePanel.Controls.Add(this.sampleLabel);
            this.samplePanel.Location = new System.Drawing.Point(12, 367);
            this.samplePanel.Name = "samplePanel";
            this.samplePanel.Size = new System.Drawing.Size(380, 209);
            this.samplePanel.TabIndex = 14;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.BackColor = global::PoeRankingTracker.Properties.Settings.Default.BackgroundColor;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.globalRankLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.showExperienceBehindLabel, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.progressBar, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.globalRankValue, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.showExperienceAheadLabel, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.showExperienceAheadValue, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.classRankLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.classRankValue, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.deadsAheadValue, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.deadsAheadlabel, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.showExperienceBehindValue, 1, 4);
            this.tableLayoutPanel.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::PoeRankingTracker.Properties.Settings.Default, "Font", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tableLayoutPanel.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::PoeRankingTracker.Properties.Settings.Default, "BackgroundColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tableLayoutPanel.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::PoeRankingTracker.Properties.Settings.Default, "FontColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tableLayoutPanel.Font = global::PoeRankingTracker.Properties.Settings.Default.Font;
            this.tableLayoutPanel.ForeColor = global::PoeRankingTracker.Properties.Settings.Default.FontColor;
            this.tableLayoutPanel.Location = new System.Drawing.Point(92, 52);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(135, 70);
            this.tableLayoutPanel.TabIndex = 15;
            // 
            // globalRankLabel
            // 
            this.globalRankLabel.AutoSize = true;
            this.globalRankLabel.Location = new System.Drawing.Point(3, 0);
            this.globalRankLabel.Name = "globalRankLabel";
            this.globalRankLabel.Size = new System.Drawing.Size(61, 13);
            this.globalRankLabel.TabIndex = 0;
            this.globalRankLabel.Text = "Global rank";
            // 
            // showExperienceBehindLabel
            // 
            this.showExperienceBehindLabel.AutoSize = true;
            this.showExperienceBehindLabel.Location = new System.Drawing.Point(3, 52);
            this.showExperienceBehindLabel.Name = "showExperienceBehindLabel";
            this.showExperienceBehindLabel.Size = new System.Drawing.Size(79, 13);
            this.showExperienceBehindLabel.TabIndex = 8;
            this.showExperienceBehindLabel.Text = "XP to lose rank";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel.SetColumnSpan(this.progressBar, 2);
            this.progressBar.Location = new System.Drawing.Point(0, 65);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(135, 5);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 16;
            this.progressBar.Value = 68;
            // 
            // globalRankValue
            // 
            this.globalRankValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.globalRankValue.AutoSize = true;
            this.globalRankValue.Location = new System.Drawing.Point(113, 0);
            this.globalRankValue.Name = "globalRankValue";
            this.globalRankValue.Size = new System.Drawing.Size(19, 13);
            this.globalRankValue.TabIndex = 1;
            this.globalRankValue.Text = "27";
            // 
            // showExperienceAheadLabel
            // 
            this.showExperienceAheadLabel.AutoSize = true;
            this.showExperienceAheadLabel.Location = new System.Drawing.Point(3, 39);
            this.showExperienceAheadLabel.Name = "showExperienceAheadLabel";
            this.showExperienceAheadLabel.Size = new System.Drawing.Size(80, 13);
            this.showExperienceAheadLabel.TabIndex = 6;
            this.showExperienceAheadLabel.Text = "XP to gain rank";
            // 
            // showExperienceAheadValue
            // 
            this.showExperienceAheadValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showExperienceAheadValue.AutoSize = true;
            this.showExperienceAheadValue.Location = new System.Drawing.Point(89, 39);
            this.showExperienceAheadValue.Name = "showExperienceAheadValue";
            this.showExperienceAheadValue.Size = new System.Drawing.Size(43, 13);
            this.showExperienceAheadValue.TabIndex = 7;
            this.showExperienceAheadValue.Text = "527983";
            // 
            // classRankLabel
            // 
            this.classRankLabel.AutoSize = true;
            this.classRankLabel.Location = new System.Drawing.Point(3, 13);
            this.classRankLabel.Name = "classRankLabel";
            this.classRankLabel.Size = new System.Drawing.Size(56, 13);
            this.classRankLabel.TabIndex = 2;
            this.classRankLabel.Text = "Class rank";
            // 
            // classRankValue
            // 
            this.classRankValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.classRankValue.AutoSize = true;
            this.classRankValue.Location = new System.Drawing.Point(119, 13);
            this.classRankValue.Name = "classRankValue";
            this.classRankValue.Size = new System.Drawing.Size(13, 13);
            this.classRankValue.TabIndex = 3;
            this.classRankValue.Text = "2";
            // 
            // deadsAheadValue
            // 
            this.deadsAheadValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deadsAheadValue.AutoSize = true;
            this.deadsAheadValue.Location = new System.Drawing.Point(119, 26);
            this.deadsAheadValue.Name = "deadsAheadValue";
            this.deadsAheadValue.Size = new System.Drawing.Size(13, 13);
            this.deadsAheadValue.TabIndex = 5;
            this.deadsAheadValue.Text = "0";
            // 
            // deadsAheadlabel
            // 
            this.deadsAheadlabel.AutoSize = true;
            this.deadsAheadlabel.Location = new System.Drawing.Point(3, 26);
            this.deadsAheadlabel.Name = "deadsAheadlabel";
            this.deadsAheadlabel.Size = new System.Drawing.Size(69, 13);
            this.deadsAheadlabel.TabIndex = 4;
            this.deadsAheadlabel.Text = "DeadsAhead";
            // 
            // showExperienceBehindValue
            // 
            this.showExperienceBehindValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showExperienceBehindValue.AutoSize = true;
            this.showExperienceBehindValue.Location = new System.Drawing.Point(89, 52);
            this.showExperienceBehindValue.Name = "showExperienceBehindValue";
            this.showExperienceBehindValue.Size = new System.Drawing.Size(43, 13);
            this.showExperienceBehindValue.TabIndex = 9;
            this.showExperienceBehindValue.Text = "199532";
            // 
            // sampleOptionsPanel
            // 
            this.sampleOptionsPanel.AutoSize = true;
            this.sampleOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sampleOptionsPanel.BackColor = global::PoeRankingTracker.Properties.Settings.Default.BackgroundColor;
            this.sampleOptionsPanel.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::PoeRankingTracker.Properties.Settings.Default, "BackgroundColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.sampleOptionsPanel.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::PoeRankingTracker.Properties.Settings.Default, "Font", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.sampleOptionsPanel.Font = global::PoeRankingTracker.Properties.Settings.Default.Font;
            this.sampleOptionsPanel.Location = new System.Drawing.Point(11, 32);
            this.sampleOptionsPanel.Name = "sampleOptionsPanel";
            this.sampleOptionsPanel.Size = new System.Drawing.Size(0, 0);
            this.sampleOptionsPanel.TabIndex = 14;
            // 
            // sampleLabel
            // 
            this.sampleLabel.AutoSize = true;
            this.sampleLabel.Location = new System.Drawing.Point(9, 10);
            this.sampleLabel.Name = "sampleLabel";
            this.sampleLabel.Size = new System.Drawing.Size(121, 13);
            this.sampleLabel.TabIndex = 13;
            this.sampleLabel.Text = "Preview (dummy values)";
            // 
            // advancedOptionsPanel
            // 
            this.advancedOptionsPanel.AutoSize = true;
            this.advancedOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.advancedOptionsPanel.Location = new System.Drawing.Point(210, 316);
            this.advancedOptionsPanel.Name = "advancedOptionsPanel";
            this.advancedOptionsPanel.Size = new System.Drawing.Size(0, 0);
            this.advancedOptionsPanel.TabIndex = 18;
            this.advancedOptionsPanel.Visible = false;
            // 
            // fontDialog
            // 
            this.fontDialog.AllowScriptChange = false;
            this.fontDialog.AllowSimulations = false;
            this.fontDialog.AllowVectorFonts = false;
            this.fontDialog.AllowVerticalFonts = false;
            this.fontDialog.Color = global::PoeRankingTracker.Properties.Settings.Default.FontColor;
            this.fontDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.fontDialog.MaxSize = 12;
            this.fontDialog.MinSize = 8;
            this.fontDialog.ShowColor = true;
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
            this.ClientSize = new System.Drawing.Size(404, 635);
            this.Controls.Add(this.samplePanel);
            this.Controls.Add(this.advancedOptionsPanel);
            this.Controls.Add(this.stylePanel);
            this.Controls.Add(this.optionsPanel);
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
            this.stylePanel.ResumeLayout(false);
            this.stylePanel.PerformLayout();
            this.optionsPanel.ResumeLayout(false);
            this.optionsPanel.PerformLayout();
            this.samplePanel.ResumeLayout(false);
            this.samplePanel.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.okProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox leagueRaceCombo;
        private System.Windows.Forms.Label leagueRaceLabel;
        private System.Windows.Forms.Label characterNameLabel;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ComboBox charactersComboBox;
        private System.Windows.Forms.TextBox accountNameTextBox;
        private System.Windows.Forms.Label accountNameLabel;
        private System.Windows.Forms.LinkLabel leagueUrlLink;
        private System.Windows.Forms.TextBox sessionIdTextBox;
        private System.Windows.Forms.Label seesionIdLabel;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.ErrorProvider infoKeyProvider;
        private System.Windows.Forms.ErrorProvider warningKeyProvider;
        private System.Windows.Forms.ErrorProvider errorKeyProvider;
        private System.Windows.Forms.Panel samplePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label globalRankLabel;
        private System.Windows.Forms.Label showExperienceBehindLabel;
        private System.Windows.Forms.Label globalRankValue;
        private System.Windows.Forms.Label showExperienceAheadLabel;
        private System.Windows.Forms.Label showExperienceAheadValue;
        private System.Windows.Forms.Label classRankLabel;
        private System.Windows.Forms.Label classRankValue;
        private System.Windows.Forms.Label deadsAheadValue;
        private System.Windows.Forms.Label deadsAheadlabel;
        private System.Windows.Forms.Label showExperienceBehindValue;
        private System.Windows.Forms.Panel sampleOptionsPanel;
        private System.Windows.Forms.Label sampleLabel;
        private System.Windows.Forms.Panel advancedOptionsPanel;
        private System.Windows.Forms.Panel stylePanel;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.Label backgroundColor;
        private System.Windows.Forms.Button backgroundColorButton;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.CheckBox showRankByClassCheckbox;
        private System.Windows.Forms.CheckBox showDeadsAheadCheckBox;
        private System.Windows.Forms.CheckBox showExperienceAheadCheckBox;
        private System.Windows.Forms.CheckBox showExperienceBehindCheckBox;
        private System.Windows.Forms.CheckBox showProgressBarCheckBox;
        private Components.TrackerProgressBar progressBar;
        private System.Windows.Forms.ErrorProvider infoProvider;
        private System.Windows.Forms.ErrorProvider okProvider;
    }
}

