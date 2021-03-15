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
        private ProjectOverviewView _projectOverviewView;
        private ProjectIssueView _projectIssueView;
        private ProjectNotesView _projectNotesView;

        public ProjectView(ProjectOverviewView projectOverviewView, ProjectIssueView projectIssueView, ProjectNotesView projectNotesView)
        {
            this._projectOverviewView = projectOverviewView;
            this._projectIssueView = projectIssueView;
            this._projectNotesView = projectNotesView;

            InitializeComponent();
        }

        public void SetDataContext(ProjectViewModel vm)
        {
            DataContext = null;
            DataContext = vm;
        }

        public class ProjectItems
        {
            public string Title { get; set; }
        }

        private void ProjectItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectItemsListView.SelectedIndex == 0)
            {
                ProjectViewFrame.Navigate(_projectOverviewView);
                _projectOverviewView.DataContext = ((ProjectViewModel)this.DataContext).ProjectOverviewViewModel;
                _projectOverviewView.BoardListControlFrame.DataContext = ((ProjectViewModel)this.DataContext).ProjectOverviewViewModel.BoardListViewModel;
            }
            else if (ProjectItemsListView.SelectedIndex == 1)
            {
                ProjectViewFrame.Navigate(_projectIssueView);
                _projectIssueView.DataContext = ((ProjectViewModel)this.DataContext).ProjectIssueViewModel;
                _projectIssueView.IssueFrame.DataContext = ((ProjectViewModel)this.DataContext).ProjectIssueViewModel.KanbanControlViewModel;
            }
            else if (ProjectItemsListView.SelectedIndex == 2)
            {
                ProjectViewFrame.Navigate(_projectNotesView);
                _projectNotesView.DataContext = ((ProjectViewModel)this.DataContext).ProjectNotesViewModel;
            }
        }
    }
}
