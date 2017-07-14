using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chem4WordSetup
{
    public partial class TaskIndicator : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Test text displayed in the label"), Category("Custom")]
        public string Description
        {
            get { return description.Text; }
            set { description.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Progress indicitor"), Category("Custom")]
        public Image Indicator
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        public TaskIndicator()
        {
            InitializeComponent();
        }
    }
}