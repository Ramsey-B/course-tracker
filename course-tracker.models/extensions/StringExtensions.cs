using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace course_tracker.models.extensions
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
            return new EmailAddressAttribute().IsValid(email);
        }

        public static bool IsValidPhoneNumber(this string str)
        {
            if (str.IsNull()) return false;
            return str.All(char.IsDigit) && str.Length == 10;
        }
    }
}
