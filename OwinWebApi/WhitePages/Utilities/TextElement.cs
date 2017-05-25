using Common;
using System.Linq;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using WhitePages.Enums;

namespace WhitePages.Utilities
{
    public class TextElement : UiElement<Label>
    {
        private readonly FilterOption _option;

        public TextElement(Window window, string text, FilterOption option) : base(window, text)
        {
            _option = option;
        }

        public Label LableElement => GetElements(_option).FirstOrDefault();

        public decimal DecimalValue => LableElement.Text.ToDecimal();
    }
}
