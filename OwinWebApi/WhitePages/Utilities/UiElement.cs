using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using WhitePages.Enums;

namespace WhitePages.Utilities
{
    public abstract class UiElement<T> where T : UIItem
    {
        private readonly Window _window;

        private readonly string _searchString;

        protected UiElement(Window window, string searchString)
        {
            _window = window;
            _searchString = searchString;
        }

        protected UiElement(string searchString)
        {
            _searchString = searchString;
        }

        public virtual IEnumerable<T> GetElements(AutomationProperty elementProperty)
        {
            return _window.GetMultiple<T>(SearchCriteria.ByNativeProperty(elementProperty, _searchString));
        }

        public virtual IEnumerable<T> GetElements(FilterOption option)
        {
            switch (option)
            {
                case FilterOption.Equal:
                    return _window.GetMultiple<T>().Where(x => x.Name.Equals(_searchString));
                case FilterOption.Contains:
                    return _window.GetMultiple<T>().Where(x => x.Name.Contains(_searchString));
                case FilterOption.StartWith:
                    return _window.GetMultiple<T>().Where(x => x.Name.StartsWith(_searchString));
                case FilterOption.EndWith:
                    return _window.GetMultiple<T>().Where(x => x.Name.EndsWith(_searchString));
            }
            return null;
        }

        public virtual IEnumerable<T> GetElements()
        {
            return
                _window.GetMultiple<T>(SearchCriteria.All)
                    .Where(
                        i =>
                            !string.IsNullOrEmpty(i.Name) && i.Name.IndexOf(_searchString, StringComparison.CurrentCultureIgnoreCase) != -1
                            || !string.IsNullOrEmpty(i.HelpText) && i.HelpText.IndexOf(_searchString, StringComparison.CurrentCultureIgnoreCase) != -1
                            || !string.IsNullOrEmpty(i.Id) && i.Id.IndexOf(_searchString, StringComparison.CurrentCultureIgnoreCase) != -1
                            || !string.IsNullOrEmpty(i.LegacyHelpText) && i.LegacyHelpText.IndexOf(_searchString, StringComparison.CurrentCultureIgnoreCase) != -1);
        }
    }
}
