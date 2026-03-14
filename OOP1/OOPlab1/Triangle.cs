using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Triangle : Shape
    {
        private int side;
        private Pen pen;
        private int x;
        private int y;
        public Triangle(int side, int x, int y, Pen pen)
        {
            this.pen = pen;
            this.side = side;
            this.x = x;
            this.y = y;
        }
        public override void Draw(Graphics g)
        {
            float height = (float)(Math.Sqrt(3) / 2 * side);
            PointF top = new PointF(x, y - 2 / 3f * height);
            PointF left = new PointF(x - side / 2, y + 1 / 3f * height);
            PointF right = new PointF(x + side / 2, y + 1 / 3f * height);
            PointF[] points = new PointF[] { top, left, right };
            g.DrawPolygon(pen, points);
        }
    }
}
