using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace SharedService
{
    public class ExcelChart
    {
        private readonly Excel.Application _xlApp;

        private Excel.Workbook _xlWorkBook;

        private readonly object _misValue;

        public ExcelChart(string fileName)
        {
            _xlApp = new Excel.Application();

            if (_xlApp == null)
                throw new DriveNotFoundException("Excel is not properly installed!!");

            _xlWorkBook = _xlApp.Workbooks.Open(fileName);

            _misValue = System.Reflection.Missing.Value;
        }

        public IList<string> TempCreatedFiles { get; } = new List<string>();

        public void ExportCharts(string item, string folder, string format)
        {
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)_xlWorkBook.Worksheets.Item[item];
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);

            foreach (Excel.ChartObject xlChart in xlCharts)
            {
                Excel.Chart chartPage = xlChart.Chart;
                chartPage.Export(Path.Combine(folder, $"{chartPage.Name}.{format}"), format, _misValue);
            }
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
                    TempCreatedFiles.Add(chartFile);
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
                    for (int i = 0; i < splitBoardWidth; i++)
                    {
                        if (combinedImageY < combinedImageHeight)
                        {
                            for (int x = 0; x < combinedImageWidth; x++)
                            {
                                combinedImage.SetPixel(x, combinedImageY, splitBoarderColor);
                            }
                            combinedImageY++;
                        }
                    }
                }
                combinedImage.Save(imageFile);
            }
            finally
            {
                _xlWorkBook.Close(true, _misValue, _misValue);
            }
        }
    }
}
