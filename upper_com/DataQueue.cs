using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace upper_com
{
    internal class DataQueue
    {
        /**
         * 管理每个队列的数据，计算均值和标准差，并检查数据是否在限值内
         */
        private Queue<double> queue;

        // 队列的最大大小，默认为 20
        private int maxSize;

        // 用于计算限值的系数，默认为 3
        private double k;

        private List<double> averageHistory;

        public DataQueue(int size = 20, double kValue = 3.0)
        {
            queue = new Queue<double>();
            maxSize = size;
            k = kValue;
            averageHistory = new List<double>();
        }

        // 添加数据到队列中，并检查是否需要报警
        // allowOutOfBounds: 是否允许超出限值的数据入队列
        public void AddData(double data, bool allowOutOfBounds = false)
        {
            // 队列满
            if (queue.Count == maxSize)
            {
                // 平均值
                double mean = queue.Average();
                // 标准差
                double stdDev = Math.Sqrt(queue.Select(x => Math.Pow(x - mean, 2)).Average());
                // 下限
                double lowerLimit = mean - k * stdDev;
                // 上限
                double upperLimit = mean + k * stdDev;

                // 判断不在限值内，报警
                if (data < lowerLimit || data > upperLimit)
                {
                    Console.WriteLine("Data out of bounds! Alarm triggered.");
                    // 数据不入队列
                    if (!allowOutOfBounds)
                        return;
                }

                // 出队
                queue.Dequeue();
            }

            // 入队
            queue.Enqueue(data);
            // 
            UpdateAverageHistory();

            if (averageHistory.Count >= 20)
            {
                CheckForTrend();
            }
        }

        // 更新最近 20 个平均值的历史记录。
        private void UpdateAverageHistory()
        {
            double currentAverage = queue.Average();
            averageHistory.Add(currentAverage);

            if (averageHistory.Count > 20)
            {
                averageHistory.RemoveAt(0);
            }
        }

        // 检查最近 20 个平均值是否有持续上升或下降的趋势。
        private void CheckForTrend()
        {
            bool increasing = true;
            bool decreasing = true;

            for (int i = 1; i < averageHistory.Count; i++)
            {
                if (averageHistory[i] <= averageHistory[i - 1])
                    increasing = false;
                if (averageHistory[i] >= averageHistory[i - 1])
                    decreasing = false;
            }

            // 持续变大/持续变小，报警
            if (increasing || decreasing)
            {
                MessageBox.Show("Trend detected! Alarm triggered.");
            }
        }
    }
}
