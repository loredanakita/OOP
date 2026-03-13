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
            btnOK = new Button();
            btnCancel = new Button();
            panelParams = new Panel();
            panelParams.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new Point(15, 245);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(112, 34);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(133, 245);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // panelParams
            // 
            panelParams.Controls.Add(btnOK);
            panelParams.Controls.Add(btnCancel);
            panelParams.Location = new Point(269, 89);
            panelParams.Name = "panelParams";
            panelParams.Size = new Size(262, 296);
            panelParams.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelParams);
            Name = "Form1";
            Text = "Form1";
            Paint += Form1_Paint;
            panelParams.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private Panel panelParams;
    }
}
