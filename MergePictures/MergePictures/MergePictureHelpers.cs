using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace MergePictures
{
    public static class MergePictureHelpers
    {
        public static string NewImageFile =>
            $@"{ConfigurationManager.AppSettings["filePath"]}\{DateTime.UtcNow.Ticks.ToString()}.jpg";

        public static Bitmap MergeImages(string fromImage, string toImage)
        {
            var overwrite = new OverwritePixel(fromImage, toImage);
            return overwrite.OverwriteWithoutPlotBackground();
        }

        public static Bitmap MergeAllImages(string fromImage, string toImage)
        {
            var overwrite = new OverwritePixel(fromImage, toImage);
            return overwrite.OverwriteWithPlotBackground();
        }

        public static void SaveAs(this Bitmap source, string fileName,  ImageFormat format) =>
            source.Save(fileName, format);

        // todo: format can be set from app settings
        public static void Save(this Bitmap source) =>
            SaveAs(source, NewImageFile, ImageFormat.Jpeg);
    }
}
