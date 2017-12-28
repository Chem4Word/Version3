// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using Chem4Word.Properties;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.UI.UserControls
{
    public partial class UcEditFormula : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private UcMoleculeLabelEditor _parent;

        public UcEditFormula(UcMoleculeLabelEditor parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public UcEditFormula()
        {
            InitializeComponent();
        }

        private void UcEditFormula_Load(object sender, EventArgs e)
        {
            //
        }

        public string Id { get; set; }

        public bool IsLoading { get; set; }

        public string Convention
        {
            get
            {
                return txtConvention.Text;
            }
            set
            {
                if (txtConvention != null)
                {
                    txtConvention.Text = value;
                    SetIcon();
                }
            }
        }

        public string FormulaText
        {
            get
            {
                return txtFormula.Text;
            }
            set
            {
                if (txtFormula != null)
                {
                    txtFormula.Text = value;
                }
                SetIcon();
            }
        }

        public bool CanEdit
        {
            get
            {
                return txtFormula.ReadOnly;
            }
            set
            {
                txtFormula.ReadOnly = !value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return btnDeleteFormula.Enabled;
            }
            set
            {
                btnDeleteFormula.Enabled = value;
            }
        }

        private void SetIcon()
        {
            if (string.IsNullOrEmpty(txtFormula.Text) || txtFormula.Text.Contains("<") || txtFormula.Text.Contains(">"))
            {
                pbFormulaCheck.Image = Resources.LabelError;
            }
            else
            {
                pbFormulaCheck.Image = Resources.LabelValid;
            }
        }

        private void OnDeleteFormulaClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                _parent.DeleteFourmulaAt(Id);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void txtFormula_TextChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!IsLoading)
                {
                    _parent.ChangeFormulaValueAt(Id, txtFormula.Text);
                    SetIcon();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}