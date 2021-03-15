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
    /// Interaction logic for TagListControl.xaml
    /// </summary>
    public partial class TagListControl : UserControl
    {
        public IEnumerable<Tag> Tags { get; set; }

        public TagListControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }
    }
}
