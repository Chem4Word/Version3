using Chem4Word.Core.UI.Forms;
using Chem4Word.Renderer.OoXmlV3.OOXML;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace Chem4Word.Renderer.OoXmlV3
{
    public class Renderer : IChem4WordRenderer
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public string Name => "Open Office Xml Renderer V3";
        public string Description => "This is the standard renderer used in Chem4Word V3";
        public bool HasSettings => true;

        public Point TopLeft { get; set; }
        public string ProductAppDataPath { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }

        public string Cml { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        private Options _rendererOptions = new Options();

        public Renderer()
        {
            // Nothing to do here
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
                        _rendererOptions = JsonConvert.DeserializeObject<Options>(json);
                    }
                    else
                    {
                        _rendererOptions.RestoreDefaults();
                        string json = JsonConvert.SerializeObject(_rendererOptions, Formatting.Indented);
                        File.WriteAllText(optionsFile, json);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
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

                Options tempOptions = _rendererOptions.Clone();
                settings.SettingsPath = ProductAppDataPath;
                settings.RendererOptions = tempOptions;

                DialogResult dr = settings.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _rendererOptions = tempOptions.Clone();
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

        public string Render()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string result = null;

            try
            {
                Telemetry.Write(module, "Verbose", "Called");
                if (HasSettings)
                {
                    LoadSettings();
                }

                string guid = Properties["Guid"];
                result = OoXmlFile.CreateFromCml(Cml, guid, _rendererOptions, Telemetry, TopLeft);

                //int ii = 2;
                //int dd = 0;
                //int bang = ii / dd;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }

            return result;
        }
    }
}