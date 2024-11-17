using System.Collections.Generic;

namespace upper_com
{
    internal class VoltageData
    {
        // 电流测试编号
        public int currentNo { get; set; }

        // 电流测试信号
        public bool currentStartSignal { get; set; }

        // 电流测试信号
        public bool currentEndSignal { get; set; }

        // 压力传送信号
        public bool voltageTransformSignal { get; set; }

        // 15组压力数据
        public List<double> voltageList { get; set; }

        public double duration { get; set; }
    }
}
