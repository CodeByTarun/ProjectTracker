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
    /// Interaction logic for LimitedInputUserControl.xaml
    /// </summary>
    public partial class LimitedInputUserControl : UserControl
    {
        #region DP Label

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty);  }
            set { SetValue(InputTextProperty, value); }
        }

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(LimitedInputUserControl), new PropertyMetadata(""));

        #endregion

        public LimitedInputUserControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public string Title { get; set; }
        public int MaxLength { get; set; }
    }
}
