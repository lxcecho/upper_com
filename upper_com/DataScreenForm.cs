using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using Org.BouncyCastle.Bcpg.Sig;
using NPOI.SS.Formula.Functions;
using Microsoft.Office.Interop.Excel;

namespace upper_com
{
    public partial class DataDetection : Form
    {
        float X;
        float Y;

        #region 数据定时刷新
        private FileSystemWatcher currentFileWatcher;
        private FileSystemWatcher voltageFileWatcher;
        private readonly string currentFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_current.xlsx";
        private readonly string voltageFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_voltage.xlsx";
        private readonly object currentFileLock = new object();
        private readonly object voltageFileLock = new object();
        #endregion

        private System.Windows.Forms.Timer dataChangeTimer;

        private string placeholderText = "请用英文括号和英文逗号分隔，如:(500,1000),(600, 2000)";

        private MultimeterDetection multimeterDetection;

        private PLCDetection plcDetection;

        public DataDetection()
        {
            InitializeComponent();

            #region 控件自适应窗口大小
            this.Resize += new EventHandler(Form1_Resize);
            X = this.Width;
            Y = this.Height;
            setTag(this);
            #endregion

            // 程序启动时加载数据
            LoadData();

            StartServerInBackground();

            //new VisaCommunication().Test("192.168.1.25");
        }

