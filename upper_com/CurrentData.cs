using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    internal class CurrentData
    {

        public int serialNo {  get; set; }

        public List<double> stableList { get; set; }

        public List<double> mutationList { get; set; }

        public double totalDuration { get; set; }

        // 构造函数
        public CurrentData()
        {
            stableList = new List<double>();
            mutationList = new List<double>();
        }
    }
}
