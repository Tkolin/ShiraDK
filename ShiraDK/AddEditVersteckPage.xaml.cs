using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiraDK
{
    /// <summary>
    /// Логика взаимодействия для AddEditVersteckPage.xaml
    /// </summary>
    public partial class AddEditVersteckPage : Page
    {
        WareHouse _wareHouse = new WareHouse();
        Items _item = new Items();
        bool add = true;
        BitmapImage _image;
        List<Users> organizers = new List<Users>();

        public AddEditVersteckPage(WareHouse _wareHouse)
        {
             
            InitializeComponent();
            if (_wareHouse != null)
            {
                this._wareHouse = _wareHouse;
                this._item = _wareHouse.Items;
                add = false;
            }
            DataContext = _wareHouse;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (itemIdTBox.Text == null || recepiptDatePicer.SelectedDate == null || countTBox.Text == null ||
                userIdCBox.SelectedItem == null || itemNameTBox.Text == null)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
           
            try
            {
                _wareHouse.DateOfReceipt = (DateTime)recepiptDatePicer.SelectedDate;
                _wareHouse.UserID = ((Users)userIdCBox.SelectedItem).ID;
                _wareHouse.Quantity = Convert.ToInt32(countTBox.Text);

                _item.Name = itemNameTBox.Text;
                _item.Width = Convert.ToDouble(xValueTBox.Text);
                _item.Height = Convert.ToDouble(yValueTBox.Text);
                _item.Length = Convert.ToDouble(zValueTBox.Text);
                _item.Description = descriptionTBox.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте введёные данные!");
                return;
            }
            if (_image != null)
                 _item.Image = ConvertToArray(_image);


            if (add)
            {
                DBEntities.GetContext().WareHouse.Add(_wareHouse);
                DBEntities.GetContext().Items.Add(_item);
            }

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
            catch {
                MessageBox.Show("Ошибка изображения в базе данных");
            }
            return null;

        }

        public byte[] ConvertToArray(BitmapImage value)
        {
            BitmapImage image = (BitmapImage)value;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            userIdCBox.ItemsSource = DBEntities.GetContext().Users.ToList();

            if (!add)
            {
                wareHouseIdTBox.Text = _wareHouse.Number.ToString();
                recepiptDatePicer.SelectedDate = _wareHouse.DateOfReceipt;  
                countTBox.Text = _wareHouse.Quantity.ToString();
                userIdCBox.SelectedItem = _wareHouse.Users;

                itemIdTBox.Text = _item.Name;
                itemNameTBox.Text = _item.Name;
                xValueTBox.Text = _item.Width.ToString();
                yValueTBox.Text = _item.Height.ToString();
                zValueTBox.Text = _item.Length.ToString();
                descriptionTBox.Text = _item.Description;
                if(_item.Image != null)
                {//конвертация из базы
                    MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                    ms.Write(_item.Image, 0, _item.Image.Length);
                    _image = ConvertToBitmap(_item.Image);
                    imageItemImg.Source = _image;
                }
            }
            else
            {
                recepiptDatePicer.SelectedDate = DateTime.Now;
            }
           
        }


        private void LoadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                _image = new BitmapImage(new Uri(filename));
                imageItemImg.Source = _image;
            }
            
        }

        private void deletImageBtn_Click(object sender, RoutedEventArgs e)
        {
            _image = null;
            imageItemImg.Source = null;
        }





    

        private void xValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void yValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void zValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void countTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
    }
}
