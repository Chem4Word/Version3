// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Model.Converters;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Chem4Word.Model.Converters.CML;
using Chem4Word.Model.Converters.Json;

namespace Chem4Word.Editor.ChemDoodleWeb702
{
    public class Editor : IChem4WordEditor
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public string Name => "ChemDoodle Web Structure Editor V7.0.2";

        public string Description => "The standard free editor using the ChemDoodle Web 7.0.2 (JavaScript) structure editor";

        public bool HasSettings => true;

        public Point TopLeft { get; set; }

        public string Cml { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public IChem4WordTelemetry Telemetry { get; set; }

        /// <summary>
        /// Where to find the settings for this Plug In
        /// </summary>
        public string ProductAppDataPath { get; set; }

        private Options _editorOptions = new Options();

        public Editor()
        {
            // Nothing to do here
        }

        public void LoadSettings()
        {
            if (!string.IsNullOrEmpty(ProductAppDataPath))
            {
                // Load User Options
                string fileName = $"{_product}.json";
                string optionsFile = Path.Combine(ProductAppDataPath, fileName);
                if (File.Exists(optionsFile))
                {
                    string json = File.ReadAllText(optionsFile);
                    _editorOptions = JsonConvert.DeserializeObject<Options>(json);
                    string temp = JsonConvert.SerializeObject(_editorOptions, Formatting.Indented);
                    if (!json.Equals(temp))
                    {
                        File.WriteAllText(optionsFile, temp);
                    }
                }
                else
                {
                    _editorOptions.RestoreDefaults();
                    string json = JsonConvert.SerializeObject(_editorOptions, Formatting.Indented);
                    File.WriteAllText(optionsFile, json);
                }
            }
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

                Options tempOptions = _editorOptions.Clone();
                settings.SettingsPath = ProductAppDataPath;
                settings.EditorOptions = tempOptions;

                DialogResult dr = settings.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _editorOptions = tempOptions.Clone();
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

        public DialogResult Edit()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            DialogResult result = DialogResult.Cancel;

            try
            {
                Telemetry.Write(module, "Verbose", "Called");
                if (HasSettings)
                {
                    LoadSettings();
                }

                CMLConverter cmlConverter = new CMLConverter();
                Model.Model model = cmlConverter.Import(Cml);
                JSONConverter jsonConverter = new JSONConverter();
                string json = jsonConverter.Export(model);

                ChemDoodleWeb cdwEditor = new ChemDoodleWeb();
                cdwEditor.TopLeft = TopLeft;
                cdwEditor.Telemetry = Telemetry;
                cdwEditor.ProductAppDataPath = ProductAppDataPath;
                cdwEditor.UserOptions = _editorOptions;
                cdwEditor.Before_JSON = json;
                cdwEditor.IsSingleMolecule = model.Molecules.Count == 1;
                cdwEditor.AverageBondLength = model.MeanBondLength;

                result = cdwEditor.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Properties = new Dictionary<string, string>();
                    Properties.Add("Before_Formula", cdwEditor.Before_Formula);
                    Properties.Add("After_Formula", cdwEditor.After_Formula);
                    json = cdwEditor.After_JSON;
                    cdwEditor.Close();
                    model = jsonConverter.Import(json);
                    Cml = cmlConverter.Export(model);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }

            return result;
        }
    }
}