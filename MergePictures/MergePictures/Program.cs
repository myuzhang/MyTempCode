using System;

namespace MergePictures
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{args[0]},{args[1]}");
            var newFile = Console.ReadLine();
            if (args.Length.Equals(2))
            {
                var merge = MergePictureHelpers.MergeImages(args[0], args[1]);
                var mergeAll = MergePictureHelpers.MergeAllImages(args[0], args[1]);
                merge.Save();
                mergeAll.Save();
            }
        }
    }
}
