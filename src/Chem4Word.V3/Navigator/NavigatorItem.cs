// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Controls.Annotations;
using Chem4Word.Core.UI.Forms;
using Microsoft.Office.Core;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chem4Word.Navigator
{
    public class NavigatorItem : INotifyPropertyChanged
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private string _cmlID;

        public string CMLId
        {
            get { return _cmlID; }
            set
            {
                _cmlID = value;
                OnPropertyChanged();
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private object _chemicalStructure = "";

        public object ChemicalStructure
        {
            get { return _chemicalStructure; }
            set
            {
                _chemicalStructure = value;
                OnPropertyChanged();
            }
        }

        private CustomXMLPart _xmlPart;

        public CustomXMLPart XMLPart
        {
            set
            {
                _xmlPart = value;
                OnPropertyChanged();
            }

            get { return _xmlPart; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}