using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ColorMapGeneration;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UnitTest
{
    [TestClass]
    public class DrawColorMapTests
    {
        [TestMethod]
        public void GradientColorTest()
        {
            string file = @"c:\temp\gradation.jpg";
            int width = 1000;
            int height = 1000;
            int mid = 600;
            var g = new GradientExample(width, height);
            //Color from = Color.FromArgb(255, 255, 0, 0);   // Opaque red
            //Color to = Color.FromArgb(255, 0, 255, 0);  // Opaque green
            Color from = Color.LightGreen;
            Color to = Color.OrangeRed;

            for (int y = 0; y < height; y++)
            {
                g.DrawSolid(new Point(0, y), new Point(mid, y), from);
                g.DrawGradient(new Point(mid, y), new Point(mid + 200, y), from, to);
                g.DrawSolid(new Point(mid + 200, y), new Point(width, y), to);
            }

            g.Save(file);
        }
    }
}
