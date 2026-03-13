using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    // Stores all created shapes and renders them using ShapeRenderer
    class ShapeList
    {
        private List<Shape> list = new List<Shape>(); // collection of all added shapes
        private ShapeRenderer renderer = new ShapeRenderer(); // used to draw each shape

        // Adds a shape to the collection
        public void Add(Shape shape)
        {
            list.Add(shape);
        }

        // Draws all shapes onto the given graphics surface
        public void Draw(Graphics g)
        {
            foreach (var item in list)
            {
                renderer.Draw(item, g);
            }
        }
    }
}
