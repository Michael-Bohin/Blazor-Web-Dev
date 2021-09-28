using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_E
	{
		public readonly Q A, B, C;
		public readonly Op o1;

		public Zadani_Fractions_S02_E(Q A, Q B, Q C, Op o1) {
			this.A = A; this.B = B; this.C = C; this.o1 = o1;
		}

		public (Q, Q, Q, Op) Unpack() => (A, B, C, o1);
	}

	public class EGenerator_Fractions_S02_E : ExcerciseGenerator <Zadani_Fractions_S02_E>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		/// 
		public EGenerator_Fractions_S02_E() : base(5) {
			List<Q> moznaA = SetOfRationals.GetEasilyFractionable(); // to be implemented;
			List<Q> moznaB = SetOfRationals.GetAll(1, 30, true);
			List<Q> moznaC = SetOfRationals.GetAll(1, 9, true);
			List<Op> addSub = new() { Op.Add, Op.Sub };
			
			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach(Op o1 in addSub)
							Consider(A, B, C, o1);

			CreateStatsLog(moznaA.Count , moznaB.Count , moznaC.Count , addSub.Count);
		}

		void Consider(Q A, Q B, Q C, Op o1) {
			// Jmenovatel B je k násobek jmenovatele A pro k > 1
			// Čitatel B je k násobek jmenovatele C pro k > 1
			// Výsledek náleží do EasyZT

			int decision = -1;
			if( !(  B.Den != A.Den ) )
				decision = 0;
			else if( !( B.Den % A.Den == 0 ) )
				decision = 1;
			else if( !( B.Num != C.Den ) )
				decision = 2;
			else if( !( B.Num % C.Den == 0 ) )
				decision = 3;
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(A, B, C, o1) )
				decision = 4;
			
			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_E( A.Copy(), B.Copy(), C.Copy(), o1) );
			} else {
				legit.Add( new Zadani_Fractions_S02_E( A.Copy(), B.Copy(), C.Copy(), o1) );
			}
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q A, Q B, Q C, Op o1) {
			Q result = (A / B).Operate(C, o1);
		    return IsEasyZt(result);
		}

		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		///
		protected override Excercise Construct(Zadani_Fractions_S02_E z) { 
			(Q A, Q B, Q C, Op o1) = z.Unpack(); // Q resultExpected = (A / B).Operate(C, o1);
			string[] steps = new string[7];
			string[] comments = new string[7];

			// Step 1:
			steps[0] = $"{A.ToDouble()} : {B} {Repr(o1)} {C}";
			comments[0] = $"Číslo {A.ToDouble()} převeď na zlomek.";

			// Step 2:
			steps[1] = $"{A} : {B} {Repr(o1)} {C}";
			comments[1] = $"Dělení dvou zlomků převeď na jejich násobení. (U pravého operandu prohoď čitatel se jmenovatelem.)";

			// Step 3:
			Q invB = B.GetInverse();
			steps[2] = $"{A} ∙ {invB} {Repr(o1)} {C}";
			comments[2] = $"V součinu zlomků vykrať násobek {A.Den}. A poté spočítej součin zbylých zlomků.";

			// Step 4:
			Q vykracenySoucin = new( A.Num * (invB.Num / A.Den), invB.Den);
			steps[3] = $"{vykracenySoucin} {Repr(o1)} {C}";
			comments[3] = $"Zlomek {C} rozšiř tak, aby měl stejný jmenovatel jako zlomek {vykracenySoucin}.";

			// Step 5:
			(vykracenySoucin, C) = vykracenySoucin.ExpandToLCD( C );
			steps[4] = $"{vykracenySoucin} {Repr(o1)} {C}";
			comments[4] = $"Sečti/odečti zlomky se stejným jmenovatelem.";

			// Step 6:
			Q result = vykracenySoucin.AtomicOperate(C, o1);
			steps[5] = $"{result}";
			comments[5] = $"Ověř, že výsledek je zlomek v základním tvaru. Pokud ne, převeď ho na něj.";

			// Step 7:
			result.Reduce();
			steps[6] = $"{result}";
			comments[6] = "Hotovo! 😎😎";
			return new EFractions_S02(steps, comments, result);
		}
	}
}
