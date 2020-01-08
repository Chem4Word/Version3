// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Chem4Word.Core.UI.Forms;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using Point = System.Windows.Point;

namespace Chem4Word.Searcher.ExamplePlugIn
{
    public class Searcher : IChem4WordSearcher
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public int DisplayOrder => -1; // Don't Show
        public string ShortName => "Example";
        public string Name => "Example Search PlugIn";
        public string Description => "Does nothing ...";

        public bool HasSettings => true;
        public Point TopLeft { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }
        public string ProductAppDataPath { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public string Cml { get; set; }

        private Options _searcherOptions = new Options();

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
            DialogResult dr = DialogResult.Cancel;

            try
            {
                // ToDo: Set any (extra) Properties required before calling this function
                // ToDo: Set Cml property with search result
                // ToDo: Return DialogResult.OK if operation not cancelled
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }

            return dr;
        }

        public Image Image
        {
            get { return null; }
        }
    }
}