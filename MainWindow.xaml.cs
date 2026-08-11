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
        public List<PasswordItem> passwordList = new List<PasswordItem>();

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
            MyDynamicItems.Add("Изменение пароля");
            MyDynamicItems.Add("Удаление комментария и пароля");
            // Set the DataContext to this class for binding
            mode.ItemsSource = MyDynamicItems;
        }

        private void action_Click(object sender, RoutedEventArgs e)
        {
            // Добавление комментария и пароля
            if (mode.SelectedIndex == 0)
            {
                if (Regex.Replace(commentTextBox.Text, @"\s+", "").Equals(""))
                {
                    MessageBox.Show("Введите комментарий");
                    return;
                }

                if (Regex.Replace(passwordTextBox.Text, @"\s+", "").Equals(""))
                {
                    MessageBox.Show("Введите пароль");
                    return;
                }

                passwordList.Add(new PasswordItem { comment = commentTextBox.Text, password = passwordTextBox.Text});

                commentsListView.Items.Add(commentTextBox.Text);
                passwordsListView.Items.Add(passwordTextBox.Text);

                commentTextBox.Text = "";
                passwordTextBox.Text = "";
            }
            // Изменение комментария
            if (mode.SelectedIndex == 1)
            {
                if (commentsListView.SelectedIndex >= 0)
                {
                    if (Regex.Replace(commentTextBox.Text, @"\s+", "").Equals(""))
                    {
                        MessageBox.Show("Введите комментарий");
                        return;
                    }

                    passwordList[commentsListView.SelectedIndex].comment = commentTextBox.Text;
                    commentsListView.Items.Clear();

                    foreach (var password in passwordList)
                    {
                        commentsListView.Items.Add(password.comment);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите комментарий");
                }
            }
            
            // Изменение пароля
            if(mode.SelectedIndex == 2)
            {
                if (passwordsListView.SelectedIndex >= 0)
                {
                    if (Regex.Replace(passwordTextBox.Text, @"\s+", "").Equals(""))
                    {
                        MessageBox.Show("Введите пароль");
                        return;
                    }

                    passwordList[passwordsListView.SelectedIndex].password = passwordTextBox.Text;
                    passwordsListView.Items.Clear();

                    foreach (var password in passwordList)
                    {
                        passwordsListView.Items.Add(password.password);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите пароль");
                }
            }
            // Удаление комментария и пароля
            if (mode.SelectedIndex == 3)
            {
                if (commentsListView.SelectedIndex >= 0)
                {
                    passwordList.RemoveAt(commentsListView.SelectedIndex);
                    commentsListView.Items.Clear();

                    foreach (var password in passwordList)
                    {
                        commentsListView.Items.Add(password.comment);
                        passwordsListView.Items.Add(password.password);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите комментарий");
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
                action.Content = "Изменить пароль";
            }
            else if (mode.SelectedIndex == 3)
            {
                action.Content = "Удалить комментарий и пароль";
            }
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Text = PasswordGenerator.generatePassword(userSettings);
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {

            // Окно для ввода мастер-пароля
            InputWindow masterPassword = new InputWindow("Мастер-пароль");

            masterPassword.Owner = this;
            masterPassword.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (masterPassword.ShowDialog()==false)
            {
                MessageBox.Show("Введите мастер-пароль");
                return;
            }
            SHA256 sha256 = SHA256.Create();
            
            // Convert the input string to a byte array
            byte[] inputBytes = Encoding.UTF8.GetBytes(masterPassword.Result);

            // Compute the hash
            byte[] hashBytes = SHA256.HashData(inputBytes);

            byte[] arrayIV = new byte[12];
            for (int i = 0; i < arrayIV.Length; i++)
            {
                arrayIV[i] = 2;
            }


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "JSON files(*.json)|*.json|All files(*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Title = "Сохранить пароли";

            if (saveFileDialog.ShowDialog()==true)
            {
                CryptographyClass.EncryptAndSave(passwordList, saveFileDialog.FileName, hashBytes, arrayIV);
                
            }
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 2. Configure properties
            
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // 3. Show the dialog and check if the user clicked 'OK'
            if (openFileDialog.ShowDialog() == true)
            {
                // Окно для ввода мастер-пароля
                InputWindow masterPassword = new InputWindow("Мастер-пароль");

                masterPassword.Owner = this;
                masterPassword.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (masterPassword.ShowDialog() == false)
                {
                    MessageBox.Show("Введите мастер-пароль");
                    return;
                }
                byte[] inputBytes = Encoding.UTF8.GetBytes(masterPassword.Result);
                // Compute the hash
                byte[] hashBytes = SHA256.HashData(inputBytes);
                passwordList = CryptographyClass.LoadAndDecrypt<PasswordItem>(openFileDialog.FileName, hashBytes);
                commentsListView.Items.Clear();
                passwordsListView.Items.Clear();
                foreach (PasswordItem passwordItem in passwordList)
                {
                    commentsListView.Items.Add(passwordItem.comment);
                    passwordsListView.Items.Add(passwordItem.password);
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var SetWindow = new SettingsWindow(userSettings);

            SetWindow.Owner = this;
            SetWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (SetWindow.ShowDialog() == true)
            {
                userSettings = SetWindow.settings;
            }
        }

        private void passwordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
        }
    }
}