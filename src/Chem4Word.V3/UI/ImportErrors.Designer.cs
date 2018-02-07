namespace Chem4Word.UI
{
    partial class ImportErrors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportErrors));
            this.Errors = new System.Windows.Forms.TextBox();
            this.Warnings = new System.Windows.Forms.TextBox();
            this.Abort = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.OuterSplitContainer = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.InnerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.panelFormulae = new System.Windows.Forms.Panel();
            this.panelNames = new System.Windows.Forms.Panel();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.flexDisplay = new Chem4Word.Controls.FlexDisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.OuterSplitContainer)).BeginInit();
            this.OuterSplitContainer.Panel1.SuspendLayout();
            this.OuterSplitContainer.Panel2.SuspendLayout();
            this.OuterSplitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InnerSplitContainer)).BeginInit();
            this.InnerSplitContainer.Panel1.SuspendLayout();
            this.InnerSplitContainer.Panel2.SuspendLayout();
            this.InnerSplitContainer.SuspendLayout();
            this.panelFormulae.SuspendLayout();
            this.panelNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // Errors
            // 
            this.Errors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Errors.Location = new System.Drawing.Point(0, 0);
            this.Errors.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Errors.Multiline = true;
            this.Errors.Name = "Errors";
            this.Errors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Errors.Size = new System.Drawing.Size(428, 186);
            this.Errors.TabIndex = 0;
            this.Errors.WordWrap = false;
            // 
            // Warnings
            // 
            this.Warnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Warnings.Location = new System.Drawing.Point(0, 0);
            this.Warnings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Warnings.Multiline = true;
            this.Warnings.Name = "Warnings";
            this.Warnings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Warnings.Size = new System.Drawing.Size(428, 205);
            this.Warnings.TabIndex = 0;
            this.Warnings.WordWrap = false;
            // 
            // Abort
            // 
            this.Abort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Abort.Location = new System.Drawing.Point(730, 418);
            this.Abort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Abort.Name = "Abort";
            this.Abort.Size = new System.Drawing.Size(87, 28);
            this.Abort.TabIndex = 0;
            this.Abort.Text = "Abort";
            this.Abort.UseVisualStyleBackColor = true;
            this.Abort.Click += new System.EventHandler(this.Abort_Click);
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Continue.Location = new System.Drawing.Point(637, 418);
            this.Continue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(87, 28);
            this.Continue.TabIndex = 1;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Error(s)";
            // 
            // OuterSplitContainer
            // 
            this.OuterSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OuterSplitContainer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.OuterSplitContainer.Location = new System.Drawing.Point(12, 12);
            this.OuterSplitContainer.Name = "OuterSplitContainer";
            // 
            // OuterSplitContainer.Panel1
            // 
            this.OuterSplitContainer.Panel1.Controls.Add(this.panel1);
            // 
            // OuterSplitContainer.Panel2
            // 
            this.OuterSplitContainer.Panel2.Controls.Add(this.InnerSplitContainer);
            this.OuterSplitContainer.Size = new System.Drawing.Size(805, 396);
            this.OuterSplitContainer.SplitterDistance = 373;
            this.OuterSplitContainer.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.elementHost2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 396);
            this.panel1.TabIndex = 0;
            // 
            // InnerSplitContainer
            // 
            this.InnerSplitContainer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.InnerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InnerSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.InnerSplitContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InnerSplitContainer.Name = "InnerSplitContainer";
            this.InnerSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // InnerSplitContainer.Panel1
            // 
            this.InnerSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.InnerSplitContainer.Panel1.Controls.Add(this.panelFormulae);
            // 
            // InnerSplitContainer.Panel2
            // 
            this.InnerSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.InnerSplitContainer.Panel2.Controls.Add(this.panelNames);
            this.InnerSplitContainer.Size = new System.Drawing.Size(428, 396);
            this.InnerSplitContainer.SplitterDistance = 186;
            this.InnerSplitContainer.SplitterWidth = 5;
            this.InnerSplitContainer.TabIndex = 7;
            // 
            // panelFormulae
            // 
            this.panelFormulae.BackColor = System.Drawing.SystemColors.Control;
            this.panelFormulae.Controls.Add(this.Errors);
            this.panelFormulae.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormulae.Location = new System.Drawing.Point(0, 0);
            this.panelFormulae.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelFormulae.Name = "panelFormulae";
            this.panelFormulae.Size = new System.Drawing.Size(428, 186);
            this.panelFormulae.TabIndex = 4;
            // 
            // panelNames
            // 
            this.panelNames.BackColor = System.Drawing.SystemColors.Control;
            this.panelNames.Controls.Add(this.Warnings);
            this.panelNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNames.Location = new System.Drawing.Point(0, 0);
            this.panelNames.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelNames.Name = "panelNames";
            this.panelNames.Size = new System.Drawing.Size(428, 205);
            this.panelNames.TabIndex = 2;
            // 
            // elementHost2
            // 
            this.elementHost2.BackColor = System.Drawing.Color.White;
            this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost2.Location = new System.Drawing.Point(0, 0);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(371, 394);
            this.elementHost2.TabIndex = 0;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = this.flexDisplay;
            // 
            // ImportErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 459);
            this.Controls.Add(this.OuterSplitContainer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Abort);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ImportErrors";
            this.Text = "Import Error(s) or Warnings(s)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportErrors_FormClosing);
            this.Load += new System.EventHandler(this.ImportErrors_Load);
            this.OuterSplitContainer.Panel1.ResumeLayout(false);
            this.OuterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OuterSplitContainer)).EndInit();
            this.OuterSplitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.InnerSplitContainer.Panel1.ResumeLayout(false);
            this.InnerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InnerSplitContainer)).EndInit();
            this.InnerSplitContainer.ResumeLayout(false);
            this.panelFormulae.ResumeLayout(false);
            this.panelFormulae.PerformLayout();
            this.panelNames.ResumeLayout(false);
            this.panelNames.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Errors;
        private System.Windows.Forms.TextBox Warnings;
        private System.Windows.Forms.Button Abort;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer OuterSplitContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private Controls.FlexDisplayControl flexDisplay;
        public System.Windows.Forms.SplitContainer InnerSplitContainer;
        private System.Windows.Forms.Panel panelFormulae;
        private System.Windows.Forms.Panel panelNames;
    }
}