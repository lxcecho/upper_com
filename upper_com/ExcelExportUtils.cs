using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;// 导出xls格式用HSSF
using NPOI.XSSF.UserModel;// 导出xlsx格式用XSSF
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NPOI.SS.Util;
using System.Collections;
using System.Data;

namespace upper_com
{
    internal class ExcelExportUtils
    {
        /// <summary>
        /// 由DataGridView导出Excel
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sheetName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ExportToExcel(DataGridView grid, string sheetName = "result", string filePath = null)
        {
            if (grid.Rows.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetSaveFilePath();
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = GetIsCompatible(filePath);

            IWorkbook workbook = CreateWorkbook(isCompatible);
            ICellStyle cellStyle = GetCellStyle(workbook);
            ISheet sheet = workbook.CreateSheet(sheetName);


            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(grid.Columns[i].Name);
                cell.CellStyle = cellStyle;
            }

            int rowIndex = 1;
            foreach (DataGridViewRow row in grid.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int n = 0; n < grid.Columns.Count; n++)
                {
                    dataRow.CreateCell(n).SetCellValue((row.Cells[n].Value ?? "").ToString());
                }
                rowIndex++;
            }

            AutoColumnWidth(sheet, headerRow.LastCellNum - 1);


            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();

            sheet = null;
            headerRow = null;
            workbook = null;
            MessageBox.Show("文件： " + filePath + ".xls 保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return filePath;
        }

        /// <summary>
        /// 获取要保存的文件名称（含完整路径）
        /// </summary>
        /// <returns></returns>
        public static string GetSaveFilePath()
        {
            SaveFileDialog saveFileDig = new SaveFileDialog();
            saveFileDig.Filter = "Excel Office97-2003(*.xls)|*.xls|Excel Office2007及以上(*.xlsx)|*.xlsx";
            saveFileDig.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            saveFileDig.FilterIndex = 0;
            saveFileDig.OverwritePrompt = true;
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);// 获取当前系统桌面路径
            saveFileDig.InitialDirectory = dir;
            string filePath = null;
            if (saveFileDig.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDig.FileName;
            }
            return filePath;
        }

        /// <summary>
        /// 打开文件对话框，并返回文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetOpenFilePath()
        {
            //创建对话框的对象
            OpenFileDialog ofd = new OpenFileDialog();
            //设置对话框的标题
            ofd.Title = "请选择要打开的文件";
            //设置对话框可以多选
            ofd.Multiselect = true;
            //设置对话框的初始目录
            ofd.InitialDirectory = @"C:\Users\Administrator\Desktop";
            //设置对话框打开文件的类型
            ofd.Filter = "Excel文件(.xls)|*.xls|Excel文件(.xlsx)|*.xlsx";

            //展示对话框
            ofd.ShowDialog();

            //获得在打开对话框中选中的文件的路径
            string filePath = ofd.FileName;//全路径

            return filePath;
        }

