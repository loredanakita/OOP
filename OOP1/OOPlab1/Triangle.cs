// Triangle.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Triangle shape - contains only data properties
    /// </summary>
    class Triangle : Shape
    {
        public int Side { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Triangle(int side, int x, int y, Pen pen)
        {
            Side = side;
            X = x;
            Y = y;
            Pen = pen;
        }

        public override ShapeData GetShapeData()
        {
            return new ShapeData
            {
                ShapeType = "Triangle",
                Pen = this.Pen,
                Parameters = new Dictionary<string, object>
                {
                    { "X", X },
                    { "Y", Y },
                    { "Side", Side }
                }
            };
        }
    }
}