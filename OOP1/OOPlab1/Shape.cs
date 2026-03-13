using System;
using System.Drawing;

namespace OOPlab1
{
    /// <summary>
    /// Base class for all shapes - contains only data, no drawing methods
    /// </summary>
    abstract class Shape
    {
        // Properties common to all shapes
        public Pen Pen { get; set; }
        
        // Method to get shape data for rendering
        public abstract ShapeData GetShapeData();
    }
    
    /// <summary>
    /// Container for shape data to be used by renderer
    /// </summary>
    public class ShapeData
    {
        public string ShapeType { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        public Pen Pen { get; set; }
    }
}