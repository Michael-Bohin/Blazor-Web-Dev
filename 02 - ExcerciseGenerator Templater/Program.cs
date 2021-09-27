using System;
using static System.Console;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Templater {
    class Program {
	    static void Main() {
		    string targetDirectory = @"..\..\..\input";
            // Process the list of files found in the directory.
            string [] fileEntries = Directory.GetFiles(targetDirectory);
            foreach(string fileName in fileEntries)
                ProcessFile(fileName);
	    }

       
        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            WriteLine($"\n\nProcessing file : '{path}'.");
            List<string> inputFile = new();
            try
            {
			    using StreamReader sr = new(path);
			    while (sr.ReadLine() is string line)
                    inputFile.Add(line);
                sr.Dispose();
		    }
            catch (Exception e)
            {
                WriteLine($"\nThe file {path} could not be read:");
                WriteLine(e.Message);
            }

            GeneratorTemplater gt = new(path, inputFile);
            gt.ParseInputData();
            gt.ConstructTemplate();
            gt.SaveTemplate();
        }
    }
} // end namespace Templater