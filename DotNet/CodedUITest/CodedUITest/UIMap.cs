namespace CodedUITest
{
    using System;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    using System.Drawing;
    using System.Windows.Input;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    
    
    public partial class UIMap
    {
        private WinWindow _calculator;

        public WinWindow Calculator
        {
            get
            {
                if (_calculator == null)
                {
                    _calculator = new WinWindow();
                    _calculator.SearchConfigurations.Add(SearchConfiguration.VisibleOnly);
                    _calculator.SearchProperties[WinWindow.PropertyNames.Name] = "Calculator";
                    _calculator.SearchProperties[WinWindow.PropertyNames.ClassName] = "CalcFrame";
                    _calculator.WindowTitles.Add("Calculator");
                }
                return _calculator;
            }
        }

        public string GetResult()
        {
            var text = new WinText(Calculator);
            text.SearchProperties[WinText.PropertyNames.Name] = CalculatorActionButton.ResultText;
            text.WindowTitles.Add("Calculator");
            return text.DisplayText;
        }

        public int Calculate(int i, int j, string action)
        {
            Mouse.Click(GetNumButton(i));
            Mouse.Click(GetActionButton(action));
            Mouse.Click(GetNumButton(j));
            Mouse.Click(GetActionButton(CalculatorActionButton.EqualsButton));

            switch (action)
            {
                case CalculatorActionButton.AddButton:
                    return i + j;
                case CalculatorActionButton.MultiplyButton:
                    return i * j;
            }
            return 0;
        }

        private WinButton GetNumButton(int num)
        {
            return GetCalButton(num.ToString());
        }

        private WinButton GetActionButton(string action)
        {
            return GetCalButton(action);
        }

        private WinButton GetCalButton(string button)
        {
            var winButton = new WinButton(Calculator);
            winButton.SearchConfigurations.Add(SearchConfiguration.VisibleOnly);
            winButton.SearchProperties[WinButton.PropertyNames.Name] = button;
            winButton.WindowTitles.Add("Calculator");
            return winButton;
        }
    }
}
