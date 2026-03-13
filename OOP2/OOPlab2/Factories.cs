using System.Drawing;

namespace OOPlab1
{
    // Contract for all shape factories
    interface IShapeFactory
    {
        string Name { get; } // display name shown in the UI combo box
        Shape Create(int x, int y, Color color, int size); // creates a shape with user-supplied parameters
    }

    // Creates Square instances; size is the side length
    class SquareFactory : IShapeFactory
    {
        public string Name => "Square";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Square(size, x, y, new Pen(color, 3));
        }
    }

    // Creates Circle instances; size is the radius
    class CircleFactory : IShapeFactory
    {
        public string Name => "Circle";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Circle(size, x, y, new Pen(color, 3));
        }
    }

    // Creates Ellipse instances; size is the width, height is set to half of size
    class EllipseFactory : IShapeFactory
    {
        public string Name => "Ellipse";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Ellipse(size, size / 2, x, y, new Pen(color, 3));
        }
    }

    // Creates Line instances; size is the length along both axes
    class LineFactory : IShapeFactory
    {
        public string Name => "Line";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Line(x, y, x + size, y + size, new Pen(color, 3));
        }
    }

    // Creates Rectangle instances; size is the width, height is set to half of size
    class RectangleFactory : IShapeFactory
    {
        public string Name => "Rectangle";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Rectangle(size, size / 2, x, y, new Pen(color, 3));
        }
    }

    // Creates Triangle instances; size is the side length
    class TriangleFactory : IShapeFactory
    {
        public string Name => "Triangle";

        public Shape Create(int x, int y, Color color, int size)
        {
            return new Triangle(size, x, y, new Pen(color, 3));
        }
    }
}
