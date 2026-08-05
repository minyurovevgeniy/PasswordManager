using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    static class PasswordGenerator
    {
        const string alphabetUpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string alphabetLowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "1234567890";
        const string specialChars = "!?.,:;-_@#$%";


        public static string generatePassword(UserSettings userSettings)
        {
            StringBuilder chars = new StringBuilder("");

            if ((bool)userSettings.AlphabetUpperCaseLetters)
            {
                chars.Append(alphabetUpperCaseLetters);
            }

            if ((bool)userSettings.AlphabetLowerCaseLetters)
            {
                chars.Append(alphabetLowerCaseLetters);
            }

            if ((bool)userSettings.Digits)
            {
                chars.Append(digits);
            }

            if ((bool)userSettings.SpecialChars)
            {
                chars.Append(specialChars);
            }

            StringBuilder sb = new StringBuilder("");
            Random rand = new Random();

            for (int i = 0; i < userSettings.PasswordLength; i++)
            {
                sb.Append(chars[rand.Next(chars.Length)]);
            }

            return sb.ToString();
        }
    }
}
