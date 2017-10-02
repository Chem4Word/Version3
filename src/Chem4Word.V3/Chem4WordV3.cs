using Chem4Word.Core;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using Chem4Word.Library;
using Chem4Word.Model.Converters;
using Chem4Word.Telemetry;
using IChem4Word.Contracts;
using Microsoft.Office.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml.Linq;
using Chem4Word.Core.Helpers;
using Extensions = Microsoft.Office.Tools.Word.Extensions;
using OfficeTools = Microsoft.Office.Tools;
using Word = Microsoft.Office.Interop.Word;
using WordTools = Microsoft.Office.Tools.Word;

namespace Chem4Word
{
    public partial class Chem4WordV3
    {
        // Internal variables for class
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];

        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static CustomRibbon Ribbon;

        public int VersionsBehind = 0;
        public XDocument AllVersions;
        public XDocument ThisVersion;

        public bool EventsEnabled = true;

        private bool ChemistrySelected = false;
        private bool _markAsChemistryHandled = false;
        private int _rightClickEvents = 0;

        public bool LibraryState = false;

        public C4wAddInInfo AddInInfo = new C4wAddInInfo();
        public Options SystemOptions = new Options();
        public TelemetryWriter Telemetry = new TelemetryWriter();

        public List<IChem4WordEditor> Editors;
        public List<IChem4WordRenderer> Renderers;
        public List<IChem4WordSearcher> Searchers;

        public Dictionary<string, int> LibraryNames;

        private static readonly string[] ContextMenusTargets = { "Text", "Table Text", "Spelling", "Grammar", "Grammar (2)", "Lists", "Table Lists" };
        const string ContextMenuTag = "2829AECC-061C-4DC5-8CC0-CAEC821B9127";
        const string ContextMenuText = "Convert to Chemistry";

        public int WordWidth
        {
            get
            {
                int width = 0;

                try
                {
                    CommandBar commandBar1 = Application.CommandBars["Ribbon"];
                    if (commandBar1 != null)
                    {
                        width = Math.Max(width, commandBar1.Width);
                    }
                    CommandBar commandBar2 = Application.CommandBars["Status Bar"];
                    if (commandBar2 != null)
                    {
                        width = Math.Max(width, commandBar2.Width);
                    }
                }
                catch
                {
                    //
                }

                return width;
            }
        }

        public Point WordTopLeft
        {
            get
            {
                Point pp = new Point();

                try
                {
                    // Get position of Standard CommandBar (<ALT>+F)
                    CommandBar commandBar = Application.CommandBars["Standard"];
                    pp.X = commandBar.Left + commandBar.Height;
                    pp.Y = commandBar.Top + commandBar.Height;
                }
                catch
                {
                    //
                }
                return pp;
            }
        }

        public int WordVersion
        {
            get
            {
                int version = -1;

                try
                {
                    switch (Application.Version)
                    {
                        case "12.0":
                            version = 2007;
                            break;

                        case "14.0":
                            version = 2010;
                            break;

                        case "15.0":
                            version = 2013;
                            break;

                        case "16.0":
                            version = 2016;
                            break;
                    }
                }
                catch
                {
                    //
                }

                return version;
            }
        }

        /// <summary>
        ///   Override the BeginInit method to load Chemistry Fonts.
        /// </summary>
        public override void BeginInit()
        {
            base.BeginInit();

            // Install 2 Chemisty fonts into current process.
            // Font name: Ms ChemSans, MS ChemSerif
            //Chem4Word.Font.ChemistryFontManager.AddChemistryFontToProcess();
        }

        private void C4WLiteAddIn_Startup(object sender, EventArgs e)
        {
            Debug.WriteLine("C4WLiteAddIn_Startup ...");
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            PerformStartUpActions();
        }

        private void C4WLiteAddIn_Shutdown(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Debug.WriteLine(module);

            PerformShutDownActions();
        }

        private void PerformStartUpActions()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Debug.WriteLine(module);

