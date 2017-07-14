namespace Chem4Word.UI
{
    partial class EditLabels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditLabels));
            this.btnSave = new System.Windows.Forms.Button();
            this.rtbConcise = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlEx1 = new Chem4Word.Core.UI.Controls.TabControlEx();
            this.tabPage_m1 = new System.Windows.Forms.TabPage();
            this.ucMoleculeLabelEditor_m1 = new Chem4Word.UI.UserControls.UcMoleculeLabelEditor();
            this.tabControlEx1.SuspendLayout();
            this.tabPage_m1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(850, 470);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // rtbConcise
            // 
            this.rtbConcise.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbConcise.BackColor = System.Drawing.SystemColors.Window;
            this.rtbConcise.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbConcise.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConcise.Location = new System.Drawing.Point(14, 8);
            this.rtbConcise.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbConcise.Name = "rtbConcise";
            this.rtbConcise.ReadOnly = true;
            this.rtbConcise.Size = new System.Drawing.Size(922, 33);
            this.rtbConcise.TabIndex = 7;
            this.rtbConcise.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(10, 470);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(833, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "...";
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEx1.Controls.Add(this.tabPage_m1);
            this.tabControlEx1.Location = new System.Drawing.Point(14, 48);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(922, 415);
            this.tabControlEx1.TabIndex = 9;
            // 
            // tabPage_m1
            // 
            this.tabPage_m1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_m1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage_m1.Controls.Add(this.ucMoleculeLabelEditor_m1);
            this.tabPage_m1.Location = new System.Drawing.Point(0, 23);
            this.tabPage_m1.Name = "tabPage_m1";
            this.tabPage_m1.Size = new System.Drawing.Size(922, 392);
            this.tabPage_m1.TabIndex = 0;
            this.tabPage_m1.Text = "Set at runtime";
            // 
            // ucMoleculeLabelEditor_m1
            // 
            this.ucMoleculeLabelEditor_m1.BackColor = System.Drawing.SystemColors.Control;
            this.ucMoleculeLabelEditor_m1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMoleculeLabelEditor_m1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMoleculeLabelEditor_m1.Location = new System.Drawing.Point(0, 0);
            this.ucMoleculeLabelEditor_m1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucMoleculeLabelEditor_m1.Molecule = null;
            this.ucMoleculeLabelEditor_m1.Name = "ucMoleculeLabelEditor_m1";
            this.ucMoleculeLabelEditor_m1.Size = new System.Drawing.Size(920, 390);
            this.ucMoleculeLabelEditor_m1.TabIndex = 0;
            this.ucMoleculeLabelEditor_m1.Used1D = null;
            // 
            // EditLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 506);
            this.Controls.Add(this.tabControlEx1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbConcise);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EditLabels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Labels";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditLabels_FormClosing);
            this.Load += new System.EventHandler(this.FormEditLabels_Load);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage_m1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RichTextBox rtbConcise;
        private System.Windows.Forms.Label label1;
        private Core.UI.Controls.TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage_m1;
        private UserControls.UcMoleculeLabelEditor ucMoleculeLabelEditor_m1;
    }
}