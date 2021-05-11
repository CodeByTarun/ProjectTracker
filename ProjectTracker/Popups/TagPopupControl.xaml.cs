using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
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

namespace ProjectTracker.Popups
{
    /// <summary>
    /// Interaction logic for TagPopupControl.xaml
    /// </summary>
    public partial class TagPopup : UserControl
    {

        private object _tag;

        public TagPopup()
        {
            InitializeComponent();
            DeleteTagPopup.Visibility = Visibility.Hidden;
        }

        private void CloseDeleteTagPopup_Click(object sender, RoutedEventArgs e)
        {
            DeleteTagPopup.Visibility = Visibility.Hidden;
            _tag = null;
        }

        private void ConfirmDeleteTagButton_Click(object sender, RoutedEventArgs e)
        {   
            DeleteTagPopup.Visibility = Visibility.Hidden;

            if (_tag != null)
            {
                if ((this.DataContext as TagPopupViewModel).DeleteTagCommand.CanExecute(_tag))
                {
                    (this.DataContext as TagPopupViewModel).DeleteTagCommand.Execute(_tag);
                    _tag = null;
                }
            }
        }

        private void DeleteTagButton_Click(object sender, RoutedEventArgs e)
        {
            _tag = ((sender as Button).Parent as Grid).DataContext;
            DeleteTagPopup.Visibility = Visibility.Visible;
        }
    }
}
