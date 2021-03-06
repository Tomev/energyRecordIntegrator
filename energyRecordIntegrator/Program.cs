﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;

namespace energyRecordIntegrator
{ 
    class Program
    {
        // For excel file handling
        static int startRow = 4;
        static int trainColumn = 15;
        static int timeStartColumn = 6;
        static int timeFinishColumn = 11;
        static int driverColumn = 32;
        static int managerColumn = 40;
        static int recordTableIndex = 0;
        static int plannedTrainNumberColumn = 2;

        // Configuration data
        static string pathToDir = Directory.GetCurrentDirectory() + @"\";
        static List<string> extensions = new List<string> { ".xls", ".xlsx" };
        static string newFileHeader = "EZT\tt[rok-mi-dz]\tt[h:min]\tEwe[kWh]\tEwy[kWh]\tpozycja\t1 maszynista\tkierownik pociągu\tPlanowy numer pociągu";
        static string newEnergyFileName = "U_ENERGIA.TXT";

        static void Main(string[] args)
        {
            // https://github.com/ExcelDataReader/ExcelDataReader/issues/241
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Console.WriteLine("Creating list of txt objects.");

            string pathToEnergyFile = pathToDir + "ENERGIA.TXT";

            List<TxtEnergyRecord> txtEnergyRecordsList = GetTxtEnergyRecords(pathToEnergyFile);

            Console.WriteLine("Finding xls and xlsx file paths.");

            var excelFilesList = Directory.GetFiles(pathToDir, "*.*")
                                    .Where(excelFile => extensions.Contains(Path.GetExtension(excelFile).ToLower()));

            Console.WriteLine("Creating xls train data objects.");

            List<XlsEnergyRecord> xlsEnergyRecordsList = GetXlsEnergyRecords(excelFilesList);

            Console.WriteLine("Extracting excel data to txt file.");

            foreach(TxtEnergyRecord txtEnergyRecord in txtEnergyRecordsList)
            {
                foreach(XlsEnergyRecord xlsEnergyRecord in xlsEnergyRecordsList)
                {
                    txtEnergyRecord.ExtractEligibleData(xlsEnergyRecord);
                }
            }

            Console.WriteLine("Writing file.");

            string newEnergyFilePath = pathToDir + newEnergyFileName;

            using (StreamWriter file = new StreamWriter(newEnergyFilePath, true, System.Text.Encoding.Default))
            {
                file.WriteLine(newFileHeader);

                foreach(TxtEnergyRecord txtEnergyRecord in txtEnergyRecordsList)
                {
                    file.WriteLine(txtEnergyRecord.ToString());
                }
            }

            Console.WriteLine("File created successfully.");
        }

        static private List<TxtEnergyRecord> GetTxtEnergyRecords(string pathToFile)
        {
            List<TxtEnergyRecord> txtEnergyRecordsList = new List<TxtEnergyRecord>();

            string line;

            StreamReader file = new StreamReader(pathToFile, System.Text.Encoding.GetEncoding("windows-1250"));

            // Ingnore first line (headers)
            line = file.ReadLine();

            while ((line = file.ReadLine()) != null)
            {
                txtEnergyRecordsList.Add(new TxtEnergyRecord(line));
            }

            file.Close();

            return txtEnergyRecordsList;
        }

        static private List<XlsEnergyRecord> GetXlsEnergyRecords(IEnumerable<string> excelFilesList)
        {
            List<XlsEnergyRecord> xlsEnergyRecordsList = new List<XlsEnergyRecord>();

            foreach (string excelFilePath in excelFilesList)
            {

                Console.WriteLine("Extracting data from: {0}.", excelFilePath);
                int j = 0;

                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;

                    reader = ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);
                    var dataTable = dataSet.Tables[recordTableIndex];

                    // Note that there's additional condition for for to exit.
                    for (int i = startRow; i < dataTable.Rows.Count && !dataTable.Rows[i][timeStartColumn].ToString().Equals(""); ++i)
                    {
                        xlsEnergyRecordsList.Add(
                            new XlsEnergyRecord(
                                dataTable.Rows[i][timeStartColumn].ToString(),
                                dataTable.Rows[i][timeFinishColumn].ToString(),
                                dataTable.Rows[i][driverColumn].ToString(),
                                dataTable.Rows[i][managerColumn].ToString(),
                                dataTable.Rows[i][trainColumn].ToString(),
                                dataTable.Rows[i][plannedTrainNumberColumn].ToString()
                                )
                            );
                    }
                }
            }

            return xlsEnergyRecordsList;
        }

    }
}
