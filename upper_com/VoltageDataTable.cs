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

        public double average { get; set; }

        public double upper { get; set; }

        public double lower { get; set; }

        public VoltageDataTable()
        {

        }

        public VoltageDataTable(int currentNo, bool voltageTransformSignal, float average, float upper, float lower)
        {
            this.currentNo = currentNo;
            this.voltageTransformSignal = voltageTransformSignal;
            this.average = average;
            this.upper = upper;
            this.lower = lower;
        }

    }
}
