using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
using Q = RationalNumber;
using M = MathAlgorithms; // required?

    public class EGenerator_Fractions_S02_A : ExcerciseGenerator
    {
        public record Zadani {
            public readonly Q A;
            public readonly Q B;
            public readonly int C;
            public readonly Q D;

            public Zadani(Q A, Q B, int C, Q D) {
                this.A = A; this.B = B; this.C = C; this.D = D;
            }
        }

        readonly List<Zadani> illegal1 = new();
        readonly List<Zadani> illegal2 = new();
        readonly List<Zadani> illegal3 = new();
        readonly List<Zadani> illegal4 = new();
        readonly List<Zadani> legit = new();

        public EGenerator_Fractions_S02_A() {
            // at this point generate all possible zadani in constructor
            // also filter each to belong to appropriate category
            List<Q> moznaA = SetOfRationals.GetAll(1, 9, true);
            List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
            List<Q> moznaD = SetOfRationals.GetAll(10, 19, true);
            List<int> moznaC = new();
            for (int i = 2; i < 11; i++) moznaC.Add(i);

            foreach (Q A in moznaA)
                foreach (Q B in moznaB)
                    foreach (int C in moznaC)
                        foreach (Q D in moznaD)
                            Consider( A, B, C, D);
        }

        void Consider(Q A, Q B, int C, Q D) {
            // from pedagogic point of view: 
            // 1. LCM(A.q, B.q) == D.q
            // 2. A.q = B.q
            // 3. Vysledky aritmetiky kroku B se NErovnaji (tady asi spadne 99% kombinaci)
            // 4. Vysledek nenalezi do dostatecne jednoduchych vysledku 
            // 5. Kombinace je pedagogicky legitimni 
            Zadani z = new(A, B, C, D);

            if(M.EuclidsLCM(A.Den, B.Den) == D.Den) {
                illegal1.Add(z);
                return;
            }

            if(A.Den == B.Den) {
                illegal1.Add(z);
                return;
            }

            if( VysledekAritmetikySeRovna(A, B, C, D)) {
                illegal3.Add(z);
                return;
            }

            if( VysledekNenaleziDoMoznychVysledku(A, B, C, D))
            {
                illegal4.Add(z);
                return;
            }

            legit.Add(z);
        }

        public override Excercise GetOne() {
            throw new NotImplementedException();
        }

        public override Excercise[] GetTen() {
            throw new NotImplementedException();
        }

        public override Excercise[] UnsafeGetAll() {
            throw new NotImplementedException();
        }
    } // end egen fractions 
} // end namespace IE
