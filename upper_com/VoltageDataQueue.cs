using NLog;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{

    internal class VoltageDataQueue
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region 数据定时刷新
        private FileSystemWatcher voltageFileWatcher;
        private readonly string voltageFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_voltage.xlsx";
        private readonly object voltageFileLock = new object();
        private readonly SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);
        #endregion
        private DataGridView dataGridView2;

        /**
         * 管理每个队列的数据，计算均值和标准差，并检查数据是否在限值内
         */
        private Queue<VoltageData> volDataQueue;
        private List<double[]> recentAverages;
        private int consecutiveLimit;

        // 队列的最大大小，默认为 20
        private int maxSize;

        // 用于计算限值的系数，默认为 3
        private int k;

        private List<VoltageDataTable> voltageDataTables;

        private IWorkbook workbook;
        private ISheet sheet1;
        private ISheet sheet2;

        public VoltageDataQueue(DataGridView dataGridView2, int maxSize = 20, int kValue = 3, int consecutiveLimit = 20)
        {
            this.volDataQueue = new Queue<VoltageData>(maxSize);
            this.recentAverages = new List<double[]>();
            this.maxSize = maxSize;
            this.k = kValue;
            this.consecutiveLimit = consecutiveLimit;
            this.voltageDataTables = new List<VoltageDataTable>();

            this.dataGridView2 = dataGridView2;
            InitializeFileWatcher();



            workbook = new XSSFWorkbook();
            sheet1 = workbook.CreateSheet("Sheet1");
            sheet2 = workbook.CreateSheet("Sheet2");

            // 写入标题行
            IRow headerRow = sheet1.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("压力值编号");
            headerRow.CreateCell(1).SetCellValue("压力值V");
            headerRow.CreateCell(2).SetCellValue("压力上限");
            headerRow.CreateCell(3).SetCellValue("压力下限");

            // 写入Sheet2表头
            IRow headerRow2 = sheet2.CreateRow(0);
            // 第一列写入电流测试编号
            headerRow2.CreateCell(0).SetCellValue("电流测试编号");
            // 第二列写入电流测试开始信号
            headerRow2.CreateCell(1).SetCellValue("电流测试开始信号");
            // 第三列写入电流测试结束信号
            headerRow2.CreateCell(2).SetCellValue("电流测试结束信号");
            // 第四列写入压力传送信号
            headerRow2.CreateCell(3).SetCellValue("压力传送信号");
            // 第五到20列，写入15组压力数据
            for (int i = 0; i < 15; i++)
            {
                headerRow2.CreateCell(4 + i).SetCellValue($"压力值{i + 1}");
            }
        }

        private void InitializeFileWatcher()
        {
            string directoryPath = @"D:\upperCom";
            // 检查并创建目录
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private async void UpdateVoltagetData(VoltageDataTable voltageData)
        {
            if (this.dataGridView2.InvokeRequired)
            {
                // 先更新DataGridView
                this.dataGridView2.Invoke((MethodInvoker)delegate
                {
                    AddVoltageDataToGridView(voltageData);
                });
            }
            else
            {
                AddVoltageDataToGridView(voltageData);
            }
            await WriteDataToExcel(voltageData);
        }

        private void AddVoltageDataToGridView(VoltageDataTable voltageData)
        {
            // 更新 DataGridView
            this.dataGridView2.Rows.Add(voltageData.VolNo, voltageData.Average, voltageData.Upper, voltageData.Lower);
            if (this.dataGridView2.Rows.Count > 15)
            {
                this.dataGridView2.Rows.RemoveAt(0);
            }
        }

        private async Task SaveExcelFile()
        {
            await fileLock.WaitAsync();
            try
            {
                // 写入文件
                using (var fs = new FileStream(voltageFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await Task.Run(() => workbook.Write(fs));
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"{voltageFilePath}文件写入错误: {ex.Message}");
                MessageBox.Show($"{voltageFilePath}文件写入错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"{voltageFilePath}发生错误: {ex.Message}");
                MessageBox.Show($"{voltageFilePath}发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public async Task AddData(VoltageData newData)
        {
            if (volDataQueue.Count >= maxSize)
            {
                double[] means = CalculateMeans();
                double[] stdDevs = CalculateStandardDeviations(means);
                bool outOfLimits = false;

                for (int i = 0; i < 15; i++)
                {
                    double lowerLimit = means[i] - k * stdDevs[i];
                    double upperLimit = means[i] + k * stdDevs[i];

                    // 将计算的平均值和上下限写入 VoltageDataTable，用来显示界面
                    VoltageDataTable voltageData = new VoltageDataTable
                    {
                        VolNo = "压力值" + (i + 1),
                        Average = means[i].ToString("F2"),
                        Upper = upperLimit.ToString("F2"),
                        Lower = lowerLimit.ToString("F2")
                    };
                    //voltageDataTables.Add(voltageData);
                    Console.WriteLine("voldata=" + voltageData);
                    
                    UpdateVoltagetData(voltageData);

                    if (newData.Pressures[i] < lowerLimit || newData.Pressures[i] > upperLimit)
                    {
                        outOfLimits = true;
                        MessageBox.Show($"警告：数据超出限值范围！压力{i + 1}的值{newData.Pressures[i]}不在范围[{lowerLimit}, {upperLimit}]内。");
                        break;
                    }
                }

                if (outOfLimits)
                {
                    // 如果需要入队列，可以在此处添加逻辑
                    return;
                }

                volDataQueue.Dequeue();
            }

            volDataQueue.Enqueue(newData);
            UpdateRecentAverages();
            CheckConsecutiveChanges();

            // 将newData写入到Sheet2
            await WriteNewDataToSheet2(newData);

            // 保存Excel文件
            await SaveExcelFile();
        }

        private async Task WriteNewDataToSheet2(VoltageData all)
        {
            await fileLock.WaitAsync();
            try
            {
                // 找到最后一行
                int lastRowNum = sheet2.LastRowNum;

                // 将 serialNo 和 vols 数据写入到第二个 Sheet
                IRow newRow = sheet2.CreateRow(++lastRowNum);

                // 第一列写入电流测试编号
                newRow.CreateCell(0).SetCellValue(all.CurrentNo);
                // 第二列写入电流测试开始信号
                newRow.CreateCell(1).SetCellValue(all.CurrentStartSignal ? "True" : "False");
                // 第三列写入电流测试结束信号
                newRow.CreateCell(2).SetCellValue(all.CurrentEndSignal ? "True" : "False");
                // 第四列写入压力传送信号
                newRow.CreateCell(3).SetCellValue(all.VoltageTransformSignal ? "True" : "False");
                // 第五到20列，写入15组压力数据
                for (int i = 0; i < all.Pressures.Length; i++)
                {
                    newRow.CreateCell(4 + i).SetCellValue(all.Pressures[i]);
                }
            }
            finally
            {
                fileLock.Release();
            }
        }

        private async Task WriteDataToExcel(VoltageDataTable voltageData)
        {
            await fileLock.WaitAsync();
            try
            {
                // 找到最后一行
                int lastRowNum = sheet1.LastRowNum;
                IRow newRow = sheet1.CreateRow(lastRowNum + 1);
                // 追加数据
                newRow.CreateCell(0).SetCellValue(voltageData.VolNo);
                newRow.CreateCell(1).SetCellValue(voltageData.Average);
                newRow.CreateCell(2).SetCellValue(voltageData.Upper);
                newRow.CreateCell(3).SetCellValue(voltageData.Lower);
            }
            finally
            {
                fileLock.Release();
            }
        }

        private double[] CalculateMeans()
        {
            double[] means = new double[15];
            foreach (var data in volDataQueue)
            {
                for (int i = 0; i < 15; i++)
                {
                    means[i] += data.Pressures[i];
                }
            }
            for (int i = 0; i < 15; i++)
            {
                means[i] /= volDataQueue.Count;
            }
            return means;
        }

        private double[] CalculateStandardDeviations(double[] means)
        {
            double[] stdDevs = new double[15];
            foreach (var data in volDataQueue)
            {
                for (int i = 0; i < 15; i++)
                {
                    stdDevs[i] += Math.Pow(data.Pressures[i] - means[i], 2);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                stdDevs[i] = Math.Sqrt(stdDevs[i] / volDataQueue.Count);
            }
            return stdDevs;
        }

        private void UpdateRecentAverages()
        {
            double[] currentAverages = CalculateMeans();
            recentAverages.Add(currentAverages);

            if (recentAverages.Count > consecutiveLimit)
            {
                recentAverages.RemoveAt(0);
            }
        }

        private void CheckConsecutiveChanges()
        {
            if (recentAverages.Count < consecutiveLimit) return;

            for (int i = 0; i < 15; i++)
            {
                bool allIncreasing = true;
                bool allDecreasing = true;

                for (int j = 1; j < recentAverages.Count; j++)
                {
                    if (recentAverages[j][i] <= recentAverages[j - 1][i])
                        allIncreasing = false;
                    if (recentAverages[j][i] >= recentAverages[j - 1][i])
                        allDecreasing = false;
                }

                if (allIncreasing || allDecreasing)
                {
                    MessageBox.Show($"警告：压力值{i + 1}的连续20个平均值都变大或变小！");
                }
            }
        }

    }
}
