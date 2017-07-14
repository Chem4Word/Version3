namespace Chem4Word.UI.UserControls
{
    partial class UcEditName
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pbNameCheck = new System.Windows.Forms.PictureBox();
            this.btnDeleteName = new System.Windows.Forms.Button();
            this.txtDictRef = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbNameCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(182, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(276, 23);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(447, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pbNameCheck
            // 
            this.pbNameCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbNameCheck.Image = global::Chem4Word.Properties.Resources.LabelValid;
            this.pbNameCheck.Location = new System.Drawing.Point(467, 5);
            this.pbNameCheck.Name = "pbNameCheck";
            this.pbNameCheck.Size = new System.Drawing.Size(37, 37);
            this.pbNameCheck.TabIndex = 5;
            this.pbNameCheck.TabStop = false;
            // 
            // btnDeleteName
            // 
            this.btnDeleteName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteName.FlatAppearance.BorderSize = 0;
            this.btnDeleteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteName.Image = global::Chem4Word.Properties.Resources.TrashRed;
            this.btnDeleteName.Location = new System.Drawing.Point(507, 0);
            this.btnDeleteName.Name = "btnDeleteName";
            this.btnDeleteName.Size = new System.Drawing.Size(44, 41);
            this.btnDeleteName.TabIndex = 6;
            this.btnDeleteName.UseVisualStyleBackColor = true;
            this.btnDeleteName.Click += new System.EventHandler(this.OnDeleteNameClick);
            // 
            // txtDictRef
            // 
            this.txtDictRef.Location = new System.Drawing.Point(0, 10);
            this.txtDictRef.Name = "txtDictRef";
            this.txtDictRef.ReadOnly = true;
            this.txtDictRef.Size = new System.Drawing.Size(176, 23);
            this.txtDictRef.TabIndex = 7;
            // 
            // UcEditNameV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.txtDictRef);
            this.Controls.Add(this.btnDeleteName);
            this.Controls.Add(this.pbNameCheck);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UcEditNameV2";
            this.Size = new System.Drawing.Size(554, 43);
            this.Load += new System.EventHandler(this.UcEditName_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbNameCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbNameCheck;
        private System.Windows.Forms.Button btnDeleteName;
        private System.Windows.Forms.TextBox txtDictRef;
    }
}
