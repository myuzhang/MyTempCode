using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Enums;
using Common.Models;

namespace Common
{
    public static class Utility
    {
        public static string ToSide(this int i)
        {
            switch (i)
            {
                case (int) LeftRight.Left:
                    return LeftRight.Left.ToString().ToLower();
                case (int) LeftRight.Right:
                    return LeftRight.Right.ToString().ToLower();
                default:
                    return String.Empty;
            }
        }

        public static int ToSide(this string side)
        {
            if (LeftRight.Left.ToString().Equals(side, StringComparison.CurrentCultureIgnoreCase))
                return (int) LeftRight.Left;
            if (LeftRight.Right.ToString().Equals(side, StringComparison.CurrentCultureIgnoreCase))
                return (int) LeftRight.Right;
            return 0;
        }

        public static string ToUrlSpace(this string value) =>
            string.IsNullOrEmpty(value) ? null : value.Replace(" ", "%20");

        public static DateTime ToDateTime(this string value, string format) =>
            string.IsNullOrEmpty(value)
                ? default(DateTime)
                : DateTime.ParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture);

        public static string RemoveLeadingQuestionNumber(this string value) =>
            string.IsNullOrEmpty(value) ? null : Regex.Replace(value, @"(^\w+\d+.|^\d+.)", string.Empty).Trim();

        public static string RemoveHtmlTab(this string value) =>
            string.IsNullOrEmpty(value) ? null : Regex.Replace(value, @"(<br />|<strong>|</strong>)", string.Empty).Trim();

        public static string GetFirstName(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var names = value.Split(' ');
            return names[0].FirstCharToUpper();
        }

        public static string FirstCharToUpper1(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return value.First().ToString().ToUpper() + string.Join("", value.Skip(1));
        }

        public static bool IsQuestionContainKeyword(this EntityQuestionSet set, string keyword)
        {
            if (set == null || string.IsNullOrEmpty(keyword))
                return false;

            IList<string> answers = new List<string>
            {
                set.AnswerTitle1,
                set.AnswerTitle2,
                set.AnswerTitle3,
                set.AnswerTitle4,
                set.AnswerTitle5
            };

            return answers.Any(a => a.ToLower().Contains(keyword.ToString()));
        }
    }
}