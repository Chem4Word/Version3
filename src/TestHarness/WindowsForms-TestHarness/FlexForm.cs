﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Chem4Word.Model;
using Chem4Word.Model.Converters.CML;
using Chem4Word.Model.Converters.MDL;

namespace WinFormsTestHarness
{
    public partial class FlexForm : Form
    {
        public FlexForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("All molecule files (*.mol, *.sdf, *.cml)|*.mol;*.sdf;*.cml");
            sb.Append("|CML molecule files (*.cml)|*.cml");
            sb.Append("|MDL molecule files (*.mol, *.sdf)|*.mol;*.sdf");

            openFileDialog1.FileName = "*.*";
            openFileDialog1.Filter = sb.ToString();

            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string fileType = Path.GetExtension(openFileDialog1.FileName).ToLower();
                string filename = Path.GetFileName(openFileDialog1.FileName);
                string mol = File.ReadAllText(openFileDialog1.FileName);
                string cml = "";

                CMLConverter cmlConvertor = new CMLConverter();
                SdFileConverter sdFileConverter = new SdFileConverter();
                Model model = null;

                switch (fileType)
                {
                    case ".mol":
                    case ".sdf":
                        model = sdFileConverter.Import(mol);
                        model.RefreshMolecules();
                        model.Relabel();
                        cml = cmlConvertor.Export(model);
                        //model.DumpModel("After Import");

                        break;

                    case ".cml":
                    case ".xml":
                        model = cmlConvertor.Import(mol);
                        model.RefreshMolecules();
                        model.Relabel();
                        cml = cmlConvertor.Export(model);
                        break;
                }

                this.Text = filename;
                this.display1.Chemistry = cml;
            }
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }
    }
}