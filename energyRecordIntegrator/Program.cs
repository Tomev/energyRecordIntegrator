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
            // https://github.com/ExcelDataReader/ExcelDataReader/issues/241
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string pathToDir = @"d:\Dysk Google\Praca\ojciec\2018\energia\";
            string pathToEnergyFile = pathToDir + "ENERGIA.TXT";

            List<TxtEnergyRecord> txtEnergyRecordsList = GetTxtEnergyRecords(pathToEnergyFile);

            // Find xls and xlsx files in given path
            List<string> extensions = new List<string> { ".xls", ".xlsx" };
            var excelFilesList = Directory.GetFiles(pathToDir, "*.*")
                                    .Where(excelFile => extensions.Contains(Path.GetExtension(excelFile).ToLower()));

            // Use this list to gather data of trains.

            List<XlsEnergyRecord> xlsEnergyRecordsList = new List<XlsEnergyRecord>();

            int startRow = 4;
            int trainColumn = 15;
            int timeStartColumn = 6;
            int timeFinishColumn = 11;
            int driverColumn = 32;
            int managerColumn = 40;
            int recordTableIndex = 0;

           
            foreach(string excelFilePath in excelFilesList)
            {
                System.Console.WriteLine(excelFilePath);

                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;

                    reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                    //// reader.IsFirstRowAsColumnNames
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);
                    var dataTable = dataSet.Tables[0];

                    for(int i = startRow; i < dataTable.Rows.Count; ++i)
                    {
                        xlsEnergyRecordsList.Add(
                            new XlsEnergyRecord(
                                dataTable.Rows[i][timeStartColumn].ToString(),
                                dataTable.Rows[i][timeFinishColumn].ToString(),
                                dataTable.Rows[i][driverColumn].ToString(),
                                dataTable.Rows[i][managerColumn].ToString(),
                                dataTable.Rows[i][trainColumn].ToString()
                                )
                            );
                    }

                    //var data = dataTable.Rows[4][40];

                    System.Console.WriteLine(xlsEnergyRecordsList.Count);
                    //...
                }
            }
            
            System.Console.WriteLine("There were {0} lines in TXT file.", txtEnergyRecordsList.Count);
            System.Console.WriteLine("{0} excel files found.\n", excelFilesList.Count());
            System.Console.WriteLine("{0} excel objects created.", xlsEnergyRecordsList.Count);

            System.Console.WriteLine(txtEnergyRecordsList[0].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[1].ToString());
            System.Console.WriteLine(txtEnergyRecordsList[2].ToString());

            System.Console.WriteLine(xlsEnergyRecordsList[0].ToString());
            System.Console.WriteLine(xlsEnergyRecordsList[1].ToString());
            System.Console.WriteLine(xlsEnergyRecordsList[2].ToString());

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
