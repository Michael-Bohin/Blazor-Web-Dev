using System;

namespace InfiniteEngine {
	using Q = RationalNumber;

    class MathAlgorithms {
        // Contains general math algorithms, that are usefull across different topics of math.
		// There are either multiple steps algorithms, or relatively simple equations 
		// that given a long name of function improve code readability of generators. 
        
        // find greatest common divisor of two integers:
        public static int EuclidsGCD(int a, int b) {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0)
                return b;
            if (b == 0)
                return a;

            int t;
            while(a % b != 0) {
                t = b;
                b = a % b;
                a = t;
            }
            return b;
        }

		// least common multiple -> multiply the two numbers and divide the result by GCD
        public static int EuclidsLCM(int a, int b) => a * b / EuclidsGCD(a, b);

		public static bool VysledekAritmetikySeRovna(Q A, Q B, int C, Q D, Op opA, Op opB) {
			int lcm = EuclidsLCM(A.Den, B.Den);
			int expandedA = A.Num * (lcm / A.Den);
			int expandedB = B.Num * (lcm / B.Den);
			int nahore = opA == Op.Add ? expandedA + expandedB : expandedA - expandedB;
			int dole = opB == Op.Add ? D.Den * C + D.Num : D.Den * C - D.Num;
			return nahore == dole;
		}

		public static bool VysledekNaleziDoMnozinyEasyZlomky(Q A, Q B, int C, Q D, Op opA, Op opB) {
			// spocitej vysledek, podivej jestli vysledek.Num je v [-10, 10] a vysledek.Den v [2, 10]
			Q top = opA == Op.Add ? A + B : A - B;
			Q bottom = opB == Op.Add ? (new Q(C) + D) : (new Q(C) - D);
			Q vysledek = top / bottom;
			int cit = vysledek.Num;
			int jm = vysledek.Den;
			return -11 < cit && cit < 11 && 1 < jm && jm < 11;
		}

		// je reduced vysledek operace cele cislo? 
		public static bool AritmetikaVedeNaNeceleCislo(Q A, Q B, Op op) => op == Op.Add ? (A + B).IsInteger() : (A - B).IsInteger();
		
		// existuje k z prirozenych cisel t.ž.: 
		// k >= 2 && (k * y) == x && x neni nula 
		public static bool XJeKNasobekY(int x, int y) => x % y == 0 && x != y && x != 0;
	}
}