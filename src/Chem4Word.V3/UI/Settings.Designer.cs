using Chem4Word.Core.UI.Controls;

namespace Chem4Word.UI
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSearcherSettings = new System.Windows.Forms.Button();
            this.btnRendererSettings = new System.Windows.Forms.Button();
            this.btnEditorSettings = new System.Windows.Forms.Button();
            this.lblSearcherDescription = new System.Windows.Forms.Label();
            this.cboSearchers = new System.Windows.Forms.ComboBox();
            this.lblRendererDescription = new System.Windows.Forms.Label();
            this.lblEditorDescription = new System.Windows.Forms.Label();
            this.cboRenderers = new System.Windows.Forms.ComboBox();
            this.cboEditors = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtChemSpiderRdfUri = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChemSpiderWsUri = new System.Windows.Forms.TextBox();
            this.chkUseWebServices = new System.Windows.Forms.CheckBox();
            this.chkTelemetryEnabled = new System.Windows.Forms.CheckBox();
            this.cboUpdateFrequency = new System.Windows.Forms.ComboBox();
            this.chkAutomaticUpdates = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControlEx1 = new Chem4Word.Core.UI.Controls.TabControlEx();
            this.tabPlugIns = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RendererGroup = new System.Windows.Forms.GroupBox();
            this.EditorGroup = new System.Windows.Forms.GroupBox();
            this.tabWebServices = new System.Windows.Forms.TabPage();
            this.lblProWebServices = new System.Windows.Forms.Label();
            this.tabTelemetry = new System.Windows.Forms.TabPage();
            this.lblProTelemetry = new System.Windows.Forms.Label();
            this.tabUpdates = new System.Windows.Forms.TabPage();
            this.lblProUpdates = new System.Windows.Forms.Label();
            this.tabLibrary = new System.Windows.Forms.TabPage();
            this.ClearLibraryButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblGalleryDesc = new System.Windows.Forms.Label();
            this.importGalleryButton = new System.Windows.Forms.Button();
            this.tabMaintenance = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MaintenanceInformation = new System.Windows.Forms.Label();
            this.OpenPlugInFolder = new System.Windows.Forms.Button();
            this.OpenLibraryFolder = new System.Windows.Forms.Button();
            this.OpenSettingsFolder = new System.Windows.Forms.Button();
            this.tabControlEx1.SuspendLayout();
            this.tabPlugIns.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.RendererGroup.SuspendLayout();
            this.EditorGroup.SuspendLayout();
            this.tabWebServices.SuspendLayout();
            this.tabTelemetry.SuspendLayout();
            this.tabUpdates.SuspendLayout();
            this.tabLibrary.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabMaintenance.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(536, 389);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnOkClick);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaults.Location = new System.Drawing.Point(442, 389);
            this.btnSetDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(88, 28);
            this.btnSetDefaults.TabIndex = 9;
            this.btnSetDefaults.Text = "Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.OnSetDefaultsClick);
            // 
            // btnSearcherSettings
            // 
            this.btnSearcherSettings.Enabled = false;
            this.btnSearcherSettings.FlatAppearance.BorderSize = 0;
            this.btnSearcherSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearcherSettings.Image = global::Chem4Word.Properties.Resources.Preferences;
            this.btnSearcherSettings.Location = new System.Drawing.Point(510, 15);
            this.btnSearcherSettings.Name = "btnSearcherSettings";
            this.btnSearcherSettings.Size = new System.Drawing.Size(36, 36);
            this.btnSearcherSettings.TabIndex = 23;
            this.toolTip1.SetToolTip(this.btnSearcherSettings, "Renderer Options");
            this.btnSearcherSettings.UseVisualStyleBackColor = true;
            this.btnSearcherSettings.Click += new System.EventHandler(this.OnSearcherSettingsClick);
            // 
            // btnRendererSettings
            // 
            this.btnRendererSettings.Enabled = false;
            this.btnRendererSettings.FlatAppearance.BorderSize = 0;
            this.btnRendererSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRendererSettings.Image = global::Chem4Word.Properties.Resources.Preferences;
            this.btnRendererSettings.Location = new System.Drawing.Point(510, 15);
            this.btnRendererSettings.Name = "btnRendererSettings";
            this.btnRendererSettings.Size = new System.Drawing.Size(36, 36);
            this.btnRendererSettings.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnRendererSettings, "Renderer Options");
            this.btnRendererSettings.UseVisualStyleBackColor = true;
            this.btnRendererSettings.Click += new System.EventHandler(this.OnRendererSettingsClick);
            // 
            // btnEditorSettings
            // 
            this.btnEditorSettings.Enabled = false;
            this.btnEditorSettings.FlatAppearance.BorderSize = 0;
            this.btnEditorSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditorSettings.Image = global::Chem4Word.Properties.Resources.Preferences;
            this.btnEditorSettings.Location = new System.Drawing.Point(510, 15);
            this.btnEditorSettings.Name = "btnEditorSettings";
            this.btnEditorSettings.Size = new System.Drawing.Size(36, 36);
            this.btnEditorSettings.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnEditorSettings, "Editor Options");
            this.btnEditorSettings.UseVisualStyleBackColor = true;
            this.btnEditorSettings.Click += new System.EventHandler(this.OnEditorSettingsClick);
            // 
            // lblSearcherDescription
            // 
            this.lblSearcherDescription.Location = new System.Drawing.Point(9, 53);
            this.lblSearcherDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearcherDescription.Name = "lblSearcherDescription";
            this.lblSearcherDescription.Size = new System.Drawing.Size(495, 42);
            this.lblSearcherDescription.TabIndex = 24;
            this.lblSearcherDescription.Text = "...";
            // 
            // cboSearchers
            // 
            this.cboSearchers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchers.FormattingEnabled = true;
            this.cboSearchers.Location = new System.Drawing.Point(9, 22);
            this.cboSearchers.Name = "cboSearchers";
            this.cboSearchers.Size = new System.Drawing.Size(495, 24);
            this.cboSearchers.TabIndex = 21;
            this.cboSearchers.SelectedIndexChanged += new System.EventHandler(this.OnSearchersSelectedIndexChanged);
            // 
            // lblRendererDescription
            // 
            this.lblRendererDescription.Location = new System.Drawing.Point(6, 53);
            this.lblRendererDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRendererDescription.Name = "lblRendererDescription";
            this.lblRendererDescription.Size = new System.Drawing.Size(498, 42);
            this.lblRendererDescription.TabIndex = 20;
            this.lblRendererDescription.Text = "...";
            // 
            // lblEditorDescription
            // 
            this.lblEditorDescription.Location = new System.Drawing.Point(6, 53);
            this.lblEditorDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEditorDescription.Name = "lblEditorDescription";
            this.lblEditorDescription.Size = new System.Drawing.Size(498, 42);
            this.lblEditorDescription.TabIndex = 19;
            this.lblEditorDescription.Text = "...";
            // 
            // cboRenderers
            // 
            this.cboRenderers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRenderers.FormattingEnabled = true;
            this.cboRenderers.Location = new System.Drawing.Point(6, 22);
            this.cboRenderers.Name = "cboRenderers";
            this.cboRenderers.Size = new System.Drawing.Size(498, 24);
            this.cboRenderers.TabIndex = 9;
            this.cboRenderers.SelectedIndexChanged += new System.EventHandler(this.OnRenderersSelectedIndexChanged);
            // 
            // cboEditors
            // 
            this.cboEditors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditors.FormattingEnabled = true;
            this.cboEditors.Location = new System.Drawing.Point(6, 22);
            this.cboEditors.Name = "cboEditors";
            this.cboEditors.Size = new System.Drawing.Size(498, 24);
            this.cboEditors.TabIndex = 6;
            this.cboEditors.SelectedIndexChanged += new System.EventHandler(this.OnEditorsSelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 50);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "ChemSpider Rdf Service Uri";
            // 
            // txtChemSpiderRdfUri
            // 
            this.txtChemSpiderRdfUri.Location = new System.Drawing.Point(206, 47);
            this.txtChemSpiderRdfUri.Margin = new System.Windows.Forms.Padding(4);
            this.txtChemSpiderRdfUri.Name = "txtChemSpiderRdfUri";
            this.txtChemSpiderRdfUri.Size = new System.Drawing.Size(290, 23);
            this.txtChemSpiderRdfUri.TabIndex = 25;
            this.txtChemSpiderRdfUri.Text = "https://rdf.chemspider.com/";
            this.txtChemSpiderRdfUri.TextChanged += new System.EventHandler(this.txtChemSpiderRdfUri_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "ChemSpider Web Service Uri";
            // 
            // txtChemSpiderWsUri
            // 
            this.txtChemSpiderWsUri.Location = new System.Drawing.Point(206, 14);
            this.txtChemSpiderWsUri.Margin = new System.Windows.Forms.Padding(4);
            this.txtChemSpiderWsUri.Name = "txtChemSpiderWsUri";
            this.txtChemSpiderWsUri.Size = new System.Drawing.Size(290, 23);
            this.txtChemSpiderWsUri.TabIndex = 17;
            this.txtChemSpiderWsUri.Text = "https://www.chemspider.com/";
            this.txtChemSpiderWsUri.TextChanged += new System.EventHandler(this.txtChemSpiderWsUri_TextChanged);
            // 
            // chkUseWebServices
            // 
            this.chkUseWebServices.AutoSize = true;
            this.chkUseWebServices.Checked = true;
            this.chkUseWebServices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseWebServices.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseWebServices.ForeColor = System.Drawing.Color.Red;
            this.chkUseWebServices.Location = new System.Drawing.Point(15, 84);
            this.chkUseWebServices.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseWebServices.Name = "chkUseWebServices";
            this.chkUseWebServices.Size = new System.Drawing.Size(154, 20);
            this.chkUseWebServices.TabIndex = 16;
            this.chkUseWebServices.Text = "Use Web Services *";
            this.chkUseWebServices.UseVisualStyleBackColor = true;
            this.chkUseWebServices.CheckedChanged += new System.EventHandler(this.chkUseWebServices_CheckedChanged);
            // 
            // chkTelemetryEnabled
            // 
            this.chkTelemetryEnabled.AutoSize = true;
            this.chkTelemetryEnabled.Checked = true;
            this.chkTelemetryEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTelemetryEnabled.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTelemetryEnabled.ForeColor = System.Drawing.Color.Red;
            this.chkTelemetryEnabled.Location = new System.Drawing.Point(15, 16);
            this.chkTelemetryEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.chkTelemetryEnabled.Name = "chkTelemetryEnabled";
            this.chkTelemetryEnabled.Size = new System.Drawing.Size(158, 20);
            this.chkTelemetryEnabled.TabIndex = 2;
            this.chkTelemetryEnabled.Text = "Telemetry Enabled *";
            this.chkTelemetryEnabled.UseVisualStyleBackColor = true;
            this.chkTelemetryEnabled.CheckedChanged += new System.EventHandler(this.chkTelemetryEnabled_CheckedChanged);
            // 
            // cboUpdateFrequency
            // 
            this.cboUpdateFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUpdateFrequency.FormattingEnabled = true;
            this.cboUpdateFrequency.Items.AddRange(new object[] {
            "Daily",
            "Weekly"});
            this.cboUpdateFrequency.Location = new System.Drawing.Point(180, 48);
            this.cboUpdateFrequency.Margin = new System.Windows.Forms.Padding(4);
            this.cboUpdateFrequency.Name = "cboUpdateFrequency";
            this.cboUpdateFrequency.Size = new System.Drawing.Size(83, 24);
            this.cboUpdateFrequency.TabIndex = 14;
            this.cboUpdateFrequency.SelectedIndexChanged += new System.EventHandler(this.cboUpdateFrequency_SelectedIndexChanged);
            // 
            // chkAutomaticUpdates
            // 
            this.chkAutomaticUpdates.AutoSize = true;
            this.chkAutomaticUpdates.Checked = true;
            this.chkAutomaticUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutomaticUpdates.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticUpdates.ForeColor = System.Drawing.Color.Red;
            this.chkAutomaticUpdates.Location = new System.Drawing.Point(15, 16);
            this.chkAutomaticUpdates.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutomaticUpdates.Name = "chkAutomaticUpdates";
            this.chkAutomaticUpdates.Size = new System.Drawing.Size(197, 20);
            this.chkAutomaticUpdates.TabIndex = 13;
            this.chkAutomaticUpdates.Text = "Enable Automatic Updates";
            this.chkAutomaticUpdates.UseVisualStyleBackColor = true;
            this.chkAutomaticUpdates.CheckedChanged += new System.EventHandler(this.chkAutomaticUpdates_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(15, 51);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 16);
            this.label11.TabIndex = 12;
            this.label11.Text = "Update check frequency";
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEx1.Controls.Add(this.tabPlugIns);
            this.tabControlEx1.Controls.Add(this.tabWebServices);
            this.tabControlEx1.Controls.Add(this.tabTelemetry);
            this.tabControlEx1.Controls.Add(this.tabUpdates);
            this.tabControlEx1.Controls.Add(this.tabLibrary);
            this.tabControlEx1.Controls.Add(this.tabMaintenance);
            this.tabControlEx1.Location = new System.Drawing.Point(12, 12);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(612, 370);
            this.tabControlEx1.TabIndex = 11;
            // 
            // tabPlugIns
            // 
            this.tabPlugIns.BackColor = System.Drawing.SystemColors.Control;
            this.tabPlugIns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPlugIns.Controls.Add(this.groupBox2);
            this.tabPlugIns.Controls.Add(this.RendererGroup);
            this.tabPlugIns.Controls.Add(this.EditorGroup);
            this.tabPlugIns.Location = new System.Drawing.Point(0, 23);
            this.tabPlugIns.Name = "tabPlugIns";
            this.tabPlugIns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlugIns.Size = new System.Drawing.Size(612, 347);
            this.tabPlugIns.TabIndex = 0;
            this.tabPlugIns.Text = "Plug Ins";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboSearchers);
            this.groupBox2.Controls.Add(this.btnSearcherSettings);
            this.groupBox2.Controls.Add(this.lblSearcherDescription);
            this.groupBox2.Location = new System.Drawing.Point(15, 222);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(553, 102);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Searcher";
            // 
            // RendererGroup
            // 
            this.RendererGroup.Controls.Add(this.cboRenderers);
            this.RendererGroup.Controls.Add(this.btnRendererSettings);
            this.RendererGroup.Controls.Add(this.lblRendererDescription);
            this.RendererGroup.Location = new System.Drawing.Point(15, 114);
            this.RendererGroup.Name = "RendererGroup";
            this.RendererGroup.Size = new System.Drawing.Size(553, 102);
            this.RendererGroup.TabIndex = 26;
            this.RendererGroup.TabStop = false;
            this.RendererGroup.Text = "Renderer";
            // 
            // EditorGroup
            // 
            this.EditorGroup.Controls.Add(this.cboEditors);
            this.EditorGroup.Controls.Add(this.lblEditorDescription);
            this.EditorGroup.Controls.Add(this.btnEditorSettings);
            this.EditorGroup.Location = new System.Drawing.Point(15, 6);
            this.EditorGroup.Name = "EditorGroup";
            this.EditorGroup.Size = new System.Drawing.Size(553, 102);
            this.EditorGroup.TabIndex = 25;
            this.EditorGroup.TabStop = false;
            this.EditorGroup.Text = "Editor";
            // 
            // tabWebServices
            // 
            this.tabWebServices.BackColor = System.Drawing.SystemColors.Control;
            this.tabWebServices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabWebServices.Controls.Add(this.lblProWebServices);
            this.tabWebServices.Controls.Add(this.label2);
            this.tabWebServices.Controls.Add(this.chkUseWebServices);
            this.tabWebServices.Controls.Add(this.label6);
            this.tabWebServices.Controls.Add(this.txtChemSpiderWsUri);
            this.tabWebServices.Controls.Add(this.txtChemSpiderRdfUri);
            this.tabWebServices.Location = new System.Drawing.Point(0, 23);
            this.tabWebServices.Name = "tabWebServices";
            this.tabWebServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabWebServices.Size = new System.Drawing.Size(612, 347);
            this.tabWebServices.TabIndex = 1;
            this.tabWebServices.Text = "Web Services";
            // 
            // lblProWebServices
            // 
            this.lblProWebServices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProWebServices.AutoSize = true;
            this.lblProWebServices.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProWebServices.ForeColor = System.Drawing.Color.Red;
            this.lblProWebServices.Location = new System.Drawing.Point(15, 316);
            this.lblProWebServices.Name = "lblProWebServices";
            this.lblProWebServices.Size = new System.Drawing.Size(231, 19);
            this.lblProWebServices.TabIndex = 27;
            this.lblProWebServices.Text = "* Professional Version Only";
            // 
            // tabTelemetry
            // 
            this.tabTelemetry.BackColor = System.Drawing.SystemColors.Control;
            this.tabTelemetry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabTelemetry.Controls.Add(this.lblProTelemetry);
            this.tabTelemetry.Controls.Add(this.chkTelemetryEnabled);
            this.tabTelemetry.Location = new System.Drawing.Point(0, 23);
            this.tabTelemetry.Name = "tabTelemetry";
            this.tabTelemetry.Size = new System.Drawing.Size(612, 347);
            this.tabTelemetry.TabIndex = 2;
            this.tabTelemetry.Text = "Telemetry";
            // 
            // lblProTelemetry
            // 
            this.lblProTelemetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProTelemetry.AutoSize = true;
            this.lblProTelemetry.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProTelemetry.ForeColor = System.Drawing.Color.Red;
            this.lblProTelemetry.Location = new System.Drawing.Point(15, 316);
            this.lblProTelemetry.Name = "lblProTelemetry";
            this.lblProTelemetry.Size = new System.Drawing.Size(231, 19);
            this.lblProTelemetry.TabIndex = 3;
            this.lblProTelemetry.Text = "* Professional Version Only";
            // 
            // tabUpdates
            // 
            this.tabUpdates.BackColor = System.Drawing.SystemColors.Control;
            this.tabUpdates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabUpdates.Controls.Add(this.lblProUpdates);
            this.tabUpdates.Controls.Add(this.cboUpdateFrequency);
            this.tabUpdates.Controls.Add(this.label11);
            this.tabUpdates.Controls.Add(this.chkAutomaticUpdates);
            this.tabUpdates.Location = new System.Drawing.Point(0, 23);
            this.tabUpdates.Name = "tabUpdates";
            this.tabUpdates.Size = new System.Drawing.Size(612, 347);
            this.tabUpdates.TabIndex = 3;
            this.tabUpdates.Text = "Updates";
            // 
            // lblProUpdates
            // 
            this.lblProUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProUpdates.AutoSize = true;
            this.lblProUpdates.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProUpdates.ForeColor = System.Drawing.Color.Red;
            this.lblProUpdates.Location = new System.Drawing.Point(15, 316);
            this.lblProUpdates.Name = "lblProUpdates";
            this.lblProUpdates.Size = new System.Drawing.Size(231, 19);
            this.lblProUpdates.TabIndex = 15;
            this.lblProUpdates.Text = "* Professional Version Only";
            // 
            // tabLibrary
            // 
            this.tabLibrary.BackColor = System.Drawing.SystemColors.Control;
            this.tabLibrary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabLibrary.Controls.Add(this.ClearLibraryButton);
            this.tabLibrary.Controls.Add(this.groupBox1);
            this.tabLibrary.Location = new System.Drawing.Point(0, 23);
            this.tabLibrary.Name = "tabLibrary";
            this.tabLibrary.Padding = new System.Windows.Forms.Padding(3);
            this.tabLibrary.Size = new System.Drawing.Size(612, 347);
            this.tabLibrary.TabIndex = 4;
            this.tabLibrary.Text = "Library";
            // 
            // ClearLibraryButton
            // 
            this.ClearLibraryButton.Image = global::Chem4Word.Properties.Resources.Gallery_Delete;
            this.ClearLibraryButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearLibraryButton.Location = new System.Drawing.Point(25, 153);
            this.ClearLibraryButton.Name = "ClearLibraryButton";
            this.ClearLibraryButton.Size = new System.Drawing.Size(190, 48);
            this.ClearLibraryButton.TabIndex = 2;
            this.ClearLibraryButton.Text = "Clear the Library !";
            this.ClearLibraryButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClearLibraryButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ClearLibraryButton.UseVisualStyleBackColor = true;
            this.ClearLibraryButton.Click += new System.EventHandler(this.OnClearLibraryClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblGalleryDesc);
            this.groupBox1.Controls.Add(this.importGalleryButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import";
            // 
            // lblGalleryDesc
            // 
            this.lblGalleryDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGalleryDesc.Location = new System.Drawing.Point(7, 23);
            this.lblGalleryDesc.Name = "lblGalleryDesc";
            this.lblGalleryDesc.Size = new System.Drawing.Size(567, 51);
            this.lblGalleryDesc.TabIndex = 0;
            this.lblGalleryDesc.Text = "The Library replaces the Gallery from previous versions of Chem4Word.\r\nYou can im" +
    "port structures from your old Gallery (or any other folder of cml files) into th" +
    "e Library.  ";
            // 
            // importGalleryButton
            // 
            this.importGalleryButton.Image = global::Chem4Word.Properties.Resources.Gallery_Toggle;
            this.importGalleryButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importGalleryButton.Location = new System.Drawing.Point(10, 73);
            this.importGalleryButton.Name = "importGalleryButton";
            this.importGalleryButton.Size = new System.Drawing.Size(190, 48);
            this.importGalleryButton.TabIndex = 0;
            this.importGalleryButton.Text = "Import into Library ...";
            this.importGalleryButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.importGalleryButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.importGalleryButton.UseVisualStyleBackColor = true;
            this.importGalleryButton.Click += new System.EventHandler(this.OnLibraryImportClick);
            // 
            // tabMaintenance
            // 
            this.tabMaintenance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabMaintenance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabMaintenance.Controls.Add(this.label4);
            this.tabMaintenance.Controls.Add(this.label3);
            this.tabMaintenance.Controls.Add(this.label1);
            this.tabMaintenance.Controls.Add(this.MaintenanceInformation);
            this.tabMaintenance.Controls.Add(this.OpenPlugInFolder);
            this.tabMaintenance.Controls.Add(this.OpenLibraryFolder);
            this.tabMaintenance.Controls.Add(this.OpenSettingsFolder);
            this.tabMaintenance.Location = new System.Drawing.Point(0, 23);
            this.tabMaintenance.Name = "tabMaintenance";
            this.tabMaintenance.Padding = new System.Windows.Forms.Padding(3);
            this.tabMaintenance.Size = new System.Drawing.Size(612, 347);
            this.tabMaintenance.TabIndex = 5;
            this.tabMaintenance.Text = "Maintenance";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(197, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(369, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "This folder is where the Chem4Word Plug-Ins are installed.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(197, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(369, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "This folder is where Chem4Word stores the Library.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(197, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "This folder is where Chem4Word stores its settings.";
            // 
            // MaintenanceInformation
            // 
            this.MaintenanceInformation.Location = new System.Drawing.Point(15, 15);
            this.MaintenanceInformation.Name = "MaintenanceInformation";
            this.MaintenanceInformation.Size = new System.Drawing.Size(551, 26);
            this.MaintenanceInformation.TabIndex = 5;
            this.MaintenanceInformation.Text = "You should back up the following folders reguarly.";
            // 
            // OpenPlugInFolder
            // 
            this.OpenPlugInFolder.Image = global::Chem4Word.Properties.Resources.File_Open;
            this.OpenPlugInFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenPlugInFolder.Location = new System.Drawing.Point(15, 149);
            this.OpenPlugInFolder.Name = "OpenPlugInFolder";
            this.OpenPlugInFolder.Size = new System.Drawing.Size(176, 48);
            this.OpenPlugInFolder.TabIndex = 4;
            this.OpenPlugInFolder.Text = "Open Plug-Ins Folder";
            this.OpenPlugInFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OpenPlugInFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenPlugInFolder.UseVisualStyleBackColor = true;
            this.OpenPlugInFolder.Click += new System.EventHandler(this.OnOpenPlugInFolderClick);
            // 
            // OpenLibraryFolder
            // 
            this.OpenLibraryFolder.Image = global::Chem4Word.Properties.Resources.File_Open;
            this.OpenLibraryFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenLibraryFolder.Location = new System.Drawing.Point(15, 95);
            this.OpenLibraryFolder.Name = "OpenLibraryFolder";
            this.OpenLibraryFolder.Size = new System.Drawing.Size(176, 48);
            this.OpenLibraryFolder.TabIndex = 2;
            this.OpenLibraryFolder.Text = "Open Library Folder";
            this.OpenLibraryFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OpenLibraryFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenLibraryFolder.UseVisualStyleBackColor = true;
            this.OpenLibraryFolder.Click += new System.EventHandler(this.OnOpenLibraryFolderClick);
            // 
            // OpenSettingsFolder
            // 
            this.OpenSettingsFolder.Image = global::Chem4Word.Properties.Resources.File_Open;
            this.OpenSettingsFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenSettingsFolder.Location = new System.Drawing.Point(15, 41);
            this.OpenSettingsFolder.Name = "OpenSettingsFolder";
            this.OpenSettingsFolder.Size = new System.Drawing.Size(176, 48);
            this.OpenSettingsFolder.TabIndex = 0;
            this.OpenSettingsFolder.Text = "Open Settings Folder";
            this.OpenSettingsFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OpenSettingsFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OpenSettingsFolder.UseVisualStyleBackColor = true;
            this.OpenSettingsFolder.Click += new System.EventHandler(this.OnOpenSettingsFolderClick);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 432);
            this.Controls.Add(this.tabControlEx1);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "User Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPlugIns.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.RendererGroup.ResumeLayout(false);
            this.EditorGroup.ResumeLayout(false);
            this.tabWebServices.ResumeLayout(false);
            this.tabWebServices.PerformLayout();
            this.tabTelemetry.ResumeLayout(false);
            this.tabTelemetry.PerformLayout();
            this.tabUpdates.ResumeLayout(false);
            this.tabUpdates.PerformLayout();
            this.tabLibrary.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabMaintenance.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnSetDefaults;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtChemSpiderRdfUri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChemSpiderWsUri;
        private System.Windows.Forms.CheckBox chkUseWebServices;
        private System.Windows.Forms.CheckBox chkTelemetryEnabled;
        private System.Windows.Forms.Button btnRendererSettings;
        private System.Windows.Forms.ComboBox cboRenderers;
        private System.Windows.Forms.Button btnEditorSettings;
        private System.Windows.Forms.ComboBox cboEditors;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cboUpdateFrequency;
        private System.Windows.Forms.CheckBox chkAutomaticUpdates;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblEditorDescription;
        private System.Windows.Forms.Label lblRendererDescription;
        private System.Windows.Forms.Label lblSearcherDescription;
        private System.Windows.Forms.Button btnSearcherSettings;
        private System.Windows.Forms.ComboBox cboSearchers;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPlugIns;
        private System.Windows.Forms.TabPage tabWebServices;
        private System.Windows.Forms.TabPage tabTelemetry;
        private System.Windows.Forms.TabPage tabUpdates;
        private System.Windows.Forms.Label lblProWebServices;
        private System.Windows.Forms.Label lblProTelemetry;
        private System.Windows.Forms.Label lblProUpdates;
        private System.Windows.Forms.TabPage tabLibrary;
        private System.Windows.Forms.Button importGalleryButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblGalleryDesc;
        private System.Windows.Forms.Button ClearLibraryButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox RendererGroup;
        private System.Windows.Forms.GroupBox EditorGroup;
        private System.Windows.Forms.TabPage tabMaintenance;
        private System.Windows.Forms.Button OpenSettingsFolder;
        private System.Windows.Forms.Button OpenPlugInFolder;
        private System.Windows.Forms.Button OpenLibraryFolder;
        private System.Windows.Forms.Label MaintenanceInformation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}