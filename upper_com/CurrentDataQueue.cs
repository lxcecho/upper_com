using Microsoft.Office.Interop.Excel;
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

    internal class CurrentDataQueue
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region 数据定时刷新
        private FileSystemWatcher currentFileWatcher;
        private readonly string currentFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_current.xlsx";
        private readonly object currentFileLock = new object();
        private readonly SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);
        #endregion
        private DataGridView dataGridView1;

        /**
         * 管理每个队列的数据，计算均值和标准差，并检查数据是否在限值内
         */
        private Queue<CurrentData> curQueue;

        private Queue<(double meanI1, double meanI2)> meanHistory;

        // 队列的最大大小，默认为 20
        private int maxSize;

        // 用于计算限值的系数，默认为 3
        private int k;

        IWorkbook workbook;
        ISheet sheet1;
        ISheet sheet2;


        public CurrentDataQueue(DataGridView dataGridView1, int size = 20, int kValue = 3)
        {
            curQueue = new Queue<CurrentData>(maxSize);
            maxSize = size;
            k = kValue;
            this.meanHistory = new Queue<(double, double)>(20);

            this.dataGridView1 = dataGridView1;
            InitializeFileWatcher();

            workbook = new XSSFWorkbook();
            sheet1 = workbook.CreateSheet("Sheet1");
            sheet2 = workbook.CreateSheet("Sheet2");

            // 写入标题行
            IRow headerRow = sheet1.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("测试编号");
            headerRow.CreateCell(1).SetCellValue("时间");
            headerRow.CreateCell(2).SetCellValue("I1均值");
            headerRow.CreateCell(3).SetCellValue("I1上限");
            headerRow.CreateCell(4).SetCellValue("I1下限");
            headerRow.CreateCell(5).SetCellValue("I2均值");
            headerRow.CreateCell(6).SetCellValue("I2上限");
            headerRow.CreateCell(7).SetCellValue("I2下限");

            // 写入Sheet2表头
            IRow headerRow2 = sheet2.CreateRow(0);
            headerRow2.CreateCell(0).SetCellValue("序号");
            for (int i = 0; i < 15; i++)
            {
                headerRow2.CreateCell(1 + i).SetCellValue($"电流{i + 1}组");
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

            // 监控电流文件
            /*currentFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(currentFilePath),
                Filter = Path.GetFileName(currentFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            currentFileWatcher.Changed += OnCurrentFileChanged;
            currentFileWatcher.EnableRaisingEvents = true;*/
        }

        private void OnCurrentFileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Delay(100).ContinueWith(_ => LoadLatestCurrentDataToDataGridView());
        }

        private void LoadLatestCurrentDataToDataGridView()
        {
            lock (currentFileLock)
            {
                if (this.dataGridView1.InvokeRequired)
                {
                    this.dataGridView1.Invoke((MethodInvoker)delegate
                    {
                        UpdateCurrentDataGridView();
                    });
                }
                else
                {
                    UpdateCurrentDataGridView();
                }
            }
        }

        private void UpdateCurrentDataGridView()
        {
            // 清除现有数据
            this.dataGridView1.Rows.Clear();

            if (!File.Exists(currentFilePath))
            {
                Logger.Info("当天记录压力数据文件文件不存在，表格数据为空。");
                Console.WriteLine("当天记录的电流数据文件文件不存在，表格数据为空。");
                return;
            }

            try
            {
                using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // 读取最新的15条数据
                    int startRow = Math.Max(1, sheet.LastRowNum - 14);
                    for (int i = startRow; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        object[] rowData = new object[row.LastCellNum];
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            rowData[j] = row.GetCell(j)?.ToString();
                        }
                        this.dataGridView1.Rows.Add(rowData);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"文件访问错误: {ex.Message}");
                MessageBox.Show($"文件访问错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task AddData(CurrentData all, bool allowOutOfBounds = false)
        {
            // 队列满
            if (curQueue.Count == maxSize)
            {
                // Flatten the queue to calculate mean and standard deviation
                var allPairs = curQueue.SelectMany(cd => cd.Curs).ToList();
                var i1Values = allPairs.Select(x => x.i1).ToList();
                var i2Values = allPairs.Select(x => x.i2).ToList();
                double meanI1 = i1Values.Average();
                double meanI2 = i2Values.Average();
                double stdI1 = Math.Sqrt(i1Values.Average(v => Math.Pow(v - meanI1, 2)));
                double stdI2 = Math.Sqrt(i2Values.Average(v => Math.Pow(v - meanI2, 2)));

                // Calculate limits
                double lowerLimitI1 = meanI1 - k * stdI1;
                double upperLimitI1 = meanI1 + k * stdI1;
                double lowerLimitI2 = meanI2 - k * stdI2;
                double upperLimitI2 = meanI2 + k * stdI2;

                // 在这里处理更新的数据
                var currentData = new CurrentDataTable();

                /* 初始化 CurrentData 对象的参数 */
                int serialNo = all.SerialNo;
                currentData.serialNo = serialNo;
                currentData.curDate = DateTime.Now.ToString();
                currentData.smoothAverage = meanI1.ToString("F2");
                currentData.smoothLower = lowerLimitI1.ToString("F2");
                currentData.smoothUpper = upperLimitI1.ToString("F2");
                currentData.mutationAverage = meanI2.ToString("F2");
                currentData.mutationLower = lowerLimitI2.ToString("F2");
                currentData.mutationUpper = upperLimitI2.ToString("F2");

                Console.WriteLine("CurrentDataProcessed called=" + currentData.ToString());

                UpdateCurrentData(currentData);
                
                // Check if data is within limits
                foreach (var (i1, i2) in all.Curs)
                {
                    if (!(lowerLimitI1 <= i1 && i1 <= upperLimitI1 && lowerLimitI2 <= i2 && i2 <= upperLimitI2))
                    {
                        allowOutOfBounds = true;
                        Logger.Info($"警告：数据超出限值范围！电流值不在限值范围内。");
                        MessageBox.Show($"警告：数据超出限值范围！电流值不在限值范围内。");
                        break;
                    }
                }

                if (allowOutOfBounds)
                {
                    return;
                }

                curQueue.Dequeue();

            }

            curQueue.Enqueue(all);

            CheckConsecutiveChanges();

            await WriteCursDataToSheet2Async(all);
            await SaveExcelFileAsync();
        }

        private void CheckConsecutiveChanges()
        {
            // Update mean history
            var currentMeans = GetAverage();

            if (meanHistory.Count == 20)
            {
                meanHistory.Dequeue();
            }
            meanHistory.Enqueue(currentMeans);

            // Check for consistent increase or decrease
            if (meanHistory.Count == 20)
            {
                var i1Means = meanHistory.Select(x => x.meanI1).ToList();
                var i2Means = meanHistory.Select(x => x.meanI2).ToList();
                if (i1Means.Zip(i1Means.Skip(1), (x, y) => x < y).All(b => b) ||
                    i1Means.Zip(i1Means.Skip(1), (x, y) => x > y).All(b => b))
                {
                    MessageBox.Show($"警告：i1的连续20个平均值都变大或变小！");
                    Logger.Info("Alarm: i1 means consistently increasing or decreasing!");
                }
                if (i2Means.Zip(i2Means.Skip(1), (x, y) => x < y).All(b => b) ||
                    i2Means.Zip(i2Means.Skip(1), (x, y) => x > y).All(b => b))
                {
                    MessageBox.Show($"警告：i1的连续20个平均值都变大或变小！");
                    Logger.Info("Alarm: i2 means consistently increasing or decreasing!");
                }
            }
        }

        private async Task SaveExcelFileAsync()
        {
            await fileLock.WaitAsync();
            try
            {
                // 写入文件
                using (var fs = new FileStream(currentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await Task.Run(() => workbook.Write(fs));
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"{currentFilePath}文件写入错误: {ex.Message}");
                MessageBox.Show($"{currentFilePath}文件写入错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.Info($"{currentFilePath}发生错误: {ex.Message}");
                MessageBox.Show($"{currentFilePath}发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fileLock.Release();
            }

        }

        public (double meanI1, double meanI2) GetAverage()
        {
            if (!curQueue.Any())
            {
                return (0, 0);
            }

            double totalI1 = 0;
            double totalI2 = 0;
            int totalCount = 0;

            foreach (var data in curQueue)
            {
                foreach (var (i1, i2) in data.Curs)
                {
                    totalI1 += i1;
                    totalI2 += i2;
                    totalCount++;
                }
            }

            double meanI1 = totalI1 / totalCount;
            double meanI2 = totalI2 / totalCount;

            return (meanI1, meanI2);
        }


        private async Task WriteCursDataToSheet2Async(CurrentData all)
        {
            await fileLock.WaitAsync();
            try
            {
                // 找到最后一行
                int lastRowNum = sheet2.LastRowNum;

                // 将 serialNo 和 curs 数据写入到第二个 Sheet
                IRow newRow = sheet2.CreateRow(++lastRowNum);

                // 第一列写入电流测试编号
                newRow.CreateCell(0).SetCellValue(all.SerialNo);

                for (int i = 0; i < 15; i++)
                {
                    newRow.CreateCell(1 + i).SetCellValue(all.Curs[i].ToString());
                }
            }
            finally
            {
                fileLock.Release();
            }
        }

        private async void UpdateCurrentData(CurrentDataTable currentData)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                // 先更新DataGridView
                this.dataGridView1.Invoke((MethodInvoker)delegate
                {
                    AddCurrentDataToGridView(currentData);
                });
            }
            else
            {
                AddCurrentDataToGridView(currentData);
            }
            await WriteCurrentDataToFileAsync(currentData);
        }

        private void AddCurrentDataToGridView(CurrentDataTable currentData)
        {
            if (currentData != null)
            {
                // 更新 DataGridView
                this.dataGridView1.Rows.Add(currentData.serialNo, currentData.curDate, currentData.smoothAverage,
                    currentData.smoothUpper, currentData.smoothLower, currentData.mutationAverage,
                    currentData.mutationUpper, currentData.mutationLower);

                // 检查当前行数是否超过 15
                if (this.dataGridView1.Rows.Count >= 15)
                {
                    // 删除最旧的行（第一行）
                    this.dataGridView1.Rows.RemoveAt(0);
                }
            }
        }

        private async Task WriteCurrentDataToFileAsync(CurrentDataTable currentData)
        {
            await fileLock.WaitAsync();
            try
            {
                // 找到最后一行
                int lastRowNum = sheet1.LastRowNum;
                IRow newRow = sheet1.CreateRow(lastRowNum + 1);
                if (currentData != null)
                {
                    // 追加数据
                    newRow.CreateCell(0).SetCellValue(currentData.serialNo);
                    newRow.CreateCell(1).SetCellValue(currentData.curDate);
                    //newRow.CreateCell(2).SetCellValue(currentData.GetSmoothCur());
                    newRow.CreateCell(2).SetCellValue(currentData.smoothAverage);
                    newRow.CreateCell(3).SetCellValue(currentData.smoothUpper);
                    newRow.CreateCell(4).SetCellValue(currentData.smoothLower);
                    //newRow.CreateCell(6).SetCellValue(currentData.GetMutationCur());
                    newRow.CreateCell(5).SetCellValue(currentData.mutationAverage);
                    newRow.CreateCell(6).SetCellValue(currentData.mutationUpper);
                    newRow.CreateCell(7).SetCellValue(currentData.mutationLower);
                }
            }
            finally
            {
                fileLock.Release();
            }
        }
    }
}
