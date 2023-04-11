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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        List<Users> users;
        public LoginPage()
        {
            InitializeComponent();
        
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (loginSearch())
            {
                case -1:
                    MessageBox.Show("Ошибка входа");
                    break;
                case 2:
                    NavigationService.Navigate(new EventPage(2));
                    break;
                case 3:
                    NavigationService.Navigate(new TicketPage(3));
                    break;
                default:
                    NavigationService.Navigate(new MenyPage(loginSearch()));
                    break;
            }

                
        }
        private void nextGostBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventPage(2));
        }
        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrPage());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private int loginSearch()
        {
            foreach (Users checkUser in users)
            {
                if (checkUser.Login.ToLower() == loginBox.Text.ToLower() &&
                    checkUser.Password == passwordBox.Password)
                {
                    return (int)checkUser.RoleID;
                }

            }
            return -1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            users = DBEntities.GetContext().Users.ToList();
        }


    }
}
