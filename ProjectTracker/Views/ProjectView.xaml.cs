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
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private bool _canNavigate;
        private ProjectOverviewView _projectOverviewView;
        private ProjectIssueView _projectIssueView;

        public ProjectViewModel ProjectViewModel { get; set; }

        public ProjectView(ProjectOverviewView projectOverviewView, ProjectIssueView projectIssueView)
        {
            this._projectOverviewView = projectOverviewView;
            this._projectIssueView = projectIssueView;

            InitializeComponent();
        }

        public void SetDataContext(ProjectViewModel vm)
        {
            ProjectViewModel = vm;
            DataContext = vm;

            _projectOverviewView.DataContext = ((ProjectViewModel)this.DataContext).ProjectOverviewViewModel;
            _projectOverviewView.BoardListControlFrame.DataContext = ((ProjectViewModel)this.DataContext).ProjectOverviewViewModel.BoardListViewModel;
            _projectIssueView.DataContext = ((ProjectViewModel)this.DataContext).ProjectIssueViewModel;
            _projectIssueView.IssueFrame.DataContext = ((ProjectViewModel)this.DataContext).ProjectIssueViewModel.KanbanControlViewModel;
        }

        private void BoardListKanbanBoardToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                NavigateToPage(_projectOverviewView);
            }
        }

        private void BoardListKanbanBoardToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                NavigateToPage(_projectIssueView);
            }
        }

        private void NavigateToPage(Page page)
        {
            _canNavigate = true;
            ProjectViewFrame.Navigate(page);
            _canNavigate = false;
        }

        private void ProjectViewFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (_canNavigate == false)
            {
                e.Cancel = true;
            }
        }
    }
}
