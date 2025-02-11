﻿using Ivi.Visa;
using NationalInstruments.Visa;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{
    internal class VisaClient
    {
        private string multimerIp;

        private static ResourceManager rm;

        private static MessageBasedSession mbSession;

        private static readonly object lockObj = new object();

        private CancellationTokenSource cancellationTokenSource;

        public bool Connected { get; set; }

        public VisaClient(string multimerIp)
        {
            this.multimerIp = multimerIp;
            this.cancellationTokenSource = new CancellationTokenSource();
            InitializeSession(cancellationTokenSource.Token);
        }

        private async void InitializeSession(CancellationToken token)
        {
            if (mbSession == null)
            {
                lock (lockObj)
                {
                    if (rm == null)
                    {
                        rm = new ResourceManager();
                    }
                }

                while (mbSession == null)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested(); // 检查取消请求

                        var resource = $"TCPIP0::{this.multimerIp}::inst0::INSTR";
                        mbSession = (MessageBasedSession)rm.Open(resource);
                        mbSession.TimeoutMilliseconds = 5000; // 设置超时时间
                        Connected = true;
                        Console.WriteLine("万用表连接成功...");
                    }
                    catch (Exception ex)
                    {
                        Connected = false;
                        MessageBox.Show($"万用表连接失败！！！发生错误: {ex}" + ", 请检查设备和网络情况！！！");
                        await Task.Delay(5000, token); // 每5秒重试一次，支持取消
                    }
                }
            }
        }

        public bool Write(string opcode)
        {
            try
            {
                mbSession.RawIO.Write(opcode);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command data: {ex.Message}");
            }
            return false;
        }

        public string Read()
        {
            try
            {
                return mbSession.RawIO.ReadString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving data: {ex.Message}");
            }
            return null;
        }

        public byte[] Read(long count)
        {
            try
            {
                return mbSession.RawIO.Read(count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving data: {ex.Message}");
            }
            return null;
        }

        public void CancelConnection()
        {
            cancellationTokenSource.Cancel(); // 请求取消连接操作
        }
    }
}
