using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages.Utilities
{
    public class DialogWindow
    {
        private readonly Window _modalWindows;

        public DialogWindow(Window window, string title)
        {
            _modalWindows = window.ModalWindow(title);
        }

        public T GetUiItem<T>(SearchCriteria searchCriteria) where T : class => _modalWindows.Get(searchCriteria) as T;
    }
}