        private void StartServerInBackground()
        {
            // 创建并启动一个后台任务来运行服务器
            Task.Run(async () =>
            {
                try
                {
                    multimeterDetection = new MultimeterDetection(this.myLED2, this.dataGridView1);
                    await multimeterDetection.SendConfigCommand();
                    // 在客户端连接成功后启用按钮
                    /*this.Invoke((Action)(() =>
                    {
                        myBtn.Enabled = true;
                    }));*/
                }
                catch (Exception ex)
                {
                    // 在UI线程上显示错误信息
                    this.Invoke((System.Action)(() =>
                    {
                        MessageBox.Show($"服务器启动失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void InputTextBox_Enter(object sender, EventArgs e)
        {
            if (inputTextBox.Text == placeholderText)
            {
                inputTextBox.Text = "";
                inputTextBox.ForeColor = Color.Black;
            }
        }

        private void InputTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                inputTextBox.Text = placeholderText;
                inputTextBox.ForeColor = Color.Gray;
            }
        }

        private void syncBtn_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Text == placeholderText || string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                MessageBox.Show("请输入数据。");
                return;
            }

            string input = inputTextBox.Text.Trim();

            // 验证输入格式是否正确
            if (IsValidInput(input))
            {
                // 生成15个相同的数据对，以逗号分隔
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < 15; i++)
                {
                    result.Append(input);
                    if (i < 14) // 在最后一个数据对后不添加逗号
                    {
                        result.Append(", ");
                    }
                }

                // 显示结果
                inputTextBox.Text = result.ToString();
                inputTextBox.ForeColor = Color.Black;
            }
            else
            {
                MessageBox.Show("请输入正确格式的数据，例如：(500,1000)");
            }
        }

        // 验证输入格式的简单方法
        private bool IsValidInput(string input)
        {
            // 使用正则表达式验证格式 (500,1000)
            return Regex.IsMatch(input, @"^\(\d+,\d+\)$");
        }

        private void LoadData()
        {
            LoadLatestCurrentDataToDataGridView();
            LoadLatestVoltageDataToDataGridView();
        }

        private void LoadLatestCurrentDataToDataGridView()
        {
            lock (currentFileLock)
            {
                if (this.dataGridView1.InvokeRequired)
                {
                    this.dataGridView1.Invoke((MethodInvoker)delegate
                    {
                        UpdateCurrentDataGridView();
                    });
                }
                else
                {
                    UpdateCurrentDataGridView();
                }
            }
        }
        private void UpdateCurrentDataGridView()
        {
            // 清除现有数据
            this.dataGridView1.Rows.Clear();

            if (!File.Exists(currentFilePath))
            {
                //MessageBox.Show("当天记录压力数据文件文件不存在，表格数据为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("当天记录的电流数据文件文件不存在，表格数据为空。");
                return;
            }

            try
            {
                using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // 读取最新的15条数据
                    int startRow = Math.Max(1, sheet.LastRowNum - 14);
                    for (int i = startRow; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        object[] rowData = new object[row.LastCellNum];
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            rowData[j] = row.GetCell(j)?.ToString();
                        }
                        this.dataGridView1.Rows.Add(rowData);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"文件访问错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLatestVoltageDataToDataGridView()
        {
            lock (voltageFileLock)
            {
                if (this.dataGridView2.InvokeRequired)
                {
                    this.dataGridView2.Invoke((MethodInvoker)delegate
                    {
                        UpdateVoltageDataGridView();
                    });
                }
                else
                {
                    UpdateVoltageDataGridView();
                }
            }
        }

        private void UpdateVoltageDataGridView()
        {
            // 清除现有数据
            this.dataGridView2.Rows.Clear();

            if (!File.Exists(voltageFilePath))
            {
                //MessageBox.Show("当天记录的电流数据文件文件不存在，表格数据为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("当天记录的电流数据文件文件不存在，表格数据为空。");
                return;
            }

            try
            {
                using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // 读取最新的15条数据
                    int startRow = Math.Max(1, sheet.LastRowNum - 14);
                    for (int i = startRow; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        object[] rowData = new object[row.LastCellNum];
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            rowData[j] = row.GetCell(j)?.ToString();
                        }
                        this.dataGridView2.Rows.Add(rowData);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"文件访问错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UpdateVoltagetData(VoltageData voltageData)
        {
            if (this.dataGridView2.InvokeRequired)
            {
                // 先更新DataGridView
                this.dataGridView2.Invoke((MethodInvoker)delegate
                {
                    AddVoltageDataToGridView(voltageData);
                });
            }
            else
            {
                AddVoltageDataToGridView(voltageData);
            }

            // 异步写入文件
            await Task.Run(() => WriteVoltageDataToFile(voltageData));
        }

        private void AddVoltageDataToGridView(VoltageData voltageData)
        {
            // 更新 DataGridView
            this.dataGridView1.Rows.Add(voltageData.GetCurrentNo(), voltageData.GetVoltageTransformSignal());
            if (this.dataGridView2.Rows.Count > 15)
            {
                this.dataGridView2.Rows.RemoveAt(0);
            }
        }

        private void WriteVoltageDataToFile(VoltageData voltageData)
        {
            lock (voltageFileLock)
            {
                try
                {
                    IWorkbook workbook;
                    ISheet sheet;

                    if (!File.Exists(voltageFilePath))
                    {
                        workbook = new XSSFWorkbook();
                        sheet = workbook.CreateSheet("Sheet1");

                        // 写入标题行
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.CreateCell(0).SetCellValue("电流测试编号");
                        headerRow.CreateCell(1).SetCellValue("压力传送信号");
                        headerRow.CreateCell(2).SetCellValue("压力值V");
                        headerRow.CreateCell(3).SetCellValue("压力均值");
                        headerRow.CreateCell(4).SetCellValue("压力上限");
                        headerRow.CreateCell(5).SetCellValue("压力下限");
                    }
                    else
                    {
                        // 打开现有文件并追加数据
                        using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            workbook = new XSSFWorkbook(fs);
                            sheet = workbook.GetSheetAt(0);
                        }
                    }

                    // 找到最后一行
                    int lastRowNum = sheet.LastRowNum;
                    IRow newRow = sheet.CreateRow(lastRowNum + 1);
                    // 追加数据
                    newRow.CreateCell(0).SetCellValue(voltageData.GetCurrentNo());
                    newRow.CreateCell(1).SetCellValue(voltageData.GetVoltageTransformSignal());
                    //newRow.CreateCell(2).SetCellValue(voltageData.GetSmoothCur());
                    //newRow.CreateCell(3).SetCellValue(voltageData.GetSmoothAverage());

                    // 写入文件
                    using (var fs = new FileStream(voltageFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        workbook.Write(fs);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"文件写入错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        #region 控件自适应窗口大小
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }

        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                con.Font = new System.Drawing.Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            // throw new Exception("The method or operation is not implemented.");
            float newx = (this.Width) / X;
            //  float newy = (this.Height - this.statusStrip1.Height) / (Y - y);
            float newy = this.Height / Y;
            setControls(newx, newy, this);
            this.Text = this.Width.ToString() + " " + this.Height.ToString();
        }
        #endregion

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void k_label_Click(object sender, EventArgs e)
        {

        }

        private void k_value_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取当前系统时间的方法
        /// </summary>
        /// <returns>当前时间</returns>
        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        private int InvalidateParamsForInt(string text)
        {
            if (string.IsNullOrEmpty(text)
                || !int.TryParse(text, out _)
                || int.Parse(text) <= 0)
            {
                MessageBox.Show("k 值输入错误，请输入大于 0 的十进制有效数字！");
                return 0;
            }
            else
            {
                return int.Parse(text);
            }
        }

        /// <summary>
        /// TODO 行填充
        /// </summary>
        /// <returns>TODO</returns>
        private void AddItem(CurrentData currentData)
        {
            // 此处的代码不能进行循环！必须封装为一个方法，通过方法的循环，才能实现循环！
            DataGridViewRow dgvr = new DataGridViewRow();
            foreach (DataGridViewColumn c in this.dataGridView1.Columns)
            {
                dgvr.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
            }
            dgvr.Cells[0].Value = currentData.GetSerialNo();
            dgvr.Cells[1].Value = currentData.GetCurDate();
            //dgvr.Cells[2].Value = currentData.GetSmoothCur();
            dgvr.Cells[3].Value = currentData.GetSmoothAverage();
            dgvr.Cells[4].Value = currentData.GetSmoothUpper();
            dgvr.Cells[5].Value = currentData.GetSmoothLower();
            //dgvr.Cells[6].Value = currentData.GetMutationCur();
            dgvr.Cells[7].Value = currentData.GetMutationAverage();
            dgvr.Cells[8].Value = currentData.GetMutationUpper();
            dgvr.Cells[9].Value = currentData.GetMutationLower();
            this.dataGridView1.Rows.Add(dgvr);
        }

        private ChartForm chartFormForCurrent;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CellClick event triggered");
            if (e.RowIndex >= 0) // 确保点击的是有效行
            {
                // 从选中的行中获取数据
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 检查第一列的值是否为 null
                if (row.Cells[0].Value != null)
                {
                    string serialNo = row.Cells[0].Value.ToString(); // 假设第一列是 serialNo

                    // 从 Excel 文件中加载相应的电流值
                    List<double> currentValues = LoadCurrentValuesFromExcel(serialNo);

                    if (currentValues.Count > 0)
                    {
                        if (chartFormForCurrent == null || chartFormForCurrent.IsDisposed)
                        {
                            // 创建并显示新的ChartForm
                            chartFormForCurrent = new ChartForm(currentValues.ToArray(), serialNo);
                            chartFormForCurrent.Show();
                        }
                        else
                        {
                            // 更新现有的ChartForm的数据
                            chartFormForCurrent.UpdateChart(currentValues.ToArray(), serialNo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("未找到对应的电流数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("该行的序列号为空。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ChartForm chartFormForVoltage；
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // 确保点击的是有效行
            {
                // 从选中的行中获取数据
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                // 检查第一列的值是否为 null
                if (row.Cells[0].Value != null)
                {
                    string serialNo = row.Cells[0].Value.ToString(); // 假设第一列是 serialNo

                    // 从 Excel 文件中加载相应的电压值
                    List<double> voltageValues = LoadVoltageValuesFromExcel(serialNo);

                    if (voltageValues.Count > 0)
                    {
                        
                        if (chartFormForVoltage == null || chartFormForVoltage.IsDisposed)
                        {
                            // 创建并显示新的ChartForm
                            chartFormForVoltage = new ChartForm(voltageValues.ToArray(), serialNo);
                            chartFormForVoltage.Show();
                        }
                        else
                        {
                            // 更新现有的ChartForm的数据
                            chartFormForCurrent.UpdateChart(voltageValues.ToArray(), serialNo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("未找到对应的电压数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("该行的序列号为空。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<double> LoadCurrentValuesFromExcel(string serialNo)
        {
            List<double> currentValues = new List<double>();

            lock (currentFileLock)
            {
                try
                {
                    if (File.Exists(currentFilePath))
                    {
                        using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            IWorkbook workbook = new XSSFWorkbook(fs);
                            ISheet sheet = workbook.GetSheet("Sheet2");

                            if (sheet != null)
                            {
                                for (int i = 0; i <= sheet.LastRowNum; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row != null)
                                    {
                                        ICell serialNoCell = row.GetCell(0);
                                        if (serialNoCell != null && serialNoCell.ToString() == serialNo)
                                        {
                                            ICell currentValueCell = row.GetCell(1);
                                            if (currentValueCell != null && double.TryParse(currentValueCell.ToString(), out double currentValue))
                                            {
                                                currentValues.Add(currentValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"文件读取错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return currentValues;
        }

        private Queue<(int start, int duration)> LoadTimerData(string inputDataTime)
        {
            Queue<(int start, int duration)> dataQueue = new Queue<(int, int)>();

            dataQueue.Clear();

            var pairs = inputDataTime.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var trimmedPair = pair.Trim();
                if (IsValidInput(trimmedPair))
                {
                    var numbers = trimmedPair.Trim('(', ')').Split(',');
                    int start = int.Parse(numbers[0]);
                    int duration = int.Parse(numbers[1]);
                    dataQueue.Enqueue((start, duration));
                }
                else
                {
                    MessageBox.Show($"Invalid input format: {trimmedPair}");
                }
            }
            return dataQueue;
        }

        private void myBtn_Click(object sender, EventArgs e)
        {
            // 1. 参数校验
            /*InputData inputData = new InputData();
            inputData.K = InvalidateParamsForInt(this.k_value.Text.Trim());
            inputData.Num = InvalidateParamsForInt(this.n_value.Text.Trim());
            inputData.DataQueue = LoadTimerData(inputTextBox.Text.Trim()); ;*/

            MyButton button = sender as MyButton;
            if (button.IsPlaying)
            {

                //MessageBox.Show("暂停");
                this.myLED3.IsFlash = false;
                this.myLED3.LedStatus = true;
                this.myLED3.LedTrueColor = Color.Green;
                // TODO PLC Test
                if (plcDetection != null)
                {
                    plcDetection.SetPlaying(false);
                }
                if (multimeterDetection != null)
                {
                    // multimeterDetection.SetInputDate(inputData);
                    multimeterDetection.SetIsPlaying(false);
                    //_ = multimeterDetection.StartCollectingData();
                }
            }
            else
            {
                //MessageBox.Show("开始");
                this.myLED3.IsFlash = true;
                this.myLED3.LedStatus = true;
                this.myLED3.LedTrueColor = Color.Green;

                if (plcDetection != null)
                {
                    plcDetection.SetPlaying(true);
                }

                // TODO PLC Test
                if (multimeterDetection != null)
                {
                    /*multimeterDetection.SetInputDate(inputData);
                    multimeterDetection.SetIsPlaying(true);
                    _ = multimeterDetection.StartCollectingData();*/

                    _ = multimeterDetection.TestData();
                }
            }
        }

        private void myLED1_Load(object sender, EventArgs e)
        {

        }

        private void connPlcBtn_Click(object sender, EventArgs e)
        {
            string ipAddress = this.plcIp.IpAddress;
            Console.WriteLine($"IP Address: {ipAddress}");
            if (string.IsNullOrWhiteSpace(ipAddress) || !IsValidIp(ipAddress))
            {
                MessageBox.Show("请输入正确的IP地址！！！");
            }
            plcDetection = new PLCDetection(ipAddress, multimeterDetection, this.myLED1);
            _ = plcDetection.PlcConn();
        }

        private bool IsValidIp(string ipAddress)
        {
            // 正则表达式用于验证IP地址格式
            string pattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            if (Regex.IsMatch(ipAddress, pattern))
            {
                // 检查每个部分是否在0到255之间
                string[] parts = ipAddress.Split('.');
                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int num))
                    {
                        if (num < 0 || num > 255)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private List<double> LoadVoltageValuesFromExcel(string serialNo)
        {
            List<double> voltageValues = new List<double>();

            lock (voltageFileLock)
            {
                try
                {
                    if (File.Exists(voltageFilePath))
                    {
                        using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            IWorkbook workbook = new XSSFWorkbook(fs);
                            ISheet sheet = workbook.GetSheet("Sheet2");

                            if (sheet != null)
                            {
                                for (int i = 0; i <= sheet.LastRowNum; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    if (row != null)
                                    {
                                        ICell serialNoCell = row.GetCell(0);
                                        if (serialNoCell != null && serialNoCell.ToString() == serialNo)
                                        {
                                            ICell voltageValueCell = row.GetCell(1);
                                            if (voltageValueCell != null && double.TryParse(voltageValueCell.ToString(), out double voltageValue))
                                            {
                                                voltageValues.Add(voltageValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"文件读取错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return voltageValues;
        }
    }
}
