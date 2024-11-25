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
using System.Collections;
using DotNetSiemensPLCToolBoxLibrary.Communication.Library;
using MathNet.Numerics.Distributions;
using NLog;

namespace upper_com
{
    internal class MultimeterDetection
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private string multimerIp;

        private MyLED myLED2;

        private DataGridView dataGridView1;

        private bool isPlaying;

        private InputData inputData;

        Queue<(int start, int duration)> timeQueue;

        private bool isCollectingData = true; // 控制数据采集的标志

        private int currentSerialNo; // 电流测试编号

        private CurrentDataQueue currentDataQueue;

        private VisaClient visaClient;

        public double T { get; set; }

        public MultimeterDetection()
        {

        }

        public MultimeterDetection(DataGridView dataGridView1)
        {
            this.dataGridView1 = dataGridView1;
        }

        public MultimeterDetection(MyLED myLED2, DataGridView dataGridView1)
        {
            this.myLED2 = myLED2;
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
                    // 发送设置命令
                    visaClient.Write("FUNCtion \"CURR:DC\"");
                    visaClient.Write("CURRent:DC:RANGe 0.2");
                    visaClient.Write("CURRent:DC:NPLC F");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送配置命令时出错: {ex.Message}");
                Logger.Info($"发送配置命令时出错: {ex.Message}");
            }
        }

        public async Task MultimeterListenerHandler()
        {
            try
            {
                // TODO 调试万用表测试
                if (visaClient.Connected)
                {
                    await FetchgingData();
                }
                else
                {
                    Console.WriteLine("万用表数据采集停止，什么都不做！！！！！！！！");
                    Logger.Info("万用表数据采集停止，什么都不做！！！！！！！！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"数据采集出错了，请检查一下设备连接！！！！: {ex.Message}");
                MessageBox.Show($"数据采集出错了，请检查一下设备连接！！！！: {ex.Message}");
            }
        }

        private async Task FetchgingData()
        {
            if (inputData != null && inputData.DataQueue.Count > 0)
            {
                currentDataQueue = new CurrentDataQueue(this.dataGridView1, inputData.Num, inputData.K); // 初始化 DataQueue
            }

            timeQueue = new Queue<(int start, int duration)>(inputData.DataQueue);

            while (this.isPlaying && visaClient.Connected)
            {
                if (!isCollectingData)
                {
                    // 当 isCollectingData 为 false 时，重新赋值 timeQueue
                    timeQueue = new Queue<(int start, int duration)>(inputData.DataQueue);
                    // 可以选择在这里等待 isCollectingData 变为 true
                    await Task.Delay(10); // 延迟以避免过于频繁的检查
                    continue; // 继续下一次循环，等待 isCollectingData 变为 true
                }

                // 循环检查收集数据的状态，此状态由PLC设置
                while (isCollectingData)
                {
                    // TODO 调试使用，真正需要放在收到起始信号位置开始
                    Stopwatch durationWatch = Stopwatch.StartNew();

                    CurrentData all = new CurrentData();

                    // 循环遍历15组时间设置
                    while (timeQueue.Count > 0)
                    {
                        List<List<double>> curData = new List<List<double>>();

                        var data = timeQueue.Dequeue();

                        Stopwatch stopwatch = Stopwatch.StartNew();// 重置并启动计时器

                        // Wait until the start time is reached
                        while (stopwatch.ElapsedMilliseconds < data.start)
                        {
                            if (!isCollectingData)
                            {
                                return; // Exit if data collection is stopped
                            }
                            await Task.Delay(1); // Small delay to prevent busy-waiting
                        }

                        // 从 start 开始一直采集数据

                        while (isCollectingData)
                        {
                            visaClient.Write("CREAD?");
                            byte[] responseBytes = visaClient.Read(10240);

                            if (responseBytes.Length > 0)
                            {
                                string result = Encoding.UTF8.GetString(responseBytes);
                                Console.WriteLine("result: " + result);
                                curData.Add(ProcessingCReadResult(result));
                            }

                            // 收到停止收集信号
                            if (!isCollectingData)
                            //if (stopwatch.ElapsedMilliseconds > 2000)
                            {
                                stopwatch.Stop();
                                // 计时结束
                                double duration = durationWatch.ElapsedMilliseconds / 1000.0; ;
                                durationWatch.Stop();

                                all.Curs.Add(ProcessData(curData, data));
                                all.SerialNo = this.currentSerialNo;

                                break;
                            }
                        }
                    }
                    Console.WriteLine("all Data: " + all);
                    _ = currentDataQueue.AddData(all);
                }
            }
            await Task.Delay(100);
        }

        private (double i1, double i2) ProcessData(List<List<double>> curData, (int start, int duration) data)
        {
            List<double> smoothList = ProcessingCurrentData(curData, (data.duration - data.start) / 1000.0);
            List<double> allList = ProcessingCurrentData(curData, 1.0);

            double stable = smoothList.Count > 0 ? smoothList.Average() : 0;
            double mutation = allList.Max();
            Console.WriteLine("stable: " + stable + ", mutation=" + mutation);
            return (stable, mutation);
        }

        private List<double> ProcessingCReadResult(string result)
        {
            List<double> dataList = new List<double>();
            string[] dataArray = result.Split(',');
            foreach (string item in dataArray)
            {
                if (double.TryParse(item, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double number))
                {
                    dataList.Add(number * 3000.0);
                }
                else
                {
                    Console.WriteLine($"无法解析的项: {item}");
                }
            }
            return dataList;
        }

        private List<double> ProcessingCurrentData(List<List<double>> list, double duration)
        {
            List<double> result = new List<double>();
            int fullLists = (int)Math.Floor(duration); // 完整的列表数
            double remainingDuration = duration - fullLists; // 剩余的时间

            // 处理完整的列表
            for (int i = 0; i < fullLists; i++)
            {
                if (i < list.Count)
                {
                    result.AddRange(list[i]);
                }
            }

            // 处理剩余的部分
            if (fullLists < list.Count)
            {
                double ratio = remainingDuration > 0 ? remainingDuration : 1.0;
                int countToExtract = (int)Math.Ceiling(list[fullLists].Count * ratio);
                result.AddRange(list[fullLists].Take(countToExtract));
            }

            // 输出结果或进一步处理
            /*Console.WriteLine("Processed Data:");
            foreach (var data in result)
            {
                Console.WriteLine(data);
            }*/

            return result;
        }

        public async Task TestData()
        {
            List<CurrentDataQueue> queues = new List<CurrentDataQueue>();
            for (int i = 0; i < 2; i++)
            {
                queues.Add(new CurrentDataQueue(this.dataGridView1, 20, 3));
            }

            // Simulate adding data
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {

                List<(double i1, double i2)> pairs = new List<(double, double)>();

                for (int j = 0; j < 4500; j++)
                {
                    pairs.Add((rand.NextDouble(), rand.NextDouble()));
                }
                CurrentData data = new CurrentData(1001, pairs);
                queues[i % 2].AddData(data);

            }
        }

        private void UpdateLEDStatus(Color color)
        {
            this.myLED2.IsFlash = false;
            this.myLED2.LedStatus = true;
            this.myLED2.LedTrueColor = color;
        }
    }

}
