using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Templater {	
	partial class GeneratorTemplater {
        void ParseInputData_() {
			WriteLine("... Processing input data ... ");
			// VypisContents(); // jen pro dev time
			
			try {
				// prepare input
				IdentifyAndAssertElements();
				PrepareContent();

				// parse individual sections of input
				ParseZadani();
				ParseIllegalSets();
				ParseComments();
				ParseLocalVars();
				ParseConstarints();
				ParseStepsCount();
			}
			catch(Exception e) {
				permissionToWrite = false;
				WriteLine($"\nThe file {path} contains one or more problems.\n Parsing input was not successful.");
                WriteLine(e.Message);
			}
	    }

		/* !!! jen pro dev time !!!
		 * 
		 * void VypisContents() { 
            for(int i = 0; i < content.Count-1; i++)
                sb.Append(content[i] + "\n");
            sb.Append(content[^1]);
            sb.Append("\n>>>>>>>>___________<<<<<\n");
		}*/
	}
}
