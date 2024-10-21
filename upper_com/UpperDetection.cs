using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DataDetection());
            //int a = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(401) / 5));
            //Console.WriteLine(a);
        }
    }
}
