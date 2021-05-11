using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;

namespace ProjectTracker.Controls
{
    /// <summary>
    /// Interaction logic for ProjectListControl.xaml
    /// </summary>
    public partial class ProjectListControl : UserControl
    {
        public ProjectListControl()
        {
            InitializeComponent();
        }

        // DataGrid
        private void ProjectDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectDataGrid.SelectedIndex = -1;
        }
        private void MenuGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                Border button = (Border)sender;
                button.ContextMenu.IsOpen = true;
                button.ContextMenu.StaysOpen = true;
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ProjectListViewModel).OpenProject((this.DataContext as ProjectListViewModel).SelectedProject);
        }
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ProjectListViewModel).EditProject((this.DataContext as ProjectListViewModel).SelectedProject);
        }
        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ProjectListViewModel).ShowDeleteDialog((this.DataContext as ProjectListViewModel).SelectedProject);                  
        }
    }
}
