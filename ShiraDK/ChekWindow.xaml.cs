
using Microsoft.Win32;
using System;
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
using System.Windows.Shapes;
 

 

namespace ShiraDK
{
    /// <summary>
    /// Логика взаимодействия для ChekWindow.xaml
    /// </summary>
    public partial class ChekWindow : Window
    {
        
        public ChekWindow(  string n0,
                            string n1,
                            string n2,
                            string n3,
                            string n4,
                            string n5,
                            string n6,
                            string n7,
                            string n8,
                            string n9)
        {
            InitializeComponent();
            tB0.Text = n0;
            tB1.Text = n1;
            tB2.Text = n2;
            tB3.Text = n3;
            tB4.Text = n4;
            tB5.Text = n5;
            tB6.Text = n6;
            tB7.Text = n7;
            tB8.Text = n8;
            tB9.Text = n9;

        }
        public static string filePath = "";
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {

            PrintDialog printDialog = new PrintDialog();
            NonPastPanel.Visibility = Visibility.Collapsed;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(chekGrid, "My First Print Job");
            }
            NonPastPanel.Visibility = Visibility.Visible;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
