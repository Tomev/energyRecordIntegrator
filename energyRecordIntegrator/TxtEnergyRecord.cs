using System;
using System.Collections.Generic;
using System.Text;

namespace energyRecordIntegrator
{
    class TxtEnergyRecord
    {
        enum DatePart { Year = 0, Month = 1, Day = 2 };
        enum TimePart { Hour = 0, Minute = 1 };

        private string position;
        private string EZT;
        private string driverName;
        private double energyIn;
        private double energyOut;
        private DateTime departureDateTime;

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

            return new DateTime(int.Parse(dateParts[DatePart.Year]),
                                int.Parse(dateParts[DatePart.Month]),
                                int.Parse(dateParts[DatePart.Day]),
                                int.Parse(timeParts[TimePart.Hour]),
                                int.Parse(timeParts[TimePart.Minute]), 
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
            return departureDateTime.Hour.ToString() + ":" + departureDateTime.Minute.ToString();
        }
    }
}
