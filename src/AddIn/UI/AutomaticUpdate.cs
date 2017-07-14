// Created by Mike Williams - 06/01/2016
//
// -----------------------------------------------------------------------
//   Copyright (c) 2016, The Outercurve Foundation.
//   This software is released under the Apache License, Version 2.0.
//   The license and further copyright text can be found in the file LICENSE.TXT at
//   the root directory of the distribution.
// -----------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using IChem4Word.Contracts;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Chem4Word.UI
{
    public partial class AutomaticUpdate : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public XDocument NewVersions { get; set; }
        public XDocument CurrentVersion { get; set; }

        private string _downloadUrl = string.Empty;

        private IChem4WordTelemetry _telemetry;

        private bool _closedByCode = false;

        public System.Windows.Point TopLeft { get; set; }

        //private const int CP_NOCLOSE_BUTTON = 0x200;

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}

        public AutomaticUpdate(IChem4WordTelemetry telemetry)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                _telemetry = telemetry;

                InitializeComponent();

                _telemetry.Write(module, "AutomaticUpdate", "Shown");
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void OnCodeplexPageLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _telemetry.Write(module, "Audit", "Fired");

            try
            {
                Process.Start("https://chem4word.codeplex.com/releases");
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void OnRichTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _telemetry.Write(module, "Audit", "Fired");

            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void AutomaticUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_closedByCode && e.CloseReason == CloseReason.UserClosing)
                {
                    _telemetry.Write(module, "AutomaticUpdate", "User Dismissed Form");
                }
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void OnUpdateNowClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _telemetry.Write(module, "Audit", "Fired");

            try
            {
                string source = Path.Combine(Globals.Chem4WordV3.AddInInfo.DeploymentPath, "Chem4WordUpdater.exe");
                string destination = Path.Combine(Path.GetTempPath(), "Chem4WordUpdater.exe");
                File.Copy(source, destination, true);

                _telemetry.Write(module, "AutomaticUpdate", "Starting download of " + _downloadUrl);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = _downloadUrl;
                psi.FileName = destination;
                Process.Start(psi);

                _closedByCode = true;
                Close();
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void OnUpdateLaterClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _telemetry.Write(module, "Audit", "Fired");

            try
            {
                _telemetry.Write(module, "AutomaticUpdate", "User Defered Update");
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void AutomaticUpdate_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                string currentVersionNumber = CurrentVersion.Root.Element("Number").Value;
                DateTime currentReleaseDate = SafeDate.Parse(CurrentVersion.Root.Element("Released").Value);

                lblInfo.Text = "Your current version of Chem4Word is " + currentVersionNumber + "; Released " + currentReleaseDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                _telemetry.Write(module, "AutomaticUpdate", lblInfo.Text);

                var versions = NewVersions.XPathSelectElements("//Version");
                foreach (var version in versions)
                {
                    if (string.IsNullOrEmpty(_downloadUrl))
                    {
                        _downloadUrl = version.Element("Url").Value;
                    }

                    var thisVersionNumber = version.Element("Number").Value;
                    DateTime thisVersionDate = SafeDate.Parse(version.Element("Released").Value);

                    if (currentReleaseDate >= thisVersionDate)
                    {
                        break;
                    }

                    AddHeaderLine("Version " + thisVersionNumber + "; Released " + thisVersionDate.ToString("dd-MMM-yyyy"), Color.Blue);
                    var changes = version.XPathSelectElements("Changes/Change");
                    foreach (var change in changes)
                    {
                        if (change.Value.StartsWith("Note:"))
                        {
                            AddBulletItem(change.Value.Remove(0, 6), Color.Red);
                        }
                        else
                        {
                            AddBulletItem(change.Value, Color.Black);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(_telemetry, TopLeft, module, ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void AddHeaderLine(string line, Color colour)
        {
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.SelectionFont = new Font("Arial", 16);
            richTextBox1.SelectionColor = colour;
            richTextBox1.AppendText(line + Environment.NewLine);
        }

        private void AddLine(string line, Color colour)
        {
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.SelectionFont = new Font("Arial", 12);
            richTextBox1.SelectionColor = colour;
            richTextBox1.AppendText(line + Environment.NewLine);
        }

        private void AddBulletItem(string line, Color colour)
        {
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.BulletIndent = 25;
            richTextBox1.SelectionBullet = true;
            richTextBox1.SelectionFont = new Font("Arial", 12);
            richTextBox1.SelectionColor = colour;
            richTextBox1.AppendText(line + Environment.NewLine);
            richTextBox1.SelectionBullet = false;
        }

        private string LookForUninstall(string root, string branch, string name)
        {
            string result = string.Empty;

            try
            {
                RegistryKey key = null;
                switch (root)
                {
                    case "HKLM":
                        key = Registry.LocalMachine.OpenSubKey(branch);
                        break;

                    case "HKCU":
                        key = Registry.CurrentUser.OpenSubKey(branch);
                        break;
                }

                if (key != null)
                {
                    //string[] xx = RegistryUtility.GetSubKeys();

                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            //Debug.WriteLine("Found key " + subkeyName);
                            if (subkey != null)
                            {
                                string displayName = subkey.GetValue("DisplayName") as string;
                                //Debug.WriteLine(" Display Name is " + displayName);
                                if (!string.IsNullOrEmpty(displayName))
                                {
                                    if (displayName.ToLower().Equals(name.ToLower()))
                                    {
                                        result = subkeyName;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    key = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //
            }

            return result;
        }
    }
}