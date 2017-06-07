using Microsoft.VisualStudio.TestTools.UnitTesting;
using MergeImage;

namespace UnitTest
{
    [TestClass]
    [DeploymentItem("Samples", "Samples")]
    [DeploymentItem("Settings.json")]
    public class MergeImageTests
    {
        private const string from = @"Samples\DHA.png";
        private const string to = @"Samples\ColorMap.png";

        [TestMethod]
        public void TestProgram()
        {
            Program.Main(new string[] {from, to});
        }

        [TestMethod]
        public void TestOverwrite()
        {
            var mergeAll = MergePictureHelpers.MergeImages(from, to);
            mergeAll.Save();
        }

        [TestMethod]
        public void TestOverwriteAll()
        {
            var mergeAll = MergePictureHelpers.MergeAllImages(from, to);
            mergeAll.Save();
        }
    }
}
