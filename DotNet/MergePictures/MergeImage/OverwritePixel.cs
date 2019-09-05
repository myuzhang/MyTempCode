using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;

namespace MergeImage
{
    internal class OverwritePixel
    {
        private readonly string _fromImageFile;
        private readonly string _toImageFile;
        private readonly Settings _settings;

        public OverwritePixel(string fromImageFile, string toImageFile)
        {
            _toImageFile = toImageFile;
            _fromImageFile = fromImageFile;

            if (!File.Exists(_fromImageFile))
                throw new ArgumentException($"from image file {_fromImageFile} doesn't exist");
            if (!File.Exists(_toImageFile))
                throw new ArgumentException($"from image file {_toImageFile} doesn't exist");

            using (StreamReader r = new StreamReader("Settings.json"))
            {
                string json = r.ReadToEnd();
                _settings = JsonConvert.DeserializeObject<Settings>(json);
            }
        }

        public Bitmap OverwriteWithPlotBackground()
        {
            var from = new Bitmap(_fromImageFile);
            var to = GetScaledColorImage(_toImageFile);

            var newFrom = Overwrite(from, to);
            to = GetScaledColorImage(_toImageFile);
            return RefillColorToOutOfPlot(newFrom, to);
        }

        private Bitmap RefillColorToOutOfPlot(Bitmap from, Bitmap to)
        {
            for (int x = _settings.BorderWidthInPixel; x < from.Width - _settings.BorderWidthInPixel; x++)
            {
                for (int y = _settings.BorderWidthInPixel; y < from.Height - _settings.BorderWidthInPixel; y++)
                {
                    var p = from.GetPixel(x, y);
                    if (p.Name.Equals(_settings.PlotBorderColorName) ||
                        p.R <= _settings.TolorencePlotBorderR && p.G <= _settings.TolorencePlotBorderG && p.B <= _settings.TolorencePlotBorderB)
                        break;

                    if (p.Name.Equals(_settings.BackgroundColorName) ||
                        p.R >= _settings.TolorenceBackgroundR && p.G >= _settings.TolorenceBackgroundG && p.B >= _settings.TolorenceBackgroundB)
                        from.SetPixel(x, y, to.GetPixel(x, y));
                }
            }
            return from;
        }

        private Bitmap Overwrite(Bitmap from, Bitmap to)
        {
            int blockPixel = (from.Width - _settings.BorderWidthInPixel * (_settings.BlockNumber - 1)) / _settings.BlockNumber;
            for (int y = _settings.BorderWidthInPixel; y < from.Height - _settings.BorderWidthInPixel; y++)
            {
                for (int blockNum = 0; blockNum < _settings.BlockNumber; blockNum++)
                {
                    int validStart;
                    int validEnd;
                    int leftBorder = blockPixel * blockNum + _settings.BorderWidthInPixel * blockNum;
                    int rightBorder = leftBorder + blockPixel - 1;
                    if (GetValidHorizontalColorRange(from, y, leftBorder, rightBorder, out validStart, out validEnd))
                        OverwriteHorizon(from, to, y, validStart, validEnd);

                    OverwriteHorizon(from, to, y, rightBorder, rightBorder + _settings.BorderWidthInPixel * 2, false);
                }
            }
            return to;
        }

        private bool GetValidHorizontalColorRange(
            Bitmap source,
            int imageY,
            int leftBorder,
            int rightBorder,
            out int validStart,
            out int validEnd)
        {
            validStart = 0;
            validEnd = 0;

            bool found = false;

            if (leftBorder > rightBorder)
                return false;

            // avoid rim color
            leftBorder++;
            rightBorder--;

            for (int x = leftBorder; x <= rightBorder; x++)
            {
                var p = source.GetPixel(x, imageY);
                if (!p.Name.Equals(_settings.BackgroundColorName))
                {
                    if (p.R >= _settings.TolorenceBackgroundR && p.G >= _settings.TolorenceBackgroundG && p.B >= _settings.TolorenceBackgroundB)
                    {
                        continue;
                    }
                    validStart = x;
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;

            for (int x = rightBorder; x >= leftBorder; x--)
            {
                var p = source.GetPixel(x, imageY);
                if (!p.Name.Equals(_settings.BackgroundColorName))
                {
                    if (p.R >= _settings.TolorenceBackgroundR && p.G >= _settings.TolorenceBackgroundG && p.B >= _settings.TolorenceBackgroundB)
                    {
                        continue;
                    }
                    validEnd = x;
                    break;
                }
            }
            return true;
        }

        private void OverwriteHorizon(Bitmap from, Bitmap to, int y, int startX, int endX, bool withBackground = true)
        {
            if (startX < 0 || endX < 0)
                return;

            if (startX >= from.Width)
                startX = from.Width - 1;

            if (endX >= from.Width)
                endX = from.Width - 1;

            if (withBackground)
            {
                for (int x = startX; x <= endX; x++)
                    to.SetPixel(x, y, from.GetPixel(x, y));
                return;
            }

            for (int x = startX; x <= endX; x++)
            {
                if (!from.GetPixel(x, y).Name.Equals(_settings.BackgroundColorName))
                    to.SetPixel(x, y, from.GetPixel(x, y));
            }
        }

        // FIXME: can only scale color map
        private Bitmap GetScaledColorImage(string imageFile)
        {
            var from = new Bitmap(_fromImageFile);
            var to = new Bitmap(_toImageFile);

            var returnImage = new Bitmap(imageFile);

            if (from.Width != to.Width || from.Height != to.Height)
            {
                if (!_settings.NeedScale)
                    throw new ArgumentException("Merging pictures don't have the same size");

                return new Bitmap(returnImage, new Size(from.Width, from.Height));
            }

            return returnImage;
        }
    }
}
