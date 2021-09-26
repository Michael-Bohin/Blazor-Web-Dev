using System;
using static System.Console;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Templater {



    class GeneratorTemplater {
        readonly string path;
        readonly List<string> content;
        StringBuilder sb = new();


        public GeneratorTemplater(string path, List<string> content) {
            this.path = path;
            this.content = content;
	    }

        public void PrepareTemplate() {
            WriteLine("... Preparing template ...");
            for(int i = 0; i < content.Count-1; i++)
                sb.Append(content[i] + "\n");
        
            sb.Append(content[^1]);

            ProcessData();

            sb.Append("\n>>>>>>>>___________<<<<<\n");

            AppendFixedHeader();
            AppendRecordZadani();
            AppendHeaderTridy();
            AppendConstructor();
            AppendConsider();
            AppendConsiderHelperMethods();
            AppendConstruct();
	    }

        public void SaveTemplate() {
             WriteLine("... Saving template ...");
            // get proper file name 
            string fileName = @"..\..\..\output-templates\" + Path.GetFileName(path);
            using StreamWriter sw = new(fileName);
            sw.Write(sb.ToString());
	    }

        readonly string OB = @"{"; // open curly brace 
        readonly string CB = @"}"; // close curly brace
        readonly string QM2x = @""""""; // two quotation marks  
        readonly string QM = @"""";

        void SBA(string s) => sb.Append(s);

        void ProcessData() {
            WriteLine("... Processing data ... ");
	    }

        void AppendFixedHeader() {
            SBA("using System.Collections.Generic;\n");
            SBA("\n");
            SBA("namespace InfiniteEngine\n");
            SBA("{\n");
            SBA("	using Q = RationalNumber;\n");
            SBA("	using M = MathAlgorithms;\n\n");
	    }

        void AppendRecordZadani() {
            SBA("	public record Zadani_Fractions_S02_E\n");
            SBA("   {\n");
            SBA("		public readonly int A;\n");
            SBA("		public readonly Q B, C, D, E;\n");
            SBA("		public readonly Op o1, o2;\n");
            SBA("\n");
            SBA("		public Zadani_Fractions_S02_E(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {\n");
            SBA("			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.o1 = o1; this.o2 = o2;\n");
	        SBA("		}\n");
            SBA("\n");
            SBA("		public (int, Q, Q, Q, Q, Op, Op) Unpack() => (A, B, C, D, E, o1, o2);\n");
            SBA("	}\n");
            SBA("\n");
         }

        void AppendHeaderTridy() {
            SBA("	public class EGenerator_Fractions_S02_E : ExcerciseGenerator <Zadani_Fractions_S02_E>\n");
            SBA("	{\n");
            SBA("		/// \n");
            SBA("		/// Jaká je množina pedagogicky legit zadání?\n");
            SBA("		/// \n");
	    }

        void AppendConstructor() {
            SBA("		public EGenerator_Fractions_S02_E() : base(4) {\n");
            SBA("			List<int> moznaA = GetRange(2, 10);\n");
            SBA("			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);\n");
            SBA("			List<Q> moznaC = SetOfRationals.GetAll(1, 9, true);\n");
            SBA("			List<Q> moznaD = SetOfRationals.GetAll(1, 9, true);\n");
            SBA("			List<Q> moznaE = SetOfRationals.GetAll(1, 9, true);\n");
            SBA("\n");
            SBA("			foreach (int A in moznaA)\n");
            SBA("				foreach (Q B in moznaB)\n");
            SBA("					foreach (Q C in moznaC)\n");
            SBA("						foreach (Q D in moznaD)\n");
            SBA("							foreach(Q E in moznaE)\n");
            SBA("								foreach((Op o1, Op o2) in addSubCombinations)\n");
            SBA("									Consider(A, B, C, D, E, o1, o2);\n");
            SBA("\n");
            SBA("			CreateStatsLog(moznaA.Count , moznaB.Count , moznaC.Count , moznaD.Count , moznaE.Count , addSubCombinations.Length);\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void AppendConsider() {
            SBA("		void Consider(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {\n");
            SBA("			// masivni constraint: (A-B) * C == 1\n");


            SBA("			// D y E je ruzne od nuly\n");
            SBA("			// D.Den != E.Den\n");
            SBA("			// Vysledek nalezi do EasyZT\n");
            SBA("\n");
            SBA("           int decision = -1;\n");
            SBA("			if( !( ((Q)A-B) * C == (Q)1 ) )\n");
            SBA("				decision = 0;\n");
            SBA("			else if( !( (D.Operate(E, o2)).Num != 0 )  )\n");
            SBA("				decision = 1;\n");
            SBA("			else if( !( D.Den != E.Den ) )\n");
            SBA(" 				decision = 2;\n");
            SBA("			else if( ! VysledekNaleziDoMnozinyEasyZlomky(D, E, o2) )\n");
            SBA("				decision = 3;\n");
            SBA("\n");
            SBA("			if(decision != -1) {\n");
            SBA("				illegalCounter[decision]++;\n");
            SBA("				if(illegalCounter[decision] < 1000)\n");
            SBA("					illegal[decision].Add( new Zadani_Fractions_S02_E( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );\n");
            SBA("			} else {\n");
            SBA("				legit.Add( new Zadani_Fractions_S02_E( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );\n");
            SBA("			}\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void AppendConsiderHelperMethods() {
            SBA("       static bool VysledekNaleziDoMnozinyEasyZlomky(Q D, Q E, Op o) {\n");
            SBA("			Q result = D.Operate(E, o).GetInverse();\n");
            SBA("			return IsEasyZt(result.GetSimplestForm());\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void AppendConstruct() {
            SBA("       /// \n");
            SBA("		/// Kuchařka řešení: Jak se zadání řeší?\n");
            SBA("		///\n");
            SBA("		protected override Excercise Construct(Zadani_Fractions_S02_E z) {\n");
            SBA("			(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) = z.Unpack();\n");
            SBA("			string[] steps = new string[7];\n");
            SBA("			string[] comments = new string[7];\n");
            SBA("			int rightLCD = M.EuclidsLCM(D.Den, E.Den);\n");
            SBA("\n");
            SBA("			// Step 1:\n");
            SBA($"			steps[0] = ${QM2x};\n");
            SBA($"			comments[0] = ${QM2x};\n");
            SBA("\n");
            SBA("			// Step 2:\n");
            SBA($"			steps[1] = ${QM2x};\n");
            SBA($"			comments[1] = ${QM2x};\n");
            SBA("\n");
            SBA("			// Step 3:\n");
            SBA($"			steps[2] = ${QM2x};\n");
            SBA($"			comments[2] = ${QM2x};\n");
            SBA("\n");
            SBA("           // Step 4:\n");
            SBA($"			steps[3] = ${QM2x};\n");
            SBA($"			comments[6] = {QM}Hotovo! 😎😎{QM};\n");
            SBA("			return new EFractions_S02(steps, comments, D);\n");
            SBA("		}\n");
            SBA("	}\n");
            SBA("}\n");
            SBA("\n");
	    }
    }

    class Program {
	    static void Main() {
		    string inputFolder = @"..\..\..\input";
            ProcessDirectory(inputFolder);
	    }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string [] fileEntries = Directory.GetFiles(targetDirectory);
            foreach(string fileName in fileEntries)
                ProcessFile(fileName);
        }


        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            WriteLine($"\nProcessing file : '{path}'.");
            try
            {
			    using StreamReader sr = new(path);
                List<string> inputFile =new();
			
			    while (sr.ReadLine() is string line)
                    inputFile.Add(line);
                sr.Dispose();

                GeneratorTemplater gt = new(path, inputFile);
                gt.PrepareTemplate();
                gt.SaveTemplate();
            
		    }
            catch (Exception e)
            {
                WriteLine("The file could not be read:");
                WriteLine(e.Message);
            }

        }
    }
} // end namespace Templater