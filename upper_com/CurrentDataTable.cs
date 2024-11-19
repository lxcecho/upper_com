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

        public string smoothAverage { set; get; }

        public string smoothUpper { set; get; }

        public string smoothLower { set; get; }

        // =========突变段=============
        //private double mutationCur;

        public string mutationAverage { set; get; }

        public string mutationUpper { set; get; }

        public string mutationLower { set; get; }

        public CurrentDataTable()
        {

        }

        public CurrentDataTable(int serialNo, string curDate, /*string smoothCur,*/ string smoothAverage,
            string smoothUpper, string smoothLower, /*string mutationCur,*/
            string mutationAverage, string mutationUpper, string mutationLower)
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
