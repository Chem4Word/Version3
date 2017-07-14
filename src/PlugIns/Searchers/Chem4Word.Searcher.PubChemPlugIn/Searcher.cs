﻿using Chem4Word.Core.UI.Forms;
using Chem4Word.Searcher.PubChemPlugIn.Properties;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Point = System.Windows.Point;

namespace Chem4Word.Searcher.PubChemPlugIn
{
    public class Searcher : IChem4WordSearcher
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private Options _searcherOptions = new Options();

        public bool HasSettings => true;

        public string ShortName => "Pubchem";
        public string Name => "PubChem Search PlugIn";
        public string Description => "Searches the PubChem public database";
        public Image Image => Resources.PubChem_Logo;

        public int DisplayOrder
        {
            get
            {
                LoadSettings();
                return _searcherOptions.DisplayOrder;
            }
        }

        public Point TopLeft { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }
        public string ProductAppDataPath { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public string Cml { get; set; }

        public Searcher()
        {
            // Nothing to do here
        }

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
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
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
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        public DialogResult Search()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            DialogResult result = DialogResult.Cancel;
            try
            {
                SearchPubChem searcher = new SearchPubChem();

                searcher.TopLeft = TopLeft;
                searcher.Telemetry = Telemetry;
                searcher.ProductAppDataPath = ProductAppDataPath;
                searcher.UserOptions = _searcherOptions;

                result = searcher.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Properties = new Dictionary<string, string>();
                    Cml = searcher.Cml;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
            return result;
        }
    }
}