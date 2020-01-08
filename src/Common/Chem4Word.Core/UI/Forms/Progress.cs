// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows.Forms;

namespace Chem4Word.Core.UI.Forms
{
    public partial class Progress : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        public System.Windows.Point TopLeft { get; set; }

        public int Value
        {
            get { return customProgressBar1.Value; }
            set
            {
                if (value >= customProgressBar1.Minimum && value <= customProgressBar1.Maximum)
                {
                    customProgressBar1.Value = value;
                    SetProgressBarText();
                }
            }
        }

        public int Minimum
        {
            get { return customProgressBar1.Minimum; }
            set
            {
                if (value >= 0)
                {
                    customProgressBar1.Minimum = value;
                }
            }
        }

        public int Maximum
        {
            get { return customProgressBar1.Maximum; }
            set
            {
                if (value > 0)
                {
                    customProgressBar1.Maximum = value;
                }
            }
        }

        public string Message
        {
            get { return label1.Text; }
            set
            {
                label1.Text = value;
                Application.DoEvents();
                this.Refresh();
            }
        }

        public Progress()
        {
            InitializeComponent();
        }

        public void Increment(int value)
        {
            customProgressBar1.Value += value;
            SetProgressBarText();
            //Debug.WriteLine(customProgressBar1.Text);
        }

        private void SetProgressBarText()
        {
            if (customProgressBar1.Value > 0)
            {
                customProgressBar1.Text = $"{customProgressBar1.Value}/{customProgressBar1.Maximum}";
            }
            else
            {
                customProgressBar1.Text = "";
            }
        }

        // Disable Close Button
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void FormProgress_Load(object sender, System.EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }
#if DEBUG
            this.TopMost = false;
#endif
        }
    }
}