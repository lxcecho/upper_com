using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    internal class CurrentData
    {

        public int SerialNo { get; set; }

        public List<(double i1, double i2)> Curs { get; private set; }

        public CurrentData(int serialNo, List<(double i1, double i2)> curs)
        {
            SerialNo = serialNo;
            Curs = curs;
        }

        public CurrentData() { } 

        //public double totalDuration { get; set; }


    }
}
