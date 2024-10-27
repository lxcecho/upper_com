﻿using System;
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

        private string placeholderText = "用英文逗号分隔，如：0.1,0.5,1.3";

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

            InitializeFileWatcher();

            // 定时器模拟数据变更
            // InitializeDataChangeTimer();

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

            string input = inputTextBox.Text;
            //string[] dataItems = input.Split(',');
            string[] dataItems = new string[15];
            for (int i = 0; i < 15; i++)
            {
                dataItems[i] = input;
            }
            inputTextBox.Text = string.Join(",", dataItems);
            inputTextBox.ForeColor = Color.Black;
        }

        private void InitializeDataChangeTimer()
        {
            dataChangeTimer = new System.Windows.Forms.Timer
            {
                Interval = 10000 // 每10秒模拟一次数据变更
            };
            dataChangeTimer.Tick += (s, e) => CheckForDataChanges();
            dataChangeTimer.Start();
        }

        private void InitializeFileWatcher()
        {
            string directoryPath = @"D:\upperCom";
            // 检查并创建目录
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 监控电流文件
            currentFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(currentFilePath),
                Filter = Path.GetFileName(currentFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            currentFileWatcher.Changed += OnCurrentFileChanged;
            currentFileWatcher.EnableRaisingEvents = true;

            // 监控电压文件
            voltageFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(voltageFilePath),
                Filter = Path.GetFileName(voltageFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            voltageFileWatcher.Changed += OnVoltageFileChanged;
            voltageFileWatcher.EnableRaisingEvents = true;
        }

        private void OnCurrentFileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Delay(100).ContinueWith(_ => LoadLatestCurrentDataToDataGridView());
        }

        private void OnVoltageFileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Delay(100).ContinueWith(_ => LoadLatestVoltageDataToDataGridView());
        }

        private void LoadData()
        {
            LoadLatestCurrentDataToDataGridView();
            LoadLatestVoltageDataToDataGridView();
        }

        // 模拟数据变更
        private void CheckForDataChanges()
        {
            for (int i = 0; i < 10; i++)
            {
                CurrentData cur = new CurrentData(i, DateTime.Now.ToString(), 0.2, 3.02, 3.62, 73.2, 93.2, 113.2, 763.2, 103.2);
                UpdateCurrentData(cur);
            }

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

        private async void UpdateCurrentData(CurrentData currentData)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                // 先更新DataGridView
                this.dataGridView1.Invoke((MethodInvoker)delegate
                {
                    AddCurrentDataToGridView(currentData);
                });
            }
            else
            {
                AddCurrentDataToGridView(currentData);
            }

            // 异步写入文件
            await Task.Run(() => WriteCurrentDataToFile(currentData));
        }

        private void AddCurrentDataToGridView(CurrentData currentData)
        {
            // 更新 DataGridView
            this.dataGridView1.Rows.Add(currentData.GetSerialNo(), currentData.GetCurDate(), currentData.GetSmoothCur(), currentData.GetSmoothAverage(),
                currentData.GetSmoothUpper(), currentData.GetSmoothLower(), currentData.GetMutationCur(), currentData.GetMutationAverage(),
                currentData.GetMutationUpper(), currentData.GetMutationLower());
            if (this.dataGridView1.Rows.Count > 15)
            {
                this.dataGridView1.Rows.RemoveAt(0);
            }
        }

        private void WriteCurrentDataToFile(CurrentData currentData)
        {
            lock (currentFileLock)
            {
                try
                {
                    IWorkbook workbook;
                    ISheet sheet;

                    if (!File.Exists(currentFilePath))
                    {
                        workbook = new XSSFWorkbook();
                        sheet = workbook.CreateSheet("Sheet1");

                        // 写入标题行
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.CreateCell(0).SetCellValue("序号");
                        headerRow.CreateCell(1).SetCellValue("时间");
                        headerRow.CreateCell(2).SetCellValue("电流I1");
                        headerRow.CreateCell(3).SetCellValue("I1均值");
                        headerRow.CreateCell(4).SetCellValue("I1上限");
                        headerRow.CreateCell(5).SetCellValue("I1下限");
                        headerRow.CreateCell(6).SetCellValue("电流I2");
                        headerRow.CreateCell(7).SetCellValue("I2均值");
                        headerRow.CreateCell(8).SetCellValue("I2上限");
                        headerRow.CreateCell(9).SetCellValue("I2下限");
                    }
                    else
                    {
                        // 打开现有文件并追加数据
                        using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            workbook = new XSSFWorkbook(fs);
                            sheet = workbook.GetSheetAt(0);
                        }
                    }

                    // 找到最后一行
                    int lastRowNum = sheet.LastRowNum;
                    IRow newRow = sheet.CreateRow(lastRowNum + 1);
                    // 追加数据
                    newRow.CreateCell(0).SetCellValue(currentData.GetSerialNo());
                    newRow.CreateCell(1).SetCellValue(currentData.GetCurDate());
                    newRow.CreateCell(2).SetCellValue(currentData.GetSmoothCur());
                    newRow.CreateCell(3).SetCellValue(currentData.GetSmoothAverage());
                    newRow.CreateCell(4).SetCellValue(currentData.GetSmoothUpper());
                    newRow.CreateCell(5).SetCellValue(currentData.GetSmoothLower());
                    newRow.CreateCell(6).SetCellValue(currentData.GetMutationCur());
                    newRow.CreateCell(7).SetCellValue(currentData.GetMutationAverage());
                    newRow.CreateCell(8).SetCellValue(currentData.GetMutationUpper());
                    newRow.CreateCell(9).SetCellValue(currentData.GetMutationLower());

                    // 写入文件
                    using (var fs = new FileStream(currentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
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


        // 负责监听客户端的线程
        Thread threadWatch = null;
        // 负责监听客户端的套接字   
        Socket socketWatch = null;

        private void listenerHandler()
        {
            try
            {
                // 定义一个套接字用于监听客户端发来的信息  包含 3 个参数(IP4 寻址协议, 流式连接, TCP 协议)
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 服务端发送信息 需要 1 个 IP 地址和端口号
                // TODO 默认使用 127.0.0.1 65533
                string ip = "127.0.0.1";
                IPAddress ipaddress = IPAddress.Parse(ip);
                // 将 IP 地址和端口号绑定到网络节点 endpoint 上 
                int port = 65533;
                IPEndPoint endpoint = new IPEndPoint(ipaddress, port);
                // 监听绑定的网络节点
                socketWatch.Bind(endpoint);
                // 将套接字的监听队列长度限制为 20
                socketWatch.Listen(20);
                // 创建一个监听线程 
                threadWatch = new Thread(WatchConnecting);
                // 将窗体线程设置为与后台同步
                threadWatch.IsBackground = true;
                // 启动线程
                threadWatch.Start();
                // 启动线程后 warningMsg 显示相应提示
                // warningMsg.Text = "开始监听并处理数据！！！！！!" + "\r\n";
            }
            catch (Exception ex)
            {
                // txtMsg.AppendText("服务端启动服务失败!" + "\r\n");
                // this.btnServerConn.Enabled = true;
            }
        }

        // 创建一个负责和客户端通信的套接字 
        Socket socConnection = null;

        /**
         * 监听客户端发来的请求
         */
        private void WatchConnecting()
        {
            Console.WriteLine("====ffffffffffffffff===");
            // 持续不断监听客户端发来的请求
            while (true)
            {
                // PLC 起始信号
                /*if ()
                {

                }*/

                // 一旦监听到一个客户端的连接，将会创建一个与该客户端连接的套接字
                socConnection = socketWatch.Accept();
                // txtMsg.AppendText("客户端连接成功! " + "\r\n");
                // 创建一个通信线程 
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ServerRecMsg);
                Thread thr = new Thread(pts);
                thr.IsBackground = true;
                // 启动线程
                thr.Start(socConnection);
            }
        }

        /**
         * 接收客户端发来的信息 
         * socketClientPara：客户端套接字对象
         */
        private void ServerRecMsg(object socketClientPara)
        {
            Socket socketServer = socketClientPara as Socket;
            while (true)
            {
                // 创建一个内存缓冲区 其大小为 1024*1024 字节，即1M
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                Console.WriteLine("接受到了客户端：" + socketServer.RemoteEndPoint.ToString() + "连接....");

                //// 平稳段I1数据集合
                //List<double> curren_1 = new List<double>();
                //// 突变段I2数据集合
                //List<double> curren_2 = new List<double>();

                int length = -1;
                try
                {
                    // TODO 将返回的数据进行计算处理
                    //int k = int.Parse(this.k_value.Text.Trim());
                    //// 1. 计算平均值/标准差
                    //double average = curAverageCalculator(curren_1);
                    //double standardDeviation = standardDeviationCalculator(curren_1);
                    //// 2. 上下限
                    //double upperLimit = upperLimitCalculator(average, standardDeviation, k);
                    //double lowerLimit = lowerLimitCalculator(average, standardDeviation, k);

                    //// 3. 判断是否告警
                    ///

                    // 接受客户端数据
                    length = socketServer.Receive(arrServerRecMsg);
                    string clientMsg = AnalyticData(arrServerRecMsg, length);
                    Console.WriteLine(GetCurrentTime() + ": 接受到客户端数据：" + clientMsg);
                    // 发送数据
                    string sendMsg = "服务端返回信息:" + clientMsg;
                    socketServer.Send(PackData(sendMsg));

                    //Console.WriteLine("=======泵不住啦！！！！！！！！" + length);
                }
                catch (Exception)
                {
                    string str = socketServer.RemoteEndPoint.ToString();
                    Console.WriteLine("泵不住啦！！！！！！！！");
                    break;
                }
            }
        }

        /// <summary>
        /// 打包服务器数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>数据包</returns>
        private static byte[] PackData(string message)
        {
            byte[] contentBytes = null;
            byte[] temp = Encoding.UTF8.GetBytes(message);

            if (temp.Length < 126)
            {
                contentBytes = new byte[temp.Length + 2];
                contentBytes[0] = 0x81;
                contentBytes[1] = (byte)temp.Length;
                Array.Copy(temp, 0, contentBytes, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                contentBytes = new byte[temp.Length + 4];
                contentBytes[0] = 0x81;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(temp.Length & 0xFF);
                contentBytes[3] = (byte)(temp.Length >> 8 & 0xFF);
                Array.Copy(temp, 0, contentBytes, 4, temp.Length);
            }
            else
            {
                // 暂不处理超长内容  
            }

            return contentBytes;
        }

        /// <summary>
        /// 解析客户端数据包
        /// </summary>
        /// <param name="recBytes">服务器接收的数据包</param>
        /// <param name="recByteLength">有效数据长度</param>
        /// <returns></returns>
        private static string AnalyticData(byte[] recBytes, int recByteLength)
        {
            if (recByteLength < 2) { return string.Empty; }

            bool fin = (recBytes[0] & 0x80) == 0x80; // 1bit，1表示最后一帧  
            if (!fin)
            {
                return string.Empty;// 超过一帧暂不处理 
            }

            bool mask_flag = (recBytes[1] & 0x80) == 0x80; // 是否包含掩码  
            if (!mask_flag)
            {
                return string.Empty;// 不包含掩码的暂不处理
            }

            int payload_len = recBytes[1] & 0x7F; // 数据长度  

            byte[] masks = new byte[4];
            byte[] payload_data;

            if (payload_len == 126)
            {
                Array.Copy(recBytes, 4, masks, 0, 4);
                payload_len = (UInt16)(recBytes[2] << 8 | recBytes[3]);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 8, payload_data, 0, payload_len);

            }
            else if (payload_len == 127)
            {
                Array.Copy(recBytes, 10, masks, 0, 4);
                byte[] uInt64Bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    uInt64Bytes[i] = recBytes[9 - i];
                }
                UInt64 len = BitConverter.ToUInt64(uInt64Bytes, 0);

                payload_data = new byte[len];
                for (UInt64 i = 0; i < len; i++)
                {
                    payload_data[i] = recBytes[i + 14];
                }
            }
            else
            {
                Array.Copy(recBytes, 2, masks, 0, 4);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 6, payload_data, 0, payload_len);

            }

            for (var i = 0; i < payload_len; i++)
            {
                payload_data[i] = (byte)(payload_data[i] ^ masks[i % 4]);
            }

            return Encoding.UTF8.GetString(payload_data);
        }

        // 计算平均值
        private double curAverageCalculator(List<double> numbers)
        {
            double average = numbers.Average();
            Console.WriteLine($"Average: {average}");
            return average;
        }

        // 计算标准差
        private double standardDeviationCalculator(List<double> numbers)
        {
            double average = curAverageCalculator(numbers);
            double sumOfSquares = numbers.Select(num => Math.Pow(num - average, 2)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquares / numbers.Count);
            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
            return standardDeviation;
        }

        // 计算上下限
        private double upperLimitCalculator(double average, double standardDeviation, double k)
        {
            double upperLimit = average + k * standardDeviation;
            Console.WriteLine($"Upper Limit: {upperLimit}");
            return upperLimit;
        }

        private double lowerLimitCalculator(double average, double standardDeviation, double k)
        {
            double lowerLimit = average - k * standardDeviation;
            Console.WriteLine($"Lower Limit: {lowerLimit}");
            return lowerLimit;
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
        private void invalidateParams()
        {
            string kVal = this.k_value.Text.Trim();
            string nVal = this.n_value.Text.Trim();

            if (string.IsNullOrEmpty(kVal)
                || !int.TryParse(kVal, out _)
                || int.Parse(kVal) <= 0)
            {
                MessageBox.Show("k 值输入错误，请输入大于 0 的十进制有效数字！");
            }
            if (string.IsNullOrEmpty(nVal)
                || !int.TryParse(nVal, out _)
                || int.Parse(nVal) <= 0)
            {
                MessageBox.Show("N值输入错误，请输入大于0十进制有效数字！");
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
            dgvr.Cells[2].Value = currentData.GetSmoothCur();
            dgvr.Cells[3].Value = currentData.GetSmoothAverage();
            dgvr.Cells[4].Value = currentData.GetSmoothUpper();
            dgvr.Cells[5].Value = currentData.GetSmoothLower();
            dgvr.Cells[6].Value = currentData.GetMutationCur();
            dgvr.Cells[7].Value = currentData.GetMutationAverage();
            dgvr.Cells[8].Value = currentData.GetMutationUpper();
            dgvr.Cells[9].Value = currentData.GetMutationLower();
            this.dataGridView1.Rows.Add(dgvr);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void myBtn_Click(object sender, EventArgs e)
        {
            // 1. 参数校验
            invalidateParams();

            MyButton button = sender as MyButton;
            if (button.IsPlaying)
            {
                //MessageBox.Show("暂停");
                this.myLED3.IsFlash = false;
                this.myLED3.LedStatus = true;
                this.myLED3.LedTrueColor = Color.Green;
                new PLCDetection(false);
                new MultimeterDetection(false);
            }
            else
            {
                //MessageBox.Show("开始");
                this.myLED3.IsFlash = true;
                this.myLED3.LedStatus = true;
                this.myLED3.LedTrueColor = Color.Green;
                new PLCDetection(true);
                new MultimeterDetection(true);
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
            MultimeterDetection multimeterDetection = new MultimeterDetection(5525, this.myLED2);
            PLCDetection plcDetection = new PLCDetection(ipAddress, multimeterDetection, this.myLED1);
            plcDetection.PlcConn();
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
