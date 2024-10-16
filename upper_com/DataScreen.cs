using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace upper_com
{
    public partial class DataDetection : Form
    {
        public DataDetection()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
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
                warningMsg.Text = "开始监听并处理数据！！！！！!" + "\r\n";
                this.startBtn.Enabled = false;
            }
            catch (Exception ex)
            {
                // txtMsg.AppendText("服务端启动服务失败!" + "\r\n");
                // this.btnServerConn.Enabled = true;
            }
        }

        //创建一个负责和客户端通信的套接字 
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
                    Console.WriteLine(GetCurrentTime()+ ": 接受到客户端数据：" + clientMsg);
                    // 发送数据
                    string sendMsg = "服务端返回信息:" + clientMsg;
                    socketServer.Send(PackData(sendMsg));

                    //Console.WriteLine("=======泵不住啦！！！！！！！！" + length);
                }
                catch (Exception ex)
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

        private void btnStartClick(object sender, EventArgs e)
        {
            // 1. 参数校验
            invalidateParams();

            // warningMsg.Text = "Hello World!";

            // 2. TODO 数据监听并处理
            this.listenerHandler();
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
                MessageBox.Show("k值输入错误，请输入大于0的十进制有效数字！");
            }
            if (string.IsNullOrEmpty(nVal)
                || !int.TryParse(nVal, out _)
                || int.Parse(nVal) <= 0)
            {
                MessageBox.Show("N值输入错误，请输入大于0十进制有效数字！");
            }
        }

        /// <summary>
        /// IP 校验接口
        /// </summary>
        /// <returns>IP是否合法：true-合法；false-不合法</returns>
        // 
        public bool IsValidIPAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }
        
        /// <summary>
        /// 端口号校验接口
        /// </summary>
        /// <returns>端口是否合法</returns>
        // 
        public bool IsValidPort(string portString)
        {
            if (int.TryParse(portString, out int port))
            {
                return port >= 0 && port <= 65535;
            }
            return false;
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void k_value_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEndClick(object sender, EventArgs e)
        {
            // MessageBox.Show("数据已经停止采集，采集数据记录在 D://upper//dataDetect.excl");
            dataFilling();
        }

        /// <summary>
        /// TODO 数据填充
        /// </summary>
        /// <returns>TODO</returns>
        private void dataFilling()
        {
            for(int i=0; i<100; i++)
            {
                AddItem(new CurrentData(i, i + "5", 3.2, 3.2, 3.2, 3.2, 3.2, 3.2, 3.2, 3.2));
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
            dgvr.Cells[1].Value = currentData.GetTimer();
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


    }
}
