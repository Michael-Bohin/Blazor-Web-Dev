using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;

	public record Zadani_Fractions_S02_D : Zadani
	{
		public readonly int A, B, D, E, F; // C is 'missing' beacuse it is A * B
		public readonly Op o1, o2;

		public Zadani_Fractions_S02_D(int A, int B, int D, int E, int F, Op o1, Op o2) {
			this.A = A; this.B = B; this.D = D; this.E = E; this.F = F; this.o1 = o1; this.o2 = o2;
		}
		public (int , int, int, int, int, Op, Op ) Unpack() => (A, B, D, E, F, o1, o2);
	}

	public class EGenerator_Fractions_S02_D : ExcerciseGenerator<Zadani_Fractions_S02_D>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		///
		public EGenerator_Fractions_S02_D() : base(4) {
			List<int> moznaA = GetRange(2, 9);
			List<int> moznaB = GetRange(2, 9);
			List<int> moznaD = GetRange(1, 9);
			List<int> moznaE = GetRange(1, 9);
			List<int> moznaF = GetRange(1, 9);

			foreach(int A in moznaA)
				foreach(int B in moznaB)
					foreach(int D in moznaD)
						foreach(int E in moznaE)
							foreach(int F in moznaF)
								foreach((Op x, Op y) in addSubCombinations)
									Consider(A, B, D, E, F, x, y);

			CreateStatsLog(moznaA.Count, moznaB.Count, moznaD.Count, moznaE.Count, moznaF.Count, addSubCombinations.Length);
		}

		void Consider(int A, int B, int D, int E, int F, Op o1, Op o2) {
			// E y F neni 0 && E y F neni C
			// A * B neni D
			// A neni D && B neni D
			// Vysledek nalezi do EasyZT
			int EyF = o2 == Op.Add ? E + F : E - F;
			int C = A * B;

			int decision = -1;
			if( !(  EyF != 0 && EyF != C ) ) 
				decision = 0;
			else if( !( C != D )  ) 
				decision = 1;
			else if( !( A != D && B != D )  ) 
				decision = 2;
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(C, D, E, F, o1, o2) )
				decision = 3;
			
			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_D( A, B, D, E, F, o1, o2) );
			} else {
				legit.Add( new Zadani_Fractions_S02_D( A, B, D, E, F, o1, o2 ) );
			}
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(int C, int D, int E, int F, Op o1, Op o2) {
			Q top = ((Q)1).Operate( new Q(D, C), o1);
			Q bottom = o2 == Op.Add ? new(E+F, C) : new(E-F, C);
			return IsEasyZt(top / bottom);
		}
		
		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		/// 
		protected override Excercise Construct(Zadani_Fractions_S02_D z) { 
			(int A, int B, int D, int E, int F, Op o1, Op o2) = z.Unpack();
			int C = A * B;
			string[] steps = new string[6];
			string[] comments = new string[6];
			
			// Step 1:
			string topLeft = Fraction.ToHTML($"{A} ∙ {B}", $"{C}");
			string topRight = Fraction.ToHTML($"{D}", $"{A} ∙ {B}");
			string bottom = Fraction.ToHTML($"{E} {Repr(o2)} {F}", $"{C}");
			steps[0] = Fraction.ToHTML($"{topLeft} {Repr(o1)} {topRight}", $"{bottom}");
			comments[0] = $"Spočítej všechny vnitřní operace zlomků.";

			// Step 2:
			topLeft = Fraction.ToHTML($"{C}", $"{C}");
			topRight = Fraction.ToHTML($"{D}", $"{C}");
			Q EyFC = o2 == Op.Add ? new(E+F, C) : new(E-F, C);

			steps[1] = Fraction.ToHTML($"{topLeft} {Repr(o1)} {topRight}", $"{EyFC}");
			comments[1] = $"Sečti/odečti všechny {XtinyCesky(C)}.";

			// Step 3:
			Q top = o1 == Op.Add ? new(C+D, C) : new(C-D, C);
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
