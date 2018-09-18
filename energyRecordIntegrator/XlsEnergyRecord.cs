using System;
using System.Collections.Generic;
using System.Text;

namespace energyRecordIntegrator
{
    class XlsEnergyRecord
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private string _driverName;
        private string _managerName;
        private string _trainName;
        private string _plannedTrainNumber;

        public XlsEnergyRecord(string startDate, string endDate, 
            string driverName, string managerName, string trainName, string plannedTrainNumber)
        {
            _startDate = DateTime.Parse(startDate);
            _endDate = DateTime.Parse(endDate);
            _driverName = driverName;
            _managerName = managerName;
            _trainName = trainName;
            _plannedTrainNumber = plannedTrainNumber;
        }

        public string ToString()
        {
            return _driverName + "\t" + _managerName + "\t" + _trainName + "\t"
                + _startDate.ToString("dd-MM-yyyy hh:mm") + _endDate.ToString("dd-MM-yyyy hh:mm");
        }

        public string GetTrainName()
        {
            return _trainName;
        }

        public string GetDriverName()
        {
            return _driverName;
        }

        public string GetManagerName()
        {
            return _managerName;
        }

        public DateTime GetStartTime()
        {
            return _startDate;
        }

        public DateTime GetEndTime()
        {
            return _endDate;
        }

        public string GetPlannedTrainNumber()
        {
            return _plannedTrainNumber;
        }
    }
}
