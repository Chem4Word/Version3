using Chem4Word.Core.UI.Controls;

namespace Chem4Word.Renderer.OoXmlV3
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
            this.tabRendering = new System.Windows.Forms.TabPage();
            this.chkShowCarbons = new System.Windows.Forms.CheckBox();
            this.chkShowHydrogens = new System.Windows.Forms.CheckBox();
            this.chkColouredAtoms = new System.Windows.Forms.CheckBox();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.chkShowAtomPositions = new System.Windows.Forms.CheckBox();
            this.chkShowRingCentres = new System.Windows.Forms.CheckBox();
            this.chkShowCharacterBox = new System.Windows.Forms.CheckBox();
            this.chkShowMoleculeBox = new System.Windows.Forms.CheckBox();
            this.chkClipLines = new System.Windows.Forms.CheckBox();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabControlEx.SuspendLayout();
            this.tabRendering.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEx
            // 
            this.tabControlEx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEx.Controls.Add(this.tabRendering);
            this.tabControlEx.Controls.Add(this.tabDebug);
            this.tabControlEx.Location = new System.Drawing.Point(13, 13);
            this.tabControlEx.Name = "tabControlEx";
            this.tabControlEx.SelectedIndex = 0;
            this.tabControlEx.Size = new System.Drawing.Size(428, 183);
            this.tabControlEx.TabIndex = 0;
            // 
            // tabRendering
            // 
            this.tabRendering.BackColor = System.Drawing.SystemColors.Control;
            this.tabRendering.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabRendering.Controls.Add(this.chkShowCarbons);
            this.tabRendering.Controls.Add(this.chkShowHydrogens);
            this.tabRendering.Controls.Add(this.chkColouredAtoms);
            this.tabRendering.Location = new System.Drawing.Point(0, 20);
            this.tabRendering.Name = "tabRendering";
            this.tabRendering.Padding = new System.Windows.Forms.Padding(3);
            this.tabRendering.Size = new System.Drawing.Size(428, 163);
            this.tabRendering.TabIndex = 0;
            this.tabRendering.Text = "Rendering";
            // 
            // chkShowCarbons
            // 
            this.chkShowCarbons.AutoSize = true;
            this.chkShowCarbons.Checked = true;
            this.chkShowCarbons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCarbons.Location = new System.Drawing.Point(12, 82);
            this.chkShowCarbons.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowCarbons.Name = "chkShowCarbons";
            this.chkShowCarbons.Size = new System.Drawing.Size(165, 17);
            this.chkShowCarbons.TabIndex = 9;
            this.chkShowCarbons.Text = "Show All Carbon Atom Labels";
            this.chkShowCarbons.UseVisualStyleBackColor = true;
            this.chkShowCarbons.CheckedChanged += new System.EventHandler(this.chkShowCarbonLabels_CheckedChanged);
            // 
            // chkShowHydrogens
            // 
            this.chkShowHydrogens.AutoSize = true;
            this.chkShowHydrogens.Checked = true;
            this.chkShowHydrogens.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowHydrogens.Location = new System.Drawing.Point(12, 12);
            this.chkShowHydrogens.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowHydrogens.Name = "chkShowHydrogens";
            this.chkShowHydrogens.Size = new System.Drawing.Size(142, 17);
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
            this.chkColouredAtoms.Location = new System.Drawing.Point(12, 47);
            this.chkColouredAtoms.Margin = new System.Windows.Forms.Padding(4);
            this.chkColouredAtoms.Name = "chkColouredAtoms";
            this.chkColouredAtoms.Size = new System.Drawing.Size(158, 17);
            this.chkColouredAtoms.TabIndex = 8;
            this.chkColouredAtoms.Text = "Show Atom Labels in Colour";
            this.chkColouredAtoms.UseVisualStyleBackColor = true;
            this.chkColouredAtoms.CheckedChanged += new System.EventHandler(this.chkColouredAtoms_CheckedChanged);
            // 
            // tabDebug
            // 
            this.tabDebug.BackColor = System.Drawing.SystemColors.Control;
            this.tabDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabDebug.Controls.Add(this.chkShowAtomPositions);
            this.tabDebug.Controls.Add(this.chkShowRingCentres);
            this.tabDebug.Controls.Add(this.chkShowCharacterBox);
            this.tabDebug.Controls.Add(this.chkShowMoleculeBox);
            this.tabDebug.Controls.Add(this.chkClipLines);
            this.tabDebug.Location = new System.Drawing.Point(0, 20);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(428, 163);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            // 
            // chkShowAtomPositions
            // 
            this.chkShowAtomPositions.AutoSize = true;
            this.chkShowAtomPositions.Checked = true;
            this.chkShowAtomPositions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAtomPositions.Location = new System.Drawing.Point(12, 37);
            this.chkShowAtomPositions.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowAtomPositions.Name = "chkShowAtomPositions";
            this.chkShowAtomPositions.Size = new System.Drawing.Size(125, 17);
            this.chkShowAtomPositions.TabIndex = 21;
            this.chkShowAtomPositions.Text = "Show Atom Positions";
            this.chkShowAtomPositions.UseVisualStyleBackColor = true;
            this.chkShowAtomPositions.CheckedChanged += new System.EventHandler(this.chkShowAtomCentres_CheckedChanged);
            // 
            // chkShowRingCentres
            // 
            this.chkShowRingCentres.AutoSize = true;
            this.chkShowRingCentres.Checked = true;
            this.chkShowRingCentres.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowRingCentres.Location = new System.Drawing.Point(12, 62);
            this.chkShowRingCentres.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowRingCentres.Name = "chkShowRingCentres";
            this.chkShowRingCentres.Size = new System.Drawing.Size(169, 17);
            this.chkShowRingCentres.TabIndex = 20;
            this.chkShowRingCentres.Text = "Show Centre of detected rings";
            this.chkShowRingCentres.UseVisualStyleBackColor = true;
            this.chkShowRingCentres.CheckedChanged += new System.EventHandler(this.chkShowRingCentres_CheckedChanged);
            // 
            // chkShowCharacterBox
            // 
            this.chkShowCharacterBox.AutoSize = true;
            this.chkShowCharacterBox.Checked = true;
            this.chkShowCharacterBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCharacterBox.Location = new System.Drawing.Point(12, 112);
            this.chkShowCharacterBox.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowCharacterBox.Name = "chkShowCharacterBox";
            this.chkShowCharacterBox.Size = new System.Drawing.Size(188, 17);
            this.chkShowCharacterBox.TabIndex = 19;
            this.chkShowCharacterBox.Text = "Show Bounding Box of Characters";
            this.chkShowCharacterBox.UseVisualStyleBackColor = true;
            this.chkShowCharacterBox.CheckedChanged += new System.EventHandler(this.chkShowCharacterBox_CheckedChanged);
            // 
            // chkShowMoleculeBox
            // 
            this.chkShowMoleculeBox.AutoSize = true;
            this.chkShowMoleculeBox.Checked = true;
            this.chkShowMoleculeBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowMoleculeBox.Location = new System.Drawing.Point(12, 87);
            this.chkShowMoleculeBox.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowMoleculeBox.Name = "chkShowMoleculeBox";
            this.chkShowMoleculeBox.Size = new System.Drawing.Size(191, 17);
            this.chkShowMoleculeBox.TabIndex = 18;
            this.chkShowMoleculeBox.Text = "Show Bounding Box of Molecule(s)";
            this.chkShowMoleculeBox.UseVisualStyleBackColor = true;
            this.chkShowMoleculeBox.CheckedChanged += new System.EventHandler(this.chkShowMoleculeBox_CheckedChanged);
            // 
            // chkClipLines
            // 
            this.chkClipLines.AutoSize = true;
            this.chkClipLines.Checked = true;
            this.chkClipLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClipLines.Location = new System.Drawing.Point(12, 12);
            this.chkClipLines.Margin = new System.Windows.Forms.Padding(4);
            this.chkClipLines.Name = "chkClipLines";
            this.chkClipLines.Size = new System.Drawing.Size(67, 17);
            this.chkClipLines.TabIndex = 13;
            this.chkClipLines.Text = "Clip lines";
            this.chkClipLines.UseVisualStyleBackColor = true;
            this.chkClipLines.CheckedChanged += new System.EventHandler(this.chkClipLines_CheckedChanged);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaults.Location = new System.Drawing.Point(260, 203);
            this.btnSetDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(88, 28);
            this.btnSetDefaults.TabIndex = 13;
            this.btnSetDefaults.Text = "Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(354, 203);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 244);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControlEx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tabControlEx.ResumeLayout(false);
            this.tabRendering.ResumeLayout(false);
            this.tabRendering.PerformLayout();
            this.tabDebug.ResumeLayout(false);
            this.tabDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControlEx tabControlEx;
        private System.Windows.Forms.TabPage tabRendering;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.Button btnSetDefaults;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkShowHydrogens;
        private System.Windows.Forms.CheckBox chkColouredAtoms;
        private System.Windows.Forms.CheckBox chkClipLines;
        private System.Windows.Forms.CheckBox chkShowRingCentres;
        private System.Windows.Forms.CheckBox chkShowCharacterBox;
        private System.Windows.Forms.CheckBox chkShowMoleculeBox;
        private System.Windows.Forms.CheckBox chkShowAtomPositions;
        private System.Windows.Forms.CheckBox chkShowCarbons;
    }
}