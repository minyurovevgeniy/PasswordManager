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


        public static string generatePassword(UserSettings userSettings, int passwordLength)
        {
            StringBuilder chars = new StringBuilder("");

            if (userSettings.AlphabetUpperCaseLetters)
            {
                chars.Append(alphabetUpperCaseLetters);
            }

            if (userSettings.AlphabetLowerCaseLetters)
            {
                chars.Append(alphabetLowerCaseLetters);
            }

            if (userSettings.Digits)
            {
                chars.Append(digits);
            }

            if (userSettings.SpecialChars)
            {
                chars.Append(specialChars);
            }

            StringBuilder sb = new StringBuilder("");
            Random rand = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                sb.Append(chars[rand.Next(chars.Length)]);
            }

            return sb.ToString();
        }
    }
}
