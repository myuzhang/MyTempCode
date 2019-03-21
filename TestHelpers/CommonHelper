using System;
using System.Collections.Generic;
using System.Linq;
using AutomationFramework.Configuration;
using OpenQA.Selenium.Support.UI;

namespace TestHelpers
{
    public static class CommonHelper
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default(TValue);
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source,
            Dictionary<TKey, TValue> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
                else
                    source[item.Key] = item.Value;
            }
        }

        public static bool WaitFor(Func<bool> func)
        {
            var wait = new DefaultWait<bool>(false)
            {
                Timeout = TimeSpan.FromSeconds(TestRunHelper.Settings.WaitElementInSecond),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };

            return wait.Until(f => func());
        }

        public static bool IsGuid(this string guidString) =>
            !string.IsNullOrWhiteSpace(guidString) && Guid.TryParse(guidString, out _);

        public static bool IsInt(this string numberString) =>
            !string.IsNullOrWhiteSpace(numberString) && int.TryParse(numberString, out _);

        public static DateTime SydneyDateTime
        {
            get
            {
                var delta = DateTime.Now - DateTime.UtcNow;
                var deltaHours = 11 - delta.TotalHours;
                return DateTime.Now.AddHours(deltaHours);
            }
        }
    }
}
