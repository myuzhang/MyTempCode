using Common;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using WhitePages.Enums;

namespace WhitePages.Utilities
{
    public class Adjuster
    {
        private readonly AdjusterContainer _adjusterContainer;
        
        private const double OffsetPercentage = 0.30;

        private readonly double _offset;

        public Adjuster(Window window, SearchCriteria searchCriteria)
        {
            _adjusterContainer = new AdjusterContainer(window.GetNextType<Label>(searchCriteria));
            var root = window.GetParent(searchCriteria);
            _offset = (root.Bounds.Right - root.Bounds.Left) * OffsetPercentage;
        }

        public decimal LastValue => _adjusterContainer.LastValue.ToDecimal();

        public decimal CurrentValue => _adjusterContainer.CurrentValue.ToDecimal();

        public void ChangeValueByOneClick(AdjustButton button)
        {
            _adjusterContainer.ChangeValueByOneClick(button, _offset);
        }

        public bool SetValue(string value)
        {
            return _adjusterContainer.SetValue(value, _offset);
        }

        public bool SetValue(string value, double precision)
        {
            return _adjusterContainer.SetValue(value, precision, _offset);
        }
    }
}
