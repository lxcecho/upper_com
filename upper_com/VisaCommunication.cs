using System;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace upper_com
{
    internal class VisaCommunication
    {
        public void Test(string ip)
        {
            try
            {
                // 创建资源管理器
                using (var rm = new ResourceManager())
                {

                    var resource = string.Format("TCPIP0::{0}::inst0::INSTR", ip);
                    var mbSession = (MessageBasedSession)rm.Open(resource);
                    mbSession.RawIO.Write("*IDN?\n");
                    System.Console.WriteLine(mbSession.RawIO.ReadString() +" lxcecho");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误: " + ex.Message);
            }
        }
    }
}
