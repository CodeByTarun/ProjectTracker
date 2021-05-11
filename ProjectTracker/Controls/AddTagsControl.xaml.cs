using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddTagsControl.xaml
    /// </summary>
    public partial class AddTagsControl : UserControl
    {
        #region DP Labels
        public IEnumerable<Tag> TagList
        {
            get { return (IEnumerable<Tag>)GetValue(TagListProperty); }
            set { SetValue(TagListProperty, value); }
        }
        public ICollection<Tag> ItemTags
        {
            get { return (ICollection<Tag>)GetValue(ItemTagsProperty); }
            set { SetCurrentValue(ItemTagsProperty, value); }
        }
        public Tag SelectedTag
        {
            get { return (Tag)GetValue(SelectedTagProperty); }
            set { SetValue(SelectedTagProperty, value); }
        }
        public string TagSearchText
        {
            get { return (string)GetValue(TagSearchTextProperty); }
            set { SetValue(TagSearchTextProperty, value); }
        }
        public ICommand AddTagCommand
        {
            get { return (ICommand)GetValue(AddTagCommandProperty); }
            set { SetValue(AddTagCommandProperty, value); }
        }
        public ICommand RemoveTagCommand
        {
            get { return (ICommand)GetValue(RemoveTagCommandProperty); }
            set { SetValue(RemoveTagCommandProperty, value); }
        }

        public static readonly DependencyProperty TagListProperty =
            DependencyProperty.Register("TagList", typeof(IEnumerable<Tag>), typeof(AddTagsControl));

        public static readonly DependencyProperty ItemTagsProperty =
            DependencyProperty.Register("ItemTags", typeof(ICollection<Tag>), typeof(AddTagsControl));

        public static readonly DependencyProperty SelectedTagProperty =
            DependencyProperty.Register("SelectedTag", typeof(Tag), typeof(AddTagsControl));

        public static readonly DependencyProperty TagSearchTextProperty =
            DependencyProperty.Register("TagSearchText", typeof(string), typeof(AddTagsControl), new PropertyMetadata(""));

        public static readonly DependencyProperty AddTagCommandProperty =
            DependencyProperty.Register("AddTagCommand", typeof(ICommand), typeof(AddTagsControl));

        public static readonly DependencyProperty RemoveTagCommandProperty =
            DependencyProperty.Register("RemoveTagCommand", typeof(ICommand), typeof(AddTagsControl));
        #endregion

        public AddTagsControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void TagListComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.IsDropDownOpen = true;
        }

        private void TagListComboBox_DropDownClosed(object sender, EventArgs e)
        {
            AddTagCommand.Execute(TagListComboBox.SelectedItem);
            TagListComboBox.SelectedIndex = -1;

            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(TagListComboBox), null);
            Keyboard.ClearFocus();
        }

        private void RemoveTagButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveTagCommand.Execute((sender as FrameworkElement).DataContext);
        }
    }
}
