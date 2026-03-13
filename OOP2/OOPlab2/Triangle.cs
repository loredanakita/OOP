using System.Drawing;

namespace OOPlab1
{
    // Stores data for an equilateral triangle shape
    class Triangle : Shape
    {
        public int Side; // side length in pixels
        public Pen Pen;  // outline pen
        public int X;    // center X
        public int Y;    // center Y

        public Triangle(int side, int x, int y, Pen pen)
        {
            Side = side;
            X = x;
            Y = y;
            Pen = pen;
        }
    }
}
