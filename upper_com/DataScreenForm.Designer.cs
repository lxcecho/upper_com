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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle145 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle146 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle147 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle148 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle149 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle150 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle151 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle152 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle153 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle154 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle155 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle156 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle157 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle158 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle159 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle160 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle161 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle162 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.kLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nLabel = new System.Windows.Forms.Label();
            this.k_value = new System.Windows.Forms.TextBox();
            this.n_value = new System.Windows.Forms.TextBox();
            this.syncBtn = new System.Windows.Forms.Button();
            this.myBtn = new upper_com.MyButton();
            this.myLED1 = new upper_com.MyLED();
            this.myLED2 = new upper_com.MyLED();
            this.myLED3 = new upper_com.MyLED();
            this.label2 = new System.Windows.Forms.Label();
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
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.myLED3);
            this.splitContainer1.Panel2.Controls.Add(this.myLED2);
            this.splitContainer1.Panel2.Controls.Add(this.syncBtn);
            this.splitContainer1.Panel2.Controls.Add(this.inputTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.myBtn);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.myLED1);
            this.splitContainer1.Panel2.Controls.Add(this.kLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.nLabel);
            this.splitContainer1.Panel2.Controls.Add(this.k_value);
            this.splitContainer1.Panel2.Controls.Add(this.n_value);
            this.splitContainer1.Size = new System.Drawing.Size(1484, 705);
            this.splitContainer1.SplitterDistance = 1096;
            this.splitContainer1.TabIndex = 6;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle145.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle145.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle145.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle145.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle145.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle145.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle145.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle145;
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
            // 
            // curNo
            // 
            this.curNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle146.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.curNo.DefaultCellStyle = dataGridViewCellStyle146;
            this.curNo.HeaderText = "电流测试编号";
            this.curNo.MinimumWidth = 6;
            this.curNo.Name = "curNo";
            // 
            // valtageTransformSignal
            // 
            this.valtageTransformSignal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle147.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valtageTransformSignal.DefaultCellStyle = dataGridViewCellStyle147;
            this.valtageTransformSignal.HeaderText = "压力传送信号";
            this.valtageTransformSignal.MinimumWidth = 6;
            this.valtageTransformSignal.Name = "valtageTransformSignal";
            // 
            // voltage1
            // 
            this.voltage1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle148.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.voltage1.DefaultCellStyle = dataGridViewCellStyle148;
            this.voltage1.HeaderText = "压力值V1";
            this.voltage1.MinimumWidth = 6;
            this.voltage1.Name = "voltage1";
            // 
            // valAverage
            // 
            this.valAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle149.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valAverage.DefaultCellStyle = dataGridViewCellStyle149;
            this.valAverage.HeaderText = "压力均值";
            this.valAverage.MinimumWidth = 6;
            this.valAverage.Name = "valAverage";
            // 
            // valUpper
            // 
            this.valUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle150.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.valUpper.DefaultCellStyle = dataGridViewCellStyle150;
            this.valUpper.HeaderText = "压力上限";
            this.valUpper.MinimumWidth = 6;
            this.valUpper.Name = "valUpper";
            // 
            // volLower
            // 
            this.volLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle151.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.volLower.DefaultCellStyle = dataGridViewCellStyle151;
            this.volLower.HeaderText = "压力下限";
            this.volLower.MinimumWidth = 6;
            this.volLower.Name = "volLower";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle152.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle152.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle152.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle152.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle152.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle152.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle152.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle152;
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1092, 344);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // serialNo
            // 
            this.serialNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle153.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.serialNo.DefaultCellStyle = dataGridViewCellStyle153;
            this.serialNo.HeaderText = "序号";
            this.serialNo.MinimumWidth = 6;
            this.serialNo.Name = "serialNo";
            // 
            // curDate
            // 
            this.curDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle154.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.curDate.DefaultCellStyle = dataGridViewCellStyle154;
            this.curDate.HeaderText = "时间";
            this.curDate.MinimumWidth = 6;
            this.curDate.Name = "curDate";
            // 
            // smoothCur
            // 
            this.smoothCur.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle155.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothCur.DefaultCellStyle = dataGridViewCellStyle155;
            this.smoothCur.HeaderText = "电流I1";
            this.smoothCur.MinimumWidth = 6;
            this.smoothCur.Name = "smoothCur";
            // 
            // smoothAverage
            // 
            this.smoothAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle156.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothAverage.DefaultCellStyle = dataGridViewCellStyle156;
            this.smoothAverage.HeaderText = "I1均值";
            this.smoothAverage.MinimumWidth = 6;
            this.smoothAverage.Name = "smoothAverage";
            // 
            // smoothUpper
            // 
            this.smoothUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle157.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothUpper.DefaultCellStyle = dataGridViewCellStyle157;
            this.smoothUpper.HeaderText = "I1上限";
            this.smoothUpper.MinimumWidth = 6;
            this.smoothUpper.Name = "smoothUpper";
            // 
            // smoothLower
            // 
            this.smoothLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle158.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.smoothLower.DefaultCellStyle = dataGridViewCellStyle158;
            this.smoothLower.HeaderText = "I1下限";
            this.smoothLower.MinimumWidth = 6;
            this.smoothLower.Name = "smoothLower";
            // 
            // mutationCur
            // 
            this.mutationCur.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle159.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationCur.DefaultCellStyle = dataGridViewCellStyle159;
            this.mutationCur.HeaderText = "电流I2";
            this.mutationCur.MinimumWidth = 6;
            this.mutationCur.Name = "mutationCur";
            // 
            // mutationAverage
            // 
            this.mutationAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle160.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationAverage.DefaultCellStyle = dataGridViewCellStyle160;
            this.mutationAverage.HeaderText = "I2均值";
            this.mutationAverage.MinimumWidth = 6;
            this.mutationAverage.Name = "mutationAverage";
            // 
            // mutationUpper
            // 
            this.mutationUpper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle161.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationUpper.DefaultCellStyle = dataGridViewCellStyle161;
            this.mutationUpper.HeaderText = "I2上限";
            this.mutationUpper.MinimumWidth = 6;
            this.mutationUpper.Name = "mutationUpper";
            // 
            // mutationLower
            // 
            this.mutationLower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle162.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mutationLower.DefaultCellStyle = dataGridViewCellStyle162;
            this.mutationLower.HeaderText = "I2下限";
            this.mutationLower.MinimumWidth = 6;
            this.mutationLower.Name = "mutationLower";
            // 
            // inputTextBox
            // 
            this.inputTextBox.ForeColor = System.Drawing.Color.Gray;
            this.inputTextBox.Location = new System.Drawing.Point(22, 279);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(330, 25);
            this.inputTextBox.TabIndex = 23;
            this.inputTextBox.Text = "用英文逗号分隔，如：0.1,0.5,1.3";
            this.inputTextBox.Enter += new System.EventHandler(this.InputTextBox_Enter);
            this.inputTextBox.Leave += new System.EventHandler(this.InputTextBox_Leave);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(151, 186);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(201, 25);
            this.textBox1.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(19, 189);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "总 时 长 T：";
            // 
            // kLabel
            // 
            this.kLabel.AutoSize = true;
            this.kLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kLabel.Location = new System.Drawing.Point(19, 102);
            this.kLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.kLabel.Name = "kLabel";
            this.kLabel.Size = new System.Drawing.Size(93, 15);
            this.kLabel.TabIndex = 0;
            this.kLabel.Text = "k      值：";
            this.kLabel.Click += new System.EventHandler(this.k_label_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 236);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "请输入15组平稳段持续时间设置：";
            // 
            // nLabel
            // 
            this.nLabel.AutoSize = true;
            this.nLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nLabel.Location = new System.Drawing.Point(19, 145);
            this.nLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nLabel.Name = "nLabel";
            this.nLabel.Size = new System.Drawing.Size(93, 15);
            this.nLabel.TabIndex = 1;
            this.nLabel.Text = "N      值：";
            // 
            // k_value
            // 
            this.k_value.Location = new System.Drawing.Point(151, 99);
            this.k_value.Margin = new System.Windows.Forms.Padding(4);
            this.k_value.Name = "k_value";
            this.k_value.Size = new System.Drawing.Size(201, 25);
            this.k_value.TabIndex = 2;
            this.k_value.Text = "3";
            this.k_value.TextChanged += new System.EventHandler(this.k_value_TextChanged);
            // 
            // n_value
            // 
            this.n_value.Location = new System.Drawing.Point(151, 142);
            this.n_value.Margin = new System.Windows.Forms.Padding(4);
            this.n_value.Name = "n_value";
            this.n_value.Size = new System.Drawing.Size(201, 25);
            this.n_value.TabIndex = 3;
            this.n_value.Text = "20";
            // 
            // syncBtn
            // 
            this.syncBtn.Location = new System.Drawing.Point(269, 232);
            this.syncBtn.Name = "syncBtn";
            this.syncBtn.Size = new System.Drawing.Size(68, 23);
            this.syncBtn.TabIndex = 24;
            this.syncBtn.Text = "SYNC";
            this.syncBtn.UseVisualStyleBackColor = true;
            this.syncBtn.Click += new System.EventHandler(this.syncBtn_Click);
            // 
            // myBtn
            // 
            this.myBtn.IsPlaying = false;
            this.myBtn.Location = new System.Drawing.Point(290, 435);
            this.myBtn.Name = "myBtn";
            this.myBtn.Size = new System.Drawing.Size(62, 63);
            this.myBtn.TabIndex = 22;
            this.myBtn.UseVisualStyleBackColor = true;
            this.myBtn.Click += new System.EventHandler(this.myBtn_Click);
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
            this.myLED1.LedFalseColor = System.Drawing.Color.DimGray;
            this.myLED1.LedStatus = false;
            this.myLED1.LedTrueColor = System.Drawing.Color.DimGray;
            this.myLED1.Location = new System.Drawing.Point(22, 12);
            this.myLED1.Name = "myLED1";
            this.myLED1.Size = new System.Drawing.Size(64, 50);
            this.myLED1.TabIndex = 13;
            this.myLED1.Load += new System.EventHandler(this.myLED1_Load);
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
            this.myLED2.Location = new System.Drawing.Point(151, 12);
            this.myLED2.Name = "myLED2";
            this.myLED2.Size = new System.Drawing.Size(64, 50);
            this.myLED2.TabIndex = 25;
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
            this.myLED3.Location = new System.Drawing.Point(273, 12);
            this.myLED3.Name = "myLED3";
            this.myLED3.Size = new System.Drawing.Size(64, 50);
            this.myLED3.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(135, 473);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 20);
            this.label2.TabIndex = 29;
            this.label2.Text = "开始采集：";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn curNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn valtageTransformSignal;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltage1;
        private System.Windows.Forms.DataGridViewTextBoxColumn valAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn valUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn volLower;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private MyButton myBtn;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button syncBtn;
        private MyLED myLED3;
        private MyLED myLED2;
        private System.Windows.Forms.Label label2;
    }
}

