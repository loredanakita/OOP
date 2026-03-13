using System.Drawing;

namespace OOPlab1
{
    // Stores data for an ellipse shape
    class Ellipse : Shape
    {
        public int Width;  // total width in pixels
        public int Height; // total height in pixels
        public Pen Pen;    // outline pen
        public int X;      // center X
        public int Y;      // center Y

        public Ellipse(int width, int height, int x, int y, Pen pen)
        {
            Width = width;
            Height = height;
            Pen = pen;
            X = x;
            Y = y;
        }
    }
}
