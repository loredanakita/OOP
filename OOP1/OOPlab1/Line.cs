// Line.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Line shape - contains only data properties
    /// </summary>
    class Line : Shape
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public Line(int x1, int y1, int x2, int y2, Pen pen)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Pen = pen;
        }

        public override ShapeData GetShapeData()
        {
            return new ShapeData
            {
                ShapeType = "Line",
                Pen = this.Pen,
                Parameters = new Dictionary<string, object>
                {
                    { "X1", X1 },
                    { "Y1", Y1 },
                    { "X2", X2 },
                    { "Y2", Y2 }
                }
            };
        }
    }
}