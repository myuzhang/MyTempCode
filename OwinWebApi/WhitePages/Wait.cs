using System;
using System.Threading;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages
{
    public static class Wait
    {
        public static bool UntilElementAppear(Func<object> element)
        {
            try
            {
                if (element != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static T GetWithWait<T>(this Window window, SearchCriteria searchCriteria, int timeout = 30) where T : UIItem
        {
            for (int i = 0; i < timeout; i++)
            {
                var result = window.Get<T>(searchCriteria);
                if (result != null)
                {
                    return result;
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return null;
        }

        public static bool Until(Func<bool> func, int timeout = 30)
        {
            for (int i = 0; i < timeout; i++)
            {
                if (func())
                    return true;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            return false;
        }

        public static void FromMilliSeconds(int fromMilliSeconds = 100) => Thread.Sleep(fromMilliSeconds);

        public static void FromSeconds(int fromSeconds = 1) => Thread.Sleep(TimeSpan.FromSeconds(fromSeconds));
    }
}
