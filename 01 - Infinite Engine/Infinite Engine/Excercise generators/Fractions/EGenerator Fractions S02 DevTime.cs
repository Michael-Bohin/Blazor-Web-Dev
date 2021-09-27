using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_DevTime
	{
		public readonly int A;
		public readonly Q B, C, D, E;
		public readonly Op o1, o2;

		public Zadani_Fractions_S02_DevTime(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.o1 = o1; this.o2 = o2;
		}

		public (int, Q, Q, Q, Q, Op, Op) Unpack() => (A, B, C, D, E, o1, o2);
	}

	public class EGenerator_Fractions_S02_DevTime : ExcerciseGenerator <Zadani_Fractions_S02_DevTime>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		/// 
		public EGenerator_Fractions_S02_DevTime() : base(4) {
			List<int> moznaA = GetRange(2, 10);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaC = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaD = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaE = SetOfRationals.GetAll(1, 9, true);

			foreach (int A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach(Q E in moznaE)
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
			if( !( ((Q)A-B) * C == (Q)1 ) )
				decision = 0;
			else if( !( (D.Operate(E, o2)).Num != 0 )  )
				decision = 1;
			else if( !( D.Den != E.Den ) )
 				decision = 2;
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(D, E, o2) )
				decision = 3;

			if(decision != -1) {
				illegalCounter[decision]++;
				if(illegalCounter[decision] < 1000)
					illegal[decision].Add( new Zadani_Fractions_S02_DevTime( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );
			} else {
			legit.Add( new Zadani_Fractions_S02_DevTime( A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), o1, o2) );
			}
		}

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q D, Q E, Op o) {
			Q result = D.Operate(E, o).GetInverse();
			return IsEasyZt(result.GetSimplestForm());
		}

		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		///
		protected override Excercise Construct(Zadani_Fractions_S02_DevTime z) {
			(int A, Q B, Q C, Q D, Q E, Op o1, Op o2) = z.Unpack();
			string[] steps = new string[7];
			string[] comments = new string[7];


			// Step 1:
			steps[0] = $"";
			comments[0] = $"";

			// Step 2:
			steps[1] = $"";
			comments[1] = $"";

			// Step 3:
			steps[2] = $"";
			comments[2] = $"";

			// Step 4:
			steps[3] = $"";
			comments[3] = $"";

			// Step 5:
			steps[4] = $"";
			comments[4] = $"";

			// Step 6:
			steps[5] = $"";
			comments[5] = $"";

			// Step 7:
			Q result = (Q)1;
			steps[6] = $"";
			comments[6] = "Hotovo! 😎😎";
			return new EFractions_S02(steps, comments, result);
		}
	}
}
