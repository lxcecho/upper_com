using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace upper_com
{
    public class ChartForm : Form
    {
        private Chart chart;

        public ChartForm(double[] xValues, double[] yValues, string title)
        {
            InitializeComponent(title);
            CreateChart(xValues, yValues);
        }

        public ChartForm(double[] yValues, string title)
        {
            InitializeComponent(title);
            CreateChart(yValues);
        }

        private void InitializeComponent(string title)
        {
            this.chart = new Chart();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.Dock = DockStyle.Fill;
            this.Controls.Add(this.chart);
            this.Text = title;
            this.ResumeLayout(false);
        }

        private void CreateChart(double[] xValues, double[] yValues)
        {
            ChartArea chartArea = new ChartArea();
            // 去掉背景的横线
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;

            // 设置 X 轴为时间类型
            chartArea.AxisX.Minimum = xValues.Min();
            chartArea.AxisX.Maximum = xValues.Max();
            chartArea.AxisX.Interval = 0.1; // 设置间隔

            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Red,
                ChartType = SeriesChartType.Line
            };

            for (int i = 0; i < xValues.Length; i++)
            {
                series.Points.AddXY(xValues[i], yValues[i]);
            }

            chart.Series.Add(series);

            // 自定义表格的名称
            chart.Titles.Add("T曲线");
        }

        private void CreateChart(double[] yValues)
        {
            ChartArea chartArea = new ChartArea();
            // 去掉背景的横线
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Current Values",
                Color = System.Drawing.Color.Red,
                ChartType = SeriesChartType.Line
            };

            for (int i = 0; i < yValues.Length; i++)
            {
                series.Points.AddXY(i, yValues[i]); // 使用索引作为 X 轴
            }

            chart.Series.Add(series);

            // 自定义表格的名称
            chart.Titles.Add("T曲线");
        }

        public void UpdateChart(double[] yValues, string title)
        {
            if (chart.Series.Count > 0)
            {
                this.Text = title;
                Series series = chart.Series[0];
                series.Points.Clear();
                for (int i = 0; i < yValues.Length; i++)
                {
                    series.Points.AddXY(i, yValues[i]);
                }
            }
        }

        public void UpdateChart(double[] xValues, double[] yValues, string title)
        {
            if (chart.Series.Count > 0)
            {
                this.Text = title;
                Series series = chart.Series[0];
                series.Points.Clear();
                for (int i = 0; i < xValues.Length; i++)
                {
                    series.Points.AddXY(xValues[i], yValues[i]);
                }
            }
        }
    }
}