        /// <summary>
        /// 判断Excel文件是否为兼容模式（.xls)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetIsCompatible(string filePath)
        {
            return filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase);
        }



        /// <summary>
        /// 创建工作薄
        /// </summary>
        /// <param name="isCompatible">true就是.xls</param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(bool isCompatible)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook();
            }
            else
            {
                return new XSSFWorkbook();
            }
        }

        /// <summary>
        /// 创建工作薄(依据文件流)
        /// </summary>
        /// <param name="isCompatible"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(bool isCompatible, Stream stream)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook(stream);
            }
            else
            {
                return new XSSFWorkbook(stream);
            }
        }

        #region 传入一个文件路径，返回一个IWorkbook对象

        /// <summary>
        /// 传入一个文件路径，返回一个IWorkbook对象
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static IWorkbook CreateWorkbook(string filepath)
        {
            IWorkbook workbook = null;
            bool isCompatible = GetIsCompatible(filepath);

            using (FileStream fs = File.Open(filepath, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                //把xls文件读入workbook变量里，之后就可以关闭了  
                workbook = CreateWorkbook(isCompatible, fs);
                fs.Close();
            }

            return workbook;
        }

        #endregion

        #region 打开一个excel文件，设置单元格的值，再保存文件

        /// <summary>
        /// 打开一个excel文件，设置单元格的值，再保存文件
        /// </summary>
        /// <param name="ExcelPath"></param>
        /// <param name="sheetname"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetCellValue(String ExcelPath, String sheetname, int column, int row, String value)
        {
            bool returnb = false;
            try
            {
                IWorkbook wk = null;
                bool isCompatible = GetIsCompatible(ExcelPath);
                using (FileStream fs = File.Open(ExcelPath, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite))
                {
                    //把xls文件读入workbook变量里，之后就可以关闭了  
                    wk = CreateWorkbook(isCompatible, fs);
                    fs.Close();
                }
                //把xls文件读入workbook变量里，之后就可以关闭了  

                //ISheet sheet = wk.GetSheet(sheetname);
                ISheet sheet = wk.GetSheetAt(0);
                ICell cell = sheet.GetRow(row).GetCell(column);

                cell.SetCellValue(value);

                using (FileStream fileStream = File.Open(ExcelPath,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    wk.Write(fileStream);
                    fileStream.Close();
                }
                returnb = true;
            }
            catch (Exception)
            {
                returnb = false;
                throw;
            }
            return returnb;
        }

        #endregion

        #region 打开一个文件，读取excel文件某个单元格的值（多少行，多少列）

        /// <summary>
        /// 打开一个文件，读取excel文件某个单元格的值（多少行，多少列）
        /// </summary>
        /// <param name="ExcelPath"></param>
        /// <param name="sheetname"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static String GetCellValue(string ExcelPath, String sheetname, int column, int row)
        {
            String returnStr = null;
            try
            {
                IWorkbook wk = null;
                bool isCompatible = GetIsCompatible(ExcelPath);
                using (FileStream fs = File.Open(ExcelPath, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite))
                {
                    //把xls文件读入workbook变量里，之后就可以关闭了  
                    wk = CreateWorkbook(isCompatible, fs);
                    fs.Close();
                }
                //把xls文件读入workbook变量里，之后就可以关闭了  
                //ISheet sheet = wk.GetSheet(sheetname);
                ISheet sheet = wk.GetSheetAt(0);
                ICell cell = sheet.GetRow(row).GetCell(column);
                returnStr = cell.ToString();
            }
            catch (Exception)
            {
                returnStr = "Exception";
                throw;
            }
            return returnStr;
        }

        #endregion

        #region  打开一个文件，删除多少行以后的数据（是删除，不是清空数据）

        /// <summary>
        /// 打开一个文件，删除多少行以后的数据（是删除，不是清空数据）
        /// </summary>
        /// <param name="fileMatchPath"></param>
        /// <param name="rowIndex">从多少行后开始删除</param>
        public static void DelRowsData(string fileMatchPath, int startRowIndex)
        {
            IWorkbook wk = null;
            bool isCompatible = GetIsCompatible(fileMatchPath);
            using (FileStream fs = File.Open(fileMatchPath, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                //把xls文件读入workbook变量里，之后就可以关闭了  
                wk = CreateWorkbook(isCompatible, fs);
                fs.Close();
            }
            ISheet sheet = wk.GetSheetAt(0);
            for (int i = startRowIndex; i <= sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i) == null)
                {
                    i++;
                    continue;
                }
                sheet.RemoveRow(sheet.GetRow(i));
            }


            //转为字节数组 
            MemoryStream stream = new MemoryStream();
            wk.Write(stream);
            var buf = stream.ToArray();
            //保存为Excel文件  这种方式能保存.xls和.xlsx文件
            using (FileStream fs = new FileStream(fileMatchPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        #endregion



        /// <summary>
        /// 创建表格头单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static ICellStyle GetCellStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;

            return style;
        }

        /// <summary>
        /// 遍历打印二维数组
        /// </summary>
        /// <param name="array"></param>
        public static void PrintTwoArrayTest(object[,] array)
        {
            Console.WriteLine("============测试打印二维数组==============");
            int row = array.GetLength(0);
            int column = array.GetLength(1);
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    if (array[r, c] != null)
                    {
                        string value = array[r, c].ToString();
                        Console.Write($"{value}  |");
                    }

                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 传入2个二维数组，进行条件匹配替换，返回替换后的一个二维数组
        /// </summary>
        /// <param name="refArray">参考的数组</param>
        /// <param name="matchArray">带替换的数组</param>
        /// <param name="refColumn01">参考列1</param>
        /// <param name="refColumn02">参考列2</param>
        /// <param name="refColTarget01">被复制的值的列1</param>
        /// <param name="matchColumn01">带替换的参考列1</param>
        /// <param name="matchColumn02">带替换的参考列2</param>
        /// <param name="matchColTarget01">带粘贴的值的列1</param>
        /// <returns></returns>
        public static string[,] GetMatchArray(string[,] refArray, string[,] matchArray, int refColumn01, int refColumn02, int refColTarget01, int matchColumn01, int matchColumn02, int matchColTarget01)
        {
            Console.WriteLine("============遍历2个二维数,匹配替换==============");
            int row = refArray.GetLength(0);
            int column = matchArray.GetLength(1);
            int row02 = matchArray.GetLength(0);
            int iMatch = 0;
            for (int r = 0; r < row; r++)
            {
                string value01 = refArray[r, refColumn01];//第1列的数据
                string value02 = refArray[r, refColumn02];//第2列的数据
                if (value01 != null && value02 != null)
                {
                    if (value01.Length > 0 | value02.Length > 0)
                    {
                        for (int r02 = 0; r02 < row02; r02++)
                        {
                            string match01 = matchArray[r02, matchColumn01];//第1列的数据
                            string match02 = matchArray[r02, matchColumn02];//第2列的数据
                            if (value01 == match01 && value02 == match02)
                            {
                                matchArray[r02, matchColTarget01] = refArray[r, refColTarget01];
                                iMatch++;
                                Console.WriteLine($"匹配了{iMatch}次");
                            }
                        }
                    }
                }
            }
            return matchArray;
        }

        /// <summary>
        /// 传入2个数组，根据相同条件匹配，吧ref的目标写入match中
        /// </summary>
        /// <param name="refArray">参考的数组</param>
        /// <param name="matchArray">带替换的数组</param>
        /// <param name="refColumn01"></param>
        /// <param name="refColTarget01"></param>
        /// <param name="matchColumn01"></param>
        /// <param name="matchColTarget01"></param>
        /// <returns></returns>
        public static string[,] GetMatchArray(string[,] refArray, string[,] matchArray, int refColumn01, int refColTarget01, int matchColumn01, int matchColTarget01)
        {
            Console.WriteLine("============遍历2个二维数,匹配替换==============");
            int row = refArray.GetLength(0);
            int column = matchArray.GetLength(1);
            int row02 = matchArray.GetLength(0);
            int iMatch = 0;
            for (int r = 0; r < row; r++)
            {
                string value01 = string.Empty;
                value01 = refArray[r, refColumn01];//遍历第一个数组第1列的数据
                //value01 = value01.Trim();
                if (value01 != null)
                {
                    if (value01.Length > 0)
                    {
                        for (int r02 = 0; r02 < row02; r02++)
                        {
                            string match01 = string.Empty;
                            match01 = matchArray[r02, matchColumn01];//遍历第一个数组第1列的数据
                            //match01 = match01.Trim();                                          
                            if (value01 == match01)
                            {
                                matchArray[r02, matchColTarget01] = refArray[r, refColTarget01];
                                iMatch++;
                                Console.WriteLine($"匹配了{iMatch}次");
                            }
                        }
                    }
                }
            }
            return matchArray;
        }

        /// <summary>
        /// 遍历一个数组，如果第二列的数值大于等于第一列的数值，替换字符串
        /// </summary>
        /// <param name="matchArray"></param>
        /// <param name="refColumn01"></param>
        /// <param name="refColumn02"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string[,] GetMatchArray(string[,] matchArray, int refColumn01, int refColumn02, string sValue)
        {
            Console.WriteLine("============遍历2个二维数,匹配替换==============");
            int row = matchArray.GetLength(0);
            int column = matchArray.GetLength(1);

            int iMatch = 0;
            for (int r = 0; r < row; r++)
            {
                string value01 = matchArray[r, refColumn01];//第1列的数据
                string value02 = matchArray[r, refColumn02];//第2列的数据
                try
                {
                    int i01 = Convert.ToInt32(value01);
                    int i02 = Convert.ToInt32(value02);
                    if (i01 >= i02)
                    {
                        matchArray[r, refColumn02] = sValue + $"（数量：{value02}）";
                    }
                }
                catch
                {

                }

            }
            return matchArray;
        }

        #region 打开excel文件，获取某一行的数据

        /// <summary>
        /// 打开excel文件，获取某一行的数据
        /// </summary>
        /// <param name="filepath">文件全路径</param>
        /// <param name="iRow">哪一行的数据</param>
        /// <param name="sheet_Number">哪一个sheet表</param>
        /// <returns></returns>
        public static ArrayList GetRowData(string filepath, int sheet_Number, int iRow)
        {
            ArrayList arrayList = new ArrayList();
            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1);
                IRow currentRow = sheet.GetRow(iRow - 1);
                for (int c = 0; c < currentRow.LastCellNum; c++)
                {
                    //获取每个单元格(r行c列的数据）
                    ICell cell = currentRow.GetCell(c);
                    //获取单元格的内容
                    string value = string.Empty;
                    if (cell != null)
                    {
                        value = cell.ToString(); //如果单元格为空，这里会报错的
                        arrayList.Add(value);
                    }
                }
                return arrayList;
            }
        }

        #endregion

        /// <summary>
        ///  打开excel文件，根据某一行的数据，根据字符串内容，返回这个字符串所在的列的索引
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="iRow"></param>
        /// <param name="sheet_Number">从1开始的</param>
        /// <param name="s1">注意字符串的顺序</param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <returns></returnss
        public static ArrayList GetDataIndexs(string filepath, int sheet_Number, int iRow, string s1, string s2, string s3)
        {
            ArrayList arrayList = new ArrayList();
            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1);
                IRow currentRow = sheet.GetRow(iRow - 1);
                for (int c = 0; c < currentRow.LastCellNum; c++)
                {
                    //获取每个单元格(r行c列的数据）
                    ICell cell = currentRow.GetCell(c);
                    //获取单元格的内容
                    string value = string.Empty;
                    if (cell != null)
                    {
                        value = cell.ToString(); //如果单元格为空，这里会报错的
                        if (value == s1 | value == s2 || value == s3)
                        {
                            arrayList.Add(c);
                        }
                    }
                }
                Console.WriteLine("==========测试打印索引值============");
                foreach (var a in arrayList)
                {
                    Console.WriteLine($"{a} |");
                }
                return arrayList;
            }
        }

        /// <summary>
        /// 打开excel文件，根据某一行的字符串，然后这个字符串所在列的索引
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sheet_Number"></param>
        /// <param name="iRow"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static int GetDataIndex(string filepath, int sheet_Number, int iRow, string sValue)
        {
            int i = 0;
            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1);
                IRow currentRow = sheet.GetRow(iRow - 1);
                for (int c = 0; c < currentRow.LastCellNum; c++)
                {
                    //获取每个单元格(r行c列的数据）
                    ICell cell = currentRow.GetCell(c);
                    //获取单元格的内容
                    string value = string.Empty;
                    if (cell != null)
                    {
                        value = cell.ToString(); //如果单元格为空，这里会报错的
                        if (value == sValue)
                        {
                            i = c;
                        }
                    }
                }
            }
            return i;
        }

        /// <summary>
        /// 打开一个文件，把第几行的数据取出来，返回一个字典 单元格的值：列的索引
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sheet_Number">第几张工作表（从1开始）</param>
        /// <param name="iRow">第几行（从1开始）</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetDataDictionary(string filepath, int sheet_Number, int iRow)
        {
            Dictionary<string, int> DataDict = new Dictionary<string, int>();

            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1);
                IRow currentRow = sheet.GetRow(iRow - 1);
                for (int c = 0; c < currentRow.LastCellNum; c++)
                {
                    //获取每个单元格(r行c列的数据）
                    ICell cell = currentRow.GetCell(c);
                    //获取单元格的内容
                    string value = string.Empty;
                    if (cell != null)
                    {
                        value = cell.ToString(); //如果单元格为空，这里会报错的
                        if (!DataDict.ContainsKey(value))
                        {
                            if (value == "*预计交货日期" | value == "预计交货日期")
                            {
                                value = "*预计交货日期";
                            }

                            DataDict.Add(value, c);
                        }
                        else
                        {
                            if (filepath.Contains("销售订单")) //销售订单模板的第二个备注填写收货地址
                            {
                                if (value == "备注") //如果有两个备注
                                {
                                    //DataDict.Add("采购员", c);
                                    DataDict.Add("收货地址", c);

                                }

                            }

                        }
                    }
                }

                //Console.WriteLine("================开始遍历字典===============");
                //foreach (KeyValuePair<string, int> kv in DataDict)//通过KeyValuePair遍历元素
                //{
                //    Console.WriteLine($"Key:{kv.Key},Value:{kv.Value}");
                //}
                return DataDict;
            }
        }

        /// <summary>
        ///  打开一个文件，根据第几张表第几行的中的两个字符，返回值：一个字典
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sheet_Number"></param>
        /// <param name="strColumnKey"></param>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDataDictionary(string filepath, int sheet_Number, int iRow, string strColumnKey, string strColumnValue)
        {
            Dictionary<string, int> dic = GetDataDictionary(filepath, 1, iRow);
            int iColumnKey = dic[strColumnKey];
            int iColumnValue = dic[strColumnValue];

            Dictionary<string, string> DataDict = new Dictionary<string, string>();

            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1);
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow rowdata = sheet.GetRow(i);
                    if (rowdata != null)
                    {
                        ICell cellKey = rowdata.GetCell(iColumnKey);//如果rowdata是null,这里报错
                        ICell cellValue = rowdata.GetCell(iColumnValue);
                        if (cellKey != null && cellValue != null)
                        {
                            if (!DataDict.ContainsKey(cellKey.ToString()))
                            {
                                string strCellKey = cellKey.ToString();
                                string strCellValue = cellValue.ToString();
                                DataDict.Add(strCellKey, strCellValue);
                            }
                        }
                    }
                }
                return DataDict;
            }
        }

        /// <summary>
        /// 自适应列宽
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cols"></param>
        public static void AutoColumnWidth(ISheet sheet, int cols)
        {
            for (int col = 0; col <= cols; col++)
            {
                sheet.AutoSizeColumn(col);//自适应宽度，但是其实还是比实际文本要宽
                int columnWidth = (int)(sheet.GetColumnWidth(col) / 256);//获取当前列宽度
                for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    ICell cell = row.GetCell(col);
                    if (cell != null)
                    {
                        int contextLength = Encoding.UTF8.GetBytes(cell.ToString()).Length;//获取当前单元格的内容宽度
                        columnWidth = columnWidth < contextLength ? contextLength : columnWidth;
                    }


                }
                sheet.SetColumnWidth(col, columnWidth * 200);//经过测试200比较合适。

            }
        }

        /// <summary>
        /// 自适应列宽和打印页缩放
        /// </summary>
        /// <param name="filePath">excel文件路径</param>
        /// <param name="scale">缩放比例(77,80,100等）</param>
        public static void AutoColumnWidth(string filePath, short scale = 80)
        {
            //【1】打开excel文件的第几张表，第几行的数据，返回一个字典{列名：列的索引}
            //字典的作用：可以根据列名快速找到对应的列索引
            Dictionary<string, int> dicData = GetDataDictionary(filePath, 1, 1);

            bool isCompatible = GetIsCompatible(filePath);
            IWorkbook workbook = null;

            using (FileStream fs = File.Open(filePath, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                //把xls文件读入workbook变量里，之后就可以关闭了  
                workbook = CreateWorkbook(isCompatible, fs);
                fs.Close();
            }

            ISheet sheet = workbook.GetSheetAt(0);

            int rowCount = sheet.LastRowNum;


            for (int col = 0; col < sheet.GetRow(0).LastCellNum; col++)
            {
                //自适应列宽
                sheet.AutoSizeColumn(col);
            }

            //sheet.PrintSetup.FitWidth = 1;
            //sheet.PrintSetup.FitHeight = 0;

            //设置打印页面缩放比例
            sheet.PrintSetup.Scale = scale;

            #region 非常的耗时，不推荐使用

            ////开始遍历【遍历行操作】
            //for (int r = 1; r <= rowCount; r++) //从第二行开始遍历
            //{
            //    IRow currentRow = sheet.GetRow(r); //读取当前行数据
            //    if (currentRow == null) //如果为空，重新创建一行，防止null报错
            //    {
            //        sheet.CreateRow(r);
            //        currentRow = sheet.GetRow(r);
            //    }

            //    for (int i = 0; i < currentRow.LastCellNum; i++)
            //    {
            //        //sheet.AutoSizeColumn(i);//非常的耗时，不推荐用
            //    }              
            //}

            #endregion

            //转为字节数组 
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();
            //保存为Excel文件  这种方式能保存.xls和.xlsx文件
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }




        #region 根据一个文件，获取一个Excel文件的最大行数和列数（不成熟不建议用）

        /// <summary>
        /// 根据一个文件，获取一个Excel文件的最大行数和列数（不成熟不建议用）
        /// </summary>
        /// <param name="filepath">excel表格保存的地址，包括"文件名.xls</param>
        /// <param name="sheet_number">代表将要读取的sheet表的索引位置</param>
        /// <returns>行数Array[0],列数Array[2]</returns>
        public static Array GetRowCountAndColumnCount(string filepath, int sheet_number)
        {
            int rowMaxCount = 0;
            int columnMaxCount = 0;
            FileStream readStream = null;
            try
            {
                if (!string.IsNullOrEmpty(filepath) && sheet_number > 0)
                {
                    readStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    bool isCompatible = GetIsCompatible(filepath);
                    IWorkbook workbook = CreateWorkbook(isCompatible, readStream);
                    ISheet sheet = workbook.GetSheetAt(sheet_number - 1);
                    if (sheet != null)
                    {
                        rowMaxCount = sheet.LastRowNum + 1; //有效行数(NPOI读取的有效行数不包括列头，所以需要加1)
                        for (int c = 0; c <= sheet.LastRowNum; c++)
                        {
                            IRow row = sheet.GetRow(c);
#pragma warning disable CS1525 // 表达式项“=”无效
                            if (row != null && row.LastCellNum > -1)
#pragma warning restore CS1525 // 表达式项“=”无效

                            {
                                columnMaxCount = row.LastCellNum;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }
            }

            int[] array = new int[2];
            array[1] = rowMaxCount;
            array[2] = columnMaxCount;
            return array;
        }

        #endregion


        /// <summary>
        /// Excel转化为二维数组
        /// </summary>
        /// <param name="filepath">文件全路径</param>
        /// <param name="arrayRowNnmber">数组的行数</param>
        /// <param name="arrayColumnNumber">数组的列数</param>
        /// <param name="sheet_Number">要遍历的sheet索引</param>
        /// <returns>返回一个二维素组</returns>
        public static string[,] ToTwoArray(string filepath, int arrayRowNnmber, int arrayColumnNumber, int sheet_Number)
        {
            string[,] array = new string[arrayRowNnmber, arrayColumnNumber];
            bool isCompatible = GetIsCompatible(filepath);
            using (FileStream fsRead = File.OpenRead(filepath))
            {
                IWorkbook workbook = CreateWorkbook(isCompatible, fsRead);
                ISheet sheet = workbook.GetSheetAt(sheet_Number - 1); //获取第一个工作表（sheet）
                int rowCount = sheet.LastRowNum;
                if (!isCompatible)//如果是xlsx格式的，行数要-2
                {
                    rowCount = rowCount - 2;
                }
                for (int r = 0; r <= rowCount; r++)
                {
                    IRow currentRow = sheet.GetRow(r); //读取当前行数据
                    Console.Write($"第{r}行有{currentRow.LastCellNum}列有数据:--->");
                    for (int c = 0; c < currentRow.LastCellNum; c++)
                    {

                        //获取每个单元格(r行c列的数据）
                        ICell cell = currentRow.GetCell(c);
                        //获取单元格的内容
                        string value = string.Empty;
                        if (cell != null)
                        {
                            value = cell.ToString(); //如果单元格为空，这里会报错的
                        }

                        Console.Write($"{value}  |");
                        array[r, c] = value;
                    }

                    Console.WriteLine();

                }

                return array;
            }
        }

        /// <summary>
        /// 从工作表中生成DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headerRowIndex"></param>
        /// <returns></returns>
        private static DataTable GetDataTableFromSheet(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取
                    cellCount = i + 1;
                    break;
                }
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                if (row != null && !string.IsNullOrEmpty(row.Cells[0].StringCellValue))
                {
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }

                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }



        #region 公共导出方法



        /// <summary>
        /// 获取sheet表名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] GetSheetName(string filePath)
        {
            int sheetNumber = 0;
            var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (filePath.IndexOf(".xlsx") > 0)
            {
                //2007版本
                var xssfworkbook = new XSSFWorkbook(file);
                sheetNumber = xssfworkbook.NumberOfSheets;

                string[] sheetNames = new string[sheetNumber];

                for (int i = 0; i < sheetNumber; i++)
                {
                    sheetNames[i] = xssfworkbook.GetSheetName(i);
                }
                return sheetNames;
            }
            else if (filePath.IndexOf(".xls") > 0)
            {
                //2003版本
                var hssfworkbook = new HSSFWorkbook(file);
                sheetNumber = hssfworkbook.NumberOfSheets;

                string[] sheetNames = new string[sheetNumber];

                for (int i = 0; i < sheetNumber; i++)
                {
                    sheetNames[i] = hssfworkbook.GetSheetName(i);
                }
                return sheetNames;
            }
            return null;
        }

        /// <summary>
        /// 根据表名获取表
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, string sheetName)
        {
            string outMsg = "";
            var dt = new DataTable();
            string fileType = Path.GetExtension(filePath).ToLower();

            try
            {
                ISheet sheet = null;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (fileType == ".xlsx")
                {
                    //2007版
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet != null)
                    {
                        dt = GetSheetDataTable(sheet, out outMsg);
                    }
                }
                else if (fileType == ".xls")
                {
                    //2003版
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet != null)
                    {
                        dt = GetSheetDataTable(sheet, out outMsg);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return dt;
        }

        private static int sheetCellNumMax = 12;

        /// <summary>
        /// 获取sheet表对应的DataTable
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private static DataTable GetSheetDataTable(ISheet sheet, out string strMsg)
        {
            strMsg = "";
            DataTable dt = new DataTable();
            string sheetName = sheet.SheetName;
            int startIndex = 0;// sheet.FirstRowNum;
            int lastIndex = sheet.LastRowNum;

            //最大列数
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && cellCount < row.LastCellNum)
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }
            //列名设置
            try
            {

                //maxRow.LastCellNum = 12 // L
                for (int i = 0; i < sheetCellNumMax; i++)//maxRow.FirstCellNum
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
                    //DataColumn column = new DataColumn("Column" + (i + 1).ToString());
                    //dt.Columns.Add(column);
                }
            }
            catch
            {
                strMsg = "工作表" + sheetName + "中无数据";
                return null;
            }
            //数据填充
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            ICell cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    drNew[j] = "";
                                    break;
                                case CellType.Numeric:
                                    short format = cell.CellStyle.DataFormat;
                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                        drNew[j] = cell.DateCellValue;
                                    else
                                        drNew[j] = cell.NumericCellValue;
                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                        drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    break;
                                case CellType.String:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        drNew[j] = cell.NumericCellValue;
                                        if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                            drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            drNew[j] = cell.StringCellValue;
                                        }
                                        catch { }
                                    }
                                    break;
                                default:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                            }
                        }
                    }
                }
                dt.Rows.Add(drNew);
            }
            return dt;
        }







        #endregion

        #region 公共导入方法

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <param name="isCompatible">是否为兼容模式</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex, bool isCompatible)
        {
            IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
            ISheet sheet = null;
            int sheetIndex = -1;
            if (int.TryParse(sheetName, out sheetIndex))
            {
                sheet = workbook.GetSheetAt(sheetIndex);
            }
            else
            {
                sheet = workbook.GetSheet(sheetName);
            }

            DataTable table = GetDataTableFromSheet(sheet, headerRowIndex);

            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                bool isCompatible = GetIsCompatible(excelFilePath);
                return ImportFromExcel(stream, sheetName, headerRowIndex, isCompatible);
            }
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <param name="isCompatible">是否为兼容模式</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportFromExcel(Stream excelFileStream, int headerRowIndex, bool isCompatible)
        {
            DataSet ds = new DataSet();
            IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                DataTable table = GetDataTableFromSheet(sheet, headerRowIndex);
                ds.Tables.Add(table);
            }

            excelFileStream.Close();
            workbook = null;

            return ds;
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportFromExcel(string excelFilePath, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                bool isCompatible = GetIsCompatible(excelFilePath);
                return ImportFromExcel(stream, headerRowIndex, isCompatible);
            }
        }

        #endregion

        #region 公共转换方法

        /// <summary>
        /// 将Excel的列索引转换为列名，列索引从0开始，列名从A开始。如第0列为A，第1列为B...
        /// </summary>
        /// <param name="index">列索引</param>
        /// <returns>列名，如第0列为A，第1列为B...</returns>
        public static string ConvertColumnIndexToColumnName(int index)
        {
            index = index + 1;
            int system = 26;
            char[] digArray = new char[100];
            int i = 0;
            while (index > 0)
            {
                int mod = index % system;
                if (mod == 0) mod = system;
                digArray[i++] = (char)(mod - 1 + 'A');
                index = (index - 1) / 26;
            }
            StringBuilder sb = new StringBuilder(i);
            for (int j = i - 1; j >= 0; j--)
            {
                sb.Append(digArray[j]);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 转化日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DateTime ConvertDate(object date)
        {
            string dtStr = (date ?? "").ToString();

            DateTime dt = new DateTime();

            if (DateTime.TryParse(dtStr, out dt))
            {
                return dt;
            }

            try
            {
                string spStr = "";
                if (dtStr.Contains("-"))
                {
                    spStr = "-";
                }
                else if (dtStr.Contains("/"))
                {
                    spStr = "/";
                }
                string[] time = dtStr.Split(spStr.ToCharArray());
                int year = Convert.ToInt32(time[2]);
                int month = Convert.ToInt32(time[0]);
                int day = Convert.ToInt32(time[1]);
                string years = Convert.ToString(year);
                string months = Convert.ToString(month);
                string days = Convert.ToString(day);
                if (months.Length == 4)
                {
                    dt = Convert.ToDateTime(date);
                }
                else
                {
                    string rq = "";
                    if (years.Length == 1)
                    {
                        years = "0" + years;
                    }
                    if (months.Length == 1)
                    {
                        months = "0" + months;
                    }
                    if (days.Length == 1)
                    {
                        days = "0" + days;
                    }
                    rq = "20" + years + "-" + months + "-" + days;
                    dt = Convert.ToDateTime(rq);
                }
            }
            catch
            {
                throw new Exception("日期格式不正确，转换日期失败！");
            }
            return dt;
        }

        /// <summary>
        /// 转化数字
        /// </summary>
        /// <param name="d">数字字符串</param>
        /// <returns></returns>
        public static decimal ConvertDecimal(object d)
        {
            string dStr = (d ?? "").ToString();
            decimal result = 0;
            if (decimal.TryParse(dStr, out result))
            {
                return result;
            }
            else
            {
                throw new Exception("数字格式不正确，转换数字失败！");
            }

        }

        #endregion
    }
}
