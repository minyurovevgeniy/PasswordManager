using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PasswordManager
{
    public class PasswordItem
    {
        public string comment { get; set; }
        public string password { get; set; }
    }
}
