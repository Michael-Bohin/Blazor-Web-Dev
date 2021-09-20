using System;
using System.Collections.Generic;
using static System.Console;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;
	public record Zadani
	{
		public readonly Q A, B, D;
		public readonly int C;
		public readonly Op opA, opB;

		public Zadani(Q A, Q B, int C, Q D, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_A : ExcerciseGenerator
	{
		public EGenerator_Fractions_S02_A() : base(4) {
			// at this point generate all possible zadani in constructor
			// also filter each to belong to appropriate category
			List<Q> moznaA = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaD = SetOfRationals.GetAll(10, 19, true);
			List<int> moznaC = new();
			for (int i = 2; i < 11; i++)
				moznaC.Add(i);
			(Op, Op)[] operatorCombinations = new (Op, Op)[] { (Op.Add, Op.Add), (Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) };

			aritmetickaKontrola = $"\nPro kontrolu aritmeticky by melo existovat celkem {moznaA.Count} * {moznaB.Count} * {moznaC.Count} * {moznaD.Count} * {operatorCombinations.Length} = {moznaA.Count * moznaB.Count * moznaC.Count * moznaD.Count * operatorCombinations.Length} moznosti.\n";

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (int C in moznaC)
						foreach (Q D in moznaD)
							foreach((Op opA, Op opB) in operatorCombinations)
								Consider(new Zadani(A.Copy(), B.Copy(), C, D.Copy(), opA, opB));

			CreateStatsLog();
		}

		protected override void Consider(Zadani z) {
			// from pedagogic point of view: 
			// 1. A.q != B.q
			// 2. LCM(A.q, B.q) != D.q
			// 3. Vysledky aritmetiky kroku B se rovnaji (tady spadne +- 90% kombinaci)
			// 4. Vysledek nalezi do dostatecne jednoduchych vysledku 
			// 5. Kombinace je pedagogicky legitimni
			Q A = z.A;
			Q B = z.B;
			int C = z.C;
			Q D = z.D;
			Op opA = z.opA;
			Op opB = z.opB;
			
			if (! (A.Den != B.Den))
				illegal[0].Add(z);
			else if ( ! (M.EuclidsLCM(A.Den, B.Den) != D.Den))
				illegal[1].Add(z);
			else if ( ! VysledekAritmetikySeRovna( A, B, C, D , opA, opB ))
				illegal[2].Add(z);
			else if ( ! VysledekNaleziDoMoznychVysledku( A, B, C, D, opA, opB))
				illegal[3].Add(z);
			else
				legit.Add(z);
		}

		static bool VysledekAritmetikySeRovna(Q A, Q B, int C, Q D, Op opA, Op opB) {
			int lcm = M.EuclidsLCM(A.Den, B.Den);
			int expandedA = A.Num * (lcm / A.Den);
			int expandedB = B.Num * (lcm / B.Den);
			int nahore = opA == Op.Add ? expandedA + expandedB : expandedA - expandedB;
			int dole = opB == Op.Add ? D.Den * C + D.Num : D.Den * C - D.Num;
			return nahore == dole;
		}

		static bool VysledekNaleziDoMoznychVysledku(Q A, Q B, int C, Q D, Op opA, Op opB) {
			// spocitej vysledek, podivej jestli vysledek.Num je v [-10, 10] a vysledek.Den v [2, 10]
			Q top = opA == Op.Add ? A + B : A - B;
			Q bottom = opB == Op.Add ? (new Q(C) + D) : (new Q(C) - D);
			Q vysledek = top / bottom;
			int cit = vysledek.Num;
			int jm = vysledek.Den;
			return -11 < cit && cit < 11 && 1 < jm && jm < 11;
		}

		protected override Excercise Construct(Zadani z) {
			int C = z.C; Op opA = z.opA; Op opB = z.opB;
			Q A = z.A.Copy(); Q B = z.B.Copy(); Q D = z.D.Copy(); // @ defensive programming
			string opReprA = opA == Op.Add ? "+" : "-";
			string opReprB = opB == Op.Add ? "+" : "-";
			string[] steps = new string[6];
			string[] comments = new string[6];
			int lcm = M.EuclidsLCM(A.Den, B.Den);

			// step 1:
			steps[0] = Fraction.ToHTML($"{A} {opReprA} {B}", $"{C} {opReprB} {D}");
			string aJmenovatel = A.Den < 21 ? xtiny[A.Den] : $"zlomek se jmenovatelem {A.Den}";
			string bJmenovatel = B.Den < 21 ? xtiny[B.Den] : $"zlomek se jmenovatelem {B.Den}";
			string cilovyJmenovatel = lcm < 21 ? xtiny[lcm] : $"zlomek se jmenovatelem {lcm}";
			comments[0] = $"V čitateli rozšiř {aJmenovatel} a {bJmenovatel} na {cilovyJmenovatel}.<br>Ve jmenovateli převeď číslo {C} na zlomek se jmenovatelem {D.Den}.";

			// step 2:
			A.Expand(lcm / A.Den);
			B.Expand(lcm / B.Den);
			Q cRational = new(C * D.Den, D.Den);
			steps[1] = Fraction.ToHTML($"{A} {opReprA} {B}", $"{cRational} {opReprB} {D}");
			comments[1] = $"Sečti/odečti zlomky se stejným jmenovatelem.";

			// step 3: 
			// !! aritmeticke operace automaticky prevadi na zakladni zlomek, zde nepouzivat, protoze 
			// ty operatory jsou chytrejsi nez deti na ZS a z pohledu deti udelaji vic kroku najednou 
			Q X = opA == Op.Add ? new(A.Num + B.Num, A.Den) : new(A.Num - B.Num, A.Den);
			Q Y = opB == Op.Add ? new(cRational.Num + D.Num, D.Den) : new(cRational.Num - D.Num, D.Den);
			steps[2] = Fraction.ToHTML(X.ToString(), Y.ToString());
			comments[2] = "Převeď dělení zlomků na jejich násobení.";

			// step 4: 
			Y.Inverse();
			steps[3] = $"{X} ∙ {Y}";
			comments[3] = $"Vykrať {X.Num} v čitateli i jmenovateli.";

			// step 5:
			Q result = new(Y.Num, X.Den);
			steps[4] = $"{result}";
			comments[4] = $"Převeď výsledný zlomek na jeho základní tvar.";

			// step 6: 
			result.Reduce();
			steps[5] = $"{result}";
			comments[5] = "Hotovo! 😎😎";

			return new EFractions_S02_A(steps, comments, result);
			// ! dodelat: a) detekovani ruznych koncu prevod na ZT nebo neprevod, 4 mozne kombinace +- v zadani
		}
	} // end egen fractions 
} // end namespace IE

/* -> prilis nebezpecne pri 4.8 milionu kombinaci prikladu
    *public List<Excercise> GetAllIllegal1 => ConstructExcercises(illegal1);
public List<Excercise> GetAllIllegal2 => ConstructExcercises(illegal2);
public List<Excercise> GetAllIllegal3 => ConstructExcercises(illegal3);
public List<Excercise> GetAllIllegal4 => ConstructExcercises(illegal4);
public List<Excercise> GetAllLegit => ConstructExcercises(legit);*/