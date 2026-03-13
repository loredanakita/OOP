using System;
using System.Collections.Generic;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Factory for creating shapes without switch/if statements
    /// New shapes can be added by registration without modifying existing code
    /// </summary>
    class ShapeFactory
    {
        private delegate Shape Creator(Dictionary<string, object> parameters, Pen pen);
        private static Dictionary<string, Creator> _creators = new Dictionary<string, Creator>();

        /// <summary>
        /// Register a new shape type with its creator function
        /// </summary>
        public static void RegisterShape(string shapeType, Func<Dictionary<string, object>, Pen, Shape> creator)
        {
            _creators[shapeType] = new Creator(creator);
        }

        /// <summary>
        /// Create a shape by type
        /// </summary>
        public static Shape CreateShape(string shapeType, Dictionary<string, object> parameters, Pen pen)
        {
            if (_creators.ContainsKey(shapeType))
            {
                return _creators[shapeType](parameters, pen);
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }

        /// <summary>
        /// Get list of available shape types
        /// </summary>
        public static List<string> GetAvailableShapes()
        {
            return new List<string>(_creators.Keys);
        }
    }

    /// <summary>
    /// Registration class - call this to register all shapes
    /// </summary>
    static class ShapeRegistration
    {
        public static void RegisterAllShapes()
        {
            // Register Circle
            ShapeFactory.RegisterShape("Circle", (parameters, pen) =>
            {
                return new Circle(
                    Convert.ToInt32(parameters["Radius"]),
                    Convert.ToInt32(parameters["X"]),
                    Convert.ToInt32(parameters["Y"]),
                    pen
                );
            });

            // Register Rectangle
            ShapeFactory.RegisterShape("Rectangle", (parameters, pen) =>
            {
                return new Rectangle(
                    Convert.ToInt32(parameters["Width"]),
                    Convert.ToInt32(parameters["Height"]),
                    Convert.ToInt32(parameters["X"]),
                    Convert.ToInt32(parameters["Y"]),
                    pen
                );
            });

            // Register Square (special case of Rectangle)
            ShapeFactory.RegisterShape("Square", (parameters, pen) =>
            {
                int side = Convert.ToInt32(parameters["Side"]);
                return new Rectangle(
                    side,
                    side,
                    Convert.ToInt32(parameters["X"]),
                    Convert.ToInt32(parameters["Y"]),
                    pen
                );
            });

            // Register Triangle
            ShapeFactory.RegisterShape("Triangle", (parameters, pen) =>
            {
                return new Triangle(
                    Convert.ToInt32(parameters["Side"]),
                    Convert.ToInt32(parameters["X"]),
                    Convert.ToInt32(parameters["Y"]),
                    pen
                );
            });

            // Register Line
            ShapeFactory.RegisterShape("Line", (parameters, pen) =>
            {
                return new Line(
                    Convert.ToInt32(parameters["X1"]),
                    Convert.ToInt32(parameters["Y1"]),
                    Convert.ToInt32(parameters["X2"]),
                    Convert.ToInt32(parameters["Y2"]),
                    pen
                );
            });

            // Register Ellipse
            ShapeFactory.RegisterShape("Ellipse", (parameters, pen) =>
            {
                return new Ellipse(
                    Convert.ToInt32(parameters["Width"]),
                    Convert.ToInt32(parameters["Height"]),
                    Convert.ToInt32(parameters["X"]),
                    Convert.ToInt32(parameters["Y"]),
                    pen
                );
            });

            // To add a new shape like Pentagon, just add registration here
            // No existing code needs to be modified!
        }
    }
}