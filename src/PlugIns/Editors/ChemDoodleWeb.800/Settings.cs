// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.UI.Forms;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Chem4Word.Editor.ChemDoodleWeb800
{
    public partial class Settings : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public System.Windows.Point TopLeft { get; set; }

        public IChem4WordTelemetry Telemetry { get; set; }

        public string SettingsPath { get; set; }

        public Options EditorOptions { get; set; }

        private bool _dirty;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Telemetry.Write(module, "Verbose", "Called");

                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }
                RestoreControls();
                _dirty = false;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowImplicitHydrogens_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                EditorOptions.ShowHydrogens = chkShowImplicitHydrogens.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkColouredAtoms_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                EditorOptions.ColouredAtoms = chkColouredAtoms.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnSetDefaults_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                DialogResult dr = UserInteractions.AskUserOkCancel("Restore default settings");
                if (dr == DialogResult.OK)
                {
                    EditorOptions.RestoreDefaults();
                    RestoreControls();
                    _dirty = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                SaveChanges();
                _dirty = false;
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(EditorOptions, Formatting.Indented);

            string fileName = $"{_product}.json";
            string optionsFile = Path.Combine(SettingsPath, fileName);
            File.WriteAllText(optionsFile, json);
        }

        private void RestoreControls()
        {
            chkColouredAtoms.Checked = EditorOptions.ColouredAtoms;
            chkShowImplicitHydrogens.Checked = EditorOptions.ShowHydrogens;
            chkShowCarbons.Checked = EditorOptions.ShowCarbons;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
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
                    DialogResult dr = UserInteractions.AskUserYesNoCancel(sb.ToString());
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
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowCarbons_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                EditorOptions.ShowCarbons = chkShowCarbons.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }
    }
}