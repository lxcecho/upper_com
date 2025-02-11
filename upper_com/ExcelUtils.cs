﻿using Microsoft.Office.Interop.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;

namespace upper_com
{
    internal class ExcelUtils
    {
        public static void SynchronizedToExcelFile(string filePath, CurrentDataTable currentData)
        {
            IWorkbook workbook;
            ISheet sheet;
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                // 创建新文件并写入数据
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("Sheet1");

                    // 写入标题行
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("测试编号");
                    headerRow.CreateCell(1).SetCellValue("时间");
                    headerRow.CreateCell(2).SetCellValue("电流I1");
                    headerRow.CreateCell(3).SetCellValue("I1均值");
                    headerRow.CreateCell(4).SetCellValue("I1上限");
                    headerRow.CreateCell(5).SetCellValue("I1下限");
                    headerRow.CreateCell(6).SetCellValue("电流I2");
                    headerRow.CreateCell(7).SetCellValue("I2均值");
                    headerRow.CreateCell(8).SetCellValue("I2上限");
                    headerRow.CreateCell(9).SetCellValue("I2下限");
                }
            }
            else
            {
                // 打开现有文件并追加数据
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fs);
                    sheet = workbook.GetSheetAt(0);
                }
            }

            // 找到最后一行
            int lastRowNum = sheet.LastRowNum;
            IRow newRow = sheet.CreateRow(lastRowNum + 1);

            // 追加数据
            newRow.CreateCell(0).SetCellValue(currentData.serialNo);
            newRow.CreateCell(1).SetCellValue(currentData.curDate);
            //newRow.CreateCell(2).SetCellValue(currentData.GetSmoothCur());
            newRow.CreateCell(3).SetCellValue(currentData.smoothAverage);
            newRow.CreateCell(4).SetCellValue(currentData.smoothUpper);
            newRow.CreateCell(5).SetCellValue(currentData.smoothLower);
            //newRow.CreateCell(6).SetCellValue(currentData.GetMutationCur());
            newRow.CreateCell(7).SetCellValue(currentData.mutationAverage);
            newRow.CreateCell(8).SetCellValue(currentData.mutationUpper);
            newRow.CreateCell(9).SetCellValue(currentData.mutationLower);


            // 写入文件
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        #region 测试用
        /*[STAThread]
        static void Main()
        {
            // 指定文件路径
            string filePath = @"D:\" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            for (int i = 0; i < 100; i++)
            {
                CurrentData cur = new CurrentData(i, i + "5", 3.2, 3.2, 3.2, 3.2, 3.2, 3.2, 3.2, 3.2);

                SynchronizedToExcelFile(filePath, cur);
            }
            Console.WriteLine("数据已写入Excel文件");
        }*/
        #endregion

        public static void SynchronizedVoltageToExcelFile(string filePath, VoltageDataTable voltageData)
        {
            IWorkbook workbook;
            ISheet sheet;
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                // 创建新文件并写入数据
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("Sheet1");

                    // 写入标题行
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("压力值编号");
                    headerRow.CreateCell(1).SetCellValue("压力值V");
                    headerRow.CreateCell(2).SetCellValue("压力上限");
                    headerRow.CreateCell(3).SetCellValue("压力下限");
                }
            }
            else
            {
                // 打开现有文件并追加数据
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fs);
                    sheet = workbook.GetSheetAt(0);
                }
            }

            // 找到最后一行
            int lastRowNum = sheet.LastRowNum;
            IRow newRow = sheet.CreateRow(lastRowNum + 1);

            // 追加数据
            newRow.CreateCell(0).SetCellValue(voltageData.VolNo);
            newRow.CreateCell(1).SetCellValue(voltageData.Average);
            newRow.CreateCell(2).SetCellValue(voltageData.Upper);
            newRow.CreateCell(3).SetCellValue(voltageData.Lower);


            // 写入文件
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
    }
}
