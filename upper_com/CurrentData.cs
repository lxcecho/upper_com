﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_com
{
    class CurrentData
    {
        private int serialNo;
        private string curDate;
        // =========平稳段==============
        private double smoothCur;
        private double smoothAverage;
        private double smoothUpper;
        private double smoothLower;
        // =========突变段=============
        private double mutationCur;
        private double mutationAverage;
        private double mutationUpper;
        private double mutationLower;

        public CurrentData()
        {

        }

        public CurrentData(int serialNo, string curDate, double smoothCur, double smoothAverage,
            double smoothUpper, double smoothLower, double mutationCur,
            double mutationAverage, double mutationUpper, double mutationLower)
        {
            this.serialNo = serialNo;
            this.curDate = curDate;
            this.smoothCur = smoothCur;
            this.smoothAverage = smoothAverage;
            this.smoothUpper = smoothUpper;
            this.smoothLower = smoothLower;
            this.mutationCur = mutationCur;
            this.mutationAverage = mutationAverage;
            this.mutationUpper = mutationUpper;
            this.mutationLower = mutationLower;
        }

        public int GetSerialNo() { return serialNo; }
        public string GetCurDate() { return curDate; }

        public double GetSmoothCur()
        {
            return smoothCur;
        }

        public double GetSmoothAverage()
        {
            return smoothAverage;
        }

        public double GetSmoothUpper()
        {
            return smoothUpper;
        }
        public double GetSmoothLower()
        {
            return smoothLower;
        }
        public double GetMutationCur()
        {
            return mutationCur;
        }
        public double GetMutationAverage()
        {
            return mutationAverage;
        }
        public double GetMutationUpper()
        {
            return mutationUpper;
        }
        public double GetMutationLower()
        {
            return mutationLower;
        }

        public void SetSerialNo(int serialNo)
        {
            this.serialNo = serialNo;
        }
        public void SetCurDate(string curDate) { this.curDate = curDate; }

        public void SetSmoothCur(double smoothCur)
        {
            this.smoothCur = smoothCur;
        }
        public void SetSmoothAverage(double smoothAverage)
        {
            this.smoothAverage = smoothAverage;
        }
        public void SetSmoothUpper(double smoothUpper)
        {
            this.smoothUpper = smoothUpper;
        }
        public void SetSmoothLower(double smoothLower)
        {
            this.smoothLower = smoothLower;
        }

        public void SetMutationCur(double mutationCur)
        {
            this.mutationCur = mutationCur;
        }
        public void SetMutationAverage(double mutationAverage)
        {
            this.mutationAverage = mutationAverage;
        }
        public void SetMutationUpper(double mutationUpper)
        {
            this.mutationUpper = mutationUpper;
        }
        public void SetMutationLower(double mutationLower)
        {
            this.mutationLower = mutationLower;
        }
    }
}
