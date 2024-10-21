using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace upper_com
{
    internal class MultimeterDetect
    {
        private string serverIp;
        private int port; // 万用表的端口号

        private TcpClient client;
        private NetworkStream stream;

        // TODO 服务器
        // TcpListener server = new TcpListener(IPAddress.Any, port);

        public MultimeterDetect(string ipAddress, int port)
        {
            
            try
            {
                client = new TcpClient(ipAddress, port);
                client.Connect(serverIp, port);
                Console.WriteLine("上位机和万用表连接成功！！！！！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("上位机和万用表连接失败：" + ex.Message);
                return;
            }
            stream = client.GetStream();
        }

        public void UpperComHandler(string command)
        {
            
            // 发送 SCPI 指令
            // 读数据：SendCommand(stream, "READ?");
            SendCommand(stream, command);

            try
            {
                // 循环监听数据
                while (true)
                {
                    if (stream.DataAvailable)
                    {
                        DateTime startTime = DateTime.Now;
                        // 接收响应
                        String data = ReceiveResponse(stream);
                        TimeSpan elapsedTime = DateTime.Now - startTime;


                        // TODO 数据解析
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show("程序泵啦！！！");
            }
            finally {
                Close(); 
            }
        }

        // 关闭连接
        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public string ReceiveResponse(NetworkStream stream)
        {
            byte[] responseBytes = new byte[1024];
            int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
            string data = Encoding.ASCII.GetString(responseBytes, 0, bytesRead).Trim();
            Console.WriteLine("Received Data: " + data);
            return data;
        }

        public void SendCommand(NetworkStream stream, string command)
        {
            byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\n");
            stream.Write(commandBytes, 0, commandBytes.Length);
            Console.WriteLine("Sent Command: " + command);
        }

    }
}
