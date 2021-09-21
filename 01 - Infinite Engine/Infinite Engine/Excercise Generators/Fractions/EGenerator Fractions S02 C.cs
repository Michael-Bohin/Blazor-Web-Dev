using System;
using System.Collections.Generic;
using static System.Console;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_C : Zadani
	{
		public readonly int A;
		public readonly Q B, C, D, E;
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_C(int A, Q B, Q C, Q D, Q E, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_C : ExcerciseGenerator <Zadani_Fractions_S02_C>
	{

	}
}
