// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace Chem4Word.UI
{
    public partial class UpdateFailure : Form
    {
        public string WebPage { get; set; }

        public System.Windows.Point TopLeft { get; set; }

        public UpdateFailure()
        {
            InitializeComponent();
        }

        private void UpdateFailure_Load(object sender, EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            webBrowser1.DocumentText = WebPage;
        }
    }
}