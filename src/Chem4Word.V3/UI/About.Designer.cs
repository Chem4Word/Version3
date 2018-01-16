namespace Chem4Word.UI
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelBody1 = new System.Windows.Forms.Label();
            this.labelChemDoodleWeb = new System.Windows.Forms.Label();
            this.pictureBoxChemDoodle = new System.Windows.Forms.PictureBox();
            this.linkToTeamSite = new System.Windows.Forms.LinkLabel();
            this.linkToFacebook = new System.Windows.Forms.LinkLabel();
            this.linkToCambridge = new System.Windows.Forms.LinkLabel();
            this.linkToSponsor = new System.Windows.Forms.LinkLabel();
            this.labelChem4WordIsFree = new System.Windows.Forms.Label();
            this.linkToSourceCode = new System.Windows.Forms.LinkLabel();
            this.linkToYouTube = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChemDoodle)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::Chem4Word.Properties.Resources.C4W_Background_512;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Blue;
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(489, 39);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Chemistry Add-In for Microsoft Word";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.Blue;
            this.labelVersion.Location = new System.Drawing.Point(12, 48);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(488, 39);
            this.labelVersion.TabIndex = 2;
            this.labelVersion.Text = "Version ...";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBody1
            // 
            this.labelBody1.BackColor = System.Drawing.Color.Transparent;
            this.labelBody1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBody1.Location = new System.Drawing.Point(4, 94);
            this.labelBody1.MaximumSize = new System.Drawing.Size(560, 246);
            this.labelBody1.Name = "labelBody1";
            this.labelBody1.Size = new System.Drawing.Size(496, 136);
            this.labelBody1.TabIndex = 3;
            this.labelBody1.Text = resources.GetString("labelBody1.Text");
            // 
            // labelChemDoodleWeb
            // 
            this.labelChemDoodleWeb.BackColor = System.Drawing.Color.Transparent;
            this.labelChemDoodleWeb.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChemDoodleWeb.Location = new System.Drawing.Point(4, 246);
            this.labelChemDoodleWeb.MaximumSize = new System.Drawing.Size(408, 246);
            this.labelChemDoodleWeb.Name = "labelChemDoodleWeb";
            this.labelChemDoodleWeb.Size = new System.Drawing.Size(370, 30);
            this.labelChemDoodleWeb.TabIndex = 4;
            this.labelChemDoodleWeb.Text = "Chem4Word incorporates an on-line Open Source HTML5 Chemistry Editor - ChemDoodle" +
    " Web Components";
            // 
            // pictureBoxChemDoodle
            // 
            this.pictureBoxChemDoodle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxChemDoodle.Image = global::Chem4Word.Properties.Resources.ChemDoodleBadge;
            this.pictureBoxChemDoodle.Location = new System.Drawing.Point(380, 246);
            this.pictureBoxChemDoodle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxChemDoodle.Name = "pictureBoxChemDoodle";
            this.pictureBoxChemDoodle.Size = new System.Drawing.Size(120, 30);
            this.pictureBoxChemDoodle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxChemDoodle.TabIndex = 5;
            this.pictureBoxChemDoodle.TabStop = false;
            this.pictureBoxChemDoodle.Click += new System.EventHandler(this.OnChemDoodleClick);
            // 
            // linkToTeamSite
            // 
            this.linkToTeamSite.AutoSize = true;
            this.linkToTeamSite.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToTeamSite.Location = new System.Drawing.Point(4, 286);
            this.linkToTeamSite.Name = "linkToTeamSite";
            this.linkToTeamSite.Size = new System.Drawing.Size(146, 14);
            this.linkToTeamSite.TabIndex = 6;
            this.linkToTeamSite.TabStop = true;
            this.linkToTeamSite.Text = "Visit the Project web site";
            this.linkToTeamSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnTeamsiteLinkClicked);
            // 
            // linkToFacebook
            // 
            this.linkToFacebook.AutoSize = true;
            this.linkToFacebook.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToFacebook.Location = new System.Drawing.Point(4, 452);
            this.linkToFacebook.Name = "linkToFacebook";
            this.linkToFacebook.Size = new System.Drawing.Size(143, 14);
            this.linkToFacebook.TabIndex = 7;
            this.linkToFacebook.TabStop = true;
            this.linkToFacebook.Text = "Join our Facebook Group";
            this.linkToFacebook.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnFacebookLinkClicked);
            // 
            // linkToCambridge
            // 
            this.linkToCambridge.AutoSize = true;
            this.linkToCambridge.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToCambridge.Location = new System.Drawing.Point(4, 322);
            this.linkToCambridge.Name = "linkToCambridge";
            this.linkToCambridge.Size = new System.Drawing.Size(311, 14);
            this.linkToCambridge.TabIndex = 8;
            this.linkToCambridge.TabStop = true;
            this.linkToCambridge.Text = "Visit the University of Cambridge Chemistry Department";
            this.linkToCambridge.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnCambridgeLinkClicked);
            // 
            // linkToSponsor
            // 
            this.linkToSponsor.AutoSize = true;
            this.linkToSponsor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToSponsor.Location = new System.Drawing.Point(4, 340);
            this.linkToSponsor.Name = "linkToSponsor";
            this.linkToSponsor.Size = new System.Drawing.Size(359, 14);
            this.linkToSponsor.TabIndex = 9;
            this.linkToSponsor.TabStop = true;
            this.linkToSponsor.Text = "Visit our page on our sponsor\'s web site (The .NET Foundation)";
            this.linkToSponsor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSponsorLinkClicked);
            // 
            // labelChem4WordIsFree
            // 
            this.labelChem4WordIsFree.BackColor = System.Drawing.Color.Transparent;
            this.labelChem4WordIsFree.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChem4WordIsFree.Location = new System.Drawing.Point(4, 385);
            this.labelChem4WordIsFree.MaximumSize = new System.Drawing.Size(560, 246);
            this.labelChem4WordIsFree.Name = "labelChem4WordIsFree";
            this.labelChem4WordIsFree.Size = new System.Drawing.Size(496, 53);
            this.labelChem4WordIsFree.TabIndex = 10;
            this.labelChem4WordIsFree.Text = "The Chemistry Add-In for Microsoft Word is available for free.  Your feedback is " +
    "important to us. Join our Facebook group to let us know if you have any suggesti" +
    "ons or problems using it.";
            // 
            // linkToSourceCode
            // 
            this.linkToSourceCode.AutoSize = true;
            this.linkToSourceCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToSourceCode.Location = new System.Drawing.Point(4, 358);
            this.linkToSourceCode.Name = "linkToSourceCode";
            this.linkToSourceCode.Size = new System.Drawing.Size(289, 14);
            this.linkToSourceCode.TabIndex = 11;
            this.linkToSourceCode.TabStop = true;
            this.linkToSourceCode.Text = "Visit our GitHub repository to view the source code";
            this.linkToSourceCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSourceCodeLinkClicked);
            // 
            // linkToYouTube
            // 
            this.linkToYouTube.AutoSize = true;
            this.linkToYouTube.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkToYouTube.Location = new System.Drawing.Point(4, 304);
            this.linkToYouTube.Name = "linkToYouTube";
            this.linkToYouTube.Size = new System.Drawing.Size(126, 14);
            this.linkToYouTube.TabIndex = 12;
            this.linkToYouTube.TabStop = true;
            this.linkToYouTube.Text = "Tutorials on YouTube";
            this.linkToYouTube.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnYouTubeLinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 513);
            this.Controls.Add(this.linkToYouTube);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.linkToSourceCode);
            this.Controls.Add(this.labelChem4WordIsFree);
            this.Controls.Add(this.linkToSponsor);
            this.Controls.Add(this.linkToCambridge);
            this.Controls.Add(this.linkToFacebook);
            this.Controls.Add(this.linkToTeamSite);
            this.Controls.Add(this.pictureBoxChemDoodle);
            this.Controls.Add(this.labelChemDoodleWeb);
            this.Controls.Add(this.labelBody1);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Text = "About - Chemistry Add-In for Microsoft Word";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChemDoodle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelBody1;
        private System.Windows.Forms.Label labelChemDoodleWeb;
        private System.Windows.Forms.PictureBox pictureBoxChemDoodle;
        private System.Windows.Forms.LinkLabel linkToTeamSite;
        private System.Windows.Forms.LinkLabel linkToFacebook;
        private System.Windows.Forms.LinkLabel linkToCambridge;
        private System.Windows.Forms.LinkLabel linkToSponsor;
        private System.Windows.Forms.Label labelChem4WordIsFree;
        private System.Windows.Forms.LinkLabel linkToSourceCode;
        private System.Windows.Forms.LinkLabel linkToYouTube;
    }
}