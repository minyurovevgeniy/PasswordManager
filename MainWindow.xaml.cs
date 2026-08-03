using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

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
            MyDynamicItems.Add("Добавление комментария и пароля");
            MyDynamicItems.Add("Изменение комментария");
            MyDynamicItems.Add("Удаление комментария и пароля");

            // Set the DataContext to this class for binding
            mode.ItemsSource = MyDynamicItems;
        }

        private void action_Click(object sender, RoutedEventArgs e)
        {
            if (password.Text.Equals(""))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            if (Regex.Replace(password.Text, @"\s+", "").Equals(""))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            if (comment.Text.Equals(""))
            {
                MessageBox.Show("Введите комментарий");
                return;
            }

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

            // Добавление комментария и пароля
            if (mode.SelectedIndex == 0)
            {
                passwords.Add(new PasswordItem(comment.Text, password.Text));
                foreach (var password in passwords)
                {
                    comments.Items.Add(password.comment);
                    passwordsListView.Items.Add(password.password);
                }
                comment.Text = "";
                password.Text = "";
            }
            // Изменение комментария
            else if (mode.SelectedIndex == 1 & comments.SelectedIndex>=0) 
            {
                passwords[comments.SelectedIndex].comment = comment.Text;
                comments.Items.Clear();

                foreach (var password in passwords)
                {
                    comments.Items.Add(password.comment);
                }
            }
            // Удаление комментария и пароля
            else if (mode.SelectedIndex == 2 & comments.SelectedIndex >= 0)
            {
                passwords.RemoveAt(comments.SelectedIndex);
                comments.Items.Clear();

                foreach (var password in passwords)
                {
                    comments.Items.Add(password.comment);
                    passwordsListView.Items.Add(password.password);
                }
            }
            
            if (mode.SelectedIndex<0)
            {
                MessageBox.Show("Выберите режим работы");
            }
        }

        private void mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mode.SelectedIndex == 0)
            {
                action.Content = "Добавить комментарий и пароль";
            }
            else if (mode.SelectedIndex == 1)
            {
                action.Content = "Изменить комментарий";
            }
            else if (mode.SelectedIndex == 2)
            {
                action.Content = "Удалить комментарий и пароль";
            }
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            password.Text = PasswordGenerator.generatePassword(userSettings, 5);
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string entered = "";
            var dialog = new InputWindow("Введите пароль");
            if (dialog.ShowDialog() == true)
            {
               entered = dialog.Result; // Получаем результат   
            }
            SHA256 sha256 = SHA256.Create();
            
            // Convert the input string to a byte array
            byte[] inputBytes = Encoding.UTF8.GetBytes("OK");

            // Compute the hash
            byte[] hashBytes = SHA256.HashData(inputBytes);

            byte[] array = new byte[12];

            SaveFileDialog saveFileDialog = new SaveFileDialog();
                // Configure dialog properties
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "JSON files(*.json)|*.json|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Title = "Сохранить пароли";

            // Show the dialog and check if the user clicked "OK"
            if (saveFileDialog.ShowDialog()==true)
            {
                // 1. Сериализация в JSON + Шифрование
                Cryptography.EncryptAndSave(passwords, saveFileDialog.FileName, hashBytes, array);
                File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(passwords));
            }
        }

        
    }
}