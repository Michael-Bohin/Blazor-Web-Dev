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
			(Op, Op)[] operatorCombinations = new (Op, Op)[] { (Op.Add, Op.Add), (Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) };

			aritmetickaKontrola = $"\nPro kontrolu aritmeticky by melo existovat celkem {moznaA.Count} * {moznaB.Count} * {moznaC.Count} * {moznaD.Count} * {operatorCombinations.Length} = {moznaA.Count * moznaB.Count * moznaC.Count * moznaD.Count * operatorCombinations.Length} moznosti.\n";

			foreach (Q A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach((Op opA, Op opB) in operatorCombinations)
								Consider(new Zadani_Fractions_S02_B(A.Copy(), B.Copy(), C.Copy(), D.Copy(), opA, opB));

			CreateStatsLog();
		}

		protected override void Consider(Zadani_Fractions_S02_B z) {
			// from pedagogic point of view: 
			// 1. "Jmenovatele dvojic A a B, C a D jsou různé"
			//			A.Den != B.Den && C.Den != D.Den
			// 2. "Aritmetika A op B vede zlomek, ktery nepreprezentuje cele cislo"
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

			if( ! (A.Den != B.Den && C.Den != D.Den) ) {
				illegal[0].Add(z);
			} else if( ! M.AritmetikaVedeNaNeceleCislo(A, B, opA) ) {
				illegal[1].Add(z);
			} else if( ! M.AritmetikaVedeNaNeceleCislo(C, D, opB) ) {
				illegal[2].Add(z);
			} else if( ! M.XJeKNasobekY(M.EuclidsLCM(C.Den, D.Den), M.EuclidsLCM(A.Den, B.Den))) {
				illegal[3].Add(z);
			} else {
				legit.Add(z);
			}
		}

		protected override Excercise Construct(Zadani_Fractions_S02_B z) { 
				
		}
	}
}
