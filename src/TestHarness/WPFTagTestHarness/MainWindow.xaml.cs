// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;

namespace WPFTagTestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            myTagControl.TagControlModel.AddTags(new ObservableCollection<string> { "three", "Four" });
            myTagControl.TagControlModel.AddKnownTags(new ObservableCollection<string> { "One", "Two" });
        }
    }
}