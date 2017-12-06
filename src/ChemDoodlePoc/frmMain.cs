// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Chem4Word.Model;
using Chem4Word.Model.Converters;

namespace ChemDoodlePoc
{
    public partial class frmMain : Form, IMessageFilter
    {
        public string MolStructure { get; set; }

        private bool _implicitHydrogens = false;
        //private bool _inSketchMode = false;
        private bool _eventsEnabled = false;

        #region Constants

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        //private string[] ma_ChemDoodleFormats = { "JSON", "MDL", "CML", "formula" };
        private string[] ma_ChemDoodleFormats = { "JSON", "MDL", "CML"};
        private string[] ma_ConvertFormats = { "CML", "MDL" };

        private string ms_AppTitle = "Chem4Word Editor Powered By ChemDoodle Web V";

        #endregion Constants

        private enum TextBoxFormat
        {
            MolFile,
            Cml,
            Json,
            Unknown
        }

        public frmMain()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            bool handled = false;

            if (m.Msg == WM_KEYDOWN)
            {
                //Debug.WriteLine("WM_KEYDOWN");
                if ((int)Control.ModifierKeys == (int)Keys.Control)
                {
                    //Debug.WriteLine("Contol Pressed");
                    Keys key = (Keys)(int)m.WParam & Keys.KeyCode;
                    switch (key)
                    {
                        #region Keys we are handling

                        case Keys.O: // File Open
                            //btnOpen_Click(null, null);
                            handled = true;
                            break;

                        case Keys.S: // File Save
                            //btnSave_Click(null, null);
                            handled = true;
                            break;

                        #endregion Keys we are handling

                        #region Keys we are supressing

                        case Keys.P:
                            handled = true;
                            break;

                        #endregion Keys we are supressing

                        // Pass the rest through
                        default:
                            break;
                    }
                }
            }

            return handled;
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = ms_AppTitle + ExecuteJavaScript("GetVersion");

            // Load previous mol after switching modes
            if (!string.IsNullOrEmpty(MolStructure))
            {
                ExecuteJavaScript("SetJSON", MolStructure, nudBondLength.Value);
            }
            _eventsEnabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string path = System.Environment.CurrentDirectory;

            browser.Navigate(new Uri("file:///" + path.Replace("\\", "/") + "/ChemDoodle/index.htm"));

            LoadSelector(cboGetAs, ma_ChemDoodleFormats, "JSON");
            LoadCaffeine(_implicitHydrogens);

