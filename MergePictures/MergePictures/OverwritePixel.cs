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

            var newFrom = Overwrite(from, to, true);
            to = new Bitmap(_toImageFile);
            return Overwrite(newFrom, to, false);
        }

        //public Bitmap ReverseOverwriteVertical(Bitmap from, Bitmap to)
        //{
        //    int blockPixel = from.Height / BlockNumber;
        //    for (int x = BorderWidthInPixel; x < from.Width - BorderWidthInPixel; x++)
        //    {
        //        for (int blockNum = 0; blockNum < BlockNumber; blockNum++)
        //        {
        //            int validStart;
        //            int validEnd;
        //            int topBorder = blockPixel * blockNum + BorderWidthInPixel;
        //            int bottomtBorder = blockPixel * (blockNum + 1) - BorderWidthInPixel;

        //            if (GetValidVerticalColorRange(from, x, topBorder, bottomtBorder, out validStart, out validEnd))
        //                OverwriteVertical(from, to, x, validStart, validEnd);

        //            OverwriteVertical(from, to, x, bottomtBorder, bottomtBorder + BorderWidthInPixel * 2, false);
        //        }
        //    }
        //}

        public Bitmap Overwrite(Bitmap from, Bitmap to, bool isByHorizon)
        {
            if (isByHorizon)
            {
                int blockPixel = from.Width / BlockNumber;
                for (int y = BorderWidthInPixel; y < from.Height - BorderWidthInPixel; y++)
                {
                    for (int blockNum = 0; blockNum < BlockNumber; blockNum++)
                    {
                        int validStart;
                        int validEnd;
                        int leftBorder = blockPixel * blockNum + BorderWidthInPixel;
                        int rightBorder = blockPixel * (blockNum + 1) - BorderWidthInPixel;

                        if (GetValidHorizontalColorRange(from, y, leftBorder, rightBorder, out validStart, out validEnd))
                            OverwriteHorizon(from, to, y, validStart, validEnd);

                        OverwriteHorizon(from, to, y, rightBorder, rightBorder + BorderWidthInPixel * 2, false);
                    }
                }
            }
            else
            {
                int blockPixel = from.Height / BlockNumber;
                for (int x = BorderWidthInPixel; x < from.Width - BorderWidthInPixel; x++)
                {
                    for (int blockNum = 0; blockNum < BlockNumber; blockNum++)
                    {
                        int validStart;
                        int validEnd;
                        int topBorder = blockPixel * blockNum + BorderWidthInPixel;
                        int bottomtBorder = blockPixel * (blockNum + 1) - BorderWidthInPixel;

                        if (GetValidVerticalColorRange(from, x, topBorder, bottomtBorder, out validStart, out validEnd))
                            OverwriteVertical(from, to, x, validStart, validEnd);

                        OverwriteVertical(from, to, x, bottomtBorder, bottomtBorder + BorderWidthInPixel * 2, false);
                    }
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
                if (!from.GetPixel(x, y).Name.Equals(BackgroundName))
                    to.SetPixel(x, y, from.GetPixel(x, y));
            }
        }

        private bool GetValidVerticalColorRange(
            Bitmap source,
            int imageX,
            int topBorder,
            int bottomBorder,
            out int validStart,
            out int validEnd)
        {
            validStart = 0;
            validEnd = 0;

            bool found = false;

            if (topBorder > bottomBorder)
                return false;

            for (int y = topBorder; y <= bottomBorder; y++)
            {
                var p = source.GetPixel(imageX, y);
                if (!p.Name.Equals(BackgroundName))
                {
                    validStart = y;
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;

            for (int y = bottomBorder; y >= topBorder; y--)
            {
                var p = source.GetPixel(imageX, y);
                if (!p.Name.Equals(BackgroundName))
                {
                    validEnd = y;
                    break;
                }
            }
            return true;
        }

        private void OverwriteVertical(Bitmap from, Bitmap to, int x, int startY, int endY, bool withBackground = true)
        {
            if (startY < 0 || endY < 0)
                return;

            if (startY >= from.Height)
                startY = from.Height - 1;

            if (endY >= from.Height)
                endY = from.Height - 1;

            if (withBackground)
            {
                for (int y = startY; y <= endY; y++)
                    to.SetPixel(x, y, from.GetPixel(x, y));
                return;
            }

            for (int y = startY; y <= endY; y++)
            {
                if (!from.GetPixel(x, y).Name.Equals(BackgroundName))
                    to.SetPixel(x, y, from.GetPixel(x, y));
            }
        }
    }
}
