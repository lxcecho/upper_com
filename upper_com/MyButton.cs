using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace upper_com
{
    public partial class MyButton : Button
    {
        public MyButton()
        {
            InitializeComponent();
        }

        private bool isPlaying = false;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                this.Invalidate(); // 重新绘制按钮
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //base.OnPaint(pevent);
            // 不调用 base.OnPaint(pevent) 以避免默认绘制
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 设置按钮为圆形
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);

            // 绘制按钮背景
            pevent.Graphics.FillEllipse(Brushes.LightGray, 0, 0, this.Width, this.Height);

            if (isPlaying)
            {
                // 绘制暂停图标
                int barWidth = this.Width / 5;
                int barHeight = this.Height / 2;
                int barX = (this.Width - 2 * barWidth) / 3;
                int barY = (this.Height - barHeight) / 2;

                pevent.Graphics.FillRectangle(Brushes.Green, barX, barY, barWidth, barHeight);
                pevent.Graphics.FillRectangle(Brushes.Green, 2 * barX + barWidth, barY, barWidth, barHeight);
            }
            else
            {
                // 绘制播放图标
                Point[] trianglePoints = {
                new Point(this.Width / 3, this.Height / 4), // 左顶点
                new Point(this.Width / 3, 3 * this.Height / 4), // 左底点
                new Point(2 * this.Width / 3, this.Height / 2) // 右顶点
            };

                pevent.Graphics.FillPolygon(Brushes.Red, trianglePoints);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            IsPlaying = !IsPlaying; // 切换状态
        }
    }
}
