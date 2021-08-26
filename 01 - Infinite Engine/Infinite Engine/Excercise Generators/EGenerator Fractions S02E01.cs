using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using Q = RationalNumber;
    using M = MathAlgorithms; // required?

    public class EGenerator_Fractions_S02E01 : ExcerciseGenerator {
        string[] steps = new string[8];
        string[] comments = new string[8];
        string[] isoMods = new string[8];

        int lowerA = 0; // difficulty bounds for CSGs
        int upperA = 0;
        int lowerC = 0;
        int upperC = 0;
       
        readonly List<Q> mnozinaA = new();
        readonly List<Q> mnozinaB;
        readonly List<Q> mnozinaBmini;

        readonly List<Q> mnozinaC = new();
        readonly List<Q> mnozinaC_Poloviny = new();
        readonly List<Q> mnozinaC_Ctvrtiny = new();
        readonly List<Q> mnozinaC_Petiny = new();
        readonly List<Q> mnozinaC_Desetiny = new();

        readonly FractionsInSimplestForm fsf = new();

        public EGenerator_Fractions_S02E01() : this(Dificulty.PRIJIMACKY) { }

        // create different sets of fractions depending on the dificulty
        public EGenerator_Fractions_S02E01(Dificulty l) : base(l) {
            mnozinaB = fsf.GetEasilyFractionable();
            mnozinaBmini = fsf.GetEasilyFractionableMini();
            SetDificultyBounds();

            if (Dificulty.MENSI == level)
                mnozinaB = mnozinaBmini;

            // define setA:
            mnozinaA = FractionsInSimplestForm.GetAll(lowerA, upperA, mnozinaB);

            // define setC:
            if (level != Dificulty.CPU)
                mnozinaC = FractionsInSimplestForm.GetAll(lowerC, upperC);
            else  // do not generate all for CPU ranges
                for (int i = 0; i < 2000; i++)
                    mnozinaC.Add(new Q(rand.Next(lowerC, upperC), rand.Next(lowerC, upperC)));

            DefineFilteredSetC();
        }

        void SetDificultyBounds() {
            switch (level) {
                case Dificulty.MENSI:
                    SetBounds(1, 4, 5, 10); break;
                case Dificulty.PRIJIMACKY:
                    SetBounds(1, 5, 6, 30); break;
                case Dificulty.VETSI:
                    SetBounds(1, 10, 11, 60); break;
                case Dificulty.OBROVSKA:
                    SetBounds(10, 50, 51, 200); break;
                case Dificulty.CPU:
                    SetBounds(50, 500, 10_000, 50_001); break;
                default: break;
            }
        }

        void SetBounds(int a, int b, int c, int d) {
            lowerA = a;
            upperA = b;
            lowerC = c;
            upperC = d;
        }

        void DefineFilteredSetC() {
            foreach (Q possibleC in mnozinaC) {
                int Cden = possibleC.Den;
                if (Cden % 2 == 0) {
                    mnozinaC_Poloviny.Add(possibleC);
                    if (Cden % 4 == 0)
                        mnozinaC_Ctvrtiny.Add(possibleC);
                }

                if (Cden % 5 == 0) {
                    mnozinaC_Petiny.Add(possibleC);
                    if (Cden % 10 == 0)
                        mnozinaC_Desetiny.Add(possibleC);
                }
            }
        }
        /// end of constructor helper functions

        public EFractions_S02E01 UnsafeGetExactlyThis(Q A, Q B, Q C, bool plus) => GetExactlyThis(A, B, C, plus);

        EFractions_S02E01 GetExactlyThis(Q A, Q B, Q C, bool plus) {
            /// Reset steps, comments and isoMods: 
            steps = new string[8];
            comments = new string[8];
            isoMods = new string[8];
            string op = plus ? "+" : "-";
            Division rightP = new(B.Copy(), C.Copy());
            Expression problem = plus ? new Addition(A.Copy(), rightP) : new Subtraction(A.Copy(), rightP);

            /// Step 0:
            steps[0] = $"{A.ToHTML()} {op} {B.ToDouble()} : {C.ToHTML()}";
            comments[0] = $"Převeď číslo {B.ToDouble()} na reprezentaci zlomkem.";
            isoMods[0] = $"{B.ToDouble()} = {B.ToHTML()}";

            /// Step 1:
            Fraction AB = new("A", "B");
            Fraction CD = new("C", "D");
            Fraction DC = new("D", "C");

            steps[1] = $"{A.ToHTML()} {op} {B.ToHTML()} : {C.ToHTML()}";
            comments[1] = $"Přetoč dělení zlomků {B.ToHTML()} : {C.ToHTML()} na jejich násobení.<br>Použij vzoreček: {AB.ToHTML()} : {CD.ToHTML()} = {AB.ToHTML()} ∙ {DC.ToHTML()}";
            isoMods[1] = $"{B.ToHTML()} : {C.ToHTML()} = {B.ToHTML()} ∙ ";
            C.Inverse();
            isoMods[1] += C.ToHTML();

            /// Step 2:
            Fraction soucinVJednom = new($"{B.Num} ∙ {C.Num}", $"{B.Den} ∙ {C.Den}");
            string leveRoznasobeni = C.Num != B.Den ? $"{B.Num} ∙ {B.Den} ∙ {C.Num / B.Den}" : $"{B.Num} ∙ {B.Den}";
            Fraction roznasobenySoucin = new(leveRoznasobeni, $"{B.Den} ∙ {C.Den}");

            string roznasobeni = C.Num != B.Den ? $"= {roznasobenySoucin.ToHTML()} " : "";
            Q vykracenyZlomek = new(B.Num * C.Num / B.Den, C.Den);

            steps[2] = $"{A.ToHTML()} {op} {B.ToHTML()} ∙ {C.ToHTML()}";
            comments[2] = $"Součin {B.ToHTML()} ∙ {C.ToHTML()} zapiš jako jeden zlomek.<br>Vykrať {B.Den} ve jmenovateli s {C.Num} v čitateli.";
            isoMods[2] = $"{B.ToHTML()} ∙ {C.ToHTML()} = {soucinVJednom.ToHTML()} {roznasobeni}= {vykracenyZlomek.ToHTML()}";

            /// Step 3:
            bool simplestForm = vykracenyZlomek.IsSimplestForm();
            Fraction primeFactorization_01 = vykracenyZlomek.GetPrimeFactorization();
            string ending = simplestForm ? "=> Je v základním tvaru." : $"= {vykracenyZlomek.GetSimplestForm().ToHTML()}";

            steps[3] = $"{A.ToHTML()} {op} {vykracenyZlomek.ToHTML()}";
            comments[3] = $"Zkontroluj, jestli je zlomek {vykracenyZlomek.ToHTML()} v zakladním tvaru. Pokud ne, převeď ho na něj.";
            isoMods[3] = $"{vykracenyZlomek.ToHTML()} = {primeFactorization_01.ToHTML()} {ending}";

            if (!simplestForm)
                vykracenyZlomek.Reduce();

            /// Step 4:
            int Aden = A.Den;
            int Bden = vykracenyZlomek.Den;
            int LCM = M.EuclidsLCM(Aden, Bden);
            int left = LCM / Aden;
            int right = LCM / Bden;
            Q jednickaLeft = new(left, left);
            Q jednickaRight = new(right, right);

            Q rozsirenyLeft = new(A.Num * left, Aden * left);
            Q rozsirenyRight = new(vykracenyZlomek.Num * right, Bden * right);
            string step05 = $"{rozsirenyLeft.ToHTML()} {op} {rozsirenyRight.ToHTML()}";

            string isoMods4 = $"LCM({Aden}, {Bden}) = {LCM}<br>";
            isoMods4 += $"Vhodná jednička nalevo: {LCM} : {Aden} = {left} => {jednickaLeft.ToHTML()}<br>";
            isoMods4 += $"Vhodná jednička napravo: {LCM} : {Bden} = {right} => {jednickaRight.ToHTML()}<br>";
            isoMods4 += $"{A.ToHTML()} {op} {vykracenyZlomek.ToHTML()} = {A.ToHTML()} ∙ {jednickaLeft.ToHTML()} {op} {vykracenyZlomek.ToHTML()} ∙ {jednickaRight.ToHTML()} = {step05}";
            
            steps[4] = $"{A.ToHTML()} {op} {vykracenyZlomek.ToHTML()}";
            comments[4] = $"Najdi nejmenší společný násobek jmenovatelů {Aden} a {Bden}.<br>LCM jako least common multiple.<br>Najdi vhodné tvary jedniček pro rozšíření na zlomky o tomto základu.";
            isoMods[4] = isoMods4;

            /// Step 5:
            string sloveso = plus ? "sečti" : "odečti";
            int jmenovatel = rozsirenyLeft.Den;
            int l = rozsirenyLeft.Num;
            int r = rozsirenyRight.Num;
            int cit = plus ? l + r : l - r;   
            Fraction spojeni = new($"{l} {op} {r}", $"{jmenovatel}");
            Q preResult = new(cit, jmenovatel);

            steps[5] = step05;
            comments[5] = $"Spoj oba zlomky do jednoho a {sloveso} je.";
            isoMods[5] = $"{step05} = {spojeni.ToHTML()} = {preResult.ToHTML()}";

            /// Step 6:
            Q result = preResult.GetSimplestForm();
            Fraction preResultFact = preResult.GetPrimeFactorization();
            ending = preResult.IsSimplestForm() ? "=> Je v základním tvaru." : $"= {result.ToHTML()}";

            steps[6] = preResult.ToHTML();
            comments[6] = $"Zkontroluj, jestli je zlomek {preResult.ToHTML()} v základním tvaru. Pokud ne, převeď ho na něj.";
            isoMods[6] = $"{preResult.ToHTML()} = {preResultFact.ToHTML()} {ending}";

            /// Step 7:
            steps[7] = result.ToHTML();
            comments[7] = "Hotovo. 😎😎";
            isoMods[7] = "";

            return new EFractions_S02E01(steps, comments, isoMods, result, problem);
        }


        public override Excercise GetOne() {
            (Q A, Q B, Q C) = ChooseAnyCSG();
            return GetExactlyThis(A, B, C, CoinFlip());
        }

        (Q, Q, Q) ChooseAnyCSG() {
            Q A = ChooseAnyQ(mnozinaA);
            Q B = ChooseAnyQ(mnozinaB);
            int Bden = B.Den;

            List<Q> filteredC = new();
            foreach (Q f in mnozinaC)
                if (f.Den % Bden == 0)
                    filteredC.Add(f);
            Q C = ChooseAnyQ(filteredC);

            return (A, B, C);
        }

        Q ChooseAnyQ(List<Q> from) => from[rand.Next(0, from.Count)].DeepCopy(); // here the deepcopy is extremely important
        public override Excercise[] GetTwo() { throw new NotImplementedException(); }
        public override Excercise[] GetTen() { throw new NotImplementedException(); }

        public Excercise[] GetEight() {
            Q[] acka = new Q[8];
            for (int i = 0; i < 8; i++)
                acka[i] = ChooseAnyQ(mnozinaA);

            Q[] becka = new Q[8];
            becka[0] = new Q(1, 2);
            becka[1] = new Q(1, 2);
            becka[2] = new Q(1, 4);
            becka[3] = new Q(3, 4);

            Q[] moznePetiny = new Q[4] { new Q(1, 5), new Q(2, 5), new Q(3, 5), new Q(4, 5) };
            Q[] mozneDesetiny = new Q[] { new Q(1, 10), new Q(3, 10), new Q(9, 10) };

            int[] permPetin = GetRandomPermutation(4);
            int[] permDesetin = GetRandomPermutation(3);

            becka[4] = moznePetiny[permPetin[0]];
            becka[5] = moznePetiny[permPetin[1]];

            becka[6] = mozneDesetiny[permDesetin[0]];
            becka[7] = mozneDesetiny[permDesetin[1]];

            int[] shuffleBecka = GetRandomPermutation(8);

            Q[] KonecnaBecka = new Q[8];
            for (int i = 0; i < 8; i++)
                KonecnaBecka[i] = becka[shuffleBecka[i]];

            Q[] cecka = new Q[8];

            for (int i = 0; i < 8; i++) {
                int BDEN = KonecnaBecka[i].Den;
                if (BDEN == 2) {
                    cecka[i] = ChooseAnyQ(mnozinaC_Poloviny);
                } else if (BDEN == 4) {
                    cecka[i] = ChooseAnyQ(mnozinaC_Ctvrtiny);
                } else if (BDEN == 5) {
                    cecka[i] = ChooseAnyQ(mnozinaC_Petiny);
                } else if (BDEN == 10) {
                    cecka[i] = ChooseAnyQ(mnozinaC_Desetiny);
                }
            }
            
            int[] permuation = GetRandomPermutation(8);

            EFractions_S02E01[] result = new EFractions_S02E01[8];
            for (int i = 0; i < 8; i++) {
                // assert result is not zero or one. in both cases, change A untill it is. 
                while( ResultIsZeroOrOne(acka[i], KonecnaBecka[i], cecka[i], permuation[i] < 4)) {
                    acka[i] = ChooseAnyQ(mnozinaA);
                }
                result[i] = GetExactlyThis(acka[i], KonecnaBecka[i], cecka[i], permuation[i] < 4);
                // je-li vysledek prilis velky proti omezenim obtiznosti, ponechej becko a vytoc nove A a C a podivej se jestli tentokrat je jiz vse ok
                int BDEN = KonecnaBecka[i].Den;
                while (!LevelIsOk(result[i])) {
                    acka[i] = ChooseAnyQ(mnozinaA);
                    if (BDEN == 2) {
                        cecka[i] = ChooseAnyQ(mnozinaC_Poloviny);
                    } else if (BDEN == 4) {
                        cecka[i] = ChooseAnyQ(mnozinaC_Ctvrtiny);
                    } else if (BDEN == 5) {
                        cecka[i] = ChooseAnyQ(mnozinaC_Petiny);
                    } else if (BDEN == 10) {
                        cecka[i] = ChooseAnyQ(mnozinaC_Desetiny);
                    }
                    result[i] = GetExactlyThis(acka[i], KonecnaBecka[i], cecka[i], CoinFlip());
                }
            }
            return result;
        }

        bool ResultIsZeroOrOne(Q A, Q B, Q C, bool plus) {
            Console.WriteLine($" Checking: {A}  {B}  {C}  {plus}");
            Q right = B / C;
            right.Reduce();
            Q result = plus ? A + right : A - right;
            result.Reduce();
            Console.WriteLine("The result is: " + result);
            if(result.Num == 0 || result.Num == 1) {
                Console.WriteLine(">>>>>>>> Zero or one detected!! <<<<<<");
                return true;
            }

            return false;
        }

        bool LevelIsOk(EFractions_S02E01 e) {
            // this depends on the level and it restrictions
            if (level == Dificulty.CPU)
                return true; // no resetrictions on CPU level.. :) 

            Q answer = e.Answer;
            int num = answer.Num;
            int den = answer.Den;
            return true;
            /*
            if (level == Dificulty.MENSI)
                return num < 21 && den < 21;
            else if (level == Dificulty.PRIJIMACKY)
                return num < 41 && den < 41;
            else if (level == Dificulty.VETSI)
                return num < 250 && den < 250 && (50 < num || 50 < den);
            return num < 10_000 && den < 10_000;*/
        }

        public Excercise[] GetAll() {
            List<Excercise> result = new();
            foreach (Q A in mnozinaA)
                foreach (Q B in mnozinaB) {
                    int den = B.Den;
                    List<Q> filtrovanaMnozinaC = new();
                    foreach (Q f in mnozinaC)
                        if (f.Den % den == 0)
                            filtrovanaMnozinaC.Add(f);
                    foreach (Q C in filtrovanaMnozinaC)
                        result.Add(GetExactlyThis(A, B, C, CoinFlip()));
                }
            return result.ToArray();
        }
    }
}