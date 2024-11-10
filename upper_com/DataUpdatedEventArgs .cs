using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    internal class DataUpdatedEventArgs : EventArgs
    {
        public double StableAverage { get; }

        public double StableLimit { get; }

        public double StableUpper { get; }

        public double MutationAverage { get; }

        public double MutationLimit { get; }

        public double MutationUpper { get; }

        public string SerialNo { get; }

        public List<double> CurrentList { get; }

        public DataUpdatedEventArgs(double stableAverage, double stableLImit, double stableUpper,
            double mutationAverage, double mutationLimit, double mutationUppper, string serialNo, List<double> currentList)
        {
            StableAverage = stableAverage;
            StableLimit = stableLImit;
            StableUpper = stableUpper;

            MutationAverage = mutationAverage;
            MutationLimit = mutationLimit;
            MutationUpper = mutationUppper;

            SerialNo = serialNo;
            CurrentList = currentList;
        }
    }
}
