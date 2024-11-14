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

namespace upper_com
{
    internal class MultimeterDetection
    {
        private readonly string ip = "192.168.1.25"; // 万用表IP

        private MyLED myLED2;

        private DataGridView dataGridView1;

        private bool isPlaying;

        private InputData inputData;

        Queue<(int start, int duration)> timeQueue;

        private bool isCollectingData; // 控制数据采集的标志

        private string currentSerialNo; // 电流测试编号

        private CurrentDataQueue currentDataQueue;

        // 静态变量确保只创建一次
        private static ResourceManager rm;
        private static MessageBasedSession mbSession;
        private static readonly object lockObj = new object();

        private CancellationTokenSource cancellationTokenSource;

        public MultimeterDetection(MyLED myLED2, DataGridView dataGridView1)
        {
            this.myLED2 = myLED2;
            this.dataGridView1 = dataGridView1;

            this.cancellationTokenSource = new CancellationTokenSource();
            InitializeSession(cancellationTokenSource.Token);
        }

        private async void InitializeSession(CancellationToken token)
        {
            if (mbSession == null)
            {
                lock (lockObj)
                {
                    if (rm == null)
                    {
                        rm = new ResourceManager();
                    }
                }

                while (mbSession == null)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested(); // 检查取消请求

                        var resource = $"TCPIP0::{ip}::inst0::INSTR";
                        mbSession = (MessageBasedSession)rm.Open(resource);
                        mbSession.TimeoutMilliseconds = 5000; // 设置超时时间

                        Console.WriteLine("连接成功");
                        UpdateLEDStatus(Color.Green);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("连接操作被取消");
                        break; // 退出循环
                    }
                    catch (VisaException ex)
                    {
                        Console.WriteLine("VISA错误: " + ex.Message);
                        UpdateLEDStatus(Color.DimGray);
                        await Task.Delay(1000, token); // 每秒重试一次，支持取消
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("发生错误: " + ex.Message);
                        UpdateLEDStatus(Color.DimGray);
                        await Task.Delay(1000, token); // 每秒重试一次，支持取消
                    }
                }
            }
        }

        public void CancelConnection()
        {
            cancellationTokenSource.Cancel(); // 请求取消连接操作
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

        public async Task SendConfigCommand()
        {
            try
            {
                // 检查连接是否成功
                if (mbSession != null)
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

                    mbSession.RawIO.Write("DISPlay 0");
                    mbSession.RawIO.Write("CURRent:DC:RANGe 0.2");
                    mbSession.RawIO.Write("CURRent:DC:NPLC F");
                }
                else
                {
                    Console.WriteLine("无法连接设备！！！！！！！！！");
                    UpdateLEDStatus(Color.DimGray);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送配置命令时出错: {ex.Message}");
            }
        }

        public void SetSignal(bool signal)
        {
            this.isCollectingData = signal;
        }

        public void SetCurrentSerialNo(string serialNo)
        {
            this.currentSerialNo = serialNo;
        }

        public async Task StartCollectingData()
        {
            // TODO 调试万用表测试
            if (this.isPlaying)
            {
                await SendReadCommandAsync();
            }
            else
            {
                MessageBox.Show("暂不处理！！！！！！！！");
            }
        }

        public async Task SendReadCommandAsync()
        {
            try
            {
                mbSession.RawIO.Write("INIT");
                // 如果需要确认命令执行，可以发送查询命令
                mbSession.RawIO.Write("*OPC?\n"); // 查询操作完成
                string responseInit = mbSession.RawIO.ReadString();
                if (responseInit.Equals("1"))
                {
                    /*mbSession.RawIO.Write("FETCH?\n");
                    string response = mbSession.RawIO.ReadString();
                    Console.WriteLine("Measurement Data: " + response);*/

                    _ = FetchgingData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command or receiving data: {ex.Message}");
            }

        }

        private async Task FetchgingData()
        {
            // 循环检查收集数据的状态，此状态由PLC设置
            while (isCollectingData)
            {
                if (inputData != null && inputData.DataQueue.Count > 0)
                {
                    // TODO 调试使用，真正需要放在收到起始信号位置开始
                    //start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    currentDataQueue = new CurrentDataQueue(this.dataGridView1, inputData.Num, inputData.K); // 初始化 DataQueue
                    timeQueue = new Queue<(int start, int duration)>(inputData.DataQueue);

                    // 循环遍历15组时间设置
                    while (timeQueue.Count > 0 && isCollectingData)
                    {
                        List<double> stableData = new List<double>();
                        List<double> mutationData = new List<double>();

                        AllCurrentData all = new AllCurrentData();

                        var data = timeQueue.Dequeue();
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start(); // 重置并启动计时器

                        Console.WriteLine("flag1=" + stopwatch.ElapsedMilliseconds + "start: " + data.start + ", duration: " + data.duration);

                        // 收集平稳段数据
                        while (stopwatch.ElapsedMilliseconds < (data.start + data.duration) && isCollectingData)
                        {
                            if (stopwatch.ElapsedMilliseconds >= data.start)
                            {
                                //Console.WriteLine("开始采集平稳段数据");
                                mbSession.RawIO.Write("FETCH?\n");
                                string response = mbSession.RawIO.ReadString();
                                if (double.TryParse(response, out double value))
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
                                //Console.WriteLine("开始采集突变段数据");
                                mbSession.RawIO.Write("FETCH?\n");
                                string response = mbSession.RawIO.ReadString();
                                if (double.TryParse(response, out double value))
                                {
                                    mutationData.Add(value * 3000);
                                }
                                Console.WriteLine("Mutation Phase Data: " + response);
                                break;
                            }
                        }

                        stopwatch.Stop();


                        // 计算平稳段和突变段的平均值
                        double stableAverage = stableData.Count > 0 ? stableData.Average() : 0;
                        double mutationAverage = mutationData.Count > 0 ? mutationData.Average() : 0;

                        // 将所有数据都添加到 DataQueue
                        all.StatbleList = stableData;
                        all.MutationList = mutationData;
                        all.SerialNo = this.currentSerialNo;
                        currentDataQueue.AddData(stableAverage, mutationAverage, all);
                    }
                }
                else
                {
                    await Task.Delay(500); // 如果没有数据或未开始采集，稍作延迟
                }
            }
        }

        public async Task TestData()
        {
            currentDataQueue = new CurrentDataQueue(this.dataGridView1, 20, 3);
            List<double> stableData = new List<double>();
            List<double> mutationData = new List<double>();

            AllCurrentData all = new AllCurrentData();

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


            // 计算平稳段和突变段的平均值
            double stableAverage = stableData.Count > 0 ? stableData.Average() : 0;
            double mutationAverage = mutationData.Count > 0 ? mutationData.Average() : 0;

            // 将所有数据都添加到 DataQueue
            all.StatbleList = stableData;
            all.MutationList = mutationData;
            all.SerialNo = "1001";
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
