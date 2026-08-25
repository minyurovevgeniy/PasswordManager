using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PasswordManager
{
    public class PasswordItem
    {
        public string id = Guid.NewGuid().ToString();
        public string login { get; set; }
        public string password { get; set; }
        public string comment { get; set; }
    }
}
