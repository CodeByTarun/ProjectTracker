using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
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

namespace ProjectTracker.Controls
{
    /// <summary>
    /// Interaction logic for BoardListControl.xaml
    /// </summary>
    public partial class BoardListControl : UserControl
    {
        public BoardListControl()
        {
            InitializeComponent();
        }

        private void OpenBoard_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BoardListViewModel).OpenBoardCommand.Execute((this.DataContext as BoardListViewModel).SelectedBoard);
        }

        private void EditBoard_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BoardListViewModel).EditBoardCommand.Execute((this.DataContext as BoardListViewModel).SelectedBoard);
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BoardListViewModel).DeleteBoardCommand.Execute((this.DataContext as BoardListViewModel).SelectedBoard);
        }

        private void BoardDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            BoardDataGrid.SelectedIndex = -1;
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

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BoardListViewModel).EditProjectCommnad.Execute(null);
        }

        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BoardListViewModel).DeleteProjectCommand.Execute(null);
        }
    }
}
