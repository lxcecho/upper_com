using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{
    internal class S7NetClient
    {
        Plc plc;

        private string plcIp;

        public bool Connected { get; set; }

        /// <summary>
        /// 初始化
        /// </summary> 
        public S7NetClient(string plcIp)
        {
            this.plcIp = plcIp;
        }

        /// <summary>
        /// 打开连接，设置监控值
        /// </summary>
        /// <param name="MonitorData">开始地址，长度</param>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                // 创建 PLC 连接对象，这里以西门子 S7-1200 为例
                // 指定CPU类型、IP地址和机架、插槽号
                // Plc plc = new Plc(CpuType.S71200, plcIp, 0, 1);
                plc = new Plc(CpuType.S71200, this.plcIp, 0, 1);
                plc.Open();
                Connected = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"服务器启动失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("lxcecho: " + ex);
                return false;
            }
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
                Console.WriteLine("lxcecho: " + plc.ReadBytes(DataType.DataBlock, 1, 0, 4));
                return plc.Read(addr);
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
    }
}
