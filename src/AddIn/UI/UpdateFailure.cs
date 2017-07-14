using System;
using System.Windows.Forms;

namespace Chem4Word.UI
{
    public partial class UpdateFailure : Form
    {
        public string WebPage { get; set; }

        public System.Windows.Point TopLeft { get; set; }

        public UpdateFailure()
        {
            InitializeComponent();
        }

        private void UpdateFailure_Load(object sender, EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            webBrowser1.DocumentText = WebPage;
        }
    }
}