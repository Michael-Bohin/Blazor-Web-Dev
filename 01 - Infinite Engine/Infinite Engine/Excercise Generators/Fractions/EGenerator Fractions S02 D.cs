using System;
using System.Collections.Generic;
using static System.Console;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_D : Zadani
	{
		public readonly int A, B, C, D, E, F;
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_D(int A, int B, int C, int D, int E, int F, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.F = F; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_D : ExcerciseGenerator <Zadani_Fractions_S02_D>
	{

	}
}
