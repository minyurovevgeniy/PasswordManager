using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    public class PasswordItem
    {
        string _comment;
        public string Comment
        { 
            get
            {
                return _comment;
            }

            set
            {
                _comment = value;

            }
        }

        string _password = "";
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;

            }
        }

        public PasswordItem(string comment, string password)
        {
            this._comment = comment;
            this._password = password;
        }

        public void generatePassword(string chars,int passwordLength)
        {
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                int maxLetterCount = rand.Next(chars.Length);
                sb.Append(chars[maxLetterCount]);
            }

            _password = sb.ToString();
        }
    }
}
