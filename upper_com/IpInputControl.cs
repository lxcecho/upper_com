using System;
using System.Windows.Forms;

namespace upper_com
{
    public partial class IpInputControl : UserControl
    {
        public IpInputControl()
        {
            InitializeComponent();
            // 绑定事件
            textBox1.KeyPress += TextBox_KeyPress;
            textBox2.KeyPress += TextBox_KeyPress;
            textBox3.KeyPress += TextBox_KeyPress;
            textBox4.KeyPress += TextBox_KeyPress;

            textBox1.TextChanged += TextBox_TextChanged;
            textBox2.TextChanged += TextBox_TextChanged;
            textBox3.TextChanged += TextBox_TextChanged;
            textBox4.TextChanged += TextBox_TextChanged;

            // 设置控件加载事件
            this.Load += IpInputControl_Load;
        }

        private void IpInputControl_Load(object sender, EventArgs e)
        {
            // 设置默认IP地址
            IpAddress = "192.168.0.1";
        }

        public string IpAddress
        {
            get
            {
                return $"{textBox1.Text}.{textBox2.Text}.{textBox3.Text}.{textBox4.Text}";
            }
            set
            {
                var parts = value.Split('.');
                if (parts.Length == 4)
                {
                    textBox1.Text = parts[0];
                    textBox2.Text = parts[1];
                    textBox3.Text = parts[2];
                    textBox4.Text = parts[3];
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length == 3)
            {
                SelectNextControl(textBox, true, true, true, true);
            }
        }
    }
}
