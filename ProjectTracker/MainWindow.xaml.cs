using ProjectTracker.Helpers;
using ProjectTracker.Model.Models;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomeView homeView;
        private ProjectListView projectListView;
        private ProjectView projectView;

        public MainWindow(MainViewModel ViewModel, HomeView homeView, ProjectListView projectListView, ProjectView projectView)
        {
            this.homeView = homeView;
            this.projectListView = projectListView;
            this.projectView = projectView;

            DataContext = ViewModel;
            
            InitializeComponent();

            TabControlHelper.SetTabListBox(TabsListBox);
            MainFrame.Navigate(homeView);
        }

        private void TabBorder_DragEnter(object sender, DragEventArgs e)
        {
            Project projectDragging = (Project)e.Data.GetData(typeof(Project));
            Project projectOver = (Project)((Border)sender).DataContext;

            if (projectDragging != projectOver && projectDragging != null)
            {
                ((MainViewModel)this.DataContext).MoveTab(projectDragging, projectOver);
            }
        }
        private void TabBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source.GetType().Name.Equals("Border"))
            {
                Border item = (Border)e.Source;

                if (item != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(this, item.DataContext, DragDropEffects.Move);
                }
            }

        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project project = (Project)((ListBox)sender).SelectedItem;

            if (project == null)
            {
                MainFrame.Navigate(homeView);
            } else if (project.Id == 0)
            {
                MainFrame.Navigate(projectListView);
            } else
            {
                MainFrame.Navigate(projectView);
            }
        }
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Project project = (Project)TabsListBox.SelectedItem;

            if (project != null && project.Id != 0)
            {
                projectView.ViewModel.CurrentProject = project;
            } 
        }
    }
}
