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
using static OfficeOpenXml.ExcelErrorValue;

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

            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;

            //StartServerInBackground();

            //new VisaCommunication().Test("192.168.1.25");
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

        /*private void StartServerInBackground()
        {
            // 创建并启动一个后台任务来运行服务器
            Task.Run(async () =>
            {
                try
                {
                    multimeterDetection = new MultimeterDetection(this.myLED2, this.dataGridView1);
                    await multimeterDetection.SendConfigCommand();
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
        }*/

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

        // 验证输入格式的简单方法
        private bool ValidateInput(string input)
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
                //MessageBox.Show("当天记录的电压数据文件文件不存在，表格数据为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("当天记录的电压数据文件文件不存在，表格数据为空。");
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

        /*private ChartForm chartFormForCurrent;*/
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

                    double[] xValues = new double[2];

                    // 从 Excel 文件中加载相应的电流值
                    List<(double currentValue, double duration)> curs = LoadCurrentValuesFromExcelForSheet2(serialNo);

                    List<double> values = new List<double>();
                    List<double> durations = new List<double>();

                    if (curs.Count > 0)
                    {
                        foreach (var (currentValue, duration) in curs)
                        {
                            values.Add(currentValue);
                            durations.Add(duration);
                            //Console.WriteLine($"Current Value: {currentValue}, Duration: {duration}");
                        }
                    }

                    if (durations.Count > 0 && durations.Distinct().ToList().Count > 0)
                    {
                        xValues[0] = 0.0;
                        xValues[1] = durations.Distinct().ToList()[0];
                    }

                    if (values.Count > 0)
                    {
                        // 创建并显示新的ChartForm
                        ChartForm chartFormForCurrent = new ChartForm(xValues, values.ToArray(), serialNo);
                        chartFormForCurrent.Show();
                        /*if (chartFormForCurrent == null || chartFormForCurrent.IsDisposed)
                        {
                            // 创建并显示新的ChartForm
                            chartFormForCurrent = new ChartForm(xValues, values.ToArray(), serialNo);
                            chartFormForCurrent.Show();
                        }
                        else
                        {
                            // 更新现有的ChartForm的数据
                            chartFormForCurrent.UpdateChart(values.ToArray(), serialNo);
                        }*/
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

        /*private ChartForm chartFormForVoltage;*/
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

                    List<double> values = new List<double>();
                    List<double> durations = new List<double>();
                    double[] xValues = new double[2];

                    // 从 Excel 文件中加载相应的电压值
                    List<(double currentValue, double duration)> vals = LoadVoltageValuesFromExcel(serialNo);

                    foreach (var (vol, duration) in vals)
                    {
                        values.Add(vol);
                        durations.Add(duration);
                        //Console.WriteLine($"Current Value: {vol}, Duration: {duration}");
                    }


                    if (durations.Count > 0)
                    {
                        xValues[0] = 0.0;
                        xValues[1] = durations.Distinct().ToList()[0];
                    }

                    if (values.Count > 0)
                    {
                        ChartForm chartFormForVoltage = new ChartForm(/*xValues, */values.ToArray(), serialNo);
                        chartFormForVoltage.Show();
                        /*if (chartFormForVoltage == null || chartFormForVoltage.IsDisposed)
                        {
                            // 创建并显示新的ChartForm
                            chartFormForVoltage = new ChartForm(*//*xValues, *//*values.ToArray(), serialNo);
                            chartFormForVoltage.Show();
                        }
                        else
                        {
                            // 更新现有的ChartForm的数据
                            chartFormForVoltage.UpdateChart(*//*xValues, *//*values.ToArray(), serialNo);
                        }*/
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

        private List<(double currentValue, double duration)> LoadCurrentValuesFromExcelForSheet2(string serialNo)
        {
            var curs = new List<(double currentValue, double duration)>();

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
                                            ICell durationCell = row.GetCell(2);

                                            if (currentValueCell != null && durationCell != null &&
                                                double.TryParse(currentValueCell.ToString(), out double currentValue) &&
                                                double.TryParse(durationCell.ToString(), out double duration))
                                            {
                                                curs.Add((currentValue, duration));
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

            return curs;
        }

        private List<(double currentValue, double duration)> LoadVoltageValuesFromExcel(string serialNo)
        {
            List<(double currentValue, double duration)> voltageValues = new List<(double currentValue, double duration)>();

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
                                            ICell durationCell = row.GetCell(2);
                                            if (voltageValueCell != null && durationCell != null
                                                && double.TryParse(voltageValueCell.ToString(), out double voltageValue)
                                                && double.TryParse(durationCell.ToString(), out double duration))
                                            {
                                                voltageValues.Add((voltageValue, duration));
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

        private Queue<(int start, int duration)> LoadTimerData(string inputDataTime)
        {
            Queue<(int start, int duration)> dataQueue = new Queue<(int, int)>();

            dataQueue.Clear();

            // 去掉所有空格
            inputDataTime = inputDataTime.Replace(" ", "");

            // 按照 "),(" 分割字符串
            var pairs = inputDataTime.Split(new[] { "),(" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                // 去掉可能的前后括号
                var cleanedPair = pair.Trim('(', ')');

                if (IsValidInput(cleanedPair))
                {
                    var numbers = cleanedPair.Split(',');
                    int start = int.Parse(numbers[0]);
                    int duration = int.Parse(numbers[1]);
                    dataQueue.Enqueue((start, duration));
                }
                else
                {
                    MessageBox.Show($"无效的输入: ({cleanedPair})");
                    //MessageBox.Show($"Invalid input format: ({cleanedPair})");
                    return null;
                }
            }
            return dataQueue;
        }

        // 验证输入格式的简单方法
        private bool IsValidInput(string input)
        {
            // 使用正则表达式验证格式 500,1000
            return Regex.IsMatch(input, @"^\d+,\d+$");
        }

        private void myBtn_Click(object sender, EventArgs e)
        {
            #region 本地调试
            //MultimeterDetection test = new MultimeterDetection(this.dataGridView1);
            //test.TestData();

            //PLCDetection testPlc = new PLCDetection(this.dataGridView2);
            //testPlc.TestData();
            #endregion

            MyButton button = sender as MyButton;

            #region PLC调试
            InputData inputData = new InputData();
            inputData.K = InvalidateParamsForInt(this.k_value.Text.Trim());
            inputData.Num = InvalidateParamsForInt(this.n_value.Text.Trim());

            if (plcDetection != null)
            {
                plcDetection.SetPlaying(true);
                plcDetection.SetInputDate(inputData);
                _ = plcDetection.PlcListenerHandler();
            }
            #endregion

            #region 万用表调试
            // 1. 参数校验
            /*InputData inputData = new InputData();
            inputData.K = InvalidateParamsForInt(this.k_value.Text.Trim());
            inputData.Num = InvalidateParamsForInt(this.n_value.Text.Trim());
            inputData.DataQueue = LoadTimerData(inputTextBox.Text.Trim());
            if (inputData.DataQueue == null || inputData.DataQueue.Count <= 0)
            {
                MessageBox.Show("15组时间数据未设置，请先设置时间数据！！！");
                button.IsPlaying = true;
                return;
            }
            if (multimeterDetection != null)
            {
                multimeterDetection.SetInputDate(inputData);
                multimeterDetection.SetIsPlaying(true);
                _ = multimeterDetection.MultimeterListenerHandler();
            }*/
            #endregion


            #region 正式环境
            // 要两个服务端都连接上才能进行开始采集数据
            /*if (multimeterDetection.MultimerOpen() && plcDetection.PlcOpen())
            {
                if (button.IsPlaying)
                {
                    //MessageBox.Show("暂停");
                    this.myLED3.IsFlash = false;
                    this.myLED3.LedStatus = true;
                    this.myLED3.LedTrueColor = Color.Green;

                    if (plcDetection != null)
                    {
                        plcDetection.SetPlaying(false);
                    }

                    if (multimeterDetection != null)
                    {
                        multimeterDetection.SetIsPlaying(false);
                    }
                }
                else
                {
                    //MessageBox.Show("开始");
                    this.myLED3.IsFlash = true;
                    this.myLED3.LedStatus = true;
                    this.myLED3.LedTrueColor = Color.Green;

                    // 1. 参数校验
                    InputData inputData = new InputData();
                    inputData.K = InvalidateParamsForInt(this.k_value.Text.Trim());
                    inputData.Num = InvalidateParamsForInt(this.n_value.Text.Trim());
                    inputData.DataQueue = LoadTimerData(inputTextBox.Text.Trim());
                    if (inputData.DataQueue == null || inputData.DataQueue.Count <= 0)
                    {
                        MessageBox.Show("15组时间数据未设置，请先设置时间数据！！！");
                        button.IsPlaying = true;
                        return;
                    }

                    if (multimeterDetection != null)
                    {
                        multimeterDetection.SetInputDate(inputData);
                        multimeterDetection.SetIsPlaying(true);
                        _ = multimeterDetection.MultimeterListenerHandler();
                    }

                    if (plcDetection != null)
                    {
                        plcDetection.SetPlaying(true);
                        plcDetection.SetInputDate(inputData);
                        _ = plcDetection.PlcListenerHandler();
                    }
                }
            }*/
            #endregion
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
            if (ValidateInput(input))
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
                MessageBox.Show("请输入正确格式的数据，例如：(500,1000),(200,8000)");
            }
        }

        private void connBtn_Click(object sender, EventArgs e)
        {
            string plcIpAddress = this.plcIp.IpAddress;
            string multimerIpAddress = this.pmultimerIp.IpAddress;
            Console.WriteLine($"IP Address: {plcIpAddress}, {multimerIpAddress}");

            if (string.IsNullOrWhiteSpace(plcIpAddress) || !IsValidIp(plcIpAddress))
            {
                MessageBox.Show("请输入正确的PLC IP地址！！！");
                return;
            }

            if (string.IsNullOrWhiteSpace(multimerIpAddress) || !IsValidIp(multimerIpAddress))
            {
                MessageBox.Show("请输入正确的仪器IP地址！！！");
                return;
            }

            #region TODO 万用表调试
            /*multimeterDetection = new MultimeterDetection(this.myLED2, this.dataGridView1, multimerIpAddress);
            if (multimeterDetection.MultimerOpen())
            {
                this.multimerLabel.Text = "万用表已连接!";
                this.multimerLabel.ForeColor = Color.DarkOliveGreen;
                // 连接成功之后，立即下发初始化指令
                multimeterDetection.SendConfigCommand();
            }*/
            #endregion

            #region TODO PLC 调试
            multimeterDetection = new MultimeterDetection(this.myLED2, this.dataGridView1);

            plcDetection = new PLCDetection(plcIpAddress, multimeterDetection, this.myLED1, this.dataGridView2);
            bool a = plcDetection.PlcOpenAsync().Result;
            if (a)
            {
                this.plcLabel.Text = "PLC已连接!";
                this.plcLabel.ForeColor = Color.DarkOliveGreen;
            }
            #endregion

            #region 正式环境
            /*multimeterDetection = new MultimeterDetection(this.myLED2, this.dataGridView1, multimerIpAddress);
            if (multimeterDetection.MultimerOpen())
            {
                this.multimerLabel.Text = "万用表已连接!";
                this.multimerLabel.ForeColor = Color.DarkOliveGreen;
                // 连接成功之后，立即下发初始化指令
                multimeterDetection.SendConfigCommand();
            }

            plcDetection = new PLCDetection(plcIpAddress, multimeterDetection, this.myLED1, this.dataGridView2);
            bool a = plcDetection.PlcOpenAsync().Result;
            if (a)
            {
                this.plcLabel.Text = "PLC已连接!";
                this.plcLabel.ForeColor = Color.DarkOliveGreen;
            }*/
            #endregion
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

    }
}
