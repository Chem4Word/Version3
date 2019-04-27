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

namespace Chem4Word.Renderer.OoXmlV3
{
    public partial class Settings : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public System.Windows.Point TopLeft { get; set; }

        public IChem4WordTelemetry Telemetry { get; set; }

        public string SettingsPath { get; set; }

        public Options RendererOptions { get; set; }

        private bool _dirty;

        public Settings()
        {
            InitializeComponent();
        }

        private void chkClipLines_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                RendererOptions.ClipLines = chkClipLines.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowHydrogens_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                RendererOptions.ShowHydrogens = chkShowHydrogens.Checked;
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
                RendererOptions.ColouredAtoms = chkColouredAtoms.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowRingCentres_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                RendererOptions.ShowRingCentres = chkShowRingCentres.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowMoleculeBox_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                RendererOptions.ShowMoleculeBoundingBoxes = chkShowMoleculeBox.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowCharacterBox_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                RendererOptions.ShowCharacterBoundingBoxes = chkShowCharacterBox.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
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

#if DEBUG
#else
                tabControlEx.TabPages.Remove(tabDebug);
#endif
                _dirty = false;
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
            string json = JsonConvert.SerializeObject(RendererOptions, Formatting.Indented);

            string fileName = $"{_product}.json";
            string optionsFile = Path.Combine(SettingsPath, fileName);
            File.WriteAllText(optionsFile, json);
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
                    RendererOptions.RestoreDefaults();
                    RestoreControls();
                    _dirty = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void RestoreControls()
        {
            chkColouredAtoms.Checked = RendererOptions.ColouredAtoms;
            chkShowHydrogens.Checked = RendererOptions.ShowHydrogens;
            chkShowCarbons.Checked = RendererOptions.ShowCarbons;

            chkClipLines.Checked = RendererOptions.ClipLines;
            chkShowCharacterBox.Checked = RendererOptions.ShowCharacterBoundingBoxes;
            chkShowMoleculeBox.Checked = RendererOptions.ShowMoleculeBoundingBoxes;
            chkShowRingCentres.Checked = RendererOptions.ShowRingCentres;
            chkShowAtomPositions.Checked = RendererOptions.ShowAtomPositions;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
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

        private void chkShowAtomCentres_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                RendererOptions.ShowAtomPositions = chkShowAtomPositions.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void chkShowCarbonLabels_CheckedChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Telemetry.Write(module, "Action", "Triggered");
            try
            {
                RendererOptions.ShowCarbons = chkShowCarbons.Checked;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }
    }
}