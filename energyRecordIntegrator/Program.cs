using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;

namespace energyRecordIntegrator
{ 
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDir = @"d:\Dysk Google\Praca\ojciec\2018\energia\";
            string pathToFile = pathToDir + "ENERGIA.TXT";

            List<TxtEnergyRecord> txtEnergyRecordsList = GetTxtEnergyRecords(pathToFile);

            // Find xls and xlsx files in given path
            List<string> extensions = new List<string> { ".xls", ".xlsx" };
            var excelFilesList = Directory.GetFiles(pathToDir, "*.*")
                                    .Where(excelFile => extensions.Contains(Path.GetExtension(excelFile).ToLower()));

            // Use this list to gather data of trains.

            foreach(string excelFilePath in excelFilesList)
            { 
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)

                    var r = ExcelReaderFactory.CreateBinaryReader(stream);

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        // 1. Use the reader methods
                        do
                        {
                            while (reader.Read())
                            {
                                // reader.GetDouble(0);
                                
                            }
                        } while (reader.NextResult());
                    }
                }
            }
            
            System.Console.WriteLine("There were {0} lines in TXT file.", txtEnergyRecordsList.Count);
            System.Console.WriteLine("{0} excel files found.\n", excelFilesList.Count());
            System.Console.WriteLine(txtEnergyRecordsList[0].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[1].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[2].ToString());

            Console.ReadLine();
        }

        static private List<TxtEnergyRecord> GetTxtEnergyRecords(string pathToFile)
        {
            List<TxtEnergyRecord> txtEnergyRecordsList = new List<TxtEnergyRecord>();

            string line;

            System.IO.StreamReader file =
                new System.IO.StreamReader(pathToFile);

            // Ingnore first line (headers)
            line = file.ReadLine();

            while ((line = file.ReadLine()) != null)
            {
                txtEnergyRecordsList.Add(new TxtEnergyRecord(line));
            }

            file.Close();

            return txtEnergyRecordsList;
        }
    }
}
