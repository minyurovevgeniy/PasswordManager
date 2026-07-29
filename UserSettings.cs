using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager
{
    class UserSettings
    {
        private bool alphabetUpperCaseLetters;
        public bool AlphabetUpperCaseLetters
        {  get
            {
                return alphabetUpperCaseLetters;
            }
        }
        bool alphabetLowerCaseLetters;
        public bool AlphabetLowerCaseLetters
        {
            get
            {
                return alphabetLowerCaseLetters;
            }
        }
        
        bool digits;
        public bool Digits
        {
            get
            {
                return digits;
            }
        }

        bool specialChars;
        public bool SpecialChars
        {
            get
            {
                return specialChars;
            }
        }

        public UserSettings(bool _alphabetUpperCaseLetters, bool _alphabetLowerCaseLetters, bool _digits, bool _specialChars)
        {
            alphabetUpperCaseLetters = _alphabetUpperCaseLetters;
            alphabetLowerCaseLetters = _alphabetLowerCaseLetters;
            digits = _digits;
            specialChars = _specialChars;
        }
    }
}
