using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Templater {
	partial class GeneratorTemplater {
		void IdentifyAndAssertElements() {
			// pro kazdy z ocekavanych elementu zkopiruj jejich radky do jim dedikovaneho List<string>
			// pokud zjistit nespravny pocet elementu, vyhod vyjimku. 
			for(int i = 0; i < content.Count; ++i) {
				string line = content[i].Trim(); // allow whitespace before and after
				if(line == "")continue;

				if(line == "<Zadani>") 
					zadani.SetFrom(i);
				if(line == "<EndZadani>") 
					zadani.SetTo(i);

				if(line == "<IllegalSets>")
					illSets.SetAt(i);

				if(line == "<Comments>") 
					comments.SetFrom(i);
				if(line == "<EndComments>")
					comments.SetTo(i);

				if(line == "<LocalVars>")
					localVars.SetFrom(i);
				if(line == "<EndLocalVars>")
					localVars.SetTo(i);

				if(line == "<Constraints>")
					constraints.SetFrom(i);
				if(line == "<EndConstraints>")
					constraints.SetTo(i);

				if(line == "<StepsCount>")
					stepsCount.SetAt(i);
			}

			zadani.AssertIsSet();
			illSets.AssertIsSet();
			comments.AssertIsSet();
			localVars.AssertIsSet();
			constraints.AssertIsSet();
			stepsCount.AssertIsSet();
		}

		void PrepareContent() {
			// now we have guarantee all elements have been identified and are correct 
			CopyDedicatedContent(zadani, contentZadani);
			CopyDedicatedContent(comments, contentComments);
			CopyDedicatedContent(localVars, contentLocalVars);
			CopyDedicatedContent(constraints, contentConstraints);
		}

		void CopyDedicatedContent(FT ft, List<string> target) {
			for(int i = ft.From + 1; i < ft.To; ++i) 
				if(content[i].Trim() != "")
					target.Add(content[i]);
		}
	}
}
