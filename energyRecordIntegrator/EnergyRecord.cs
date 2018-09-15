using System;
using System.Collections.Generic;
using System.Text;

namespace energyRecordIntegrator
{
    class EnergyRecord
    {
        private string position;
        private string EZT;
        private string driverName;
        private double energyIn;
        private double energyOut;
        private DateTime departureDateTime;

        public EnergyRecord(string position, string EZT, double energyIn, double energyOut)
        {
            this.position = position;
            this.EZT = EZT;
            this.energyIn = energyIn;
            this.energyOut = energyOut;
        }

        public EnergyRecord(string position, string EZT, double energyIn, double energyOut, DateTime departureDateTime)
            : this(position, EZT, energyIn, energyOut)
        {
            this.departureDateTime = departureDateTime;
        }

        public EnergyRecord(string position, string EZT, double energyIn, double energyOut, string time, string date)
            : this(position, EZT, energyIn, energyOut)
        {
            this.departureDateTime = GetDateTimeFromStrings(date, time);
        }

        private DateTime GetDateTimeFromStrings(string date, string time)
        {
            string[] dateParts = date.Split("-");
            string[] timeParts = time.Split(":");

            return new DateTime(int.Parse(dateParts[0]), // year
                                int.Parse(dateParts[1]), // month
                                int.Parse(dateParts[2]), // day
                                int.Parse(timeParts[0]), // hour
                                int.Parse(timeParts[1]), // minute
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
