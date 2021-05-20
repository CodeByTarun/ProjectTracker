using ProjectTracker.ClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        private bool _canNavigate;
        private HomeView homeView;

        private Collection<ProjectView> projectViewList;

        public MainView(TabViewModel tabViewModel, HomeView homeView)
        {
            this.homeView = homeView;
            this.DataContext = tabViewModel;

            projectViewList = new Collection<ProjectView>();
            
            InitializeComponent();

            NavigateToPage(homeView);
        }

        private void NavigateToPage(Page page)
        {
            _canNavigate = true;
            MainFrame.Navigate(page);
            _canNavigate = false;
        }

        public void WindowStateEventSubscriber()
        {
            Window window = Application.Current.MainWindow;
            window.StateChanged += Window_StateChanged;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (((Window)sender).WindowState == WindowState.Maximized)
            {
                btnRestore.Content = "\xE923";
            } else
            {
                btnRestore.Content = "\xE739";
            }
        }

        // Drag and Drop
        private void TabBorder_DragEnter(object sender, DragEventArgs e)
        {
            ProjectViewModel tabDragging = (ProjectViewModel)e.Data.GetData(typeof(ProjectViewModel));
            ProjectViewModel tabOver = (ProjectViewModel)((Border)sender).DataContext;

            if (tabDragging != tabOver && tabDragging != null)
            {
                ((TabViewModel)this.DataContext).MoveTab(tabDragging, tabOver);
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

        // Navigation
        public void AddProjectView(ProjectViewModel viewModel)
        {
            ProjectView projectView = new ProjectView(new ProjectOverviewView(), new ProjectIssueView());

            projectView.SetDataContext(viewModel);

            projectViewList.Add(projectView);
        }

        public void MatchProjectViewListToViewModelList()
        {
            foreach (ProjectView projectView in projectViewList.ToList())
            {
                if (projectView.ProjectViewModel.CurrentProject == null)
                {
                    projectViewList.Remove(projectView);
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MatchProjectViewListToViewModelList();

            ProjectViewModel tab = (ProjectViewModel)((ListBox)sender).SelectedItem;

            var g = projectViewList.FirstOrDefault(v => v.ProjectViewModel == tab);

            if (tab == null)
            {
                NavigateToPage(homeView);
            }
            else
            {
                if (projectViewList.FirstOrDefault(v => v.ProjectViewModel == tab) == null)
                {
                    AddProjectView(tab);
                } 

                ProjectView projectView = projectViewList.FirstOrDefault(v => v.ProjectViewModel == tab);
                NavigateToPage(projectView);
            }
        }
        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (_canNavigate == false)
            {
                e.Cancel = true;
            }
        }

        // Window Functions
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.Close();
        }
        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow.WindowState == System.Windows.WindowState.Normal)
            {
                mainWindow.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                mainWindow.WindowState = System.Windows.WindowState.Normal;
            }

        }
        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            mainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        
    }
}
