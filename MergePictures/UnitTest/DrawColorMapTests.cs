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
            int width = 500;
            int height = 500;
            var g = new GradientExample(width, height);
            Color from = Color.FromArgb(255, 255, 0, 0);   // Opaque red
            Color to = Color.FromArgb(255, 0, 255, 0);  // Opaque green

            int x = 0;
            for (int y = 0; y < height; y++)
            {
                g.DrawSolid(new Point(0, y), new Point(x, y), Color.Red);
                g.DrawGradient(new Point(x, y), new Point(width - 1, y), Color.Red, Color.Green);
                x++;
                if (x > width) break;
            }

            g.Save(file);
        }
    }
}
