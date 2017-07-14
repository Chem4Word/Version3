namespace Chem4Word.Core.UI.Forms
{
    partial class Progress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
            this.label1 = new System.Windows.Forms.Label();
            this.customProgressBar1 = new Chem4Word.Core.UI.Controls.CustomProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(660, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "...";
            // 
            // customProgressBar1
            // 
            this.customProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.customProgressBar1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.customProgressBar1.BorderColor = System.Drawing.Color.DarkGray;
            this.customProgressBar1.GradiantPosition = Chem4Word.Core.UI.Controls.CustomProgressBar.GradiantArea.None;
            this.customProgressBar1.Image = null;
            this.customProgressBar1.Location = new System.Drawing.Point(12, 12);
            this.customProgressBar1.Name = "customProgressBar1";
            this.customProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(176)))), ((int)(((byte)(37)))));
            this.customProgressBar1.RoundedCorners = false;
            this.customProgressBar1.ShowText = true;
            this.customProgressBar1.Size = new System.Drawing.Size(661, 24);
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 84);
            this.Controls.Add(this.customProgressBar1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Progress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chem4Word Progress";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormProgress_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Controls.CustomProgressBar customProgressBar1;
    }
}