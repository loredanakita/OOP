namespace OOPlab1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            textX = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textY = new TextBox();
            label3 = new Label();
            textSize = new TextBox();
            button1 = new Button();
            buttonColor = new Button();
            colorPanel = new Panel();
            label4 = new Label();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Square", "Circle", "Ellipse", "Line", "Rectangle", "Triangle" });
            comboBox1.Location = new Point(80, 29);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 33);
            comboBox1.TabIndex = 0;
            // 
            // textX
            // 
            textX.Location = new Point(384, 11);
            textX.Name = "textX";
            textX.Size = new Size(105, 31);
            textX.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(307, 14);
            label1.Name = "label1";
            label1.Size = new Size(72, 25);
            label1.TabIndex = 2;
            label1.Text = "Enter X:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(308, 50);
            label2.Name = "label2";
            label2.Size = new Size(71, 25);
            label2.TabIndex = 3;
            label2.Text = "Enter Y:";
            // 
            // textY
            // 
            textY.Location = new Point(384, 50);
            textY.Name = "textY";
            textY.Size = new Size(105, 31);
            textY.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(531, 11);
            label3.Name = "label3";
            label3.Size = new Size(47, 25);
            label3.TabIndex = 5;
            label3.Text = "Size:";
            // 
            // textSize
            // 
            textSize.Location = new Point(609, 11);
            textSize.Name = "textSize";
            textSize.Size = new Size(104, 31);
            textSize.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(794, 21);
            button1.Name = "button1";
            button1.Size = new Size(112, 47);
            button1.TabIndex = 7;
            button1.Text = "Create";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // buttonColor
            // 
            buttonColor.Location = new Point(609, 47);
            buttonColor.Name = "buttonColor";
            buttonColor.Size = new Size(104, 34);
            buttonColor.TabIndex = 8;
            buttonColor.Text = "Color:";
            buttonColor.UseVisualStyleBackColor = true;
            buttonColor.Click += buttonColor_Click;
            // 
            // colorPanel
            // 
            colorPanel.BackColor = SystemColors.ControlDark;
            colorPanel.ForeColor = Color.OrangeRed;
            colorPanel.Location = new Point(531, 50);
            colorPanel.Name = "colorPanel";
            colorPanel.Size = new Size(51, 32);
            colorPanel.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 29);
            label4.Name = "label4";
            label4.Size = new Size(65, 25);
            label4.TabIndex = 10;
            label4.Text = "Shape:";
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightCyan;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(colorPanel);
            panel1.Controls.Add(buttonColor);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(textX);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textSize);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textY);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(4, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1010, 89);
            panel1.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1014, 570);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Мини-редактор";
            Paint += Form1_Paint;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox1;
        private TextBox textX;
        private Label label1;
        private Label label2;
        private TextBox textY;
        private Label label3;
        private TextBox textSize;
        private Button button1;
        private Button buttonColor;
        private Panel colorPanel;
        private Label label4;
        private Panel panel1;
    }
}
