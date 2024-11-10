using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    internal class InputData
    { 
        // 队列大小
        private int num;

        // 可调参数
        private int k;

        // 采集时间
        Queue<(int start, int duration)> dataQueue;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public int K
        {
            get { return k; }
            set { k = value; }
        }

        public Queue<(int start, int duration)> DataQueue
        {
            get { return dataQueue; }
            set { dataQueue = value; }
        }
    }
}
