using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MergePictures
{
    public static class MergePictureHelpers
    {
        public static Bitmap MergeImages(string fromImage, string toImage)
        {
            var overwrite = new OverwritePixel(fromImage, toImage);
            return overwrite.Overwrite();
        }

        public static Bitmap MergeAllImages(string fromImage, string toImage)
        {
            var overwrite = new OverwritePixel(fromImage, toImage);
            return overwrite.OverwriteAll();
        }

        public static void SaveAs(this Bitmap source, string fileName,  ImageFormat format) =>
            source.Save(fileName, format);

        // todo: format can be set from app settings
        public static void Save(this Bitmap source) =>
            SaveAs(source, DateTime.UtcNow.Ticks.ToString(), ImageFormat.Jpeg);
    }
}
