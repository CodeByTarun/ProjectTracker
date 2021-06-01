using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.Helpers;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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


        #region Drag and Drop
        private void GroupBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType().Name.Equals("Border"))
            {
                Border item = (Border)e.Source;
                Group group = item.DataContext as Group;
                (this.DataContext as KanbanControlViewModel).GroupDraggingID = group.Id;

                if (item != null)
                {
                    DragDrop.DoDragDrop(this, item.DataContext, DragDropEffects.Move);
                    (this.DataContext as KanbanControlViewModel).MoveGroupInDatabase(group);
                    (this.DataContext as KanbanControlViewModel).GroupDraggingID = null;
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

        private void IssueBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType().Name.Equals("Border"))
            {
                Border item = (Border)e.Source;
                Issue issue = item.DataContext as Issue;
                (this.DataContext as KanbanControlViewModel).IssueDraggingID = issue.Id;

                if (item != null)
                {
                    DragDrop.DoDragDrop(this, item.DataContext, DragDropEffects.Move);
                    (this.DataContext as KanbanControlViewModel).MoveIssueInDatabase(issue);
                    (this.DataContext as KanbanControlViewModel).IssueDraggingID = null;
                }
            }
        }
        private void IssueBorder_DragEnter(object sender, DragEventArgs e)
        {
            Issue issueDragging = (Issue)e.Data.GetData(typeof(Issue));

            Issue issueOver = (Issue)((Border)sender).DataContext;

            if (issueDragging != issueOver && issueDragging != null)
            {
                (this.DataContext as KanbanControlViewModel).MoveIssues(issueDragging, issueOver);
                Thread.Sleep(100);
            }
        }
        private void GroupEmptySpaceGrid_DragEnter(object sender, DragEventArgs e)
        {
            Issue issueDragging = (Issue)e.Data.GetData(typeof(Issue));

            Group groupOver = (Group)((Border)(((Grid)((Grid)sender).Parent).Parent)).DataContext;

            if (issueDragging != null)
            {
                (this.DataContext as KanbanControlViewModel).MoveIssueToEnd(issueDragging, groupOver);
            }
        }
        #endregion

        #region Autoscroll on Drag
        private void GroupItemsControl_DragOver(object sender, DragEventArgs e)
        {
            if ((Group)e.Data.GetData(typeof(Group)) != null)
            {
                ItemsControl itemsControl = sender as ItemsControl;

                double tolerance = 40;
                double horizontalPos = e.GetPosition(KanbanGroupScrollViewer).X;
                double offset = 50;

                if (horizontalPos < tolerance)
                {
                    KanbanGroupScrollViewer.ScrollToVerticalOffset(KanbanGroupScrollViewer.HorizontalOffset - offset);
                }
                else if (horizontalPos > KanbanGroupScrollViewer.ActualWidth - tolerance)
                {
                    KanbanGroupScrollViewer.ScrollToVerticalOffset(KanbanGroupScrollViewer.HorizontalOffset + offset);
                }
            }
        }

        private void IssuesItemsControl_DragOver(object sender, DragEventArgs e)
        {
            if ((Issue)e.Data.GetData(typeof(Issue)) != null)
            {
                ItemsControl itemsControl = sender as ItemsControl;
                ScrollViewer sv = itemsControl.Parent as ScrollViewer;
                double tolerance = 40;
                double verticalPos = e.GetPosition(sv).Y;
                double offset = 50;

                if (verticalPos < tolerance)
                {
                    sv.ScrollToVerticalOffset(sv.VerticalOffset - offset);
                }
                else if (verticalPos > sv.ActualHeight - tolerance)
                {
                    sv.ScrollToVerticalOffset(sv.VerticalOffset + offset);
                }
            }
        }
        #endregion

        #region Context Menu Actions
        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            object group = ((sender as FrameworkElement).TemplatedParent as FrameworkElement).DataContext;
            (this.DataContext as KanbanControlViewModel).EditGroupCommand.Execute(group);
        }
        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            object group = ((sender as FrameworkElement).TemplatedParent as FrameworkElement).DataContext;
            (this.DataContext as KanbanControlViewModel).DeleteGroupCommand.Execute(group);
        }

        private void EditIssue_Click(object sender, RoutedEventArgs e)
        {
            object issue = ((sender as FrameworkElement).TemplatedParent as FrameworkElement).DataContext;
            (this.DataContext as KanbanControlViewModel).EditIssueCommand.Execute(issue);
        }
        private void DeleteIssue_Click(object sender, RoutedEventArgs e)
        {
            object issue = ((sender as FrameworkElement).TemplatedParent as FrameworkElement).DataContext;
            (this.DataContext as KanbanControlViewModel).DeleteIssueCommand.Execute(issue);
        }
        #endregion
    }
}
