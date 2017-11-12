﻿using System.Windows;
using System.Windows.Controls;

namespace Chem4Word.Controls
{
    /// <summary>
    /// Interaction logic for FlexDisplayControl.xaml
    /// </summary>
    public partial class FlexDisplayControl : UserControl
    {
        public FlexDisplayControl()
        {
            InitializeComponent();
        }

        public bool ShowCarbonLabels
        {
            get { return (bool)GetValue(ShowCarbonLabelsProperty); }
            set { SetValue(ShowCarbonLabelsProperty, value); }
        }

        public static readonly DependencyProperty ShowCarbonLabelsProperty
            = DependencyProperty.Register("ShowCarbonLabels", typeof(bool), typeof(FlexDisplayControl), new PropertyMetadata(default(bool)));

        public object Chemistry
        {
            get { return (object)GetValue(ChemistryProperty); }
            set { SetValue(ChemistryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Chemistry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChemistryProperty =
            DependencyProperty.Register("Chemistry", typeof(object), typeof(FlexDisplayControl), new PropertyMetadata(null));
    }
}