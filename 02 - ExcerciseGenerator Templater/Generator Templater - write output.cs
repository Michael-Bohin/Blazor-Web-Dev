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

        
        StringBuilder promenne = new();

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
            tuplePromenne = RepresentRadu(jmenaPromennych);
                
            SBA($"		public ({tupleRepr}) Unpack() => ({tuplePromenne});\n");
            SBA("	}\n");
            SBA("\n");
        }

        StringBuilder tuplePromenne = new();

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

            SBA("			\n");
            StringBuilder nestedForeach = new();
            int counterOdsazeni = 0;
            foreach(SetDefinition def in pd.valueCombinations) 
                if(def.type != "Op") {
                    nestedForeach.Append($"{tabs[counterOdsazeni+3]}");
                    nestedForeach.Append($"foreach ({def.type} {abeceda[counterOdsazeni]} in mozna{abeceda[counterOdsazeni]})\n");
                    counterOdsazeni++;
				}

            // tohle rozmyslet jak psat jinak 
            // az teprve potom napsat kod ktery generuje tenhle kod @ addSubCombinations
            nestedForeach.Append($"{tabs[counterOdsazeni+3]}foreach((Op o1, Op o2) in addSubCombinations)\n"); 
            counterOdsazeni++;
            nestedForeach.Append($"{tabs[counterOdsazeni+3]}Consider({tuplePromenne});\n");

            SBA(nestedForeach.ToString());
            SBA("\n");

            /// addSubCombinations se pouziva i zde!
            StringBuilder callStats = new();
            callStats.Append($"{tabs[3]}CreateStatsLog(moznaA.Count");

            counterOdsazeni = 1;
            for(int i = 1; i < pd.valueCombinations.Count; ++i) {
                SetDefinition def = pd.valueCombinations[i];
                if(def.type != "Op") {
                    callStats.Append($" , mozna{abeceda[counterOdsazeni]}.Count");
                    counterOdsazeni++;
                }
			}    
            callStats.Append(" , addSubCombinations.Length);\n");

            SBA(callStats.ToString());
            SBA("		}\n");
            SBA("\n");
	    }

        StringBuilder valCombsParametrised = new();

        void Consider() {
            StringBuilder comments = new();
            foreach(string line in contentComments) 
                comments.Append($"{tabs[3]}// {line}\n");

            SBA($"		void Consider({promenne}) {OB}\n");
			SBA(comments.ToString());
            SBA("\n");
            SBA("			int decision = -1;\n");
            SBA($"			if( !(  {contentConstraints[0]} ) )\n");
            SBA("				decision = 0;\n");
            for(int i = 1; i < contentConstraints.Count; ++i) {
                string removedMethodID = contentConstraints[i].StartsWith("<m>") ? $"{contentConstraints[i].Remove(0, 3)}" : $"( {contentConstraints[i]} )";

                SBA($"			else if( !{removedMethodID} )\n");
                SBA($"				decision = {i};\n");
			}
            SBA("			\n");
            SBA("			if(decision != -1) {\n");
            SBA("				illegalCounter[decision]++;\n");
            SBA("				if(illegalCounter[decision] < 1000)\n");

            StringBuilder varNamesWithQCopy = new();
            int counter = 0;
            int OpsCounter = 1;
            foreach(string promenna in poradiPromenych) {
                if(promenna == "Op") {
                    varNamesWithQCopy.Append($"o{OpsCounter}");
                    OpsCounter++;
				}
                if(promenna == "int") 
                    varNamesWithQCopy.Append($"{abeceda[counter]}");
                if(promenna == "Q") 
                    varNamesWithQCopy.Append($"{abeceda[counter]}.Copy()");
				counter++;
                if(counter != poradiPromenych.Count )
                    varNamesWithQCopy.Append(", ");
            }

            SBA($"					illegal[decision].Add( new Zadani_{pd.className}( {varNamesWithQCopy}) );\n");
            SBA("			} else {\n");
            SBA($"				legit.Add( new Zadani_{pd.className}( {varNamesWithQCopy}) );\n");
            SBA("			}\n");
            SBA("		}\n");
            SBA("\n");
	    }

        void ConsiderHelperMethods() {
            // foreach constraing the is described by method (StartsWith("<m>"))
            // write method id with body: "throw new NotImplementedException();"
            foreach(string line in contentConstraints) {
                if (line.StartsWith("<m>")) {
                    SBA($"		static bool{line.Remove(0, 3)} {OB}\n");
                    SBA("		    throw new NotImplementedException();\n");
                    SBA("		}\n");
                    SBA("\n");
				}
			}
	    }

        void Construct() {
            SBA("		/// \n");
            SBA("		/// Kuchařka řešení: Jak se zadání řeší?\n");
            SBA("		///\n");
            SBA($"		protected override Excercise Construct(Zadani_{pd.className} z) {OB} \n");
            SBA($"			({promenne}) = z.Unpack();\n");
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

///            SBA(">>>> Dev Time Control: <<<<\n");
///            SBA(">>>> Dev Time Control: <<<<\n");