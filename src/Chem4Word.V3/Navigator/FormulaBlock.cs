// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Chem4Word.Navigator
{
    public class FormulaBlock : TextBlock
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public string Formula
        {
            get { return (string)GetValue(FormulaProperty); }
            set { SetValue(FormulaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormulaProperty =
            DependencyProperty.Register("Formula", typeof(string), typeof(FormulaBlock), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, FormulaChangedCallback));

        private static void FormulaChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                FormulaBlock fb = d as FormulaBlock;

                string newFormula = (string)args.NewValue;

                var parts = Chem4Word.Helpers.FormulaHelper.Parts(newFormula);

                foreach (FormulaPart formulaPart in parts)
                {
                    //add in the new element

                    Run atom = new Run(formulaPart.Atom);
                    fb.Inlines.Add(atom);

                    if (formulaPart.Count > 1)
                    {
                        Run subs = new Run(formulaPart.Count.ToString());

                        subs.BaselineAlignment = BaselineAlignment.Subscript;
                        subs.FontSize = subs.FontSize - 2;
                        fb.Inlines.Add(subs);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}