// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Converters;
using Chem4WordTests;
using System.Windows;
using System.Windows.Controls;

namespace ControlTestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FlexDisplay_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            CMLConverter conv = new CMLConverter();

            ComboBoxItem cb1 = new ComboBoxItem();

            cb1.Tag = conv.Import(ChemistryValues.TESTOSTERONE);
            cb1.Content = "testosterone";
            SelectStructureCombo.Items.Add(cb1);

            ComboBoxItem cb2 = new ComboBoxItem();

            cb2.Tag = ChemistryValues.PARAFUCHSIN_CARBOL;

            cb2.Content = "parafuchsin carbol";
            SelectStructureCombo.Items.Add(cb2);

            ComboBoxItem cb3 = new ComboBoxItem();

            cb3.Tag = conv.Import(ChemistryValues.PHTHALOCYANINE);
            cb3.Content = "phthalocyanine";
            SelectStructureCombo.Items.Add(cb3);

            ComboBoxItem cb4 = new ComboBoxItem();

            cb4.Tag = conv.Import(ChemistryValues.THEMONSTER);
            cb4.Content = "The Monster";
            SelectStructureCombo.Items.Add(cb4);

            ComboBoxItem cb5 = new ComboBoxItem();

            cb5.Tag = conv.Import(ChemistryValues.INSULIN);
            cb5.Content = "insulin";
            SelectStructureCombo.Items.Add(cb5);

            ComboBoxItem cb6 = new ComboBoxItem();

            cb6.Tag = conv.Import(ChemistryValues.CHARGESPLUS);
            cb6.Content = "lots of charges";
            SelectStructureCombo.Items.Add(cb6);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}