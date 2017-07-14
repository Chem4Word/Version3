using System.Windows.Controls;

namespace Chem4Word.Controls.TagControl
{
    /// <summary>
    /// Interaction logic for TagControlItem.xaml
    /// </summary>
    public partial class TagControlItem : UserControl
    {
        public TagControlItem()
        {
            InitializeComponent();
        }

        public string Label
        {
            get { return TagItemLabel.Content.ToString(); }
            set { TagItemLabel.Content = value; }
        }
    }
}