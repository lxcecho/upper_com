using System;
using System.Threading.Tasks;
using S7.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DotNetSiemensPLCToolBoxLibrary.Communication.Library;

namespace upper_com
{
    internal class PLCDetection
    {
        private string plcIp;

        private MultimeterDetection multimeterDetection;

        private MyLED myLED1;

        private bool isPlaying;

        private DataGridView dataGridView2;

        private S7NetClient s7NetClient;

        private VoltageDataQueue voltageDataQueue;

        private InputData inputData;

        public void SetInputDate(InputData data)
        {
            this.inputData = data;
        }

        public InputData GetInputData()
        {
            return this.inputData;
        }

        public void SetPlaying(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public PLCDetection()
        {

        }

        public PLCDetection(DataGridView dataGridView2)
        {
            this.dataGridView2 = dataGridView2;
        }

        public PLCDetection(string plcIp, MultimeterDetection multimeterDetection, MyLED myLED1, DataGridView dataGridView2)
        {
            this.plcIp = plcIp;
            this.multimeterDetection = multimeterDetection;
            this.myLED1 = myLED1;
            this.dataGridView2 = dataGridView2;

            this.s7NetClient = new S7NetClient(this.plcIp);
        }

        public async Task<bool> PlcOpenAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            // 打开与 PLC 的连接
            bool isConnected = await this.s7NetClient.OpenAsync(cancellationTokenSource.Token);

            if (isConnected)
            {
                Console.WriteLine("PLC 连接成功...");
                // TODO LED 灯要变绿
                UpdateLEDStatus(Color.Green);
                return true;
            }
            else
            {
                UpdateLEDStatus(Color.DimGray);
                Console.WriteLine("PLC 连接失败！！！");
                return false;
            }

        }
        private void UpdateLEDStatus(Color color)
        {
            this.myLED1.IsFlash = false;
            this.myLED1.LedStatus = true;
            this.myLED1.LedTrueColor = color;
        }

        public async Task PlcListenerHandler()
        {
            try
            {
                if (this.isPlaying && s7NetClient.Connected)
                {
                    await VoltageDataHandler();
                }
                else
                {
                    Console.WriteLine("PLC数据采集停止，什么都不做！！！！！！！！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show($"数据采集出错了，请检查一下PLC连接！！！！: {ex.Message}");
            }
        }

        private async Task VoltageDataHandler()
        {
            if (inputData != null)
            {
                voltageDataQueue = new VoltageDataQueue(this.dataGridView2, inputData.Num, inputData.K);

                while (this.isPlaying && s7NetClient.Connected)
                {
                    VoltageData all = new VoltageData();

                    // Read Int at offset 0.0 (电流测试编号)
                    int currentTestNumber = s7NetClient.ReadBytesToInt(0, 2);
                    Console.WriteLine("电流测试编号: " + currentTestNumber);
                    all.CurrentNo = currentTestNumber;
                    multimeterDetection.SetCurrentSerialNo(currentTestNumber);

                    // Read Bool at offset 2.0 (电流测试开始)
                    bool currentTestStart = Convert.ToBoolean(s7NetClient.Read("DB33.DBX2.0"));
                    Console.WriteLine("电流测试Start: " + currentTestStart);
                    all.CurrentStartSignal = currentTestStart;
                    if (currentTestStart)
                    {
                        // 收到起始信号
                        multimeterDetection.SetSignal(true);
                    }

                    // Read Bool at offset 2.1 (电流测试结束)
                    bool currentTestEnd = Convert.ToBoolean(s7NetClient.Read("DB33.DBX2.1"));
                    Console.WriteLine("电流测试结束: " + currentTestEnd);
                    all.CurrentEndSignal = currentTestEnd;
                    if (currentTestEnd)
                    {
                        // TODO 停止收集万用表上报数据
                        multimeterDetection.SetSignal(false);
                    }

                    // Read Bool at offset 2.2 (压力传送结束)
                    bool voltageTransferEnd = Convert.ToBoolean(s7NetClient.Read("DB33.DBX2.2"));
                    Console.WriteLine("压力传送结束: " + voltageTransferEnd);
                    all.VoltageTransformSignal = voltageTransferEnd;

                    double[] voltageList = new double[15]; ;

                    // Read Real values at offsets 4.0 to 60.0 (压力当前值1 to 压力当前值15)
                    int startByteAddress = 4; // 压力值1的起始字节地址
                    for (int i = 0; i < 15; i++)
                    {
                        float voltageValue = s7NetClient.ReadBytesToFloat(startByteAddress + i * 4, 4);
                        Console.WriteLine($"压力当前值{i + 1}: " + voltageValue);
                        voltageList[i] = voltageValue;

                    }
                    all.Pressures = voltageList;

                    _ = voltageDataQueue.AddData(all);
                }
            }
        }

        private void DebugBySTR()
        {
            // Read Int at offset 0.0
            object curNo = s7NetClient.Read("DB33.DBD0");
            Console.WriteLine($"电流测试记录: {curNo}");

            // Read Bool at offset 4.0
            object IFlag = s7NetClient.Read("DB33.DBX4.0");
            Console.WriteLine($"电流测试标志: {IFlag}");

            object VFlag = s7NetClient.Read("DB33.DBX4.1");
            Console.WriteLine($"压力测试标志: {VFlag}");

            // Read Real values at offsets 4.0 to 60.0
            for (int i = 0; i < 15; i++)
            {
                object voltageValue = s7NetClient.Read($"DB33.DBD{4 + i * 4}");
                Console.WriteLine($"压力测试值{i + 1}: {voltageValue}");
            }
        }

        public async Task TestData()
        {

            int numberOfQueues = 2;
            List<VoltageDataQueue> queues = new List<VoltageDataQueue>();

            for (int i = 0; i < numberOfQueues; i++)
            {
                queues.Add(new VoltageDataQueue(this.dataGridView2));
            }

            // 模拟数据输入
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                double[] pressures = new double[15];
                for (int j = 0; j < 15; j++)
                {
                    pressures[j] = random.NextDouble() * 100; // 生成随机数据
                }
                VoltageData newData = new VoltageData(1001, true, false, true, pressures);
                Console.WriteLine("voldata=" + newData.ToString());
                _ = queues[i % numberOfQueues].AddData(newData);
            }
        }
    }
}
