using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Tools
{
    public class Pasword
    {
        public const int ANY = 0;
        public const int LOWER_CASE = 1;
        public const int UPPER_CASE = 1 << 1;
        public const int NUMBERS = 1 << 2;
        public const int SPECIALS = 1 << 3;
        public const int ALL = LOWER_CASE | UPPER_CASE | NUMBERS | SPECIALS;

        const string _LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string _UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string _NUMBERS = "123456789";
        const string _SPECIALS = @"!@£$%^&*()#€";

        public static bool IsValid(string pwd, int passwordSize, int options)
        {
            if (pwd.Length < passwordSize) return false;

            bool hasLower = false, hasUpper = false, hasNumbers = false, hasSpecial = false;
            foreach (var c in pwd)
            {
                hasLower = _LOWER_CASE.Contains(c);
                hasUpper = _UPPER_CASE.Contains(c);
                hasNumbers = _NUMBERS.Contains(c);
                hasSpecial = _SPECIALS.Contains(c);
            }
            return (
                (hasLower || (options & LOWER_CASE) == 0)
                && (hasUpper || (options & UPPER_CASE) == 0)
                && (hasNumbers || (options & NUMBERS) == 0)
                && (hasSpecial || (options & SPECIALS) == 0)
            );
        }

        public static string Generate(int passwordSize, int options = ALL)
        {
            var opt = options == ANY ? ALL : options;

            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if ((opt & LOWER_CASE) > 0) charSet += _LOWER_CASE;
            if ((opt & UPPER_CASE) > 0) charSet += _UPPER_CASE;
            if ((opt & NUMBERS) > 0) charSet += _NUMBERS;
            if ((opt & SPECIALS) > 0) charSet += _SPECIALS;
            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }
}
