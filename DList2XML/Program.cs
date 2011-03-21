using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DList2XML
{
    class Program
    {
        static string InputDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string OutputFile = InputDirectory + @"\dlist.xml";
        static bool OverwriteOutput = false;
        static string[] ExcludedDirectories = new string[0];
        static bool Recursive = false;
        static bool Verbose = false;
        static ArrayList filelist = new ArrayList();

        static void Main(string[] args)
        {
            try
            {
                

                if (args.Length == 0)
                    DisplayHelp(true);

                foreach (string arg in args)
                {
                    string myArg = arg.Substring(0, 2);
                    switch (myArg)
                    {
                        case "-i":
                            string[] aInput = arg.Split('=');
                            InputDirectory = aInput[1];
                            break;
                        case "-w":
                            OverwriteOutput=true;
                            break;
                        case "-o":
                            string[] aOutput = arg.Split('=');
                            OutputFile = aOutput[1];                            
                            break;

                        //case "-e":
                        //    string[] aExclude = arg.Split('=');

                        //    break;
                        case "-r":
                            Recursive = true;
                            break;
                        case "-v":
                            Verbose = true;
                            break;
                        default:
                            throw new Exception(string.Format("Unknown argument \"{0}\". Exiting", myArg));
                    }
                }

                // Validate input                
                if (!Directory.Exists(InputDirectory))
                    throw new Exception("Input directory does not exist");

                if (Directory.Exists(Path.GetDirectoryName(OutputFile)))
                {
                    if (OverwriteOutput == false)
                        if (File.Exists(OutputFile))
                            throw new Exception("Output file exists. To overwrite use -w");
                }
                else
                    throw new Exception("Output directory does not exist");
                
                

                if (Verbose == true)
                {
                    Console.WriteLine(string.Format(" Input directory is:    {0}", InputDirectory));
                    Console.WriteLine(string.Format(" Output file is:        {0}", OutputFile));
                    Console.WriteLine(string.Format(" Output file overwrite: {0}", OverwriteOutput.ToString()));
                    //Console.WriteLine(string.Format(" Excluded directory is: {0}", ExcludedDirectories.ToString()));
                    Console.WriteLine(string.Format(" Recursive Search:      {0}", Recursive.ToString()));
                    Console.WriteLine(string.Format(" Verbose:               {0}", Verbose.ToString()));
                }

                ProcessDirectory(InputDirectory);

                ClearCurrentConsoleLine();
                Console.WriteLine(string.Format("Found {0} files.", filelist.Count));

                ProcessFileList();

                //Console.Write("\nPress any key to exit...");
                //Console.ReadKey();
            }
            catch (Exception ex) { Console.WriteLine(string.Format("EXCEPTION: {0}", ex.Message)); }
        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            ClearCurrentConsoleLine();
            Console.Write(string.Format("Processing directory: {0}", targetDirectory));
            //Console.SetCursorPosition(0, Console.CursorTop - 1);
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            if (Recursive == true)
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            //Console.WriteLine("Processed file '{0}'.", path);
            filelist.Add(path);
        }

        private static void ProcessFileList()
        {
            Int64 totalsize = 0;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineOnAttributes = false;

            using (XmlWriter writer = XmlWriter.Create(OutputFile, settings))
            {
                writer.WriteStartElement("FileListing");

                for (int fileindex = 0; fileindex < filelist.Count; fileindex++)
                {
                    writer.WriteStartElement("File");
                    //writer.WriteAttributeString("FileID", "1");
                    FileInfo fi = new FileInfo(filelist[fileindex].ToString());
                    totalsize = totalsize + fi.Length;
                    writer.WriteElementString("HASH", GenerateHash(fi.FullName));
                    writer.WriteElementString("Name", fi.Name);
                    writer.WriteElementString("Path", fi.DirectoryName);
                    writer.WriteElementString("Length", fi.Length.ToString());
                    writer.WriteElementString("LastModified", fi.LastWriteTimeUtc.ToString());

                    //Console.WriteLine(fi.Length.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();
            }

            Console.WriteLine(string.Format("total size: {0} bytes.", totalsize.ToString()));
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
                Console.WriteLine("  -w, overwrite output file");
                //Console.WriteLine("  -e, excluded directory");
                Console.WriteLine("  -r, recrusive");
                Console.WriteLine("  -v, verbose, print out what is happening");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static string GenerateHash(string value)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(value);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}
