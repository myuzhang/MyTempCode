using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages.Utilities
{
    public abstract class PopupDialogWindow
    {
        private readonly DialogWindow _popup;

        protected PopupDialogWindow(Window window, string title)
        {
            _popup = new DialogWindow(window, title);
        }

        public virtual string Message => _popup.GetUiItem<Label>(SearchCriteria.ByAutomationId("65535")).Text;

        public virtual void ClickOk() => _popup.GetUiItem<Button>(SearchCriteria.ByAutomationId("2")).Click();
    }
}
