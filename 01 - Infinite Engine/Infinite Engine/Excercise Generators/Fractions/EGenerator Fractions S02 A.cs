using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
using Q = RationalNumber;
using M = MathAlgorithms; // required?

    public class ToHTML
    {
        public static string Fraction(string left, string right)
        {
            Fraction f = new(left, right);
            return f.ToString();
        }
    }


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

        string[] cisla = new string[] { "nula", "jedna", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět"};
        string[] desitky = new string[] { "nula", "deset", "dvacet", "třicet", "čtyřicet", "padesát", "šedesát", "sedmdesát", "osmdesát", "devadesát"};
        string[] xtiny = new string[] { "nula", "jedniny", "poloviny", "třetiny", "čtvrtiny", "pětiny", "šestiny", "sedminy", "osminy", "devítiny", "jedenáctiny", "dvanáctiny", "třináctiny", "čtrnáctiny", "patnáctiny", "šestnáctiny", "sedmnáctiny", "osmnáctiny", "dvacetiny"};

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
            // 2. A.q == B.q
            // 3. Vysledky aritmetiky kroku B se NErovnaji (tady asi spadne 99% kombinaci)
            // 4. Vysledek nenalezi do dostatecne jednoduchych vysledku 
            // 5. Kombinace je pedagogicky legitimni
            Zadani z = new(A, B, C, D);

            if(M.EuclidsLCM(A.Den, B.Den) == D.Den) {
                illegal1.Add(z);
                return;
            }

            if(A.Den == B.Den) {
                illegal2.Add(z);
                return;
            }

            if( VysledekAritmetikySe_NE_Rovna(A, B, C, D)) {
                illegal3.Add(z);
                return;
            }

            if( VysledekNenaleziDoMoznychVysledku(A, B, C, D)) {
                illegal4.Add(z);
                return;
            }

            legit.Add(z);
        }

        static bool VysledekAritmetikySe_NE_Rovna(Q A, Q B, int C, Q D) {
            int lcm = M.EuclidsLCM(A.Den, B.Den);
            int nahore = (A.Num * lcm / A.Den) - (B.Num * lcm / B.Den);
            int dole = D.Den * C - D.Num;
            return nahore != dole;
        }

        static bool VysledekNenaleziDoMoznychVysledku(Q A, Q B, int C, Q D) {
            // spocitej vysledek, podivej jestli vysledek.Num je v [-10, 10] a vysledek.Den v [2, 10]
            Q vysledek = A + B / new Q(C) + D;
            int cit = vysledek.Num;
            int jm = vysledek.Den;
            return cit < -10 || 10 < cit || jm < 2 || 10 < jm;
        }

        List<Excercise> ConstructExcercises(List<Zadani> seznamZadani) {
            List<Excercise> result = new();
            foreach(Zadani z in seznamZadani)
                result.Add(Construct(z.A, z.B, z.C, z.D));
            return result;
        }

        

        Excercise Construct(Q a, Q b, int C, Q d) {
            Q A = a.Copy(); Q B = b.Copy(); Q D = d.Copy(); // @ defensive programming

            List<string> steps = new();
            List<string> comments = new();

            int lcm = M.EuclidsLCM(A.Den, B.Den);

            // step 1:
            string step = ToHTML.Fraction($"{A} + {B}", $"{C} + {D}");
            string comment = $"Ve jmenovateli převeď číslo {C} na zlomek se jmenovatelem {D.Den}";
            steps.Add(step);
            comments.Add(comment);

            // step 2:
            A.Expand(lcm / A.Den);
            B.Expand(lcm / B.Den);
            Q cRational = new(C * D.Den, D.Den);
            step = ToHTML.Fraction($"{A} + {B}", $"{cRational} + {D}");
            comment = $"Sečti zlomky";

            steps.Add(step);
            comments.Add(comment);

            // step 3: 
            // !! aritmeticke operace automaticky prevadi na zakladni zlomek, zde nepouzivat, protoze 
            // ty operatory jsou chytrejsi nez deti na ZS a z pohledu deti udelaji vic kroku najednou 
            Q X = new(A.Num + B.Num, A.Den);
            Q Y = new(cRational.Num + D.Num, D.Den);
            step = ToHTML.Fraction(X.ToString(), Y.ToString());
            comment = "Převeď dělení zlomků na jejich násobení.";

            steps.Add(step);
            comments.Add(comment);
        
            // step 4: 
            Y.Inverse();
            step = $"{X} ∙ {Y}";
            comment = $"Vykrať {X.Num} v čitateli i jmenovateli.";

            steps.Add(step);
            comments.Add(comment);

            // step 5:
            Q result = new(X.Num, Y.Den);
            step = $"{result}";
            comment = $"Převeď výsledný zlomek na jeho základní tvar.";

            steps.Add(step);
            comments.Add(comment);

            // step 6: 
            result.Reduce();
            step = $"{result}";
            comment = "Hotovo! :) :)";

            steps.Add(step);
            comments.Add(comment);
            
            return new EFractions_S02_A(steps.ToArray(), comments.ToArray(), result);

            // ! dodelat: a) detekovani ruznych koncu prevod na ZT nebo neprevod, 4 mozne kombinace +- v zadani
        }

        public List<Excercise> GetAllIllegal1 => ConstructExcercises(illegal1);
        public List<Excercise> GetAllIllegal2 => ConstructExcercises(illegal2);
        public List<Excercise> GetAllIllegal3 => ConstructExcercises(illegal3);
        public List<Excercise> GetAllIllegal4 => ConstructExcercises(illegal4);
        public List<Excercise> GetAllLegit => ConstructExcercises(legit);

        public override Excercise GetOne() {
            int pick = rand.Next(legit.Count);
            Zadani z = legit[pick];
            return Construct(z.A.Copy(), z.B.Copy(), z.C, z.D.Copy());
        }

        public override Excercise[] GetTen() {
            Excercise[] result = new Excercise[10];
            int[] randPerm = GetRandomPermutation(legit.Count);
            for(int i = 0; i < 10; i ++) {
                Zadani z = legit[randPerm[i]];
                result[i] = (Construct(z.A, z.B, z.C, z.D));
            }
            return result;
        }
    } // end egen fractions 
} // end namespace IE
