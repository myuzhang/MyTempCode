using System;
using System.Configuration;
using System.Drawing;
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

        public Bitmap Overwrite()
        {
            var from = new Bitmap(_fromImageFile);
            var to = new Bitmap(_toImageFile);

            if (from.Width != to.Width || from.Height != to.Height)
                throw new ArgumentException("Merging pictures don't have the same size");

            for (int y = 0; y < from.Height; y++)
            {
                for (int x = 0; x < from.Height; x++)
                {
                    if (!from.GetPixel(x, y).Equals(Color.White))
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
                throw new ArgumentException("Merging pictures don't have the same size");

            int blockPixel = from.Width/BlockNumber;

            for (int y = 0; y < from.Height; y++)
            {
                for (int blockNum = 0; blockNum < BlockNumber; blockNum++)
                {
                    int validStart;
                    int validEnd;
                    int leftBorder = blockPixel*blockNum + BorderWidthInPixel;
                    int rightBorder = blockPixel*(blockNum + 1) - BorderWidthInPixel;

                    if (GetValidColorRange(from, y, leftBorder, rightBorder, out validStart, out validEnd))
                        Overwrite(from, to, y, validStart, validEnd);
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

            if (leftBorder > rightBorder)
                return false;

            for (int x = leftBorder; x <= rightBorder; x++)
            {
                var p = source.GetPixel(x, imageY);
                if (!p.Equals(Color.White))
                {
                    validStart = x;
                    break;
                }
            }
            if (validStart == 0)
                return false;

            for (int x = rightBorder; x <= leftBorder; x--)
            {
                var p = source.GetPixel(x, imageY);
                if (!p.Equals(Color.White))
                {
                    validEnd = x;
                    break;
                }
            }
            return true;
        }

        private void Overwrite(Bitmap from, Bitmap to, int imageY, int startX, int endX)
        {
            if (startX < 0 || endX < 0)
                return;

            for (int x = startX; x <= endX; x++)
                to.SetPixel(x, imageY, from.GetPixel(x, imageY));
        }
    }
}
