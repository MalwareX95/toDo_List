﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace toDo_List.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer.IsEnabled = true;
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void TimePicker_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
