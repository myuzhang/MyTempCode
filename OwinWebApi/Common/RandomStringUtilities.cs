using System;
using System.Linq;

namespace Common
{
    public static class RandomStringUtilities
    {
        // refer to: http://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        public static string RandomString(string feed, int length = 8)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(feed, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string WithNumber(int length = 8)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return RandomString(chars, length);
        }

        public static string WithOnlyNumber(int length = 8)
        {
            const string chars = "0123456789";
            return RandomString(chars, length);
        }

        public static string WithoutNumber(int length = 8)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            return RandomString(chars, length);
        }

        public static string AsName(int length = 8)
        {
            return WithoutNumber(1).ToUpper() + WithoutNumber(length - 1).ToLower();
        }

        public static string AsEmail(int length = 8)
        {
            return WithoutNumber(8) + '@' + WithoutNumber(4) + ".com";
        }

        public static string ToEmail(this string value)
        {
            return value + '@' + WithoutNumber(4) + ".com";
        }

        public static string AsPhoneNumber(int length = 10)
        {
            return "04" + WithOnlyNumber(length - 2);
        }
    }
}
