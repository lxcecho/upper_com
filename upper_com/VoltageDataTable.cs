using NPOI.SS.Formula.Functions;
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
        public string VolNo { get; set; }

        public string Average { get; set; }

        public string Upper { get; set; }

        public string Lower { get; set; }

        public VoltageDataTable()
        {

        }

        public VoltageDataTable(string volNo, string average, string upper, string lower)
        {
            VolNo = volNo;
            Average = average;
            Upper = upper;
            Lower = lower;
        }
        public override string ToString()
        {
            return $"VolNo: {VolNo}, Average: {Average}, Upper: {Upper}, Lower: {Lower}";
        }
    }
}
