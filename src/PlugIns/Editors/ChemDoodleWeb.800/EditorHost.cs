// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Wpf;
using Chem4Word.Model.Converters.CML;
using Chem4Word.Model.Converters.Json;
using IChem4Word.Contracts;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.Editor.ChemDoodleWeb800
{
    public partial class EditorHost : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public System.Windows.Point TopLeft { get; set; }

        public DialogResult Result = DialogResult.Cancel;

        public IChem4WordTelemetry Telemetry { get; set; }

        public string ProductAppDataPath { get; set; }

        public Options UserOptions { get; set; }

        private string _cml;

        public string OutputValue { get; set; }

        public EditorHost(string cml)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            _cml = cml;
            InitializeComponent();
        }

        private void EditorHost_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Cursor.Current = Cursors.WaitCursor;

            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            CMLConverter cc = new CMLConverter();
            JSONConverter jc = new JSONConverter();
            Model.Model model = cc.Import(_cml);

            WpfChemDoodle editor = new WpfChemDoodle();
            editor.Telemetry = Telemetry;
            editor.ProductAppDataPath = ProductAppDataPath;
            editor.UserOptions = UserOptions;
            editor.TopLeft = TopLeft;

            editor.StructureJson = jc.Export(model);
            editor.IsSingleMolecule = model.Molecules.Count == 1;
            editor.AverageBondLength = model.MeanBondLength;

            editor.InitializeComponent();
            elementHost1.Child = editor;
            editor.OnButtonClick += OnWpfButtonClick;

            this.Show();
            Application.DoEvents();
        }

        private void OnWpfButtonClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            WpfEventArgs args = (WpfEventArgs)e;

            if (args.Button.ToUpper().Equals("OK"))
            {
                DialogResult = DialogResult.OK;
                CMLConverter cc = new CMLConverter();
                JSONConverter jc = new JSONConverter();
                Model.Model model = jc.Import(args.OutputValue);
                OutputValue = cc.Export(model);
            }

            if (args.Button.ToUpper().Equals("CANCEL"))
            {
                DialogResult = DialogResult.Cancel;
                OutputValue = "";
            }

            WpfChemDoodle editor = elementHost1.Child as WpfChemDoodle;
            if (editor != null)
            {
                editor.OnButtonClick -= OnWpfButtonClick;
                editor = null;
            }
            Hide();
        }
    }
}