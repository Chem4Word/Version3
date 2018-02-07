// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using IChem4Word.Contracts;
using System;
using System.Windows.Forms;

namespace Chem4Word.Core.UI.Forms
{
    public partial class ReportError : Form
    {
        private IChem4WordTelemetry _telemetry;
        private string _exceptionMessage = string.Empty;
        private string _operation = string.Empty;
        private string _callStack = string.Empty;

        public System.Windows.Point TopLeft { get; set; }

        public ReportError(IChem4WordTelemetry telemetry, System.Windows.Point topLeft, string operation, Exception ex)
        {
            InitializeComponent();

            try
            {
                TopLeft = topLeft;
                _telemetry = telemetry;

                _operation = operation;
                _callStack = ex.ToString();
                _exceptionMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    _exceptionMessage += Environment.NewLine + ex.InnerException.Message;
                }
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        private void ErrorReport_Load(object sender, EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            try
            {
                textBox1.Text = _exceptionMessage;
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ReportError_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(_exceptionMessage))
            {
                _telemetry.Write(_operation, "Exception", _exceptionMessage);
            }
            if (!string.IsNullOrEmpty(_callStack))
            {
                _telemetry.Write(_operation, "Exception", _callStack);
            }

            if (DialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(EmailAddress.Text))
                {
                    _telemetry.Write(_operation, "Exception(Data)", EmailAddress.Text);
                }
                if (!string.IsNullOrEmpty(richTextBox1.Text))
                {
                    _telemetry.Write(_operation, "Exception(Data)", richTextBox1.Text);
                }
            }
        }
    }
}