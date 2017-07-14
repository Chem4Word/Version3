using System.Collections.ObjectModel;
using System.Windows;

namespace WPFTagTestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            myTagControl.TagControlModel.AddTags(new ObservableCollection<string> { "three", "Four" });
            myTagControl.TagControlModel.AddKnownTags(new ObservableCollection<string> { "One", "Two" });
        }
    }
}