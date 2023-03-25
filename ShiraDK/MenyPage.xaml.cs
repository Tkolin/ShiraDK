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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiraDK
{
    /// <summary>
    /// Логика взаимодействия для MenyPage.xaml
    /// </summary>
    public partial class MenyPage : Page
    {
        int role;
        public MenyPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void versteckBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new VersteckPage(role));
        }

        private void ticketBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TicketPage(role));
        }

        private void userBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserPage(role));
        }

        private void enevtBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EventPage(role));
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            switch (role)
            {
                case 1:
                    versteckBtn.Visibility = Visibility.Visible;
                    ticketBtn.Visibility = Visibility.Visible;
                    userBtn.Visibility = Visibility.Visible;
                    enevtBtn.Visibility = Visibility.Visible;
                    break;
                case 2:
                    enevtBtn.Visibility = Visibility.Visible;
                    break;
                case 3:
                    ticketBtn.Visibility = Visibility.Visible;
                    enevtBtn.Visibility = Visibility.Visible;
                    break;
                case 4:
                    versteckBtn.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
