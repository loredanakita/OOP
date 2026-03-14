using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Square : Shape
    {
        private int side;
        private Pen pen;
        private int x;
        private int y;
        public Square(int side, int x, int y, Pen pen)
        {
            this.pen = pen;
            this.side = side;
            this.x = x;
            this.y = y;
        }
        public override void Draw(Graphics g)
        {
            g.DrawRectangle(pen, x, y, side, side);
        }
    }
}
