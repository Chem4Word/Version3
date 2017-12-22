// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chem4WordSetup
{
    public partial class TaskIndicator : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Test text displayed in the label"), Category("Custom")]
        public string Description
        {
            get { return description.Text; }
            set { description.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Progress indicitor"), Category("Custom")]
        public Image Indicator
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        public TaskIndicator()
        {
            InitializeComponent();
        }
    }
}