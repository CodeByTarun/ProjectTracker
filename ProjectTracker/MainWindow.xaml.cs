using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using ProjectTracker.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ProjectTracker
{

    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel, MainView mainView)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            MainFrame.Navigate(mainView);
        }
    }
}
