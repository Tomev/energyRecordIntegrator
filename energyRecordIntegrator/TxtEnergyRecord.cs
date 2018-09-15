using System;
using System.Collections.Generic;
using System.Text;

namespace energyRecordIntegrator
{
    class TxtEnergyRecord
    {
        enum DatePart:int { Year = 0, Month = 1, Day = 2 };
        enum TimePart:int { Hour = 0, Minute = 1 };
        enum LinePart:int { EZT = 0, Date = 1, Time = 2, EnIn = 3, EnOut = 4, Position = 5 };

        private string EZT;
        private DateTime departureDateTime;
        private double energyIn;
        private double energyOut;
        private string position;
        private string driverName;

        public TxtEnergyRecord(string line)
        {
            string[] lineParts = line.Split("\t");
            this.EZT = lineParts[(int) LinePart.EZT];
            this.departureDateTime = GetDateTimeFromStrings(
                lineParts[(int) LinePart.Date], lineParts[(int) LinePart.Time]);
            this.energyIn = Double.Parse(lineParts[(int) LinePart.EnIn]);
            this.energyOut = Double.Parse(lineParts[(int)LinePart.EnOut]);
            this.position = lineParts[(int)LinePart.Position];
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut)
        {
            this.position = position;
            this.EZT = EZT;
            this.energyIn = energyIn;
            this.energyOut = energyOut;
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut, DateTime departureDateTime)
            : this(position, EZT, energyIn, energyOut)
        {
            this.departureDateTime = departureDateTime;
        }

        public TxtEnergyRecord(string position, string EZT, double energyIn, double energyOut, string time, string date)
            : this(position, EZT, energyIn, energyOut)
        {
            this.departureDateTime = GetDateTimeFromStrings(date, time);
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
            return EZT + "\t" + GetDateStringFromDateTime() + "\t" + 
            GetTimeStringFromDateTime() + "\t" + energyIn.ToString() + "\t" +
            energyOut.ToString() + "\t" + position + "\t" + driverName + "\n";
        }

        private string GetDateStringFromDateTime()
        {
            return departureDateTime.Year.ToString() + "-"
                    + departureDateTime.Month.ToString() + "-"
                    + departureDateTime.Day.ToString();
        }

        private string GetTimeStringFromDateTime()
        {
            int hour = departureDateTime.Hour;
            int minute = departureDateTime.Minute;

            string time = "";

            if (hour < 10) time = "0";
            time += hour.ToString() + ":";

            if (minute < 10) time += "0";
            time += minute.ToString();

            return time;
        }
    }
}
