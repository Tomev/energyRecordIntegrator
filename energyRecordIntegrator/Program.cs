using System;

namespace energyRecordIntegrator
{ 
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDir = @"d:\Dysk Google\Praca\ojciec\2018\energia\";
            string pathToFile = pathToDir + "ENERGIA.TXT";

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(pathToFile);
            while ((line = file.ReadLine()) != null)
            {
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);


            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
