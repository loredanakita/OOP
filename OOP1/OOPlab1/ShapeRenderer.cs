// ShapeRenderer.cs
using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Responsible for drawing shapes - separates rendering from shape data
    /// </summary>
    class ShapeRenderer
    {
        public void DrawShape(Graphics g, Shape shape)
        {
            ShapeData data = shape.GetShapeData();

            switch (data.ShapeType)
            {
                case "Circle":
                    DrawCircle(g, data);
                    break;
                case "Rectangle":
                    DrawRectangle(g, data);
                    break;
                case "Square":
                    DrawSquare(g, data);
                    break;
                case "Triangle":
                    DrawTriangle(g, data);
                    break;
                case "Line":
                    DrawLine(g, data);
                    break;
                case "Ellipse":
                    DrawEllipse(g, data);
                    break;
                default:
                    throw new NotSupportedException($"Shape type {data.ShapeType} not supported");
            }
        }

        private void DrawCircle(Graphics g, ShapeData data)
        {
            int x = (int)data.Parameters["X"];
            int y = (int)data.Parameters["Y"];
            int radius = (int)data.Parameters["Radius"];

            float new_x = x - radius;
            float new_y = y - radius;
            float diameter = 2 * radius;
            g.DrawEllipse(data.Pen, new_x, new_y, diameter, diameter);
        }

        private void DrawRectangle(Graphics g, ShapeData data)
        {
            int x = (int)data.Parameters["X"];
            int y = (int)data.Parameters["Y"];
            int width = (int)data.Parameters["Width"];
            int height = (int)data.Parameters["Height"];

            g.DrawRectangle(data.Pen, x, y, width, height);
        }

        private void DrawSquare(Graphics g, ShapeData data)
        {
            int x = (int)data.Parameters["X"];
            int y = (int)data.Parameters["Y"];
            int side = (int)data.Parameters["Side"];

            g.DrawRectangle(data.Pen, x, y, side, side);
        }

        private void DrawTriangle(Graphics g, ShapeData data)
        {
            int side = (int)data.Parameters["Side"];
            int x = (int)data.Parameters["X"];
            int y = (int)data.Parameters["Y"];

            float height = (float)(Math.Sqrt(3) / 2 * side);
            PointF top = new PointF(x, y - 2 / 3f * height);
            PointF left = new PointF(x - side / 2, y + 1 / 3f * height);
            PointF right = new PointF(x + side / 2, y + 1 / 3f * height);
            PointF[] points = new PointF[] { top, left, right };
            g.DrawPolygon(data.Pen, points);
        }

        private void DrawLine(Graphics g, ShapeData data)
        {
            int x1 = (int)data.Parameters["X1"];
            int y1 = (int)data.Parameters["Y1"];
            int x2 = (int)data.Parameters["X2"];
            int y2 = (int)data.Parameters["Y2"];

            g.DrawLine(data.Pen, x1, y1, x2, y2);
        }

        private void DrawEllipse(Graphics g, ShapeData data)
        {
            int x = (int)data.Parameters["X"];
            int y = (int)data.Parameters["Y"];
            int width = (int)data.Parameters["Width"];
            int height = (int)data.Parameters["Height"];

            // For ellipse, x and y are center coordinates
            float new_x = x - width / 2f;
            float new_y = y - height / 2f;

            g.DrawEllipse(data.Pen, new_x, new_y, width, height);
        }
    }
}