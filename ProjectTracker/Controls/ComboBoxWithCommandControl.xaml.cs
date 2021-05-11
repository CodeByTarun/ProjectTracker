using ProjectTracker.Model.Models;
using System.ComponentModel;
using System;
using System.Collections.Generic;
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
using System.Runtime.CompilerServices;

namespace ProjectTracker.Controls
{
    public partial class ComboBoxWithCommandControl : UserControl
    {
        #region DP Labels
        public IEnumerable<object> Items
        {
            get { return (IEnumerable<Object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public object SelectedItem
        {
            get { return (Object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public ICommand DropDownCommand
        {
            get { return (ICommand)GetValue(DropDownCommandProperty); }
            set { SetValue(DropDownCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
                DependencyProperty.Register("Items", typeof(IEnumerable<object>), typeof(ComboBoxWithCommandControl));

        public static readonly DependencyProperty SelectedItemProperty =
                DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBoxWithCommandControl));

        public static readonly DependencyProperty DropDownCommandProperty =
                DependencyProperty.Register("DropDownCommand", typeof(ICommand), typeof(ComboBoxWithCommandControl));

        
        #endregion

        public string PlaceholderText { get; set; }
        public bool IsPathNameRequired { get; set; }


        public ComboBoxWithCommandControl()
        {
            InitializeComponent();
        }

        private void ItemComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (DropDownCommand != null)
            {
                if (ComboBoxWithCommand.SelectedItem != null)
                {
                    DropDownCommand.Execute(ComboBoxWithCommand.SelectedItem);
                }
                else
                {
                    DropDownCommand.Execute(null);
                }
            }
            
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(ComboBoxWithCommand), null);
            Keyboard.ClearFocus();
        }

        private void ComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.IsDropDownOpen = true;
        }

        private void RestFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxWithCommand.SelectedIndex = -1;

            if (DropDownCommand != null)
            {
                DropDownCommand.Execute(null);
            }
        }
    }
}
