using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
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

namespace ProjectTracker.Controls
{
    /// <summary>
    /// Interaction logic for KanbanControl.xaml
    /// </summary>
    public partial class KanbanControl : UserControl
    {
        public KanbanControl()
        {
            InitializeComponent();
        }

        private void GroupBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType().Name.Equals("Border"))
            {
                Border item = (Border)e.Source;

                if (item != null)
                {
                    DragDrop.DoDragDrop(this, item.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void GroupBorder_DragEnter(object sender, DragEventArgs e)
        {
            Group groupDragging = (Group)e.Data.GetData(typeof(Group));

            Group groupOver = (Group)((Border)sender).DataContext;

            if (groupDragging != groupOver && groupDragging != null)
            {
                (this.DataContext as KanbanControlViewModel).MoveGroups(groupDragging, groupOver);
            }
        }

        private void GroupBorder_DragOver(object sender, DragEventArgs e)
        {

        }

        private void GroupBorder_Drop(object sender, DragEventArgs e)
        {

        }

        private void GroupEmptySpaceGrid_Drop(object sender, DragEventArgs e)
        {

        }

        private void GroupEmptySpaceGrid_DragEnter(object sender, DragEventArgs e)
        {

        }
    }
}
