using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace CodedUITest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        ApplicationUnderTest _app;

        public CodedUITest1()
        {
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\CalData.csv", "CalData#csv", DataAccessMethod.Sequential)]
        [DeploymentItem("caldata.csv")]
        public void CalculatorAdd()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            //this.UIMap.AddMethod();
            int i = Int32.Parse(TestContext.DataRow["num1"].ToString());
            int j = Int32.Parse(TestContext.DataRow["num2"].ToString());
            int result = this.UIMap.Calculate(i, j, CalculatorActionButton.AddButton);
            string actualResult = this.UIMap.GetResult();
            Assert.AreEqual(result.ToString(), actualResult);            
        }

        [TestMethod]
        public void CalculatorMutiply()
        {
            int result = this.UIMap.Calculate(8, 9, CalculatorActionButton.MultiplyButton);
            string actualResult = this.UIMap.GetResult();
            Assert.AreEqual(result.ToString(), actualResult);
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        //Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            _app = ApplicationUnderTest.Launch(@"C:\Windows\System32\calc.exe");
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            _app.Close();        
        }

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
