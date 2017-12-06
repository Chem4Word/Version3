// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.UI.UserControls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chem4Word.UI
{
    public partial class WpfHost : Form
    {
        public System.Windows.Point TopLeft { get; set; }

        public Size FormSize { get; set; }

        public string InputValue { get; set; }

        public string OutputValue { get; set; }

        public WpfHost(string controlName)
        {
            InitializeComponent();

            // Could this be from a list of supported PlugIns ???
            switch (controlName)
            {
                case "UserControl1":
                    UserControl1 _uc1 = new UserControl1();
                    _uc1.InputValue = "Hello #1";
                    _uc1.InitializeComponent();
                    elementHost1.Child = _uc1;
                    _uc1.OnOkButtonClick += OnWpfOkButtonClick;
                    break;

                case "UserControl2":
                    UserControl2 _uc2 = new UserControl2();
                    _uc2.InputValue = "Hello #2";
                    _uc2.InitializeComponent();
                    elementHost1.Child = _uc2;
                    _uc2.OnOkButtonClick += OnWpfOkButtonClick;
                    break;
            }
            this.Text = $"WpfHost for {controlName}";
        }

        private void OnWpfOkButtonClick(object sender, EventArgs e)
        {
            WpfEventArgs args = (WpfEventArgs)e;
            if (args.Button.Equals("OK"))
            {
                OutputValue = args.OutputValue;
                Close();
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void WpfHost_Load(object sender, EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            if (!FormSize.IsEmpty)
            {
                MinimumSize = FormSize;

                Width = FormSize.Width;
                Height = FormSize.Height;
            }
        }
    }

    public class WpfEventArgs : EventArgs
    {
        public string Button { get; set; }
        public string OutputValue { get; set; }
    }
}
