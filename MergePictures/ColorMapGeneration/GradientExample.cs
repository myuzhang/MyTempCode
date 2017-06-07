using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorMapGeneration
{
    public class GradientExample
    {
        // refer to: https://msdn.microsoft.com/en-us/library/0sdy66e6(v=vs.110).aspx

        Bitmap _myBitmap;
        Graphics _g;

        public GradientExample(int width, int height)
        {
            _myBitmap = new Bitmap(width, height);
            _g = Graphics.FromImage(_myBitmap);
        }

        public void DrawGradient(Point startPoint, Point endPoint, Color fromColor, Color toColor)
        {
            if (startPoint.Equals(endPoint)) return;
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
                startPoint,
                endPoint,
                fromColor,
                toColor);

            DrawLine(startPoint, endPoint, linGrBrush);
        }
        
        public void DrawSolid(Point startPoint, Point endPoint, Color color)
        {
            if (startPoint.Equals(endPoint)) return;
            SolidBrush solidBrush = new SolidBrush(color);
            DrawLine(startPoint, endPoint, solidBrush);
        }

        public void Save(string fileName)
        {
            _myBitmap.Save(fileName, ImageFormat.Jpeg);
        }

        private void DrawLine(Point startPoint, Point endPoint, Brush brush)
        {
            Pen pen = new Pen(brush);

            //for (int y = startPoint.Y; y < endPoint.Y; y++)
                _g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }
    }
}
