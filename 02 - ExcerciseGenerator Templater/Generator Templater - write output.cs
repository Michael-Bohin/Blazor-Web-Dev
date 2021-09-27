using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Templater {
	// methods processing writting output:
	partial class GeneratorTemplater {
		readonly string OB = @"{"; // open curly brace 
		private readonly string CB = @"}"; // close curly brace
        readonly string QM2x = @""""""; // two quotation marks  
        readonly string QM = @"""";

        void SBA(string s) => sb.Append(s);

        int intCount = 0;
        int QCount = 0;
        int OpCount = 0;
        
        List<string> poradiPromenych = new();
        List<string> jmenaQ = new();
        List<string> jmenaInt = new();

        readonly List<string> abeceda = new() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L",/* Carefull M would violate shortcut for MathAlgorithms */ "M", "N", "O", "P" };

        void ConstructTemplate_() {
            WriteLine("... Preparing template ...");

            DevTimeInfo();
            
            DefineZadaniVariables();

            FixedHeader();
            RecordZadani();
            HeaderTridy();
            Constructor();
            Consider();
            ConsiderHelperMethods();
            Construct();
		}

        void DevTimeInfo() {
            foreach(var item in pd.valueCombinations) {
                 WriteLine(item);
                 if(item is MethodSetDefinition msd)
                    foreach(var item2 in msd.methodParameters)
                        WriteLine(item2);
			}
		}

        void DefineZadaniVariables() {
            for(int i = 0; i < pd.valueCombinations.Count; ++i) {
                string type = pd.valueCombinations[i].type;
                if(type == "int") {
                    intCount++;
                    poradiPromenych.Add("int");
				} else if(type == "Q") {
                    QCount++;
                    poradiPromenych.Add("Q");
				} else if(type == "Op") {
                    OpCount++;
                    poradiPromenych.Add("Op");
				} else {
                    throw new InvalidInputException("DefineZadaniVariables metoda kriticky selhala. Do debug this.");
				}
            }

            int counter = 0;
            foreach(string promenna in poradiPromenych) {
                if(promenna == "Op")
                    continue;
                if(promenna == "int") 
                    jmenaInt.Add(abeceda[counter]);
                if(promenna == "Q") 
                    jmenaQ.Add(abeceda[counter]);
				counter++;
            }
		}

        void FixedHeader() {
            SBA("using System.Collections.Generic;\n");
            SBA("\n");
            SBA("namespace InfiniteEngine\n");
            SBA("{\n");
            SBA("	using Q = RationalNumber;\n");
            SBA("	using M = MathAlgorithms;\n\n");
	    }

        

        void RecordZadani() {
            SBA($"	public record Zadani_{pd.className}\n");
            SBA("	{\n");

            AppendTypesDefs(jmenaInt, "int");
            AppendTypesDefs(jmenaQ, "Q");
            if(0 < OpCount) {
                SBA($"		public readonly Op o1");
                for(int i = 1; i < OpCount; ++i)
                    SBA($", o{i+1}");
                SBA(";\n");
			}
            SBA("\n");
            StringBuilder promenne = new();

            List<string> jmenaPromennych = new();

            promenne.Append($"{poradiPromenych[0]} A");
            jmenaPromennych.Add("A");
            int j = 1;
            for(int i = 1; i < poradiPromenych.Count; ++i) {
                if(poradiPromenych[i] != "Op") {
                    promenne.Append($", {poradiPromenych[j]} {abeceda[j]}");
                    jmenaPromennych.Add(abeceda[j]);
                    j++;
				}  
			}

            for(int i = 0; i < OpCount; ++i) {
                promenne.Append($", Op o{i+1}");
                jmenaPromennych.Add($"o{i+1}");
			}
            
            SBA($"		public Zadani_{pd.className}({promenne}) {OB}\n");

            StringBuilder ctorAssignments = new();
            bool first = true;
            foreach(string jmeno in jmenaPromennych) {
                if(first) 
                    first = false;
				else 
                    ctorAssignments.Append(' '); // pred kazdy neprvni assignment pridej whitespace
				
                ctorAssignments.Append($"this.{jmeno} = {jmeno};");
			}
            
            SBA($"			{ctorAssignments}\n");
	        SBA("		}\n");
            SBA("\n");

            StringBuilder tupleRepr = RepresentRadu(poradiPromenych);
            StringBuilder tuplePromenne = RepresentRadu(jmenaPromennych);
                
            SBA($"		public ({tupleRepr}) Unpack() => ({tuplePromenne});\n");
            SBA("	}\n");
            SBA("\n");
        }

        void AppendTypesDefs(List<string> list, string type) {
            if(0 < list.Count) {
                SBA($"		public readonly {type} {list[0]}");
                for(int i = 1; i < list.Count; ++i)
                    SBA($", {list[i]}");
                SBA(";\n");
			}
		}

        static StringBuilder RepresentRadu(List<string> rada) {
            StringBuilder result = new();
            bool first = true;
            foreach(string s in rada) {
                if(first) 
                    first = false;
				else 
                    result.Append(", ");
                result.Append(s);
			}
            return result;
		}

        void HeaderTridy() {
            SBA($"	public class EGenerator_{pd.className} : ExcerciseGenerator <Zadani_{pd.className}>\n");
            SBA("	{\n");
            SBA("		/// \n");
            SBA("		/// Jaká je množina pedagogicky legit zadání?\n");
            SBA("		/// \n");
	    }

        void Constructor() {
            SBA($"		public EGenerator_{pd.className}() : base({pd.illegalSets}) {OB}\n");

            string ctorOdsazeni = "			";
            StringBuilder inicializaceLists = new();
            int counter = 0;
            foreach(SetDefinition def in pd.valueCombinations)
                if(def.type != "Op") {
                    inicializaceLists.Append($"{ctorOdsazeni}List<{def.type}> mozna{abeceda[counter]} = {def.GetDefinition()};\n");
                    counter++;
				}
             
            SBA(inicializaceLists.ToString());

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

        void Consider() {
            SBA("		void Consider(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {\n");
            SBA("			// masivni constraint: (A-B) * C == 1\n");
            SBA("			// D y E je ruzne od nuly\n");
            SBA("			// D.Den != E.Den\n");
            SBA("			// Vysledek nalezi do EasyZT\n");
            SBA("\n");
            SBA("			int decision = -1;\n");
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
            SBA($"					illegal[decision].Add( new Zadani_{pd.className}( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );\n");
            SBA("			} else {\n");
            SBA($"			legit.Add( new Zadani_{pd.className}( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );\n");
            SBA("			}\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void ConsiderHelperMethods() {
            SBA("		static bool VysledekNaleziDoMnozinyEasyZlomky(Q D, Q E, Op o) {\n");
            SBA("			Q result = D.Operate(E, o).GetInverse();\n");
            SBA("			return IsEasyZt(result.GetSimplestForm());\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void Construct() {
            SBA("		/// \n");
            SBA("		/// Kuchařka řešení: Jak se zadání řeší?\n");
            SBA("		///\n");
            SBA($"		protected override Excercise Construct(Zadani_{pd.className} z) {OB}\n");
            SBA("			(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) = z.Unpack();\n");
            SBA($"			string[] steps = new string[{pd.stepsCount}];\n");
            SBA($"			string[] comments = new string[{pd.stepsCount}];\n");
            SBA("\n\n");

            for(int i = 0; i < pd.stepsCount-1; ++i) // all except last
                AppendStep(i);

            SBA($"			// Step {pd.stepsCount}:\n");
            SBA($"			Q result = (Q)1;\n");
            SBA($"			steps[{pd.stepsCount-1}] = ${QM2x};\n");
            SBA($"			comments[{pd.stepsCount-1}] = {QM}Hotovo! 😎😎{QM};\n");
            SBA("			return new EFractions_S02(steps, comments, result);\n");
            SBA("		}\n");
            SBA("	}\n");
            SBA("}\n");
	    }

        void AppendStep(int i) {
            SBA($"			// Step {i+1}:\n");
            SBA($"			steps[{i}] = ${QM2x};\n");
            SBA($"			comments[{i}] = ${QM2x};\n");
            SBA("\n");
		}
	}
}
