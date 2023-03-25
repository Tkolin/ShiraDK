using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ItemForEventPage.xaml
    /// </summary>
    public partial class ItemForEventPage : Page
    {
        Events events;
        public ItemForEventPage(Events events)
        {
            InitializeComponent();
            this.events = events;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void addItemForEvent_Click(object sender, RoutedEventArgs g)
        {
            // Проверяем, что в таблице выбрана строка
            if (dataGrid.SelectedItem == null)
                return;

            // Получаем выбранный элемент (объект класса Items)
            Items item = dataGrid.SelectedItem as Items;

            // Получаем значение количества из текстового поля и преобразуем в int
            int countTBox = Convert.ToInt32(countItems.Text);

            // Получаем список всех ItemsForEvents, связанных с данной датой начала мероприятия
            List<ItemsForEvents> itemsForEvent = DBEntities.GetContext().ItemsForEvents
                .Where(e => e.Events.DateStart.Date == events.DateStart.Date).ToList();

            // Вычисляем общее количество доступного инвентаря данного типа
            int count = 0;
            foreach (ItemsForEvents itemForEvent in itemsForEvent.Where(i => i.Items == item))
            {
                count += (int)itemForEvent.Quantity;
            }
            count = (int)item.Count - count;

            // Проверяем, достаточно ли инвентаря для добавления в мероприятие
            if (count >= countTBox)
            {
                try
                {
                    // Создаем новый объект ItemsForEvents и заполняем его поля
                    ItemsForEvents ife = new ItemsForEvents();
                    ife.Items = item;
                    ife.Quantity = countTBox;
                    ife.Events = events;

                    // Добавляем объект в базу данных
                    DBEntities.GetContext().ItemsForEvents.Add(ife);
                    // Сохраняем изменения в базе данных
                    DBEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Недостаточно инвентаря!");
            }
            UpdateGrid();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            // Получаем список Items, у которых Count больше нуля
            List<Items> items = DBEntities.GetContext().Items
                .Where(x => x.Count > 0)
                .ToList();

            // Получаем список ItemsForEvents из базы данных
            List<ItemsForEvents> itemsForEvent = DBEntities.GetContext().ItemsForEvents.ToList();

            // Отображаем только те ItemsForEvents, которые связаны с событием events.ID
            dataGridEvents.ItemsSource = itemsForEvent.Where(e => e.EventsID == events.ID).ToList();

            // Фильтруем список itemsForEvent по событиям, начало которых совпадает с началом события events
            itemsForEvent = itemsForEvent.Where(e => e.Events.DateStart.Date == events.DateStart.Date).ToList();

            // Создаем новый список Items, который будет использоваться для отображения в пользовательском интерфейсе
            List<Items> viewItems = new List<Items>();

            // Для каждого элемента из списка items выполняем следующее:
            foreach (Items item in items)
            {
                // Получаем суммарное количество элементов, которые используются в событиях из списка itemsForEvent
                int count = 0;
                foreach (ItemsForEvents itemForEvent in itemsForEvent.Where(i => i.Items == item))
                {
                    count += (int)itemForEvent.Quantity;
                }

                // Вычисляем доступное количество элементов для каждого элемента из списка items
                count = (int)item.Count - count;

                // Если количество доступных элементов больше нуля, добавляем элемент в список viewItems
                if (count > 0)
                    viewItems.Add(item);
            }
            dataGrid.ItemsSource = viewItems;
        }

        private void delItemForEvent_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvents.SelectedItem == null)
                return;

            var ItemForEvents = dataGridEvents.SelectedItems.Cast<ItemsForEvents>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {ItemForEvents.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().ItemsForEvents.RemoveRange(ItemForEvents);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdateGrid();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGridEvents.SelectedItem = null;
            if (dataGrid.SelectedItem == null)
                return;
            Items _item = ((Items)dataGrid.SelectedItem);

            itemIdTBox.Text = _item.ID.ToString();
            itemNameTBox.Text = _item.ID.ToString();
            descriptionTBox.Text = _item.ID.ToString();
            if (((Items)dataGrid.SelectedItem).Image != null)
            {
                //конвертация из базы
                MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                ms.Write(_item.Image, 0, _item.Image.Length);
                imageItemImg.Source = ConvertToBitmap(_item.Image);
            }

            delItemForEvent.Visibility = Visibility.Collapsed;
            addItemForEvent.Visibility = Visibility.Visible;
            countItems.Visibility = Visibility.Visible;
        }
        public BitmapImage ConvertToBitmap(byte[] value)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(value);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                MessageBox.Show("Ошибка изображения в базе данных");
            }
            return null;

        }

        private void dataGridEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.SelectedItem = null;
            if (dataGridEvents.SelectedItem == null)
                return;
            Items _item = ((WareHouse)dataGridEvents.SelectedItem).Items;

            itemIdTBox.Text = _item.ID.ToString();
            itemNameTBox.Text = _item.ID.ToString();
            descriptionTBox.Text = _item.ID.ToString();
            if (((Items)dataGrid.SelectedItem).Image != null)
            {
                //конвертация из базы
                MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                ms.Write(_item.Image, 0, _item.Image.Length);
                imageItemImg.Source = ConvertToBitmap(_item.Image);
            }

            delItemForEvent.Visibility = Visibility.Visible;
            addItemForEvent.Visibility = Visibility.Collapsed;
            countItems.Visibility = Visibility.Collapsed;
        }
    }
}
