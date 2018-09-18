using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace energyRecordIntegrator
{
    class TxtEnergyRecord
    {
        enum DatePart { Year = 0, Month = 1, Day = 2 };
        enum TimePart { Hour = 0, Minute = 1 };
        enum LinePart { EZT = 0, Date = 1, Time = 2, EnIn = 3, EnOut = 4, Position = 5 };

        private string _EZT;
        private DateTime _departureDateTime;
        private double _energyIn;
        private double _energyOut;
        private string _position;
        private string _driverName;
        private string _managerName;

        public TxtEnergyRecord(string line)
        {
            string[] lineParts = line.Split("\t");

            _EZT = lineParts[(int) LinePart.EZT];
            _departureDateTime = DateTime.Parse(lineParts[(int) LinePart.Date] + " " + lineParts[(int) LinePart.Time]);
            _energyIn = double.Parse(lineParts[(int) LinePart.EnIn], CultureInfo.InvariantCulture);
            _energyOut = double.Parse(lineParts[(int) LinePart.EnOut], CultureInfo.InvariantCulture);
            _position = lineParts[(int) LinePart.Position];
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut)
        {
            _position = position;
            _EZT = EZT;
            _energyIn = energyIn;
            _energyOut = energyOut;
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut, DateTime departureDateTime)
            : this(position, EZT, energyIn, energyOut)
        {
            _departureDateTime = departureDateTime;
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut, string time, string date)
            : this(position, EZT, energyIn, energyOut)
        {
            _departureDateTime = DateTime.Parse(date + " " + time);
        }

        public override string ToString()
        {
            return _EZT + "\t" + _departureDateTime.ToString("yyyy-MM-dd") + "\t" +
            _departureDateTime.ToString("HH:mm") + "\t" + _energyIn.ToString().Replace(".", ",") + "\t" +
            _energyOut.ToString().Replace(".", ",") + "\t" + _position + "\t" + _driverName + "\t" + _managerName +  "\n";
        }

        public void SetDriverName(string driverName)
        {
            _driverName = driverName;
        }

        public void SetManagerName(string managerName)
        {
            _managerName = managerName;
        }

        public bool ExtractEligibleData(XlsEnergyRecord xlsEnergyRecord)
        {
            if(! _EZT.Equals(xlsEnergyRecord.GetTrainName()))
            {
                return false;
            }

            if(_departureDateTime >= xlsEnergyRecord.GetStartTime() && _departureDateTime <= xlsEnergyRecord.GetEndTime())
            {
                _driverName = xlsEnergyRecord.GetDriverName();
                _managerName = xlsEnergyRecord.GetManagerName();

                return true;
            }

            return false;
            
        }
    }
}
