using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using S7.Net.Types;
using DateTime = System.DateTime;
using System.Windows.Forms;
using Ivi.Visa;
using NationalInstruments.Visa;
using System.IO;
using System.Diagnostics;
using System.Timers;
using System.Linq;
using Org.BouncyCastle.Bcpg.Sig;
using NPOI.POIFS.Crypt.Dsig;

namespace upper_com
{
    internal class MultimeterDetection
    {
        private string multimerIp;

        private MyLED myLED2;

        private DataGridView dataGridView1;

        private bool isPlaying;

        private InputData inputData;

        Queue<(int start, int duration)> timeQueue;

        private bool isCollectingData; // 控制数据采集的标志

        private int currentSerialNo; // 电流测试编号

        private CurrentDataQueue currentDataQueue;

        private VisaClient visaClient;

        public double T {  get; set; }

        public MultimeterDetection()
        {

        }

        public MultimeterDetection(DataGridView dataGridView1)
        {
            this.dataGridView1 = dataGridView1;
        }

        public MultimeterDetection(MyLED myLED2, DataGridView dataGridView1, string multimerIp)
        {
            this.multimerIp = multimerIp;
            this.myLED2 = myLED2;
            this.dataGridView1 = dataGridView1;

            visaClient = new VisaClient(this.multimerIp);
        }

        public void SetInputDate(InputData data)
        {
            this.inputData = data;
        }

        public InputData GetInputData()
        {
            return this.inputData;
        }

        public void SetIsPlaying(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public bool GetIsPlaying()
        {
            return this.isPlaying;
        }

        public void SetSignal(bool signal)
        {
            this.isCollectingData = signal;
        }

        public void SetCurrentSerialNo(int serialNo)
        {
            this.currentSerialNo = serialNo;
        }


        public bool MultimerOpen()
        {
            if (visaClient.Connected)
            {
                UpdateLEDStatus(Color.Green);
                return true;
            }
            else
            {
                UpdateLEDStatus(Color.DimGray);
                return false;
            }
        }

        public void SendConfigCommand()
        {
            try
            {
                // 检查连接是否成功
                if (visaClient.Connected)
                {
                    // 发送查询命令

                    /*// 设置挡位，自动调整量程
                    mbSession.RawIO.Write("CONF:CURR:DC AUTO");
                    // 如果需要确认命令执行，可以发送查询命令
                    mbSession.RawIO.Write("*OPC?\n"); // 查询操作完成
                    string responseConfig = mbSession.RawIO.ReadString();
                    if (responseConfig.Equals("1"))
                    {
                        // 设置触发源为立即触发
                        mbSession.RawIO.Write("TRIG:SOUR IMM");
                    }*/

                    visaClient.Write("FUNCtion \"CURR:DC\"");
                    visaClient.Write("CURRent:DC:RANGe 0.2");
                    visaClient.Write("CURRent:DC:NPLC F");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送配置命令时出错: {ex.Message}");
            }
        }

        public async Task MultimeterListenerHandler()
        {
            // TODO 调试万用表测试
            if (visaClient.Connected && this.isPlaying)
            {
                await SendReadCommandAsync();
            }
            else
            {
                Console.WriteLine("数据采集停止，什么都不做！！！！！！！！");
            }
        }

        public async Task SendReadCommandAsync()
        {
            try
            {
                if (visaClient.Connected)
                {
                    /*visaClient.Write("FUNCtion \"CURR:DC\"");
                    visaClient.Write("CURRent:DC:RANGe 0.2");
                    visaClient.Write("CURRent:DC:NPLC F");*/

                    // 如果需要确认命令执行，可以发送查询命令
                    /*visaClient.Write("*OPC?\n"); // 查询操作完成
                    string responseInit = visaClient.Read();*/

                    await FetchgingData();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command or receiving data: {ex.Message}");
            }

        }

        private async Task FetchgingData()
        {
            if (inputData != null && inputData.DataQueue.Count > 0)
            {
                currentDataQueue = new CurrentDataQueue(this.dataGridView1, inputData.Num, inputData.K); // 初始化 DataQueue

                DateTime start;
                DateTime end;

                // 循环检查收集数据的状态，此状态由PLC设置
                while (isCollectingData && visaClient.Connected)
                {
                    // TODO 调试使用，真正需要放在收到起始信号位置开始
                    start = DateTime.Now;

                    timeQueue = new Queue<(int start, int duration)>(inputData.DataQueue);

                    // 循环遍历15组时间设置
                    while (timeQueue.Count > 0 && isCollectingData)
                    {
                        List<double> stableData = new List<double>();
                        List<double> mutationData = new List<double>();

                        CurrentData all = new CurrentData();

                        var data = timeQueue.Dequeue();

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start(); // 重置并启动计时器

                        // Console.WriteLine("flag1=" + stopwatch.ElapsedMilliseconds + "start: " + data.start + ", duration: " + data.duration);

                        // 收集平稳段数据
                        while (stopwatch.ElapsedMilliseconds <= (data.start + data.duration) && isCollectingData)
                        {
                            if (stopwatch.ElapsedMilliseconds >= data.start)
                            {
                                visaClient.Write("CREAD?");
                                string response = visaClient.Read();
                                if (!string.IsNullOrEmpty(response) && double.TryParse(response, out double value))
                                {
                                    stableData.Add(value * 3000);
                                }
                                Console.WriteLine("Stable Phase Data: " + response);
                            }
                        }

                        // 收集突变段数据
                        while (isCollectingData)
                        {
                            if (stopwatch.ElapsedMilliseconds >= data.start + data.duration)
                            {
                                visaClient.Write("CREAD?");
                                string response = visaClient.Read();
                                if (!string.IsNullOrEmpty(response) && double.TryParse(response, out double value))
                                {
                                    mutationData.Add(value * 3000);
                                }
                                Console.WriteLine("Mutation Phase Data: " + response);
                            }
                        }

                        end = DateTime.Now;
                        stopwatch.Stop();

                        double duration = (end - start).TotalSeconds;
                        all.totalDuration = duration;
                        T = duration;

                        // 计算平稳段和突变段的平均值
                        double stableAverage = stableData.Count > 0 ? stableData.Average() : 0;
                        double mutationAverage = mutationData.Count > 0 ? mutationData.Average() : 0;

                        // 将所有数据都添加到 DataQueue
                        all.stableList = stableData;
                        all.mutationList = mutationData;
                        all.serialNo = this.currentSerialNo;
                        currentDataQueue.AddData(stableAverage, mutationAverage, all);
                    }
                }
            }
        }

        public async Task TestData()
        {
            currentDataQueue = new CurrentDataQueue(this.dataGridView1, 20, 3);
            List<double> stableData = new List<double>();
            List<double> mutationData = new List<double>();

            CurrentData all = new CurrentData();

            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);
            stableData.Add(0.99877156E-03 * 3000);

            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);
            mutationData.Add(1.99877156E-03 * 3000);

            all.totalDuration = 2.0;
            T = 2.0;

            // 计算平稳段和突变段的平均值
            double stableAverage = stableData.Count > 0 ? stableData.Average() : 0;
            double mutationAverage = mutationData.Count > 0 ? mutationData.Average() : 0;

            // 将所有数据都添加到 DataQueue
            all.stableList = stableData;
            all.mutationList = mutationData;
            all.serialNo = 1001;
            currentDataQueue.AddData(stableAverage, mutationAverage, all);
        }

        private void UpdateLEDStatus(Color color)
        {
            this.myLED2.IsFlash = false;
            this.myLED2.LedStatus = true;
            this.myLED2.LedTrueColor = color;
        }
    }

}
