using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{
    internal class S7NetClient
    {
        Plc plc;

        private string plcIp;

        private CancellationTokenSource cancellationTokenSource;

        public bool Connected { get; set; }

        /// <summary>
        /// 初始化
        /// </summary> 
        public S7NetClient(string plcIp)
        {
            this.plcIp = plcIp;
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 打开连接，设置监控值
        /// </summary>
        /// <param name="MonitorData">开始地址，长度</param>
        /// <returns></returns>
        public async Task<bool> OpenAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    plc = new Plc(CpuType.S71200, this.plcIp, 0, 1);
                    plc.Open();
                    Connected = true;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PLC连接失败！！！发生错误: {ex}" + ", 请检查设备和网络情况！！！");
                    // 等待一段时间后重试连接
                    await Task.Delay(5000, cancellationToken); // 每5秒重试一次，支持取消
                }
            }
            return false;
        }

        public string Close()
        {
            try
            {
                plc.Close();
                Connected = false;
                return "PLC已断开";
            }
            catch (Exception ex)
            {
                Console.WriteLine("lxcecho: " + ex);
                return ex.Message;
            }
        }

        public bool Read(int db, int startAdd, int len, out byte[] deviceValue)
        {
            deviceValue = new byte[len];
            try
            {
                deviceValue = plc.ReadBytes(DataType.DataBlock, db, startAdd, len);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("lxcecho: " + ex);
                Connected = false;
                return false;
            }
        }

        public object Read(string addr)
        {
            try
            {
                object result = plc.Read(addr);
                Console.WriteLine("lxcecho: " + result);
                return result;
            }
            catch (Exception ex)
            {
                Connected = false;
                Console.WriteLine("lxcecho: " + ex);
            }
            return null;
        }

        public string Write(string deviceName, object deviceValue)
        {
            try
            {
                plc.Write(deviceName, deviceValue);
                return "0";
            }
            catch (Exception ex)
            {
                Console.WriteLine("lxcecho: " + ex);
                return "-1";
            }
        }

        public void CancelReconnect()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
