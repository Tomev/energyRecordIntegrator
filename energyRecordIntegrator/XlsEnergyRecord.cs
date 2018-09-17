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

        public XlsEnergyRecord(string day, string startHour, string endHour, 
            string driverName, string managerName, string trainName)
        {
            _startDate = DateTime.Parse(day + " " + startHour);
            _endDate = DateTime.Parse(day + " " + endHour);

            _driverName = driverName;
            _managerName = managerName;
            _trainName = trainName;
        }
    }
}
