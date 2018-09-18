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
            System.Console.WriteLine("Creating list of txt objects.");

            // https://github.com/ExcelDataReader/ExcelDataReader/issues/241
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string pathToDir = @"d:\Dysk Google\Praca\ojciec\2018\energia\";
            string pathToEnergyFile = pathToDir + "ENERGIA.TXT";

            List<TxtEnergyRecord> txtEnergyRecordsList = GetTxtEnergyRecords(pathToEnergyFile);

            System.Console.WriteLine("Finding xls file names.");

            // Find xls and xlsx files in given path
            List<string> extensions = new List<string> { ".xls", ".xlsx" };
            var excelFilesList = Directory.GetFiles(pathToDir, "*.*")
                                    .Where(excelFile => extensions.Contains(Path.GetExtension(excelFile).ToLower()));

            System.Console.WriteLine("Creating xls train data objects.");

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
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;

                    reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);
                    var dataTable = dataSet.Tables[recordTableIndex];

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
                }
            }

            // Extract xls data to txt data.
            foreach(TxtEnergyRecord txtEnergyRecord in txtEnergyRecordsList)
            {
                foreach(XlsEnergyRecord xlsEnergyRecord in xlsEnergyRecordsList)
                {
                    txtEnergyRecord.ExtractEligibleData(xlsEnergyRecord);
                }
            }

            // Write it to file
            System.Console.WriteLine("Writing file.");

            string newEnergyFileName = "U_ENERGIA.TXT";
            string newEnergyFilePath = pathToDir + newEnergyFileName;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(newEnergyFilePath, true))
            {

                file.WriteLine("EZT\tt[rok-mi-dz]\tt[h:min]\tEwe[kWh]\tEwy[kWh]\tpozycja\tdriver name\tmanagerName\n");

                int l = 0;

                foreach(TxtEnergyRecord txtEnergyRecord in txtEnergyRecordsList)
                {
                    file.WriteLine(txtEnergyRecord.ToString());
                }
            }
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
