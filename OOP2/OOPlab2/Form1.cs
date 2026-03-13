using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OOPlab1
{
    // Main form; handles user input and triggers shape creation and rendering
    public partial class Form1 : Form
    {
        private List<IShapeFactory> factories = new List<IShapeFactory>(); // one factory per shape type
        private ShapeList shapeList = new ShapeList(); // all shapes drawn so far

        public Form1()
        {
            InitializeComponent();

            // Order must match comboBox1 items: Square, Circle, Ellipse, Line, Rectangle, Triangle
            factories.Add(new SquareFactory());
            factories.Add(new CircleFactory());
            factories.Add(new EllipseFactory());
            factories.Add(new LineFactory());
            factories.Add(new RectangleFactory());
            factories.Add(new TriangleFactory());
        }

        // Opens a color picker and updates the preview panel with the chosen color
        private void buttonColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.Color = colorPanel.BackColor;
                if (dlg.ShowDialog() == DialogResult.OK)
                    colorPanel.BackColor = dlg.Color;
            }
        }

        // Reads user input, creates the selected shape, and triggers a repaint
        private void button1_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index < 0) return;

            // Validate all numeric inputs before proceeding
            if (!int.TryParse(textX.Text, out int x) ||
                !int.TryParse(textY.Text, out int y) ||
                !int.TryParse(textSize.Text, out int size) || size <= 0)
            {
                MessageBox.Show("Please enter valid numbers for X, Y and size.");
                return;
            }

            Color color = colorPanel.BackColor; // use the color from the preview panel

            Shape shape = factories[index].Create(x, y, color, size);
            shapeList.Add(shape);

            Invalidate(); // trigger Paint event to redraw all shapes
        }


        // Redraws all shapes on every Paint event
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            shapeList.Draw(e.Graphics);
        }
    }
}
