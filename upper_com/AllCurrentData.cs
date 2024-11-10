using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    internal class AllCurrentData
    {

        private string serialNo;

        private List<double> stableList;

        private List<double> mutationList;

        // 构造函数
        public AllCurrentData()
        {
            stableList = new List<double>();
            mutationList = new List<double>();
        }

        // 属性用于访问和修改 serialNo
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        // 属性用于访问和修改 currentList
        public List<double> StatbleList
        {
            get { return stableList; }
            set { stableList = value; }
        }

        public List<double> MutationList
        {
            get { return mutationList; }
            set { mutationList = value; }
        }

    }
}
