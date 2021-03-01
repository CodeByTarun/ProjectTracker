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
    /// Interaction logic for ProjectIssueView.xaml
    /// </summary>
    public partial class ProjectIssueView : Page
    {
        public ProjectIssueView()
        {
            InitializeComponent();
        }

        private void EditBoard_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as ProjectIssueViewModel).EditBoardCommand.CanExecute(null))
            {
                (this.DataContext as ProjectIssueViewModel).EditBoardCommand.Execute(null);
            }
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as ProjectIssueViewModel).DeleteBoardCommand.CanExecute(null))
            {
                (this.DataContext as ProjectIssueViewModel).DeleteBoardCommand.Execute(null);
            }
        }

        private void IssueFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void IssueFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void UpdateFrameDataContext(object sender)
        {
            var content = IssueFrame.Content as FrameworkElement;
            if (content == null || this.DataContext == null)
                return;
            content.DataContext = (this.DataContext as ProjectIssueViewModel).KanbanControlViewModel;
        }
    }
}
