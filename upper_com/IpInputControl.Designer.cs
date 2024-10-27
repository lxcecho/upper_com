namespace upper_com
{
    partial class IpInputControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // textBox1
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.MaxLength = 3;
            this.textBox1.Size = new System.Drawing.Size(30, 20);

            // textBox2
            this.textBox2.Location = new System.Drawing.Point(49, 3);
            this.textBox2.MaxLength = 3;
            this.textBox2.Size = new System.Drawing.Size(30, 20);

            // textBox3
            this.textBox3.Location = new System.Drawing.Point(95, 3);
            this.textBox3.MaxLength = 3;
            this.textBox3.Size = new System.Drawing.Size(30, 20);

            // textBox4
            this.textBox4.Location = new System.Drawing.Point(141, 3);
            this.textBox4.MaxLength = 3;
            this.textBox4.Size = new System.Drawing.Size(30, 20);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 6);
            this.label1.Text = ".";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 6);
            this.label2.Text = ".";

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 6);
            this.label3.Text = ".";

            // IpInputControl
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox4);
            this.Size = new System.Drawing.Size(180, 30);
        }

        #endregion
    }
}
