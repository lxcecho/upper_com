﻿using System;
using System.Threading.Tasks;
using S7.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

                    //DebugByByte();
                    DebugBySTR();

                    // 总 T
                    /*all.duration = multimeterDetection.T;
                    double average = voltageList.Average();
                    voltageDataQueue.AddData(average, all);*/
                }
            }
        }

        private void DebugByByte()
        {
            byte[] deviceValue;

            VoltageData all = new VoltageData();

            // Read Int at offset 0.0 (电流测试编号)
            if (s7NetClient.Read(1, 0, 2, out deviceValue))
            {
                int currentTestNumber = BitConverter.ToInt16(deviceValue, 0);
                Console.WriteLine("电流测试编号: " + currentTestNumber);

                all.currentNo = currentTestNumber;
                multimeterDetection.SetCurrentSerialNo(currentTestNumber);
            }

            // Read Bool at offset 2.0 (电流测试开始)
            if (s7NetClient.Read(1, 2, 1, out deviceValue))
            {
                bool currentTestStart = deviceValue[0] != 0;
                Console.WriteLine("电流测试开始: " + currentTestStart);
                all.currentStartSignal = currentTestStart;
                if (currentTestStart)
                {
                    // 收到起始信号
                    multimeterDetection.SetSignal(currentTestStart);
                }
            }

            // Read Bool at offset 2.1 (电流测试结束)
            if (s7NetClient.Read(1, 3, 1, out deviceValue))
            {
                bool currentTestEnd = deviceValue[0] != 0;
                Console.WriteLine("电流测试结束: " + currentTestEnd);
                all.currentEndSignal = currentTestEnd;
                if (currentTestEnd)
                {
                    // TODO 停止收集万用表上报数据
                    multimeterDetection.SetSignal(false);
                }
            }

            // Read Bool at offset 2.2 (压力传送结束)
            if (s7NetClient.Read(1, 4, 1, out deviceValue))
            {
                bool pressureTransferEnd = deviceValue[0] != 0;
                all.voltageTransformSignal = pressureTransferEnd;
                Console.WriteLine("压力传送结束: " + pressureTransferEnd);
            }

            List<double> voltageList = new List<double>();

            // Read Real values at offsets 4.0 to 60.0 (压力当前值1 to 压力当前值15)
            for (int i = 0; i < 15; i++)
            {
                int offset = 8 + i * 4;
                if (s7NetClient.Read(1, offset, 4, out deviceValue))
                {
                    float pressureValue = BitConverter.ToSingle(deviceValue, 0);
                    Console.WriteLine($"压力当前值{i + 1}: " + pressureValue);
                    voltageList.Add(pressureValue);
                }
            }

            // TODO 处理压力数据
            if (voltageList != null && voltageList.Count > 0)
            {
                all.voltageList = voltageList;
            }
        }

        private void DebugBySTR()
        {
            // 读取整数（Int）类型
            int 电流测试记录 = (int)s7NetClient.Read("DB1.DBD0");

            // 读取布尔（Bool）类型
            bool 压力测试标志 = (bool)s7NetClient.Read("DB1.DBX4.0");
            bool 电流测试标志 = (bool)s7NetClient.Read("DB1.DBX4.1");

            // 读取实数（Real）类型
            double 压力测试值 = Convert.ToDouble(s7NetClient.Read("DB1.DBD8"));
            double 电流测试值 = Convert.ToDouble(s7NetClient.Read("DB1.DBD12"));
            double 压力测试值2 = Convert.ToDouble(s7NetClient.Read("DB1.DBD16"));
            double 压力测试值3 = Convert.ToDouble(s7NetClient.Read("DB1.DBD20"));
            double 压力测试值4 = Convert.ToDouble(s7NetClient.Read("DB1.DBD24"));
            double 压力测试值5 = Convert.ToDouble(s7NetClient.Read("DB1.DBD28"));
            double 压力测试值6 = Convert.ToDouble(s7NetClient.Read("DB1.DBD32"));
            double 压力测试值7 = Convert.ToDouble(s7NetClient.Read("DB1.DBD36"));
            double 压力测试值8 = Convert.ToDouble(s7NetClient.Read("DB1.DBD40"));
            double 压力测试值9 = Convert.ToDouble(s7NetClient.Read("DB1.DBD44"));
            double 压力测试值10 = Convert.ToDouble(s7NetClient.Read("DB1.DBD48"));
            double 压力测试值11 = Convert.ToDouble(s7NetClient.Read("DB1.DBD52"));
            double 压力测试值12 = Convert.ToDouble(s7NetClient.Read("DB1.DBD56"));
            double 压力测试值13 = Convert.ToDouble(s7NetClient.Read("DB1.DBD60"));

            // 输出读取的值
            Console.WriteLine($"电流测试记录: {电流测试记录}");
            Console.WriteLine($"压力测试标志: {压力测试标志}");
            Console.WriteLine($"电流测试标志: {电流测试标志}");
            Console.WriteLine($"压力测试值: {压力测试值}");
            Console.WriteLine($"电流测试值: {电流测试值}");
            Console.WriteLine($"压力测试值2: {压力测试值2}");
            Console.WriteLine($"压力测试值3: {压力测试值3}");
            Console.WriteLine($"压力测试值4: {压力测试值4}");
            Console.WriteLine($"压力测试值5: {压力测试值5}");
            Console.WriteLine($"压力测试值6: {压力测试值6}");
            Console.WriteLine($"压力测试值7: {压力测试值7}");
            Console.WriteLine($"压力测试值8: {压力测试值8}");
            Console.WriteLine($"压力测试值9: {压力测试值9}");
            Console.WriteLine($"压力测试值10: {压力测试值10}");
            Console.WriteLine($"压力测试值11: {压力测试值11}");
            Console.WriteLine($"压力测试值12: {压力测试值12}");
            Console.WriteLine($"压力测试值13: {压力测试值13}");
        }

        public async Task TestData()
        {
            voltageDataQueue = new VoltageDataQueue(this.dataGridView2, 20, 3);

            List<double> voltageList = new List<double>();

            VoltageData all = new VoltageData();

            voltageList.Add(0.99877156E-03 * 3000);
            voltageList.Add(0.89877156E-03 * 3000);
            voltageList.Add(0.98877156E-03 * 3000);
            voltageList.Add(0.79877156E-03 * 3000);
            voltageList.Add(0.91877156E-03 * 3000);
            voltageList.Add(0.92877156E-03 * 3000);
            voltageList.Add(0.93877156E-03 * 3000);
            voltageList.Add(0.89877156E-03 * 3000);
            voltageList.Add(0.94877156E-03 * 3000);
            voltageList.Add(0.90877156E-03 * 3000);

            all.duration = 2.0;

            // 计算平稳段和突变段的平均值
            double average = voltageList.Count > 0 ? voltageList.Average() : 0;
            // 将所有数据都添加到 DataQueue
            all.voltageList = voltageList;
            all.currentNo = 1001;
            all.currentStartSignal = true;
            all.currentEndSignal = false;
            all.voltageTransformSignal = true;

            voltageDataQueue.AddData(average, all);
        }
    }
}
