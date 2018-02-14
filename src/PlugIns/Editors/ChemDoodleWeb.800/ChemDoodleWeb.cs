// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using IChem4Word.Contracts;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.Editor.ChemDoodleWeb800
{
    public partial class ChemDoodleWeb : Form, IMessageFilter
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private const int WM_KEYDOWN = 0x0100;

        private string ms_AppTitle = "Chem4Word Editor - Powered By ChemDoodle Web V";

        public System.Windows.Point TopLeft { get; set; }

        public IChem4WordTelemetry Telemetry { get; set; }

        public string ProductAppDataPath { get; set; }

        public Options UserOptions { get; set; }

        public string Before_JSON { get; set; }

        public string Before_Formula { get; set; }

        public string After_JSON { get; set; }

        public string After_Formula { get; set; }

        public bool IsSingleMolecule { get; set; }

        public double AverageBondLength { get; set; }

        private string _tempJson = null;

        private bool _eventsEnabled = false;
        private bool _saveSettings = false;

        private Stopwatch _sw = new Stopwatch();

        public ChemDoodleWeb()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            bool handled = false;
            try
            {
                if (m.Msg == WM_KEYDOWN)
                {
                    //Debug.WriteLine("WM_KEYDOWN");
                    if ((int)Control.ModifierKeys == (int)Keys.Control)
                    {
                        //Debug.WriteLine("Contol Pressed");
                        Keys key = (Keys)(int)m.WParam & Keys.KeyCode;
                        switch (key)
                        {
                            #region Keys we are handling

                            // None

                            #endregion Keys we are handling

                            #region Keys we are supressing

                            case Keys.O: // Open File
                            case Keys.S: // Save File
                            case Keys.P: // Print
                                handled = true;
                                break;

                            #endregion Keys we are supressing

                            // Pass the rest through
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }

            return handled;
        }

        private void DelTree(string sPath)
        {
            DelTree(sPath, null);
        }

        private void DelTree(string sPath, List<string> listOfFilesToIgnore)
        {
            DirectoryInfo di = new DirectoryInfo(sPath);
            DirectoryInfo[] subdirs = di.GetDirectories();
            FileInfo[] filesList = di.GetFiles();
            foreach (FileInfo f in filesList)
            {
                if (listOfFilesToIgnore == null)
                {
                    File.Delete(f.FullName);
                }
                else
                {
                    if (!listOfFilesToIgnore.Contains(f.FullName))
                    {
                        File.Delete(f.FullName);
                    }
                }
            }
            foreach (DirectoryInfo subdir in subdirs)
            {
                DelTree(Path.Combine(sPath, subdir.ToString()), listOfFilesToIgnore);
            }
        }

        private void FormChemDoodleEditor_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _sw.Start();

                _eventsEnabled = false;

                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                this.Show();
                Application.DoEvents();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                string otherVersion = Path.Combine(ProductAppDataPath, "ChemDoodle-Web-702.txt");
                if (File.Exists(otherVersion))
                {
                    Telemetry.Write(module, "Information", "Deleting CDW 702 resources from disk");
                    File.Delete(otherVersion);
                    DelTree(Path.Combine(ProductAppDataPath, "ChemDoodleWeb"));
                }

                string markerFile = Path.Combine(ProductAppDataPath, "ChemDoodle-Web-800.txt");
                if (!File.Exists(markerFile))
                {
                    Telemetry.Write(module, "Information", "Writing resources to disk");
                    File.WriteAllText(markerFile, "Delete this file to refresh ChemDoodle Web");

                    Stream stream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.ChemDoodleWeb_800.zip");

                    // NB: Top level of zip file must be the folder ChemDoodleWeb
                    using (ZipFile zip = ZipFile.Read(stream))
                    {
                        zip.ExtractAll(ProductAppDataPath, ExtractExistingFileAction.OverwriteSilently);
                    }

                    string cssfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Chem4Word.css");
                    File.WriteAllText(Path.Combine(ProductAppDataPath, "Chem4Word.css"), cssfile);

                    string jsfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Chem4Word.js");
                    File.WriteAllText(Path.Combine(ProductAppDataPath, "Chem4Word.js"), jsfile);
                }

                Telemetry.Write(module, "Information", "Writing html to disk");
                string htmlfile = "";
                if (IsSingleMolecule)
                {
                    htmlfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Single.html");
                    chkSingleOrMany.Checked = false;
                    chkSingleOrMany.ImageIndex = 0;
                    toolTip1.SetToolTip(chkSingleOrMany, "Change to Multiple molecules mode");
                }
                else
                {
                    htmlfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Multi.html");
                    chkSingleOrMany.Checked = true;
                    chkSingleOrMany.ImageIndex = 1;
                    toolTip1.SetToolTip(chkSingleOrMany, "Change to Single molecule mode");
                }
                File.WriteAllText(Path.Combine(ProductAppDataPath, "Editor.html"), htmlfile);

                long sofar = sw.ElapsedMilliseconds;

                Telemetry.Write(module, "Timing", $"Writing resources to disk took {sofar}ms");

                _tempJson = Before_JSON;

                _eventsEnabled = true;

                Telemetry.Write(module, "Information", $"Starting browser");
                browser.Navigate(Path.Combine(ProductAppDataPath, "Editor.html"));
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                object obj = null;

                _eventsEnabled = false;

                long sofar = _sw.ElapsedMilliseconds;
                Telemetry.Write(module, "Timing", $"ChemDoodle Web loaded in {sofar.ToString("#,##0", CultureInfo.InvariantCulture)}ms");

                this.Text = ms_AppTitle + ExecuteJavaScript("GetVersion");

                // Send JSON to ChemDoodle
                ExecuteJavaScript("SetJSON", _tempJson, AverageBondLength);

                if (AverageBondLength < Constants.MinimumBondLength - Constants.BondLengthTolerance
                    || AverageBondLength > Constants.MaximumBondLength + Constants.BondLengthTolerance)
                {
                    nudBondLength.Value = (decimal)Constants.StandardBondLength;
                }
                else
                {
                    AverageBondLength = Math.Round(AverageBondLength / 5.0) * 5;
                    nudBondLength.Value = (int)AverageBondLength;
                }
                ExecuteJavaScript("ReScale", nudBondLength.Value);

                if (UserOptions.ShowHydrogens)
                {
                    ExecuteJavaScript("ShowHydrogens", true);
                    chkToggleShowHydrogens.Checked = true;
                }
                else
                {
                    ExecuteJavaScript("ShowHydrogens", false);
                    chkToggleShowHydrogens.Checked = false;
                }

                if (UserOptions.ColouredAtoms)
                {
                    ExecuteJavaScript("AtomsInColour", true);
                    chkColouredAtoms.Checked = true;
                }
                else
                {
                    ExecuteJavaScript("AtomsInColour", false);
                    chkColouredAtoms.Checked = false;
                }

                if (UserOptions.ShowCarbons)
                {
                    ExecuteJavaScript("ShowCarbons", true);
                    chkToggleShowCarbons.Checked = true;
                }
                else
                {
                    ExecuteJavaScript("ShowCarbons", false);
                    chkToggleShowCarbons.Checked = false;
                }

                obj = ExecuteJavaScript("GetFormula");
                if (obj != null)
                {
                    Before_Formula = obj.ToString();
                }

                long sofar2 = _sw.ElapsedMilliseconds;
                Telemetry.Write(module, "Timing", $"ChemDoodle Web ready in {sofar2.ToString("#,##0", CultureInfo.InvariantCulture)}ms");

                _eventsEnabled = true;
                _sw.Reset();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private object ExecuteJavaScript(string p_FunctionName, params object[] p_Args)
        {
            return browser.Document.InvokeScript(p_FunctionName, p_Args);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                object obj = null;

                obj = ExecuteJavaScript("GetFormula");
                if (obj != null)
                {
                    After_Formula = obj.ToString();
                }

                obj = ExecuteJavaScript("GetJSON");
                if (obj != null)
                {
                    string mol = obj.ToString();
                    if (!string.IsNullOrEmpty(mol))
                    {
                        JToken molJson = JObject.Parse(obj.ToString());
                        After_JSON = molJson.ToString();

                        DialogResult = DialogResult.OK;
                        Hide();
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                        Hide();
                    }
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                    Hide();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void FormChemDoodleEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (DialogResult == DialogResult.OK)
                {
                    if (_saveSettings)
                    {
                        SaveSettings();
                    }
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(UserOptions, Formatting.Indented);

            string fileName = $"{_product}.json";
            string optionsFile = Path.Combine(ProductAppDataPath, fileName);
            File.WriteAllText(optionsFile, json);
        }

        private void chkToggleShowHydrogens_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    if (chkToggleShowHydrogens.Checked)
                    {
                        ExecuteJavaScript("ShowHydrogens", true);
                        UserOptions.ShowHydrogens = true;
                    }
                    else
                    {
                        ExecuteJavaScript("ShowHydrogens", false);
                        UserOptions.ShowHydrogens = false;
                    }
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnAddExplicitHydrogens_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    ExecuteJavaScript("AddExplicitHydrogens");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnRemoveExplicitHydrogens_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    ExecuteJavaScript("RemoveHydrogens");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void nudBondLength_ValueChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", $"Triggered; New value {nudBondLength.Value}");
                    ExecuteJavaScript("ReScale", nudBondLength.Value);
                    AverageBondLength = (double)nudBondLength.Value;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    ExecuteJavaScript("Flip");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    ExecuteJavaScript("Mirror");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkSingleOrMany_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            _sw.Reset();
            _sw.Start();

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    if (IsSingleMolecule)
                    {
                        _tempJson = (string)ExecuteJavaScript("GetJSON");
                    }
                    else
                    {
                        _tempJson = (string)ExecuteJavaScript("GetFirstMolJSON");
                    }

                    if (chkSingleOrMany.Checked)
                    {
                        // Now in Multi molecules mode
                        chkSingleOrMany.ImageIndex = 1;
                        toolTip1.SetToolTip(chkSingleOrMany, "Change to Single molecule mode");
                        IsSingleMolecule = false;
                    }
                    else
                    {
                        // Now in Single molecule mode
                        chkSingleOrMany.ImageIndex = 0;
                        toolTip1.SetToolTip(chkSingleOrMany, "Change to Multiple molecules mode");
                        IsSingleMolecule = true;
                    }

                    string htmlfile = "";
                    if (IsSingleMolecule)
                    {
                        htmlfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Single.html");
                    }
                    else
                    {
                        htmlfile = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "ChemDoodleWeb.Multi.html");
                    }

                    File.WriteAllText(Path.Combine(ProductAppDataPath, "Editor.html"), htmlfile);
                    AverageBondLength = (double)nudBondLength.Value;
                    browser.Navigate(Path.Combine(ProductAppDataPath, "Editor.html"));
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkColouredAtoms_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    if (chkColouredAtoms.Checked)
                    {
                        UserOptions.ColouredAtoms = true;
                        ExecuteJavaScript("AtomsInColour", true);
                    }
                    else
                    {
                        UserOptions.ColouredAtoms = false;
                        ExecuteJavaScript("AtomsInColour", false);
                    }
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkToggleShowCarbons_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (_eventsEnabled)
                {
                    Telemetry.Write(module, "Action", "Triggered");
                    if (chkToggleShowCarbons.Checked)
                    {
                        UserOptions.ShowCarbons = true;
                        ExecuteJavaScript("ShowCarbons", true);
                    }
                    else
                    {
                        UserOptions.ShowCarbons = false;
                        ExecuteJavaScript("ShowCarbons", false);
                    }
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }
    }
}