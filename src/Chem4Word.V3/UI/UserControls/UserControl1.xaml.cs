// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

namespace Chem4Word.UI.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public string InputValue { get; set; }

        public delegate void EventHandler(object sender, WpfEventArgs args);

        public event EventHandler OnOkButtonClick;

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Text = "User Control #1" + Environment.NewLine + InputValue;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WpfEventArgs args = new WpfEventArgs();
            args.OutputValue = "One is done !";
            args.Button = "OK";

            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(this, args);
            }
        }
    }
}
