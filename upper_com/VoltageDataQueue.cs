using NPOI.POIFS.Crypt.Dsig;
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

    internal class VoltageDataQueue
    {

        #region 数据定时刷新
        private FileSystemWatcher voltageFileWatcher;
        private readonly string voltageFilePath = @"D:\upperCom\" + DateTime.Now.ToString("yyyy-MM-dd") + "_voltage.xlsx";
        private readonly object voltageFileLock = new object();
        #endregion
        private DataGridView dataGridView2;

        /**
         * 管理每个队列的数据，计算均值和标准差，并检查数据是否在限值内
         */
        private Queue<double> queue;

        // 队列的最大大小，默认为 20
        private int maxSize;

        // 用于计算限值的系数，默认为 3
        private int k;

        private List<double> averageMutationHistory;

        public VoltageDataQueue(DataGridView dataGridView2, int size = 20, int kValue = 3)
        {
            queue = new Queue<double>();
            maxSize = size;
            k = kValue;
            averageMutationHistory = new List<double>();

            this.dataGridView2 = dataGridView2;
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

            // 监控电压文件
            voltageFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(voltageFilePath),
                Filter = Path.GetFileName(voltageFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };
            voltageFileWatcher.Changed += OnVoltageFileChanged;
            voltageFileWatcher.EnableRaisingEvents = true;
        }

        private void OnVoltageFileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Delay(100).ContinueWith(_ => LoadLatestVoltageDataToDataGridView());
        }

        private void LoadLatestVoltageDataToDataGridView()
        {
            lock (voltageFileLock)
            {
                if (this.dataGridView2.InvokeRequired)
                {
                    this.dataGridView2.Invoke((MethodInvoker)delegate
                    {
                        UpdateVoltageDataGridView();
                    });
                }
                else
                {
                    UpdateVoltageDataGridView();
                }
            }
        }

        private void UpdateVoltageDataGridView()
        {
            // 清除现有数据
            this.dataGridView2.Rows.Clear();

            if (!File.Exists(voltageFilePath))
            {
                //MessageBox.Show("当天记录的电压数据文件文件不存在，表格数据为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("当天记录的电压数据文件文件不存在，表格数据为空。");
                return;
            }

            try
            {
                using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                        this.dataGridView2.Rows.Add(rowData);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"文件访问错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // 异步写入文件
            await Task.Run(() => WriteVoltageDataToFile(voltageData));
        }

        private void AddVoltageDataToGridView(VoltageDataTable voltageData)
        {
            // 更新 DataGridView
            this.dataGridView2.Rows.Add(voltageData.currentNo, voltageData.voltageTransformSignal,
                voltageData.average, voltageData.upper, voltageData.lower);
            if (this.dataGridView2.Rows.Count > 15)
            {
                this.dataGridView2.Rows.RemoveAt(0);
            }
        }

        private void WriteVoltageDataToFile(VoltageDataTable voltageData)
        {
            lock (voltageFileLock)
            {
                try
                {
                    IWorkbook workbook;
                    ISheet sheet;

                    if (!File.Exists(voltageFilePath))
                    {
                        workbook = new XSSFWorkbook();
                        sheet = workbook.CreateSheet("Sheet1");

                        // 写入标题行
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.CreateCell(0).SetCellValue("电流测试编号");
                        headerRow.CreateCell(1).SetCellValue("压力传送信号");
                        headerRow.CreateCell(2).SetCellValue("压力值V");
                        headerRow.CreateCell(3).SetCellValue("压力上限");
                        headerRow.CreateCell(4).SetCellValue("压力下限");
                    }
                    else
                    {
                        // 打开现有文件并追加数据
                        using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            workbook = new XSSFWorkbook(fs);
                            sheet = workbook.GetSheetAt(0);
                        }
                    }

                    // 找到最后一行
                    int lastRowNum = sheet.LastRowNum;
                    IRow newRow = sheet.CreateRow(lastRowNum + 1);
                    // 追加数据
                    newRow.CreateCell(0).SetCellValue(voltageData.currentNo);
                    newRow.CreateCell(1).SetCellValue(voltageData.voltageTransformSignal);
                    newRow.CreateCell(2).SetCellValue(voltageData.average);
                    newRow.CreateCell(3).SetCellValue(voltageData.upper);
                    newRow.CreateCell(4).SetCellValue(voltageData.lower);

                    // 写入文件
                    using (var fs = new FileStream(voltageFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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

        // 添加数据到队列中，并检查是否需要报警
        // allowOutOfBounds: 是否允许超出限值的数据入队列
        public void AddData(double value, VoltageData all, bool allowOutOfBounds = false)
        {
            // 队列满
            if (queue.Count == maxSize)
            {
                // Calculate mean and standard deviation
                double mean = queue.Average();
                double stdDev = Math.Sqrt(queue.Select(x => Math.Pow(x - mean, 2)).Average());
                double lowerLimit = mean - k * stdDev;
                double upperLimit = mean + k * stdDev;

                // Check if value is within limits
                if (value < lowerLimit || value > upperLimit)
                {
                    MessageBox.Show("预警：数据不在上下限范围内！！！！！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (!allowOutOfBounds)
                    {
                        return;
                    }
                }
                // Dequeue
                queue.Dequeue();
            }

            // Enqueue
            queue.Enqueue(value);

            UpdateAverageHistory();

            if (averageMutationHistory.Count >= 20)
            {
                CheckForTrend();
            }

            // TODO 这里要处理元数据，画T曲线
            VoltageDataProcessed(all);

        }

        // 更新最近 20 个平均值的历史记录。
        private void UpdateAverageHistory()
        {
            double volAverage = queue.Average();
            averageMutationHistory.Add(volAverage);

            if (averageMutationHistory.Count > 20)
            {
                averageMutationHistory.RemoveAt(0);
            }
        }

        // 检查最近 20 个平均值是否有持续上升或下降的趋势。
        private void CheckForTrend()
        {
            bool increasing = true;
            bool decreasing = true;

            for (int i = 1; i < averageMutationHistory.Count; i++)
            {
                if (averageMutationHistory[i] <= averageMutationHistory[i - 1])
                {
                    increasing = false;
                }
                if (averageMutationHistory[i] >= averageMutationHistory[i - 1])
                {
                    decreasing = false;
                }
            }

            if (increasing || decreasing)
            {
                MessageBox.Show("预警：数据连续20组均值增大/变小！！！！！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void VoltageDataProcessed(VoltageData all)
        {
            Console.WriteLine("OnDataUpdated called.");

            // 在这里处理更新的数据
            var voltage = new VoltageDataTable();

            voltage.currentNo = all.currentNo;
            voltage.voltageTransformSignal = all.voltageTransformSignal;
            if (all.voltageList != null && all.voltageList.Count > 0)
            {
                List<double> list = all.voltageList;
                double mean = list.Average();
                voltage.average = mean.ToString("F2");
                // Calculate standard deviation
                double stdDev = (double)Math.Sqrt(list.Select(x => Math.Pow(x - mean, 2)).Average());

                // Calculate limits
                voltage.lower = (mean - k * stdDev).ToString("F2");
                voltage.upper = (mean + k * stdDev).ToString("F2");
            }

            UpdateVoltagetData(voltage);


            // 异步将 curs 数据写入到 Excel 文件的第二个 Sheet
            await WriteCursDataToSheet2Async(all);
        }

        private async Task WriteCursDataToSheet2Async(VoltageData all)
        {
            await Task.Run(() =>
            {
                lock (voltageFileLock)
                {
                    try
                    {
                        IWorkbook workbook;
                        ISheet sheet;

                        // 打开或创建 Excel 文件
                        if (!File.Exists(voltageFilePath))
                        {
                            workbook = new XSSFWorkbook();
                            workbook.CreateSheet("Sheet1"); // 创建第一个 Sheet
                            workbook.CreateSheet("Sheet2"); // 创建第二个 Sheet
                        }
                        else
                        {
                            using (var fs = new FileStream(voltageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                workbook = new XSSFWorkbook(fs);
                            }
                        }

                        // 获取第二个 Sheet
                        sheet = workbook.GetSheet("Sheet2") ?? workbook.CreateSheet("Sheet2");

                        // 找到最后一行
                        int lastRowNum = sheet.LastRowNum;

                        // 将 serialNo 和 curs 数据写入到第二个 Sheet
                        foreach (var cur in all.voltageList)
                        {
                            IRow newRow = sheet.CreateRow(++lastRowNum);
                            newRow.CreateCell(0).SetCellValue(all.currentNo); // 第一列写入 serialNo
                            newRow.CreateCell(1).SetCellValue(cur);      // 第二列写入 curs 值
                            newRow.CreateCell(2).SetCellValue(all.duration);      // 第三列写入 T 值
                        }

                        // 写入文件
                        using (var fs = new FileStream(voltageFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
    }
}
