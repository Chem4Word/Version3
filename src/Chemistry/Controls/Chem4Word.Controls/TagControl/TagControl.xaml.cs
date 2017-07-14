using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Chem4Word.Controls.TagControl
{
    /// <summary>
    ///     Interaction logic for TagControl.xaml
    /// </summary>
    public partial class TagControl : UserControl
    {
        public TagControl()
        {
            InitializeComponent();

            // Just create a model to work with
            TagControlModel = new TagControlModel(KnownTags, Tags);

            // Use the model for data binding to the control
            MainGrid.DataContext = TagControlModel;
        }

        public ObservableCollection<string> Tags = new ObservableCollection<string>();
        public ObservableCollection<string> KnownTags = new ObservableCollection<string>();

        public TagControlModel TagControlModel
        {
            get { return (TagControlModel)GetValue(TagControlModelProperty); }
            set { SetValue(TagControlModelProperty, value); }
        }

        public static readonly DependencyProperty TagControlModelProperty = DependencyProperty.Register("TagControlModel", typeof(TagControlModel), typeof(TagControl));
    }
}