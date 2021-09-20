using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine.Excercise_Generators.Fractions
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_B : Zadani
	{
		public readonly Q A, B, D;
		public readonly int C;
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_B(Q A, Q B, int C, Q D, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.opA = opA; this.opB = opB;
		}
	}
}
