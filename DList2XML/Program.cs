using System;
using System.Collections.Generic;
using System.Text;

namespace DList2XML
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
                DisplayHelp(true);
            Console.ReadKey();
        }

        private static void DisplayHelp(bool bExit)
        {
            if (bExit == true)
            {
                Console.WriteLine("DList2XML arguments");
                Console.WriteLine("  -i, input directory");
                Console.WriteLine("  -o, output file");
                Console.WriteLine("  -e, excluded directory");
                Console.WriteLine("  -r, recrusive");
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}
