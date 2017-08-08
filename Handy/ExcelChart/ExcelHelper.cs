using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelChart
{
    public class ExcelHelper : IDisposable
    {
        private readonly Excel.Application _xlApp;

        private Excel.Workbook _xlWorkBook;

        private readonly object _misValue;

        public ExcelHelper()
        {
            _xlApp = new Excel.Application();

            if (_xlApp == null)
                throw new DriveNotFoundException("Excel is not properly installed!!");

            _misValue = System.Reflection.Missing.Value;
        }

        public void CreateExcel(string fileName)
        {
            _xlWorkBook = _xlWorkBook ?? _xlApp.Workbooks.Add(_misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[1];

            //add data 
            AddData(xlWorkSheet);

            _xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlOpenXMLWorkbook, _misValue,
                _misValue, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                Excel.XlSaveConflictResolution.xlUserResolution, true,
                _misValue, _misValue, _misValue);
        }

        public void ReadExcel(string fileName)
        {
            _xlWorkBook = _xlApp.Workbooks.Open(fileName);
        }

        public void GenerateChart(int item, string chartFileName, string format)
        {
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[item];

            var chartPage = MakePolarChart(xlWorkSheet);

            //export chart as picture file
            chartPage.Export(chartFileName, format, _misValue);
        }
        
        private Excel.Chart MakePolarChart(Excel.Worksheet xlWorkSheet)
        {
            xlWorkSheet.Range["A1", "A5"].Value2 = 22;
            xlWorkSheet.Range["B1", "B5"].Value2 = 55;

            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = xlCharts.Add(10, 80, 300, 250);
            Excel.Chart chartPage = myChart.Chart;

            chartPage.SetSourceData(xlWorkSheet.Range["A1", "B5"],
                Excel.XlRowCol.xlColumns);
            chartPage.ChartType = Excel.XlChartType.xlRadar;

            Excel.ChartGroup group =
                (Excel.ChartGroup)chartPage.RadarGroups(1);

            group.HasRadarAxisLabels = true;

            return chartPage;
        }

        private Excel.Chart MakeColumnChart(Excel.Worksheet xlWorkSheet)
        {
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = xlCharts.Add(10, 80, 300, 250);
            Excel.Chart chartPage = myChart.Chart;

            var chartRange = xlWorkSheet.Range["A1", "d5"];
            chartPage.SetSourceData(chartRange, _misValue);
            chartPage.ChartType = Excel.XlChartType.xlColumnClustered;

            return chartPage;
        }

        private void AddData(Excel.Worksheet xlWorkSheet)
        {
            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, 2] = "Student1";
            xlWorkSheet.Cells[1, 3] = "Student2";
            xlWorkSheet.Cells[1, 4] = "Student3";

            xlWorkSheet.Cells[2, 1] = "Term1";
            xlWorkSheet.Cells[2, 2] = "80";
            xlWorkSheet.Cells[2, 3] = "65";
            xlWorkSheet.Cells[2, 4] = "45";

            xlWorkSheet.Cells[3, 1] = "Term2";
            xlWorkSheet.Cells[3, 2] = "78";
            xlWorkSheet.Cells[3, 3] = "72";
            xlWorkSheet.Cells[3, 4] = "60";

            xlWorkSheet.Cells[4, 1] = "Term3";
            xlWorkSheet.Cells[4, 2] = "82";
            xlWorkSheet.Cells[4, 3] = "80";
            xlWorkSheet.Cells[4, 4] = "65";

            xlWorkSheet.Cells[5, 1] = "Term4";
            xlWorkSheet.Cells[5, 2] = "75";
            xlWorkSheet.Cells[5, 3] = "82";
            xlWorkSheet.Cells[5, 4] = "68";
        }

        public void ExportCharts(string item, string folder, string format)
        {
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[item];
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);

            foreach (Excel.ChartObject xlChart in xlCharts)
            {
                Excel.Chart chartPage = xlChart.Chart;
                //chartPage.SetBackgroundPicture(@"c:\temp\background.png");
                chartPage.Export(Path.Combine(folder, $"{chartPage.Name}.{format}"), format, _misValue);
            }
        }

        public void ExportChartsAsCombined(string item, string imageFile, int imagesPerRow, int splitBoardWidth)
        {
            if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(imageFile))
                throw new ArgumentNullException($"The parameter item or imageFile is null");

            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[item];
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);

            IList<string> chartFiles = new List<string>();
            string folder = Path.GetDirectoryName(imageFile);
            string format = Path.GetExtension(imageFile).TrimStart('.');
            foreach (Excel.ChartObject xlChart in xlCharts)
            {
                Excel.Chart chartPage = xlChart.Chart;
                var chartFile = Path.Combine(folder, $"{chartPage.Name}.{format}");
                chartFiles.Add(chartFile);
                chartPage.Export(chartFile, format, _misValue);
            }

            var chartImage = chartFiles.FirstOrDefault();
            if (chartImage == null) throw new ArgumentNullException($"No chart found");

            var rows = (int) Math.Ceiling(chartFiles.Count / (decimal) imagesPerRow);

            var imageBitmap = new Bitmap(chartImage);

            var combinedImageWidth = imageBitmap.Width * imagesPerRow + (imagesPerRow - 1) * splitBoardWidth;
            var combinedImageHeight = imageBitmap.Height * rows + (imagesPerRow - 1) * splitBoardWidth;
            var combinedImage = new Bitmap(combinedImageWidth, combinedImageHeight);

            // copy images row by row
            int combinedImageY = 0;
            for (int row = 0; row < rows; row++)
            {
                // copy images one after one in one row
                for (int colomn = 0; colomn < imagesPerRow; colomn++)
                {
                    int imageIndex = colomn + row * imagesPerRow;
                    var image = new Bitmap(chartFiles[imageIndex]);
                    var combinedImageX = (imageBitmap.Width + splitBoardWidth) * colomn;
                    for (int y = 0; y < image.Height; y++)
                    {
                        // copy image
                        for (int x = 0; x < image.Width; x++)
                        {
                            combinedImage.SetPixel(combinedImageX + x, combinedImageY + y, image.GetPixel(x, y));
                        }
                        
                        // draw vertical boarder
                        for (int i = 0; i < splitBoardWidth; i++)
                        {
                            var boarderX = combinedImageX + imageBitmap.Width + i;
                            if (boarderX < combinedImageWidth)
                            {
                                combinedImage.SetPixel(boarderX, combinedImageY + y, Color.Black);
                            }
                        }
                    }
                }
                combinedImageY += imageBitmap.Height;
                // draw horizon boarder
                {
                    if (combinedImageY < combinedImageHeight)
                    {
                        for (int x = 0; x < combinedImageWidth; x++)
                        {
                            combinedImage.SetPixel(x, combinedImageY, Color.Black);
                        }
                        combinedImageY++;
                    }
                }
            }
            combinedImage.Save(imageFile);
        }

        public void ExportChartsAsOneImage(string item, string imageFile, int imagesPerRow, int splitBoardWidth,
            Color splitBoarderColor)
        {
            try
            {
                if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(imageFile))
                    throw new ArgumentNullException($"The parameter item or imageFile is null");

                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[item];
                Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);

                IList<string> chartFiles = new List<string>();
                string folder = Path.GetDirectoryName(imageFile);
                if (folder == null) throw new ArgumentNullException($"Image file should be specified the full path");
                string format = Path.GetExtension(imageFile).TrimStart('.');

                for (int j = 1; j <= xlCharts.Count; j++)
                {
                    var chartFile = Path.Combine(folder, $"Temp Chart {j}.{format}");
                    Excel.ChartObject chart = (Excel.ChartObject)xlCharts.Item(j);
                    chart.Activate();
                    chart.Chart.Export(chartFile, format, true);
                    chartFiles.Add(chartFile);
                }

                var chartImage = chartFiles.FirstOrDefault();
                if (chartImage == null) throw new ArgumentNullException($"No chart found");

                var rows = (int)Math.Ceiling(chartFiles.Count / (decimal)imagesPerRow);

                var imageBitmap = new Bitmap(chartImage);

                var combinedImageWidth = imageBitmap.Width * imagesPerRow + (imagesPerRow - 1) * splitBoardWidth;
                var combinedImageHeight = imageBitmap.Height * rows + (imagesPerRow - 1) * splitBoardWidth;
                var combinedImage = new Bitmap(combinedImageWidth, combinedImageHeight);

                // copy images row by row
                int combinedImageY = 0;
                for (int row = 0; row < rows; row++)
                {
                    // copy images one after one in one row
                    for (int colomn = 0; colomn < imagesPerRow; colomn++)
                    {
                        int imageIndex = colomn + row * imagesPerRow;
                        var image = new Bitmap(chartFiles[imageIndex]);
                        var combinedImageX = (imageBitmap.Width + splitBoardWidth) * colomn;
                        for (int y = 0; y < image.Height; y++)
                        {
                            // copy image
                            for (int x = 0; x < image.Width; x++)
                            {
                                combinedImage.SetPixel(combinedImageX + x, combinedImageY + y, image.GetPixel(x, y));
                            }

                            // draw vertical boarder
                            for (int i = 0; i < splitBoardWidth; i++)
                            {
                                var boarderX = combinedImageX + imageBitmap.Width + i;
                                if (boarderX < combinedImageWidth)
                                {
                                    combinedImage.SetPixel(boarderX, combinedImageY + y, splitBoarderColor);
                                }
                            }
                        }
                    }
                    combinedImageY += imageBitmap.Height;
                    // draw horizon boarder
                    if (combinedImageY < combinedImageHeight)
                    {
                        for (int x = 0; x < combinedImageWidth; x++)
                        {
                            combinedImage.SetPixel(x, combinedImageY, splitBoarderColor);
                        }
                        combinedImageY++;
                    }
                }
                combinedImage.Save(imageFile);
            }
            finally
            {
                _xlWorkBook.Close(true, _misValue, _misValue);
            }
        }

        public void Dispose()
        {
            _xlWorkBook.Close(true, _misValue, _misValue);
        }
    }
}
