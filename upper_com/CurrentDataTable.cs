using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    class CurrentDataTable
    {
        public int serialNo { set; get; }

        public string curDate { set; get; }

        // =========平稳段==============
        //private double smoothCur;

        public double smoothAverage { set; get; }

        public double smoothUpper { set; get; }

        public double smoothLower { set; get; }

        // =========突变段=============
        //private double mutationCur;

        public double mutationAverage { set; get; }

        public double mutationUpper { set; get; }

        public double mutationLower { set; get; }

        public CurrentDataTable()
        {

        }

        public CurrentDataTable(int serialNo, string curDate, /*double smoothCur,*/ double smoothAverage,
            double smoothUpper, double smoothLower, /*double mutationCur,*/
            double mutationAverage, double mutationUpper, double mutationLower)
        {
            this.serialNo = serialNo;
            this.curDate = curDate;
            //this.smoothCur = smoothCur;
            this.smoothAverage = smoothAverage;
            this.smoothUpper = smoothUpper;
            this.smoothLower = smoothLower;
            //this.mutationCur = mutationCur;
            this.mutationAverage = mutationAverage;
            this.mutationUpper = mutationUpper;
            this.mutationLower = mutationLower;
        }
    }
}
