using System;
using System.Drawing.Imaging;

namespace MergeImage
{
    class Program
    {
        internal static void Main(string[] args)
        {
            if (args.Length.Equals(2) || args.Length.Equals(3))
            {

                Console.Write($"Merge {args[0]} to color map {args[1]}");

                //var merge = MergePictureHelpers.MergeImages(args[0], args[1]);
                //merge.Save();

                var mergeAll = MergePictureHelpers.MergeAllImages(args[0], args[1]);
                if (args.Length.Equals(3))
                {
                    mergeAll.SaveAs(args[2], ImageFormat.Jpeg);
                    Console.WriteLine($" with name {args[2]}");
                }
                else
                {
                    var fileName = mergeAll.Save();
                    Console.WriteLine($" with name {fileName}");
                }
            }
        }
    }
}
