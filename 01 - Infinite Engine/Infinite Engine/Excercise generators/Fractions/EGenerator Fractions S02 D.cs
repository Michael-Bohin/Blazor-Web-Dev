using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;

	public record Zadani_Fractions_S02_D : Zadani
	{
		public readonly int A, B, D, E, F; // C is 'missing' beacuse it is A * B
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_D(int A, int B, int D, int E, int F, Op opA, Op opB) {
			this.A = A; this.B = B; this.D = D; this.E = E; this.F = F; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_D : ExcerciseGenerator<Zadani_Fractions_S02_D>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		///
		public EGenerator_Fractions_S02_D() : base(4) {
			List<int> moznaA = new();
			AddRange(moznaA, 2, 9);

			List<int> moznaB = new();
			AddRange(moznaB, 2, 9);

			List<int> moznaD = new();
			AddRange(moznaD, 1, 9);

			List<int> moznaE = new();
			AddRange(moznaE, 1, 9);

			List<int> moznaF = new();
			AddRange(moznaF, 1, 9);

			(Op, Op)[] operatorCombinations = new (Op, Op)[] { (Op.Add, Op.Add), (Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) };

			aritmetickaKontrola = $"\nPro kontrolu aritmeticky by melo existovat celkem {moznaA.Count} * {moznaB.Count} * {moznaD.Count} * {moznaE.Count} * {moznaF.Count} *{operatorCombinations.Length} = {moznaA.Count * moznaB.Count * moznaD.Count * moznaE.Count * moznaF.Count *operatorCombinations.Length} moznosti.\n";

			foreach(int A in moznaA)
				foreach(int B in moznaB)
					foreach(int D in moznaD)
						foreach(int E in moznaE)
							foreach(int F in moznaF)
								foreach((Op x, Op y) in operatorCombinations)
									Consider(A, B, D, E, F, x, y);
			CreateStatsLog();
		}

		void AddRange(List<int> target, int from, int to) {
			// inclusive from, to
			for(int i = from; i <= to; i++)
				target.Add(i);
		}

		void Consider(int A, int B, int D, int E, int F, Op x, Op y) {
			// E y F neni 0 && E y F neni C
			// A * B neni D
			// A neni D && B neni D
			// Vysledek nalezi do EasyZT
			int EyF = y == Op.Add ? E + F : E - F;
			int C = A * B;

			if( !(  EyF != 0 && EyF != C ) ) 
				ProcessZadani(A, B, D, E, F, x, y , 0);
			else if( !( C != D )  ) 
				ProcessZadani(A, B, D, E, F, x, y , 1);
			else if( !( A != D && B != D )  ) 
				ProcessZadani(A, B, D, E, F, x, y , 2);
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(A, B, C, D, E, F, x, y) )
				ProcessZadani(A, B, D, E, F, x, y , 3);
			else
				legit.Add( GetZadani(A, B, D, E, F, x, y) );
		}

		void ProcessZadani(int A, int B, int D, int E, int F, Op x, Op y, int i) {
			illegalCounter[i]++;
			if(illegalCounter[i] < 1000)
				illegal[i].Add( GetZadani(A, B, D, E, F, x, y) );
		}

		static Zadani_Fractions_S02_D GetZadani(int A, int B, int D, int E, int F, Op x, Op y) => new (A, B, D, E, F, x, y);

		static bool VysledekNaleziDoMnozinyEasyZlomky(int A, int B, int C, int D, int E, int F, Op x, Op y) {
			Q right = new(D, C);
			Q top = x == Op.Add ? (Q)1 + right : (Q)1 - right;
			Q bottom = y == Op.Add ? new(E+F, C) : new(E-F, C);
			return IsEasyZt(top / bottom);
		}
		
		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		/// 
		protected override Excercise Construct(Zadani_Fractions_S02_D z) { 
			int A = z.A;
			int B = z.B;
			int C = A * B;
			int D = z.D;
			int E = z.E;
			int F = z.F;
			Op opX = z.opA;
			Op opY = z.opB;
			string x = Repr(opX);
			string y = Repr(opY);

			string[] steps = new string[6];
			string[] comments = new string[6];
			
			// Step 1:
			string topLeft = Fraction.ToHTML($"{A} ∙ {B}", $"{C}");
			string topRight = Fraction.ToHTML($"{D}", $"{A} ∙ {B}");
			string bottom = Fraction.ToHTML($"{E} {y} {F}", $"{C}");
			steps[0] = Fraction.ToHTML($"{topLeft} {x} {topRight}", $"{bottom}");
			comments[0] = $"Spočítej všechny vnitřní operace zlomků.";

			// Step 2:
			topLeft = Fraction.ToHTML($"{C}", $"{C}");
			topRight = Fraction.ToHTML($"{D}", $"{C}");
			Q EyFC = opY == Op.Add ? new(E+F, C) : new(E-F, C);

			steps[1] = Fraction.ToHTML($"{topLeft} {x} {topRight}", $"{EyFC}");
			comments[1] = $"Sečti/odečti všechny {XtinyCesky(C)}.";

			// Step 3:
			Q top = opX == Op.Add ? new(C+D, C) : new(C-D, C);
			steps[2] = Fraction.ToHTML($"{top}", $"{EyFC}");
			comments[2] = $"Převeď dělení zlomků na jejich násobení.";

			// Step 4:
			if(EyFC.Num == 0) {
				comments[3] = $">> RUNTIME MATH HELL EXCEPTION AVOIDED<<";
				return new EFractions_S02(steps, comments, EyFC);	
			}
			EyFC.Inverse();
			steps[3] = $"{top} ∙ {EyFC}";
			comments[3] = $"Vykrať číslo {C}.";

			// Step 5:
			Q result = new(top.Num, EyFC.Den);
			steps[4] = $"{result}";
			comments[4] = $"Výsledný zlomek převeď na jeho základní tvar.";

			// Step 6:
			result.Reduce();
			steps[5] = $"{result}";
			comments[5] = "Hotovo! 😎😎";
			return new EFractions_S02(steps, comments, result);		
		}
	}
}
