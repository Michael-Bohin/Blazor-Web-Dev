using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Templater {
	partial class GeneratorTemplater {
		readonly string[] tabs = new string[] { 
			"", // 0
			"	", // 1
			"		",
			"			", // 3
			"				", // 4 
			"					", // 5
			"						", // 6
			"							", // 7
			"								", // 8
			"									", // 9
			"										", // 10
			"											", // 11
			"												" // 12
		};
	}
}
