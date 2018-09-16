using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace energyRecordIntegrator
{ 
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDir = @"d:\Dysk Google\Praca\ojciec\2018\energia\";
            string pathToFile = pathToDir + "ENERGIA.TXT";

            List<TxtEnergyRecord> txtEnergyRecordsList = new List<TxtEnergyRecord>();

            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(pathToFile);

            // Ingnore first line (headers)
            line = file.ReadLine();

            while ((line = file.ReadLine()) != null)
            {
                txtEnergyRecordsList.Add(new TxtEnergyRecord(line));
            }

            file.Close();


            // Find xls and xlsx files in given path

            List<string> extensions = new List<string> { ".xls", ".xlsx" };
            var excelFilesList = Directory.GetFiles(pathToDir, "*.*")
                                    .Where(excelFile => extensions.Contains(Path.GetExtension(excelFile).ToLower()));
            



            System.Console.WriteLine("There were {0} lines.", txtEnergyRecordsList.Count);
            System.Console.WriteLine(txtEnergyRecordsList[0].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[1].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[2].ToString());

            Console.ReadLine();
        }
    }
}
