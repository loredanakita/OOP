using System.Drawing;

namespace OOPlab1
{
    // Stores data for a rectangle shape
    class Rectangle : Shape
    {
        public int Width;  // width in pixels
        public int Height; // height in pixels
        public Pen Pen;    // outline pen
        public int X;      // top-left X
        public int Y;      // top-left Y

        public Rectangle(int width, int height, int x, int y, Pen pen)
        {
            Width = width;
            Height = height;
            Pen = pen;
            X = x;
            Y = y;
        }
    }
}
