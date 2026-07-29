using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    public class PasswordItem
    {
        string _comment;
        public string Comment
        { get
            {
                return _comment;
            }

            set
            {
                _comment = value;

            }
        }


        string _password="";

        public PasswordItem(string comment, string password)
        {
            this._comment = comment;
            this._password = password;
        }

        void generatePassword(string chars)
        {

        }
    }
}
