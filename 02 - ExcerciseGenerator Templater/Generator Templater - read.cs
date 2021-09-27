using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Templater {
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

		/*void VypisContents() { // !!! jen pro dev time !!!
            for(int i = 0; i < content.Count-1; i++)
                sb.Append(content[i] + "\n");
            sb.Append(content[^1]);
            sb.Append("\n>>>>>>>>___________<<<<<\n");
		}*/
	}
}
