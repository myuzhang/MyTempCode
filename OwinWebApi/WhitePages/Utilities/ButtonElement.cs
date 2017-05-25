using System.Linq;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages.Utilities
{
    public class ButtonElement : UiElement<Button>
    {
        public ButtonElement(Window window, string searchString) : base(window, searchString)
        {
        }

        public ButtonElement(string searchString) : base(searchString)
        {
        }

        public Button FirstMatchedButton => GetElements().FirstOrDefault();

        public void ClickFirtMatchedButton()
        {
            if (FirstMatchedButton.Enabled)
            {
                FirstMatchedButton.Click();
            }
        }

        public bool IsMatchedButtonClickable()
        {
            return !FirstMatchedButton.Enabled;
        }
    }
}
