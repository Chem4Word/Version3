﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using Chem4Word.Library;
using Chem4Word.Model;
using Chem4Word.Model.Converters;
using Chem4Word.Model.Geometry;
using Chem4Word.Navigator;
using Chem4Word.UI;
using Chem4Word.WebServices;
using IChem4Word.Contracts;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Chem4Word.Telemetry;
using CustomTaskPane = Microsoft.Office.Tools.CustomTaskPane;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word
{
    public partial class CustomRibbon
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private static object _missing = Type.Missing;

        /*
            Notes :-
            Custom Ribbon Help for Office 2010 VSTO Add-Ins
            http://www.codeproject.com/Articles/463282/Custom-Ribbon-Help-for-Office-VSTO-Add-ins
        */

        private void CustomRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Chem4Word.Chem4WordV3.Ribbon = this;
                RibbonTab tab = this.Tabs[0];

                string tabLabel = "Chemistry";
#if DEBUG
                tabLabel += " (Debug 3.0)";
#endif
                if (Globals.Chem4WordV3.WordVersion == 2013)
                {
                    tab.Label = tabLabel.ToUpper();
                }
                else
                {
                    tab.Label = tabLabel;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnRenderAsButtonClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                Word.Application app = Globals.Chem4WordV3.Application;
                Word.Document doc = app.ActiveDocument;
                Word.ContentControl cc = null;

                try
                {
                    RibbonButton b = sender as RibbonButton;
                    Debug.WriteLine($"User chose {b.Tag}");

                    Word.Selection sel = app.Selection;

                    CustomXMLPart customXmlPart = null;

                    if (sel.ContentControls.Count > 0)
                    {
                        cc = sel.ContentControls[1];
                        //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            string chosenState = b.Tag.ToString();
                            string prefix = "2D";
                            string guid = cc?.Tag;
                            if (guid.Contains(":"))
                            {
                                prefix = cc?.Tag.Split(':')[0];
                                guid = cc?.Tag.Split(':')[1];
                            }

                            if (!prefix.Equals(chosenState))
                            {
                                // Stop Screen Updating and Disable Document Event Handlers
                                app.ScreenUpdating = false;
                                Globals.Chem4WordV3.DisableDocumentEvents(doc);

                                // Erase old CC
                                cc.LockContents = false;
                                cc.Range.Delete();
                                cc.Delete();

                                customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(guid, app.ActiveDocument);
                                if (customXmlPart != null)
                                {
                                    CMLConverter conv = new CMLConverter();
                                    Model.Model model = conv.Import(customXmlPart.XML);

                                    if (chosenState.Equals("2D"))
                                    {
                                        string bookmarkName = "C4W_" + guid;
                                        if (Globals.Chem4WordV3.SystemOptions == null)
                                        {
                                            Globals.Chem4WordV3.LoadOptions();
                                        }
                                        Globals.Chem4WordV3.SystemOptions.WordTopLeft = Globals.Chem4WordV3.WordTopLeft;
                                        IChem4WordRenderer renderer =
                                            Globals.Chem4WordV3.GetRendererPlugIn(
                                                Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                                        if (renderer != null)
                                        {
                                            renderer.Properties = new Dictionary<string, string>();
                                            renderer.Properties.Add("Guid", guid);
                                            renderer.Cml = customXmlPart.XML;

                                            string tempfileName = renderer.Render();
                                            if (File.Exists(tempfileName))
                                            {
                                                cc = Insert2D(doc, tempfileName, bookmarkName, guid);
                                            }
                                        }
                                        else
                                        {
                                            cc = null;
                                        }
                                    }
                                    else
                                    {
                                        bool isFormula = false;
                                        string text;
                                        if (chosenState.Equals("c0"))
                                        {
                                            Globals.Chem4WordV3.Telemetry.Write(module, "Information", "Render structure as Overall ConciseFormula");
                                            text = model.ConciseFormula;
                                            isFormula = true;
                                        }
                                        else
                                        {
                                            string source;
                                            text = GetInlineText(model, chosenState, ref isFormula, out source);
                                            Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Render structure as {source}");
                                        }

                                        cc = Insert1D(app, doc, text, isFormula, chosenState + ":" + guid);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Get out of here
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                finally
                {
                    // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                    app.ScreenUpdating = true;
                    Globals.Chem4WordV3.EnableDocumentEvents(doc);

                    if (cc != null)
                    {
                        app.Selection.SetRange(cc.Range.End, cc.Range.End);
                    }
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void AddDynamicMenuItems()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");
            try
            {
                ShowAsMenu.Items.Clear();

                Word.Application app = Globals.Chem4WordV3.Application;
                Word.Document doc = app.ActiveDocument;
                Word.Selection sel = app.Selection;
                Word.ContentControl cc = null;
                CustomXMLPart customXmlPart = null;

                if (sel.ContentControls.Count > 0)
                {
                    cc = sel.ContentControls[1];
                    //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                    if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                    {
                        string prefix = "2D";
                        if (cc.Tag.Contains(":"))
                        {
                            prefix = cc?.Tag.Split(':')[0];
                        }

                        // Add 2D menu Item
                        RibbonButton ribbonButton = this.Factory.CreateRibbonButton();
                        ribbonButton.Tag = "2D";
                        if (prefix.Equals(ribbonButton.Tag))
                        {
                            ribbonButton.Image = Properties.Resources.SmallTick;
                        }
                        ribbonButton.Label = "2D";
                        ribbonButton.SuperTip = "Render as 2D image";
                        ribbonButton.Click += OnRenderAsButtonClick;
                        ShowAsMenu.Items.Add(ribbonButton);

                        Word.Application app1 = Globals.Chem4WordV3.Application;
                        customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc?.Tag, app1.ActiveDocument);
                        if (customXmlPart != null)
                        {
                            string cml = customXmlPart.XML;
                            CMLConverter conv = new CMLConverter();
                            Model.Model model = conv.Import(cml);

                            if (model.Molecules.Count > 1)
                            {
                                // Concise Formula
                                ribbonButton = Factory.CreateRibbonButton();
                                ribbonButton.Tag = "c0";
                                if (prefix.Equals(ribbonButton.Tag))
                                {
                                    ribbonButton.Image = Properties.Resources.SmallTick;
                                }
                                ribbonButton.Label = model.ConciseFormula;
                                ribbonButton.SuperTip = "Render as concise formula";
                                ribbonButton.Click += OnRenderAsButtonClick;
                                ShowAsMenu.Items.Add(ribbonButton);
                            }

                            foreach (Molecule mol in model.Molecules)
                            {
                                RibbonSeparator separator = this.Factory.CreateRibbonSeparator();
                                ShowAsMenu.Items.Add(separator);

                                // Concise Formula
                                ribbonButton = Factory.CreateRibbonButton();
                                ribbonButton.Tag = $"{mol.Id}.f0";
                                if (prefix.Equals(ribbonButton.Tag))
                                {
                                    ribbonButton.Image = Properties.Resources.SmallTick;
                                }
                                ribbonButton.Label = mol.ConciseFormula;
                                ribbonButton.SuperTip = "Render as concise formula";
                                ribbonButton.Click += OnRenderAsButtonClick;
                                ShowAsMenu.Items.Add(ribbonButton);

                                // Other Formulae
                                foreach (Formula f in mol.Formulas)
                                {
                                    ribbonButton = this.Factory.CreateRibbonButton();
                                    ribbonButton.Tag = f.Id;
                                    if (prefix.Equals(ribbonButton.Tag))
                                    {
                                        ribbonButton.Image = Properties.Resources.SmallTick;
                                    }
                                    ribbonButton.Label = f.Inline;
                                    ribbonButton.SuperTip = "Render as formula";
                                    ribbonButton.Click += OnRenderAsButtonClick;
                                    ShowAsMenu.Items.Add(ribbonButton);
                                }

                                // Chemical Names

                                foreach (ChemicalName n in mol.ChemicalNames)
                                {
                                    ribbonButton = this.Factory.CreateRibbonButton();
                                    ribbonButton.Tag = n.Id;
                                    if (prefix.Equals(ribbonButton.Tag))
                                    {
                                        ribbonButton.Image = Properties.Resources.SmallTick;
                                    }
                                    ribbonButton.Label = n.Name;
                                    ribbonButton.SuperTip = "Render as name";
                                    ribbonButton.Click += OnRenderAsButtonClick;
                                    ShowAsMenu.Items.Add(ribbonButton);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnDrawOrEditClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                PerformEdit();
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnOptionsClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                try
                {
                    Settings optionsForm = new Settings();

                    if (Globals.Chem4WordV3.SystemOptions == null)
                    {
                        Globals.Chem4WordV3.LoadOptions();
                    }
                    Options tempOptions = Globals.Chem4WordV3.SystemOptions.Clone();

                    optionsForm.SystemOptions = tempOptions;
                    optionsForm.TopLeft = Globals.Chem4WordV3.WordTopLeft;

                    DialogResult dr = optionsForm.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Globals.Chem4WordV3.SystemOptions = tempOptions.Clone();
                        Globals.Chem4WordV3.Telemetry = new TelemetryWriter(Globals.Chem4WordV3.SystemOptions.TelemetryEnabled);
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        public static void InsertFile()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Word.Application app = Globals.Chem4WordV3.Application;
            Word.Document doc = app.ActiveDocument;
            Word.ContentControl cc = null;

            try
            {
                Globals.Chem4WordV3.EvaluateChemistryAllowed();
                if (Globals.Chem4WordV3.ChemistryAllowed)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("All molecule files (*.cml, *.mol, *.sdf)|*.cml;*.mol;*.sdf");
                    sb.Append("|CML molecule files (*.cml)|*.cml");
                    sb.Append("|MDL molecule files (*.mol, *.sdf)|*.mol;*.sdf");

                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = sb.ToString();

                    DialogResult dr = ofd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Importing file '{ofd.SafeFileName}'");
                        string fileType = Path.GetExtension(ofd.FileName).ToLower();
                        Model.Model model = null;
                        string mol = File.ReadAllText(ofd.FileName);
                        string cml = string.Empty;

                        switch (fileType)
                        {
                            case ".cml":
                                CMLConverter cmlConverter = new CMLConverter();
                                model = cmlConverter.Import(mol);
                                break;

                            case ".mol":
                            case ".sdf":
                                SdFileConverter sdFileConverter = new SdFileConverter();
                                model = sdFileConverter.Import(mol);
                                break;

                            default:
                                break;
                        }

                        if (model != null)
                        {
                            dr = DialogResult.OK;
                            if (model.AllErrors.Count > 0 || model.AllWarnings.Count > 0)
                            {
                                //Globals.Chem4WordV3.Telemetry.Write(module, "Exception(Data)", mol);
                                if (model.AllErrors.Count > 0)
                                {
                                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception(Data)", string.Join(Environment.NewLine, model.AllErrors));
                                }
                                if (model.AllWarnings.Count > 0)
                                {
                                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception(Data)", string.Join(Environment.NewLine, model.AllWarnings));
                                }

                                ImportErrors f = new ImportErrors();
                                f.TopLeft = Globals.Chem4WordV3.WordTopLeft;
                                f.Model = model;
                                dr = f.ShowDialog();
                            }

                            if (dr == DialogResult.OK)
                            {
                                double before = model.MeanBondLength;
                                if (before < Constants.MinimumBondLength - Constants.BondLengthTolerance
                                    || before > Constants.MaximumBondLength + Constants.BondLengthTolerance)
                                {
                                    model.ScaleToAverageBondLength(Constants.StandardBondLength);
                                    double after = model.MeanBondLength;
                                    Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Structure rescaled from {before.ToString("#0.00")} to {after.ToString("#0.00")}");
                                }

                                // Always generate new Guid on Import
                                model.CustomXmlPartGuid = Guid.NewGuid().ToString("N");

                                // Ensure each molecule has a Consise Furmula set
                                foreach (var molecule in model.Molecules)
                                {
                                    if (string.IsNullOrEmpty(molecule.ConciseFormula))
                                    {
                                        molecule.ConciseFormula = molecule.CalculatedFormula();
                                    }
                                }

                                CMLConverter cmlConverter = new CMLConverter();
                                cml = cmlConverter.Export(model);

                                #region Insert OoXml Drawing into document

                                app.ScreenUpdating = false;
                                Globals.Chem4WordV3.DisableDocumentEvents(doc);

                                string guidString = model.CustomXmlPartGuid;
                                string bookmarkName = "C4W_" + guidString;

                                if (Globals.Chem4WordV3.SystemOptions == null)
                                {
                                    Globals.Chem4WordV3.LoadOptions();
                                }
                                Globals.Chem4WordV3.SystemOptions.WordTopLeft = Globals.Chem4WordV3.WordTopLeft;
                                IChem4WordRenderer renderer =
                                    Globals.Chem4WordV3.GetRendererPlugIn(
                                        Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                                if (renderer == null)
                                {
                                    UserInteractions.WarnUser("Unable to find a Renderer Plug-In");
                                }
                                else
                                {
                                    renderer.Properties = new Dictionary<string, string>();
                                    renderer.Properties.Add("Guid", guidString);
                                    renderer.Cml = cml;

                                    string tempfileName = renderer.Render();

                                    if (File.Exists(tempfileName))
                                    {
                                        cc = Insert2D(doc, tempfileName, bookmarkName, guidString);

                                        doc.CustomXMLParts.Add(cml);

                                        try
                                        {
                                            // Delete the temporary file now we are finished with it
                                            File.Delete(tempfileName);
                                        }
                                        catch
                                        {
                                            // Not much we can do here
                                        }
                                    }
                                }

                                #endregion Insert OoXml Drawing into document
                            }
                        }
                        else
                        {
                            if (mol.ToLower().Contains("v3000"))
                            {
                                UserInteractions.InformUser("Sorry, V3000 molfiles are not supported");
                            }
                            else
                            {
                                Exception x = new Exception("Could not import file");
                                Globals.Chem4WordV3.Telemetry.Write(module, "Exception(Data)", mol);
                                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, x).ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    UserInteractions.InformUser("Can't insert chemistry here because " + Globals.Chem4WordV3.ChemistryProhibitedReason);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            finally
            {
                // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                app.ScreenUpdating = true;
                Globals.Chem4WordV3.EnableDocumentEvents(doc);

                if (cc != null)
                {
                    // Move selection point into the Content Control which was just inserted
                    app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                }
            }
        }

        public void ActivateChemistryTab()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                this.RibbonUI.ActivateTab(Chem4WordV3.ControlId.ToString());
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public bool BeforeButtonChecks(RibbonButton button)
        {
            if (Globals.Chem4WordV3.SystemOptions == null)
            {
                Globals.Chem4WordV3.LoadOptions();
            }
            return true;
        }

        public void AfterButtonChecks(RibbonButton button)
        {
            RegistryHelper.SendSetupActions();
            RegistryHelper.SendUpdateActions();

            if (Globals.Chem4WordV3.SystemOptions == null)
            {
                Globals.Chem4WordV3.LoadOptions();
            }
            UpdateHelper.CheckForUpdates(Globals.Chem4WordV3.SystemOptions.AutoUpdateFrequency);
        }

        private static List<string> GetUsed2D(Word.Document doc, string guidString)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            // Using $"{}" to coerce null to empty string
            List<string> targets = (from Word.ContentControl ccs in doc.ContentControls
                                    orderby ccs.Range.Start
                                    where $"{ccs.Title}" == Constants.ContentControlTitle & $"{ccs.Tag}".Equals(guidString)
                                    select ccs.Tag).ToList();
            return targets;
        }

        private static List<string> GetUsed1D(Word.Document doc, string guidString)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            // Using $"{}" to coerce null to empty string
            List<string> targets = (from Word.ContentControl ccs in doc.ContentControls
                                    orderby ccs.Range.Start
                                    where $"{ccs.Title}" == Constants.ContentControlTitle & $"{ccs.Tag}".Contains(guidString)
                                    select ccs.Tag).ToList();
            return targets;
        }

        private static void UpdateStructures(Word.Application app, Word.Document doc, Model.Model model,
            string guidString, string tempFilename)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            // Use LINQ to get a list of all our ContentControls
            // Using $"{}" to coerce null to empty string
            List<Word.ContentControl> targets = (from Word.ContentControl ccs in doc.ContentControls
                                                 orderby ccs.Range.Start
                                                 where $"{ccs.Title}" == Constants.ContentControlTitle & $"{ccs.Tag}".Contains(guidString)
                                                 select ccs).ToList();

            foreach (Word.ContentControl cc in targets)
            {
                string prefix = "";
                string ccTag = cc?.Tag;

                if (ccTag.Contains(":"))
                {
                    prefix = ccTag?.Split(':')[0];
                }

                if (ccTag.Equals(guidString))
                {
                    // Only 2D Structures if filename supplied
                    if (!string.IsNullOrEmpty(tempFilename))
                    {
                        string bookmarkName = "C4W_" + guidString;
                        Update2D(doc, cc, tempFilename, bookmarkName, guidString);
                    }
                }
                else
                {
                    // 1D Structures
                    if (prefix.Equals("c0"))
                    {
                        Update1D(app, cc, model.ConciseFormula, true, $"c0:{guidString}");
                    }
                    else
                    {
                        bool isFormula = false;
                        string source;
                        string text = GetInlineText(model, prefix, ref isFormula, out source);
                        Update1D(app, cc, text, isFormula, $"{prefix}:{guidString}");
                    }
                }
            }
        }

        public static string GetInlineText(Model.Model model, string prefix, ref bool isFormula, out string source)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string text = "";
            source = "";

            foreach (Molecule m in model.Molecules)
            {
                if (prefix.Equals($"{m.Id}.f0"))
                {
                    text = m.ConciseFormula;
                    source = "ConciseFormula";
                    isFormula = true;
                }

                // Only check formulae if necessary
                if (string.IsNullOrEmpty(text))
                {
                    foreach (Formula f in m.Formulas)
                    {
                        if (f.Id.Equals(prefix))
                        {
                            text = f.Inline;
                            if (!string.IsNullOrEmpty(f.Convention))
                            {
                                if (f.Convention.ToLower().Contains("formula"))
                                {
                                    source = f.Convention;
                                    isFormula = true;
                                }
                            }
                            break;
                        }
                    }
                }

                // Only check names if necessary
                if (string.IsNullOrEmpty(text))
                {
                    foreach (ChemicalName n in m.ChemicalNames)
                    {
                        if (n.Id.Equals(prefix))
                        {
                            text = n.Name;
                            source = n.DictRef;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(text))
                {
                    break; // Out of molecules loop
                }
            }

            // Handle not found gracefully
            if (string.IsNullOrEmpty(text))
            {
                text = $"Unable to find formula or name with id of '{prefix}'";
            }

            return text;
        }

        public static Word.ContentControl Insert2D(Word.Document doc, string tempfileName, string bookmarkName, string tag)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Word.ContentControl cc = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText,
                ref _missing);

            cc.Range.InsertFile(tempfileName, bookmarkName);
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                doc.Bookmarks[bookmarkName].Delete();
            }

            cc.Tag = tag;
            cc.Title = Constants.ContentControlTitle;
            cc.LockContents = true;

            return cc;
        }

        private static void Update2D(Word.Document doc, Word.ContentControl cc, string tempfileName, string bookmarkName, string tag)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            cc.LockContents = false;
            if (cc.Type == Word.WdContentControlType.wdContentControlPicture)
            {
                // Handle old Word 2007 style
                Word.Range range = cc.Range;
                cc.Delete();
                cc = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText, range);
                cc.Tag = tag;
                cc.Title = Constants.ContentControlTitle;
                cc.Range.Delete();
            }
            else
            {
                cc.Range.Delete();
            }

            cc.Range.InsertFile(tempfileName, bookmarkName);
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                doc.Bookmarks[bookmarkName].Delete();
            }

            cc.Tag = tag;
            cc.Title = Constants.ContentControlTitle;
            cc.LockContents = true;
        }

        public static Word.ContentControl Insert1D(Word.Application app, Word.Document doc, string text, bool isFormula, string tag)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Word.ContentControl cc = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText,
                ref _missing);

            bool existingState = app.AutoCorrect.CorrectSentenceCaps;
            app.AutoCorrect.CorrectSentenceCaps = false;

            SetRichText(cc, text, isFormula);

            app.AutoCorrect.CorrectSentenceCaps = existingState;
            cc.Tag = tag;
            cc.Title = Constants.ContentControlTitle;
            cc.LockContents = true;

            return cc;
        }

        private static void Update1D(Word.Application app, Word.ContentControl cc, string text, bool isFormula, string tag)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            cc.LockContents = false;
            cc.Range.Delete();

            bool existingState = app.AutoCorrect.CorrectSentenceCaps;
            app.AutoCorrect.CorrectSentenceCaps = false;

            SetRichText(cc, text, isFormula);

            app.AutoCorrect.CorrectSentenceCaps = existingState;
            cc.Tag = tag;
            cc.Title = Constants.ContentControlTitle;
            cc.LockContents = true;
        }

        private static void SetRichText(Word.ContentControl cc, string text, bool isFormula)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            if (isFormula)
            {
                Word.Range r = cc.Range;
                List<FormulaPart> parts = FormulaHelper.Parts(text);
                foreach (var part in parts)
                {
                    switch (part.Count)
                    {
                        case 0: // Seperator or multiplier
                        case 1: // No Subscript
                            if (!string.IsNullOrEmpty(part.Atom))
                            {
                                r.InsertAfter(part.Atom);
                                r.Font.Subscript = 0;
                                r.Start = cc.Range.End;
                            }
                            break;

                        default: // With Subscript
                            if (!string.IsNullOrEmpty(part.Atom))
                            {
                                r.InsertAfter(part.Atom);
                                r.Font.Subscript = 0;
                                r.Start = cc.Range.End;
                            }

                            if (part.Count > 0)
                            {
                                r.InsertAfter($"{part.Count}");
                                r.Font.Subscript = 1;
                                r.Start = cc.Range.End;
                            }
                            break;
                    }
                }
            }
            else
            {
                cc.Range.Text = text;
            }
        }

        public static void PerformEdit()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Word.Application app = Globals.Chem4WordV3.Application;
            Word.Document doc = app.ActiveDocument;
            Word.ContentControl cc = null;

            try
            {
                if (Globals.Chem4WordV3.SystemOptions == null)
                {
                    Globals.Chem4WordV3.LoadOptions();
                }

                Globals.Chem4WordV3.EvaluateChemistryAllowed();
                if (Globals.Chem4WordV3.ChemistryAllowed)
                {
                    CustomXMLPart customXmlPart = null;
                    string beforeCml = Properties.Resources.EmptyStructure_cml;

                    bool isNewDrawing = true;

                    Word.Selection sel = app.Selection;

                    if (sel.ContentControls.Count > 0)
                    {
                        cc = sel.ContentControls[1];
                        //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            Word.Application app1 = Globals.Chem4WordV3.Application;
                            customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc?.Tag, app1.ActiveDocument);
                            if (customXmlPart != null)
                            {
                                beforeCml = customXmlPart.XML;
                                isNewDrawing = false;
                            }
                        }
                        else
                        {
                            // Get out of here
                            return;
                        }
                    }

                    if (Globals.Chem4WordV3.SystemOptions == null)
                    {
                        Globals.Chem4WordV3.LoadOptions();
                    }
                    IChem4WordEditor editor =
                        Globals.Chem4WordV3.GetEditorPlugIn(Globals.Chem4WordV3.SystemOptions.SelectedEditorPlugIn);

                    if (editor == null)
                    {
                        UserInteractions.WarnUser("Unable to find an Editor Plug-In");
                    }
                    else
                    {
                        editor.Cml = beforeCml;
                        DialogResult chemEditorResult = editor.Edit();

                        if (chemEditorResult == DialogResult.OK)
                        {
                            // Stop Screen Updating and Disable Document Event Handlers
                            app.ScreenUpdating = false;
                            Globals.Chem4WordV3.DisableDocumentEvents(doc);

                            CMLConverter cmlConverter = new CMLConverter();
                            SdFileConverter molConverter = new SdFileConverter();

                            Model.Model beforeModel = cmlConverter.Import(beforeCml);
                            Model.Model afterModel = cmlConverter.Import(editor.Cml);

                            int matchedMolecules = 0;

                            #region Copy old formulae and labels to new model if molecule Id and Concise Formula match

                            foreach (Molecule beforeMolecule in beforeModel.Molecules)
                            {
                                string concise = beforeMolecule.ConciseFormula;
                                string molId = beforeMolecule.Id;

                                foreach (Molecule afterMolecule in afterModel.Molecules)
                                {
                                    if (afterMolecule.ConciseFormula.Equals(concise) && afterMolecule.Id.Equals(molId))
                                    {
                                        foreach (var formula in beforeMolecule.Formulas)
                                        {
                                            Formula f = new Formula();
                                            f.Id = formula.Id;
                                            f.Convention = formula.Convention;
                                            f.Inline = formula.Inline;
                                            afterMolecule.Formulas.Add(f);
                                        }
                                        foreach (var name in beforeMolecule.ChemicalNames)
                                        {
                                            ChemicalName n = new ChemicalName();
                                            n.Id = name.Id;
                                            n.DictRef = name.DictRef;
                                            n.Name = name.Name;
                                            afterMolecule.ChemicalNames.Add(n);
                                        }
                                        matchedMolecules++;
                                        break;
                                    }
                                }
                            }

                            #endregion Copy old formulae and labels to new model if molecule Id and Concise Formula match

                            bool showLabelEditor = afterModel.Molecules.Count != matchedMolecules;

                            #region ChemSpider Calls

                            int chemSpiderCalls = afterModel.Molecules.Count * 2;

                            Progress pb = new Progress();
                            pb.TopLeft = Globals.Chem4WordV3.WordTopLeft;
                            pb.Value = 0;
                            pb.Maximum = chemSpiderCalls;

                            foreach (Molecule mol in afterModel.Molecules)
                            {
                                Dictionary<string, string> synonyms = new Dictionary<string, string>();

                                // ChemSpider InChiKey (1.03) generator does not support 0 bonds or Elements > 104
                                List<Bond> nullBonds = mol.Bonds.Where(b => b.OrderValue != null && b.OrderValue.Value < 1).ToList();
                                int max = mol.Atoms.Max(x => ((Element)x.Element).AtomicNumber);
                                if (nullBonds.Any() || max >= 104)
                                {
                                    Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Not sending structure to ChemSpider; Null Bonds: {nullBonds?.Count} Max Atomic Number: {max}");
                                    synonyms.Add(Constants.ChemspiderInchiKeyName, "Not Requested");
                                }
                                else
                                {
                                    pb.Show();
                                    pb.Increment(1);
                                    pb.Message = $"Fetching InChiKey from ChemSpider for molecule {mol.Id}";

                                    Model.Model temp = new Model.Model();
                                    temp.Molecules.Add(mol);

                                    string afterMolFile = molConverter.Export(temp);
                                    mol.ConciseFormula = mol.CalculatedFormula();

                                    Chemspider cs = new Chemspider(Globals.Chem4WordV3.Telemetry);
                                    string inchiKey = cs.GetInchiKey(afterMolFile);

                                    pb.Increment(1);
                                    pb.Message = $"Fetching Synonyms from ChemSpider for molecule {mol.Id}";

                                    synonyms = cs.GetSynonyms(inchiKey);
                                    if (string.IsNullOrEmpty(inchiKey))
                                    {
                                        synonyms.Add(Constants.ChemspiderInchiKeyName, "Unknown");
                                    }
                                    else
                                    {
                                        synonyms.Add(Constants.ChemspiderInchiKeyName, inchiKey);
                                    }
                                }

                                foreach (KeyValuePair<string, string> kvp in synonyms)
                                {
                                    bool updated;
                                    switch (kvp.Key)
                                    {
                                        case Constants.ChemspiderFormulaName:
                                        case Constants.ChemSpiderSmilesName:
                                            updated = false;
                                            foreach (var formula in mol.Formulas)
                                            {
                                                if (formula.Convention.Equals(kvp.Key))
                                                {
                                                    formula.Inline = kvp.Value;
                                                    updated = true;
                                                    break;
                                                }
                                            }
                                            if (!updated)
                                            {
                                                Formula f = new Formula();
                                                f.Convention = kvp.Key;
                                                f.Inline = kvp.Value;
                                                mol.Formulas.Add(f);
                                            }
                                            break;

                                        case Constants.ChemspiderIdName:
                                        case Constants.ChemSpiderSynonymName:
                                        case Constants.ChemspiderInchiKeyName:
                                            updated = false;
                                            foreach (var name in mol.ChemicalNames)
                                            {
                                                if (name.DictRef.Equals(kvp.Key))
                                                {
                                                    name.Name = kvp.Value;
                                                    updated = true;
                                                    break;
                                                }
                                            }
                                            if (!updated)
                                            {
                                                ChemicalName n = new ChemicalName();
                                                n.DictRef = kvp.Key;
                                                n.Name = kvp.Value;
                                                mol.ChemicalNames.Add(n);
                                            }
                                            break;
                                    }
                                }
                            }

                            pb.Value = 0;
                            pb.Hide();
                            pb.Close();

                            #endregion ChemSpider Calls

                            string guidString;
                            string fullTag;

                            if (isNewDrawing)
                            {
                                guidString = Guid.NewGuid().ToString("N"); // No dashes
                                fullTag = guidString;
                            }
                            else
                            {
                                fullTag = cc?.Tag;
                                guidString = CustomXmlPartHelper.GuidFromTag(cc?.Tag);
                                if (string.IsNullOrEmpty(guidString))
                                {
                                    guidString = Guid.NewGuid().ToString("N"); // No dashes
                                }
                            }

                            afterModel.CustomXmlPartGuid = guidString;
                            afterModel.Relabel(true);

                            if (showLabelEditor)
                            {
                                EditLabels el = new EditLabels();
                                el.TopLeft = Globals.Chem4WordV3.WordTopLeft;
                                el.Cml = cmlConverter.Export(afterModel);
                                el.Used1D = GetUsed1D(doc, guidString);
                                if (afterModel.Molecules.Count > 1)
                                {
                                    el.Message = "Warning: At least one Concise formula has changed; Please correct or delete any labels as necessary!";
                                }
                                else
                                {
                                    el.Message = "Warning: Concise formula has changed; Please correct or delete any labels as necessary!";
                                }

                                // Show Label Editor
                                DialogResult dr = el.ShowDialog();
                                if (dr == DialogResult.OK)
                                {
                                    afterModel = cmlConverter.Import(el.Cml);
                                }
                            }

                            string afterCml = cmlConverter.Export(afterModel);

                            if (Globals.Chem4WordV3.SystemOptions == null)
                            {
                                Globals.Chem4WordV3.LoadOptions();
                            }
                            Globals.Chem4WordV3.SystemOptions.WordTopLeft = Globals.Chem4WordV3.WordTopLeft;
                            IChem4WordRenderer renderer =
                                Globals.Chem4WordV3.GetRendererPlugIn(
                                    Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                            if (renderer == null)
                            {
                                UserInteractions.WarnUser("Unable to find a Renderer Plug-In");
                            }
                            else
                            {
                                string tempfileName = null;

                                // Always render the file.
                                renderer.Properties = new Dictionary<string, string>();
                                renderer.Properties.Add("Guid", guidString);
                                renderer.Cml = afterCml;

                                tempfileName = renderer.Render();

                                if (!isNewDrawing)
                                {
                                    // Erase old CC
                                    cc.LockContents = false;
                                    Debug.WriteLine(cc.Type);
                                    if (cc.Type == Word.WdContentControlType.wdContentControlPicture)
                                    {
                                        cc.Range.InlineShapes[1].Delete();
                                    }
                                    else
                                    {
                                        cc.Range.Delete();
                                    }
                                    cc.Delete();
                                }

                                // Insert a new CC
                                cc = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText, ref _missing);
                                Debug.WriteLine("Inserted ContentControl " + cc.ID);

                                cc.Title = Constants.ContentControlTitle;
                                if (isNewDrawing)
                                {
                                    cc.Tag = guidString;
                                }
                                else
                                {
                                    cc.Tag = fullTag;
                                }

                                if (File.Exists(tempfileName))
                                {
                                    UpdateStructures(app, doc, afterModel, guidString, tempfileName);

                                    #region Replace CustomXMLPart with our new cml

                                    if (customXmlPart != null)
                                    {
                                        customXmlPart.Delete();
                                    }

                                    doc.CustomXMLParts.Add(afterCml);

                                    #endregion Replace CustomXMLPart with our new cml

                                    // Delete the temporary file now we are finished with it
                                    try
                                    {
                                        File.Delete(tempfileName);
                                    }
                                    catch
                                    {
                                        // Not much we can do here
                                    }
                                }
                                else
                                {
                                    if (isNewDrawing)
                                    {
                                        cc.Delete();
                                        cc = null;
                                    }
                                }

                                //Globals.Chem4WordV3.Diagnostics(doc, "After PerformEdit()");
                            }
                        }
                    }
                }
                else
                {
                    UserInteractions.InformUser("Can't edit chemistry here because " + Globals.Chem4WordV3.ChemistryProhibitedReason);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            finally
            {
                // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                app.ScreenUpdating = true;
                Globals.Chem4WordV3.EnableDocumentEvents(doc);

                if (cc != null)
                {
                    // Move selection point into the Content Control which was just edited or added
                    app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                    //Globals.Chem4WordV3.SelectChemistry(app.Selection);
                }
            }
        }

        private void OnViewCmlClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    Word.Application app = Globals.Chem4WordV3.Application;
                    Word.Selection sel = app.Selection;
                    Word.ContentControl cc = null;
                    CustomXMLPart customXmlPart = null;

                    if (sel.ContentControls.Count > 0)
                    {
                        cc = sel.ContentControls[1];
                        //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            Word.Application app1 = Globals.Chem4WordV3.Application;
                            customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc?.Tag, app1.ActiveDocument);
                            if (customXmlPart != null)
                            {
                                XmlViewer viewer = new XmlViewer();
                                viewer.TopLeft = Globals.Chem4WordV3.WordTopLeft;
                                viewer.XmlString = customXmlPart.XML;
                                viewer.ShowDialog();
                            }
                            app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnImportClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                InsertFile();
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnExportClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                ExportFile();
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void ExportFile()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Word.Application app = Globals.Chem4WordV3.Application;
                Word.Selection sel = app.Selection;
                Word.ContentControl cc = null;
                CustomXMLPart customXmlPart = null;

                if (sel.ContentControls.Count > 0)
                {
                    cc = sel.ContentControls[1];
                    //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                    if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                    {
                        Word.Application app1 = Globals.Chem4WordV3.Application;
                        customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc?.Tag, app1.ActiveDocument);
                        if (customXmlPart != null)
                        {
                            Model.Model m = new Model.Model();
                            CMLConverter cmlConverter = new CMLConverter();
                            m = cmlConverter.Import(customXmlPart.XML);
                            m.CustomXmlPartGuid = "";

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "CML molecule files (*.cml)|*.cml|MDL molecule files (*.mol, *.sdf)|*.mol;*.sdf";
                            DialogResult dr = sfd.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                FileInfo fi = new FileInfo(sfd.FileName);
                                Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Exporting to '{fi.Name}'");
                                string fileType = Path.GetExtension(sfd.FileName).ToLower();
                                switch (fileType)
                                {
                                    case ".cml":
                                        string temp = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                            + Environment.NewLine
                                            + cmlConverter.Export(m);
                                        File.WriteAllText(sfd.FileName, temp);
                                        break;

                                    case ".mol":
                                    case ".sdf":
                                        // https://www.chemaxon.com/marvin-archive/6.0.2/marvin/help/formats/mol-csmol-doc.html
                                        double before = m.MeanBondLength;
                                        // Set bond length to 1.54 angstroms (Å)
                                        m.ScaleToAverageBondLength(1.54);
                                        double after = m.MeanBondLength;
                                        Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Structure rescaled from {before.ToString("#0.00")} to {after.ToString("#0.00")}");
                                        SdFileConverter converter = new SdFileConverter();
                                        File.WriteAllText(sfd.FileName, converter.Export(m));
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnEditLabelsClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    Word.Application app = Globals.Chem4WordV3.Application;
                    Word.Document doc = app.ActiveDocument;
                    Word.Selection sel = app.Selection;
                    Word.ContentControl cc = null;
                    CustomXMLPart customXmlPart = null;

                    if (sel.ContentControls.Count > 0)
                    {
                        cc = sel.ContentControls[1];
                        //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc?.Tag + " Title: " + cc.Title);
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            Word.Application app1 = Globals.Chem4WordV3.Application;
                            string guid = CustomXmlPartHelper.GuidFromTag(cc?.Tag);

                            customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc?.Tag, app1.ActiveDocument);
                            if (customXmlPart != null)
                            {
                                string cml = customXmlPart.XML;

                                EditLabels f = new EditLabels();
                                f.TopLeft = Globals.Chem4WordV3.WordTopLeft;
                                f.Cml = cml;
                                f.Used1D = GetUsed1D(doc, guid);
                                f.Message = "";

                                DialogResult dr = f.ShowDialog();
                                if (dr == DialogResult.OK)
                                {
                                    customXmlPart.Delete();
                                    doc.CustomXMLParts.Add(f.Cml);

                                    CMLConverter conv = new CMLConverter();
                                    Model.Model model = conv.Import(f.Cml);
                                    UpdateStructures(app, doc, model, guid, "");

                                    app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnViewAsItemsLoading(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            BeforeButtonChecks(sender as RibbonButton);
            try
            {
                AddDynamicMenuItems();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            AfterButtonChecks(sender as RibbonButton);
        }

        private void OnSearchItemsLoading(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            BeforeButtonChecks(sender as RibbonButton);
            try
            {
                WebSearchMenu.Items.Clear();

                if (Globals.Chem4WordV3.Searchers != null)
                {
                    foreach (IChem4WordSearcher searcher in Globals.Chem4WordV3.Searchers.OrderBy(s => s.DisplayOrder))
                    {
                        RibbonButton ribbonButton = this.Factory.CreateRibbonButton();

                        ribbonButton.Label = searcher.ShortName;
                        ribbonButton.Tag = searcher.Name;
                        ribbonButton.SuperTip = searcher.Description;
                        ribbonButton.Image = searcher.Image;
                        ribbonButton.Click += OnSearcherClick;

                        WebSearchMenu.Items.Add(ribbonButton);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            AfterButtonChecks(sender as RibbonButton);
        }

        private void OnSearcherClick(object sender, RibbonControlEventArgs ribbonControlEventArgs)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                try
                {
                    RibbonButton clicked = sender as RibbonButton;
                    if (clicked != null)
                    {
                        IChem4WordSearcher searcher = Globals.Chem4WordV3.GetSearcherPlugIn(clicked.Tag);
                        if (searcher != null)
                        {
                            DialogResult dr = searcher.Search();
                            if (dr == DialogResult.OK)
                            {
                                Word.Application app = Globals.Chem4WordV3.Application;
                                Word.Document doc = app.ActiveDocument;
                                Word.Selection sel = app.Selection;
                                Word.ContentControl cc = null;

                                Model.Model model = null;
                                CMLConverter cmlConverter = new CMLConverter();
                                model = cmlConverter.Import(searcher.Cml);

                                if (model != null)
                                {
                                    double before = model.MeanBondLength;
                                    if (before < Constants.MinimumBondLength - Constants.BondLengthTolerance
                                        || before > Constants.MaximumBondLength + Constants.BondLengthTolerance)
                                    {
                                        model.ScaleToAverageBondLength(Constants.StandardBondLength);
                                        double after = model.MeanBondLength;
                                        Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Structure rescaled from {before.ToString("#0.00")} to {after.ToString("#0.00")}");
                                    }

                                    // Always generate new Guid on Import
                                    model.CustomXmlPartGuid = Guid.NewGuid().ToString("N");

                                    // Ensure each molecule has a Consise Furmula set
                                    foreach (var molecule in model.Molecules)
                                    {
                                        if (string.IsNullOrEmpty(molecule.ConciseFormula))
                                        {
                                            molecule.ConciseFormula = molecule.CalculatedFormula();
                                        }
                                    }

                                    string cml = cmlConverter.Export(model);

                                    #region Insert OoXml Drawing into document

                                    app.ScreenUpdating = false;
                                    Globals.Chem4WordV3.DisableDocumentEvents(doc);

                                    string guidString = model.CustomXmlPartGuid;
                                    string bookmarkName = "C4W_" + guidString;

                                    if (Globals.Chem4WordV3.SystemOptions == null)
                                    {
                                        Globals.Chem4WordV3.LoadOptions();
                                    }
                                    Globals.Chem4WordV3.SystemOptions.WordTopLeft = Globals.Chem4WordV3.WordTopLeft;
                                    IChem4WordRenderer renderer =
                                        Globals.Chem4WordV3.GetRendererPlugIn(
                                            Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                                    if (renderer == null)
                                    {
                                        UserInteractions.WarnUser("Unable to find a Renderer Plug-In");
                                    }
                                    else
                                    {
                                        renderer.Properties = new Dictionary<string, string>();
                                        renderer.Properties.Add("Guid", guidString);
                                        renderer.Cml = cml;

                                        string tempfileName = renderer.Render();

                                        if (File.Exists(tempfileName))
                                        {
                                            cc = Insert2D(doc, tempfileName, bookmarkName, guidString);

                                            doc.CustomXMLParts.Add(cml);

                                            try
                                            {
                                                // Delete the temporary file now we are finished with it
                                                File.Delete(tempfileName);
                                            }
                                            catch
                                            {
                                                // Not much we can do here
                                            }
                                        }
                                    }

                                    #endregion Insert OoXml Drawing into document
                                }
                                else
                                {
                                    Exception x = new Exception("Could not import search result, Model is null");
                                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft,
                                        module, x).ShowDialog();
                                }

                                app.ScreenUpdating = true;
                                Globals.Chem4WordV3.EnableDocumentEvents(doc);

                                if (cc != null)
                                {
                                    // Move selection point into the Content Control which was just inserted
                                    app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnSaveToLibraryClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    Word.Application app = Globals.Chem4WordV3.Application;
                    Word.Document doc = app.ActiveDocument;
                    Word.Selection sel = app.Selection;
                    Word.ContentControl cc = null;
                    CustomXMLPart customXmlPart = null;

                    if (sel.ContentControls.Count > 0)
                    {
                        Model.Model m = null;

                        cc = sel.ContentControls[1];
                        //Debug.WriteLine("Existing CC ID: " + cc.ID + " Tag: " + cc.Tag + " Title: " + cc.Title);
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            Word.Application app1 = Globals.Chem4WordV3.Application;

                            customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc.Tag, app1.ActiveDocument);
                            if (customXmlPart != null)
                            {
                                string cml = customXmlPart.XML;
                                m = new CMLConverter().Import(cml);
                                if (Globals.Chem4WordV3.LibraryNames == null)
                                {
                                    Globals.Chem4WordV3.LoadLibrary();
                                }
                                LibraryModel.ImportCml(cml);
                                Globals.Chem4WordV3.LibraryNames = LibraryModel.GetLibraryNames();
                            }

                            CustomTaskPane custTaskPane = null;
                            foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                            {
                                if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.LibraryTaskPaneTitle)
                                {
                                    custTaskPane = taskPane;
                                }
                            }

                            if (custTaskPane != null)
                            {
                                (custTaskPane.Control as LibraryHost)?.Refresh();
                            }

                            UserInteractions.InformUser($"Structure '{m?.ConciseFormula}' added into Library");
                            Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Structure '{m?.ConciseFormula}' added into Library");
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);
                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnNavigatorClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            BeforeButtonChecks(sender as RibbonButton);
            try
            {
                //see https://msdn.microsoft.com/en-us/library/bb608620(v=vs.100).aspx

                Debug.WriteLine($"OnNavigatorClick() {ShowNavigator.Checked}");

                Word.Application app = Globals.Chem4WordV3.Application;

                if (Globals.Chem4WordV3.EventsEnabled)
                {
                    app.System.Cursor = Word.WdCursorType.wdCursorWait;

                    if (app.Documents.Count > 0)
                    {
                        CustomTaskPane custTaskPane = null;
                        foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                        {
                            if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.NavigatorTaskPaneTitle)
                            {
                                custTaskPane = taskPane;
                            }
                        }

                        if (ShowNavigator.Checked)
                        {
                            if (custTaskPane == null)
                            {
                                custTaskPane =
                                    Globals.Chem4WordV3.CustomTaskPanes.Add(new NavigatorHost(app, app.ActiveDocument),
                                        Constants.NavigatorTaskPaneTitle, app.ActiveWindow);

                                custTaskPane.Width = Globals.Chem4WordV3.WordWidth / 4;
                                custTaskPane.VisibleChanged += OnNavigatorPaneVisibleChanged;
                            }
                            custTaskPane.Visible = true;
                            Globals.Chem4WordV3.EvaluateChemistryAllowed();
                        }
                        else
                        {
                            if (custTaskPane != null)
                            {
                                custTaskPane.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        ShowNavigator.Checked = false;
                    }

                    app.System.Cursor = Word.WdCursorType.wdCursorNormal;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }

            AfterButtonChecks(sender as RibbonButton);
        }

        private void OnNavigatorPaneVisibleChanged(object sender, EventArgs eventArgs)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"OnNavigatorPaneVisibleChanged() {ShowNavigator.Checked}");

                CustomTaskPane taskPane = sender as CustomTaskPane;
                Word.Application app = Globals.Chem4WordV3.Application;

                if (Globals.Chem4WordV3.EventsEnabled)
                {
                    if (taskPane != null)
                    {
                        Word.Window window = taskPane.Window;
                        if (window != null)
                        {
                            string taskdoc = window.Document.Name;
                            Debug.WriteLine(taskdoc);

                            if (taskdoc.Equals(app.ActiveDocument.Name))
                            {
                                Debug.WriteLine($"Navigator Visible: {taskPane.Visible}");
                                if (ShowNavigator.Checked != taskPane.Visible)
                                {
                                    ShowNavigator.Checked = taskPane.Visible;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnShowLibraryClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            BeforeButtonChecks(sender as RibbonButton);
            if (Globals.Chem4WordV3.LibraryNames == null)
            {
                Globals.Chem4WordV3.LoadLibrary();
            }
            try
            {
                // See https://msdn.microsoft.com/en-us/library/bb608590.aspx
                Word.Application app = Globals.Chem4WordV3.Application;
                using (new UI.WaitCursor())
                {
                    if (Globals.Chem4WordV3.EventsEnabled)
                    {
                        //app.System.Cursor = Word.WdCursorType.wdCursorWait;

                        if (app.Documents.Count > 0)
                        {
                            CustomTaskPane custTaskPane = null;
                            foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                            {
                                if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.LibraryTaskPaneTitle)
                                {
                                    custTaskPane = taskPane;
                                }
                            }

                            Globals.Chem4WordV3.LibraryState = ShowLibrary.Checked;
                            ShowLibrary.Label = ShowLibrary.Checked ? "Close" : "Open ";

                            if (ShowLibrary.Checked)
                            {
                                if (custTaskPane == null)
                                {
                                    custTaskPane =
                                        Globals.Chem4WordV3.CustomTaskPanes.Add(new LibraryHost(),
                                            Constants.LibraryTaskPaneTitle, app.ActiveWindow);

                                    // Opposite side to Navigator's default placement
                                    custTaskPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionLeft;

                                    custTaskPane.Width = Globals.Chem4WordV3.WordWidth / 4;
                                    custTaskPane.VisibleChanged += OnLibraryPaneVisibleChanged;
                                    (custTaskPane.Control as LibraryHost)?.Refresh();
                                }
                                custTaskPane.Visible = true;
                                Globals.Chem4WordV3.EvaluateChemistryAllowed();
                            }
                            else
                            {
                                if (custTaskPane != null)
                                {
                                    custTaskPane.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            ShowLibrary.Checked = false;
                        }
                    }
                    //app.System.Cursor = Word.WdCursorType.wdCursorNormal;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            AfterButtonChecks(sender as RibbonButton);
        }

        public void OnLibraryPaneVisibleChanged(object sender, EventArgs eventArgs)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"OnLibraryPaneVisibleChanged() {ShowLibrary.Checked}");

                Word.Application app = Globals.Chem4WordV3.Application;
                CustomTaskPane taskPane = sender as CustomTaskPane;

                if (Globals.Chem4WordV3.EventsEnabled)
                {
                    if (taskPane != null)
                    {
                        Word.Window window = taskPane.Window;
                        if (window != null)
                        {
                            string taskdoc = window.Document.Name;
                            Debug.WriteLine(taskdoc);
                            if (taskdoc.Equals(app.ActiveDocument.Name))
                            {
                                //Debug.WriteLine($"Library Visible: {taskPane.Visible}");
                                if (ShowLibrary.Checked != taskPane.Visible)
                                {
                                    ShowLibrary.Checked = taskPane.Visible;
                                }
                                if (ShowLibrary.Checked)
                                {
                                    (taskPane.Control as LibraryHost)?.Refresh();
                                }
                                ShowLibrary.Label = ShowLibrary.Checked ? "Close" : "Open";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnSeparateClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                Word.Application app = Globals.Chem4WordV3.Application;
                Word.Document doc = app.ActiveDocument;
                Word.ContentControl cc = null;

                // Stop Screen Updating and Disable Document Event Handlers
                app.ScreenUpdating = false;
                Globals.Chem4WordV3.DisableDocumentEvents(doc);

                try
                {
                    CustomXMLPart customXmlPart = null;
                    Word.Selection sel = app.Selection;

                    if (sel.ContentControls.Count > 0)
                    {
                        cc = sel.ContentControls[1];
                        if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                        {
                            string fullTag = cc.Tag;
                            string guidString = CustomXmlPartHelper.GuidFromTag(cc.Tag);

                            Word.Application app1 = Globals.Chem4WordV3.Application;
                            customXmlPart = CustomXmlPartHelper.GetCustomXmlPart(cc.Tag, app1.ActiveDocument);
                            if (customXmlPart != null)
                            {
                                string beforeCml = customXmlPart.XML;
                                CMLConverter cmlConverter = new CMLConverter();
                                Model.Model model = cmlConverter.Import(beforeCml);

                                Packer packer = new Packer();
                                packer.Model = model;

                                packer.Pack(model.MeanBondLength * 2);

                                string afterCml = cmlConverter.Export(model);

                                if (Globals.Chem4WordV3.SystemOptions == null)
                                {
                                    Globals.Chem4WordV3.LoadOptions();
                                }
                                IChem4WordRenderer renderer =
                                    Globals.Chem4WordV3.GetRendererPlugIn(
                                        Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                                if (renderer == null)
                                {
                                    UserInteractions.WarnUser("Unable to find a Renderer Plug-In");
                                }
                                else
                                {
                                    renderer.Properties = new Dictionary<string, string>();
                                    renderer.Properties.Add("Guid", guidString);
                                    renderer.Cml = afterCml;

                                    string tempfileName = renderer.Render();

                                    if (File.Exists(tempfileName))
                                    {
                                        cc.LockContents = false;
                                        cc.Range.Delete();
                                        cc.Delete();

                                        // Insert a new CC
                                        cc = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText, ref _missing);
                                        Debug.WriteLine("Inserted ContentControl " + cc.ID);

                                        cc.Title = Constants.ContentControlTitle;
                                        cc.Tag = fullTag;

                                        UpdateStructures(app, doc, model, guidString, tempfileName);

                                        customXmlPart.Delete();
                                        doc.CustomXMLParts.Add(afterCml);

                                        // Delete the temporary file now we are finished with it
                                        try
                                        {
                                            File.Delete(tempfileName);
                                        }
                                        catch
                                        {
                                            // Not much we can do here
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                finally
                {
                    // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                    app.ScreenUpdating = true;
                    Globals.Chem4WordV3.EnableDocumentEvents(doc);

                    if (cc != null)
                    {
                        // Move selection point into the Content Control which was just edited or added
                        app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                        //Globals.Chem4WordV3.SelectChemistry(app.Selection);
                    }
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnUpdateClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    if (Globals.Chem4WordV3.ThisVersion == null || Globals.Chem4WordV3.AllVersions == null)
                    {
                        using (new UI.WaitCursor())
                        {
                            UpdateHelper.FetchUpdateInfo();
                        }
                    }
                    UpdateHelper.ShowUpdateForm();
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnShowAboutClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    About fa = new About();
                    fa.TopLeft = Globals.Chem4WordV3.WordTopLeft;

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    UpdateHelper.ReadThisVersion(assembly);
                    if (Globals.Chem4WordV3.ThisVersion != null)
                    {
                        string[] parts = Globals.Chem4WordV3.ThisVersion.Root.Element("Number").Value.Split(' ');
                        string temp = Globals.Chem4WordV3.ThisVersion.Root.Element("Number").Value;
                        int idx = temp.IndexOf(" ");
                        fa.VersionString = $"Chem4Word V3 {temp.Substring(idx + 1)} [{fvi.FileVersion}]";
                    }
                    else
                    {
                        fa.VersionString = $"Chem4Word Version {fvi.FileVersion}";
                    }

                    fa.ShowDialog();
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnShowHomeClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                try
                {
                    Process.Start("https://www.chem4word.co.uk");
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnCheckForUpdatesClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);

                UpdateHelper.ClearSettings();

                if (Globals.Chem4WordV3.SystemOptions == null)
                {
                    Globals.Chem4WordV3.LoadOptions();
                }

                int behind = UpdateHelper.CheckForUpdates(Globals.Chem4WordV3.SystemOptions.AutoUpdateFrequency);
                if (behind == 0)
                {
                    UserInteractions.InformUser("Your version of Chem4Word is the latest");
                }

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnReadManualClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                try
                {
                    string userManual = Path.Combine(Globals.Chem4WordV3.AddInInfo.DeploymentPath, "Manual", "Chem4Word-Version3-User-Manual.docx");
                    if (File.Exists(userManual))
                    {
                        Globals.Chem4WordV3.Telemetry.Write(module, "ReadManual", userManual);
                        Globals.Chem4WordV3.Application.Documents.Open(userManual, ReadOnly: true);
                    }
                    else
                    {
                        userManual = Path.Combine(Globals.Chem4WordV3.AddInInfo.DeploymentPath, @"..\..\..\..\doc", "Chem4Word-Version3-User-Manual.docx");
                        if (File.Exists(userManual))
                        {
                            Globals.Chem4WordV3.Telemetry.Write(module, "ReadManual", userManual);
                            Globals.Chem4WordV3.Application.Documents.Open(userManual, ReadOnly: true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void OnYouTubeClick(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            BeforeButtonChecks(sender as RibbonButton);

            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            if (Globals.Chem4WordV3.EventsEnabled)
            {
                Globals.Chem4WordV3.EventsEnabled = false;

                BeforeButtonChecks(sender as RibbonButton);
                try
                {
                    Process.Start("https://www.youtube.com/channel/UCKX2kG9kZ3zoX0nCen5lfpQ");
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
                AfterButtonChecks(sender as RibbonButton);

                Globals.Chem4WordV3.EventsEnabled = true;
            }
        }

        private void ButtonsDisabled_Click(object sender, RibbonControlEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            BeforeButtonChecks(sender as RibbonButton);
            try
            {
                Globals.Chem4WordV3.EvaluateChemistryAllowed();
                UserInteractions.InformUser($"Chem4Word buttons are disabled because {Globals.Chem4WordV3.ChemistryProhibitedReason}");
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            AfterButtonChecks(sender as RibbonButton);
        }
    }
}