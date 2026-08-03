using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    public class PasswordItem
    {
        public string comment;
        public string password = "";

        public PasswordItem(string comment, string password)
        {
            this.comment = comment;
            this.password = password;
        }
    }
}
