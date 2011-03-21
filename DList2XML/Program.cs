using System;
using System.Collections.Generic;
using System.Text;

namespace DList2XML
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string InputDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string OutputFile = InputDirectory+@"\dlist.xml";
                string[] ExcludedDirectories = new string[0];
                bool Recursive = false;
                bool Verbose = false;

                if (args.Length == 0)
                    DisplayHelp(true);

                foreach (string arg in args)
                {
                    string myArg = arg.Substring(0, 2);
                    switch (myArg)
                    {
                        case "-i":
                            string[] aInput = arg.Split('=');

                            break;

                        case "-o":
                            string[] aOutput = arg.Split('=');

                            break;

                        case "-e":
                            string[] aExclude = arg.Split('=');

                            break;
                        case "-r":
                            Recursive = true;
                            break;
                        case "-v":
                            Verbose = true;
                            break;
                    }
                }


                if (Verbose == true)
                {
                    Console.WriteLine(string.Format(" Input directory is:    {0}", InputDirectory));
                    Console.WriteLine(string.Format(" Output file is:        {0}", OutputFile));
                    Console.WriteLine(string.Format(" Excluded directory is: {0}", ExcludedDirectories.ToString()));
                    Console.WriteLine(string.Format(" Recursive Search:      {0}", Recursive.ToString()));
                    Console.WriteLine(string.Format(" Verbose:               {0}", Verbose.ToString()));
                }
            }
            catch (Exception ex) { Console.WriteLine(string.Format("EXCEPTION: {0}", ex.Message)); }
        }

        private static void DisplayHelp(bool bExit)
        {
            if (bExit == true)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("DList2XML arguments");
                Console.WriteLine("  -i, input directory");
                Console.WriteLine("  -o, output file");
                Console.WriteLine("  -e, excluded directory");
                Console.WriteLine("  -r, recrusive");
                Console.WriteLine("  -v, verbose, print out what is happening");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}
