// Square.cs
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Square shape - special case of Rectangle
    /// </summary>
    class Square : Rectangle
    {
        public Square(int side, int x, int y, Pen pen)
            : base(side, side, x, y, pen)
        {
        }

        public override ShapeData GetShapeData()
        {
            return new ShapeData
            {
                ShapeType = "Square",
                Pen = this.Pen,
                Parameters = new Dictionary<string, object>
                {
                    { "X", X },
                    { "Y", Y },
                    { "Side", Width } // Width equals Height equals Side
                }
            };
        }
    }
}