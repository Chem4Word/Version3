﻿using Chem4Word.Core;
using Chem4Word.Core.UI.Forms;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Chem4Word.Searcher.ChEBIPlugin
{
    public partial class Settings : Form
    {
        #region Fields

        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private bool _dirty;

        #endregion Fields

        #region Constructors

        public Settings()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Options SearcherOptions { get; set; }
        public string SettingsPath { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }
        public System.Windows.Point TopLeft { get; set; }

        #endregion Properties

        #region Methods

        public void RestoreControls()
        {
            txtChebiWsUri.Text = SearcherOptions.ChEBIWebServiceUri;
            nudDisplayOrder.Value = SearcherOptions.DisplayOrder;
            nudResultsPerCall.Value = SearcherOptions.MaximumResults;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SaveChanges();
                _dirty = false;
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void btnSetDefaults_Click(object sender, System.EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                DialogResult dr = UserInteractions.AskUserOkCancel("Restore default settings");
                if (dr == DialogResult.OK)
                {
                    SearcherOptions.RestoreDefaults();
                    RestoreControls();
                    _dirty = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void nudDisplayOrder_ValueChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SearcherOptions.DisplayOrder = (int)nudDisplayOrder.Value;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void nudResultsPerCall_ValueChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SearcherOptions.MaximumResults = (int)nudResultsPerCall.Value;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(SearcherOptions, Formatting.Indented);

            string fileName = $"{_product}.json";
            string optionsFile = Path.Combine(SettingsPath, fileName);
            File.WriteAllText(optionsFile, json);
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
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
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
                _dirty = false;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void txtChebiWsUri_TextChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SearcherOptions.ChEBIWebServiceUri = txtChebiWsUri.Text;
                _dirty = true;
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        #endregion Methods
    }
}