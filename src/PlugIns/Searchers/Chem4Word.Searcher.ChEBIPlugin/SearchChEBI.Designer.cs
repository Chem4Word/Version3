namespace Chem4Word.Searcher.ChEBIPlugin
{
    partial class SearchChEBI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchChEBI));
            this.SearchFor = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.LabelInfo = new System.Windows.Forms.Label();
            this.ImportButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ResultsListView = new System.Windows.Forms.ListView();
            this.IDHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ScoreHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ShowMolfile = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.flexDisplayControl1 = new Chem4Word.Controls.FlexDisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchFor
            // 
            this.SearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchFor.Location = new System.Drawing.Point(14, 15);
            this.SearchFor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchFor.Name = "SearchFor";
            this.SearchFor.Size = new System.Drawing.Size(895, 23);
            this.SearchFor.TabIndex = 2;
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(915, 12);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(87, 29);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // LabelInfo
            // 
            this.LabelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelInfo.AutoSize = true;
            this.LabelInfo.Location = new System.Drawing.Point(14, 539);
            this.LabelInfo.Name = "LabelInfo";
            this.LabelInfo.Size = new System.Drawing.Size(20, 16);
            this.LabelInfo.TabIndex = 11;
            this.LabelInfo.Text = "...";
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ImportButton.Location = new System.Drawing.Point(916, 533);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(87, 29);
            this.ImportButton.TabIndex = 12;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(14, 54);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ResultsListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(989, 461);
            this.splitContainer1.SplitterDistance = 454;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 13;
            // 
            // ResultsListView
            // 
            this.ResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IDHeader,
            this.NameHeader,
            this.ScoreHeader});
            this.ResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsListView.FullRowSelect = true;
            this.ResultsListView.GridLines = true;
            this.ResultsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ResultsListView.Location = new System.Drawing.Point(0, 0);
            this.ResultsListView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ResultsListView.Name = "ResultsListView";
            this.ResultsListView.Size = new System.Drawing.Size(452, 459);
            this.ResultsListView.TabIndex = 0;
            this.ResultsListView.UseCompatibleStateImageBehavior = false;
            this.ResultsListView.View = System.Windows.Forms.View.Details;
            this.ResultsListView.SelectedIndexChanged += new System.EventHandler(this.ResultsListView_SelectedIndexChanged);
            this.ResultsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ResultsListView_MouseDoubleClick);
            // 
            // IDHeader
            // 
            this.IDHeader.Text = "ID";
            // 
            // NameHeader
            // 
            this.NameHeader.Text = "Name";
            this.NameHeader.Width = 240;
            // 
            // ScoreHeader
            // 
            this.ScoreHeader.Text = "Score";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.elementHost1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 459);
            this.panel1.TabIndex = 0;
            // 
            // ShowMolfile
            // 
            this.ShowMolfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowMolfile.Location = new System.Drawing.Point(822, 533);
            this.ShowMolfile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShowMolfile.Name = "ShowMolfile";
            this.ShowMolfile.Size = new System.Drawing.Size(87, 29);
            this.ShowMolfile.TabIndex = 14;
            this.ShowMolfile.Text = "MolFile";
            this.ShowMolfile.UseVisualStyleBackColor = true;
            this.ShowMolfile.Click += new System.EventHandler(this.ShowMolfile_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.White;
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Padding = new System.Windows.Forms.Padding(12);
            this.elementHost1.Size = new System.Drawing.Size(526, 457);
            this.elementHost1.TabIndex = 1;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.flexDisplayControl1;
            // 
            // SearchChEBI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 580);
            this.Controls.Add(this.ShowMolfile);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.LabelInfo);
            this.Controls.Add(this.SearchFor);
            this.Controls.Add(this.SearchButton);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SearchChEBI";
            this.Text = "Search ChEBI public database";
            this.Load += new System.EventHandler(this.SearchChEBI_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchFor;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label LabelInfo;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView ResultsListView;
        private System.Windows.Forms.ColumnHeader IDHeader;
        private System.Windows.Forms.ColumnHeader NameHeader;
        private System.Windows.Forms.ColumnHeader ScoreHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Controls.FlexDisplayControl flexDisplayControl1;
        private System.Windows.Forms.Button ShowMolfile;
    }
}