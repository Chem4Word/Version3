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
    public partial class UcEditName : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private UcMoleculeLabelEditor _parent;

        public UcEditName()
        {
            InitializeComponent();
        }

        public UcEditName(UcMoleculeLabelEditor parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void UcEditName_Load(object sender, EventArgs e)
        {
            //
        }

        public string Id { get; set; }

        public bool IsLoading { get; set; }

        public bool CanDelete
        {
            get
            {
                return btnDeleteName.Enabled;
            }
            set
            {
                btnDeleteName.Enabled = value;
            }
        }

        public bool CanEdit
        {
            get
            {
                return txtName.ReadOnly;
            }
            set
            {
                txtName.ReadOnly = !value;
            }
        }

        public string DictRef
        {
            get
            {
                return txtDictRef.Text;
            }
            set
            {
                if (txtDictRef != null)
                {
                    txtDictRef.Text = value;
                    SetIcon();
                }
            }
        }

        public string NameText
        {
            get
            {
                return txtName.Text;
            }
            set
            {
                if (txtName != null)
                {
                    txtName.Text = value;
                }
                SetIcon();
            }
        }

        private void SetIcon()
        {
            if (string.IsNullOrEmpty(txtName.Text) || txtName.Text.Contains("<") || txtName.Text.Contains(">"))
            {
                pbNameCheck.Image = Resources.LabelError;
            }
            else
            {
                pbNameCheck.Image = Resources.LabelValid;
            }
        }

        private void OnDeleteNameClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");

            try
            {
                _parent.DeleteChemicalNameAt(Id);
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!IsLoading)
                {
                    _parent.ChangeNameValueAt(Id, txtName.Text);
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