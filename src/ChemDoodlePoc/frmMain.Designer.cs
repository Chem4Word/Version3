namespace ChemDoodlePoc
{
    partial class frmMain
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnConvertModel = new System.Windows.Forms.Button();
            this.btnMirror = new System.Windows.Forms.Button();
            this.btnFlip = new System.Windows.Forms.Button();
            this.chkShowColouredAtoms = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radSketcher = new System.Windows.Forms.RadioButton();
            this.radSingle = new System.Windows.Forms.RadioButton();
            this.btnAddExpliciyHydrogens = new System.Windows.Forms.Button();
            this.chkShowHydrogenCount = new System.Windows.Forms.CheckBox();
            this.btnFormula = new System.Windows.Forms.Button();
            this.btnRemoveH = new System.Windows.Forms.Button();
            this.nudBondLength = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.cboGetAs = new System.Windows.Forms.ComboBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtStructure = new System.Windows.Forms.TextBox();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBondLength)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.btnConvertModel);
            this.splitContainer1.Panel1.Controls.Add(this.btnMirror);
            this.splitContainer1.Panel1.Controls.Add(this.btnFlip);
            this.splitContainer1.Panel1.Controls.Add(this.chkShowColouredAtoms);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddExpliciyHydrogens);
            this.splitContainer1.Panel1.Controls.Add(this.chkShowHydrogenCount);
            this.splitContainer1.Panel1.Controls.Add(this.btnFormula);
            this.splitContainer1.Panel1.Controls.Add(this.btnRemoveH);
            this.splitContainer1.Panel1.Controls.Add(this.nudBondLength);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnOpen);
            this.splitContainer1.Panel1.Controls.Add(this.cboGetAs);
            this.splitContainer1.Panel1.Controls.Add(this.btnGet);
            this.splitContainer1.Panel1.Controls.Add(this.btnSend);
            this.splitContainer1.Panel1.Controls.Add(this.txtStructure);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.browser);
            this.splitContainer1.Size = new System.Drawing.Size(1004, 477);
            this.splitContainer1.SplitterDistance = 468;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(5, 88);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(62, 23);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnConvertModel
            // 
            this.btnConvertModel.Location = new System.Drawing.Point(5, 59);
            this.btnConvertModel.Name = "btnConvertModel";
            this.btnConvertModel.Size = new System.Drawing.Size(207, 23);
            this.btnConvertModel.TabIndex = 25;
            this.btnConvertModel.Text = "? -> CML -> JSON";
            this.btnConvertModel.UseVisualStyleBackColor = true;
            this.btnConvertModel.Click += new System.EventHandler(this.btnConvertModel_Click);
            // 
            // btnMirror
            // 
            this.btnMirror.Location = new System.Drawing.Point(5, 30);
            this.btnMirror.Name = "btnMirror";
            this.btnMirror.Size = new System.Drawing.Size(75, 23);
            this.btnMirror.TabIndex = 24;
            this.btnMirror.Text = "Mirror";
            this.btnMirror.UseVisualStyleBackColor = true;
            this.btnMirror.Click += new System.EventHandler(this.btnMirror_Click);
            // 
            // btnFlip
            // 
            this.btnFlip.Location = new System.Drawing.Point(86, 30);
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size(75, 23);
            this.btnFlip.TabIndex = 23;
            this.btnFlip.Text = "Flip";
            this.btnFlip.UseVisualStyleBackColor = true;
            this.btnFlip.Click += new System.EventHandler(this.btnFlip_Click);
            // 
            // chkShowColouredAtoms
            // 
            this.chkShowColouredAtoms.AutoSize = true;
            this.chkShowColouredAtoms.Checked = true;
            this.chkShowColouredAtoms.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowColouredAtoms.Location = new System.Drawing.Point(288, 12);
            this.chkShowColouredAtoms.Name = "chkShowColouredAtoms";
            this.chkShowColouredAtoms.Size = new System.Drawing.Size(56, 17);
            this.chkShowColouredAtoms.TabIndex = 22;
            this.chkShowColouredAtoms.Text = "Colour";
            this.chkShowColouredAtoms.UseVisualStyleBackColor = true;
            this.chkShowColouredAtoms.CheckedChanged += new System.EventHandler(this.chkShowColouredAtoms_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radSketcher);
            this.groupBox1.Controls.Add(this.radSingle);
            this.groupBox1.Location = new System.Drawing.Point(363, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(91, 70);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // radSketcher
            // 
            this.radSketcher.AutoSize = true;
            this.radSketcher.Location = new System.Drawing.Point(9, 41);
            this.radSketcher.Name = "radSketcher";
            this.radSketcher.Size = new System.Drawing.Size(68, 17);
            this.radSketcher.TabIndex = 1;
            this.radSketcher.Text = "Sketcher";
            this.radSketcher.UseVisualStyleBackColor = true;
            this.radSketcher.CheckedChanged += new System.EventHandler(this.radSketcher_CheckedChanged);
            // 
            // radSingle
            // 
            this.radSingle.AutoSize = true;
            this.radSingle.Checked = true;
            this.radSingle.Location = new System.Drawing.Point(8, 19);
            this.radSingle.Name = "radSingle";
            this.radSingle.Size = new System.Drawing.Size(74, 17);
            this.radSingle.TabIndex = 0;
            this.radSingle.TabStop = true;
            this.radSingle.Text = "Single Mol";
            this.radSingle.UseVisualStyleBackColor = true;
            this.radSingle.CheckedChanged += new System.EventHandler(this.radSingle_CheckedChanged);
            // 
            // btnAddExpliciyHydrogens
            // 
            this.btnAddExpliciyHydrogens.Location = new System.Drawing.Point(167, 3);
            this.btnAddExpliciyHydrogens.Name = "btnAddExpliciyHydrogens";
            this.btnAddExpliciyHydrogens.Size = new System.Drawing.Size(44, 23);
            this.btnAddExpliciyHydrogens.TabIndex = 20;
            this.btnAddExpliciyHydrogens.Text = "+ H";
            this.btnAddExpliciyHydrogens.UseVisualStyleBackColor = true;
            this.btnAddExpliciyHydrogens.Click += new System.EventHandler(this.btnAddExpliciyHydrogens_Click);
            // 
            // chkShowHydrogenCount
            // 
            this.chkShowHydrogenCount.AutoSize = true;
            this.chkShowHydrogenCount.Checked = true;
            this.chkShowHydrogenCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowHydrogenCount.Location = new System.Drawing.Point(288, 36);
            this.chkShowHydrogenCount.Name = "chkShowHydrogenCount";
            this.chkShowHydrogenCount.Size = new System.Drawing.Size(65, 17);
            this.chkShowHydrogenCount.TabIndex = 19;
            this.chkShowHydrogenCount.Text = "H Count";
            this.chkShowHydrogenCount.UseVisualStyleBackColor = true;
            this.chkShowHydrogenCount.CheckedChanged += new System.EventHandler(this.chkShowHydrogenCount_CheckedChanged);
            // 
            // btnFormula
            // 
            this.btnFormula.Location = new System.Drawing.Point(175, 90);
            this.btnFormula.Name = "btnFormula";
            this.btnFormula.Size = new System.Drawing.Size(62, 23);
            this.btnFormula.TabIndex = 18;
            this.btnFormula.Text = "Formula";
            this.btnFormula.UseVisualStyleBackColor = true;
            this.btnFormula.Click += new System.EventHandler(this.btnFormula_Click);
            // 
            // btnRemoveH
            // 
            this.btnRemoveH.Location = new System.Drawing.Point(168, 30);
            this.btnRemoveH.Name = "btnRemoveH";
            this.btnRemoveH.Size = new System.Drawing.Size(44, 23);
            this.btnRemoveH.TabIndex = 15;
            this.btnRemoveH.Text = "- H";
            this.btnRemoveH.UseVisualStyleBackColor = true;
            this.btnRemoveH.Click += new System.EventHandler(this.btnRemoveH_Click);
            // 
            // nudBondLength
            // 
            this.nudBondLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudBondLength.Location = new System.Drawing.Point(288, 59);
            this.nudBondLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBondLength.Name = "nudBondLength";
            this.nudBondLength.Size = new System.Drawing.Size(56, 20);
            this.nudBondLength.TabIndex = 13;
            this.nudBondLength.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudBondLength.ValueChanged += new System.EventHandler(this.nudBondLength_ValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(86, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(5, 3);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 10;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // cboGetAs
            // 
            this.cboGetAs.FormattingEnabled = true;
            this.cboGetAs.Location = new System.Drawing.Point(387, 90);
            this.cboGetAs.Name = "cboGetAs";
            this.cboGetAs.Size = new System.Drawing.Size(66, 21);
            this.cboGetAs.TabIndex = 9;
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(243, 90);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(66, 23);
            this.btnGet.TabIndex = 3;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(315, 90);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(66, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtStructure
            // 
            this.txtStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStructure.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStructure.Location = new System.Drawing.Point(12, 119);
            this.txtStructure.Multiline = true;
            this.txtStructure.Name = "txtStructure";
            this.txtStructure.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStructure.Size = new System.Drawing.Size(442, 347);
            this.txtStructure.TabIndex = 0;
            this.txtStructure.WordWrap = false;
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(532, 477);
            this.browser.TabIndex = 0;
            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.browser_DocumentCompleted);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFileDialog1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 477);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "Set at runtime ...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBondLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtStructure;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.ComboBox cboGetAs;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.NumericUpDown nudBondLength;
        private System.Windows.Forms.Button btnRemoveH;
        private System.Windows.Forms.Button btnFormula;
        private System.Windows.Forms.CheckBox chkShowHydrogenCount;
        private System.Windows.Forms.Button btnAddExpliciyHydrogens;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radSketcher;
        private System.Windows.Forms.RadioButton radSingle;
        private System.Windows.Forms.CheckBox chkShowColouredAtoms;
        private System.Windows.Forms.Button btnMirror;
        private System.Windows.Forms.Button btnFlip;
        private System.Windows.Forms.Button btnConvertModel;
        private System.Windows.Forms.Button btnClear;
    }
}

