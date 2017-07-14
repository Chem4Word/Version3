using System.Windows.Forms;

namespace Chem4WordTests
{
    public partial class ControlTestForm : Form
    {
        public ControlTestForm()
        {
            InitializeComponent();
        }

        public string Chemistry
        {
            get { return (string)this.FlexDisplay1.Chemistry; }
            set { this.FlexDisplay1.Chemistry = value; }
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }
    }
}