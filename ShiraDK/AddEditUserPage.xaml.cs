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
    /// Логика взаимодействия для AddEditUserPage.xaml
    /// </summary>
    public partial class AddEditUserPage : Page
    {
        Users _user = new Users();
        bool add = true;
        public AddEditUserPage(Users _user)
        {

            InitializeComponent();
            if (_user != null)
            {
                this._user = _user;
                add = false;
            }
            DataContext = _user;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(roleCbox.SelectedItem == null ){

                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            try
            {
                _user.FirstName = firstNameTBox.Text;
                _user.LastName = lastNameTBox.Text;
                _user.RoleID = ((Roles)roleCbox.SelectedItem).ID;
                _user.Login = loginTBox.Text;
                _user.Password = passwordTBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте введёные данные!");
                return;
            }

            if (add)
                DBEntities.GetContext().Users.Add(_user);

            try
            {
                DBEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            roleCbox.ItemsSource = DBEntities.GetContext().Roles.ToList();

            if (!add)
            {
                idTBox.Text = _user.ID.ToString();
                firstNameTBox.Text = _user.FirstName;
                lastNameTBox.Text = _user.LastName;
                roleCbox.SelectedItem = _user.Roles;
                loginTBox.Text = _user.Login;
                passwordTBox.Text = _user.Password;

            }
        }
    }
}
