using System;
using System.IO;

namespace RunbeckCodeExercise
{
    class Program
    {
        static ProcessInfo pi;

        static void Main()
        {
            // display title as the Runbeck Code Excercise.
            Console.WriteLine("\nRunbeck Code Excercise (Process CSV/TSV) in C#\r");
            Console.WriteLine("Written by Mark Freymann 3/13/2020\r");
            Console.WriteLine("----------------------------------------------\n");

            RunProgram();
        }

        static void RunProgram()
        {
            // initialize process
            pi = new ProcessInfo();

            // get required user input and validate
            Console.WriteLine("Begin User Input (enter 'q' to quit)\n");

            // ask the user Where is the file located ?
            pi.FilePath = GetValidFilePath("Where is the file located ? ");

            // ask the user Is the file format CSV(comma - separated values) or TSV(tab-separated values)?
            pi.FileType = GetValidFileType("Is the file format CSV or TSV ? ");

            // ask the user How many fields should each record contain?
            pi.FieldCount = GetValidInteger("How many fields should each record contain ? ");

            // process file
            ProcessFile();

            // ask for another file.  
            if (GetAnotherFile("Would you like to process another file ? ")) RunProgram(); else return;
        }

        static void ProcessFile()
        {
            Console.WriteLine("\nProcess Results:\n");

            // read the file line by line.  
            using (StreamReader file = new StreamReader(pi.FilePath))
            {
                string line;
                while ((line = file.ReadLine()) != null) // loop while line exists
                {
                    pi.Count++; // keep count of all lines
                    if (pi.Count > 1) // skip header row
                    {
                        if (line.Split(pi.Delimiter).Length == pi.FieldCount) // check if user input matches field count
                        { // valid line
                            pi.ValidCount++; // keep count of valid lines
                            // open ValidFile and write line, exit module on error
                            if (!WritetoFile(pi.ValidPath, line, pi.ValidCount > 1)) return;
                        }
                        else
                        { // invalid line
                            pi.InValidCount++; // keep count of invalid lines
                            // open InValidFile and write line, exit module on error
                            if (!WritetoFile(pi.InValidPath, line, pi.InValidCount > 1)) return;
                        }
                    }
                }
            };

            if (pi.Count > 0) // show results of process (if any)
            {
                Console.WriteLine("There were {0} lines.", pi.Count - 1);
                if (pi.ValidCount > 0) Console.WriteLine("{0} Valid written to file '{1}'", pi.ValidCount, pi.ValidPath);
                if (pi.InValidCount > 0) Console.WriteLine("{0} InValid written to file '{1}'", pi.InValidCount, pi.InValidPath);
            }
            else
                Console.WriteLine("There were no lines in file.");

            Console.WriteLine("\nProcess Complete\n");
            Console.WriteLine("----------------------------------------------\n");
        }

        static bool WritetoFile(string Path, string Line, bool Append)
        {
            try
            {
                using (StreamWriter File = new System.IO.StreamWriter(Path, Append))
                {
                    File.WriteLine(Line);
                };
                return true; // success
            }
            catch (UnauthorizedAccessException) // catch if write is denied
            {
                Console.Write("UnAuthorizedAccessException: \nUnable to write to file '{0}'\n", Path);
                Console.WriteLine("----------------------------------------------\n");
                return false; // fail
            }
        }

        #region Validation

        static string GetValidFilePath(string inputQuery)
        {
            // assume the file is valid as long as it exists in the directory (does not check for file extension type)
            Console.Write(inputQuery);
            string val;
            while (true) // continue until valid path or q is entered
            {
                val = Console.ReadLine();
                if (val.ToUpper() == "Q") // user quit
                    Environment.Exit(0);
                else if (!File.Exists(val)) // validate file exists
                    Console.Write("'{0}' is not a valid file location. Please enter valid file location : ", val);
                else // the value is valid file
                {
                    //Console.WriteLine("'{0}' is valid file location.\n", val);
                    break;
                }
            }
            return val;
        }

        static string GetValidFileType(string inputQuery)
        {
            Console.Write(inputQuery);
            string val;
            while (true) // continue until valid type or q is entered
            {
                val = Console.ReadLine().ToUpper();
                if (val == "Q") // user quit
                    Environment.Exit(0);
                else if (val != "CSV" && val != "TSV") // validate file type
                    Console.Write("'{0}' is not a valid file type. Please enter CSV or TSV : ", val);
                else // the value is valid file type
                {
                    //Console.WriteLine("'{0}' is valid file type.\n", val);
                    break;
                }
            }
            return val;
        }

        static int GetValidInteger(string inputQuery)
        {
            Console.Write(inputQuery);
            int val;
            while (true) // continue until valid integer is entered
            {
                string result = Console.ReadLine();
                if (result.ToUpper() == "Q") // user quit
                    Environment.Exit(0);
                else if (!int.TryParse(result, out val)) // validate integer
                    Console.Write("'{0}' is not valid integer : ", result);
                else // the value is an integer
                {
                    //Console.WriteLine("'{0}' is valid integer.\n", val);
                    break;
                }
            }
            return val;
        }

        static bool GetAnotherFile(string inputQuery)
        {
            Console.Write(inputQuery);
            string val;
            while (true) // continue until valid type or q is entered
            {
                val = Console.ReadLine().ToUpper();
                if (val == "Q") // user quit
                    Environment.Exit(0);
                else if (val != "Y" && val != "N") // validate file type
                    Console.Write("'{0}' is not a valid response. Please enter Y or N : ", val);
                else // the value is valid response
                {
                    break;
                }
            }
            return val == "Y";
        }

        #endregion Validation

    }

    class ProcessInfo
    {

        public ProcessInfo()
        {
            FilePath = null;
            FileType = null;
            FieldCount = 0;
            Count = 0;
            ValidCount = 0;
            InValidCount = 0;
        }

        public string FilePath { get; set; }
        public string FileType { get; set; }
        public int FieldCount { get; set; }
        public int Count { get; set; }
        public int ValidCount { get; set; }
        public int InValidCount { get; set; }
        public char[] Delimiter { get { return new char[] { FileType == "CSV" ? ',' : '\t' }; } }
        public string ValidPath
        {
            get { return Path.GetDirectoryName(FilePath) + @"\" + Path.GetFileName(FilePath).Split(".")[0] + "_valid." + FileType.ToLower(); }
        }
        public string InValidPath
        {
            get { return Path.GetDirectoryName(FilePath) + @"\" + Path.GetFileName(FilePath).Split(".")[0] + "_invalid." + FileType.ToLower(); }
        }
    }
}