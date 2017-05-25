using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace Common
{
    public static class ImageUtilities
    {
        // Replace the resection with null color
        // The source bit map should be full screen picture
        public static Bitmap ResectBitmap(this Bitmap source, Rect resection)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                int heightY = (int) (resection.Y + resection.Height);
                int widthX = (int)(resection.X + resection.Width);
                for (var y = (int) resection.Y; y < heightY; y++)
                {
                    for (var x = (int) resection.X; x < widthX; x++)
                    {
                        source.SetPixel(x, y, Color.White);
                    }
                }
                return source;
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(e.Message);
            }
        }

        // Set source bit map to null color out of resection
        // The source bit map should be full screen picture
        public static Bitmap RangBitmap(this Bitmap source, Rect resection)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                for (var y = 0; y < source.Height; y++)
                {
                    for (var x = 0; x < source.Width; x++)
                    {
                        if (x >= resection.Left && x <= resection.Right && y >= resection.Top && y <= resection.Bottom)
                        {
                            continue; // in the range and do nothing
                        }
                        source.SetPixel(x, y, Color.White);
                    }
                }
                return source;
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(e.Message);
            }
        }
        
        public static bool CompareScreenShot(this Bitmap firstImage, Bitmap secondImage)
        {
            MemoryStream ms = new MemoryStream();
            firstImage.Save(ms, ImageFormat.Png);
            var firstBitmap = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;

            secondImage.Save(ms, ImageFormat.Png);
            var secondBitmap = Convert.ToBase64String(ms.ToArray());

            return firstBitmap.Equals(secondBitmap);
        }

        public static bool CompareScreenShotByPixel(this Bitmap firstImage, Bitmap secondImage)
        {
            if (firstImage == null)
                throw new ArgumentNullException(nameof(firstImage));

            if (firstImage.Width != secondImage.Width ||
                firstImage.Height != secondImage.Height)
                return false;

            for (int y = 0; y < (int) firstImage.Height; y++)
            {
                for (int x = 0; x < (int) firstImage.Width; x++)
                {
                    var first = firstImage.GetPixel(x, y);
                    var second = secondImage.GetPixel(x, y);
                    if (first != second)
                        return false;
                }
            }

            return true;
        }

        public static bool CompareScreenShot(this Bitmap actualImage, Bitmap expectedImage, Rect rang, IList<Rect> resections)
        {
            if (actualImage == null)
                throw new ArgumentNullException(nameof(actualImage));
            
            // make the expectedImage as same size as the actualImage
            expectedImage = expectedImage.CutVerticalBitmapAs(actualImage, PixelFormat.Format32bppRgb, false);

            actualImage = actualImage.RangBitmap(rang);
            expectedImage = expectedImage.RangBitmap(rang);

            foreach (Rect rect in resections)
            {
                actualImage = actualImage.ResectBitmap(rect);
                expectedImage = expectedImage.ResectBitmap(rect);
            }

            actualImage.Save(@"c:\Temp\actual.png", ImageFormat.Png);
            expectedImage.Save(@"c:\Temp\expected.png", ImageFormat.Png);

            return actualImage.CompareScreenShotByPixel(expectedImage);
            //return actualImage.CompareScreenShot(expectedImage);
        }

        public static ImageFormat ToImageFormat(this string format)
        {
            switch (format.ToLower())
            {
                case "jpeg": return ImageFormat.Jpeg;
                default: // png
                    return ImageFormat.Png;
            }
        }

        public static Bitmap CutVerticalBitmapAs(
            this Bitmap source,
            Bitmap toExpected,
            PixelFormat format,
            bool fromLeft = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Width <= toExpected.Width)
                return source;

            Rectangle rect;
            if (fromLeft)
            {
                rect = new Rectangle(0, 0, toExpected.Width, toExpected.Height);
            }
            else
            {
                var offset = source.Width - toExpected.Width;
                rect = new Rectangle(offset, 0, toExpected.Width, toExpected.Height);
            }

            return source.Clone(rect, format);
        }
    }
}
