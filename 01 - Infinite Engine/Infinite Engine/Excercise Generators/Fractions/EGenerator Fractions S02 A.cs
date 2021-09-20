using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms; // required?

	public class EGenerator_Fractions_S02_A : ExcerciseGenerator
	{
		public record Zadani
		{
			public readonly Q A;
			public readonly Q B;
			public readonly int C;
			public readonly Q D;

			public Zadani(Q A, Q B, int C, Q D) {
				this.A = A; this.B = B; this.C = C; this.D = D;
			}
		}

		readonly List<Zadani> illegal1 = new();
		readonly List<Zadani> illegal2 = new();
		readonly List<Zadani> illegal3 = new();
		readonly List<Zadani> illegal4 = new();
		readonly List<Zadani> legit = new();
		readonly string stats;
		public string Stats => stats;
		readonly string[] xtiny = new string[] { "nula", "jedniny", "poloviny", "třetiny", "čtvrtiny", "pětiny", "šestiny", "sedminy", "osminy", "devítiny", "desetiny", "jedenáctiny", "dvanáctiny", "třináctiny", "čtrnáctiny", "patnáctiny", "šestnáctiny", "sedmnáctiny", "osmnáctiny", "devatenáctiny", "dvacetiny" };

		public EGenerator_Fractions_S02_A() {
			// at this point generate all possible zadani in constructor
			// also filter each to belong to appropriate category
			List<Q> moznaA = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaD = SetOfRationals.GetAll(10, 19, true);
			List<int> moznaC = new();
			for (int i = 2; i < 11; i++)
				moznaC.Add(i);

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (int C in moznaC)
						foreach (Q D in moznaD)
							Consider(A, B, C, D);

			stats = CreateStatsLog(moznaA, moznaB, moznaC, moznaD);
		}

		void Consider(Q A, Q B, int C, Q D) {
			// from pedagogic point of view: 
			// 1. LCM(A.q, B.q) == D.q
			// 2. A.q == B.q
			// 3. Vysledky aritmetiky kroku B se NErovnaji (tady asi spadne 99% kombinaci)
			// 4. Vysledek nenalezi do dostatecne jednoduchych vysledku 
			// 5. Kombinace je pedagogicky legitimni
			Zadani z = new(A.Copy(), B.Copy(), C, D.Copy());

			if (A.Den == B.Den)
				illegal1.Add(z);
			else if (M.EuclidsLCM(A.Den, B.Den) == D.Den)
				illegal2.Add(z);
			else if (VysledekAritmetikySe_NE_Rovna(A.Copy(), B.Copy(), C, D.Copy()))
				illegal3.Add(z);
			else if (VysledekNenaleziDoMoznychVysledku(A.Copy(), B.Copy(), C, D.Copy()))
				illegal4.Add(z);
			else
				legit.Add(z);
		}

		static bool VysledekAritmetikySe_NE_Rovna(Q A, Q B, int C, Q D) {
			int lcm = M.EuclidsLCM(A.Den, B.Den);
			int expandedA = A.Num * (lcm / A.Den);
			int expandedB = B.Num * (lcm / B.Den);
			int nahore = expandedA + expandedB;
			int dole = D.Den * C + D.Num;
			return nahore != dole;
		}

		static bool VysledekNenaleziDoMoznychVysledku(Q A, Q B, int C, Q D) {
			// spocitej vysledek, podivej jestli vysledek.Num je v [-10, 10] a vysledek.Den v [2, 10]
			Q vysledek = (A + B) / (new Q(C) + D);
			int cit = vysledek.Num;
			int jm = vysledek.Den;
			return cit < -10 || 10 < cit || jm < 2 || 10 < jm;
		}

		List<Excercise> ConstructExcercises(List<Zadani> seznamZadani) {
			List<Excercise> result = new();
			foreach (Zadani z in seznamZadani)
				result.Add(Construct(z.A, z.B, z.C, z.D));
			return result;
		}

		Excercise Construct(Q a, Q b, int C, Q d) {
			Q A = a.Copy(); Q B = b.Copy(); Q D = d.Copy(); // @ defensive programming
			string[] steps = new string[6];
			string[] comments = new string[6];
			int lcm = M.EuclidsLCM(A.Den, B.Den);

			// step 1:
			steps[0] = Fraction.ToHTML($"{A} + {B}", $"{C} + {D}");
			string aJmenovatel = A.Den < 21 ? xtiny[A.Den] : $"zlomek se jmenovatelem {A.Den}";
			string bJmenovatel = B.Den < 21 ? xtiny[B.Den] : $"zlomek se jmenovatelem {B.Den}";
			string cilovyJmenovatel = lcm < 21 ? xtiny[lcm] : $"zlomek se jmenovatelem {lcm}";
			comments[0] = $"V čitateli rozšiř {aJmenovatel} a {bJmenovatel} na {cilovyJmenovatel}.<br>Ve jmenovateli převeď číslo {C} na zlomek se jmenovatelem {D.Den}.";

			// step 2:
			A.Expand(lcm / A.Den);
			B.Expand(lcm / B.Den);
			Q cRational = new(C * D.Den, D.Den);
			steps[1] = Fraction.ToHTML($"{A} + {B}", $"{cRational} + {D}");
			comments[1] = $"Sečti zlomky";

			// step 3: 
			// !! aritmeticke operace automaticky prevadi na zakladni zlomek, zde nepouzivat, protoze 
			// ty operatory jsou chytrejsi nez deti na ZS a z pohledu deti udelaji vic kroku najednou 
			Q X = new(A.Num + B.Num, A.Den);
			Q Y = new(cRational.Num + D.Num, D.Den);
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

		public override Excercise GetOne() {
			int pick = rand.Next(legit.Count);
			Zadani z = legit[pick];
			return Construct(z.A.Copy(), z.B.Copy(), z.C, z.D.Copy());
		}

		public override Excercise[] GetTen() {
			Excercise[] result = new Excercise[10];
			int[] randPerm = GetRandomPermutation(legit.Count);
			for (int i = 0; i < 10; i++) {
				Zadani z = legit[randPerm[i]];
				result[i] = Construct(z.A, z.B, z.C, z.D);
			}
			return result;
		}

		public List<Excercise> GetIllegal1(int count) => GetPedagogicSet(illegal1, count);
		public List<Excercise> GetIllegal2(int count) => GetPedagogicSet(illegal2, count);
		public List<Excercise> GetIllegal3(int count) => GetPedagogicSet(illegal3, count);
		public List<Excercise> GetIllegal4(int count) => GetPedagogicSet(illegal4, count);
		public List<Excercise> GetLegit(int count) => GetPedagogicSet(legit, count);

		public List<Excercise> GetPedagogicSet(List<Zadani> zList, int count) {
			List<Zadani> subList = new();
			if (count < zList.Count)
				for (int i = 0; i < count; i++)
					subList.Add(zList[rand.Next(zList.Count)]);
			else
				for (int i = 0; i < zList.Count; i++)
					subList.Add(zList[i]);
			return ConstructExcercises(subList);
		}

		string CreateStatsLog(List<Q> moznaA, List<Q> moznaB, List<int> moznaC, List<Q> moznaD) {
			StringBuilder sb = new();
			int ill_1 = illegal1.Count; int ill_2 = illegal2.Count; int ill_3 = illegal3.Count; int ill_4 = illegal4.Count; int leg = legit.Count;
			int total = ill_1 + ill_2 + ill_3 + ill_4 + leg;
			sb.Append($"Total possible: {total} --> {(double)total / total * 100}%\n");
			sb.Append($"illegal1 count: {ill_1} --> {(double)ill_1 / total * 100}%\n");
			sb.Append($"illegal2 count: {ill_2} --> {(double)ill_2 / total * 100}%\n");
			sb.Append($"illegal3 count: {ill_3} --> {(double)ill_3 / total * 100}%\n");
			sb.Append($"illegal4 count: {ill_4} --> {(double)ill_4 / total * 100}%\n");
			sb.Append($"legit count: {leg} --> {(double)leg / total * 100}%\n");

			sb.Append($"\n\nmoznaA count: {moznaA.Count}\n");
			sb.Append($"moznaA count: {moznaB.Count}\n");
			sb.Append($"moznaC count: {moznaC.Count}\n");
			sb.Append($"moznaD count: {moznaD.Count}\n");

			sb.Append($"Pro kontrolu aritmeticky by melo existovat celkem {moznaA.Count}*{moznaB.Count}*{moznaC.Count}*{moznaD.Count}={moznaA.Count * moznaB.Count * moznaC.Count * moznaD.Count} moznosti.\n");

			sb.Append("\n\nNasleduje vypis promennych:\n");
			sb.Append("\nZlomky A, B (mnozina EasyZT) :\n");
			foreach (Q A in moznaA)
				sb.Append($"{A.Num} / {A.Den}\n");

			sb.Append("\nCela cisla C:\n");
			foreach (int C in moznaC)
				sb.Append($"{C}\n");

			sb.Append("\nZlomky D (mnozina MediumZT) :\n");
			foreach (Q D in moznaD)
				sb.Append($"{D.Num} / {D.Den}\n");
			return sb.ToString();
		}
	} // end egen fractions 
} // end namespace IE

/* -> prilis nebezpecne pri 4.8 milionu kombinaci prikladu
    *public List<Excercise> GetAllIllegal1 => ConstructExcercises(illegal1);
public List<Excercise> GetAllIllegal2 => ConstructExcercises(illegal2);
public List<Excercise> GetAllIllegal3 => ConstructExcercises(illegal3);
public List<Excercise> GetAllIllegal4 => ConstructExcercises(illegal4);
public List<Excercise> GetAllLegit => ConstructExcercises(legit);*/