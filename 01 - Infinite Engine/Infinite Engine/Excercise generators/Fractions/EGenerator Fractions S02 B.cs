using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_B
	{
		public readonly Q A, B, C, D;
		public readonly Op o1, o2;

		public Zadani_Fractions_S02_B(Q A, Q B, Q C, Q D, Op o1, Op o2) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.o1 = o1; this.o2 = o2;
		}

		public (Q, Q, Q, Q, Op, Op) Unpack() => (A, B, C, D, o1, o2);
	}

	public class EGenerator_Fractions_S02_B : ExcerciseGenerator <Zadani_Fractions_S02_B>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		///
		public EGenerator_Fractions_S02_B():base(4) {
			List<Q> moznaA = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaC = SetOfRationals.GetEasyMediumZTSet();
			List<Q> moznaD = SetOfRationals.GetAll(1, 9, true);

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach((Op o1, Op o2) in addSubCombinations)
								Consider( A, B, C, D, o1, o2);

			CreateStatsLog(moznaA.Count, moznaB.Count, moznaC.Count, moznaD.Count, addSubCombinations.Length);
		}

		protected void Consider(Q A, Q B, Q C, Q D, Op o1, Op o2) {
			// 1. A.Den != B.Den && C.Den != D.Den : "Jmenovatele A a B, C a D jsou různé"
			// 2. E.Num < F.Den (slabsi, ale rychlejsi podminka na K nasobek)
			// 3. F.Den je k nasobek E.Num
			// 4. VysledekNaleziDoEasyZT
			Q E = A.AtomicOperate(B, o1);
			Q F = C.AtomicOperate(D, o2);

			int decision = -1; // for legit zadani
			if( ! (A.Den != B.Den && C.Den != D.Den) ) {
				decision = 0;
			} else if( ! (E.Num < F.Den ) ) {
				decision = 1;
			} else if( ! ( F.Den % E.Num == 0 ) ) {
				decision = 2;
			} else if( ! VysledekNaleziDoMnozinyEasyZlomky(A, B, C, D, o1, o2) ) {
				decision = 3;
			}

			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_B( A.Copy(), B.Copy(), C.Copy(), D.Copy(), o1, o2) );
			} else {
				legit.Add( new Zadani_Fractions_S02_B( A.Copy(), B.Copy(), C.Copy(), D.Copy(), o1, o2) );
			}
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q A, Q B, Q C, Q D, Op o1, Op o2) {
			return IsEasyZt(A.Operate(B, o1) * C.Operate(D, o2) );
		}

		///
		/// Kuchařka řešení: Jak se zadání řeší?
		/// 
		protected override Excercise Construct(Zadani_Fractions_S02_B z) { 
			(Q A, Q B, Q C, Q D, Op o1, Op o2) = z.Unpack();
			string[] steps = new string[7];
			string[] comments = new string[7];
			int lcmTop = M.EuclidsLCM(A.Den, B.Den);	
			int lcmBottom = M.EuclidsLCM(C.Den, D.Den);

			// step 1:
			steps[0] = $"( {A} {Repr(o1)} {B} )∙( {C} {Repr(o2)} {D} )";
			comments[0] = $"V levé závorce rozšiř {XtinyCesky(A.Den)} a {XtinyCesky(B.Den)} na {XtinyCesky(lcmTop)}.<br>";
			comments[0] += $"V pravé závorce rozšiř {XtinyCesky(C.Den)} a {XtinyCesky(D.Den)} na {XtinyCesky(lcmBottom)}.";

			// step 2:
			(Q expA, Q expB) = A.ExpandToLCD(B);
			(Q expC, Q expD) = C.ExpandToLCD(D);
			steps[1] = $"( {expA} {Repr(o1)} {expB} )∙( {expC} {Repr(o2)} {expD} )";
			comments[1] = $"Sečti/odečti zlomky v závorkách.";

			// step 3:
			Q E = A.AtomicOperate(B, o1);
			Q F = C.AtomicOperate(D, o2);
			steps[2] = $"{E} ∙ {F}";
			comments[2] = $"Všimni si, že {F.Den} je násobek {E.Num}. Rozlož {F.Den} na násobek {E.Num}.";

			// step 4:
			if(E.Num == 0)
				return new EFractions_S02(steps, comments, E); 

			int fDen = F.Den / E.Num;
			if(fDen == 0)
				return new EFractions_S02(steps, comments, E); // in place in order for illegal zadani's to not fall on 0 division, must never occur in legit excercises
			steps[3] = $"{E} ∙ {Fraction.ToHTML($"{F.Num}", $"{E.Num} ∙ {F.Den / E.Num}")}";
			comments[3] = $"{E.Num} vykrať.";

			// step 5:
			
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