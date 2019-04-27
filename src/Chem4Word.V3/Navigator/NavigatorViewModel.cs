// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using Chem4Word.Model.Converters.CML;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ContentControl = Microsoft.Office.Interop.Word.ContentControl;

namespace Chem4Word.Navigator
{
    public class NavigatorViewModel : DependencyObject
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        //used for XAML data binding
        public ObservableCollection<NavigatorItem> NavigatorItems { get; }

        //references the custom XML parts in the document
        private CustomXMLParts _parts { get; }

        //local reference to the active document
        private Document _doc;

        public NavigatorViewModel()
        {
            NavigatorItems = new ObservableCollection<NavigatorItem>();
        }

        public NavigatorViewModel(Document doc) : this()
        {
            //get a reference to the document
            _doc = doc;
            _parts = _doc.CustomXMLParts.SelectByNamespace(CML.cml.NamespaceName);
            _parts.PartAfterAdd += OnPartAfterAdd;
            _parts.PartAfterLoad += OnPartAfterLoad;
            _parts.PartBeforeDelete += OnPartBeforeDelete;

            LoadModel();
        }

        /// <summary>
        /// Loads up the model initially from the document
        /// </summary>
        private void LoadModel()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                var converter = new CMLConverter();
                if (NavigatorItems.Any())
                {
                    NavigatorItems.Clear();
                }
                if (_doc != null)
                {
                    Dictionary<string, int> added = new Dictionary<string, int>();

                    var navItems = from ContentControl ccs in _doc.ContentControls
                                   join CustomXMLPart part in _parts
                                     on CustomXmlPartHelper.GuidFromTag(ccs?.Tag) equals CustomXmlPartHelper.GetCmlId(part)
                                   orderby ccs.Range.Start
                                   let chemModel = converter.Import(part.XML)
                                   select new NavigatorItem() { CMLId = ccs?.Tag, ChemicalStructure = part.XML, XMLPart = part, Name = chemModel.ConciseFormula };

                    foreach (NavigatorItem navigatorItem in navItems)
                    {
                        string guid = CustomXmlPartHelper.GuidFromTag(navigatorItem.CMLId);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            if (!added.ContainsKey(guid))
                            {
                                NavigatorItems.Add(navigatorItem);
                                added.Add(guid, 1);
                            }
                        }
                    }

                    Debug.WriteLine("Number of items loaded = {0}", NavigatorItems.Count);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        /// handles deletion of an XML Part...removes the corresponding navigator item
        /// </summary>
        /// <param name="OldPart">The custom XML part that gets deleted</param>
        private void OnPartBeforeDelete(CustomXMLPart OldPart)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                var oldPart = NavigatorItems.FirstOrDefault(ni => CustomXmlPartHelper.GetCmlId(ni.XMLPart) == CustomXmlPartHelper.GetCmlId(OldPart));
                NavigatorItems.Remove(oldPart);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        /// Occurs after a new custom XMl part is loaded into the document
        /// Useful for updating the Navigator
        /// </summary>
        /// <param name="NewPart"></param>
        private void OnPartAfterLoad(CustomXMLPart NewPart)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                var converter = new CMLConverter();
                //get the chemistry
                var chemModel = converter.Import(NewPart.XML);
                //find out which content control macthes the custom XML part
                try
                {
                    // ReSharper disable once InconsistentNaming
                    var matchingCC = (from ContentControl cc in _doc.ContentControls
                                      orderby cc.Range.Start
                                      where CustomXmlPartHelper.GuidFromTag(cc?.Tag) == CustomXmlPartHelper.GetCmlId(NewPart)
                                      select cc).First();

                    //get the ordinal position of the content control
                    int start = 0;
                    foreach (ContentControl cc in _doc.ContentControls)
                    {
                        if (cc.ID == matchingCC.ID)
                        {
                            break;
                        }
                        start += 1;
                    }

                    //insert the new navigator item at the ordinal position
                    var newNavItem = new NavigatorItem()
                    {
                        CMLId = matchingCC?.Tag,
                        ChemicalStructure = NewPart.XML,
                        XMLPart = NewPart,
                        Name = chemModel.ConciseFormula
                    };
                    try
                    {
                        NavigatorItems.Insert(start, newNavItem);
                    }
                    catch (ArgumentOutOfRangeException) //can happen when there are more content controls than navigator items
                    {
                        //so simply insert the new navigator item at the end
                        NavigatorItems.Add(newNavItem);
                    }
                }
                catch (InvalidOperationException)
                {
                    //sequence contains no elements - thrown on close
                    //just ignore
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnPartAfterAdd(CustomXMLPart NewPart)
        {
        }

        public ListBoxItem SelectedItem
        {
            get { return (ListBoxItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ListBoxItem), typeof(NavigatorViewModel), new PropertyMetadata(null));
    }
}