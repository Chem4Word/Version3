// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace Chem4Word.Controls
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        public DisplayControl()
        {
            InitializeComponent();
        }

        public bool ShowCarbonLabels
        {
            get { return (bool)GetValue(ShowCarbonLabelsProperty); }
            set { SetValue(ShowCarbonLabelsProperty, value); }
        }

        public static readonly DependencyProperty ShowCarbonLabelsProperty
            = DependencyProperty.Register("ShowCarbonLabels", typeof(bool), typeof(DisplayControl), new PropertyMetadata(default(bool)));

        public object Chemistry
        {
            get { return (object)GetValue(ChemistryProperty); }
            set { SetValue(ChemistryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Chemistry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChemistryProperty =
            DependencyProperty.Register("Chemistry", typeof(object), typeof(DisplayControl), new PropertyMetadata(null));
    }
}