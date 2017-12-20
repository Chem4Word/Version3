// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Library;
using IChem4Word.Contracts;
using Microsoft.Office.Tools;
using Newtonsoft.Json;
using Ookii.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using static Chem4Word.Core.UserInteractions;

namespace Chem4Word.UI
{
    public partial class Settings : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public Options SystemOptions;

        private bool _dirty;

        public System.Windows.Point TopLeft { get; set; }

        public Settings()
        {
            InitializeComponent();
        }

        private string EnsureTrailingSlash(string input)
        {
            string output = input;

            if (!output.EndsWith("/"))
            {
                output = input + "/";
            }

            return output;
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                SaveChanges();
                _dirty = false;
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SaveChanges()
        {
            SystemOptions.ChemSpiderRdfServiceUri = EnsureTrailingSlash(SystemOptions.ChemSpiderRdfServiceUri);
            SystemOptions.ChemSpiderWebServiceUri = EnsureTrailingSlash(SystemOptions.ChemSpiderWebServiceUri);

            string json = JsonConvert.SerializeObject(SystemOptions, Formatting.Indented);

            string padPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
            string fileName = $"{Globals.Chem4WordV3.AddInInfo.ProductName}.json";
            string optionsFile = Path.Combine(padPath, fileName);
            File.WriteAllText(optionsFile, json);
        }

        private void FormOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (_dirty)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Do you wish to save your changes?");
                    sb.AppendLine("  Click 'Yes' to save your changes and exit.");
                    sb.AppendLine("  Click 'No' to discard your changes and exit.");
                    sb.AppendLine("  Click 'Cancel' to return to the form.");
                    DialogResult dr = AskUserYesNoCancel(sb.ToString());
                    switch (dr)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;

                        case DialogResult.Yes:
                            SaveChanges();
                            DialogResult = DialogResult.OK;
                            break;

                        case DialogResult.No:
                            DialogResult = DialogResult.Cancel;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                LoadSettings();

                // Remove Tabs for Professional Features
                tabControlEx1.TabPages.Remove(tabTelemetry);
                tabControlEx1.TabPages.Remove(tabUpdates);
                chkUseWebServices.Visible = false;
                lblProWebServices.Visible = false;

                _dirty = false;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void LoadSettings()
        {
            cboEditors.Items.Clear();
            cboRenderers.Items.Clear();
            cboSearchers.Items.Clear();
            btnEditorSettings.Enabled = false;
            btnRendererSettings.Enabled = false;
            btnSearcherSettings.Enabled = false;

            string selectedEditor = SystemOptions.SelectedEditorPlugIn;
            foreach (IChem4WordEditor editor in Globals.Chem4WordV3.Editors)
            {
                PlugInComboItem pci = new PlugInComboItem()
                {
                    Name = editor.Name,
                    Description = editor.Description
                };
                int item = cboEditors.Items.Add(pci);
                if (editor.Name.Equals(selectedEditor))
                {
                    btnEditorSettings.Enabled = editor.HasSettings;
                    cboEditors.SelectedIndex = item;
                }
            }

            string selectedRenderer = SystemOptions.SelectedRendererPlugIn;
            foreach (IChem4WordRenderer renderer in Globals.Chem4WordV3.Renderers)
            {
                PlugInComboItem pci = new PlugInComboItem()
                {
                    Name = renderer.Name,
                    Description = renderer.Description
                };
                int item = cboRenderers.Items.Add(pci);
                if (renderer.Name.Equals(selectedRenderer))
                {
                    btnRendererSettings.Enabled = renderer.HasSettings;
                    cboRenderers.SelectedIndex = item;
                }
            }

            foreach (IChem4WordSearcher searcher in Globals.Chem4WordV3.Searchers.OrderBy(s => s.DisplayOrder))
            {
                PlugInComboItem pci = new PlugInComboItem()
                {
                    Name = searcher.Name,
                    Description = searcher.Description
                };
                int item = cboSearchers.Items.Add(pci);
                if (cboSearchers.Items.Count == 1)
                {
                    btnSearcherSettings.Enabled = searcher.HasSettings;
                    cboSearchers.SelectedIndex = item;
                }
            }

            chkUseWebServices.Checked = SystemOptions.UseWebServices;
            txtChemSpiderRdfUri.Text = SystemOptions.ChemSpiderRdfServiceUri;
            txtChemSpiderWsUri.Text = SystemOptions.ChemSpiderWebServiceUri;

            chkTelemetryEnabled.Checked = SystemOptions.TelemetryEnabled;
            cboUpdateFrequency.Items.Clear();
            cboUpdateFrequency.Items.Add("Daily");
            cboUpdateFrequency.Items.Add("Weekly");
            switch (SystemOptions.AutoUpdateFrequency)
            {
                case 1:
                    cboUpdateFrequency.SelectedIndex = 0;
                    break;

                case 7:
                    cboUpdateFrequency.SelectedIndex = 1;
                    break;

                default:
                    cboUpdateFrequency.SelectedIndex = 0;
                    break;
            }
        }

        private void OnSetDefaultsClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                DialogResult dr = UserInteractions.AskUserOkCancel("Restore default settings");
                if (dr == DialogResult.OK)
                {
                    SystemOptions.RestoreDefaults();
                    LoadSettings();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnEditorsSelectedIndexChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                PlugInComboItem pci = cboEditors.SelectedItem as PlugInComboItem;
                SystemOptions.SelectedEditorPlugIn = pci.Name;
                lblEditorDescription.Text = pci.Description;
                IChem4WordEditor editor = Globals.Chem4WordV3.GetEditorPlugIn(pci.Name);
                btnEditorSettings.Enabled = editor.HasSettings;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnRenderersSelectedIndexChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                PlugInComboItem pci = cboRenderers.SelectedItem as PlugInComboItem;
                SystemOptions.SelectedRendererPlugIn = pci.Name;
                lblRendererDescription.Text = pci.Description;
                IChem4WordRenderer renderer = Globals.Chem4WordV3.GetRendererPlugIn(pci.Name);
                btnRendererSettings.Enabled = renderer.HasSettings;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnSearchersSelectedIndexChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                PlugInComboItem pci = cboSearchers.SelectedItem as PlugInComboItem;
                lblSearcherDescription.Text = pci.Description;
                IChem4WordSearcher searcher = Globals.Chem4WordV3.GetSearcherPlugIn(pci.Name);
                btnSearcherSettings.Enabled = searcher.HasSettings;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnEditorSettingsClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                IChem4WordEditor editor = Globals.Chem4WordV3.GetEditorPlugIn(cboEditors.SelectedItem.ToString());
                editor.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
                editor.ChangeSettings(new Point(TopLeft.X + Constants.TopLeftOffset, TopLeft.Y + Constants.TopLeftOffset));
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnRendererSettingsClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                IChem4WordRenderer renderer = Globals.Chem4WordV3.GetRendererPlugIn(cboRenderers.SelectedItem.ToString());
                renderer.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
                renderer.ChangeSettings(new Point(TopLeft.X + Constants.TopLeftOffset, TopLeft.Y + Constants.TopLeftOffset));
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnSearcherSettingsClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                IChem4WordSearcher searcher = Globals.Chem4WordV3.GetSearcherPlugIn(cboSearchers.SelectedItem.ToString());
                searcher.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
                searcher.ChangeSettings(new Point(TopLeft.X + Constants.TopLeftOffset, TopLeft.Y + Constants.TopLeftOffset));
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkUseWebServices_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SystemOptions.UseWebServices = chkUseWebServices.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void txtChemSpiderWsUri_TextChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SystemOptions.ChemSpiderWebServiceUri = txtChemSpiderWsUri.Text;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void txtChemSpiderRdfUri_TextChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SystemOptions.ChemSpiderRdfServiceUri = txtChemSpiderRdfUri.Text;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkTelemetryEnabled_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SystemOptions.TelemetryEnabled = chkTelemetryEnabled.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void cboUpdateFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                string selected = cboUpdateFrequency.SelectedItem.ToString();

                switch (selected)
                {
                    case "Daily":
                        SystemOptions.AutoUpdateFrequency = 1;
                        break;

                    case "Weekly":
                        SystemOptions.AutoUpdateFrequency = 7;
                        break;
                }
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkAutomaticUpdates_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SystemOptions.AutoUpdateEnabled = chkAutomaticUpdates.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnGalleryImportClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                int fileCount = 0;
                StringBuilder sb;

                // Start with V2 Add-In data path
                string importFolder = Path.Combine(Globals.Chem4WordV3.AddInInfo.AppDataPath, @"Chemistry Add-In for Word");
                if (Directory.Exists(importFolder))
                {
                    if (Directory.Exists(Path.Combine(importFolder, "Chemistry Gallery")))
                    {
                        importFolder = Path.Combine(importFolder, "Chemistry Gallery");
                    }
                }
                else
                {
                    importFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                if (Directory.Exists(importFolder))
                {
                    // Fix scrolling to selected item by using code from https://social.msdn.microsoft.com/Forums/expression/en-US/1257aebc-22a6-44f6-975b-74f5067728bc/autoposition-showfolder-dialog?forum=vbgeneral

                    VistaFolderBrowserDialog browser = new VistaFolderBrowserDialog();

                    browser.Description = "Select a folder to import cml files from";
                    browser.UseDescriptionForTitle = true;
                    browser.RootFolder = Environment.SpecialFolder.Desktop;
                    browser.ShowNewFolderButton = false;
                    browser.SelectedPath = importFolder;
                    DialogResult dr = browser.ShowDialog();

                    if (dr == DialogResult.OK)
                    {
                        string selectedFolder = browser.SelectedPath;
                        string doneFile = Path.Combine(selectedFolder, "library-import-done.txt");

                        sb = new StringBuilder();
                        sb.AppendLine("Do you want to import the Gallery structures into the Library?");
                        sb.AppendLine("(This cannot be undone.)");
                        dr = AskUserYesNo(sb.ToString());
                        if (dr == DialogResult.Yes)
                        {
                            if (File.Exists(doneFile))
                            {
                                sb = new StringBuilder();
                                sb.AppendLine($"All files have been imported already from '{selectedFolder}'");
                                sb.AppendLine("Do you want to rerun the import?");
                                dr = AskUserYesNo(sb.ToString());
                                if (dr == DialogResult.Yes)
                                {
                                    File.Delete(doneFile);
                                }
                            }
                        }

                        if (dr == DialogResult.Yes)
                        {
                            Progress pb = new Progress();

                            try
                            {
                                var xmlFiles = Directory.GetFiles(selectedFolder, "*.cml");

                                pb.Maximum = xmlFiles.Length;
                                pb.TopLeft = new Point(TopLeft.X + Constants.TopLeftOffset, TopLeft.Y + Constants.TopLeftOffset);
                                pb.Show();

                                foreach (string cmlFile in xmlFiles)
                                {
                                    pb.Message = cmlFile.Replace(selectedFolder, ".");
                                    pb.Increment(1);

                                    var cml = File.ReadAllText(cmlFile);
                                    if (LibraryModel.ImportCml(cml))
                                    {
                                        fileCount++;
                                    }
                                }

                                pb.Hide();
                                pb.Close();

                                File.WriteAllText(doneFile, $"{fileCount} cml files imported into library");
                                FileInfo fi = new FileInfo(doneFile);
                                fi.Attributes = FileAttributes.Hidden;

                                Globals.Chem4WordV3.LibraryNames = LibraryModel.GetLibraryNames();

                                InformUser($"Successfully imported {fileCount} structures from '{selectedFolder}'.");
                            }
                            catch (Exception ex)
                            {
                                pb.Hide();
                                pb.Close();
                                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnClearLibraryClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This will delete all the structures from the Library");
                sb.AppendLine("It will not delete any tags.");
                sb.AppendLine("");
                sb.AppendLine("Do you want to proceed?");
                sb.AppendLine("This cannot be undone.");
                DialogResult dr = AskUserYesNo(sb.ToString(), MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    LibraryModel.DeleteAllChemistry();
                    Globals.Chem4WordV3.LibraryNames = LibraryModel.GetLibraryNames();

                    var app = Globals.Chem4WordV3.Application;
                    foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                    {
                        if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.LibraryTaskPaneTitle)
                        {
                            var custTaskPane = taskPane;
                            (custTaskPane.Control as LibraryHost)?.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnOpenSettingsFolderClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                Process.Start(Globals.Chem4WordV3.AddInInfo.ProductAppDataPath);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnOpenLibraryFolderClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                Process.Start(Globals.Chem4WordV3.AddInInfo.ProgramDataPath);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void OnOpenPlugInFolderClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                Process.Start(Path.Combine(Globals.Chem4WordV3.AddInInfo.DeploymentPath, "PlugIns"));
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }
    }

    public class PlugInComboItem
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}