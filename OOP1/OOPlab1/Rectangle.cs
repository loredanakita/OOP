// Rectangle.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Rectangle shape - contains only data properties
    /// </summary>
    class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Rectangle(int width, int height, int x, int y, Pen pen)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
            Pen = pen;
        }

        public override ShapeData GetShapeData()
        {
            return new ShapeData
            {
                ShapeType = "Rectangle",
                Pen = this.Pen,
                Parameters = new Dictionary<string, object>
                {
                    { "X", X },
                    { "Y", Y },
                    { "Width", Width },
                    { "Height", Height }
                }
            };
        }
    }
}