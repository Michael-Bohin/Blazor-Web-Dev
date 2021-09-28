using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_C
	{
		public readonly int A;
		public readonly Q B, C, D, E;
		public readonly Op o1, o2;

		public Zadani_Fractions_S02_C(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.o1 = o1; this.o2 = o2;
		}

		public (int, Q, Q, Q, Q, Op, Op) Unpack() => (A, B, C, D, E, o1, o2);
	}

	public class EGenerator_Fractions_S02_C : ExcerciseGenerator <Zadani_Fractions_S02_C>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		/// 
		public EGenerator_Fractions_S02_C() : base(4) {
			List<int> moznaA = GetRange(2, 10);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaC = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaD = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaE = SetOfRationals.GetAll(1, 9, true);
			
			foreach (int A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach (Q E in moznaE)
								foreach((Op o1, Op o2) in addSubCombinations)
									Consider(A, B, C, D, E, o1, o2);

			CreateStatsLog(moznaA.Count , moznaB.Count , moznaC.Count , moznaD.Count , moznaE.Count , addSubCombinations.Length);
		}

		void Consider(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {
			// masivni constraint: (A-B) * C == 1
			// D y E je ruzne od nuly
			// D.Den != E.Den
			// Vysledek nalezi do EasyZT

			int decision = -1;
			if( !(  ((Q)A-B) * C == (Q)1 ) )
				decision = 0;
			else if( !( (D.Operate(E, o2)).Num != 0 ) )
				decision = 1;
			else if( !( D.Den != E.Den ) )
				decision = 2;
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(D, E, o2) )
				decision = 3;
			
			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_C( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );
			} else {
				legit.Add( new Zadani_Fractions_S02_C( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );
			}
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q D, Q E, Op o) {
			Q result = D.Operate(E, o).GetInverse();
			return IsEasyZt(result.GetSimplestForm());
		}

		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		///
		protected override Excercise Construct(Zadani_Fractions_S02_C z) { 
			(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) = z.Unpack();
			string[] steps = new string[7];
			string[] comments = new string[7];
			int rightLCD = M.EuclidsLCM(D.Den, E.Den);

			// Step 1:
			steps[0] = $"( {A} {Repr(o1)} {B} ) ∙ {C} : ( {D} {Repr(o2)} {E} )";
			comments[0] = $"V levé závorce rozšiř číslo {A} na {XtinyCesky(B.Den)}.<br>V pravé závorce rozšiř {XtinyCesky(D.Den)} a {XtinyCesky(E.Den)} na {XtinyCesky(rightLCD)}.";

			// Step 2:
			Q F = new (A * B.Den, B.Den);
			(D, E) = D.ExpandToLCD( E );
			steps[1] = $"( {F} {Repr(o1)} {B} ) ∙ {C} : ( {D} {Repr(o2)} {E} )";
			comments[1] = "Sečti/odečti zlomky v závorkách.";

			// Step 3:
			F = F.AtomicOperate(B, o1);
			D = D.AtomicOperate(E, o2);
			steps[2] = $"{F} ∙ {C} : {D}";
			comments[2] = $"Vykrať {F.Num} a {F.Den}.";

			// Step 4:
			F.Num = 1; F.Den = 1; C.Num = 1; C.Den = 1;
			steps[3] = $"{F} ∙ {C} : {D}";
			comments[3] = $"Dělení dvou zlomků převeď na násobení.";

			// Step 5:
			// make illegal cases not cause runtime math hell excpetion
			if(D.Num == 0) {
				comments[4] = $">> RUNTIME MATH HELL EXCEPTION AVOIDED<<";
				return new EFractions_S02(steps, comments, D);	
			}
			D.Inverse();
			steps[4] = $"{F} ∙ {D}";
			comments[4] = $"Vynásob jedničku.";

			// Step 6:
			steps[5] = $"{D}";
			comments[5] = $"Výsledný zlomek převeď na jeho základní tvar.";

			// Step 7:
			D.Reduce();
			steps[6] = $"{D}";
			comments[6] = "Hotovo! 😎😎";
			return new EFractions_S02(steps, comments, D);	
		}
	}
}
