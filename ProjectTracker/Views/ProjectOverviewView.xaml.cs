using ProjectTracker.ClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectTracker.Views
{
    /// <summary>
    /// Interaction logic for ProjectOverviewView.xaml
    /// </summary>
    public partial class ProjectOverviewView : Page
    {
        public ProjectOverviewView()
        {
            InitializeComponent();
        }

        private void BoardListControlFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void BoardListControlFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void UpdateFrameDataContext(object sender)
        {
            var content = BoardListControlFrame.Content as FrameworkElement;
            if (content == null || this.DataContext == null)
                return;
            content.DataContext = (this.DataContext as ProjectOverviewViewModel).BoardListViewModel;
        }
    }
}
