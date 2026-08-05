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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public bool alphabetUpperCaseLetters = false;
        public bool alphabetLowerCaseLetters = false;
        public bool digits = false;
        public bool specialChars = false;
        public int passwordLength = 2;

        public UserSettings settings;

        public SettingsWindow(UserSettings userSettings)
        {
            InitializeComponent();
            
            AlphabetUpperCaseLetters.IsChecked = userSettings.AlphabetUpperCaseLetters;
            passwordLengthSlider.Value = userSettings.PasswordLength;
            passwordLengthText.Text = "Длина пароля: " + userSettings.PasswordLength.ToString();
            AlphabetLowerCaseLetters.IsChecked = userSettings.AlphabetLowerCaseLetters;
            Digits.IsChecked = userSettings.Digits;
            SpecialChars.IsChecked = userSettings.SpecialChars;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            settings = new UserSettings(AlphabetUpperCaseLetters.IsChecked, AlphabetLowerCaseLetters.IsChecked, Digits.IsChecked, SpecialChars.IsChecked, passwordLength);
            this.DialogResult = true;
            this.Close();
        }

        private void passwordLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            passwordLength = (int)e.NewValue;
            if (!this.IsLoaded) return;
            passwordLengthText.Text = "Длина пароля: " + passwordLength.ToString();
        }
    }
}
