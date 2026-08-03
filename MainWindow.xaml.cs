using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> MyDynamicItems { get; set; }
        public List<PasswordItem> passwords = new List<PasswordItem>();

        UserSettings userSettings;
        public MainWindow()
        {
            InitializeComponent();

            userSettings = new UserSettings(true, true, false, false,5);

            // Initialize the collection
            MyDynamicItems = new ObservableCollection<string>();

            // Dynamically add items anywhere in your logic
            MyDynamicItems.Add("Добавление");
            MyDynamicItems.Add("Изменение");
            MyDynamicItems.Add("Удаление");

            // Set the DataContext to this class for binding
            mode.ItemsSource = MyDynamicItems;
        }

        private void action_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.Replace(comment.Text, @"\s+", "").Equals(""))
            {
                MessageBox.Show("Введите комментарий");
                return;
            }

            if (comment.Text.Equals(""))
            {
                MessageBox.Show("Введите комментарий");
                return;
            }

            if (mode.SelectedIndex == 0)
            {                
                passwords.Add(new PasswordItem(comment.Text, ""));
                comments.Items.Add(comment.Text);
                comment.Text = "";
            }
            else if (mode.SelectedIndex == 1 & comments.SelectedIndex>=0) 
            {
                passwords[comments.SelectedIndex].Comment = comment.Text;
                comments.Items.Clear();

                foreach (var password in passwords)
                {
                    comments.Items.Add(password.Comment);
                }
            }
            else if (mode.SelectedIndex == 2 & comments.SelectedIndex >= 0)
            {
                passwords.RemoveAt(comments.SelectedIndex);
                comments.Items.Clear();

                foreach (var password in passwords)
                {
                    comments.Items.Add(password.Comment);
                }
            }
        }

        private void mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mode.SelectedIndex == 0)
            {
                action.Content = "Добавить";
            }
            else if (mode.SelectedIndex == 1)
            {
                action.Content = "Изменить";
            }
            else if (mode.SelectedIndex == 2)
            {
                action.Content = "Удалить";
            }
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            password.Text = PasswordGenerator.generatePassword(userSettings, 5);
        }
    }
}