// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Searcher.ChEBIPlugin.Properties;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Point = System.Windows.Point;

namespace Chem4Word.Searcher.ChEBIPlugin
{
    public class Searcher : IChem4WordSearcher
    {
        #region Fields

        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private Options _searcherOptions = new Options();

        #endregion Fields

        #region Constructors

        public Searcher()
        {
            //nothing to do here
        }

        #endregion Constructors

        #region Properties

        public string Cml { get; set; }
        public string Description => "Searches the Chemical Entities of Biological Interest database.";

        public int DisplayOrder
        {
            get
            {
                LoadSettings();

                return _searcherOptions.DisplayOrder;
            }
        }

        public bool HasSettings => true;
        public Image Image => Resources.chebi;
        public string Name => "ChEBI Search PlugIn";
        public string ProductAppDataPath { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public string ShortName => "ChEBI";
        public IChem4WordTelemetry Telemetry { get; set; }
        public Point TopLeft { get; set; }

        #endregion Properties

        #region Methods

        public bool ChangeSettings(Point topLeft)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Telemetry.Write(module, "Verbose", "Called");
                if (HasSettings)
                {
                    LoadSettings();
                }

                Settings settings = new Settings();
                settings.Telemetry = Telemetry;
                settings.TopLeft = topLeft;

                Options tempOptions = _searcherOptions.Clone();
                settings.SettingsPath = ProductAppDataPath;
                settings.SearcherOptions = tempOptions;

                DialogResult dr = settings.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _searcherOptions = tempOptions.Clone();
                }
                settings.Close();
                settings = null;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
            return true;
        }

        public void LoadSettings()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!string.IsNullOrEmpty(ProductAppDataPath))
                {
                    // Load User Options
                    string fileName = $"{_product}.json";
                    string optionsFile = Path.Combine(ProductAppDataPath, fileName);
                    if (File.Exists(optionsFile))
                    {
                        string json = File.ReadAllText(optionsFile);
                        _searcherOptions = JsonConvert.DeserializeObject<Options>(json);
                        string temp = JsonConvert.SerializeObject(_searcherOptions, Formatting.Indented);
                        if (!json.Equals(temp))
                        {
                            File.WriteAllText(optionsFile, temp);
                        }
                    }
                    else
                    {
                        _searcherOptions.RestoreDefaults();
                        string json = JsonConvert.SerializeObject(_searcherOptions, Formatting.Indented);
                        File.WriteAllText(optionsFile, json);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        public DialogResult Search()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                using (new WaitCursor())
                {
                    var searcher = new SearchChEBI();
                    searcher.TopLeft = TopLeft;
                    searcher.Telemetry = Telemetry;
                    searcher.ProductAppDataPath = ProductAppDataPath;
                    searcher.UserOptions = _searcherOptions;
                    using (new WaitCursor(Cursors.Default))
                    {
                        DialogResult dr = searcher.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            Properties = new Dictionary<string, string>();
                            Telemetry.Write(module, "Information", $"Importing Id {searcher.ChebiId}");
                            Cml = searcher.Cml;
                        }

                        return dr;
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
                return DialogResult.Cancel;
            }
        }

        #endregion Methods
    }
}