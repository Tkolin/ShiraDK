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
    /// Логика взаимодействия для AddEditEventPage.xaml
    /// </summary>
    public partial class AddEditEventPage : Page
    {
 
        Events _event = new Events();
        Organizers _organizers = new Organizers();
        List<Organizers> organizers = new List<Organizers>();
        bool add = true;
        
        public AddEditEventPage(Events _event)
        {
 
            InitializeComponent();
            if (_event != null)
            {
                this._event = _event;
                add = false;
            }

            DataContext = _event;
            DataContext = _organizers;
 
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (eventNameTBox.Text == null || startTimeDatePicer.SelectedDate == null ||
                 durationTBox.Text == null || orginazerIdCBox.SelectedItem == null || countTBox.Text.Length == 0)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            try
            {
                _event.AvailableOfSeats = Convert.ToInt32(countTBox.Text);
                _event.NumberOfSeats = Convert.ToInt32(countTBox.Text);
                _event.Name = eventNameTBox.Text;
                _event.DateStart = (DateTime)startTimeDatePicer.SelectedDate;
                _event.Description = DescriptionTBox.Text;
                _event.Duration = Convert.ToDouble(durationTBox.Text);
                _event.Price = Convert.ToDouble(priceTBox.Text);
                _event.OrganizerID = ((Organizers)orginazerIdCBox.SelectedValue).ID;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте введёные данные!");
                return;
            }
            if (add) 
                DBEntities.GetContext().Events.Add(_event);
            
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
   


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }



        private void addOrganaizerBtn_Click(object sender, RoutedEventArgs e)
        {
            closeAddOrganaizerBtn.Visibility = Visibility.Visible;
            saveAddOrganaizerBtn.Visibility = Visibility.Visible;
            InformatPanel.Visibility = Visibility.Collapsed;
            orginazerIdCBox.IsEnabled = false;
            orginazerNameTBox.IsEnabled = true;
            innTBox.IsEnabled = true;
        }

        private void saveAddOrganaizerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (innTBox.Text.Length < 11)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            _organizers.Name = orginazerNameTBox.Text;
            _organizers.INN = innTBox.Text;

            DBEntities.GetContext().Organizers.Add(_organizers);


            try
            {
                DBEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            orginazerIdCBox.ItemsSource = DBEntities.GetContext().Organizers.ToList();

            closeAddOrganaizerBtn.Visibility = Visibility.Collapsed;
            saveAddOrganaizerBtn.Visibility = Visibility.Collapsed;
            InformatPanel.Visibility = Visibility.Visible;
            orginazerIdCBox.IsEnabled = true;
            orginazerNameTBox.IsEnabled = false;
            innTBox.IsEnabled = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            saveAddOrganaizerBtn.Visibility = Visibility.Collapsed;
            closeAddOrganaizerBtn.Visibility= Visibility.Collapsed;          
            orginazerNameTBox.IsEnabled = false;
            innTBox.IsEnabled = false;

            orginazerIdCBox.ItemsSource = DBEntities.GetContext().Organizers.ToList();

            if (!add)
            {
                eventIdTBox.Text = _event.ID.ToString();
                eventNameTBox.Text = _event.Name.ToString();
                startTimeDatePicer.SelectedDate = _event.DateStart;
                DescriptionTBox.Text = _event.Description;
                durationTBox.Text = _event.Duration.ToString();
                priceTBox.Text = _event.Price.ToString();

                orginazerIdCBox.SelectedItem = _event.Organizers;                
            }
        }

        private void orginazerIdCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InformatPanel.Visibility = Visibility.Visible;
       

        }

        private void closeAddOrganaizerBtn_Click(object sender, RoutedEventArgs e)
        {
            closeAddOrganaizerBtn.Visibility= Visibility.Collapsed;
            saveAddOrganaizerBtn.Visibility = Visibility.Collapsed;
            InformatPanel.Visibility = Visibility.Visible;
            orginazerIdCBox.IsEnabled = true;
            orginazerNameTBox.IsEnabled = false;
            innTBox.IsEnabled = false;
        }

        private void innTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;
            // if (innTBox.Text.Length >= 11)
               // return;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void durationTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void priceTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

            if (!Char.IsDigit(e.Text, 0)) // Проверяем, является ли введенный символ цифрой
            {
                e.Handled = true;
                return;
            }
        }

        private void countTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countTBox.Text != "")
            {
                int value = int.Parse(countTBox.Text);
                if (value > 55)
                {
                    countTBox.Text = "55";
                    countTBox.SelectionStart = countTBox.Text.Length;
                }
            }
        }
    }
}
