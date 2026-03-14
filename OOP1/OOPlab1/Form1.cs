using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPlab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ShapeList list = new ShapeList();
            list.Add(new Square(side:150, x:10, y:10, new Pen(Color.Red, 3)));          
            list.Add(new Triangle(side:170, x:270, y:100, new Pen(Color.Purple, 3)));   
            list.Add(new Circle(radius:80, x:380, y:10, new Pen(Color.Orange, 3)));      
            list.Add(new Rectangle(width:150, height:250, x:10, y:180, new Pen(Color.Green, 3))); 
            list.Add(new Ellipse(width:150, height:300, x:270, y:330, new Pen(Color.Blue, 3)));  
            list.Add(new Line(x1:370, y1:270, x2:680, y2:390, new Pen(Color.Indigo, 3)));  
            list.Draw(e.Graphics);
        }
    }
}
