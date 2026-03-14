using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPlab1
{
    class ShapeList
    {
        private List<Shape> list = new List<Shape>();
        public void Add(Shape shape)
        {
            list.Add(shape);
        }
        public void Draw(Graphics g)
        {
            foreach (var item in list)
            {
                item.Draw(g);
            }
        }
    }
}
