using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;

namespace Templater {
	/// 
	/// Definice vstupu
	/// 
	// na prvni radku je jmeno recordu
	// na kazdem dalsim radku je typ a po mezere definice jak mnozina typu vypada
			
	// Zatim rozeznavane typy: int, Q, Op
	// Rozeznavane typy se jiste hodne rozrostou... 

	// V definici mnoziny jsou dva rezimy
			
	// a) string zacina a konci ostrou zavorkou. 
	//		to znamena ze se bude volat metoda z predem znameho listu. 
	//		Soucasny predem znamy list:
	//			Q -> SOR (SetOfRationals.GetAll(int, int, bool))
	//			int -> GetRange (GetRange(int, int))

	// b) ve vsech ostatnich pripadech se obsah stringu kopiruje as is 

	// radek se splituje na prvni mezere, vse co je za ni spada do druhe casti 
	// prazdne radky se skipuji a jsou povolene 

	// mozne handled exception vedouci na warning a skipnuti celeho filu templaterem: 

	// 1. Kazdy radek prazdny , nebo zadny radek
	// 2. Pocet neprazdnych radku je 1. (Nelze protoze se musi specifikovat alespon jeden nosny typ)
	// 3. Libovolny z dalsich radku neobashuje na zacatku rozeznavany typ
	// 4. Libovolny z dalsich radku neobsahuje popis mnoziny

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
