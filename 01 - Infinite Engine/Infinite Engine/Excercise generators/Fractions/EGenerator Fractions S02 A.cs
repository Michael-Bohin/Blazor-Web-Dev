using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_A : Zadani
	{
		public readonly Q A, B, D;
		public readonly int C;
		public readonly Op o1, o2;

		public Zadani_Fractions_S02_A(Q A, Q B, int C, Q D, Op o1, Op o2) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.o1 = o1; this.o2 = o2;
		}

		public (Q , Q, int, Q, Op, Op ) Unpack() => (A, B, C, D, o1, o2);
	}

	public class EGenerator_Fractions_S02_A : ExcerciseGenerator <Zadani_Fractions_S02_A>
	{		
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		///
		public EGenerator_Fractions_S02_A() : base(4) {
			List<Q> moznaA = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<int> moznaC = GetRange(2, 10);
			List<Q> moznaD = SetOfRationals.GetAll(10, 19, true);

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (int C in moznaC)
						foreach (Q D in moznaD)
							foreach((Op o1, Op o2) in addSubCombinations)
								Consider( A, B, C, D, o1, o2 );

			CreateStatsLog(moznaA.Count, moznaB.Count, moznaC.Count, moznaD.Count, addSubCombinations.Length);
		}

		void Consider( Q A, Q B, int C, Q D, Op o1, Op o2 ) {
			// 1. A.q != B.q
			// 2. LCM(A.q, B.q) != D.q
			// 3. Vysledky aritmetiky z kroku B se rovnaji (tady spadne +- 90% kombinaci)
			// 4. Vysledek nalezi do dostatecne jednoduchych vysledku 
			
			int decision = -1; // for legit zadani's
			if (! (A.Den != B.Den))
				decision = 0;
			else if ( ! (M.EuclidsLCM(A.Den, B.Den) != D.Den))
				decision = 1;
			else if ( ! CiniteleMajiStejnouHodnotu( A, B, C, D , o1, o2 ))
				decision = 2;
			else if ( ! VysledekNaleziDoMnozinyEasyZlomky( A, B, C, D, o1, o2))
				decision = 3;
			
			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_A( A.Copy(), B.Copy(), C, D.Copy(), o1, o2) );
			} else {
				legit.Add( new Zadani_Fractions_S02_A( A.Copy(), B.Copy(), C, D.Copy(), o1, o2) );
			}
		}

		static bool CiniteleMajiStejnouHodnotu(Q A, Q B, int C, Q D, Op o1, Op o2) {
			Q nahore = A.AtomicOperate(B, o1);
			Q dole = ((Q)C).AtomicOperate(D, o2);
			return nahore.Num == dole.Num;
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q A, Q B, int C, Q D, Op o1, Op o2) {
			Q top = o1 == Op.Add ? A + B : A - B;
			Q bottom = o2 == Op.Add ?  (Q)C + D : (Q)C - D;
			return IsEasyZt(top / bottom);
		}

		///
		/// Kuchařka řešení: Jak se zadání řeší?
		/// 
		protected override Excercise Construct(Zadani_Fractions_S02_A z) { 
			(Q A, Q B, int C, Q D, Op o1, Op o2) = z.Unpack();
			string[] steps = new string[6];
			string[] comments = new string[6];
			int lcm = M.EuclidsLCM(A.Den, B.Den);

			// step 1:
			steps[0] = Fraction.ToHTML($"{A} {Repr(o1)} {B}", $"{C} {Repr(o2)} {D}");
			comments[0] = $"V čitateli rozšiř {XtinyCesky(A.Den)} a {XtinyCesky(B.Den)} na {XtinyCesky(lcm)}.<br>Ve jmenovateli převeď číslo {C} na zlomek se jmenovatelem {D.Den}.";

			// step 2:
			(Q expA, Q expB) = A.ExpandToLCD(B);
			Q cRational = new(C * D.Den, D.Den);
			steps[1] = Fraction.ToHTML($"{expA} {Repr(o1)} {expB}", $"{cRational} {Repr(o2)} {D}");
			comments[1] = $"Sečti/odečti zlomky se stejným jmenovatelem.";

			// step 3: 
			Q X = expA.AtomicOperate(expB, o1);
			Q Y = cRational.AtomicOperate(D, o2);
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

			return new EFractions_S02(steps, comments, result);
		}
	} // end egen fractions 
} // end namespace IE