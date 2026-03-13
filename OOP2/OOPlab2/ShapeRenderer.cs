using System;
using System.Drawing;

namespace OOPlab1
{
    // Handles all drawing logic; separated from shape data classes
    class ShapeRenderer
    {
        // Draws a shape onto the given graphics surface based on its runtime type
        public void Draw(Shape shape, Graphics g)
        {
            if (shape is Circle c)
            {
                float diameter = 2 * c.Radius; // diameter = 2r
                g.DrawEllipse(c.Pen, c.X, c.Y, diameter, diameter);
            }
            else if (shape is Ellipse e)
            {
                // Offset so that (X, Y) is the center, not the top-left corner
                float new_x = e.X - e.Width / 2f;
                float new_y = e.Y - e.Height / 2f;
                g.DrawEllipse(e.Pen, new_x, new_y, e.Width, e.Height);
            }
            else if (shape is Square sq)
            {
                g.DrawRectangle(sq.Pen, sq.X, sq.Y, sq.Side, sq.Side);
            }
            else if (shape is Rectangle r)
            {
                g.DrawRectangle(r.Pen, r.X, r.Y, r.Width, r.Height);
            }
            else if (shape is Triangle t)
            {
                // Compute vertices of an equilateral triangle centered at (X, Y)
                float height = (float)(Math.Sqrt(3) / 2 * t.Side);
                PointF top = new PointF(t.X, t.Y - 2 / 3f * height);
                PointF left = new PointF(t.X - t.Side / 2, t.Y + 1 / 3f * height);
                PointF right = new PointF(t.X + t.Side / 2, t.Y + 1 / 3f * height);
                g.DrawPolygon(t.Pen, new PointF[] { top, left, right });
            }
            else if (shape is Line l)
            {
                g.DrawLine(l.Pen, l.X1, l.Y1, l.X2, l.Y2);
            }
        }
    }
}
