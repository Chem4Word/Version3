// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Core.UI.Wpf;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Chem4Word.UI.WPF
{
    public partial class SettingsHost : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private bool _closedInCode = false;

        public System.Windows.Point TopLeft { get; set; }

        public Options SystemOptions
        {
            get
            {
                var sc = elementHost1.Child as SettingsControl;
                if (sc != null)
                {
                    return sc.SystemOptions;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var sc = elementHost1.Child as SettingsControl;
                if (sc != null)
                {
                    sc.SystemOptions = value;
                }
            }
        }

        public SettingsHost()
        {
            InitializeComponent();
        }

        public SettingsHost(bool runtime)
        {
            InitializeComponent();
            if (runtime)
            {
                SettingsControl sc = elementHost1.Child as SettingsControl;
                if (sc != null)
                {
                    sc.TopLeft = TopLeft;
                    sc.OnButtonClick += OnWpfButtonClick;
                }
            }
        }

        private void OnWpfButtonClick(object sender, EventArgs e)
        {
            WpfEventArgs args = (WpfEventArgs)e;
            switch (args.Button.ToLower())
            {
                case "ok":
                    DialogResult = DialogResult.OK;
                    SettingsControl sc = elementHost1.Child as SettingsControl;
                    if (sc != null)
                    {
                        SystemOptions = sc.SystemOptions;
                        SaveChanges();
                        sc.Dirty = false;
                        Hide();
                    }
                    break;

                case "cancel":
                    DialogResult = DialogResult.Cancel;
                    _closedInCode = true;
                    Hide();
                    break;
            }
        }

        private void SettingsHost_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                MinimumSize = new Size(800, 600);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private string EnsureTrailingSlash(string input)
        {
            string output = input;

            if (!output.EndsWith("/"))
            {
                output = input + "/";
            }

            return output;
        }

        private string StripTrailingSlash(string uri)
        {
            if (uri.EndsWith("/"))
            {
                uri = uri.Remove(uri.Length - 1);
            }

            return uri;
        }

        private void SaveChanges()
        {
            SystemOptions.Chem4WordWebServiceUri = StripTrailingSlash(SystemOptions.Chem4WordWebServiceUri);

            string json = JsonConvert.SerializeObject(SystemOptions, Formatting.Indented);

            string padPath = Globals.Chem4WordV3.AddInInfo.ProductAppDataPath;
            string fileName = $"{Globals.Chem4WordV3.AddInInfo.ProductName}.json";
            string optionsFile = Path.Combine(padPath, fileName);
            File.WriteAllText(optionsFile, json);
        }

        private void SettingsHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_closedInCode)
            {
                SettingsControl sc = elementHost1.Child as SettingsControl;
                if (sc != null)
                {
                    if (sc.Dirty)
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
            }
        }
    }
}