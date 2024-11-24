using System.Collections.Generic;
using System.Linq;

namespace upper_com
{
    internal class VoltageData
    {
        // 电流测试编号
        public int CurrentNo { get; set; }

        // 电流测试开始信号
        public bool CurrentStartSignal { get; set; }

        // 电流测试结束信号
        public bool CurrentEndSignal { get; set; }

        // 压力传送信号
        public bool VoltageTransformSignal { get; set; }    

        public double[] Pressures { get; set; }


        public VoltageData() { }

        public VoltageData(int currentNo, bool currentStartSignal, bool currentEndSignal, bool voltageTransformSignal, double[] pressures)
        {
            CurrentNo = currentNo;
            CurrentStartSignal = currentStartSignal;
            CurrentEndSignal = currentEndSignal;
            VoltageTransformSignal = voltageTransformSignal;
            Pressures = pressures;
        }

        public override string ToString()
        {
            string pressuresString = Pressures != null ? string.Join(", ", Pressures.Select(p => p.ToString("F2"))) : "No Pressures";
            return $"CurrentNo: {CurrentNo}, StartSignal: {CurrentStartSignal}, EndSignal: {CurrentEndSignal}, " +
                   $"TransformSignal: {VoltageTransformSignal}, Pressures: [{pressuresString}]";
        }
    }
}
