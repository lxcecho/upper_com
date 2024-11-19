using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    class VoltageDataTable
    {
        // 电流测试编号
        public int currentNo { get; set; }

        // 压力传送信号
        public bool voltageTransformSignal { get; set; }

        public string average { get; set; }

        public string upper { get; set; }

        public string lower { get; set; }

        public VoltageDataTable()
        {

        }

        public VoltageDataTable(int currentNo, bool voltageTransformSignal, string average, string upper, string lower)
        {
            this.currentNo = currentNo;
            this.voltageTransformSignal = voltageTransformSignal;
            this.average = average;
            this.upper = upper;
            this.lower = lower;
        }

    }
}
