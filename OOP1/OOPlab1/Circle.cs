// Circle.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Circle shape - contains only data properties
    /// </summary>
    class Circle : Shape
    {
        public int Radius { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Circle(int radius, int x, int y, Pen pen)
        {
            Radius = radius;
            X = x;
            Y = y;
            Pen = pen;
        }

        public override ShapeData GetShapeData()
        {
            return new ShapeData
            {
                ShapeType = "Circle",
                Pen = this.Pen,
                Parameters = new Dictionary<string, object>
                {
                    { "X", X },
                    { "Y", Y },
                    { "Radius", Radius }
                }
            };
        }
    }
}