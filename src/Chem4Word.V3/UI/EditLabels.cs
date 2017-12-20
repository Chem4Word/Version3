// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Helpers;
using Chem4Word.Model;
using Chem4Word.Model.Converters;
using Chem4Word.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.UI
{
    public partial class EditLabels : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public System.Windows.Point TopLeft { get; set; }

        public string Cml { get; set; }

        public List<string> Used1D { get; set; }

        public string Message
        {
            set { label1.Text = value; }
        }

        private Model.Model _model;
        private CMLConverter _cmlConvertor = new CMLConverter();

        public EditLabels()
        {
            InitializeComponent();
        }

        private void FormEditLabels_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                // Load Cml into model
                _model = _cmlConvertor.Import(Cml);

                #region Display Overall Concise Formula

                rtbConcise.SelectionFont = new Font("Tahoma", 10);
                rtbConcise.Text = "Overall Concise formula for this structure is ";
                rtbConcise.SelectionStart = rtbConcise.TextLength;
                List<FormulaPart> parts = FormulaHelper.Parts(_model.ConciseFormula);
                foreach (var part in parts)
                {
                    switch (part.Count)
                    {
                        case 0: // Seperator or multiplier
                        case 1: // No Subscript
                            rtbConcise.SelectionFont = new Font("Tahoma", 10);
                            rtbConcise.SelectionCharOffset = 0;
                            rtbConcise.AppendText(part.Atom);
                            break;

                        default: // With Subscript
                            rtbConcise.SelectionFont = new Font("Tahoma", 10);
                            rtbConcise.SelectionCharOffset = 0;
                            rtbConcise.AppendText(part.Atom);

                            rtbConcise.SelectionFont = new Font("Tahoma", 7);
                            rtbConcise.SelectionCharOffset = -5;
                            rtbConcise.AppendText($"{part.Count}");
                            break;
                    }
                }

                #endregion Display Overall Concise Formula

                LoadTabsControls();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void LoadTabsControls()
        {
            this.SuspendLayout();

            int idx = 0;
            foreach (var molecule in _model.Molecules)
            {
                if (idx == 0)
                {
                    tabPage_m1.Text = molecule.ConciseFormula;
                    ucMoleculeLabelEditor_m1.Molecule = molecule;
                    ucMoleculeLabelEditor_m1.Used1D = Used1D;
                    ucMoleculeLabelEditor_m1.RefreshPanels();
                }
                else
                {
                    TabPage tp = new TabPage(molecule.ConciseFormula);
                    tp.Name = tabPage_m1.Name.Replace("_m1", $"_{molecule.Id}");
                    tp.Font = tabPage_m1.Font;
                    tp.Location = tabPage_m1.Location;
                    tp.Size = tabPage_m1.Size;
                    tp.BorderStyle = tabPage_m1.BorderStyle;
                    tp.BackColor = tabPage_m1.BackColor;

                    UcMoleculeLabelEditor mle = new UcMoleculeLabelEditor();
                    mle.Name = ucMoleculeLabelEditor_m1.Name.Replace("_m1", $"_{molecule.Id}");
                    mle.Font = ucMoleculeLabelEditor_m1.Font;
                    mle.Dock = ucMoleculeLabelEditor_m1.Dock;
                    mle.Molecule = molecule;
                    mle.Used1D = Used1D;
                    mle.RefreshPanels();

                    tp.Controls.Add(mle);
                    tabControlEx1.TabPages.Add(tp);
                }
                idx++;
            }

            this.ResumeLayout();
        }

        private bool CanSave()
        {
            bool isValid = true;

            foreach (var molecule in _model.Molecules)
            {
                foreach (Formula formula in molecule.Formulas)
                {
                    if (formula.Convention.Equals(Constants.Chem4WordUserFormula))
                    {
                        if (!formula.IsValid)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid)
                {
                    foreach (ChemicalName chemicalName in molecule.ChemicalNames)
                    {
                        if (chemicalName.DictRef.Equals(Constants.Chem4WordUserSynonym))
                        {
                            if (!chemicalName.IsValid)
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                if (CanSave())
                {
                    Cml = _cmlConvertor.Export(_model);

                    DialogResult = DialogResult.OK;
                }
                else
                {
                    UserInteractions.InformUser("Can't save because at least one user defined label contains an error");
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void FormEditLabels_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (DialogResult != DialogResult.OK)
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}