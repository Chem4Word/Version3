// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Model;
using Chem4Word.Model.Converters;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChemistryModel = Chem4Word.Model.Model;

namespace Chem4Word.Controls
{
    /// <summary>
    /// Interaction logic for FlexDisplay.xaml
    /// </summary>
    [ToolboxItem(false)]
    public partial class FlexDisplay : UserControl
    {
        public FlexDisplay()
        {
            InitializeComponent();
        }

        #region Public Properties

        public bool ShowCarbonLabels
        {
            get { return (bool)GetValue(ShowCarbonLabelsProperty); }
            set { SetValue(ShowCarbonLabelsProperty, value); }
        }

        public static readonly DependencyProperty ShowCarbonLabelsProperty
            = DependencyProperty.Register("ShowCarbonLabels", typeof(bool), typeof(FlexDisplay),
                new PropertyMetadata(default(bool)));

        #region Chemistry (DependencyProperty)

        public object Chemistry
        {
            get { return (object)GetValue(ChemistryProperty); }
            set { SetValue(ChemistryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Chemistry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChemistryProperty =
            DependencyProperty.Register("Chemistry", typeof(object), typeof(FlexDisplay),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure, ChemistryChanged));

        private static void ChemistryChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var view = source as FlexDisplay;
            if (view == null)
            {
                return;
            }

            view.HandleDataContextChanged();
        }

        #endregion Chemistry (DependencyProperty)

        #region ChemistryWidth (DependencyProperty)

        public double ChemistryWidth
        {
            get { return (double)GetValue(ChemistryWidthProperty); }
            set { SetValue(ChemistryWidthProperty, value); }
        }

        public static readonly DependencyProperty ChemistryWidthProperty = DependencyProperty.Register(
            "ChemistryWidth", typeof(double), typeof(FlexDisplay), new PropertyMetadata(default(double)));

        #endregion ChemistryWidth (DependencyProperty)

        #region ChemistryHeight (DependencyProperty)

        public double ChemistryHeight
        {
            get { return (double)GetValue(ChemistryHeightProperty); }
            set { SetValue(ChemistryHeightProperty, value); }
        }

        public static readonly DependencyProperty ChemistryHeightProperty = DependencyProperty.Register(
            "ChemistryHeight", typeof(double), typeof(FlexDisplay), new PropertyMetadata(default(double)));

        #endregion ChemistryHeight (DependencyProperty)

        #endregion Public Properties

        #region Private Methods

        private void HandleDataContextChanged()
        {
            ChemistryModel chemistryModel = null;

            if (Chemistry is string)
            {
                var data = Chemistry as string;
                if (!string.IsNullOrEmpty(data))
                {
                    if (data.StartsWith("<"))
                    {
                        var conv = new CMLConverter();
                        chemistryModel = conv.Import(data);
                    }
                    if (data.Contains("M  END"))
                    {
                        var conv = new SdFileConverter();
                        chemistryModel = conv.Import(data);
                    }
                }
            }
            else
            {
                if (Chemistry != null && !(Chemistry is ChemistryModel))
                {
                    throw new ArgumentException("Object must be of type 'Chem4Word.Model.Model'.");
                }
                chemistryModel = Chemistry as ChemistryModel;
            }

            if (chemistryModel != null)
            {
                if (chemistryModel.AllAtoms.Count > 0)
                {
                    chemistryModel.RescaleForXaml(Constants.StandardBondLength * 2);

                    Debug.WriteLine($"Ring count == {chemistryModel.Molecules.SelectMany(m => m.Rings).Count()}");

                    if (ShowCarbonLabels)
                    {
                        foreach (var atom in chemistryModel.AllAtoms)
                        {
                            if (atom.Element.Equals(Globals.PeriodicTable.C))
                            {
                                atom.ShowSymbol = true;
                            }
                        }
                    }
                    Placeholder.DataContext = chemistryModel;
                }
                else
                {
                    Placeholder.DataContext = null;
                }
            }
            else
            {
                Placeholder.DataContext = null;
            }
        }

        #endregion Private Methods

        #region Private EventHandlers

        /// <summary>
        /// Add this to the OnMouseLeftButtonDown attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dynamic clobberedElement = sender;
            UserInteractions.InformUser(clobberedElement.ID);
        }

        #endregion Private EventHandlers
    }
}