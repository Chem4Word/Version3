namespace Chem4WordUpdater
{
    partial class Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.UpdateNow = new System.Windows.Forms.Button();
            this.Information = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.WordInstances = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(175, 275);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(216, 26);
            this.progressBar1.TabIndex = 5;
            // 
            // UpdateNow
            // 
            this.UpdateNow.Location = new System.Drawing.Point(395, 275);
            this.UpdateNow.Margin = new System.Windows.Forms.Padding(5);
            this.UpdateNow.Name = "UpdateNow";
            this.UpdateNow.Size = new System.Drawing.Size(86, 26);
            this.UpdateNow.TabIndex = 6;
            this.UpdateNow.Text = "Cancel";
            this.UpdateNow.UseVisualStyleBackColor = true;
            this.UpdateNow.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Information
            // 
            this.Information.BackColor = System.Drawing.Color.Transparent;
            this.Information.Location = new System.Drawing.Point(175, 197);
            this.Information.Name = "Information";
            this.Information.Size = new System.Drawing.Size(306, 73);
            this.Information.TabIndex = 7;
            this.Information.Text = "Your update is downloading ...";
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // WordInstances
            // 
            this.WordInstances.FormattingEnabled = true;
            this.WordInstances.ItemHeight = 16;
            this.WordInstances.Location = new System.Drawing.Point(179, 29);
            this.WordInstances.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WordInstances.Name = "WordInstances";
            this.WordInstances.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.WordInstances.Size = new System.Drawing.Size(298, 164);
            this.WordInstances.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(176, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Microsoft Word Processes";
            // 
            // Updater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Chem4WordUpdater.Properties.Resources.WixUIDialog;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(491, 312);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WordInstances);
            this.Controls.Add(this.Information);
            this.Controls.Add(this.UpdateNow);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Updater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chem4Word Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Updater_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button UpdateNow;
        private System.Windows.Forms.Label Information;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox WordInstances;
        private System.Windows.Forms.Label label1;
    }
}