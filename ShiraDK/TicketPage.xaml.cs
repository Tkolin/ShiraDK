using Microsoft.Office.Interop.Excel;
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
    public partial class TicketPage : System.Windows.Controls.Page
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
            clientCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            sellerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
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
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.Users == (Users)clientCBox.SelectedValue).ToList();
            if (startDatePicer.SelectedDate != null)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.PurchaseDate >= startDatePicer.SelectedDate).ToList();
            if (endDatePicer.SelectedDate != null)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.PurchaseDate <= endDatePicer.SelectedDate).ToList();
            if (sellerCBox.Text.Length > 0)
                tickets = (List<BuyingTickets>)tickets.Where(eve => eve.Users1 == (Users)sellerCBox.SelectedValue).ToList();

            dataGrid.ItemsSource = tickets.ToList();
        }

        private void otchetBtn_Click(object sender, RoutedEventArgs e)
        {
            getOtchetTask(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
        }
        public void getOtchetTask(DateTime start, DateTime end)
        {
            //подключение таблиц
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;

            //создание страницы
            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];

            //18
            //форматирование текста
            ws.StandardWidth = 18;

            ws.Range["A1:F1"].Merge();
            ws.Range["A1"].Value = "Отчёт по продажам"; ws.Range["A1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            ws.Range["B2"].Value = "Дата начала:";
            ws.Range["C2"].Value = start.Year + "." + start.Month + "." + start.Day;
            ws.Range["D2"].Value = "Дата окончания:";
            ws.Range["E2"].Value = end.Year + "." + end.Month + "." + end.Day;

            ws.Range["A4"].Value = "#"; ws.Range["A6"].ColumnWidth = 6;
            ws.Range["B4"].Value = "Название мироприятия"; ws.Range["B4"].ColumnWidth = 22;
            ws.Range["C4"].Value = "Дата продажи";
            ws.Range["D4"].Value = "Цена продажи";
            ws.Range["E4"].Value = "Кол-во"; ws.Range["e4"].ColumnWidth = 9;
            ws.Range["F4"].Value = "Сумма"; ws.Range["F4"].ColumnWidth = 10;

            List<BuyingTickets> bt = DBEntities.GetContext().BuyingTickets
                .Where(t=>t.PurchaseDate >= start).ToList();

            int startPoint = 5;
            int point = startPoint;
            foreach (BuyingTickets t in bt)
            {

                ws.Range["A"+ point].Value = point - 4;
                ws.Range["B" + point].Value = t.Events.Name;
                ws.Range["C" + point].Value = t.PurchaseDate;
                ws.Range["D" + point].Value = t.Events.Price;
                ws.Range["E" + point].Value = t.Count;
                ws.Range["F" + point].Formula = "=D"+ point + "*E"+ point;
                point++;
            }


            ws.Range["B"+point].Value = "Итого:";
            ws.Range["E"+point].Formula = "=СУММ(E"+startPoint+":E"+(point-1)+")";
            ws.Range["F"+point].Formula = "=СУММ(F"+startPoint+":F"+(point-1)+")";

            app.Calculation = XlCalculation.xlCalculationAutomatic;
            ws.Calculate();
        }

        private void chekBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            BuyingTickets bTick = dataGrid.SelectedItem as BuyingTickets;
            try
            {
                string n0 = bTick.Events.Name;
                string n1 = bTick.Events.Organizers.Name;
                string n2 = bTick.Events.Price.ToString();
                string n3 = bTick.Events.DateStart.ToString();
                string n4 = bTick.Users1.LastName + " " + bTick.Users1.FirstName;
                string n5 = bTick.Number.ToString();
                string n6 = bTick.PurchaseDate.ToString();
                string n7 = bTick.Users.LastName + " " + bTick.Users.FirstName;
                string n8 = bTick.Count.ToString();
                string n9 = (bTick.Count * bTick.Events.Price).ToString();
                ChekWindow window
                    = new ChekWindow(   n0,
                                        n1,
                                        n2,
                                        n3,
                                        n4,
                                        n5,
                                        n6,
                                        n7,
                                        n8,
                                        n9);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка!");
                MessageBox.Show("Оформите чек в ручную.","Сообщение");
                return;
            }
        }
    }
}
