using System.Drawing;

namespace upper_com
{
    partial class DataDetection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.myLED1 = new upper_com.MyLED();
            this.btnEnd = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.n_value = new System.Windows.Forms.TextBox();
            this.k_value = new System.Windows.Forms.TextBox();
            this.nLabel = new System.Windows.Forms.Label();
            this.kLabel = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.serialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothCur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationCur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 729);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(3, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.myLED1);
            this.splitContainer1.Panel1.Controls.Add(this.btnEnd);
            this.splitContainer1.Panel1.Controls.Add(this.startBtn);
            this.splitContainer1.Panel1.Controls.Add(this.n_value);
            this.splitContainer1.Panel1.Controls.Add(this.k_value);
            this.splitContainer1.Panel1.Controls.Add(this.nLabel);
            this.splitContainer1.Panel1.Controls.Add(this.kLabel);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(1674, 729);
            this.splitContainer1.SplitterDistance = 195;
            this.splitContainer1.TabIndex = 6;
            // 
            // myLED1
            // 
            this.myLED1.BorderWidth = 5;
            this.myLED1.CenterColor = System.Drawing.Color.White;
            this.myLED1.FlashInterval = 500;
            this.myLED1.GapWidth = 5;
            this.myLED1.IsBorder = true;
            this.myLED1.IsFlash = true;
            this.myLED1.IsHighLight = true;
            this.myLED1.LampColor = new System.Drawing.Color[] {
        System.Drawing.Color.Green,
        System.Drawing.Color.Gray};
            this.myLED1.LedColor = System.Drawing.Color.Green;
            this.myLED1.LedFalseColor = System.Drawing.Color.Red;
            this.myLED1.LedStatus = true;
            this.myLED1.LedTrueColor = System.Drawing.Color.Green;
            this.myLED1.Location = new System.Drawing.Point(24, 48);
            this.myLED1.Name = "myLED1";
            this.myLED1.Size = new System.Drawing.Size(79, 85);
            this.myLED1.TabIndex = 13;
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.Red;
            this.btnEnd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEnd.Font = new System.Drawing.Font("SimSun", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEnd.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btnEnd.Location = new System.Drawing.Point(956, 29);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(103, 42);
            this.btnEnd.TabIndex = 12;
            this.btnEnd.Text = "停止采集";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEndClick);
            // 
            // startBtn
            // 
            this.startBtn.BackColor = System.Drawing.Color.Green;
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startBtn.Font = new System.Drawing.Font("SimSun", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startBtn.ForeColor = System.Drawing.SystemColors.Highlight;
            this.startBtn.Location = new System.Drawing.Point(956, 91);
            this.startBtn.Margin = new System.Windows.Forms.Padding(4);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(103, 42);
            this.startBtn.TabIndex = 4;
            this.startBtn.Text = "开始采集";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.btnStartClick);
            // 
            // n_value
            // 
            this.n_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.n_value.Location = new System.Drawing.Point(256, 79);
            this.n_value.Margin = new System.Windows.Forms.Padding(4);
            this.n_value.Name = "n_value";
            this.n_value.Size = new System.Drawing.Size(52, 18);
            this.n_value.TabIndex = 3;
            this.n_value.Text = "20";
            // 
            // k_value
            // 
            this.k_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.k_value.Location = new System.Drawing.Point(256, 29);
            this.k_value.Margin = new System.Windows.Forms.Padding(4);
            this.k_value.Name = "k_value";
            this.k_value.Size = new System.Drawing.Size(52, 18);
            this.k_value.TabIndex = 2;
            this.k_value.Text = "3";
            this.k_value.TextChanged += new System.EventHandler(this.k_value_TextChanged);
            // 
            // nLabel
            // 
            this.nLabel.AutoSize = true;
            this.nLabel.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nLabel.Location = new System.Drawing.Point(166, 82);
            this.nLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nLabel.Name = "nLabel";
            this.nLabel.Size = new System.Drawing.Size(53, 18);
            this.nLabel.TabIndex = 1;
            this.nLabel.Text = "N值：";
            // 
            // kLabel
            // 
            this.kLabel.AutoSize = true;
            this.kLabel.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kLabel.Location = new System.Drawing.Point(166, 32);
            this.kLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.kLabel.Name = "kLabel";
            this.kLabel.Size = new System.Drawing.Size(53, 18);
            this.kLabel.TabIndex = 0;
            this.kLabel.Text = "k值：";
            this.kLabel.Click += new System.EventHandler(this.k_label_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialNo,
            this.curDate,
            this.smoothCur,
            this.smoothAverage,
            this.smoothUpper,
            this.smoothLower,
            this.mutationCur,
            this.mutationAverage,
            this.mutationUpper,
            this.mutationLower});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1674, 530);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // serialNo
            // 
            this.serialNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serialNo.HeaderText = "序号";
            this.serialNo.MinimumWidth = 6;
            this.serialNo.Name = "serialNo";
            // 
            // curDate
            // 
            this.curDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.curDate.HeaderText = "时间";
            this.curDate.MinimumWidth = 6;
            this.curDate.Name = "curDate";
            // 
            // smoothCur
            // 
            this.smoothCur.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.smoothCur.HeaderText = "电流I1";
            this.smoothCur.MinimumWidth = 6;
            this.smoothCur.Name = "smoothCur";
            // 
            // smoothAverage
            // 
            this.smoothAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.smoothAverage.HeaderText = "电流I1均值";
            this.smoothAverage.MinimumWidth = 6;
            this.smoothAverage.Name = "smoothAverage";
            // 
            // smoothUpper
            // 
            this.smoothUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.smoothUpper.HeaderText = "电流I1上限";
            this.smoothUpper.MinimumWidth = 6;
            this.smoothUpper.Name = "smoothUpper";
            // 
            // smoothLower
            // 
            this.smoothLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.smoothLower.HeaderText = "电流I1下限";
            this.smoothLower.MinimumWidth = 6;
            this.smoothLower.Name = "smoothLower";
            // 
            // mutationCur
            // 
            this.mutationCur.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mutationCur.HeaderText = "电流I2";
            this.mutationCur.MinimumWidth = 6;
            this.mutationCur.Name = "mutationCur";
            // 
            // mutationAverage
            // 
            this.mutationAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mutationAverage.HeaderText = "电流I2均值";
            this.mutationAverage.MinimumWidth = 6;
            this.mutationAverage.Name = "mutationAverage";
            // 
            // mutationUpper
            // 
            this.mutationUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mutationUpper.HeaderText = "电流I2上限";
            this.mutationUpper.MinimumWidth = 6;
            this.mutationUpper.Name = "mutationUpper";
            // 
            // mutationLower
            // 
            this.mutationLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mutationLower.HeaderText = "电流I2下限";
            this.mutationLower.MinimumWidth = 6;
            this.mutationLower.Name = "mutationLower";
            // 
            // DataDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1677, 729);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DataDetection";
            this.Text = "数据监控";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label nLabel;
        private System.Windows.Forms.Label kLabel;
        private System.Windows.Forms.TextBox n_value;
        private System.Windows.Forms.TextBox k_value;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button startBtn;
        private MyLED myLED1;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn curDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothCur;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationCur;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationLower;
    }
}

