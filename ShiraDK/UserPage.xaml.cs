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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        int role;
        public UserPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = DBEntities.GetContext().Users.ToList();
            DataContext = this;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            var ClientForRemoving = dataGrid.SelectedItems.Cast<Users>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {ClientForRemoving.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().Users.RemoveRange(ClientForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                    dataGrid.ItemsSource = DBEntities.GetContext().Users.ToList();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditUserPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null)
                return;
            NavigationService.Navigate(new AddEditUserPage((Users)dataGrid.SelectedValue));
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameBox.Text == null &&
                lastNameBox.Text == null)
                return;

            List<Users> events = DBEntities.GetContext().Users.ToList();

            if (firstNameBox.Text != null)
                events = (List<Users>)events.Where(eve => eve.FirstName.ToLower().IndexOf(firstNameBox.Text.ToLower()) != -1).ToList();
            if (lastNameBox.Text != null)
                events = (List<Users>)events.Where(eve => eve.LastName.ToLower().IndexOf(lastNameBox.Text.ToLower()) != -1).ToList();

            dataGrid.ItemsSource = events.ToList();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            firstNameBox.Text = null;
            lastNameBox.Text = null;


            dataGrid.ItemsSource = DBEntities.GetContext().Users.ToList();
        }
    }
}
