// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Model;
using Chem4Word.Model.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.UI.UserControls
{
    public partial class UcMoleculeLabelEditor : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private int _maxFormulaId;
        private int _maxNameId;
        private string _cml;

        public Molecule Molecule { get; set; }
        public List<string> Used1D { get; set; }

        public UcMoleculeLabelEditor()
        {
            InitializeComponent();
        }

        private int MaxId(string id, int current)
        {
            int result = current;

            string[] parts = id.Split('.');

            if (parts.Length > 1)
            {
                string s = parts[1].Replace("n", "").Replace("f", "");
                int n;
                if (int.TryParse(s, out n))
                {
                    result = Math.Max(current, n);
                }
            }

            return result;
        }

        private void RefreshFormulaePanel()
        {
            panelFormulae.AutoScroll = false;
            panelFormulae.Controls.Clear();

            int i = 0;
            foreach (var f in Molecule.Formulas)
            {
                UcEditFormula elc = new UcEditFormula(this);
                elc.IsLoading = true;
                elc.Id = f.Id;
                _maxFormulaId = MaxId(f.Id, _maxFormulaId);
                elc.Parent = panelFormulae;
                elc.Location = new Point(5, 5 + i * elc.ClientRectangle.Height);
                elc.Width = panelFormulae.Width - 10;
                elc.FormulaText = f.Inline;
                elc.CanDelete = !Used1D.Any(s => s.StartsWith(f.Id));
                elc.CanEdit = f.Convention.Equals(Constants.Chem4WordUserFormula);
                elc.Convention = f.Convention;
                elc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                elc.IsLoading = false;
                i++;
            }

            panelFormulae.AutoScroll = true;
        }

        private void RefreshNamesPanel()
        {
            panelNames.AutoScroll = false;
            panelNames.Controls.Clear();

            int i = 0;
            foreach (var n in Molecule.ChemicalNames)
            {
                UcEditName elc = new UcEditName(this);
                elc.Id = n.Id;
                _maxNameId = MaxId(n.Id, _maxNameId);
                elc.Parent = panelNames;
                elc.Location = new Point(5, 5 + i * elc.ClientRectangle.Height);
                elc.Width = panelNames.Width - 10;
                elc.DictRef = n.DictRef;
                elc.NameText = n.Name;
                elc.CanDelete = !Used1D.Any(s => s.StartsWith(n.Id));
                elc.CanEdit = n.DictRef.Equals(Constants.Chem4WordUserSynonym);
                elc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                i++;
            }

            panelNames.AutoScroll = true;
        }

        public void RefreshPanels()
        {
            RefreshFormulaePanel();
            RefreshNamesPanel();
            if (string.IsNullOrEmpty(_cml))
            {
                Model.Model model = new Model.Model();
                model.Molecules.Add(Molecule);
                CMLConverter cmlConverter = new CMLConverter();
                _cml = cmlConverter.Export(model);
                flexDisplayControl1.Chemistry = _cml;
            }
        }

        private bool StringIsValid(string input)
        {
            return string.IsNullOrEmpty(input) || input.Contains("<") || input.Contains(">");
        }

        public void ChangeFormulaValueAt(string id, string value)
        {
            foreach (Formula f in Molecule.Formulas)
            {
                if (f.Id.Equals(id))
                {
                    f.Inline = value;
                    f.IsValid = !StringIsValid(value);
                    break;
                }
            }
        }

        public void DeleteFourmulaAt(string id)
        {
            foreach (Formula f in Molecule.Formulas.ToList())
            {
                if (f.Id.Equals(id))
                {
                    Molecule.Formulas.Remove(f);
                    RefreshFormulaePanel();
                    break;
                }
            }
        }

        public void ChangeNameValueAt(string id, string name)
        {
            foreach (ChemicalName n in Molecule.ChemicalNames)
            {
                if (n.Id.Equals(id))
                {
                    n.Name = name;
                    n.IsValid = !StringIsValid(name);
                    break;
                }
            }
        }

        public void DeleteChemicalNameAt(string id)
        {
            foreach (ChemicalName n in Molecule.ChemicalNames.ToList())
            {
                if (n.Id.Equals(id))
                {
                    Molecule.ChemicalNames.Remove(n);
                    RefreshNamesPanel();
                    break;
                }
            }
        }

        private void UcMoleculeLabelEditor_Load(object sender, EventArgs e)
        {
            // Do Nothing
        }

        private void OnAddNameClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                ChemicalName n = new ChemicalName();
                n.DictRef = Constants.Chem4WordUserSynonym;
                n.Name = "";
                n.Id = $"{Molecule.Id}.n{++_maxNameId}";
                Molecule.ChemicalNames.Add(n);
                RefreshNamesPanel();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void OnAddFormulaClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                Formula f = new Formula();
                f.Convention = Constants.Chem4WordUserFormula;
                f.Inline = "";
                f.Id = $"{Molecule.Id}.f{++_maxFormulaId}";
                Molecule.Formulas.Add(f);
                RefreshFormulaePanel();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}