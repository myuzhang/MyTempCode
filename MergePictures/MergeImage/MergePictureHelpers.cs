using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MergeImage
{
    public static class MergePictureHelpers
    {
        public static string NewImageFile
        {
            get
            {
                using (StreamReader r = new StreamReader("Settings.json"))
                {
                    string json = r.ReadToEnd();
                    var settings = JsonConvert.DeserializeObject<Settings>(json);
                    var fileName = $"{DateTime.Now.ToString("ddMMMyyyy_HHmmss")}.jpg";
                    return Path.Combine(settings.MergedFileDefaultPath, fileName);
                }
            }
        }           

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

        public static void SaveAs(this Bitmap source, string fileName, ImageFormat format)
        {
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            source.Save(fileName, format);
        }            

        // todo: format can be set from app settings
        public static string Save(this Bitmap source)
        {
            var imageFile = NewImageFile;
            SaveAs(source, imageFile, ImageFormat.Jpeg);
            return imageFile;
        }            
    }
}
