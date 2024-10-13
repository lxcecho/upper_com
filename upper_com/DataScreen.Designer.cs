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
            this.btnEnd = new System.Windows.Forms.Button();
            this.portValue = new System.Windows.Forms.TextBox();
            this.ipValue = new System.Windows.Forms.TextBox();
            this.portName = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.Label();
            this.warningMsg = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.n_value = new System.Windows.Forms.TextBox();
            this.k_value = new System.Windows.Forms.TextBox();
            this.nLabel = new System.Windows.Forms.Label();
            this.kLabel = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.serialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curVal01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curValAverage01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upperLimit01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowerLimit01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curVal02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curValAverage02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upperLimit02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowerLimit02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 729);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnEnd);
            this.splitContainer1.Panel1.Controls.Add(this.portValue);
            this.splitContainer1.Panel1.Controls.Add(this.ipValue);
            this.splitContainer1.Panel1.Controls.Add(this.portName);
            this.splitContainer1.Panel1.Controls.Add(this.ipAddress);
            this.splitContainer1.Panel1.Controls.Add(this.warningMsg);
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
            this.splitContainer1.Size = new System.Drawing.Size(1650, 729);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 6;
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.Red;
            this.btnEnd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEnd.Font = new System.Drawing.Font("SimSun", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEnd.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btnEnd.Location = new System.Drawing.Point(620, 96);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(103, 42);
            this.btnEnd.TabIndex = 12;
            this.btnEnd.Text = "停止采集";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEndClick);
            // 
            // portValue
            // 
            this.portValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.portValue.Location = new System.Drawing.Point(147, 105);
            this.portValue.Margin = new System.Windows.Forms.Padding(4);
            this.portValue.Name = "portValue";
            this.portValue.Size = new System.Drawing.Size(102, 25);
            this.portValue.TabIndex = 11;
            this.portValue.Text = "65533";
            // 
            // ipValue
            // 
            this.ipValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipValue.Location = new System.Drawing.Point(147, 55);
            this.ipValue.Margin = new System.Windows.Forms.Padding(4);
            this.ipValue.Name = "ipValue";
            this.ipValue.Size = new System.Drawing.Size(102, 25);
            this.ipValue.TabIndex = 10;
            this.ipValue.Text = "192.168.204.1";
            this.ipValue.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // portName
            // 
            this.portName.AutoSize = true;
            this.portName.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.portName.Location = new System.Drawing.Point(49, 108);
            this.portName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.portName.Name = "portName";
            this.portName.Size = new System.Drawing.Size(80, 18);
            this.portName.TabIndex = 9;
            this.portName.Text = "端  口：";
            // 
            // ipAddress
            // 
            this.ipAddress.AutoSize = true;
            this.ipAddress.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ipAddress.Location = new System.Drawing.Point(49, 58);
            this.ipAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(80, 18);
            this.ipAddress.TabIndex = 8;
            this.ipAddress.Text = "IP地址：";
            this.ipAddress.Click += new System.EventHandler(this.label4_Click);
            // 
            // warningMsg
            // 
            this.warningMsg.AutoSize = true;
            this.warningMsg.Font = new System.Drawing.Font("SimSun", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.warningMsg.Location = new System.Drawing.Point(1007, 74);
            this.warningMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningMsg.Name = "warningMsg";
            this.warningMsg.Size = new System.Drawing.Size(140, 23);
            this.warningMsg.TabIndex = 5;
            this.warningMsg.Text = "WARNING...";
            this.warningMsg.Click += new System.EventHandler(this.label1_Click_2);
            // 
            // startBtn
            // 
            this.startBtn.BackColor = System.Drawing.Color.Green;
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startBtn.Font = new System.Drawing.Font("SimSun", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startBtn.ForeColor = System.Drawing.SystemColors.Highlight;
            this.startBtn.Location = new System.Drawing.Point(620, 32);
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
            this.n_value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.n_value.Location = new System.Drawing.Point(441, 105);
            this.n_value.Margin = new System.Windows.Forms.Padding(4);
            this.n_value.Name = "n_value";
            this.n_value.Size = new System.Drawing.Size(102, 25);
            this.n_value.TabIndex = 3;
            this.n_value.Text = "20";
            // 
            // k_value
            // 
            this.k_value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.k_value.Location = new System.Drawing.Point(441, 55);
            this.k_value.Margin = new System.Windows.Forms.Padding(4);
            this.k_value.Name = "k_value";
            this.k_value.Size = new System.Drawing.Size(102, 25);
            this.k_value.TabIndex = 2;
            this.k_value.Text = "3";
            this.k_value.TextChanged += new System.EventHandler(this.k_value_TextChanged);
            // 
            // nLabel
            // 
            this.nLabel.AutoSize = true;
            this.nLabel.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nLabel.Location = new System.Drawing.Point(351, 108);
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
            this.kLabel.Location = new System.Drawing.Point(351, 58);
            this.kLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.kLabel.Name = "kLabel";
            this.kLabel.Size = new System.Drawing.Size(53, 18);
            this.kLabel.TabIndex = 0;
            this.kLabel.Text = "k值：";
            this.kLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialNo,
            this.time,
            this.curVal01,
            this.curValAverage01,
            this.upperLimit01,
            this.lowerLimit01,
            this.curVal02,
            this.curValAverage02,
            this.upperLimit02,
            this.lowerLimit02});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1650, 540);
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
            // time
            // 
            this.time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.time.HeaderText = "时间";
            this.time.MinimumWidth = 6;
            this.time.Name = "time";
            // 
            // curVal01
            // 
            this.curVal01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.curVal01.HeaderText = "电流I_1";
            this.curVal01.MinimumWidth = 6;
            this.curVal01.Name = "curVal01";
            // 
            // curValAverage01
            // 
            this.curValAverage01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.curValAverage01.HeaderText = "电流I_1均值";
            this.curValAverage01.MinimumWidth = 6;
            this.curValAverage01.Name = "curValAverage01";
            // 
            // upperLimit01
            // 
            this.upperLimit01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.upperLimit01.HeaderText = "电流I_1上限";
            this.upperLimit01.MinimumWidth = 6;
            this.upperLimit01.Name = "upperLimit01";
            // 
            // lowerLimit01
            // 
            this.lowerLimit01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lowerLimit01.HeaderText = "电流I_1下限";
            this.lowerLimit01.MinimumWidth = 6;
            this.lowerLimit01.Name = "lowerLimit01";
            // 
            // curVal02
            // 
            this.curVal02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.curVal02.HeaderText = "电流I_2";
            this.curVal02.MinimumWidth = 6;
            this.curVal02.Name = "curVal02";
            // 
            // curValAverage02
            // 
            this.curValAverage02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.curValAverage02.HeaderText = "电流I_2均值";
            this.curValAverage02.MinimumWidth = 6;
            this.curValAverage02.Name = "curValAverage02";
            // 
            // upperLimit02
            // 
            this.upperLimit02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.upperLimit02.HeaderText = "电流I_2上限";
            this.upperLimit02.MinimumWidth = 6;
            this.upperLimit02.Name = "upperLimit02";
            // 
            // lowerLimit02
            // 
            this.lowerLimit02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lowerLimit02.HeaderText = "电流I_2下限";
            this.lowerLimit02.MinimumWidth = 6;
            this.lowerLimit02.Name = "lowerLimit02";
            // 
            // DataDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1653, 729);
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
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label warningMsg;
        private System.Windows.Forms.TextBox portValue;
        private System.Windows.Forms.TextBox ipValue;
        private System.Windows.Forms.Label portName;
        private System.Windows.Forms.Label ipAddress;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn curVal01;
        private System.Windows.Forms.DataGridViewTextBoxColumn curValAverage01;
        private System.Windows.Forms.DataGridViewTextBoxColumn upperLimit01;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowerLimit01;
        private System.Windows.Forms.DataGridViewTextBoxColumn curVal02;
        private System.Windows.Forms.DataGridViewTextBoxColumn curValAverage02;
        private System.Windows.Forms.DataGridViewTextBoxColumn upperLimit02;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowerLimit02;
    }
}

