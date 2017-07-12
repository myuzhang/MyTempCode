using System.Drawing.Imaging;
using Spire.Xls;

namespace ExcelChart
{
    public class SpireExcel
    {
        private Workbook _workbook;

        public SpireExcel(string fileName)
        {
            _workbook = new Workbook();
            _workbook.LoadFromFile(fileName);
        }

        public void Draw()
        {
            Worksheet sheet = _workbook.Worksheets[0];

            sheet.GridLinesVisible = false;

            Chart chart = sheet.Charts.Add(ExcelChartType.ScatterMarkers);
            chart.DataRange = sheet.Range["A2:D38"];
            chart.SeriesDataFromRange = false;

            chart.LeftColumn = 1;
            chart.TopRow = 6;
            chart.RightColumn = 9;
            chart.BottomRow = 25;

            sheet.Range["A2:L38"].Style.NumberFormat = "0;-0;0E+0;0E-0";

            chart.Series[0].CategoryLabels = sheet.Range["B2:B38"];
            chart.Series[0].Values = sheet.Range["A2:A38"];
            chart.Series[1].TrendLines.Add(TrendLineType.Linear);

            chart.Series[1].CategoryLabels = sheet.Range["D2:D38"];
            chart.Series[1].Values = sheet.Range["C2:C38"];
            //chart.Series[1].TrendLines.Add(TrendLineType.Exponential);

            //_workbook.SaveToFile("XYChart.xlsx", FileFormat.Version2013);
            _workbook.Save();
            System.Drawing.Image[] imgs = _workbook.SaveChartAsImage(sheet);

            for (int i = 0; i < imgs.Length; i++)
                imgs[i].Save($"img-{i}.png", ImageFormat.Png);

        }
    }
}
