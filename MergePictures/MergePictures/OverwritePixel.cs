using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MergePictures
{
    public class OverwritePixel
    {
        private readonly string _fromImageFile;
        private readonly string _toImageFile;

        public OverwritePixel(string fromImageFile, string toImageFile)
        {
            _toImageFile = toImageFile;
            _fromImageFile = fromImageFile;

            if (!File.Exists(_fromImageFile))
                throw new ArgumentException($"from image file {_fromImageFile} doesn't exist");
            if (!File.Exists(_toImageFile))
                throw new ArgumentException($"from image file {_toImageFile} doesn't exist");
        }

        public int BlockNumber =>
            Int32.Parse(ConfigurationManager.AppSettings["blockNumber"]);

        public int BorderWidthInPixel =>
            Int32.Parse(ConfigurationManager.AppSettings["borderWidthInPixel"]);

        public string BackgroundName =>
            ConfigurationManager.AppSettings["backgroundName"];

        public bool NeedScale =>
            bool.Parse(ConfigurationManager.AppSettings["needScale"]);

        public Bitmap OverwriteWithoutBackground()
        {
            var from = new Bitmap(_fromImageFile);
            var to = new Bitmap(_toImageFile);

            if (from.Width != to.Width || from.Height != to.Height)
            {
                if (!NeedScale)
                    throw new ArgumentException("Merging pictures don't have the same size");

                int minWidth = Math.Min(from.Width, to.Width);
                int minHeight = Math.Min(from.Height, to.Height);

                from = new Bitmap(from, new Size(minWidth, minHeight));
                to = new Bitmap(to, new Size(minWidth, minHeight));

                from.Save("from.jpg", ImageFormat.Jpeg);
                to.Save("to.jpg", ImageFormat.Jpeg);
            }                         

            for (int y = 0; y < from.Height; y++)
            {
                for (int x = 0; x < from.Height; x++)
                {
                    if (!from.GetPixel(x, y).Name.Equals(BackgroundName))
                        to.SetPixel(x, y, from.GetPixel(x, y));
                }
            }

            return to;
        }

        public Bitmap OverwriteAll()
        {
            var from = new Bitmap(_fromImageFile);
            var to = new Bitmap(_toImageFile);

            if (from.Width != to.Width || from.Height != to.Height)
            {
                if (!NeedScale)
                    throw new ArgumentException("Merging pictures don't have the same size");

                int minWidth = Math.Min(from.Width, to.Width);
                int minHeight = Math.Min(from.Height, to.Height);

                from = new Bitmap(from, new Size(minWidth, minHeight));
                to = new Bitmap(to, new Size(minWidth, minHeight));
            }

            int blockPixel = from.Width/BlockNumber;

            for (int y = BorderWidthInPixel; y < from.Height - BorderWidthInPixel; y++)
            {
                for (int blockNum = 0; blockNum < BlockNumber; blockNum++)
                {
                    int validStart;
                    int validEnd;
                    int leftBorder = blockPixel*blockNum + BorderWidthInPixel;
                    int rightBorder = blockPixel*(blockNum + 1) - BorderWidthInPixel;

                    if (GetValidColorRange(from, y, leftBorder, rightBorder, out validStart, out validEnd))
                        Overwrite(from, to, y, validStart, validEnd);

                    Overwrite(from, to, y, rightBorder, rightBorder + BorderWidthInPixel*2, false);
                }
            }

            return to;
        }

        private bool GetValidColorRange(
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

            for (int x = leftBorder; x <= rightBorder; x++)
            {
                var p = source.GetPixel(x, imageY);
                if (!p.Name.Equals(BackgroundName))
                {
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
                if (!p.Name.Equals(BackgroundName))
                {
                    validEnd = x;
                    break;
                }
            }
            return true;
        }

        private void Overwrite(Bitmap from, Bitmap to, int y, int startX, int endX, bool withBackground = true)
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
                if (!from.GetPixel(x, y).Name.Equals(BackgroundName))
                    to.SetPixel(x, y, from.GetPixel(x, y));
            }
        }
    }
}
