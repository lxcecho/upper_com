using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace upper_com
{
    internal static class UpperDetection
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DataDetection());*/

            string mysqlcon = "server=localhost;database=demo;user=root;password=Amecho00#";
            

            // 第三步：MySQL创建连接对象
            MySqlConnection con = new MySqlConnection(mysqlcon);

            //第四步：打开连接
            con.Open();

            //第五步：检测是否连接成功，进行连接成功输出
            Console.WriteLine("连接成功");

            //第六步：关闭连接
            con.Clone();
        }
    }
}
