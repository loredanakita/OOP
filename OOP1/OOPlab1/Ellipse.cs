// Ellipse.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Ellipse shape - contains only data properties, no drawing methods
    /// </summary>
    class Ellipse : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Ellipse(int width, int height, int x, int y, Pen pen)
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
                ShapeType = "Ellipse",
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