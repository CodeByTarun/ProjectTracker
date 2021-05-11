﻿using System;
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
    /// Interaction logic for BoardPopup.xaml
    /// </summary>
    public partial class BoardPopup : UserControl
    {
        public BoardPopup()
        {
            InitializeComponent();
            PopupDatePicker.DisplayDateStart = DateTime.Today;
        }
    }
}
