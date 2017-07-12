using System;
using System.IO;
using ExcelChart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [DeploymentItem("Samples", "Samples")]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateExcel()
        {
            using (var creator = new ExcelHelper())
            {
                creator.CreateExcel(@"c:\temp\test");
                creator.GenerateChart(1, @"c:\temp\testChart.png", "png");
            }
        }

        [TestMethod]
        public void ReadExcel()
        {
            var excel = new SpireExcel(@"Samples\test.xlsx");
            excel.Draw();
        }

        [TestMethod]
        public void ExportCharts()
        {
            using (var creator = new ExcelHelper())
            {
                var excelfile = Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Samples\DhaModel.xlsx");
                creator.ReadExcel(excelfile);
                creator.ExportChartsTo("Polar Plots", @"c:\temp", "png");
            }
        }
    }
}
