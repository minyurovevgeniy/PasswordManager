using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    class UserSettings
    {
        bool alphabetUpperCaseLetters;
        bool alphabetLowerCaseLetters;
        bool digits;
        bool specialChars;

        public UserSettings(bool _alphabetUpperCaseLetters, bool _alphabetLowerCaseLetters, bool _digits, bool _specialChars)
        {
            alphabetUpperCaseLetters = _alphabetUpperCaseLetters;
            alphabetLowerCaseLetters = _alphabetLowerCaseLetters;
            digits = _digits;
            specialChars = _specialChars;
        }
    }
}
