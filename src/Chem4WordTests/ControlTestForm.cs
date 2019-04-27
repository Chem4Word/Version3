// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows.Forms;

namespace Chem4WordTests
{
    public partial class ControlTestForm : Form
    {
        public ControlTestForm()
        {
            InitializeComponent();
        }

        public string Chemistry
        {
            get { return (string)this.display1.Chemistry; }
            set { this.display1.Chemistry = value; }
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }
    }
}