using System.Drawing;

namespace OOPlab1
{
    // Stores data for a square shape
    class Square : Shape
    {
        public int Side; // side length in pixels
        public Pen Pen;  // outline pen
        public int X;    // top-left X
        public int Y;    // top-left Y

        public Square(int side, int x, int y, Pen pen)
        {
            Side = side;
            X = x;
            Y = y;
            Pen = pen;
        }
    }
}
