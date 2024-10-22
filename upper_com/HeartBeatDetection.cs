using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using S7.Net;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace upper_com
{
    internal class HeartBeatDetection
    {
        // 如果所有连接都完成了，就将 LED 置成绿色图标
        public async Task PlcConn()
        {
            // 创建 PLC 连接对象，这里以西门子 S7-1200 为例
            // 指定CPU类型、IP地址和机架、插槽号
            Plc plc = new Plc(CpuType.S7300, "192.168.1.20", 0, 2);

            // 万用表配置
            string multimeterIp = "192.168.1.30"; // 万用表的IP地址
            int multimeterPort = 12345; // 万用表的端口号

            try
            {
                // 打开与 PLC 的连接
                plc.Open();

                if (plc.IsConnected)
                {
                    Console.WriteLine("PLC 连接成功...");

                    while (true)
                    {
                        // 读取起始信号
                        bool startSignal = (bool)plc.Read("DB1.DBX0.0");
                        if (startSignal)
                        {
                            Console.WriteLine("收到起始信号");
                            // 异步处理万用表数据
                            await ProcessMultimeterDataAsync(multimeterIp, multimeterPort);
                        }

                        // 读取终止信号
                        bool stopSignal = (bool)plc.Read("DB1.DBX0.1");
                        if (stopSignal)
                        {
                            Console.WriteLine("收到终止信号");
                            // TODO 停止收集万用表上报数据

                        }

                        // 等待一段时间再检查
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show("PLC 连接失败！！！");
            }
            finally
            {
                // 关闭与 PLC 的连接
                if (plc.IsConnected)
                {
                    plc.Close();
                    Console.WriteLine("PLC 连接关闭...");
                }
            }
        }

        async Task ProcessMultimeterDataAsync(string ip, int port)
        {
            try
            {
                using (TcpClient multimeterClient = new TcpClient(ip, port))
                {
                    NetworkStream stream = multimeterClient.GetStream();
                    string command = "READ?";
                    // 发送 SCPI 指令
                    // 读数据：SendCommand(stream, "READ?");
                    byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\n");
                    await stream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    Console.WriteLine("Sent Command: " + command);

                    // 循环监听数据
                    while (true)
                    {
                        if (stream.DataAvailable)
                        {
                            // 读取万用表返回的数据
                            byte[] responseBytes = new byte[1024];
                            int bytesRead = await stream.ReadAsync(responseBytes, 0, responseBytes.Length);
                            string responseData = Encoding.ASCII.GetString(responseBytes, 0, bytesRead).Trim();
                            Console.WriteLine("Received Data: " + responseData);

                            // TODO 进行计算（示例：假设数据是一个数字）
                            List<double> result = null;

                            // 写入Excel文件
                            await WriteToExcelAsync(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("处理万用表数据时发生异常: " + ex.Message);
            }
        }

        static async Task WriteToExcelAsync(List<double> result)
        {
            string fileName = $"{DateTime.Now:yyyy-MM-dd}.xlsx";
            FileInfo fileInfo = new FileInfo(fileName);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count == 0
                    ? package.Workbook.Worksheets.Add("Data")
                    : package.Workbook.Worksheets[0];

                int row = worksheet.Dimension?.Rows + 1 ?? 1;
                worksheet.Cells[row, 1].Value = DateTime.Now;
                worksheet.Cells[row, 2].Value = result;

                await package.SaveAsync();
                Console.WriteLine("数据已写入Excel文件");
            }
        }

    }
}
