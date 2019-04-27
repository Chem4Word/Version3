// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using Microsoft.Office.Interop.Word;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word.Navigator
{
    /// <summary>
    /// Interaction logic for NavigatorItemControl.xaml
    /// </summary>
    public partial class NavigatorItemControl : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public NavigatorItemControl()
        {
            InitializeComponent();
        }

        private void InsertLinkButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                if (Globals.Chem4WordV3.EventsEnabled)
                {
                    Globals.Chem4WordV3.EventsEnabled = false;
                    if (Globals.Chem4WordV3.Application.Documents.Count > 0)
                    {
                        if (ActiveDocument?.ActiveWindow?.Selection != null)
                        {
                            TaskPaneHelper.InsertChemistry(false, ActiveDocument.Application, Display);
                        }
                    }
                    Globals.Chem4WordV3.EventsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Globals.Chem4WordV3.EventsEnabled = true;
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void InsertCopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                if (Globals.Chem4WordV3.EventsEnabled)
                {
                    Globals.Chem4WordV3.EventsEnabled = false;
                    if (Globals.Chem4WordV3.Application.Documents.Count > 0)
                    {
                        if (ActiveDocument?.ActiveWindow?.Selection != null)
                        {
                            TaskPaneHelper.InsertChemistry(true, ActiveDocument.Application, Display);
                        }
                    }
                    Globals.Chem4WordV3.EventsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Globals.Chem4WordV3.EventsEnabled = true;
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public Document ActiveDocument
        {
            get { return (Document)GetValue(ActiveDocumentProperty); }
            set { SetValue(ActiveDocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveDocument.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveDocumentProperty =
            DependencyProperty.Register("ActiveDocument", typeof(Document), typeof(NavigatorItemControl), new PropertyMetadata(null));

        private void PreviousButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                PreviousControl();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void PreviousControl()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            NavigatorItem ni = DataContext as NavigatorItem;
            var currentSelPoint = ActiveDocument.ActiveWindow.Selection;
            try
            {
                var linkedControls = from Word.ContentControl cc in ActiveDocument.ContentControls
                                     orderby cc.Range.Start descending
                                     where CustomXmlPartHelper.GuidFromTag(cc?.Tag) == CustomXmlPartHelper.GetCmlId(ni.XMLPart)
                                     select cc;

                // Grab current selection point
                int current = currentSelPoint.Range.Start;
                foreach (Word.ContentControl cc in linkedControls)
                {
                    if (cc.Range.Start < current)
                    {
                        cc.Range.Select();
                        ActiveDocument.ActiveWindow.ScrollIntoView(cc.Range);
                        return;
                    }
                }

                // Fast Forward to end of document
                current = int.MaxValue;
                foreach (Word.ContentControl cc in linkedControls)
                {
                    if (cc.Range.Start < current)
                    {
                        cc.Range.Select();
                        ActiveDocument.ActiveWindow.ScrollIntoView(cc.Range);
                        return;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                NextControl();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void NextControl()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            NavigatorItem ni = DataContext as NavigatorItem;
            var currentSelPoint = ActiveDocument.ActiveWindow.Selection;
            try
            {
                var linkedControls = from Word.ContentControl cc in ActiveDocument.ContentControls
                                     orderby cc.Range.Start
                                     where CustomXmlPartHelper.GuidFromTag(cc?.Tag) == CustomXmlPartHelper.GetCmlId(ni.XMLPart)
                                     select cc;

                // Grab current selection point
                int current = currentSelPoint.Range.End;
                foreach (Word.ContentControl cc in linkedControls)
                {
                    if (cc.Range.Start > current)
                    {
                        cc.Range.Select();
                        ActiveDocument.ActiveWindow.ScrollIntoView(cc.Range);
                        return;
                    }
                }

                // Rewind to Start of document
                current = 0;
                foreach (Word.ContentControl cc in linkedControls)
                {
                    if (cc.Range.Start > current)
                    {
                        cc.Range.Select();
                        ActiveDocument.ActiveWindow.ScrollIntoView(cc.Range);
                        return;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void NavItemControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}