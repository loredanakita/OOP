using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class Ellipse : Shape
    {
        private int width;
        private int height;
        private Pen pen;
        private int x;
        private int y;

        public Ellipse(int width, int height, int x, int y, Pen pen)
        {
            this.width = width;
            this.height = height;
            this.pen = pen;
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphics g)
        {
            float new_x = x - width / 2f;
            float new_y = y - height / 2f;

            g.DrawEllipse(pen, new_x, new_y, width, height);
        }
    }

}
