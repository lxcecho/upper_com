using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace upper_com
{
    public class ChartForm : Form
    {
        private Chart chart;

        public ChartForm(double[] xValues, double[] yValues)
        {
            InitializeComponent();
            CreateChart(xValues, yValues);
        }

        public ChartForm(double[] yValues)
        {
            InitializeComponent();
            CreateChart(yValues);
        }

        private void InitializeComponent()
        {
            this.chart = new Chart();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.Dock = DockStyle.Fill;
            this.Controls.Add(this.chart);
            this.Text = "Chart";
            this.ResumeLayout(false);
        }

        private void CreateChart(double[] xValues, double[] yValues)
        {
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Blue,
                ChartType = SeriesChartType.Line
            };

            for (int i = 0; i < xValues.Length; i++)
            {
                series.Points.AddXY(xValues[i], yValues[i]);
            }

            chart.Series.Add(series);
        }

        private void CreateChart(double[] yValues)
        {
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "Current Values",
                Color = System.Drawing.Color.Blue,
                ChartType = SeriesChartType.Line
            };

            for (int i = 0; i < yValues.Length; i++)
            {
                series.Points.AddXY(i, yValues[i]); // 使用索引作为 X 轴
            }

            chart.Series.Add(series);
        }
    }
}