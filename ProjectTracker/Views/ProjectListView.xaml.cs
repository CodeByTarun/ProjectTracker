using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.Helpers;
using ProjectTracker.Model.Models;
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
    /// Interaction logic for ProjectListView.xaml
    /// </summary>
    public partial class ProjectListView : Page
    {
        ProjectListViewModel ViewModel;
        public ProjectListView(ProjectListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void ProjectDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectDataGrid.SelectedIndex = -1;
        }
        private void MenuGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                Grid button = (Grid)sender;
                button.ContextMenu.IsOpen = true;
                button.ContextMenu.StaysOpen = true;
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            TabControlHelper.AddTab(ViewModel.SelectedProject);
        }
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.EditProject(ViewModel.SelectedProject);
        }
        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveProject(ViewModel.SelectedProject);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Project project = (Project)((DataGridRow)sender).DataContext;
            TabControlHelper.AddTab(project);
        }
    }
}
