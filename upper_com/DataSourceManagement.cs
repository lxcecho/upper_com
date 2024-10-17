using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{
    class DataSourceManagement
    {

        public static void ConnDatabase()
        {
            string mysqlcon = "server=localhost;database=demo;user=root;password=Amecho00#";

            // 第三步：MySQL创建连接对象
            MySqlConnection con = new MySqlConnection(mysqlcon);

            //第四步：打开连接
            con.Open();

            //第五步：检测是否连接成功，进行连接成功输出
            Console.WriteLine("连接成功");

            //第六步：关闭连接
            con.Close();
        }

        public static void NewDatabase(string dbSource, string username, string passwd, string dbName)
        {
            // 创建连接字符串 con
            MySqlConnection con = new MySqlConnection(
                "Data Source=" + dbSource + ";Persist Security Info=yes;UserId="
                + username + "; PWD=" + passwd + ";");
            // 创建数据库的执行语句
            MySqlCommand cmd = new MySqlCommand("CREATE DATABASE " + dbName, con);
            con.Open();
            // 执行语句
            try
            {
                int res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
            }
        }

        public static void NewCurrentDatatable(string dbSource, string username,
            string passwd, string dbName, string tbName, DataGridView dataGrid)
        {
            // 获取 datagridview 中的数据
            StringBuilder sb = new StringBuilder();
            string colHeader0 = dataGrid.Columns[0].Name;
            sb.Append(colHeader0 + " int(11) NOT NULL AUTO_INCREMENT,");
            string colHeader1 = dataGrid.Columns[1].Name;
            sb.Append(colHeader1 + " TEXT(20),");
            string colHeader2 = dataGrid.Columns[2].Name;
            sb.Append(colHeader2 + " double,");
            string colHeader3 = dataGrid.Columns[3].Name;
            sb.Append(colHeader3 + " double,");
            string colHeader4 = dataGrid.Columns[4].Name;
            sb.Append(colHeader4 + " double,");
            string colHeader5 = dataGrid.Columns[5].Name;
            sb.Append(colHeader4 + " double,");
            string colHeader6 = dataGrid.Columns[6].Name;
            sb.Append(colHeader4 + " double,");
            string colHeader7 = dataGrid.Columns[7].Name;
            sb.Append(colHeader4 + " double,");
            string colHeader8 = dataGrid.Columns[8].Name;
            sb.Append(colHeader4 + " double,");
            string colHeader9 = dataGrid.Columns[9].Name;
            sb.Append(colHeader4 + " double,");

            string tbString = sb.ToString();

            // CREATE TABLE mytable (name VARCHAR(20), sex CHAR(1), birth DATE, birthaddr VARCHAR(20));

            // 创建连接字符串 con
            MySqlConnection con = new MySqlConnection("Data Source=" + dbSource
                + ";Persist Security Info=yes;UserId=" + username + "; PWD=" + passwd + ";");

            string tablecmd = "USE " + dbName + "; CREATE TABLE " + tbName + " ";
            string tableText = "(" + tbString + "PRIMARY KEY(" + colHeader0 + "));";
            string newTableCMD = tablecmd + tableText;  // + tbName + tableText;
            MySqlCommand cmd = new MySqlCommand(newTableCMD, con);
            con.Open();
            int res = cmd.ExecuteNonQuery();
            con.Close();
        }

        /**
         * 往数据库中插入数据
         */
        public static void InsertData(string dbSource, string username,
            string passwd, string dbName, CurrentData cur)
        {
            // 创建连接字符串 con
            MySqlConnection con = new MySqlConnection("Data Source=" + dbSource
                + ";Persist Security Info=yes;UserId=" + username + "; PWD=" + passwd + ";");

            // 打开数据库
            string tablecmd = "USE " + dbName + ";";
            MySqlCommand cmd = new MySqlCommand(tablecmd, con);
            con.Open();
            int res = cmd.ExecuteNonQuery();

            // INSERT INTO current_data (timer, smooth_cur, smooth_average, smooth_upper, smooth_lower, mutation_cur, mutation_average, mutation_upper, mutation_lower)
            // VALUES('hhhhhhhhh', 3, 4, 5, 3.4, 55.3, 34.5, 34.5, 3.45);
            string sql = "INSERT INTO current_data " +
                "(timer, smooth_cur, smooth_average, smooth_upper, smooth_lower, mutation_cur, mutation_average, mutation_upper, mutation_lower) VALUES("
                + "'" + cur.GetTimer() + "', " + cur.GetSmoothCur() + "', " + cur.GetSmoothAverage() + "', " + cur.GetSmoothUpper()
                + "', " + cur.GetSmoothLower() + "', " + cur.GetMutationCur() + "', " + cur.GetMutationAverage() + "', " + cur.GetMutationUpper()
                 + "', " + cur.GetMutationLower() + ");";

            MySqlCommand cmd1 = new MySqlCommand(sql, con);

            int res1 = cmd1.ExecuteNonQuery();

            con.Close();
        }

        public static void ReadDatatable(string dbSource, string dbUid,
            string dbPwd, string dbName, string tbName, DataGridView dataGrid)
        {
            string connString = "server=" + dbSource + "; database="
                + dbName + "; uid=" + dbUid + "; pwd=" + dbPwd + ";Character Set=utf8;";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = conn;
            try
            {
                conn.Open();
                string sql = "select num ,name,gender,age,salary from " + tbName;
                MySqlDataAdapter da = new MySqlDataAdapter(sql, connString);
                DataSet ds = new DataSet();
                da.Fill(ds, tbName);
                dataGrid.DataSource = ds.Tables[tbName];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "操作数据库出错！",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



    }
}
