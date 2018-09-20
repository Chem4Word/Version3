// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Wpf;
using Chem4Word.Model.Converters.Json;
using IChem4Word.Contracts;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Navigation;
using Chem4Word.Core.UI.Forms;
using Newtonsoft.Json;
using Control = System.Windows.Forms.Control;
using Path = System.IO.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace Chem4Word.Editor.ChemDoodleWeb800
{
    /// <summary>
    /// Interaction logic for WpfChemDoodle.xaml
    /// </summary>
    public partial class WpfChemDoodle : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private string AppTitle = "Chem4Word Editor - Powered By ChemDoodle Web V";

        private bool _loading;
        private bool _saveSettings;

        private Stopwatch _sw = new Stopwatch();

        public delegate void EventHandler(object sender, WpfEventArgs args);

        public event EventHandler OnButtonClick;

        public Point TopLeft { get; set; }

        public IChem4WordTelemetry Telemetry { get; set; }

        public string ProductAppDataPath { get; set; }

        public Options UserOptions { get; set; }

        public bool IsSingleMolecule { get; set; }

        public double AverageBondLength { get; set; }

        public string StructureJson { get; set; }

        public WpfChemDoodle()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _loading = true;
            InitializeComponent();
        }

        private void WpfChemDoodle_OnLoaded(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                _loading = true;
                _sw.Start();

                _sw.Start();

                DeployCdw800();
                SetupControls();
                LoadCdw();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void WebBrowser_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            var version = ExecuteJavaScript("GetVersion");
            var source = (HwndSource)PresentationSource.FromDependencyObject(WebBrowser);
            if (source != null)
            {
                var host = (System.Windows.Forms.Integration.ElementHost)Control.FromChildHandle(source.Handle);
                var form = (Form)host?.TopLevelControl;
                if (form != null)
                {
                    form.Text = AppTitle + version;
                }
            }

            // Send JSON to ChemDoodle before we do anything else
            ExecuteJavaScript("SetJSON", StructureJson, AverageBondLength);

            ExecuteJavaScript("ShowHydrogens", UserOptions.ShowHydrogens);
            ExecuteJavaScript("AtomsInColour", UserOptions.ColouredAtoms);
            ExecuteJavaScript("ShowCarbons", UserOptions.ShowCarbons);

            ExecuteJavaScript("ReScale", BondLength.Text);

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            _sw.Stop();

            Telemetry.Write(module, "Timing", $"ChemDoodle Web ready in {SafeDouble.Duration(_sw.ElapsedMilliseconds)}ms");

            _loading = false;
        }

        private void AddHydrogens_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("AddExplicitHydrogens");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void RemoveHydrogens_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("RemoveHydrogens");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void ShowHydrogens_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("ShowHydrogens", ShowHydrogens.IsChecked.Value);
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void ShowColour_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("AtomsInColour", ShowColour.IsChecked.Value);
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void FlipStructures_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("Flip");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void MirrorStructures_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("Mirror");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void ShowCarbons_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    ExecuteJavaScript("ShowCarbons", ShowCarbons.IsChecked);
                    _saveSettings = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void BondLength_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    int bondLength;
                    if (int.TryParse(BondLength.Text, out bondLength))
                    {
                        bondLength = (int)Math.Round(bondLength / 5.0) * 5;

                        if (bondLength < Constants.MinimumBondLength - Constants.BondLengthTolerance
                            || bondLength > Constants.MaximumBondLength + Constants.BondLengthTolerance)
                        {
                            bondLength = (int)Constants.StandardBondLength;
                        }

                        AverageBondLength = bondLength;
                        SetBondLength(bondLength);
                    }
                    else
                    {
                        AverageBondLength = Constants.StandardBondLength;
                        SetBondLength((int)AverageBondLength);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void IncreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    int bondLength;
                    if (int.TryParse(BondLength.Text, out bondLength))
                    {
                        if (bondLength < Constants.MaximumBondLength)
                        {
                            bondLength += 5;
                            AverageBondLength = bondLength;
                            SetBondLength(bondLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void DecreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    int bondLength;
                    if (int.TryParse(BondLength.Text, out bondLength))
                    {
                        if (bondLength > Constants.MinimumBondLength)
                        {
                            bondLength -= 5;
                            AverageBondLength = bondLength;
                            SetBondLength(bondLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (_saveSettings)
                {
                    SaveSettings();
                }

                WpfEventArgs args = new WpfEventArgs();

                // Set defaults if fetch fails
                args.OutputValue = "";
                args.Button = "Cancel";

                object obj = ExecuteJavaScript("GetJSON");
                if (obj != null)
                {
                    string mol = obj.ToString();
                    if (!string.IsNullOrEmpty(mol))
                    {
                        args.OutputValue = mol;
                        args.Button = "OK";
                    }
                }

                OnButtonClick?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                WpfEventArgs args = new WpfEventArgs();
                args.OutputValue = "";
                args.Button = "Cancel";

                OnButtonClick?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SwitchToMulti_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    object obj = ExecuteJavaScript("GetJSON");
                    if (obj != null)
                    {
                        string mol = obj.ToString();
                        if (!string.IsNullOrEmpty(mol))
                        {
                            JSONConverter jc = new JSONConverter();
                            Model.Model model = jc.Import(mol);
                            StructureJson = jc.Export(model);
                            SwapMode();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SwitchToSingle_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_loading)
                {
                    object obj1 = ExecuteJavaScript("GetJSON");
                    object obj2 = ExecuteJavaScript("GetFirstMolJSON");
                    if (obj1 != null && obj2 != null)
                    {
                        string mol1 = obj1.ToString();
                        string mol2 = obj2.ToString();
                        if (!string.IsNullOrEmpty(mol1) && !string.IsNullOrEmpty(mol2))
                        {
                            JSONConverter jc = new JSONConverter();
                            Model.Model model1 = jc.Import(mol1);
                            Model.Model model2 = jc.Import(mol2);

                            if (model1.Molecules.Count == 1)
                            {
                                StructureJson = jc.Export(model1);
                                SwapMode();
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine($"Warning your structure '{model1.ConciseFormula}' contains more than one molecule.");
                                sb.AppendLine($"Only the first molecule '{model2.ConciseFormula}' will be retained.");
                                sb.AppendLine("    Do you wish to continue?");
                                DialogResult dr = UserInteractions.AskUserYesNo(sb.ToString());
                                if (dr == DialogResult.Yes)
                                {
                                    StructureJson = jc.Export(model2);
                                    SwapMode();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void LoadCdw()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Stopwatch sw = new Stopwatch();
            sw.Start();

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

            Telemetry.Write(module, "Timing", $"Writing resources to disk took {SafeDouble.Duration(sw.ElapsedMilliseconds)}ms");

            Telemetry.Write(module, "Information", "Starting browser");
            WebBrowser.Navigate(Path.Combine(ProductAppDataPath, "Editor.html"));
        }

        private void SetupControls()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            ShowHydrogens.IsChecked = UserOptions.ShowHydrogens;
            ShowColour.IsChecked = UserOptions.ColouredAtoms;
            ShowCarbons.IsChecked = UserOptions.ShowCarbons;

            if (AverageBondLength < Constants.MinimumBondLength - Constants.BondLengthTolerance
                || AverageBondLength > Constants.MaximumBondLength + Constants.BondLengthTolerance)
            {
                BondLength.Text = Constants.StandardBondLength.ToString("0");
                AverageBondLength = Constants.StandardBondLength;
            }
            else
            {
                AverageBondLength = Math.Round(AverageBondLength / 5.0) * 5;
                BondLength.Text = AverageBondLength.ToString("0");
            }

            if (IsSingleMolecule)
            {
                SwitchToMulti.Visibility = Visibility.Visible;
                SwitchToSingle.Visibility = Visibility.Collapsed;
            }
            else
            {
                SwitchToMulti.Visibility = Visibility.Collapsed;
                SwitchToSingle.Visibility = Visibility.Visible;
            }
        }

        private void DeployCdw800()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

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

            sw.Stop();

            Telemetry.Write(module, "Timing", $"Writing resources to disk took {SafeDouble.Duration(sw.ElapsedMilliseconds)}ms");
        }

        private void SwapMode()
        {
            if (IsSingleMolecule)
            {
                SwitchToMulti.Visibility = Visibility.Collapsed;
                SwitchToSingle.Visibility = Visibility.Visible;
                IsSingleMolecule = false;
            }
            else
            {
                SwitchToMulti.Visibility = Visibility.Visible;
                SwitchToSingle.Visibility = Visibility.Collapsed;
                IsSingleMolecule = true;
            }

            LoadCdw();
        }

        private void SetBondLength(int value)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            ExecuteJavaScript("ReScale", value);
            BondLength.Text = $"{value}";
        }

        private void SaveSettings()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string json = JsonConvert.SerializeObject(UserOptions, Formatting.Indented);

            string fileName = $"{_product}.json";
            string optionsFile = Path.Combine(ProductAppDataPath, fileName);
            File.WriteAllText(optionsFile, json);
        }

        private object ExecuteJavaScript(string functionName, params object[] args)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            return WebBrowser.InvokeScript(functionName, args);
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
    }
}