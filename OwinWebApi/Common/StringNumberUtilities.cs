using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class StringNumberUtilities
    {
        // as long as there is at least one number in the string, otherwise it is 0
        // if minus symbol is at the start of the string, then it is parsed as negative value
        public static decimal ToDecimal(this string stringValue)
        {
            decimal result = 0m;
            bool negative = false;

            if (string.IsNullOrEmpty(stringValue))
            {
                return result;
            }

            if (stringValue.StartsWith("-"))
            {
                stringValue = stringValue.Substring(1);
                negative = true;
            }

            string[] seg = stringValue.Split('.');
            string p1 = Regex.Match(seg[0], @"\d+").Value;
            if (seg.Length > 1)
            {
                string p2 = Regex.Match(seg[1], @"^\d+").Value;
                p1 = p1 + '.' + p2;
            }

            if (decimal.TryParse(p1, out result))
            {
                if (negative)
                {
                    return (0 - result);
                }
                return result;
            }
            return result;
        }

        public static bool TryToDoubleOmitUnit(this string stringValue, out double doubleValue, params string[] units)
        {
            if (units.Length.Equals(0))
                units = new[] {"mm", "°"};

            foreach (var unit in units)
            {
                if (stringValue.Contains(unit))
                    stringValue = stringValue.Replace(unit, string.Empty);
            }

            return double.TryParse(stringValue.Trim(), out doubleValue);
        }

        public static string ShiftString(this string t)
        {
            return t.Substring(1, t.Length - 1) + t.Substring(0, 1);
        }

        public static string RemoveFromStart(this string t, int num)
        {
            return t.Substring(num - 1);
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string GetFirstDouble(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException($"The string {stringValue} doesn't contain any number");
            }
            var match = Regex.Match(stringValue, @"\d+\.\d+");
            if (match.Success)
            {
                return match.Value;
            }
            match = Regex.Match(stringValue, @"\d+");
            if (match.Success)
            {
                return match.Value;
            }
            return null;
        }

        public static string GetFirstInt(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException($"The string {stringValue} doesn't contain any number");
            }
            var match = Regex.Match(stringValue, @"\d+");
            if (match.Success)
            {
                return match.Value;
            }
            return null;
        }

        public static double AsDouble(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new ArgumentException($"The string {stringValue} doesn't contain any number");
            }
            double value;
            Double.TryParse(stringValue, out value);
            return value;
        }

        public static string Remove(this string stringValue, string removing)
        {
            return Regex.Replace(stringValue, removing, "");
        }

        public static string RemoveSpace(this string stringValue)
        {
            return stringValue.Replace(" ", string.Empty);
        }

        public static string GetFullFileName(this string stringValue)
        {
            var splits = stringValue.Split('\\');
            return splits.Last();
        }

        public static string GetShortFileName(this string stringValue)
        {
            var fileFullName = stringValue.GetFullFileName();
            var splits = stringValue.Split('.');
            return splits.First();
        }

        public static string AppendFileSuffix(this string stringValue, string suffix = ".kic") =>
            stringValue + suffix;

        public static string GetSolutionRootDir()
        {
            var testPath = AppDomain.CurrentDomain.BaseDirectory;
            int start = testPath.IndexOf("MyApp", StringComparison.CurrentCultureIgnoreCase);
            return testPath.Substring(0, start);
        }

        public static string WithSolutionRootDir(this string stringValue) =>
            Path.Combine(GetSolutionRootDir(), stringValue);

        public static string ToAsciiString(this string hexString)
        {
            var hexStrings = hexString.Split('-');
            StringBuilder builder = new StringBuilder();
            foreach (var hs in hexStrings)
            {
                builder.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return builder.ToString();
        }

        public static string ToAsciiHex(this string value)
        {
            // Convert the string into a byte[].
            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < asciiBytes.Length; i++)
            {
                builder.Append(asciiBytes[i]);
                if (i < asciiBytes.Length - 1)
                    builder.Append("-");
            }
            return builder.ToString();
        }

        public static double Round(this double value, int precision) =>
            Math.Round(value, precision);
    }
}
