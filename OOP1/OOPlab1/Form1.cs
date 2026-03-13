using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOPlab1
{
    public partial class Form1 : Form
    {
        private ShapeList shapeList = new ShapeList();
        private Button btnCreateShape;
        private Button btnClear;
        private Color currentColor = Color.Black;
        private int penWidth = 2;

        public Form1()
        {
            InitializeComponent();
            SetupUI();

            // Register all available shapes
            ShapeRegistration.RegisterAllShapes();

            // Add some initial shapes
            AddInitialShapes();
        }

        private void SetupUI()
        {
            this.Text = "Simple Graphics Editor";
            this.Size = new Size(900, 700);
            this.DoubleBuffered = true;

            // Create toolbar
            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.LightGray
            };

            btnCreateShape = new Button
            {
                Text = "Create Shape",
                Location = new Point(10, 8),
                Size = new Size(100, 25)
            };
            btnCreateShape.Click += BtnCreateShape_Click;

            btnClear = new Button
            {
                Text = "Clear All",
                Location = new Point(120, 8),
                Size = new Size(100, 25)
            };
            btnClear.Click += BtnClear_Click;

            // Color selection
            Label lblColor = new Label
            {
                Text = "Color:",
                Location = new Point(240, 12),
                Size = new Size(40, 20)
            };

            ComboBox cmbColor = new ComboBox
            {
                Location = new Point(280, 8),
                Size = new Size(80, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbColor.Items.AddRange(new object[] { "Black", "Red", "Green", "Blue", "Orange", "Purple" });
            cmbColor.SelectedIndex = 0;
            cmbColor.SelectedIndexChanged += (s, e) =>
            {
                switch (cmbColor.SelectedItem.ToString())
                {
                    case "Red": currentColor = Color.Red; break;
                    case "Green": currentColor = Color.Green; break;
                    case "Blue": currentColor = Color.Blue; break;
                    case "Orange": currentColor = Color.Orange; break;
                    case "Purple": currentColor = Color.Purple; break;
                    default: currentColor = Color.Black; break;
                }
            };

            // Pen width
            Label lblWidth = new Label
            {
                Text = "Width:",
                Location = new Point(380, 12),
                Size = new Size(40, 20)
            };

            NumericUpDown nudWidth = new NumericUpDown
            {
                Location = new Point(420, 8),
                Size = new Size(50, 25),
                Minimum = 1,
                Maximum = 10,
                Value = 2
            };
            nudWidth.ValueChanged += (s, e) => penWidth = (int)nudWidth.Value;

            toolbar.Controls.AddRange(new Control[] {
                btnCreateShape, btnClear, lblColor, cmbColor, lblWidth, nudWidth
            });

            this.Controls.Add(toolbar);
        }

        private void AddInitialShapes()
        {
            shapeList.Add(new Square(side: 150, x: 10, y: 60, new Pen(Color.Red, 3)));
            shapeList.Add(new Triangle(side: 170, x: 270, y: 150, new Pen(Color.Purple, 3)));
            shapeList.Add(new Circle(radius: 80, x: 380, y: 60, new Pen(Color.Orange, 3)));
            shapeList.Add(new Rectangle(width: 150, height: 250, x: 10, y: 230, new Pen(Color.Green, 3)));
            shapeList.Add(new Ellipse(width: 150, height: 300, x: 270, y: 380, new Pen(Color.Blue, 3)));
            shapeList.Add(new Line(x1: 370, y1: 320, x2: 680, y2: 440, new Pen(Color.Indigo, 3)));
        }

        private void BtnCreateShape_Click(object sender, EventArgs e)
        {
            using (var dialog = new ShapeInputDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string shapeType = dialog.GetSelectedShapeType();
                    var parameters = dialog.GetParameters();

                    if (!string.IsNullOrEmpty(shapeType))
                    {
                        try
                        {
                            Pen pen = new Pen(currentColor, penWidth);
                            Shape newShape = ShapeFactory.CreateShape(shapeType, parameters, pen);
                            shapeList.Add(newShape);
                            this.Invalidate(); // Redraw
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error creating shape: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            shapeList = new ShapeList();
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            shapeList.Draw(e.Graphics);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}