            try
            {
                Word.Application app = Globals.Chem4WordV3.Application;

                // Hook in Global Application level events
                app.WindowBeforeDoubleClick += OnWindowBeforeDoubleClick;
                app.WindowSelectionChange += OnWindowSelectionChange;
                app.WindowActivate += OnWindowActivate;
                app.WindowBeforeRightClick += OnWindowBeforeRightClick;

                // Hook in Global Document Level Events
                app.DocumentOpen += OnDocumentOpen;
                app.DocumentChange += OnDocumentChange;
                app.DocumentBeforeClose += OnDocumentBeforeClose;
                app.DocumentBeforeSave += OnDocumentBeforeSave;
                ((Word.ApplicationEvents4_Event)Globals.Chem4WordV3.Application).NewDocument += OnNewDocument;

                if (app.Documents.Count > 0)
                {
                    EnableDocumentEvents(app.Documents[1]);
                    SetButtonStates(ButtonState.CanInsert);
                }

                // Read in options file
                string padPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
                string fileName = $"{Globals.Chem4WordV3.AddInInfo.ProductName}.json";
                string optionsFile = Path.Combine(padPath, fileName);
                if (File.Exists(optionsFile))
                {
                    string json = File.ReadAllText(optionsFile);
                    SystemOptions = JsonConvert.DeserializeObject<Options>(json);
                }

                if (Globals.Chem4WordV3.AddInInfo.DeploymentPath.ToLower().Contains("vso-ci"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Hey {Environment.UserName}");
                    sb.AppendLine("");
                    sb.AppendLine("You should not be running this build configuration");
                    sb.AppendLine("Please select Debug or Release build!");
                    UserInteractions.StopUser(sb.ToString());
                }

                string libraryTarget = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProgramDataPath, Constants.LibraryFileName);
                if (!File.Exists(libraryTarget))
                {
                    ResourceHelper.WriteResource(Assembly.GetExecutingAssembly(), "Data.Library.db", libraryTarget);
                }
                LibraryNames = LibraryModel.GetLibraryNames();

                // Set parameter mustBeSigned true if assemblies must be signed by us
                LoadPlugIns(false);
                ListPlugInsFound();

                ConfigWatcher cw = new ConfigWatcher(Globals.Chem4WordV3.AddInInfo.ProductAppDataPath);

                // Deliberate crash to test Error Reporting
                //int ii = 2;
                //int dd = 0;
                //int bang = ii/dd;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void PerformShutDownActions()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Debug.WriteLine(module);

            try
            {
                //RemoveRightClickButton(_targetedPopUps);

                if (Globals.Chem4WordV3.Editors != null)
                {
                    for (int i = 0; i < Globals.Chem4WordV3.Editors.Count; i++)
                    {
                        Globals.Chem4WordV3.Editors[i].Telemetry = null;
                        Globals.Chem4WordV3.Editors[i] = null;
                    }
                }

                if (Globals.Chem4WordV3.Renderers != null)
                {
                    for (int i = 0; i < Globals.Chem4WordV3.Renderers.Count; i++)
                    {
                        Globals.Chem4WordV3.Renderers[i].Telemetry = null;
                        Globals.Chem4WordV3.Renderers[i] = null;
                    }
                }

                if (Globals.Chem4WordV3.Searchers != null)
                {
                    for (int i = 0; i < Globals.Chem4WordV3.Searchers.Count; i++)
                    {
                        Globals.Chem4WordV3.Searchers[i].Telemetry = null;
                        Globals.Chem4WordV3.Searchers[i] = null;
                    }
                }

                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            finally
            {
                // Fix reported issue with Windows 8 and WPF See
                // https://social.msdn.microsoft.com/Forums/office/en-US/bb990ddb-ecde-4161-8915-e66e913e3a3b/invalidoperationexception-localdatastoreslot-storage-has-been-freed?forum=exceldev
                // I saw this on Server 2008 R2 which is very closely related to Windows 8
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        }

        private void ListPlugInsFound()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            int count = 0;

            //Telemetry.Write(module, "Debug", $"Found {Editors.Count} Editor PlugIns");
            Debug.WriteLine($"Found {Editors.Count} Editor PlugIns");
            foreach (IChem4WordEditor editor in Editors)
            {
                count++;
                //Telemetry.Write(module, "Debug", editor.Name);
                Debug.WriteLine($"{editor.Name}");
            }

            //Telemetry.Write(module, "Debug", $"Found {Renderers.Count} Renderer PlugIns");
            Debug.WriteLine($"Found {Renderers.Count} Renderer PlugIns");
            foreach (IChem4WordRenderer renderer in Renderers)
            {
                count++;
                //Telemetry.Write(module, "Debug", renderer.Name);
                Debug.WriteLine($"{renderer.Name}");
            }

            //Telemetry.Write(module, "Debug", $"Found {Searchers.Count} Searchers PlugIns");
            Debug.WriteLine($"Found {Searchers.Count} Searcher PlugIns");
            foreach (IChem4WordSearcher searcher in Searchers)
            {
                count++;
                //Telemetry.Write(module, "Debug", searcher.Name);
                Debug.WriteLine($"{searcher.Name}");
            }
        }

        private void LoadPlugIns(bool mustBeSigned)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            // http://www.codeproject.com/Articles/453778/Loading-Assemblies-from-Anywhere-into-a-New-AppDom

            Stopwatch sw = new Stopwatch();
            sw.Start();

            string plugInPath = Path.Combine(AddInInfo.DeploymentPath, "PlugIns");
            //Telemetry.Write(module, "Debug", $"Looking for Plug-Ins in folder {plugInPath}");

            string[] files = null;
            int filesFound = 0;

            if (Directory.Exists(plugInPath))
            {
                files = Directory.GetFiles(plugInPath, "Chem4Word*.dll");
                filesFound = files.Length;
            }

            List<string> plugInsFound = new List<string>();

            #region Find Our PlugIns

            foreach (string file in files)
            {
                var manager = new AssemblyReflectionManager();

                try
                {
                    var success = manager.LoadAssembly(file, "Chem4WordTempDomain");
                    if (success)
                    {
                        #region Find Our Interfaces

                        var results = manager.Reflect(file, a =>
                        {
                            var names = new List<string>();

                            #region Get Code Signing Certificate details

                            string signedBy = "";

                            Module mod = a.GetModules().First();
                            var certificate = mod.GetSignerCertificate();
                            if (certificate != null)
                            {
                                // E=developer@chem4word.co.uk, CN="Open Source Developer, Mike Williams", O=Open Source Developer, C=GB
                                Debug.WriteLine(certificate.Subject);
                                signedBy = certificate.Subject;
                                // CN=Certum Code Signing CA SHA2, OU=Certum Certification Authority, O=Unizeto Technologies S.A., C=PL
                                Debug.WriteLine(certificate.Issuer);
                            }

                            #endregion Get Code Signing Certificate details

                            var types = a.GetTypes();
                            foreach (var t in types)
                            {
                                if (t.IsClass && t.IsPublic && !t.IsAbstract)
                                {
                                    var ifaces = t.GetInterfaces();
                                    foreach (var iface in ifaces)
                                    {
                                        if (iface.FullName.StartsWith("IChem4Word.Contracts"))
                                        {
                                            string[] parts = a.FullName.Split(',');
                                            FileInfo fi = new FileInfo(t.Module.FullyQualifiedName);
                                            names.Add($"{parts[0]}|{iface.FullName}|{fi.Name}|{signedBy}");
                                        }
                                    }
                                }
                            }

                            return names;
                        });

                        #endregion Find Our Interfaces

                        manager.UnloadAssembly(file);

                        plugInsFound.AddRange(results);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            #endregion Find Our PlugIns

            if (plugInsFound.Count == 0)
            {
                UserInteractions.StopUser("No Plug-Ins Loaded");
            }

            Editors = new List<IChem4WordEditor>();
            Renderers = new List<IChem4WordRenderer>();
            Searchers = new List<IChem4WordSearcher>();

            Type editorType = typeof(IChem4WordEditor);
            Type rendererType = typeof(IChem4WordRenderer);
            Type searcherType = typeof(IChem4WordSearcher);

            foreach (string plugIn in plugInsFound)
            {
                string[] parts = plugIn.Split('|');
                Debug.WriteLine(
                    $"Loading PlugIn {parts[0]} with Interface {parts[1]} from file {parts[2]} signed by {parts[3]}");

                bool allowed = true;
                if (mustBeSigned)
                {
                    // Is it signed by us?
                    allowed = parts[3].Contains("admin@chem4word.co.uk");
                }

                if (allowed)
                {
                    #region Find Source File

                    string sourceFile = "";
                    foreach (string file in files)
                    {
                        if (file.EndsWith(parts[2]))
                        {
                            sourceFile = file;
                            break;
                        }
                    }

                    #endregion Find Source File

                    if (!string.IsNullOrEmpty(sourceFile))
                    {
                        #region Load Editor(s)

                        if (parts[1].Contains("IChem4WordEditor"))
                        {
                            Assembly asm = Assembly.LoadFile(sourceFile);
                            Type[] types = asm.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.GetInterface(editorType.FullName) != null)
                                {
                                    IChem4WordEditor plugin = (IChem4WordEditor)Activator.CreateInstance(type);
                                    Editors.Add(plugin);
                                    break;
                                }
                            }
                        }

                        #endregion Load Editor(s)

                        #region Load Renderer(s)

                        if (parts[1].Contains("IChem4WordRenderer"))
                        {
                            Assembly asm = Assembly.LoadFile(sourceFile);
                            Type[] types = asm.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.GetInterface(rendererType.FullName) != null)
                                {
                                    IChem4WordRenderer plugin = (IChem4WordRenderer)Activator.CreateInstance(type);
                                    Renderers.Add(plugin);
                                    break;
                                }
                            }
                        }

                        #endregion Load Renderer(s)

                        #region Load Searcher(s)

                        if (parts[1].Contains("IChem4WordSearcher"))
                        {
                            Assembly asm = Assembly.LoadFile(sourceFile);
                            Type[] types = asm.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.GetInterface(searcherType.FullName) != null)
                                {
                                    IChem4WordSearcher plugin = (IChem4WordSearcher)Activator.CreateInstance(type);
                                    Searchers.Add(plugin);
                                    break;
                                }
                            }
                        }

                        #endregion Load Searcher(s)
                    }
                }
            }

            sw.Stop();
            Debug.WriteLine($"Examining {filesFound} files took {sw.ElapsedMilliseconds.ToString("#,000")}ms");
            //Telemetry.Write(module, "Verbose", $"Examining {filesFound} files took {sw.ElapsedMilliseconds.ToString("#,000")}ms");
        }

        public IChem4WordEditor GetEditorPlugIn(string name)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            IChem4WordEditor plugin = null;

            foreach (IChem4WordEditor ice in Editors)
            {
                if (ice.Name.Equals(name))
                {
                    plugin = ice;
                    plugin.Telemetry = Telemetry;
                    plugin.ProductAppDataPath = AddInInfo.ProductAppDataPath;
                    plugin.TopLeft = WordTopLeft;
                    break;
                }
            }

            return plugin;
        }

        public IChem4WordRenderer GetRendererPlugIn(string name)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            IChem4WordRenderer plugin = null;

            foreach (IChem4WordRenderer ice in Renderers)
            {
                if (ice.Name.Equals(name))
                {
                    plugin = ice;
                    plugin.Telemetry = Telemetry;
                    plugin.ProductAppDataPath = AddInInfo.ProductAppDataPath;
                    plugin.TopLeft = WordTopLeft;
                    break;
                }
            }

            return plugin;
        }

        public IChem4WordSearcher GetSearcherPlugIn(string name)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            IChem4WordSearcher plugin = null;

            foreach (IChem4WordSearcher ice in Searchers)
            {
                if (ice.Name.Equals(name))
                {
                    plugin = ice;
                    plugin.Telemetry = Telemetry;
                    plugin.ProductAppDataPath = AddInInfo.ProductAppDataPath;
                    plugin.TopLeft = WordTopLeft;
                    break;
                }
            }

            return plugin;
        }

        public void EnableDocumentEvents(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Debug.WriteLine("EnableDocumentEvents()");

            // Get reference to active document
            WordTools.Document wdoc = Extensions.DocumentExtensions.GetVstoObject(doc, Globals.Factory);

            // Hook in Content Control Events
            // See: https://msdn.microsoft.com/en-us/library/Microsoft.Office.Interop.Word.DocumentEvents2_methods%28v=office.14%29.aspx
            // See: https://msdn.microsoft.com/en-us/library/microsoft.office.interop.word.documentevents2_event_methods%28v=office.14%29.aspx

            // Remember to add corresponding code in DisableDocumentEvents()

            // ContentControlOnEnter Event Handler
            wdoc.ContentControlOnEnter += OnContentControlOnEnter;
            // ContentControlOnExit Event Handler
            wdoc.ContentControlOnExit += OnContentControlOnExit;
            // ContentControlBeforeDelete Event Handler
            wdoc.ContentControlBeforeDelete += OnContentControlBeforeDelete;
            // ContentControlAfterAdd Event Handler
            wdoc.ContentControlAfterAdd += OnContentControlAfterAdd;

            EventsEnabled = true;
        }

        public void DisableDocumentEvents(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Debug.WriteLine("DisableDocumentEvents()");

            EventsEnabled = false;

            // Get reference to active document
            WordTools.Document wdoc = Extensions.DocumentExtensions.GetVstoObject(doc, Globals.Factory);

            // Hook out Content Control Events
            // See: https://msdn.microsoft.com/en-us/library/Microsoft.Office.Interop.Word.DocumentEvents2_methods%28v=office.14%29.aspx
            // See: https://msdn.microsoft.com/en-us/library/microsoft.office.interop.word.documentevents2_event_methods%28v=office.14%29.aspx

            // Remember to add corresponding code in EnableDocumentEvents()

            // ContentControlOnEnter Event Handler
            wdoc.ContentControlOnEnter -= OnContentControlOnEnter;
            // ContentControlOnExit Event Handler
            wdoc.ContentControlOnExit -= OnContentControlOnExit;
            // ContentControlBeforeDelete Event Handler
            wdoc.ContentControlBeforeDelete -= OnContentControlBeforeDelete;
            // ContentControlAfterAdd Event Handler
            wdoc.ContentControlAfterAdd -= OnContentControlAfterAdd;
        }

        public void SetButtonStates(ButtonState state)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            if (Ribbon != null)
            {
                // Not needed, just here for completeness
                Ribbon.ChangeOptions.Enabled = true;

                switch (state)
                {
                    case ButtonState.NoDocument:
                        Ribbon.EditStructure.Enabled = false;
                        Ribbon.EditStructure.Label = "Draw";
                        Ribbon.EditLabels.Enabled = false;
                        Ribbon.ViewCml.Enabled = false;
                        Ribbon.ImportFromFile.Enabled = false;
                        Ribbon.ExportToFile.Enabled = false;
                        Ribbon.ShowAsMenu.Enabled = false;
                        Ribbon.ShowNavigator.Enabled = false;
                        Ribbon.ShowLibrary.Enabled = false;
                        Ribbon.WebSearchMenu.Enabled = false;
                        Ribbon.SaveToLibrary.Enabled = false;
                        Ribbon.ArrangeMolecules.Enabled = false;
                        break;

                    case ButtonState.CanEdit:
                        Ribbon.EditStructure.Enabled = true;
                        Ribbon.EditStructure.Label = "Edit";
                        Ribbon.EditLabels.Enabled = true;
                        Ribbon.ViewCml.Enabled = true;
                        Ribbon.ImportFromFile.Enabled = false;
                        Ribbon.ExportToFile.Enabled = true;
                        Ribbon.ShowAsMenu.Enabled = true;
                        Ribbon.ShowNavigator.Enabled = true;
                        Ribbon.ShowLibrary.Enabled = true;
                        Ribbon.WebSearchMenu.Enabled = false;
                        Ribbon.SaveToLibrary.Enabled = true;
                        Ribbon.ArrangeMolecules.Enabled = true;
                        break;

                    case ButtonState.CanInsert:
                        Ribbon.EditStructure.Enabled = true;
                        Ribbon.EditStructure.Label = "Draw";
                        Ribbon.EditLabels.Enabled = false;
                        Ribbon.ViewCml.Enabled = false;
                        Ribbon.ImportFromFile.Enabled = true;
                        Ribbon.ExportToFile.Enabled = false;
                        Ribbon.ShowAsMenu.Enabled = false;
                        Ribbon.ShowNavigator.Enabled = true;
                        Ribbon.ShowLibrary.Enabled = true;
                        Ribbon.WebSearchMenu.Enabled = true;
                        Ribbon.SaveToLibrary.Enabled = false;
                        Ribbon.ArrangeMolecules.Enabled = false;
                        break;
                }

                SetUpdateButtonState();
            }
        }

        public void SetUpdateButtonState()
        {
            if (VersionsBehind == 0)
            {
                Ribbon.Update.Visible = false;
                Ribbon.Update.Image = Properties.Resources.Shield_Good;
            }
            else
            {
                Ribbon.Update.Visible = true;
                if (VersionsBehind < 3)
                {
                    Ribbon.Update.Image = Properties.Resources.Shield_Warning;
                    Ribbon.Update.Label = "Update Advised";
                    Ribbon.Update.ScreenTip = "Please update";
                    Ribbon.Update.SuperTip = $"You are {VersionsBehind} versions behind";
                }
                else
                {
                    Ribbon.Update.Image = Properties.Resources.Shield_Danger;
                    Ribbon.Update.Label = "Update Essential";
                    Ribbon.Update.ScreenTip = "Please update";
                    Ribbon.Update.SuperTip = $"You are {VersionsBehind} versions behind";
                }
            }
        }

        public void SelectChemistry(Word.Selection sel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            EventsEnabled = false;

            try
            {
                Word.Document doc = sel.Application.ActiveDocument;

                foreach (Word.ContentControl cc in doc.ContentControls)
                {
                    if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                    {
                        if (cc.Range.Start <= sel.Range.Start && cc.Range.End >= sel.Range.End)
                        {
                            Debug.WriteLine($"  Selecting CC at {cc.Range.Start - 1} to {cc.Range.End + 1}");
                            doc.Application.Selection.SetRange(cc.Range.Start - 1, cc.Range.End + 1);
                        }
                    }
                }

                ChemistrySelected = sel.ContentControls.Count > 0;

                if (ChemistrySelected)
                {
                    Ribbon.ActivateChemistryTab();
                    SetButtonStates(ButtonState.CanEdit);
                }
                else
                {
                    SetButtonStates(ButtonState.CanInsert);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                EventsEnabled = true;
            }
        }

        #region Right Click

        #region Events

        private void OnWindowBeforeRightClick(Word.Selection sel, ref bool cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (sel.ContentControls.Count == 0)
                {
                    HandleRightClick(sel);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnCommandBarButtonClick(CommandBarButton ctrl, ref bool cancelDefault)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                _rightClickEvents++;
                Debug.WriteLine($"{module} Event#{_rightClickEvents} Handled: {_markAsChemistryHandled}");

                ClearChemistryContextMenus();
                if (!_markAsChemistryHandled)
                {
                    Debug.WriteLine($"Convert '{ctrl.Tag}' to Chemistry");
                    TargetWord tw = JsonConvert.DeserializeObject<TargetWord>(ctrl.Tag);

                    SQLiteDataReader chemistry = LibraryModel.GetChemistryByID(tw.ChemistryId);
                    string cml = null;
                    while (chemistry.Read())
                    {
                        var byteArray = (Byte[])chemistry["Chemistry"];
                        cml = Encoding.UTF8.GetString(byteArray);
                        break;
                    }

                    if (cml == null)
                    {
                        UserInteractions.WarnUser($"No match for '{tw.ChemicalName}' was found in your library");
                    }
                    else
                    {
                        Application.ActiveDocument.Range(tw.Start, tw.End).Select();
                        InsertChemistry(cml, tw.ChemicalName, false, true);
                        Telemetry.Write(module, "Information", $"Inserted 1D version of {tw.ChemicalName} from library");
                    }

                    _markAsChemistryHandled = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        #endregion

        #region Methods

        private void HandleRightClick(Word.Selection sel)
        {
            _markAsChemistryHandled = false;
            _rightClickEvents = 0;

            List<TargetWord> selectedWords = new List<TargetWord>();

            // Handling the selected text sentence by sentence should make us immune to return character sizing.
            for (int i = 1; i <= sel.Sentences.Count; i++)
            {
                var sentence = sel.Sentences[i];
                int start = sentence.Start;
                string sentenceText = sentence.Text;
                foreach (var kvp in LibraryNames)
                {
                    int idx = sentenceText.IndexOf(kvp.Key, StringComparison.InvariantCultureIgnoreCase);
                    if (idx > 0)
                    {
                        selectedWords.Add(new TargetWord
                        {
                            ChemicalName = kvp.Key,
                            Start = start + idx,
                            ChemistryId = kvp.Value,
                            End =  start + idx + kvp.Key.Length
                        });
                    }
                }
            }

            if (selectedWords.Count > 0)
            {
                AddChemistryMenuPopup(selectedWords);
            }
        }

        #endregion

        private void AddChemistryMenuPopup(List<TargetWord> selectedWords)
        {
            ClearChemistryContextMenus();

            WordTools.Document doc = Extensions.DocumentExtensions.GetVstoObject(Application.ActiveDocument, Globals.Factory);
            Application.CustomizationContext = doc.AttachedTemplate;

            foreach (string contextMenuName in ContextMenusTargets)
            {
                CommandBar contextMenu = Application.CommandBars[contextMenuName];
                if (contextMenu != null)
                {
                    CommandBarPopup popupControl = (CommandBarPopup)contextMenu.Controls.Add(
                                                                             MsoControlType.msoControlPopup,
                                                                             Type.Missing, Type.Missing, Type.Missing, true);
                    if (popupControl != null)
                    {
                        popupControl.Caption = ContextMenuText;
                        popupControl.Tag = ContextMenuTag;
                        foreach (var word in selectedWords)
                        {
                            CommandBarButton button = (CommandBarButton)popupControl.Controls.Add(
                                                                            MsoControlType.msoControlButton,
                                                                            Type.Missing, Type.Missing, Type.Missing, true);
                            if (button != null)
                            {
                                button.Caption = word.ChemicalName;
                                button.Tag = JsonConvert.SerializeObject(word);
                                button.FaceId = 1241;
                                button.Click += OnCommandBarButtonClick;
                            }
                        }
                    }
                }
            }

            ((Word.Template)doc.AttachedTemplate).Saved = true;
        }

        private void ClearChemistryContextMenus()
        {
            WordTools.Document doc = Extensions.DocumentExtensions.GetVstoObject(Application.ActiveDocument, Globals.Factory);
            Application.CustomizationContext = doc.AttachedTemplate;

            foreach (string contextMenuName in ContextMenusTargets)
            {
                CommandBar contextMenu = Application.CommandBars[contextMenuName];
                if (contextMenu != null)
                {
                    CommandBarPopup popupControl = (CommandBarPopup)contextMenu.FindControl(
                                                                         MsoControlType.msoControlPopup, Type.Missing,
                                                                         ContextMenuTag, true, true);
                    if (popupControl != null)
                    {
                        popupControl.Delete(true);
                    }
                }
            }

            ((Word.Template)doc.AttachedTemplate).Saved = true;
        }

        public static void InsertChemistry(string xml, string text, bool is2D, bool isCopy)
        {
            Word.Application app = Globals.Chem4WordV3.Application;

            Word.Document doc = app.ActiveDocument;
            Word.Selection sel = app.Selection;
            Word.ContentControl cc = null;

            if (sel.ContentControls.Count > 0)
            {
                cc = sel.ContentControls[1];
                if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                {
                    UserInteractions.WarnUser("You can't insert a chemistry object inside a chemistry object");
                }
            }
            else
            {
                app.ScreenUpdating = false;
                Globals.Chem4WordV3.DisableDocumentEvents(doc);

                CMLConverter cmlConverter = new CMLConverter();
                Model.Model chem = cmlConverter.Import(xml);
                if (chem.MeanBondLength < 5 || chem.MeanBondLength > 100)
                {
                    chem.Rescale(20);
                }

                if (isCopy)
                {
                    // Always generate new Guid on Import
                    chem.CustomXmlPartGuid = Guid.NewGuid().ToString("N");
                }

                string guidString = chem.CustomXmlPartGuid;
                string bookmarkName = "C4W_" + guidString;

                // Export just incase the CustomXmlPartGuid has been changed
                string cml = cmlConverter.Export(chem);

                if (is2D)
                {
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
                        cc = CustomRibbon.Insert2D(doc, tempfileName, bookmarkName, guidString);

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
                else
                {
                    string tag = guidString;
                    if (chem.Molecules.Count > 0 && chem.Molecules[0].ChemicalNames.Count > 0)
                    {
                        tag = $"{chem.Molecules[0].ChemicalNames[0].Id}:{guidString}";
                    }
                    bool found = false;
                    foreach (var mol in chem.Molecules)
                    {
                        foreach (var name in mol.ChemicalNames)
                        {
                            if (name.Name.ToLower().Equals(text))
                            {
                                tag = $"{name.Id}:{guidString}";
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    cc = CustomRibbon.Insert1D(app, doc, text, false, tag);
                }

                if (isCopy)
                {
                    doc.CustomXMLParts.Add(cml);
                }

                // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                app.ScreenUpdating = true;
                Globals.Chem4WordV3.EnableDocumentEvents(doc);

                if (cc != null)
                {
                    // Move selection point into the Content Control which was just edited or added
                    app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                }
            }
        }

        #endregion

        #region Document Events

        private void OnNewDocument(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnDocumentChange()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                if (Globals.Chem4WordV3.Application.Documents.Count > 0)
                {
                    Word.Document doc = null;
                    try
                    {
                        doc = Globals.Chem4WordV3.Application.ActiveDocument;
                    }
                    catch (Exception ex1)
                    {
                        // This only happens when document is in protected mode
                        Debug.WriteLine($"Module: {module}; Exception: {ex1.Message}");
                        //Telemetry.Write(module, "Information", $"Exception: {ex1.Message}");
                    }

                    if (doc != null)
                    {
                        Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");

                        // Call disable first to ensure events not registered multiple times
                        DisableDocumentEvents(doc);

                        Ribbon.ShowNavigator.Checked = false;
                        Ribbon.ShowLibrary.Checked = LibraryState;
                        Ribbon.ShowLibrary.Label = Ribbon.ShowLibrary.Checked ? "Close" : "Open ";

                        DialogResult answer = Upgrader.UpgradeIsRequired(doc);
                        if (answer == DialogResult.Yes)
                        {
                            //DisableDocumentEvents(doc);
                            Upgrader.DoUpgrade(doc);
                            //EnableDocumentEvents(doc);
                        }

                        foreach (var taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                        {
                            if (taskPane.Window != null)
                            {
                                string taskdoc = ((Word.Window)taskPane.Window).Document.Name;
                                if (doc.Name.Equals(taskdoc))
                                {
                                    if (taskPane.Title.Equals(Constants.NavigatorTaskPaneTitle))
                                    {
                                        Debug.WriteLine($"Found Navigator Task Pane. Visible: {taskPane.Visible}");
                                        Ribbon.ShowNavigator.Checked = taskPane.Visible;
                                        break;
                                    }
                                }
                            }
                        }

                        bool libraryFound = false;

                        foreach (var taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                        {
                            if (taskPane.Window != null)
                            {
                                string taskdoc = ((Word.Window)taskPane.Window).Document.Name;
                                if (doc.Name.Equals(taskdoc))
                                {
                                    if (taskPane.Title.Equals(Constants.LibraryTaskPaneTitle))
                                    {
                                        Debug.WriteLine($"Found Gallery Task Pane. Visible: {taskPane.Visible}");
                                        //Ribbon.ToggleButtonGallery.Checked = taskPane.Visible;
                                        taskPane.Visible = Ribbon.ShowLibrary.Checked;
                                        Ribbon.ShowLibrary.Label = Ribbon.ShowLibrary.Checked ? "Close" : "Open ";
                                        libraryFound = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!libraryFound)
                        {
                            if (Ribbon.ShowLibrary.Checked)
                            {
                                OfficeTools.CustomTaskPane custTaskPane =
                                    Globals.Chem4WordV3.CustomTaskPanes.Add(new LibraryHost(),
                                        Constants.LibraryTaskPaneTitle, Globals.Chem4WordV3.Application.ActiveWindow);
                                // Opposite side to Navigator's default placement
                                custTaskPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionLeft;
                                custTaskPane.Width = Globals.Chem4WordV3.WordWidth / 4;
                                custTaskPane.VisibleChanged += Ribbon.OnLibraryPaneVisibleChanged;
                                custTaskPane.Visible = true;
                                (custTaskPane.Control as LibraryHost)?.Refresh();
                            }
                        }

                        EnableDocumentEvents(doc);

                        SetButtonStates(ButtonState.CanInsert);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnDocumentOpen(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc">The document that is being saved.</param>
        /// <param name="saveAsUi">True if the Save As dialog box is displayed, whether to save a new document, in response to the Save command; or in response to the Save As command; or in response to the SaveAs or SaveAs2 method.</param>
        /// <param name="cancel">False when the event occurs.
        /// If the event procedure sets this argument to True, the document is not saved when the procedure is finished.</param>
        private void OnDocumentBeforeSave(Word.Document doc, ref bool saveAsUi, ref bool cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");

                if (!doc.ReadOnly)
                {
                    //// Exclude templates from Orphan checks
                    //if (!doc.Name.ToLower().EndsWith(".dotm"))
                    //{
                    //}
                    // Handle Word 2013+ AutoSave
                    if (WordVersion >= 2013)
                    {
                        if (!doc.IsInAutosave)
                        {
                            CustomXmlPartHelper.RemoveOrphanedXmlParts(doc);
                        }
                    }
                    else
                    {
                        CustomXmlPartHelper.RemoveOrphanedXmlParts(doc);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc">The document that's being closed.</param>
        /// <param name="cancel">False when the event occurs.
        /// If the event procedure sets this argument to True, the document doesn't close when the procedure is finished.</param>
        private void OnDocumentBeforeClose(Word.Document doc, ref bool cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");
                if (Ribbon != null)
                {
                    SetButtonStates(ButtonState.NoDocument);
                }

                Word.Application app = Globals.Chem4WordV3.Application;
                OfficeTools.CustomTaskPane custTaskPane = null;

                foreach (OfficeTools.CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
                {
                    if (app.ActiveWindow == taskPane.Window)
                    {
                        custTaskPane = taskPane;
                    }
                }
                if (custTaskPane != null)
                {
                    Globals.Chem4WordV3.CustomTaskPanes.Remove(custTaskPane);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        #endregion Document Events

        #region Window Events

        /// <summary>
        ///
        /// </summary>
        /// <param name="sel">The text selected.
        /// If no text is selected, the Sel parameter returns either nothing or the first character to the right of the insertion point.</param>
        private void OnWindowSelectionChange(Word.Selection sel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({sel.Document.Name})")}");
                Debug.WriteLine("  Selection: from " + sel.Range.Start + " to " + sel.Range.End);

                if (EventsEnabled)
                {
                    SelectChemistry(sel);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sel">The current selection.</param>
        /// <param name="cancel">False when the event occurs.
        /// If the event procedure sets this argument to True, the default double-click action does not occur when the procedure is finished.</param>
        private void OnWindowBeforeDoubleClick(Word.Selection sel, ref bool cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({sel.Document.Name})")}");
                Debug.WriteLine("  Selection: from " + sel.Range.Start + " to " + sel.Range.End);

                if (EventsEnabled)
                {
                    SelectChemistry(sel);
                }

                if (ChemistrySelected)
                {
                    CustomRibbon.PerformEdit();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="win"></param>
        private void OnWindowActivate(Word.Document doc, Word.Window win)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({doc.Name})")}");
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        #endregion Window Events

        #region Content Control Events

        /// <summary>
        ///
        /// </summary>
        /// <param name="NewContentControl"></param>
        /// <param name="InUndoRedo"></param>
        private void OnContentControlAfterAdd(Word.ContentControl NewContentControl, bool InUndoRedo)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module}");

                if (!InUndoRedo && !string.IsNullOrEmpty(NewContentControl.Tag))
                {
                    Debug.WriteLine("  Looking for " + NewContentControl.Tag);

                    Word.Document doc = NewContentControl.Application.ActiveDocument;
                    Word.Application app = Globals.Chem4WordV3.Application;
                    CustomXMLPart cxml = CustomXmlPartHelper.GetCustomXmlPart(NewContentControl.Tag, app.ActiveDocument);
                    if (cxml == null)
                    {
                        if (doc.Application.Documents.Count > 1)
                        {
                            Word.Application app1 = Globals.Chem4WordV3.Application;
                            cxml = CustomXmlPartHelper.FindCustomXmlPart(NewContentControl.Tag, app1.ActiveDocument);
                            if (cxml != null)
                            {
                                // Generate new molecule Guid and apply it
                                string newGuid = Guid.NewGuid().ToString("N");
                                NewContentControl.Tag = newGuid;

                                CMLConverter cmlConverter = new CMLConverter();
                                Model.Model model = cmlConverter.Import(cxml.XML);
                                model.CustomXmlPartGuid = newGuid;
                                doc.CustomXMLParts.Add(cmlConverter.Export(model));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contentControl"></param>
        /// <param name="Cancel"></param>
        private void OnContentControlBeforeDelete(Word.ContentControl contentControl, bool Cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({contentControl.Application.ActiveDocument.Name})")}");
                //Debug.WriteLine("CC ID: " + contentControl.ID + " Tag: " + contentControl.Tag + " Title: " + contentControl.Title);
                //WordInterop.Document doc = contentControl.Application.ActiveApp;
                //CustomXMLPart cxml = GetCustomXmlPart(contentControl.Tag);
                //if (cxml != null)
                //{
                //    cxml.Delete();
                //}
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contentControl"></param>
        /// <param name="Cancel"></param>
        private void OnContentControlOnExit(Word.ContentControl contentControl, ref bool Cancel)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({contentControl.Application.ActiveDocument.Name})")}");
                if (EventsEnabled)
                {
                    ////Debug.WriteLine("CC ID: " + contentControl.ID + " Tag: " + contentControl.Tag + " Title: " + contentControl.Title);
                    //Word.Document doc = contentControl.Application.ActiveApp;
                    //Word.Selection sel = doc.Application.Selection;
                    //Debug.WriteLine("  Selection: from " + sel.Range.Start + " to " + sel.Range.End);
                    //SelectChemistry(sel);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contentControl"></param>
        private void OnContentControlOnEnter(Word.ContentControl contentControl)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Debug.WriteLine($"{module.Replace("()", $"({contentControl.Application.ActiveDocument.Name})")}");

                if (EventsEnabled)
                {
                    Word.Document doc = contentControl.Application.ActiveDocument;
                    Word.Selection sel = doc.Application.Selection;
                    Debug.WriteLine("  Selection: from " + sel.Range.Start + " to " + sel.Range.End);

                    SelectChemistry(sel);
                    if (ChemistrySelected)
                    {
                        Ribbon.ActivateChemistryTab();
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, WordTopLeft, module, ex).ShowDialog();
            }
        }

        #endregion Content Control Events

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new EventHandler(C4WLiteAddIn_Startup);
            this.Shutdown += new EventHandler(C4WLiteAddIn_Shutdown);
        }

        #endregion VSTO generated code
    }

    public enum ButtonState
    {
        NoDocument,
        CanEdit,
        CanInsert
    }
}