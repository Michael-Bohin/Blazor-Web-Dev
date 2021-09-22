using System;
using System.Collections.Generic;
using static System.Console;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_B : Zadani
	{
		public readonly Q A, B, C, D;
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_B(Q A, Q B, Q C, Q D, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_B : ExcerciseGenerator <Zadani_Fractions_S02_B>
	{
		public EGenerator_Fractions_S02_B():base(4) {
			List<Q> moznaA, moznaB, moznaC, moznaD;
			moznaA = SetOfRationals.GetAll(1, 9, true);
			moznaB = SetOfRationals.GetAll(1, 9, true);
			moznaC = SetOfRationals.GetEasyMediumZTSet();
			moznaD = SetOfRationals.GetAll(1, 9, true);
			(Op, Op)[] operatorCombinations = new (Op, Op)[] { (Op.Add, Op.Add) ,(Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) /**/};

			aritmetickaKontrola = $"\nPro kontrolu aritmeticky by melo existovat celkem {moznaA.Count} * {moznaB.Count} * {moznaC.Count} * {moznaD.Count} * {operatorCombinations.Length} = {moznaA.Count * moznaB.Count * moznaC.Count * moznaD.Count * operatorCombinations.Length} moznosti.\n";

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach((Op opA, Op opB) in operatorCombinations)
								Consider(new Zadani_Fractions_S02_B(A.Copy(), B.Copy(), C.Copy(), D.Copy(), opA, opB));

			CreateStatsLog();
		}

		protected void Consider(Zadani_Fractions_S02_B z) {
			// from pedagogic point of view: 
			// 1. "Jmenovatele dvojic A a B, C a D jsou různé"
			//			A.Den != B.Den && C.Den != D.Den
			// 2. "Aritmetika A op B vede zlomek, ktery nereprezentuje cele cislo"
			//			( A +- B ).Den != 1    // -> A + B should not represent integer 
			// 3. "Jako bod 2, s C op D"
			//			( C +- D ).Den != 1
			// 4. "LCM(C.Den, D.Den) je k nasobek LCM(A.Den, B.Den)"
			//			LCM(C.Den, D.Den) % LCM(A.Den, B.Den) == 0 && LCM(C.Den, D.Den) > LCM(A.Den, B.Den)
			// 5. Zadani je legit. :) 

			Q A = z.A;
			Q B = z.B;
			Q C = z.C;
			Q D = z.D;
			Op opA = z.opA;
			Op opB = z.opB;

			int lcmTop = M.EuclidsLCM(A.Den, B.Den);
			int lcmBottom = M.EuclidsLCM(C.Den, D.Den);
			int expA = A.Num * (lcmTop / A.Den);
			int expB = B.Num * (lcmTop / B.Den);
			int expC = C.Num * (lcmBottom / C.Den);
			int expD = D.Num * (lcmBottom / D.Den);
			Q E = opA == Op.Add ? new(  expA + expB , lcmTop) : new( expA - expB , lcmTop);
			Q F = opB == Op.Add ? new(  expC + expD , lcmBottom) : new( expC - expD , lcmBottom);
			

			// !! illegal 0 a 1 vedou na varianty ktere pri sestrojeni na kucharku spadnou na deleni nulou
			if( ! (A.Den != B.Den && C.Den != D.Den) ) {
				ProcessZadani(z, 0);
			} else if( ! (E.Num < F.Den ) ) {
				ProcessZadani(z, 1);
			} else if( ! ( F.Den % E.Num == 0 ) ) {
				ProcessZadani(z, 2);
			} else if( ! VysledekNaleziDoMnozinyEasyZlomky(A, B, C, D, opA, opB) ) {
				ProcessZadani(z, 3);
			} else {
				legit.Add(z);
			}
		}

		

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q A, Q B, Q C, Q D, Op opA, Op opB) {
			// spocitej vysledek, podivej jestli vysledek.Num je v [-10, 10] a vysledek.Den v [2, 10]
			Q left = opA == Op.Add ? A + B : A - B;
			Q right = opB == Op.Add ? C + D : C - D;
			return IsEasyZt(left * right);
		}

		protected override Excercise Construct(Zadani_Fractions_S02_B z) { 
			Q A = z.A; Q B = z.B; Q C = z.C; Q D = z.D; // @ defensive programming
			Op opA = z.opA; Op opB = z.opB;
			string opReprA = opA == Op.Add ? "+" : "-";
			string opReprB = opB == Op.Add ? "+" : "-";
			string[] steps = new string[7];
			string[] comments = new string[7];
			int lcmTop = M.EuclidsLCM(A.Den, B.Den);	
			int lcmBottom = M.EuclidsLCM(C.Den, D.Den);

			// step 1:
			steps[0] = $"( {A} {opReprA} {B} )∙( {C} {opReprB} {D} )";

			string aJm = XtinyCesky(A.Den);
			string bJm = XtinyCesky(B.Den);
			string cJm = XtinyCesky(C.Den);
			string dJm = XtinyCesky(D.Den);
			comments[0] = $"V levé závorce rozšiř {aJm} a {bJm} na {XtinyCesky(lcmTop)}.<br>V pravé závorce rozšiř {cJm} a {dJm} na {XtinyCesky(lcmBottom)}.";

			// step 2:
			Q expA = A.GetExpandedForm(lcmTop / A.Den);
			Q expB = B.GetExpandedForm(lcmTop / B.Den);
			Q expC = C.GetExpandedForm(lcmBottom / C.Den);
			Q expD = D.GetExpandedForm(lcmBottom / D.Den);
			steps[1] = $"( {expA} {opReprA} {expB} )∙( {expC} {opReprB} {expD} )";
			comments[1] = $"Sečti/odečti zlomky v závorkách.";

			// step 3:
			// do not use inbuild RationalNumber arithemtic! performs also transformation into Simplest form 
			int topNum = opA == Op.Add ? expA.Num + expB.Num : expA.Num - expB.Num;
			int bottomNum = opB == Op.Add ? expC.Num + expD.Num : expC.Num - expD.Num;
			Q E = new(topNum, lcmTop);
			Q F = new(bottomNum, lcmBottom);

			steps[2] = $"{E} ∙ {F}";
			comments[2] = $"Všimni si, že {F.Den} je násobek {E.Num}. Rozlož {F.Den} na násobek {E.Num}.";

			// step 4:
			steps[3] = $"{E} ∙ {Fraction.ToHTML($"{F.Num}", $"{E.Num} ∙ {F.Den / E.Num}")}";
			comments[3] = $"{E.Num} vykrať.";
			// step 5:
			int fDen = F.Den / E.Num;
			if(fDen == 0)
				return new EFractions_S02(steps, comments, E); // in place in order for illegal zadani's to not fall on 0 division, must never occur in legit excercises
			E.Num = 1;
			steps[4] = $"{E} ∙ {Fraction.ToHTML($"{F.Num}", $"{fDen}")}";
			comments[4] = $"Vynásob oba zlomky.";

			// step 6:
			E.Num *= F.Num;
			E.Den *= fDen;
			steps[5] = $"{E}";
			comments[5] = $"Výsledeke převeď na základní tvar zlomku.";

			// step 7:
			E.Reduce();
			steps[6] = $"{E}";
			comments[6] = $"Hotovo! 😎😎";

			return new EFractions_S02(steps, comments, E);
		}
	}
}