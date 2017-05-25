using System;
using System.Configuration;
using Common;
using TestStack.White.UIItems;
using WhitePages.Enums;

namespace WhitePages.Utilities
{
    public class AdjusterContainer
    {
        private readonly Label _label;

        public AdjusterContainer(Label label)
        {
            _label = label;
            LastValue = CurrentValue = label.Text;
        }

        public string LastValue { get; private set; }

        public string CurrentValue { get; private set; }
        
        public void ChangeValueByOneClick(AdjustButton button, double offset)
        {
            // store the current value as last value before increase/decrease the value.
            LastValue = CurrentValue;

            if (button.Equals(AdjustButton.Left))
            {
                _label.SetMouseToAdjusterLeft(offset).Click();
            }
            else
            {
                _label.SetMouseToAdjusterRight(offset).Click();
            }
            CurrentValue = _label.Text;
        }

        public bool SetValue(string value, double offset)
        {
            if (value.Equals(CurrentValue))
                return true;

            double expectedNumberValue;
            bool isNumberValue = value.TryToDoubleOmitUnit(out expectedNumberValue);
            int maxTry = Int32.Parse(ConfigurationManager.AppSettings["maxTry"]);
            if (isNumberValue)
            {
                double actualCurrentNumberValue;
                double actualLastNumberValue;
                var button = AdjustButton.Left;
                ChangeValueByOneClick(button, offset);
                if (value.Equals(CurrentValue)) return true;
                CurrentValue.TryToDoubleOmitUnit(out actualCurrentNumberValue);
                LastValue.TryToDoubleOmitUnit(out actualLastNumberValue);
                if (Math.Abs(actualCurrentNumberValue - expectedNumberValue) >
                    Math.Abs(actualLastNumberValue - expectedNumberValue))
                    button = AdjustButton.Right;
                do
                {
                    ChangeValueByOneClick(button, offset);
                    if (maxTry-- == 0)
                        return false;
                } while (!value.Equals(CurrentValue, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                do
                {
                    ChangeValueByOneClick(AdjustButton.Left, offset);
                    if (maxTry-- == 0)
                        return false;
                } while (!value.Equals(CurrentValue, StringComparison.CurrentCultureIgnoreCase));
            }
            return true;
        }

        public bool SetValue(string value, double precision, double offset)
        {
            if (value.Equals(CurrentValue))
                return true;

            double expectedNumberValue;
            bool isNumberValue = value.TryToDoubleOmitUnit(out expectedNumberValue);
            if (isNumberValue)
            {
                int maxTry = Int32.Parse(ConfigurationManager.AppSettings["maxTry"]);
                double actualCurrentNumberValue;
                double actualLastNumberValue;

                CurrentValue.TryToDoubleOmitUnit(out actualCurrentNumberValue);
                if (Math.Abs(actualCurrentNumberValue - expectedNumberValue).Round(2) < precision)
                    return false;

                var button = AdjustButton.Left;
                ChangeValueByOneClick(button, offset);
                if (value.Equals(CurrentValue)) return true;
                CurrentValue.TryToDoubleOmitUnit(out actualCurrentNumberValue);
                LastValue.TryToDoubleOmitUnit(out actualLastNumberValue);
                if (Math.Abs(actualCurrentNumberValue - expectedNumberValue) >
                    Math.Abs(actualLastNumberValue - expectedNumberValue))
                    button = AdjustButton.Right;
                do
                {
                    ChangeValueByOneClick(button, offset);
                    if (value.Equals(CurrentValue, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                    if (maxTry-- == 0)
                        return false;
                    CurrentValue.TryToDoubleOmitUnit(out actualCurrentNumberValue);
                } while (Math.Abs(actualCurrentNumberValue - expectedNumberValue).Round(2) >= precision);
                return false;
            }
            return SetValue(value, offset);
        }
    }
}
