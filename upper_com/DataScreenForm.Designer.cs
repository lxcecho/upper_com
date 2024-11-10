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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.curNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valtageTransformSignal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltage1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.volLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.connPlcBtn = new System.Windows.Forms.Button();
            this.plcIp = new upper_com.IpInputControl();
            this.label2 = new System.Windows.Forms.Label();
            this.myLED3 = new upper_com.MyLED();
            this.myLED2 = new upper_com.MyLED();
            this.syncBtn = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.myBtn = new upper_com.MyButton();
            this.label3 = new System.Windows.Forms.Label();
            this.myLED1 = new upper_com.MyLED();
            this.kLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nLabel = new System.Windows.Forms.Label();
            this.k_value = new System.Windows.Forms.TextBox();
            this.n_value = new System.Windows.Forms.TextBox();
            this.serialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.curDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smoothLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutationLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 705);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(5, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView2);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.connPlcBtn);
            this.splitContainer1.Panel2.Controls.Add(this.plcIp);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.myLED3);
            this.splitContainer1.Panel2.Controls.Add(this.myLED2);
            this.splitContainer1.Panel2.Controls.Add(this.syncBtn);
            this.splitContainer1.Panel2.Controls.Add(this.inputTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.myBtn);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.myLED1);
            this.splitContainer1.Panel2.Controls.Add(this.kLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.nLabel);
            this.splitContainer1.Panel2.Controls.Add(this.k_value);
            this.splitContainer1.Panel2.Controls.Add(this.n_value);
            this.splitContainer1.Size = new System.Drawing.Size(1484, 705);
            this.splitContainer1.SplitterDistance = 1093;
            this.splitContainer1.TabIndex = 6;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.curNo,
            this.valtageTransformSignal,
            this.voltage1,
            this.valAverage,
            this.valUpper,
            this.volLower});
            this.dataGridView2.Location = new System.Drawing.Point(0, 338);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 27;
            this.dataGridView2.Size = new System.Drawing.Size(1093, 364);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // curNo
            // 
            this.curNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.curNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.curNo.HeaderText = "电流测试编号";
            this.curNo.MinimumWidth = 6;
            this.curNo.Name = "curNo";
            // 
            // valtageTransformSignal
            // 
            this.valtageTransformSignal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valtageTransformSignal.DefaultCellStyle = dataGridViewCellStyle3;
            this.valtageTransformSignal.HeaderText = "压力传送信号";
            this.valtageTransformSignal.MinimumWidth = 6;
            this.valtageTransformSignal.Name = "valtageTransformSignal";
            // 
            // voltage1
            // 
            this.voltage1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.voltage1.DefaultCellStyle = dataGridViewCellStyle4;
            this.voltage1.HeaderText = "压力值V1";
            this.voltage1.MinimumWidth = 6;
            this.voltage1.Name = "voltage1";
            // 
            // valAverage
            // 
            this.valAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valAverage.DefaultCellStyle = dataGridViewCellStyle5;
            this.valAverage.HeaderText = "压力均值";
            this.valAverage.MinimumWidth = 6;
            this.valAverage.Name = "valAverage";
            // 
            // valUpper
            // 
            this.valUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valUpper.DefaultCellStyle = dataGridViewCellStyle6;
            this.valUpper.HeaderText = "压力上限";
            this.valUpper.MinimumWidth = 6;
            this.valUpper.Name = "valUpper";
            // 
            // volLower
            // 
            this.volLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.volLower.DefaultCellStyle = dataGridViewCellStyle7;
            this.volLower.HeaderText = "压力下限";
            this.volLower.MinimumWidth = 6;
            this.volLower.Name = "volLower";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialNo,
            this.curDate,
            this.smoothAverage,
            this.smoothUpper,
            this.smoothLower,
            this.mutationAverage,
            this.mutationUpper,
            this.mutationLower});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1092, 336);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // connPlcBtn
            // 
            this.connPlcBtn.Font = new System.Drawing.Font("SimSun", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.connPlcBtn.Location = new System.Drawing.Point(270, 131);
            this.connPlcBtn.Name = "connPlcBtn";
            this.connPlcBtn.Size = new System.Drawing.Size(82, 38);
            this.connPlcBtn.TabIndex = 32;
            this.connPlcBtn.Text = "CONN";
            this.connPlcBtn.UseVisualStyleBackColor = true;
            this.connPlcBtn.Click += new System.EventHandler(this.connPlcBtn_Click);
            // 
            // plcIp
            // 
            this.plcIp.IpAddress = "...";
            this.plcIp.Location = new System.Drawing.Point(22, 142);
            this.plcIp.Name = "plcIp";
            this.plcIp.Size = new System.Drawing.Size(242, 27);
            this.plcIp.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(26, 597);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 20);
            this.label2.TabIndex = 29;
            this.label2.Text = "开始采集：";
            // 
            // myLED3
            // 
            this.myLED3.BorderWidth = 5;
            this.myLED3.CenterColor = System.Drawing.Color.White;
            this.myLED3.FlashInterval = 500;
            this.myLED3.GapWidth = 5;
            this.myLED3.Is3D = false;
            this.myLED3.IsBorder = true;
            this.myLED3.IsFlash = false;
            this.myLED3.IsHighLight = true;
            this.myLED3.LampColor = new System.Drawing.Color[] {
        System.Drawing.Color.Green,
        System.Drawing.Color.Red};
            this.myLED3.LedColor = System.Drawing.Color.DodgerBlue;
            this.myLED3.LedFalseColor = System.Drawing.Color.DimGray;
            this.myLED3.LedStatus = false;
            this.myLED3.LedTrueColor = System.Drawing.Color.DimGray;
            this.myLED3.Location = new System.Drawing.Point(223, 21);
            this.myLED3.Name = "myLED3";
            this.myLED3.Size = new System.Drawing.Size(81, 43);
            this.myLED3.TabIndex = 26;
            // 
            // myLED2
            // 
            this.myLED2.BorderWidth = 5;
            this.myLED2.CenterColor = System.Drawing.Color.White;
            this.myLED2.FlashInterval = 500;
            this.myLED2.GapWidth = 5;
            this.myLED2.Is3D = false;
            this.myLED2.IsBorder = true;
            this.myLED2.IsFlash = false;
            this.myLED2.IsHighLight = true;
            this.myLED2.LampColor = new System.Drawing.Color[] {
        System.Drawing.Color.Green,
        System.Drawing.Color.Red};
            this.myLED2.LedColor = System.Drawing.Color.DodgerBlue;
            this.myLED2.LedFalseColor = System.Drawing.Color.DimGray;
            this.myLED2.LedStatus = false;
            this.myLED2.LedTrueColor = System.Drawing.Color.DimGray;
            this.myLED2.Location = new System.Drawing.Point(157, 21);
            this.myLED2.Name = "myLED2";
            this.myLED2.Size = new System.Drawing.Size(73, 43);
            this.myLED2.TabIndex = 25;
            // 
            // syncBtn
            // 
            this.syncBtn.Font = new System.Drawing.Font("SimSun", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.syncBtn.Location = new System.Drawing.Point(270, 348);
            this.syncBtn.Name = "syncBtn";
            this.syncBtn.Size = new System.Drawing.Size(82, 42);
            this.syncBtn.TabIndex = 24;
            this.syncBtn.Text = "SYNC";
            this.syncBtn.UseVisualStyleBackColor = true;
            this.syncBtn.Click += new System.EventHandler(this.syncBtn_Click);
            // 
            // inputTextBox
            // 
            this.inputTextBox.ForeColor = System.Drawing.Color.Gray;
            this.inputTextBox.Location = new System.Drawing.Point(22, 406);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(330, 149);
            this.inputTextBox.TabIndex = 23;
            this.inputTextBox.Text = "请用英文括号和英文逗号分隔，如:(500,1000),(600, 2000)";
            this.inputTextBox.Enter += new System.EventHandler(this.InputTextBox_Enter);
            this.inputTextBox.Leave += new System.EventHandler(this.InputTextBox_Leave);
            // 
            // myBtn
            // 
            this.myBtn.IsPlaying = false;
            this.myBtn.Location = new System.Drawing.Point(156, 585);
            this.myBtn.Name = "myBtn";
            this.myBtn.Size = new System.Drawing.Size(51, 46);
            this.myBtn.TabIndex = 22;
            this.myBtn.UseVisualStyleBackColor = true;
            this.myBtn.Click += new System.EventHandler(this.myBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(19, 101);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "请输入PLC-IP：";
            // 
            // myLED1
            // 
            this.myLED1.BorderWidth = 5;
            this.myLED1.CenterColor = System.Drawing.Color.White;
            this.myLED1.FlashInterval = 500;
            this.myLED1.GapWidth = 5;
            this.myLED1.Is3D = false;
            this.myLED1.IsBorder = true;
            this.myLED1.IsFlash = false;
            this.myLED1.IsHighLight = true;
            this.myLED1.LampColor = new System.Drawing.Color[] {
        System.Drawing.Color.Green,
        System.Drawing.Color.Red};
            this.myLED1.LedColor = System.Drawing.Color.DodgerBlue;
            this.myLED1.LedFalseColor = System.Drawing.Color.Gray;
            this.myLED1.LedStatus = false;
            this.myLED1.LedTrueColor = System.Drawing.Color.Gray;
            this.myLED1.Location = new System.Drawing.Point(89, 21);
            this.myLED1.Name = "myLED1";
            this.myLED1.Size = new System.Drawing.Size(66, 43);
            this.myLED1.TabIndex = 13;
            this.myLED1.Load += new System.EventHandler(this.myLED1_Load);
            // 
            // kLabel
            // 
            this.kLabel.AutoSize = true;
            this.kLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kLabel.Location = new System.Drawing.Point(19, 204);
            this.kLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.kLabel.Name = "kLabel";
            this.kLabel.Size = new System.Drawing.Size(102, 15);
            this.kLabel.TabIndex = 0;
            this.kLabel.Text = "k      值：";
            this.kLabel.Click += new System.EventHandler(this.k_label_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(19, 317);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "请输入15组(t1,t2)数据：单位：ms";
            // 
            // nLabel
            // 
            this.nLabel.AutoSize = true;
            this.nLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nLabel.Location = new System.Drawing.Point(19, 247);
            this.nLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nLabel.Name = "nLabel";
            this.nLabel.Size = new System.Drawing.Size(102, 15);
            this.nLabel.TabIndex = 1;
            this.nLabel.Text = "N      值：";
            // 
            // k_value
            // 
            this.k_value.Location = new System.Drawing.Point(156, 201);
            this.k_value.Margin = new System.Windows.Forms.Padding(4);
            this.k_value.Name = "k_value";
            this.k_value.Size = new System.Drawing.Size(196, 25);
            this.k_value.TabIndex = 2;
            this.k_value.Text = "3";
            this.k_value.TextChanged += new System.EventHandler(this.k_value_TextChanged);
            // 
            // n_value
            // 
            this.n_value.Location = new System.Drawing.Point(155, 244);
            this.n_value.Margin = new System.Windows.Forms.Padding(4);
            this.n_value.Name = "n_value";
            this.n_value.Size = new System.Drawing.Size(197, 25);
            this.n_value.TabIndex = 3;
            this.n_value.Text = "20";
            // 
            // serialNo
            // 
            this.serialNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.serialNo.DefaultCellStyle = dataGridViewCellStyle9;
            this.serialNo.HeaderText = "测试编号";
            this.serialNo.MinimumWidth = 6;
            this.serialNo.Name = "serialNo";
            // 
            // curDate
            // 
            this.curDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.curDate.DefaultCellStyle = dataGridViewCellStyle10;
            this.curDate.HeaderText = "时间";
            this.curDate.MinimumWidth = 6;
            this.curDate.Name = "curDate";
            // 
            // smoothAverage
            // 
            this.smoothAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothAverage.DefaultCellStyle = dataGridViewCellStyle11;
            this.smoothAverage.HeaderText = "I1均值";
            this.smoothAverage.MinimumWidth = 6;
            this.smoothAverage.Name = "smoothAverage";
            // 
            // smoothUpper
            // 
            this.smoothUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothUpper.DefaultCellStyle = dataGridViewCellStyle12;
            this.smoothUpper.HeaderText = "I1上限";
            this.smoothUpper.MinimumWidth = 6;
            this.smoothUpper.Name = "smoothUpper";
            // 
            // smoothLower
            // 
            this.smoothLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothLower.DefaultCellStyle = dataGridViewCellStyle13;
            this.smoothLower.HeaderText = "I1下限";
            this.smoothLower.MinimumWidth = 6;
            this.smoothLower.Name = "smoothLower";
            // 
            // mutationAverage
            // 
            this.mutationAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationAverage.DefaultCellStyle = dataGridViewCellStyle14;
            this.mutationAverage.HeaderText = "I2均值";
            this.mutationAverage.MinimumWidth = 6;
            this.mutationAverage.Name = "mutationAverage";
            // 
            // mutationUpper
            // 
            this.mutationUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationUpper.DefaultCellStyle = dataGridViewCellStyle15;
            this.mutationUpper.HeaderText = "I2上限";
            this.mutationUpper.MinimumWidth = 6;
            this.mutationUpper.Name = "mutationUpper";
            // 
            // mutationLower
            // 
            this.mutationLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationLower.DefaultCellStyle = dataGridViewCellStyle16;
            this.mutationLower.HeaderText = "I2下限";
            this.mutationLower.MinimumWidth = 6;
            this.mutationLower.Name = "mutationLower";
            // 
            // DataDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 705);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DataDetection";
            this.Text = "数据监控";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
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
        private MyLED myLED1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn curNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn valtageTransformSignal;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltage1;
        private System.Windows.Forms.DataGridViewTextBoxColumn valAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn valUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn volLower;
        private MyButton myBtn;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button syncBtn;
        private MyLED myLED3;
        private MyLED myLED2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private IpInputControl plcIp;
        private System.Windows.Forms.Button connPlcBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn curDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn smoothLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutationLower;
    }
}

