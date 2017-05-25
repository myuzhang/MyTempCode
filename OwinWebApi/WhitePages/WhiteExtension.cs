using System;
using System.Drawing;
using System.Windows.Automation;
using Common;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages
{
    public static class WhiteExtension
    {
        public static T GetNextSibling<T>(this IUIItem thisItem) where T : IUIItem
        {
            var parent = TreeWalker.ControlViewWalker.GetNextSibling(thisItem.AutomationElement);
            var uiItem = new DictionaryMappedItemFactory().Create(parent, thisItem.ActionListener);
            return (T) UIItemProxyFactory.Create(uiItem, uiItem.ActionListener);
        }

        public static T GetPreviousSibling<T>(this IUIItem thisItem) where T : IUIItem
        {
            var parent = TreeWalker.ControlViewWalker.GetPreviousSibling(thisItem.AutomationElement);
            var uiItem = new DictionaryMappedItemFactory().Create(parent, thisItem.ActionListener);
            return (T) UIItemProxyFactory.Create(uiItem, uiItem.ActionListener);
        }

        public static IUIItem GetParent(this Window window, SearchCriteria searchCriteria)
        {
            IUIItem thisItem = window.Get(searchCriteria);
            var parent = TreeWalker.ControlViewWalker.GetParent(thisItem.AutomationElement);
            var uiItem = new DictionaryMappedItemFactory().Create(parent, thisItem.ActionListener);
            return UIItemProxyFactory.Create(uiItem, uiItem.ActionListener);
        }

        public static T GetNextType<T>(this Window window, SearchCriteria searchCriteria) where T : IUIItem
        {
            T thisItem = window.Get<T>(searchCriteria);
            return thisItem.GetNextSibling<T>();
        }

        public static T GetNext<T>(this Window window, SearchCriteria searchCriteria) where T : IUIItem
        {
            var thisItem = window.Get(searchCriteria);
            return thisItem.GetNextSibling<T>();
        }

        public static T GetPreviousType<T>(this Window window, SearchCriteria searchCriteria) where T : IUIItem
        {
            T thisItem = window.Get<T>(searchCriteria);
            return thisItem.GetPreviousSibling<T>();
        }

        public static T GetPrevious<T>(this Window window, SearchCriteria searchCriteria) where T : IUIItem
        {
            var thisItem = window.Get(searchCriteria);
            return thisItem.GetPreviousSibling<T>();
        }

        public static Mouse SetMouseToAdjusterLeft(this IUIItem thisItem, double offset)
        {
            Mouse.Instance.Location = new System.Windows.Point(thisItem.ClickablePoint.X - offset,
                thisItem.ClickablePoint.Y);
            return Mouse.Instance;
        }

        public static Mouse SetMouseToAdjusterRight(this IUIItem thisItem, double offset)
        {
            Mouse.Instance.Location = new System.Windows.Point(thisItem.ClickablePoint.X + offset,
                thisItem.ClickablePoint.Y);
            return Mouse.Instance;
        }

        public static bool ClickIgnoreException(this UIItem item)
        {
            try
            {
                item.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public static Bitmap TakeScreenShot(this UIItem item) => Desktop.CaptureScreenshot(item.Bounds);

        public static Bitmap TakeFullScreenShot() => Desktop.CaptureScreenshot();

        public static bool CampareScreenShot(this UIItem item, Bitmap expected)
        {
            var actual = item.TakeScreenShot();
            if (!actual.Height.Equals(expected.Height) ||
                !actual.Width.Equals(expected.Width))
                return false;
            return actual.CompareScreenShot(expected);
        }
    }
}
