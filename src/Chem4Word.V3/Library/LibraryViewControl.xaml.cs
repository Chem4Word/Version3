// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Chem4Word.Library
{
    /// <summary>
    /// Interaction logic for LibraryViewControl.xaml
    /// </summary>
    public partial class LibraryViewControl : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public LibraryViewControl()
        {
            InitializeComponent();
        }

        //load up an unfiltered model
        public void Refresh()
        {
            this.MainGrid.DataContext = new LibraryViewModel();
        }

        /// <summary>
        /// Handles filtering of the library list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    //get the view from the listbox's source
                    ICollectionView filteredView = CollectionViewSource.GetDefaultView((this.MainGrid.DataContext as LibraryViewModel).ChemistryItems);
                    //then try to match part of either its name or an alternative name to the string typed in
                    filteredView.Filter = ci =>
                    {
                        var itm = ci as LibraryViewModel.Chemistry;
                        var queryString = SearchBox.Text.ToUpper();
                        return itm.Name.ToUpper().Contains(queryString)
                               || itm.OtherNames.Any(n => n.ToUpper().Contains(queryString));
                    };

                    SearchButton.IsEnabled = false;
                    ClearButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SearchBox.Clear();
                SearchButton.IsEnabled = true;
                ClearButton.IsEnabled = false;
                ICollectionView filteredView = CollectionViewSource.GetDefaultView((this.MainGrid.DataContext as LibraryViewModel).ChemistryItems);
                filteredView.Filter = null;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    SearchButton.IsEnabled = true;
                    ClearButton.IsEnabled = false;
                }
                else
                {
                    SearchButton.IsEnabled = false;
                    ClearButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}