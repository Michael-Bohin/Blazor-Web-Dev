using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Templater {
	partial class GeneratorTemplater {
		void ParseIllegalSets() => pd.illegalSets = ParseAt(illSets);
		void ParseStepsCount() => pd.stepsCount = ParseAt(stepsCount);

		int ParseAt(AT at) {
			string line = content[at.At];
			line = line.Trim();

			if( ! int.TryParse(line, out int result))
				throw new InvalidInputException($"\n{at.element} states the number should be: {line}.\nUnfortunatelly, that does not seem to be a number!\n");

			if(result < 1)
				throw new InvalidInputException($"\n{at.element} states the number should be: {line}.\nUnfortunatelly, that does not make any sense at all!\n");

			return result;
		}

		void ParseZadani() { 
			pd.className = contentZadani[0];
			for(int i = 1; i < contentZadani.Count; ++i) 
				AddNextSetDefinition(contentZadani[i]);
		}

		void AddNextSetDefinition(string setDefinition) {
			// first decide if it is going to be defined by some method or as is. 
			// then instantiate apropriate type with correct data 
			// and add it to parsedData.valueCombinations

			string definition;
			string type;
			if(setDefinition.StartsWith("int")) {
				type = "int";
				definition = setDefinition.Remove(0, 3).Trim();
			} else if( setDefinition.StartsWith("Q")) {
				type = "Q";
				definition = setDefinition.Remove(0, 1).Trim();
			} else if( setDefinition.StartsWith("Op")) {
				type = "Op";
				definition = setDefinition.Remove(0, 2).Trim();
			} else {
				throw new InvalidInputException($"Encountered unknown value type in zadani: {setDefinition}\n");
			}

			if(definition.StartsWith("<") && definition.EndsWith(">")) {
				MethodSetDefinition msd = AddNextMethodDefinition(type, definition);
				pd.valueCombinations.Add(msd);
			}else {
				AsIsSetDefinition aisd = new(type, definition);
				pd.valueCombinations.Add(aisd);
			}
				
		}
		
		MethodSetDefinition AddNextMethodDefinition(string type, string definition) {
			definition = definition.Remove(0, 1);
			definition = definition.Remove(definition.Length-1,1);
			string[] defArgs = definition.Split(",");
			string[] methodParameters = new string[defArgs.Length-1];
			for(int i = 1; i < defArgs.Length; ++i)
				methodParameters[i-1] = defArgs[i];

			return new(type, defArgs[0], methodParameters);
		}

		void ParseComments() { }
		void ParseLocalVars() { }
		void ParseConstarints() { }
	}
}
