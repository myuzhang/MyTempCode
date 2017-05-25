using Common;
using System;
using System.Linq;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace WhitePages.Utilities
{
    public class PairTextValue
    {
        private readonly Window _window;

        private readonly string _leftValue;

        private readonly string _rightValue;

        public PairTextValue(Window window, string leftValue, string rightValue)
        {
            _window = window;
            _leftValue = leftValue;
            _rightValue = rightValue;
        }

        public decimal GetMaxValue => SelectMaxMinValue(true);

        public decimal GetMinValue => SelectMaxMinValue(false);

        private decimal SelectMaxMinValue(bool max)
        {
            var leftValue = _window.GetMultiple<Label>().First(x => x.Name.Contains(_leftValue)).Text.ToDecimal();
            var rightValue = _window.GetMultiple<Label>().First(x => x.Name.Contains(_rightValue)).Text.ToDecimal();

            if (max)
            {
                return Math.Max(leftValue, rightValue);
            }
            return Math.Min(leftValue, rightValue);
        }
    }
}
