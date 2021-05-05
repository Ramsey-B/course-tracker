using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace course_tracker.Models.extensions
{
    public static class StringExtensions
    {
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsValidEmail(this string email)
        {
            if (email.IsNull()) return false;
            var atIndex = -1;
            var dotIndex = -1;

            for (int i = 0; i < email.Length; i++)
            {
                var c = email[i];
                if (c == '@') atIndex = i;

                if (c == '.' && atIndex > -1)
                {
                    if (dotIndex > -1) return false; // multi dots after the @.
                    dotIndex = i;
                }
            }

            if (atIndex <= 0) return false; // missing @ or @ is the first char
            if (dotIndex == -1 || dotIndex == email.Length - 1) return false; // dot is missing or its the last char
            return true;
        }

        public static bool IsValidPhoneNumber(this string str, string mask = "(XXX) XXX-XXXX")
        {
            if (str.Length != mask.Length) return false;
            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                var m = mask[i];

                if (m == 'X' || m == 'x')
                {
                    if (!char.IsNumber(c)) return false;
                }
                else
                {
                    if (c != m) return false;
                }
            }
            return true;
        }
    }
}
