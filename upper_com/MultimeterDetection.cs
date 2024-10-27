using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace upper_com
{
    internal class MultimeterDetection
    {
        private readonly int port; // 万用表的端口号
        TcpListener server;
        private TcpClient client;
        private MyLED myLED2;
        private bool isPlaying;

        public MultimeterDetection(bool isPlaying)
        {
            this.isPlaying = isPlaying;
        }

        public MultimeterDetection(int port, MyLED myLED2)
        {
            this.port = port;
            // TODO 服务器
            server = new TcpListener(IPAddress.Any, port);
            this.myLED2 = myLED2;
        }

        public async Task StartServer()
        {
            server.Start();
            Console.WriteLine($"服务器已启动，正在监听端口 {port}...");

            try
            {
                // 等待客户端连接
                client = await server.AcceptTcpClientAsync();
                Console.WriteLine("客户端已连接");
                // TODO LED 灯变绿
                this.myLED2.IsFlash = false;
                this.myLED2.LedStatus = true;
                this.myLED2.LedTrueColor = Color.Green;
            }
            catch (Exception ex)
            {
                this.myLED2.IsFlash = false;
                this.myLED2.LedStatus = true;
                this.myLED2.LedTrueColor = Color.DimGray;
                Console.WriteLine($"客户端连接失败: {ex.Message}");
            }

        }

        public async Task SendReadCommandAsync()
        {
            if (client != null && client.Connected)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    byte[] commandBytes = Encoding.ASCII.GetBytes("READ?");
                    await stream.WriteAsync(commandBytes, 0, commandBytes.Length);
                    Console.WriteLine("Sent Command: " + "READ?");

                    // 发送指令后立即接收数据
                    await ReceiveDataAsync(stream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending command or receiving data: {ex.Message}");
                }
            }
        }

        private async Task ReceiveDataAsync(NetworkStream stream)
        {
            try
            {
                while (true)
                {
                    if (this.isPlaying)
                    {
                        if (stream.DataAvailable)
                        {
                            byte[] responseBytes = new byte[1024];
                            int bytesRead = await stream.ReadAsync(responseBytes, 0, responseBytes.Length);
                            string data = Encoding.ASCII.GetString(responseBytes, 0, bytesRead).Trim();
                            Console.WriteLine("Received Data: " + data);

                            // TODO 数据解析


                            // break; // 处理完数据后退出循环
                        }
                    }
                    await Task.Delay(100); // 短暂延迟以避免过度占用CPU
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving data: {ex.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
                this.myLED2.IsFlash = false;
                this.myLED2.LedStatus = true;
                this.myLED2.LedTrueColor = Color.DimGray;
                Console.WriteLine("客户端已断开连接");
            }
        }
    }

}
