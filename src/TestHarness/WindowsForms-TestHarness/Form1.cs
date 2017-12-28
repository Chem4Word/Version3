// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using Chem4Word.Model.Converters;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WinFormsTestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("All molecule files (*.mol, *.cml)|*.mol;*.cml");
            sb.Append("|CML molecule files (*.cml)|*.cml");
            sb.Append("|MDL molecule files (*.mol)|*.mol");

            openFileDialog1.Filter = sb.ToString();

            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string fileType = Path.GetExtension(openFileDialog1.FileName).ToLower();
                string filename = Path.GetFileName(openFileDialog1.FileName);
                string mol = File.ReadAllText(openFileDialog1.FileName);
                Model model = null;

                switch (fileType)
                {
                    case ".cml":
                        CMLConverter cmlConverter = new CMLConverter();
                        model = cmlConverter.Import(mol);
                        break;

                    case ".mol":
                        SdFileConverter molfileConverter = new SdFileConverter();
                        model = molfileConverter.Import(mol);
                        break;
                }

                string fCml = "";
                string fCalc = "";
                if (model != null)
                {
                    this.Text = filename;
                    foreach (var molecule in model.Molecules)
                    {
                        if (!string.IsNullOrEmpty(molecule.ConciseFormula))
                        {
                            fCml += $"{molecule.ConciseFormula} . ";
                        }
                        fCalc += $"{molecule.CalculatedFormula()} . ";
                    }

                    if (fCalc.EndsWith(" . "))
                    {
                        fCalc = fCalc.Substring(0, fCalc.Length - 3);
                    }
                    if (fCml.EndsWith(" . "))
                    {
                        fCml = fCml.Substring(0, fCml.Length - 3);
                    }
                    lblCalculated.Text = $"{fCalc}";
                    lblFromCml.Text = $"{fCml}";
                    lblOverall.Text = $"{model.ConciseFormula}";

                    flexDisplayControl1.Chemistry = mol;
                }
            }
        }
    }
}