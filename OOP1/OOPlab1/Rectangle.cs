using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Rectangle : Shape
    {
        private int width;
        private int height;
        private Pen pen;
        private int x;
        private int y;

        public Rectangle(int width, int height, int x, int y, Pen pen)
        {
            this.width = width;
            this.height = height;
            this.pen = pen;
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }
    }
}
