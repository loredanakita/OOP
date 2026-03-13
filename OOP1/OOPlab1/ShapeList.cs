using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Collection of shapes with rendering capability
    /// </summary>
    class ShapeList
    {
        private List<Shape> list = new List<Shape>();
        private ShapeRenderer renderer = new ShapeRenderer();

        public void Add(Shape shape)
        {
            list.Add(shape);
        }

        public void Draw(Graphics g)
        {
            foreach (var shape in list)
            {
                renderer.DrawShape(g, shape);
            }
        }

        public List<Shape> GetAllShapes()
        {
            return list;
        }
    }
}