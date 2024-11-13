using System;
using System.Threading.Tasks;
using S7.Net;
using System.Windows.Forms;
using System.Drawing;

namespace upper_com
{
    internal class PLCDetection
    {
        private string plcIp;
        private MultimeterDetection multimeterDetection;
        private MyLED myLED1;
        private bool isPlaying;

        public void SetPlaying(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public PLCDetection(string plcIp, MultimeterDetection multimeterDetection, MyLED myLED1)
        {
            this.plcIp = plcIp;
            this.multimeterDetection = multimeterDetection;
            this.myLED1 = myLED1;
        }

        public async Task PlcConn()
        {
            // 创建 PLC 连接对象，这里以西门子 S7-1200 为例
            // 指定CPU类型、IP地址和机架、插槽号
            // Plc plc = new Plc(CpuType.S7300, plcIp, 0, 2);

            using (Plc plc = new Plc(CpuType.S7300, plcIp, 0, 2))
            {
                try
                {
                    // 打开与 PLC 的连接
                    plc.Open();

                    if (plc != null && plc.IsConnected)
                    {
                        Console.WriteLine("PLC 连接成功...");
                        // TODO LED 灯要变绿
                        this.myLED1.IsFlash = false;
                        this.myLED1.LedStatus = true;
                        this.myLED1.LedTrueColor = Color.Green;

                        while (true)
                        {
                            if (this.isPlaying)
                            {
                                // 读取起始信号
                                bool startSignal = (bool)plc.Read("DB1.DBX0.0");
                                if (startSignal)
                                {
                                    Console.WriteLine("收到起始信号");
                                    // 收到起始信号
                                    multimeterDetection.SetSignal(true);
                                }

                                // 读取终止信号
                                bool stopSignal = (bool)plc.Read("DB1.DBX0.1");
                                if (stopSignal)
                                {
                                    Console.WriteLine("收到终止信号");
                                    // TODO 停止收集万用表上报数据
                                    multimeterDetection.SetSignal(false);
                                }

                                // 读取电流测试编号
                                string currentSerialNo = (string)plc.Read("DB1.DBX0.0");
                                multimeterDetection.SetCurrentSerialNo(currentSerialNo);

                                // TODO 处理压力数据
                            }

                            // 等待一段时间再检查
                            await Task.Delay(10);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.myLED1.IsFlash = false;
                    this.myLED1.LedStatus = true;
                    this.myLED1.LedTrueColor = Color.DimGray;
                    Console.WriteLine($"Error: {ex.Message}");
                    MessageBox.Show("PLC 连接失败！！！");
                }
                finally
                {
                    // 关闭与 PLC 的连接
                    if (plc.IsConnected)
                    {
                        this.myLED1.IsFlash = false;
                        this.myLED1.LedStatus = true;
                        this.myLED1.LedTrueColor = Color.DimGray;
                        plc.Close();
                        Console.WriteLine("PLC 连接关闭...");
                    }
                }
            }
        }
    }
}
