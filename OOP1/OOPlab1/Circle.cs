using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Circle : Shape
    {
        private int radius;
        private Pen pen;
        private int x;
        private int y;
        public Circle(int radius, int x, int y, Pen pen)
        {
            this.pen = pen;
            this.radius = radius;
            this.y = y;
            this.x = x;
        }
        public override void Draw(Graphics g)
        {
            float new_x = x - radius;
            float new_y = y - radius;
            float diameter = 2 * radius;
            g.DrawEllipse(pen, x, y, diameter, diameter);
        }
    }
}
