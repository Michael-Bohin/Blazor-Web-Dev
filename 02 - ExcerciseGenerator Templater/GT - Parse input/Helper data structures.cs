using System;
using System.Collections.Generic;
using static System.Console;

namespace Templater {
	class InvalidInputException : Exception {
		public InvalidInputException() { }
		public InvalidInputException(string s) : base(s) { }
	}

	record FT { // pro parove elementy
		private int _from = -1;
		public int From {
			get => _from;
		}

		private int _to = -1;
		public int To {
			get => _to;
		}

		public readonly string element;

		public FT(string s) => element = s;

		// navic oproti At, From musi byt nastevene pred To  a To musi byt nastavene po From

		public void SetFrom(int from) {
			if(this.From != -1)
				throw new InvalidInputException($"\nFound two different lines with binary opening {element} element.\nFirst occurence at: {this.From}\nSecond occurence at: {from}\n");
			if(this.To != -1)
				throw new InvalidInputException($"\nAttempted to set opening element with closing bracket already in place of element: {element}.\nFrom: {this.From}, To: {this.To}, from: {from}");
			
			_from = from;
		}

		public void SetTo(int to) {
			if(this.To != -1)
				throw new InvalidInputException($"\nFound two different lines with binary opening {element} element.\nFirst occurence at: {this.To}\nSecond occurence at: {to}\n");
			if(this.From == -1)
				throw new InvalidInputException($"\nAttempted to set closing element with epening element not yet in place of element: {element}.\nFrom: {this.From}, To: {this.To}, to: {to}");
			_to = to;
		}

		public void AssertIsSet() {
			if(this.From == -1 || this.To == -1) 
				throw new InvalidInputException($"\nEither From or To of element: {element} has not been found in the input document.\nFrom value: {From}\nTo value: {To}\n");
		}
	}

	record AT { // pro unarni elementy 
		private int _at = -1;
		public int At {
			get => _at;
		}

		public readonly string element;

		public AT(string s) => element = s;
			
		public void SetAt(int at) {
			//WriteLine($">>> Debug info 4: {element}   {At}");
			if(this.At != -1)
				throw new InvalidInputException($"\nFound two different lines with unary {element} element.\nFirst occurence at: {this.At}\nSecond occurence at: {at}\n");
			_at = at + 1; // unarni elementy maji hodnotu na radku nasledujicim po samotnem elementu
		}

		public void AssertIsSet() {
			//WriteLine($">>> Debug info 2: {element}   {At}");
			if(this.At == -1) 
				throw new InvalidInputException($"\nUnary element: {element} has not been found in the input document.\n");
		}
	}

	abstract record SetDefinition {
		// known types: Q, int, Op
		public readonly string type;
		private readonly List<string> knownTypes = new() { "Q", "int", "Op"};
		
		public SetDefinition(string type) {
			TypeIsKnown(type);
			this.type = type;
		}

		void TypeIsKnown(string type) {
			if(!knownTypes.Contains(type))
				throw new InvalidInputException($"\nUnknown type in <Zadani> detected. Type: {type}\n");
		}

		abstract public string GetDefinition();
	}

	record MethodSetDefinition : SetDefinition {
		public readonly string methodName;
		public readonly string[] methodParameters;

		private readonly List<string> knownQMethods = new() { "SOR" };
		private readonly List<string> knownIntMethods = new() { "GetRange" };
		private readonly List<string> knownOpMethods = new() { }; // yet empty

		public MethodSetDefinition(string type, string methodName, string[] methodParameters) : base(type) {
			MethodNameIsKnown(methodName);
			this.methodName = methodName;
			this.methodParameters = methodParameters;
		}
		
		void MethodNameIsKnown(string methodName) {
			if(type == "Q" && !knownQMethods.Contains(methodName))
				throw new InvalidInputException($"\nType 'Q' does not acknowledge method name: {methodName}. Kindly rephrase your demand.\n");
			
			if(type == "int" && !knownIntMethods.Contains(methodName))
				throw new InvalidInputException($"\nType 'int' does not acknowledge method name: {methodName}. Kindly rephrase your demand.\n");

			if(type == "Op" && !knownOpMethods.Contains(methodName))
				throw new InvalidInputException($"\nType 'Op' does not acknowledge method name: {methodName}. Kindly rephrase your demand.\n");
		}

		public override string GetDefinition() {
			if(type == "Op")
				throw new InvalidInputException("record MethodSetDefinition does not yet acknoledge enypredfined methods of type 'Op'.\n");
			
			string result = "UNEXPECTED BEHAVIOUR";
			if(type == "Q" && methodName == "SOR") {
				result = $"SetOfRationals.GetAll({methodParameters[0]}";
			} else if(type == "int" && methodName == "GetRange") {
				result = $"GetRange({methodParameters[0]}";
			} else {
				throw new InvalidInputException("record MethodSetDefinition does not recognise command. Please rephrase your demand.\n");
			}

			for(int i = 1; i < methodParameters.Length; ++i) 
					result += $", {methodParameters[i]}";
			result += ")";

			return result;
		}

	}

	record AsIsSetDefinition : SetDefinition {
		public readonly string definition;

		public AsIsSetDefinition(string type, string definition) : base(type) {
			this.definition = definition;
		}

		public override string GetDefinition() => definition;
	}

	record ParsedData {
		public string className = "";
        public List<SetDefinition> valueCombinations = new();
		public List<string> localVars = new();
        public List<string> comments = new();
		public List<string> constraints = new();
		public int illegalSets = -1; 
		public int stepsCount = -1;
	}
}