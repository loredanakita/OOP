using System.Drawing;

namespace OOPlab1
{
    // Stores data for a circle shape
    class Circle : Shape
    {
        public int Radius; // radius in pixels
        public Pen Pen;    // outline pen
        public int X;      // top-left X of bounding box
        public int Y;      // top-left Y of bounding box

        public Circle(int radius, int x, int y, Pen pen)
        {
            Radius = radius;
            X = x;
            Y = y;
            Pen = pen;
        }
    }
}
