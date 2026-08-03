using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        // Свойство, куда сохраняем результат
        public string Result { get; private set; } = string.Empty;
        public InputWindow(string prompt)
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(userInput))
            {
                MessageBox.Show("Поле не должно быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Result = userInput;      // Сохраняем результат в свойство
            this.DialogResult = true; // Важно для ShowDialog()
            this.Close();
        }
    }
}
