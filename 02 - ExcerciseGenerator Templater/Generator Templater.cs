using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;

namespace Templater {

	partial class GeneratorTemplater {
        readonly string path;
        readonly List<string> content;
        readonly StringBuilder sb = new();
        bool permissionToWrite = true;

		readonly FT zadani = new("zadani"); 
	    readonly AT illSets = new("illSets"); 
		readonly FT localVars = new("localVars"); 
		readonly FT comments = new("comments");  
		readonly FT constraints = new("constraints"); 
		readonly AT stepsCount = new("stepsCount"); 

        private readonly List<string> contentZadani = new();
		private readonly List<string> contentComments = new();
		private readonly List<string> contentLocalVars = new();
		private readonly List<string> contentConstraints = new();

        private readonly ParsedData pd = new();

        public GeneratorTemplater(string path, List<string> content) {
            this.path = path;
            this.content = content;
	    }

        public void ParseInputData() => ParseInputData_(); // lives inside seperate partial file

        public void ConstructTemplate() {
            if(permissionToWrite)
                ConstructTemplate_(); // lives inside seperate partial file
	    }

        public void SaveTemplate() {
            if(permissionToWrite) {
                WriteLine("... Saving template ...");
                // get proper file name 
                string fileName = @"..\..\..\output-templates\" + Path.GetFileName(path);
                using StreamWriter sw = new(fileName);
                sw.Write(sb.ToString());
			}
	    }
    }
}
