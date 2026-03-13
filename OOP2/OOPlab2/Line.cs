using System.Drawing;

namespace OOPlab1
{
    // Stores data for a line segment
    class Line : Shape
    {
        public int X1, Y1; // start point
        public int X2, Y2; // end point
        public Pen Pen;     // outline pen

        public Line(int x1, int y1, int x2, int y2, Pen pen)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Pen = pen;
        }
    }
}
