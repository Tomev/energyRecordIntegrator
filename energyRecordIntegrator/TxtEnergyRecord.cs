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
            _departureDateTime = GetDateTimeFromStrings(
                lineParts[(int) LinePart.Date], lineParts[(int) LinePart.Time]);
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
            _departureDateTime = GetDateTimeFromStrings(date, time);
        }

        private DateTime GetDateTimeFromStrings(string date, string time)
        {
            string[] dateParts = date.Split("-");
            string[] timeParts = time.Split(":");

            return new DateTime(int.Parse(dateParts[(int) DatePart.Year]),
                                int.Parse(dateParts[(int) DatePart.Month]),
                                int.Parse(dateParts[(int) DatePart.Day]),
                                int.Parse(timeParts[(int) TimePart.Hour]),
                                int.Parse(timeParts[(int) TimePart.Minute]), 
                                0); // seconds
        }

        public override string ToString()
        {
            return _EZT + "\t" + _departureDateTime.ToString("dd-MM-yyyy") + "\t" +
            _departureDateTime.ToString("HH:mm") + "\t" + _energyIn.ToString().Replace(".", ",") + "\t" +
            _energyOut.ToString().Replace(".", ",") + "\t" + _position + "\t" + _driverName + "\n";
        }
    }
}
