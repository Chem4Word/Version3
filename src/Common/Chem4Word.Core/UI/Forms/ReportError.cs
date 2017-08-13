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

        // ToDo: Change this to pass in Exception and see if it has an inner exception.
        public ReportError(IChem4WordTelemetry telemetry, System.Windows.Point topLeft, string operation, Exception ex)
        {
            InitializeComponent();

            try
            {
                TopLeft = topLeft;
                _telemetry = telemetry;

                _operation = operation;
                _callStack = ex.StackTrace;
                _exceptionMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    _exceptionMessage += Environment.NewLine + ex.InnerException.Message;
                    _callStack += Environment.NewLine + ex.InnerException.StackTrace;
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
            string answer = richTextBox1.Text;

            if (!string.IsNullOrEmpty(_exceptionMessage))
            {
                _telemetry.Write(_operation, "Exception(Data)", _exceptionMessage);
            }
            if (!string.IsNullOrEmpty(_callStack))
            {
                _telemetry.Write(_operation, "Exception(Data)", _callStack);
            }
            if (DialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(answer))
                {
                    _telemetry.Write(_operation, "Exception(Data)", answer);
                }
            }
        }
    }
}