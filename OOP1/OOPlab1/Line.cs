using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Line : Shape
    {
        private int x1, y1;
        private int x2, y2;
        private Pen pen;

        public Line(int x1, int y1, int x2, int y2, Pen pen)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.pen = pen;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
    }

}
