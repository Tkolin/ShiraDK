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
    /// Логика взаимодействия для AddEditTicketPage.xaml
    /// </summary>
    public partial class AddEditTicketPage : Page
    {
        BuyingTickets _ticet = new BuyingTickets();
        bool add = true;
        public AddEditTicketPage(BuyingTickets _ticet)
        {
 
            InitializeComponent();
            if (_ticet != null)
            {
                this._ticet = _ticet;
                add = false;
            }
            DataContext = _ticet;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dateSellDatePicer.SelectedDate == null || sellerCBox.SelectedItem == null || 
                eventCBox.SelectedItem == null)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            try
                { 
                _ticet.PurchaseDate =  (DateTime)dateSellDatePicer.SelectedDate;
                _ticet.SalesmanID = ((Users)sellerCBox.SelectedItem).ID;
                _ticet.BuyerID = ((Users)buyerCBox.SelectedItem).ID;
                _ticet.EventID = ((Events)eventCBox.SelectedItem).ID;
        }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте введёные данные!");
                return;
            }

            if (add)
                DBEntities.GetContext().BuyingTickets.Add(_ticet);

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
            buyerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            eventCBox.ItemsSource = DBEntities.GetContext().Events.ToList();
            sellerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();

            if (!add)
            {
                IdTBox.Text = _ticet.Number.ToString();
                dateSellDatePicer.SelectedDate = _ticet.PurchaseDate;

                buyerCBox.SelectedItem = _ticet.Users;
                sellerCBox.SelectedItem = _ticet.Users1;
                eventCBox.SelectedItem = _ticet.Events;
            }
            else
            {
                dateSellDatePicer.SelectedDate = DateTime.Now;
            }



        }
    }
}
