namespace Chem4Word.Searcher.PubChemPlugIn
{
    partial class SearchPubChem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchPubChem));
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.flexDisplayControl1 = new Chem4Word.Controls.FlexDisplayControl();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchFor = new System.Windows.Forms.TextBox();
            this.Results = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImportButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ErrorsAndWarnings = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.White;
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(390, 428);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.flexDisplayControl1;
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(789, 13);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(87, 29);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchFor
            // 
            this.SearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchFor.Location = new System.Drawing.Point(13, 15);
            this.SearchFor.Name = "SearchFor";
            this.SearchFor.Size = new System.Drawing.Size(770, 23);
            this.SearchFor.TabIndex = 0;
            // 
            // Results
            // 
            this.Results.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.Results.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Results.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.Results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Results.FullRowSelect = true;
            this.Results.GridLines = true;
            this.Results.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Results.Location = new System.Drawing.Point(0, 0);
            this.Results.Name = "Results";
            this.Results.Size = new System.Drawing.Size(467, 428);
            this.Results.TabIndex = 3;
            this.Results.UseCompatibleStateImageBehavior = false;
            this.Results.View = System.Windows.Forms.View.Details;
            this.Results.SelectedIndexChanged += new System.EventHandler(this.Results_SelectedIndexChanged);
            this.Results.DoubleClick += new System.EventHandler(this.Results_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Fomula";
            this.columnHeader3.Width = 100;
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.Location = new System.Drawing.Point(790, 525);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(87, 29);
            this.ImportButton.TabIndex = 4;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviousButton.Image = global::Chem4Word.Searcher.PubChemPlugIn.Properties.Resources.ArrowLeft;
            this.PreviousButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PreviousButton.Location = new System.Drawing.Point(12, 525);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(75, 29);
            this.PreviousButton.TabIndex = 5;
            this.PreviousButton.Text = "Prev";
            this.PreviousButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Image = global::Chem4Word.Searcher.PubChemPlugIn.Properties.Resources.ArrowRight;
            this.NextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NextButton.Location = new System.Drawing.Point(13, 490);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(74, 29);
            this.NextButton.TabIndex = 6;
            this.NextButton.Text = "Next";
            this.NextButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 47);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.Results);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.elementHost1);
            this.splitContainer1.Size = new System.Drawing.Size(865, 430);
            this.splitContainer1.SplitterDistance = 469;
            this.splitContainer1.TabIndex = 7;
            // 
            // ErrorsAndWarnings
            // 
            this.ErrorsAndWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorsAndWarnings.Location = new System.Drawing.Point(93, 490);
            this.ErrorsAndWarnings.Multiline = true;
            this.ErrorsAndWarnings.Name = "ErrorsAndWarnings";
            this.ErrorsAndWarnings.ReadOnly = true;
            this.ErrorsAndWarnings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ErrorsAndWarnings.Size = new System.Drawing.Size(690, 64);
            this.ErrorsAndWarnings.TabIndex = 8;
            this.ErrorsAndWarnings.WordWrap = false;
            // 
            // SearchPubChem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 566);
            this.Controls.Add(this.ErrorsAndWarnings);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.PreviousButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.SearchFor);
            this.Controls.Add(this.SearchButton);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchPubChem";
            this.Text = "Search PubChem public database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchPubChem_FormClosing);
            this.Load += new System.EventHandler(this.SearchPubChem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Controls.FlexDisplayControl flexDisplayControl1;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchFor;
        private System.Windows.Forms.ListView Results;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox ErrorsAndWarnings;
    }
}