using System;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;
using NLog;
using System.IO;

namespace upper_com
{
    internal static class UpperDetectionMain
    {

        #region 防止应用程序重复打开
        static Mutex mutex = new Mutex(true, "{FFAB7D56-89DB-4059-8465-4EB852326633-DemoVision}");
        #endregion

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 指定日志文件夹路径
            string logDirectory = @"D:\upperCom\logs\";

            // 检查并创建日志目录
            EnsureLogDirectoryExists(logDirectory);

            #region 防止应用程序重复打开
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DataDetection());
                Logger.Info("程序启动！！！！！！！");
            }
            else
            {
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOW, IntPtr.Zero, IntPtr.Zero);
            }
            #endregion
        }

        private static void EnsureLogDirectoryExists(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        #region 防止应用程序重复打开
        internal class NativeMethods
        {
            public const int HWND_BROADCAST = 0xffff;
            public static readonly int WM_SHOW = RegisterWindowMessage("WM_SHOWME");
            [DllImport("user32")]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
            [DllImport("user32")]
            public static extern int RegisterWindowMessage(string message);
        }
        #endregion
    }
}
