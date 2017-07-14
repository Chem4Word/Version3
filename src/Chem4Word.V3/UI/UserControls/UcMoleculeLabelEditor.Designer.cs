namespace Chem4Word.UI.UserControls
{
    partial class UcMoleculeLabelEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InnerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.panelFormulae = new System.Windows.Forms.Panel();
            this.panelNames = new System.Windows.Forms.Panel();
            this.btnAddFormula = new System.Windows.Forms.Button();
            this.btnAddName = new System.Windows.Forms.Button();
            this.OuterSplitContainer = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.flexDisplayControl1 = new Chem4Word.Controls.FlexDisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.InnerSplitContainer)).BeginInit();
            this.InnerSplitContainer.Panel1.SuspendLayout();
            this.InnerSplitContainer.Panel2.SuspendLayout();
            this.InnerSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OuterSplitContainer)).BeginInit();
            this.OuterSplitContainer.Panel1.SuspendLayout();
            this.OuterSplitContainer.Panel2.SuspendLayout();
            this.OuterSplitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.InnerSplitContainer.Size = new System.Drawing.Size(443, 394);
            this.InnerSplitContainer.SplitterDistance = 186;
            this.InnerSplitContainer.SplitterWidth = 5;
            this.InnerSplitContainer.TabIndex = 7;
            // 
            // panelFormulae
            // 
            this.panelFormulae.BackColor = System.Drawing.SystemColors.Control;
            this.panelFormulae.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormulae.Location = new System.Drawing.Point(0, 0);
            this.panelFormulae.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelFormulae.Name = "panelFormulae";
            this.panelFormulae.Size = new System.Drawing.Size(443, 186);
            this.panelFormulae.TabIndex = 4;
            // 
            // panelNames
            // 
            this.panelNames.BackColor = System.Drawing.SystemColors.Control;
            this.panelNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNames.Location = new System.Drawing.Point(0, 0);
            this.panelNames.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelNames.Name = "panelNames";
            this.panelNames.Size = new System.Drawing.Size(443, 203);
            this.panelNames.TabIndex = 2;
            // 
            // btnAddFormula
            // 
            this.btnAddFormula.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddFormula.Image = global::Chem4Word.Properties.Resources.LabelAdd;
            this.btnAddFormula.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFormula.Location = new System.Drawing.Point(0, 0);
            this.btnAddFormula.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddFormula.Name = "btnAddFormula";
            this.btnAddFormula.Size = new System.Drawing.Size(276, 48);
            this.btnAddFormula.TabIndex = 5;
            this.btnAddFormula.Text = "Add Formula";
            this.btnAddFormula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddFormula.UseVisualStyleBackColor = true;
            this.btnAddFormula.Click += new System.EventHandler(this.OnAddFormulaClick);
            // 
            // btnAddName
            // 
            this.btnAddName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAddName.Image = global::Chem4Word.Properties.Resources.LabelAdd;
            this.btnAddName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddName.Location = new System.Drawing.Point(0, 346);
            this.btnAddName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddName.Name = "btnAddName";
            this.btnAddName.Size = new System.Drawing.Size(276, 48);
            this.btnAddName.TabIndex = 3;
            this.btnAddName.Text = "Add Name";
            this.btnAddName.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddName.UseVisualStyleBackColor = true;
            this.btnAddName.Click += new System.EventHandler(this.OnAddNameClick);
            // 
            // OuterSplitContainer
            // 
            this.OuterSplitContainer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.OuterSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OuterSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.OuterSplitContainer.Name = "OuterSplitContainer";
            // 
            // OuterSplitContainer.Panel1
            // 
            this.OuterSplitContainer.Panel1.Controls.Add(this.panel1);
            // 
            // OuterSplitContainer.Panel2
            // 
            this.OuterSplitContainer.Panel2.Controls.Add(this.InnerSplitContainer);
            this.OuterSplitContainer.Size = new System.Drawing.Size(723, 394);
            this.OuterSplitContainer.SplitterDistance = 276;
            this.OuterSplitContainer.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.elementHost1);
            this.panel1.Controls.Add(this.btnAddName);
            this.panel1.Controls.Add(this.btnAddFormula);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 394);
            this.panel1.TabIndex = 0;
            // 
            // elementHost1
            // 
            this.elementHost1.BackColor = System.Drawing.Color.White;
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 48);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(276, 298);
            this.elementHost1.TabIndex = 6;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.flexDisplayControl1;
            // 
            // UcMoleculeLabelEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.OuterSplitContainer);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UcMoleculeLabelEditor";
            this.Size = new System.Drawing.Size(723, 394);
            this.Load += new System.EventHandler(this.UcMoleculeLabelEditor_Load);
            this.InnerSplitContainer.Panel1.ResumeLayout(false);
            this.InnerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InnerSplitContainer)).EndInit();
            this.InnerSplitContainer.ResumeLayout(false);
            this.OuterSplitContainer.Panel1.ResumeLayout(false);
            this.OuterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OuterSplitContainer)).EndInit();
            this.OuterSplitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddFormula;
        private System.Windows.Forms.Panel panelFormulae;
        private System.Windows.Forms.Button btnAddName;
        private System.Windows.Forms.Panel panelNames;
        public System.Windows.Forms.SplitContainer InnerSplitContainer;
        private System.Windows.Forms.SplitContainer OuterSplitContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Controls.FlexDisplayControl flexDisplayControl1;
    }
}
