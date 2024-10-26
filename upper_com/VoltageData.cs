using System.Collections.Generic;

namespace upper_com
{
    internal class VoltageData
    {
        // 电流测试编号
        private int currentNo;

        // 电流测试信号
        private bool currentTestSignal;

        // 压力传送信号
        private bool voltageTransformSignal;

        // 15组压力数据
        private List<double> voltageList;

        public void SetCurrentNo(int currentNo)
        {
            this.currentNo = currentNo;
        }

        public int GetCurrentNo()
        {
            return this.currentNo;
        }

        public void SetCurrentTestSignal(bool currentTestSignal)
        {
            this.currentTestSignal = currentTestSignal;
        }

        public bool GetCurrentTestSignal()
        {
            return this.currentTestSignal;
        }

        public void SetVoltageTransformSignal(bool voltageTransformSignal)
        {
            this.voltageTransformSignal = voltageTransformSignal;
        }

        public bool GetVoltageTransformSignal()
        {
            return this.voltageTransformSignal;
        }

        public void SetVoltageList(List<double> voltageList)
        {
            this.voltageList = voltageList;
        }
        public List<double> GetVoltageList()
        {
            return this.voltageList;
        }

    }
}
