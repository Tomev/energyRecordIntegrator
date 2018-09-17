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

        public XlsEnergyRecord(string startDate, string endDate, 
            string driverName, string managerName, string trainName)
        {
            _startDate = DateTime.Parse(startDate);
            _endDate = DateTime.Parse(endDate);
            _driverName = driverName;
            _managerName = managerName;
            _trainName = trainName;
        }

        public string ToString()
        {
            return _driverName + "\t" + _managerName + "\t" + _trainName + "\t"
                + _startDate.ToString("dd-MM-yyyy hh:mm") + _endDate.ToString("dd-MM-yyyy hh:mm");
        }
    }
}
