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
    /// Логика взаимодействия для VersteckPage.xaml
    /// </summary>
    public partial class VersteckPage : Page
    {
        int role;
        public VersteckPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = DBEntities.GetContext().WareHouse.ToList();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            WareHouse ClientForRemoving = dataGrid.SelectedItem as WareHouse;

            if (MessageBox.Show($"Не рекомендуем удолять старые записи, это может повлечь ошибку с данными. \nВы уверены?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Items items = ClientForRemoving.Items;
                        items.Count -= ClientForRemoving.Quantity;

                    DBEntities.GetContext().WareHouse.Remove(ClientForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                    dataGrid.ItemsSource = DBEntities.GetContext().WareHouse.ToList();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditVersteckPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null)
                return;
            NavigationService.Navigate(new AddEditVersteckPage((WareHouse)dataGrid.SelectedValue));
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (startDatePicer.SelectedDate == null &&
                endDatePicer.SelectedDate == null)
                return;

            List<WareHouse> wareHouses = DBEntities.GetContext().WareHouse.ToList();
         
            if (startDatePicer.SelectedDate != null)
                wareHouses = (List<WareHouse>)wareHouses.Where(eve => eve.DateOfReceipt >= startDatePicer.SelectedDate).ToList();
            if (endDatePicer.SelectedDate != null)
                wareHouses = (List<WareHouse>)wareHouses.Where(eve => eve.DateOfReceipt <= endDatePicer.SelectedDate).ToList();
    
            dataGrid.ItemsSource = wareHouses.ToList();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {

            startDatePicer.SelectedDate = null;
            endDatePicer.SelectedDate = null;
   

            dataGrid.ItemsSource = DBEntities.GetContext().WareHouse.ToList();
        }
    }
}
