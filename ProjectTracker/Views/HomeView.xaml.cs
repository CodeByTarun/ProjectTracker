using ProjectTracker.ClassLibrary.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ProjectTracker.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        public HomeView(HomeViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void HomeMainFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }
        private void HomeMainFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }
        private void UpdateFrameDataContext(object sender)
        {
            var content = HomeMainFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = (this.DataContext as HomeViewModel).ProjectListViewModel;
        }
    }
}
