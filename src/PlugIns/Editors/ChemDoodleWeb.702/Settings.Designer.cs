using Chem4Word.Core.UI.Controls;

namespace Chem4Word.Editor.ChemDoodleWeb702
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tabControlEx = new Chem4Word.Core.UI.Controls.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkShowHydrogens = new System.Windows.Forms.CheckBox();
            this.chkColouredAtoms = new System.Windows.Forms.CheckBox();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkShowCarbons = new System.Windows.Forms.CheckBox();
            this.tabControlEx.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEx
            // 
            this.tabControlEx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEx.Controls.Add(this.tabPage1);
            this.tabControlEx.Location = new System.Drawing.Point(17, 16);
            this.tabControlEx.Name = "tabControlEx";
            this.tabControlEx.SelectedIndex = 0;
            this.tabControlEx.Size = new System.Drawing.Size(399, 178);
            this.tabControlEx.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage1.Controls.Add(this.chkShowCarbons);
            this.tabPage1.Controls.Add(this.chkShowHydrogens);
            this.tabPage1.Controls.Add(this.chkColouredAtoms);
            this.tabPage1.Location = new System.Drawing.Point(0, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(399, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Display";
            // 
            // chkShowHydrogens
            // 
            this.chkShowHydrogens.AutoSize = true;
            this.chkShowHydrogens.Checked = true;
            this.chkShowHydrogens.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowHydrogens.Location = new System.Drawing.Point(16, 15);
            this.chkShowHydrogens.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkShowHydrogens.Name = "chkShowHydrogens";
            this.chkShowHydrogens.Size = new System.Drawing.Size(174, 20);
            this.chkShowHydrogens.TabIndex = 7;
            this.chkShowHydrogens.Text = "Show Implicit Hydrogens";
            this.chkShowHydrogens.UseVisualStyleBackColor = true;
            this.chkShowHydrogens.CheckedChanged += new System.EventHandler(this.chkShowHydrogens_CheckedChanged);
            // 
            // chkColouredAtoms
            // 
            this.chkColouredAtoms.AutoSize = true;
            this.chkColouredAtoms.Checked = true;
            this.chkColouredAtoms.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColouredAtoms.Location = new System.Drawing.Point(16, 61);
            this.chkColouredAtoms.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkColouredAtoms.Name = "chkColouredAtoms";
            this.chkColouredAtoms.Size = new System.Drawing.Size(193, 20);
            this.chkColouredAtoms.TabIndex = 8;
            this.chkColouredAtoms.Text = "Show Atom Labels in Colour";
            this.chkColouredAtoms.UseVisualStyleBackColor = true;
            this.chkColouredAtoms.CheckedChanged += new System.EventHandler(this.chkColouredAtoms_CheckedChanged);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaults.Location = new System.Drawing.Point(174, 203);
            this.btnSetDefaults.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(118, 34);
            this.btnSetDefaults.TabIndex = 11;
            this.btnSetDefaults.Text = "Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(298, 203);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(118, 34);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkShowCarbons
            // 
            this.chkShowCarbons.AutoSize = true;
            this.chkShowCarbons.Checked = true;
            this.chkShowCarbons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCarbons.Location = new System.Drawing.Point(16, 103);
            this.chkShowCarbons.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkShowCarbons.Name = "chkShowCarbons";
            this.chkShowCarbons.Size = new System.Drawing.Size(132, 20);
            this.chkShowCarbons.TabIndex = 9;
            this.chkShowCarbons.Text = "Show All Carbons";
            this.chkShowCarbons.UseVisualStyleBackColor = true;
            this.chkShowCarbons.CheckedChanged += new System.EventHandler(this.chkShowCarbons_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 254);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControlEx);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "ChemDoodle Web Editor - Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tabControlEx.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControlEx tabControlEx;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSetDefaults;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkShowHydrogens;
        private System.Windows.Forms.CheckBox chkColouredAtoms;
        private System.Windows.Forms.CheckBox chkShowCarbons;
    }
}