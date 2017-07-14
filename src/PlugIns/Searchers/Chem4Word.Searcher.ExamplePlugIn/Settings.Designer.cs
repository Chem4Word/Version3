using Chem4Word.Core.UI.Controls;

namespace Chem4Word.Searcher.ExamplePlugIn
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
            this.chkProperty1 = new System.Windows.Forms.CheckBox();
            this.chkProperty2 = new System.Windows.Forms.CheckBox();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
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
            this.tabControlEx.Location = new System.Drawing.Point(13, 13);
            this.tabControlEx.Name = "tabControlEx";
            this.tabControlEx.SelectedIndex = 0;
            this.tabControlEx.Size = new System.Drawing.Size(299, 145);
            this.tabControlEx.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage1.Controls.Add(this.chkProperty1);
            this.tabPage1.Controls.Add(this.chkProperty2);
            this.tabPage1.Location = new System.Drawing.Point(0, 20);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(299, 125);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Display";
            // 
            // chkProperty1
            // 
            this.chkProperty1.AutoSize = true;
            this.chkProperty1.Checked = true;
            this.chkProperty1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProperty1.Location = new System.Drawing.Point(12, 12);
            this.chkProperty1.Margin = new System.Windows.Forms.Padding(4);
            this.chkProperty1.Name = "chkProperty1";
            this.chkProperty1.Size = new System.Drawing.Size(71, 17);
            this.chkProperty1.TabIndex = 7;
            this.chkProperty1.Text = "Property1";
            this.chkProperty1.UseVisualStyleBackColor = true;
            this.chkProperty1.CheckedChanged += new System.EventHandler(this.chkProperty1_CheckedChanged);
            // 
            // chkProperty2
            // 
            this.chkProperty2.AutoSize = true;
            this.chkProperty2.Checked = true;
            this.chkProperty2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProperty2.Location = new System.Drawing.Point(12, 49);
            this.chkProperty2.Margin = new System.Windows.Forms.Padding(4);
            this.chkProperty2.Name = "chkProperty2";
            this.chkProperty2.Size = new System.Drawing.Size(71, 17);
            this.chkProperty2.TabIndex = 8;
            this.chkProperty2.Text = "Property2";
            this.chkProperty2.UseVisualStyleBackColor = true;
            this.chkProperty2.CheckedChanged += new System.EventHandler(this.chkProperty2_CheckedChanged);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaults.Location = new System.Drawing.Point(130, 165);
            this.btnSetDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(88, 28);
            this.btnSetDefaults.TabIndex = 11;
            this.btnSetDefaults.Text = "Defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(224, 165);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 206);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControlEx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Example - Settings";
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
        private System.Windows.Forms.CheckBox chkProperty1;
        private System.Windows.Forms.CheckBox chkProperty2;
    }
}