// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Application = Microsoft.Office.Interop.Word.Application;

namespace Chem4Word.Navigator
{
    /// <summary>
    /// Interaction logic for NavigatorView.xaml
    /// </summary>
    public partial class NavigatorView : UserControl
    {
        private Application _activeApplication;
        private Document _activeDoc;

        public NavigatorView()
        {
            InitializeComponent();
        }

        private void NavigatorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(nameof(sender), e.NewValue);
        }

        public Application ActiveApplication
        {
            get { return _activeApplication; }
            set
            {
                if (_activeApplication != null)
                {
                    DisconnectAppEvents();
                }
                _activeApplication = value;
                ConnectUpAppEvents();
                //force a reload
            }
        }

        public Document ActiveDocument
        {
            get { return _activeDoc; }
            set
            {
                _activeDoc = value;
                try
                {
                    if (_activeDoc != null)
                    {
                        NavigatorViewModel nvm = new NavigatorViewModel(_activeDoc);
                        this.DataContext = nvm;
                    }
                    else
                    {
                        this.DataContext = null;
                    }
                }
                catch (COMException) //document not open
                {
                    this.DataContext = null;
                }
            }
        }

        private void DisconnectAppEvents()
        {
        }

        private void ConnectUpAppEvents()
        {
        }
    }
}