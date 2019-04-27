// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Core.UI.Wpf;
using Chem4Word.Database;
using IChem4Word.Contracts;
using Ookii.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Forms = System.Windows.Forms;

namespace Chem4Word.UI.WPF
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public event EventHandler OnButtonClick;

        public Options SystemOptions { get; set; }
        public Point TopLeft { get; set; }
        public bool Dirty { get; set; }

        private bool _loading;

        public SettingsControl()
        {
            _loading = true;

            InitializeComponent();
        }

        #region Form Load

        private void SettingsControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            #region Load Images

            // Tab 1 - Plug Ins
            var imageStream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "Preferences.png");
            if (imageStream != null)
            {
                var bitmap = CreateImageFromStream(imageStream);

                EditorSettingsButtonImage.Source = bitmap;
                RendererSettingsButtonImage.Source = bitmap;
                SearcherSettingsButtonImage.Source = bitmap;
            }

            // Tab 4 - Libaray
            imageStream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "Gallery-Toggle.png");
            if (imageStream != null)
            {
                var bitmap = CreateImageFromStream(imageStream);
                ImportIntoLibraryButtonImage.Source = bitmap;
            }
            imageStream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "Gallery-Save.png");
            if (imageStream != null)
            {
                var bitmap = CreateImageFromStream(imageStream);
                ExportFromLibraryButtonImage.Source = bitmap;
            }
            imageStream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "Gallery-Delete.png");
            if (imageStream != null)
            {
                var bitmap = CreateImageFromStream(imageStream);
                EraseLibraryButtonImage.Source = bitmap;
            }

            // Tab 5 Maintenance
            imageStream = ResourceHelper.GetBinaryResource(Assembly.GetExecutingAssembly(), "File-Open.png");
            if (imageStream != null)
            {
                var bitmap = CreateImageFromStream(imageStream);
                LibraryFolderButtonImage.Source = bitmap;
                SettingsFolderButtonImage.Source = bitmap;
                PlugInsFolderButtonImage.Source = bitmap;
            }

            #endregion Load Images

            #region Set Current Values

            if (SystemOptions != null)
            {
                LoadSettings();
            }

            #endregion Set Current Values

            _loading = false;
        }

        #endregion Form Load

        #region Bottom Buttons

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            WpfEventArgs args = new WpfEventArgs();
            args.Button = "Ok";
            args.OutputValue = "";

            OnButtonClick?.Invoke(this, args);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            WpfEventArgs args = new WpfEventArgs();
            args.Button = "Cancel";
            args.OutputValue = "";

            OnButtonClick?.Invoke(this, args);
        }

        private void DefaultsButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                Forms.DialogResult dr = UserInteractions.AskUserOkCancel("Restore default settings");
                if (dr == Forms.DialogResult.OK)
                {
                    _loading = true;
                    Dirty = true;
                    SystemOptions.RestoreDefaults();
                    LoadSettings();
                    _loading = false;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        #endregion Bottom Buttons

        #region Tab 1 Events

        private void SelectedEditorSettings_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            IChem4WordEditor editor = Globals.Chem4WordV3.GetEditorPlugIn(SelectEditorPlugIn.SelectedItem.ToString());
            editor.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
            editor.ChangeSettings(new Point(SystemOptions.WordTopLeft.X + Constants.TopLeftOffset * 2, SystemOptions.WordTopLeft.Y + Constants.TopLeftOffset * 2));
        }

        private void SelectedRendererSettings_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            IChem4WordRenderer renderer = Globals.Chem4WordV3.GetRendererPlugIn(SelectRendererPlugIn.SelectedItem.ToString());
            renderer.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
            renderer.ChangeSettings(new Point(SystemOptions.WordTopLeft.X + Constants.TopLeftOffset * 2, SystemOptions.WordTopLeft.Y + Constants.TopLeftOffset * 2));
        }

        private void SelectedSearcherSettings_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            IChem4WordSearcher searcher = Globals.Chem4WordV3.GetSearcherPlugIn(SelectSearcherPlugIn.SelectedItem.ToString());
            searcher.ProductAppDataPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
            searcher.ChangeSettings(new Point(SystemOptions.WordTopLeft.X + Constants.TopLeftOffset * 2, SystemOptions.WordTopLeft.Y + Constants.TopLeftOffset * 2));
        }

        private void SelectEditorPlugIn_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            if (!_loading)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

                PlugInComboItem pci = SelectEditorPlugIn.SelectedItem as PlugInComboItem;
                SystemOptions.SelectedEditorPlugIn = pci?.Name;
                SelectedEditorPlugInDescription.Text = pci?.Description;
                IChem4WordEditor editor = Globals.Chem4WordV3.GetEditorPlugIn(pci.Name);
                SelectedEditorSettings.IsEnabled = editor.HasSettings;

                Dirty = true;
            }
        }

        private void SelectRenderer_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            if (!_loading)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

                PlugInComboItem pci = SelectRendererPlugIn.SelectedItem as PlugInComboItem;
                SystemOptions.SelectedRendererPlugIn = pci?.Name;
                SelectedRendererDescription.Text = pci?.Description;
                IChem4WordRenderer renderer = Globals.Chem4WordV3.GetRendererPlugIn(pci.Name);
                SelectedRendererSettings.IsEnabled = renderer.HasSettings;

                Dirty = true;
            }
        }

        private void SelectSearcher_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            if (!_loading)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

                PlugInComboItem pci = SelectSearcherPlugIn.SelectedItem as PlugInComboItem;
                SelectedSearcherDescription.Text = pci?.Description;
                IChem4WordSearcher searcher = Globals.Chem4WordV3.GetSearcherPlugIn(pci.Name);
                SelectedSearcherSettings.IsEnabled = searcher.HasSettings;
                Dirty = true;
            }
        }

        #endregion Tab 1 Events

        #region Tab 2 Events

        private void Chem4WordWebServiceUri_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            if (!_loading)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");
                SystemOptions.Chem4WordWebServiceUri = Chem4WordWebServiceUri.Text;
                Dirty = true;
            }
        }

        #endregion Tab 2 Events

        #region Tab 3 Events

        private void TelemetryEnabled_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            if (!_loading)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");
                SystemOptions.TelemetryEnabled = TelemetryEnabled.IsChecked.Value;
                Dirty = true;
            }
        }

        #endregion Tab 3 Events

        #region Tab 4 Events

        private void ImportIntoLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                if (Globals.Chem4WordV3.LibraryNames == null)
                {
                    Globals.Chem4WordV3.LoadNamesFromLibrary();
                }
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
                    Forms.DialogResult dr = browser.ShowDialog();

                    if (dr == Forms.DialogResult.OK)
                    {
                        string selectedFolder = browser.SelectedPath;
                        string doneFile = Path.Combine(selectedFolder, "library-import-done.txt");

                        sb = new StringBuilder();
                        sb.AppendLine("Do you want to import the Gallery structures into the Library?");
                        sb.AppendLine("(This cannot be undone.)");
                        dr = UserInteractions.AskUserYesNo(sb.ToString());
                        if (dr == Forms.DialogResult.Yes)
                        {
                            if (File.Exists(doneFile))
                            {
                                sb = new StringBuilder();
                                sb.AppendLine($"All files have been imported already from '{selectedFolder}'");
                                sb.AppendLine("Do you want to rerun the import?");
                                dr = UserInteractions.AskUserYesNo(sb.ToString());
                                if (dr == Forms.DialogResult.Yes)
                                {
                                    File.Delete(doneFile);
                                }
                            }
                        }

                        if (dr == Forms.DialogResult.Yes)
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
                                    var lib = new Database.Library();
                                    if (lib.ImportCml(cml))
                                    {
                                        fileCount++;
                                    }
                                }

                                pb.Hide();
                                pb.Close();

                                File.WriteAllText(doneFile, $"{fileCount} cml files imported into library");
                                FileInfo fi = new FileInfo(doneFile);
                                fi.Attributes = FileAttributes.Hidden;

                                Globals.Chem4WordV3.LoadNamesFromLibrary();

                                UserInteractions.InformUser($"Successfully imported {fileCount} structures from '{selectedFolder}'.");
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

        private void ExportFromLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                string exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                VistaFolderBrowserDialog browser = new VistaFolderBrowserDialog();

                browser.Description = "Select a folder to export your Library's structures as cml files";
                browser.UseDescriptionForTitle = true;
                browser.RootFolder = Environment.SpecialFolder.Desktop;
                browser.ShowNewFolderButton = false;
                browser.SelectedPath = exportFolder;
                Forms.DialogResult dr = browser.ShowDialog();

                if (dr == Forms.DialogResult.OK)
                {
                    exportFolder = browser.SelectedPath;

                    if (Directory.Exists(exportFolder))
                    {
                        Forms.DialogResult doExport = Forms.DialogResult.Yes;
                        string[] existingCmlFiles = Directory.GetFiles(exportFolder, "*.cml");
                        if (existingCmlFiles.Length > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine($"This folder contains {existingCmlFiles.Length} cml files.");
                            sb.AppendLine($"Do you wish to continue?");
                            doExport = UserInteractions.AskUserYesNo(sb.ToString(), Forms.MessageBoxDefaultButton.Button2);
                        }
                        if (doExport == Forms.DialogResult.Yes)
                        {
                            Database.Library lib = new Database.Library();

                            int exported = 0;

                            List<ChemistryDTO> dto = lib.GetAllChemistry(null);
                            foreach (var obj in dto)
                            {
                                var filename = Path.Combine(browser.SelectedPath, $"Chem4Word-{obj.Id:000000000}.cml");
                                File.WriteAllText(filename, obj.Cml);
                                exported++;
                            }

                            if (exported > 0)
                            {
                                UserInteractions.InformUser($"Exported {exported} structures to {browser.SelectedPath}");
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

        private void EraseLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                if (Globals.Chem4WordV3.LibraryNames == null)
                {
                    Globals.Chem4WordV3.LoadNamesFromLibrary();
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This will delete all the structures from the Library");
                sb.AppendLine("It will not delete any tags.");
                sb.AppendLine("");
                sb.AppendLine("Do you want to proceed?");
                sb.AppendLine("This cannot be undone.");
                Forms.DialogResult dr =
                    UserInteractions.AskUserYesNo(sb.ToString(), Forms.MessageBoxDefaultButton.Button2);
                if (dr == Forms.DialogResult.Yes)
                {
                    var lib = new Database.Library();
                    lib.DeleteAllChemistry();
                    Globals.Chem4WordV3.LoadNamesFromLibrary();

                    // Close the existing Library Pane
#if DEBUG
                    Debugger.Break();
#endif
                    //var app = Globals.Chem4WordV3.Application;
                    //foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                    //{
                    //    if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.LibraryTaskPaneTitle)
                    //    {
                    //        var custTaskPane = taskPane;
                    //        (custTaskPane.Control as LibraryHost)?.Refresh();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        #endregion Tab 4 Events

        #region Tab 5 Events

        private void SettingsFolder_OnClick(object sender, RoutedEventArgs e)
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

        private void LibraryFolder_OnClick(object sender, RoutedEventArgs e)
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

        private void PlugInsFolder_OnClick(object sender, RoutedEventArgs e)
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

        #endregion Tab 5 Events

        #region Private methods

        private void LoadSettings()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            #region Tab 1

            SelectEditorPlugIn.Items.Clear();
            SelectRendererPlugIn.Items.Clear();
            SelectSearcherPlugIn.Items.Clear();
            SelectedEditorSettings.IsEnabled = false;
            SelectedRendererSettings.IsEnabled = false;
            SelectedSearcherSettings.IsEnabled = false;

            Version browser = null;
            try
            {
                browser = new Forms.WebBrowser().Version;
            }
            catch
            {
                browser = null;
            }

            if (SystemOptions.SelectedEditorPlugIn.Equals(Constants.DefaultEditorPlugIn800))
            {
                if (browser?.Major < Constants.ChemDoodleWeb800MinimumBrowserVersion)
                {
                    SystemOptions.SelectedEditorPlugIn = Constants.DefaultEditorPlugIn702;
                }
            }

            string selectedEditor = SystemOptions.SelectedEditorPlugIn;

            foreach (IChem4WordEditor editor in Globals.Chem4WordV3.Editors)
            {
                bool add = !(editor.Name.Equals(Constants.DefaultEditorPlugIn800)
                             && browser?.Major < Constants.ChemDoodleWeb800MinimumBrowserVersion);

                if (add)
                {
                    PlugInComboItem pci = new PlugInComboItem()
                    {
                        Name = editor.Name,
                        Description = editor.Description
                    };
                    int item = SelectEditorPlugIn.Items.Add(pci);

                    if (editor.Name.Equals(selectedEditor))
                    {
                        SelectedEditorSettings.IsEnabled = editor.HasSettings;
                        SelectedEditorPlugInDescription.Text = editor.Description;
                        SelectEditorPlugIn.SelectedIndex = item;
                    }
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
                int item = SelectRendererPlugIn.Items.Add(pci);
                if (renderer.Name.Equals(selectedRenderer))
                {
                    SelectedRendererSettings.IsEnabled = renderer.HasSettings;
                    SelectedRendererDescription.Text = renderer.Description;
                    SelectRendererPlugIn.SelectedIndex = item;
                }
            }

            foreach (IChem4WordSearcher searcher in Globals.Chem4WordV3.Searchers.OrderBy(s => s.DisplayOrder))
            {
                PlugInComboItem pci = new PlugInComboItem()
                {
                    Name = searcher.Name,
                    Description = searcher.Description
                };
                int item = SelectSearcherPlugIn.Items.Add(pci);
                if (SelectSearcherPlugIn.Items.Count == 1)
                {
                    SelectedSearcherSettings.IsEnabled = searcher.HasSettings;
                    SelectedSearcherDescription.Text = searcher.Description;
                    SelectSearcherPlugIn.SelectedIndex = item;
                }
            }

            #endregion Tab 1

            #region Tab 2

            string uri = SystemOptions.Chem4WordWebServiceUri;
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                uri = Constants.DefaultChem4WordWebServiceUri;
                SystemOptions.Chem4WordWebServiceUri = uri;
                Dirty = true;
            }

            Chem4WordWebServiceUri.Text = uri;

            #endregion Tab 2

            #region Tab 3

            string betaValue = Globals.Chem4WordV3.ThisVersion.Root?.Element("IsBeta")?.Value;
            bool isBeta = betaValue != null && bool.Parse(betaValue);

            TelemetryEnabled.IsChecked = isBeta || SystemOptions.TelemetryEnabled;
            TelemetryEnabled.IsEnabled = !isBeta;
            if (!isBeta)
            {
                BetaInformation.Visibility = Visibility.Hidden;
            }

            #endregion Tab 3
        }

        private BitmapImage CreateImageFromStream(Stream stream)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        #endregion Private methods
    }
}