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
    /// Логика взаимодействия для RegistrPage.xaml
    /// </summary>
    public partial class RegistrPage : Page
    {

        public RegistrPage()
        {
            InitializeComponent();
        }

        private void registrBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!DublicateLogin()&& !NullValue())
            {
                SaveUser();
            }
            else
                MessageBox.Show("Проверте введёные данные");
        }
        public bool DublicateLogin()
        {
            List<Users> userLoginPass = DBEntities.GetContext().Users.ToList();

            foreach (Users user in userLoginPass)
                if(user.Login == loginTBox.Text)
                    return true;

            return false;
        }
        public bool NullValue()
        {
            if (loginTBox.Text.Length < 5 || passwordTBox.Text.Length < 8)
                return true;

            return false;
        }

        public void SaveUser()
        {
            Users user = new Users();
            user.Login = loginTBox.Text;
            user.Password = passwordTBox.Text;
            user.LastName = lastNameTBox.Text;
            user.RoleID = 2;
            user.FirstName = firstNameTBox.Text;

            DBEntities.GetContext().Users.Add(user);

            try
            {
                DBEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
