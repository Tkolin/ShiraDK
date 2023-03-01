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
    /// Логика взаимодействия для TicketPage.xaml
    /// </summary>
    public partial class TicketPage : Page
    {
        int role;
        public TicketPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = DBEntities.GetContext().BuyingTickets.ToList();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            var ClientForRemoving = dataGrid.SelectedItems.Cast<BuyingTickets>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {ClientForRemoving.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().BuyingTickets.RemoveRange(ClientForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                    dataGrid.ItemsSource = DBEntities.GetContext().BuyingTickets.ToList();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditTicketPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null)
                return;
            NavigationService.Navigate(new AddEditTicketPage((BuyingTickets)dataGrid.SelectedValue));
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {

            clientCBox.SelectedValue = null;
            startDatePicer.SelectedDate = null;
            endDatePicer.SelectedDate = null;
            sellerCBox.SelectedValue = null;

            dataGrid.ItemsSource = DBEntities.GetContext().BuyingTickets.ToList();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clientCBox.SelectedValue == null &&
                startDatePicer.SelectedDate == null &&
                endDatePicer.SelectedDate == null &&
                sellerCBox.SelectedValue == null)
                 return;

            List<BuyingTickets> tickets = DBEntities.GetContext().BuyingTickets.ToList();

            if (clientCBox.SelectedValue != null)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.BuyerID == (int)clientCBox.SelectedValue).ToList();
            if (startDatePicer.SelectedDate != null)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.PurchaseDate >= startDatePicer.SelectedDate).ToList();
            if (endDatePicer.SelectedDate != null)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.PurchaseDate <= endDatePicer.SelectedDate).ToList();
            if (sellerCBox.Text.Length > 0)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.SalesmanID == (int)sellerCBox.SelectedValue).ToList();

            dataGrid.ItemsSource = tickets.ToList();
        }
    }
}
