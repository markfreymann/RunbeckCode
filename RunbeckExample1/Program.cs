using System;
using System.IO;

namespace RunbeckCodeExcercise
{
    class Program
    {
        static ProcessInfo pi;
        static void Main()
        {

            // Declare User Input Variables
            pi = new ProcessInfo
            {
                FilePath = null,
                FileType = null,
                FieldCount = 0
            };

            // Display title as the Runbeck Code Excercise.
            Console.WriteLine("Runbeck Code Excercise (Process CSV/TSV) in C#\r");
            Console.WriteLine("----------------------------------------------\n");

            // get required user input
            Console.WriteLine("Begin User Input (enter 'q' to quit)\r");
            GetUserInput();

            // process file
            Console.WriteLine("Begin Process");
            //UserInput ui = GetUserInput();
        }
        static void GetUserInput()
        {
            // Ask the user Where is the file located ?
            pi.FilePath = GetValidFilePath("Where is the file located ? ");

            // Ask the user Is the file format CSV(comma - separated values) or TSV(tab-separated values)?
            pi.FileType = GetValidFileType("Is the file format CSV(comma - separated values) or TSV(tab-separated values)? [CSV/TSV] ");

            // Ask the user How many fields should each record contain?
            pi.FieldCount = GetValidInteger("How many fields should each record contain ? ");
        }
        static string GetValidFilePath(string inputQuery)
        {
            //Assume the file is valid as long as it exists in the directory (does not check for file extension type)
            Console.Write(inputQuery);
            string val;
            while (true)//continue until valid path or q is entered
            {
                val = Console.ReadLine();
                if (val == "q")
                    Environment.Exit(0);//exit the console app
                else if (!File.Exists(val))
                    Console.Write(val + " is not a valid file location. Please enter valid file location.");
                else // the value is valid file
                    break;
            }
            return val;
        }
        static string GetValidFileType(string inputQuery)
        {
            Console.Write(inputQuery);
            string val;
            while (true)//continue until valid type or q is entered
            {
                val = Console.ReadLine().ToUpper();//convert to upper for validation
                if (val == "q")
                    Environment.Exit(0);//exit the console app
                else if (val != "CSV" && val != "TSV")
                    Console.Write(val + " is not a valid file type. Please enter CSV or TSV.");
                else // the value is valid file type
                    break;
            }
            return val;
        }
        static int GetValidInteger(string inputQuery)
        {
            Console.Write(inputQuery);
            int val;
            while (true)//continue until valid integer is entered
            {
                if (!int.TryParse(Console.ReadLine(), out val))
                    Console.Write("Please enter a valid integer value: ");
                else // the value is an integer
                    break;
            }
            return val;
        }
    }
    class ProcessInfo
    {
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public int FieldCount { get; set; }
    }
}