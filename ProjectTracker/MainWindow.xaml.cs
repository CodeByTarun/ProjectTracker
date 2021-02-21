using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectTracker
{

    public partial class MainWindow : Window
    {
        private HomeView homeView;
        private ProjectView projectView;

        public MainWindow(TabViewModel ViewModel, HomeView homeView, ProjectView projectView)
        {
            this.homeView = homeView;
            this.projectView = projectView;

            DataContext = ViewModel;
            
            InitializeComponent();

            MainFrame.Navigate(homeView);
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
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectViewModel tab = (ProjectViewModel)((ListBox)sender).SelectedItem;

            if (tab == null)
            {
                MainFrame.Navigate(homeView);
            } else
            {
                MainFrame.Navigate(projectView);
            }
        }
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            int tabIndex = TabsListBox.SelectedIndex;

            if (tabIndex != -1)
            {
                projectView.SetDataContext(((TabViewModel)this.DataContext).Tabs[tabIndex]);
            } 
        }
    }
}
