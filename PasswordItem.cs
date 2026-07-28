using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    internal class PasswordItem
    {
        string comment;
        string password;

        public PasswordItem(string comment, string password)
        {
            this.comment = comment;
            this.password = password;
        }

        void generatePassword()
        {

        }
    }
}
