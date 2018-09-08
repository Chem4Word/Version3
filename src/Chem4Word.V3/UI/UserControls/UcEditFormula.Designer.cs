namespace Chem4Word.UI.UserControls
{
    partial class UcEditFormula
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
            this.btnDeleteFormula = new System.Windows.Forms.Button();
            this.pbFormulaCheck = new System.Windows.Forms.PictureBox();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.txtConvention = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbFormulaCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDeleteFormula
            // 
            this.btnDeleteFormula.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteFormula.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteFormula.FlatAppearance.BorderSize = 0;
            this.btnDeleteFormula.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteFormula.Image = global::Chem4Word.Properties.Resources.TrashRed;
            this.btnDeleteFormula.Location = new System.Drawing.Point(507, 0);
            this.btnDeleteFormula.Name = "btnDeleteFormula";
            this.btnDeleteFormula.Size = new System.Drawing.Size(44, 41);
            this.btnDeleteFormula.TabIndex = 10;
            this.btnDeleteFormula.UseVisualStyleBackColor = true;
            this.btnDeleteFormula.Click += new System.EventHandler(this.OnDeleteFormulaClick);
            // 
            // pbFormulaCheck
            // 
            this.pbFormulaCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFormulaCheck.Image = global::Chem4Word.Properties.Resources.LabelValid;
            this.pbFormulaCheck.Location = new System.Drawing.Point(467, 5);
            this.pbFormulaCheck.Name = "pbFormulaCheck";
            this.pbFormulaCheck.Size = new System.Drawing.Size(37, 37);
            this.pbFormulaCheck.TabIndex = 9;
            this.pbFormulaCheck.TabStop = false;
            // 
            // txtFormula
            // 
            this.txtFormula.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormula.Location = new System.Drawing.Point(211, 10);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(247, 23);
            this.txtFormula.TabIndex = 7;
            this.txtFormula.TextChanged += new System.EventHandler(this.txtFormula_TextChanged);
            // 
            // txtConvention
            // 
            this.txtConvention.Location = new System.Drawing.Point(0, 10);
            this.txtConvention.Name = "txtConvention";
            this.txtConvention.ReadOnly = true;
            this.txtConvention.Size = new System.Drawing.Size(205, 23);
            this.txtConvention.TabIndex = 11;
            // 
            // UcEditFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtConvention);
            this.Controls.Add(this.btnDeleteFormula);
            this.Controls.Add(this.pbFormulaCheck);
            this.Controls.Add(this.txtFormula);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UcEditFormula";
            this.Size = new System.Drawing.Size(554, 43);
            this.Load += new System.EventHandler(this.UcEditFormula_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbFormulaCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteFormula;
        private System.Windows.Forms.PictureBox pbFormulaCheck;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.TextBox txtConvention;
    }
}