            EnableButtons();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            object mol = null;
            switch (cboGetAs.Text)
            {
                case "MDL":
                    mol = ExecuteJavaScript("GetMolFile");
                    MolStructure = mol.ToString().Replace("\n", "\r\n");
                    break;

                case "JSON":
                    mol = ExecuteJavaScript("GetJSON");
                    string temp = mol.ToString();
                    JToken molJson = JObject.Parse(temp);
                    MolStructure = molJson.ToString();
                    break;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string molecule = txtStructure.Text;
            molecule = molecule.Replace("\r", "");
            TextBoxFormat format = GetTextBoxFormat(txtStructure);
            switch (format)
            {
                case TextBoxFormat.MolFile:
                    ExecuteJavaScript("SetMolFile", molecule, nudBondLength.Value);
                    break;

                case TextBoxFormat.Cml:
                    ExecuteJavaScript("SetCmlFile", molecule, nudBondLength.Value);
                    break;

                case TextBoxFormat.Json:
                    ExecuteJavaScript("SetJSON", molecule, nudBondLength.Value);
                    break;
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            object mol = null;
            switch (cboGetAs.Text)
            {
                case "MDL":
                    if (radSingle.Checked)
                    {
                        mol = ExecuteJavaScript("GetMolFile");
                        txtStructure.Text = mol.ToString().Replace("\n", "\r\n");
                    }
                    break;

                case "CML":
                    mol = ExecuteJavaScript("GetCmlFile");
                    txtStructure.Text = mol.ToString().Replace("\n", "\r\n");
                    txtStructure.Text = txtStructure.Text.Replace("><", ">\r\n<");
                    break;

                case "JSON":
                    mol = ExecuteJavaScript("GetJSON");
                    string temp = mol.ToString();
                    JToken molJson = JObject.Parse(temp);
                    txtStructure.Text = molJson.ToString();
                    break;

                case "formula":
                    mol = ExecuteJavaScript("GetFormula");
                    txtStructure.Text = mol.ToString().Replace("\n", "\r\n");
                    break;
            }
            EnableButtons();
        }

        private void btnConvertModel_Click(object sender, EventArgs e)
        {
            TextBoxFormat format = GetTextBoxFormat(txtStructure);
            switch (format)
            {
                case TextBoxFormat.Json:
                    JSONConverter converter1 = new JSONConverter();
                    Model model1 = converter1.Import((object)txtStructure.Text);
                    model1.RebuildMolecules();
                    model1.Relabel();
                    model1.CustomXmlPartGuid = Guid.NewGuid().ToString("N");
                    CMLConverter converter2 = new CMLConverter();
                    txtStructure.Text = converter2.Export(model1);
                    break;

                case TextBoxFormat.Cml:
                    CMLConverter converter3 = new CMLConverter();
                    Model model2 = converter3.Import((object)txtStructure.Text);
                    model2.RebuildMolecules();
                    model2.Relabel();
                    JSONConverter converter4 = new JSONConverter();
                    txtStructure.Text = converter4.Export(model2);
                    break;

                case TextBoxFormat.MolFile:
                    SdFileConverter converter5 = new SdFileConverter();
                    Model model3 = converter5.Import((object) txtStructure.Text);
                    model3.RebuildMolecules();
                    model3.Relabel();
                    CMLConverter converter6 = new CMLConverter();
                    txtStructure.Text = converter6.Export(model3);
                    break;
            }

            EnableButtons();
        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("All Molecule Files (*.mol, *.cml, *.json)|*.mol;*.cml;*.json");
            sb.Append("|");
            sb.Append("CML Molecule Files (*.cml)|*.cml");
            sb.Append("|");
            sb.Append("JSON Molecule Files (*.json)|*.json");
            sb.Append("|");
            sb.Append("MDL Molecule Files (*.mol)|*.mol");
            sb.Append("|");
            sb.Append("All Files (*.*)|*.*");
            openFile.FileName = "";
            openFile.Filter = sb.ToString();
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamReader myFile = new StreamReader(openFile.FileName);
                txtStructure.Text = myFile.ReadToEnd();
                myFile.Close();
                string fileNameOnly = Path.GetFileName(openFile.FileName);
                this.Text = ms_AppTitle + ExecuteJavaScript("GetVersion") + " - " + fileNameOnly;
            }
            EnableButtons();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFile.FileName = "";

            TextBoxFormat format = GetTextBoxFormat(txtStructure);
            switch (format)
            {
                case TextBoxFormat.Json:
                    saveFile.Filter = "JSON Molecule Files (*.json)|*.json|All Files (*.*)|*.*";
                    break;

                case TextBoxFormat.Cml:
                    saveFile.Filter = "CML Molecule Files (*.cml)|*.cml|All Files (*.*)|*.*";
                    break;

                case TextBoxFormat.MolFile:
                    saveFile.Filter = "MDL Molecule Files (*.mol)|*.mol|All Files (*.*)|*.*";
                    break;

                default:
                    saveFile.Filter = "All Files (*.*)|*.*";
                    break;
            }

            DialogResult result = saveFile.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFile.FileName, txtStructure.Text);
            }
        }

        private void btnConvertUsingBabel_Click(object sender, EventArgs e)
        {
            TextBoxFormat format = GetTextBoxFormat(txtStructure);
            switch (format)
            {
                case TextBoxFormat.Cml:
                    txtStructure.Text = ConvertUsingOpenBabel(txtStructure.Text, "cml", "mol");
                    break;

                case TextBoxFormat.MolFile:
                    txtStructure.Text = ConvertUsingOpenBabel(txtStructure.Text, "mol", "cml");
                    break;
            }
            EnableButtons();
        }

        private void btnRemoveH_Click(object sender, EventArgs e)
        {
            ExecuteJavaScript("RemoveHydrogens");
        }

        private void chkShowColouredAtoms_CheckedChanged(object sender, EventArgs e)
        {
            if (_eventsEnabled)
            {
                ExecuteJavaScript("AtomsInColour", chkShowColouredAtoms.Checked);
            }
        }

        private void chkShowHydrogenCount_CheckedChanged(object sender, EventArgs e)
        {
            if (_eventsEnabled)
            {
                if (chkShowHydrogenCount.Checked)
                {
                    ExecuteJavaScript("ShowImplicitHCount");
                }
                else
                {
                    ExecuteJavaScript("HideImplicitHCount");
                }
            }
        }

        private void btnFormula_Click(object sender, EventArgs e)
        {
            string formula = ExecuteJavaScript("GetFormula") as string;
            if (formula != null)
            {
                txtStructure.Text = formula.Replace("\n", "\r\n");
            }
        }

        private void btnAddExpliciyHydrogens_Click(object sender, EventArgs e)
        {
            string debug = ExecuteJavaScript("AddExplicitHydrogens") as string;
            if (debug != null)
            {
                //txtStructure.Text = debug.Replace("\n", "\r\n");
            }
        }

        private void nudBondLength_ValueChanged(object sender, EventArgs e)
        {
            if (_eventsEnabled)
            {
                ExecuteJavaScript("ReScale", nudBondLength.Value);
            }
        }

        #region Support

        private TextBoxFormat GetTextBoxFormat(TextBox p_TextBox)
        {
            TextBoxFormat result = TextBoxFormat.Unknown;
            string temp = p_TextBox.Text;

            if (temp.Contains("M  END"))
            {
                result = TextBoxFormat.MolFile;
            }
            if (temp.StartsWith("{"))
            {
                result = TextBoxFormat.Json;
            }
            if (temp.StartsWith("<"))
            {
                result = TextBoxFormat.Cml;
            }
            return result;
        }

        private void EnableButtons()
        {
            TextBoxFormat format = GetTextBoxFormat(txtStructure);

            switch (format)
            {
                case TextBoxFormat.Json:
                    btnConvertModel.Enabled = true;
                    btnSend.Enabled = true;
                    break;

                case TextBoxFormat.MolFile:
                    btnConvertModel.Enabled = true;
                    btnSend.Enabled = true;
                    break;

                case TextBoxFormat.Cml:
                    btnConvertModel.Enabled = true;
                    btnSend.Enabled = true;
                    break;
            }
        }

        private void LoadSelector(ComboBox p_ComboBox, string[] p_Choices, string p_SelectedItem)
        {
            p_ComboBox.Items.Clear();
            foreach (string s in p_Choices)
            {
                int idx = p_ComboBox.Items.Add(s);
                if (s.Equals(p_SelectedItem))
                {
                    p_ComboBox.SelectedIndex = idx;
                }
            }
        }

        private void LoadCaffeine(bool ImplicitHydrogens)
        {
            StringBuilder sb = new StringBuilder();
            if (ImplicitHydrogens)
            {
                sb.AppendLine("Caffeine 2D - Implicit Hydrogens");
                sb.AppendLine("C 8 H 10 N 4 O 2");
                sb.AppendLine("");
                sb.AppendLine(" 14 15  0  0  0  0  0  0  0  0999 V2000");
                sb.AppendLine("    3.7320    2.0000    0.0000 O   0  0  0  0  0  0  0  0  0  1  0  0");
                sb.AppendLine("    2.0000   -1.0000    0.0000 O   0  0  0  0  0  0  0  0  0  2  0  0");
                sb.AppendLine("    3.7320   -1.0000    0.0000 N   0  0  0  0  0  0  0  0  0  3  0  0");
                sb.AppendLine("    5.5443    0.8047    0.0000 N   0  0  0  0  0  0  0  0  0  4  0  0");
                sb.AppendLine("    2.8660    0.5000    0.0000 N   0  0  0  0  0  0  0  0  0  5  0  0");
                sb.AppendLine("    5.5443   -0.8047    0.0000 N   0  0  0  0  0  0  0  0  0  6  0  0");
                sb.AppendLine("    4.5981    0.5000    0.0000 C   0  0  0  0  0  0  0  0  0  7  0  0");
                sb.AppendLine("    4.5981   -0.5000    0.0000 C   0  0  0  0  0  0  0  0  0  8  0  0");
                sb.AppendLine("    3.7320    1.0000    0.0000 C   0  0  0  0  0  0  0  0  0  9  0  0");
                sb.AppendLine("    2.8660   -0.5000    0.0000 C   0  0  0  0  0  0  0  0  0 10  0  0");
                sb.AppendLine("    6.1279    0.0000    0.0000 C   0  0  0  0  0  0  0  0  0 11  0  0");
                sb.AppendLine("    3.7320   -2.0000    0.0000 C   0  0  0  0  0  0  0  0  0 12  0  0");
                sb.AppendLine("    5.8550    1.7552    0.0000 C   0  0  0  0  0  0  0  0  0 13  0  0");
                sb.AppendLine("    2.0000    1.0000    0.0000 C   0  0  0  0  0  0  0  0  0 14  0  0");
                sb.AppendLine("  1  9  2  0  0  0  0");
                sb.AppendLine("  2 10  2  0  0  0  0");
                sb.AppendLine("  3  8  1  0  0  0  0");
                sb.AppendLine("  3 10  1  0  0  0  0");
                sb.AppendLine("  3 12  1  0  0  0  0");
                sb.AppendLine("  4  7  1  0  0  0  0");
                sb.AppendLine("  4 11  1  0  0  0  0");
                sb.AppendLine("  4 13  1  0  0  0  0");
                sb.AppendLine("  5  9  1  0  0  0  0");
                sb.AppendLine("  5 10  1  0  0  0  0");
                sb.AppendLine("  5 14  1  0  0  0  0");
                sb.AppendLine("  6  8  1  0  0  0  0");
                sb.AppendLine("  6 11  2  0  0  0  0");
                sb.AppendLine("  7  8  2  0  0  0  0");
                sb.AppendLine("  7  9  1  0  0  0  0");
                sb.AppendLine("M  END");
            }
            else
            {
                sb.AppendLine("Caffeine 2D - Explicit Hydrogens");
                sb.AppendLine("C 8 H 10 N 4 O 2");
                sb.AppendLine("");
                sb.AppendLine(" 24 25  0  0  0  0  0  0  0  0999 V2000");
                sb.AppendLine("    3.7320    2.0000    0.0000 O   0  0  0  0  0  0  0  0  0  1  0  0");
                sb.AppendLine("    2.0000   -1.0000    0.0000 O   0  0  0  0  0  0  0  0  0  2  0  0");
                sb.AppendLine("    3.7320   -1.0000    0.0000 N   0  0  0  0  0  0  0  0  0  3  0  0");
                sb.AppendLine("    5.5443    0.8047    0.0000 N   0  0  0  0  0  0  0  0  0  4  0  0");
                sb.AppendLine("    2.8660    0.5000    0.0000 N   0  0  0  0  0  0  0  0  0  5  0  0");
                sb.AppendLine("    5.5443   -0.8047    0.0000 N   0  0  0  0  0  0  0  0  0  6  0  0");
                sb.AppendLine("    4.5981    0.5000    0.0000 C   0  0  0  0  0  0  0  0  0  7  0  0");
                sb.AppendLine("    4.5981   -0.5000    0.0000 C   0  0  0  0  0  0  0  0  0  8  0  0");
                sb.AppendLine("    3.7320    1.0000    0.0000 C   0  0  0  0  0  0  0  0  0  9  0  0");
                sb.AppendLine("    2.8660   -0.5000    0.0000 C   0  0  0  0  0  0  0  0  0 10  0  0");
                sb.AppendLine("    6.1279    0.0000    0.0000 C   0  0  0  0  0  0  0  0  0 11  0  0");
                sb.AppendLine("    3.7320   -2.0000    0.0000 C   0  0  0  0  0  0  0  0  0 12  0  0");
                sb.AppendLine("    5.8550    1.7552    0.0000 C   0  0  0  0  0  0  0  0  0 13  0  0");
                sb.AppendLine("    2.0000    1.0000    0.0000 C   0  0  0  0  0  0  0  0  0 14  0  0");
                sb.AppendLine("    6.7479    0.0000    0.0000 H   0  0  0  0  0  0  0  0  0 15  0  0");
                sb.AppendLine("    3.1120   -2.0000    0.0000 H   0  0  0  0  0  0  0  0  0 16  0  0");
                sb.AppendLine("    3.7320   -2.6200    0.0000 H   0  0  0  0  0  0  0  0  0 17  0  0");
                sb.AppendLine("    4.3520   -2.0000    0.0000 H   0  0  0  0  0  0  0  0  0 18  0  0");
                sb.AppendLine("    6.4443    1.5626    0.0000 H   0  0  0  0  0  0  0  0  0 19  0  0");
                sb.AppendLine("    6.0476    2.3446    0.0000 H   0  0  0  0  0  0  0  0  0 20  0  0");
                sb.AppendLine("    5.2656    1.9479    0.0000 H   0  0  0  0  0  0  0  0  0 21  0  0");
                sb.AppendLine("    2.3100    1.5369    0.0000 H   0  0  0  0  0  0  0  0  0 22  0  0");
                sb.AppendLine("    1.4631    1.3100    0.0000 H   0  0  0  0  0  0  0  0  0 23  0  0");
                sb.AppendLine("    1.6900    0.4631    0.0000 H   0  0  0  0  0  0  0  0  0 24  0  0");
                sb.AppendLine("  1  9  2  0  0  0  0");
                sb.AppendLine("  2 10  2  0  0  0  0");
                sb.AppendLine("  3  8  1  0  0  0  0");
                sb.AppendLine("  3 10  1  0  0  0  0");
                sb.AppendLine("  3 12  1  0  0  0  0");
                sb.AppendLine("  4  7  1  0  0  0  0");
                sb.AppendLine("  4 11  1  0  0  0  0");
                sb.AppendLine("  4 13  1  0  0  0  0");
                sb.AppendLine("  5  9  1  0  0  0  0");
                sb.AppendLine("  5 10  1  0  0  0  0");
                sb.AppendLine("  5 14  1  0  0  0  0");
                sb.AppendLine("  6  8  1  0  0  0  0");
                sb.AppendLine("  6 11  2  0  0  0  0");
                sb.AppendLine("  7  8  2  0  0  0  0");
                sb.AppendLine("  7  9  1  0  0  0  0");
                sb.AppendLine(" 11 15  1  0  0  0  0");
                sb.AppendLine(" 12 16  1  0  0  0  0");
                sb.AppendLine(" 12 17  1  0  0  0  0");
                sb.AppendLine(" 12 18  1  0  0  0  0");
                sb.AppendLine(" 13 19  1  0  0  0  0");
                sb.AppendLine(" 13 20  1  0  0  0  0");
                sb.AppendLine(" 13 21  1  0  0  0  0");
                sb.AppendLine(" 14 22  1  0  0  0  0");
                sb.AppendLine(" 14 23  1  0  0  0  0");
                sb.AppendLine(" 14 24  1  0  0  0  0");
                sb.AppendLine("M  END");
            }

            txtStructure.Text = sb.ToString();
            txtStructure.SelectionStart = 0;
            txtStructure.SelectionLength = 0;
        }

        private object ExecuteJavaScript(string p_FunctionName, params object[] p_Args)
        {
            return browser.Document.InvokeScript(p_FunctionName, p_Args);
        }

        private string ConvertUsingOpenBabel(string p_structure, string p_from, string p_to)
        {
            string result = "Something went wrong !";
            string openBabelExe = @"C:\Program Files (x86)\OpenBabel-2.3.2\babel.exe";

            bool found = false;
            if (File.Exists(openBabelExe))
            {
                found = true;
            }

            if (!found)
            {
                openBabelExe = @"C:\Program Files\OpenBabel-2.3.2\babel.exe";
                if (File.Exists(openBabelExe))
                {
                    found = true;
                }
            }

            if (found)
            {
                string tempFolder = Path.GetTempPath();
                string tempFileName = Guid.NewGuid().ToString();

                string inputFile = Path.Combine(tempFolder, tempFileName + "." + p_from);
                string outputFile = Path.Combine(tempFolder, tempFileName + "." + p_to);

                System.IO.File.WriteAllText(inputFile, p_structure);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = tempFolder;
                startInfo.FileName = openBabelExe;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = "-i" + p_from + " " + inputFile + " -o" + p_to + " " + outputFile;

                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }

                    if (File.Exists(outputFile))
                    {
                        System.IO.StreamReader myFile = new System.IO.StreamReader(outputFile);
                        result = myFile.ReadToEnd();
                        myFile.Close();
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
                finally
                {
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }
                    if (File.Exists(inputFile))
                    {
                        File.Delete(inputFile);
                    }
                }
            }
            else
            {
                result = "Open Babel not found !";
            }

            return result;
        }

        #endregion Support

        private void radSingle_CheckedChanged(object sender, EventArgs e)
        {
            //_inSketchMode = false;
            _eventsEnabled = false;
            Cursor.Current = Cursors.WaitCursor;
            if (!radSingle.Checked)
            {
                MolStructure = (string)ExecuteJavaScript("GetFirstMolJSON");
            }
            else
            {
                string path = System.Environment.CurrentDirectory;
                browser.Navigate(new Uri("file:///" + path.Replace("\\", "/") + "/ChemDoodle/index.htm"));
                chkShowHydrogenCount.Checked = true;
                chkShowColouredAtoms.Checked = true;
            }
        }

        private void radSketcher_CheckedChanged(object sender, EventArgs e)
        {
            //_inSketchMode = true;
            _eventsEnabled = false;
            Cursor.Current = Cursors.WaitCursor;
            if (!radSketcher.Checked)
            {
                MolStructure = (string)ExecuteJavaScript("GetFirstMolJSON");
            }
            else
            {
                string path = System.Environment.CurrentDirectory;
                browser.Navigate(new Uri("file:///" + path.Replace("\\", "/") + "/ChemDoodle/sketcher.htm"));
                chkShowHydrogenCount.Checked = true;
                chkShowColouredAtoms.Checked = true;
            }
        }

        private void btnFlip_Click(object sender, EventArgs e)
        {
            ExecuteJavaScript("Flip");
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            ExecuteJavaScript("Mirror");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStructure.Text = "";
        }
    }
}
