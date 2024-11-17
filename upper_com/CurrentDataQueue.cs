using Microsoft.Office.Interop.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{

    internal class CurrentDataQueue
    {

        #region 数据定时刷新
        private FileSystemWatcher currentFileWatcher;
        private readonly string currentFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_current.xlsx";
        private readonly object currentFileLock = new object();
        #endregion
        private DataGridView dataGridView1;

        /**
         * 管理每个队列的数据，计算均值和标准差，并检查数据是否在限值内
         */
        private Queue<(double i1, double i2)> queue;

        // 队列的最大大小，默认为 20
        private int maxSize;

        // 用于计算限值的系数，默认为 3
        private int k;

        private List<double> averageSmoothHistory;
        private List<double> averageMutationHistory;

        public CurrentDataQueue(DataGridView dataGridView1, int size = 20, int kValue = 3)
        {
            queue = new Queue<(double, double)>();
            maxSize = size;
            k = kValue;
            averageSmoothHistory = new List<double>();
            averageMutationHistory = new List<double>();

            this.dataGridView1 = dataGridView1;
            InitializeFileWatcher();
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
            currentFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(currentFilePath),
                Filter = Path.GetFileName(currentFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            currentFileWatcher.Changed += OnCurrentFileChanged;
            currentFileWatcher.EnableRaisingEvents = true;
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
                //MessageBox.Show("当天记录压力数据文件文件不存在，表格数据为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show($"文件访问错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 添加数据到队列中，并检查是否需要报警
        // allowOutOfBounds: 是否允许超出限值的数据入队列
        public void AddData(double i1, double i2, CurrentData all, bool allowOutOfBounds = false)
        {

            // 队列满
            if (queue.Count == maxSize)
            {
                // 计算I1的均值和标准差
                double meanI1 = queue.Select(x => x.i1).Average();
                double stdDevI1 = Math.Sqrt(queue.Select(x => Math.Pow(x.i1 - meanI1, 2)).Average());
                double lowerLimitI1 = meanI1 - k * stdDevI1;
                double upperLimitI1 = meanI1 + k * stdDevI1;

                // 计算I2的均值和标准差
                double meanI2 = queue.Select(x => x.i2).Average();
                double stdDevI2 = Math.Sqrt(queue.Select(x => Math.Pow(x.i2 - meanI2, 2)).Average());
                double lowerLimitI2 = meanI2 - k * stdDevI2;
                double upperLimitI2 = meanI2 + k * stdDevI2;

                // 判断I1和I2是否在限值内 不在限值内，报警
                if ((i1 < lowerLimitI1 || i1 > upperLimitI1) || (i2 < lowerLimitI2 || i2 > upperLimitI2))
                {
                    MessageBox.Show("预警：数据不在上下限范围内！！！！！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (!allowOutOfBounds)
                    {
                        return;
                    }
                }
                // 出队
                queue.Dequeue();
            }

            // 入队
            queue.Enqueue((i1, i2));

            UpdateAverageHistory();

            if (averageSmoothHistory.Count >= 20 && averageMutationHistory.Count >= 20)
            {
                CheckForTrend();
            }

            // 处理每一组数据元
            CurrentDataProcessed(all);
        }

        // 更新最近 20 个平均值的历史记录。
        private void UpdateAverageHistory()
        {
            double currentAverageI1 = queue.Select(x => x.i1).Average();
            double currentAverageI2 = queue.Select(x => x.i2).Average();
            averageSmoothHistory.Add(currentAverageI1);
            averageMutationHistory.Add(currentAverageI2);

            if (averageSmoothHistory.Count > 20)
            {
                averageSmoothHistory.RemoveAt(0);
            }

            if (averageMutationHistory.Count > 20)
            {
                averageMutationHistory.RemoveAt(0);
            }
        }

        // 检查最近 20 个平均值是否有持续上升或下降的趋势。
        private void CheckForTrend()
        {
            bool increasingI1 = true;
            bool decreasingI1 = true;
            bool increasingI2 = true;
            bool decreasingI2 = true;

            for (int i = 1; i < averageSmoothHistory.Count; i++)
            {
                if (averageSmoothHistory[i] <= averageSmoothHistory[i - 1])
                {
                    increasingI1 = false;
                }
                if (averageSmoothHistory[i] >= averageSmoothHistory[i - 1])
                {
                    decreasingI1 = false;
                }
            }

            for (int i = 1; i < averageMutationHistory.Count; i++)
            {
                if (averageMutationHistory[i] <= averageMutationHistory[i - 1])
                {
                    increasingI2 = false;
                }
                if (averageMutationHistory[i] >= averageMutationHistory[i - 1])
                {
                    decreasingI2 = false;
                }
            }

            if (increasingI1 || decreasingI1 || increasingI2 || decreasingI2)
            {
                MessageBox.Show("预警：数据连续20组均值增大/变小！！！！！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 处理数据元，用来画T曲线
        private async void CurrentDataProcessed(CurrentData all)
        {
            Console.WriteLine("OnDataUpdated called.");

            // 在这里处理更新的数据
            var currentData = new CurrentDataTable();

            /* 初始化 CurrentData 对象的参数 */
            int serialNo = all.serialNo;
            currentData.serialNo = serialNo;
            currentData.curDate = DateTime.Now.ToString();

            List<double> stableList = all.stableList;

            if (stableList != null && stableList.Count > 0)
            {
                double stableAverage = stableList.Average();
                double varianceS = stableList.Average(v => Math.Pow(v - stableAverage, 2));
                double standardDeviationS = Math.Sqrt(varianceS);

                double stableLimit = stableAverage - k * standardDeviationS;
                double stableUpper = stableAverage + k * standardDeviationS;

                currentData.smoothAverage = stableAverage;
                currentData.smoothLower = stableLimit;
                currentData.smoothUpper = stableUpper;
            }

            List<double> mutationList = all.mutationList;
            if (mutationList != null && mutationList.Count > 0)
            {
                double mutationAverage = mutationList.Average();
                double varianceM = stableList.Average(v => Math.Pow(v - mutationAverage, 2));
                double standardDeviationM = Math.Sqrt(varianceM);

                double mutationLimit = mutationAverage - k * standardDeviationM;
                double mutationUpper = mutationAverage + k * standardDeviationM;

                currentData.mutationAverage = double.Parse($"{mutationAverage:F2}");
                currentData.mutationLower = double.Parse($"{mutationLimit:F2}");
                currentData.mutationUpper = double.Parse($"{mutationUpper:F2}");
            }

            UpdateCurrentData(currentData);

            // 异步将 curs 数据写入到 Excel 文件的第二个 Sheet
            await WriteCursDataToSheet2Async(all);
        }

        private async Task WriteCursDataToSheet2Async(CurrentData all)
        {
            await Task.Run(() =>
            {
                lock (currentFileLock)
                {
                    try
                    {
                        IWorkbook workbook;
                        ISheet sheet;

                        // 打开或创建 Excel 文件
                        if (!File.Exists(currentFilePath))
                        {
                            workbook = new XSSFWorkbook();
                            workbook.CreateSheet("Sheet1"); // 创建第一个 Sheet
                            workbook.CreateSheet("Sheet2"); // 创建第二个 Sheet
                        }
                        else
                        {
                            using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                workbook = new XSSFWorkbook(fs);
                            }
                        }

                        // 获取第二个 Sheet
                        sheet = workbook.GetSheet("Sheet2") ?? workbook.CreateSheet("Sheet2");

                        // 找到最后一行
                        int lastRowNum = sheet.LastRowNum;

                        // 将元数据写入到第二个 Sheet
                        List<double> stableList = all.stableList;
                        List<double> mutationList = all.mutationList;

                        stableList.AddRange(mutationList);

                        foreach (var cur in stableList)
                        {
                            IRow newRow = sheet.CreateRow(++lastRowNum);
                            newRow.CreateCell(0).SetCellValue(all.serialNo);
                            newRow.CreateCell(1).SetCellValue(cur);
                            newRow.CreateCell(2).SetCellValue(all.totalDuration);
                        }

                        // 写入文件
                        using (var fs = new FileStream(currentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            workbook.Write(fs);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"文件写入错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        void WriteData(IEnumerable<double> dataList, int serialNo, double dataDuration, double totalDuration, ISheet sheet, int lastRowNum)
        {
            foreach (var cur in dataList)
            {
                IRow newRow = sheet.CreateRow(++lastRowNum);
                newRow.CreateCell(0).SetCellValue(serialNo); // Write serialNo in the first column
                newRow.CreateCell(1).SetCellValue(cur);          // Write current value in the second column
                newRow.CreateCell(2).SetCellValue(dataDuration);     // Write duration in the third column
                newRow.CreateCell(3).SetCellValue(totalDuration);  // Write total duration in the fourth column
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

            // 异步写入文件
            await Task.Run(() => WriteCurrentDataToFile(currentData));
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

        private void WriteCurrentDataToFile(CurrentDataTable currentData)
        {
            lock (currentFileLock)
            {
                try
                {
                    IWorkbook workbook;
                    ISheet sheet;

                    if (!File.Exists(currentFilePath))
                    {
                        workbook = new XSSFWorkbook();
                        sheet = workbook.CreateSheet("Sheet1");

                        // 写入标题行
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.CreateCell(0).SetCellValue("测试编号");
                        headerRow.CreateCell(1).SetCellValue("时间");
                        //headerRow.CreateCell(2).SetCellValue("电流I1");
                        headerRow.CreateCell(2).SetCellValue("I1均值");
                        headerRow.CreateCell(3).SetCellValue("I1上限");
                        headerRow.CreateCell(4).SetCellValue("I1下限");
                        //headerRow.CreateCell(6).SetCellValue("电流I2");
                        headerRow.CreateCell(5).SetCellValue("I2均值");
                        headerRow.CreateCell(6).SetCellValue("I2上限");
                        headerRow.CreateCell(7).SetCellValue("I2下限");
                    }
                    else
                    {
                        // 打开现有文件并追加数据
                        using (var fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            workbook = new XSSFWorkbook(fs);
                            sheet = workbook.GetSheetAt(0);
                        }
                    }

                    // 找到最后一行
                    int lastRowNum = sheet.LastRowNum;
                    IRow newRow = sheet.CreateRow(lastRowNum + 1);
                    if (currentData != null)
                    {
                        // 追加数据
                        newRow.CreateCell(0).SetCellValue(currentData.serialNo);
                        newRow.CreateCell(1).SetCellValue(currentData.curDate);
                        //newRow.CreateCell(2).SetCellValue(currentData.GetSmoothCur().ToString("F2"));
                        newRow.CreateCell(2).SetCellValue(currentData.smoothLower.ToString("F2"));
                        newRow.CreateCell(3).SetCellValue(currentData.smoothUpper.ToString("F2"));
                        newRow.CreateCell(4).SetCellValue(currentData.smoothLower.ToString("F2"));
                        //newRow.CreateCell(6).SetCellValue(currentData.GetMutationCur().ToString("F2"));
                        newRow.CreateCell(5).SetCellValue(currentData.mutationAverage.ToString("F2"));
                        newRow.CreateCell(6).SetCellValue(currentData.mutationUpper.ToString("F2"));
                        newRow.CreateCell(7).SetCellValue(currentData.smoothLower.ToString("F2"));
                    }

                    // 写入文件
                    using (var fs = new FileStream(currentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        workbook.Write(fs);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"文件写入错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
