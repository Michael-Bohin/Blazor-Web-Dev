using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /*
         string[] comments = new string[8] {
                $"Převeď číslo {B.ToDouble()} na reprezentaci zlomkem.",
                $"Přetoč dělení zlomků {B.ToHTML()} : {C.ToHTML()} na jejich násobení.<br>Použij vzoreček: {AB.ToHTML()} : {CD.ToHTML()} = {AB.ToHTML()} ∙ {DC.ToHTML()}",
                $"V součinu {B.ToHTML()} ∙ {inverseC.ToHTML()} Vykrať jedničku ve tvaru {tvarVhodneJednicky.ToHTML()} = {roznasobenaVhodnaJednicka.ToHTML()}.<br>Poté zapiš součin jako jeden zlomek.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {stepThree.rightOperand.ToHTML()} v zakladním tvaru. Pokud ne, převeď ho na něj.",
                $"Najdi nejmenší společný násobek jmenovatelů {leftFour.Denominator.ToHTML()} a {rightFour.Denominator.ToHTML()}. (NSN nebo LCM jako least common multiple)<br>Zjisti jakým vhodným tvarem jedničky čísla rozšíříš na zlomky o tomto základu.",
                $"Spoj oba zlomky do jednoho a {sloveso} je.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {steps[6].ToHTML()} v základním tvaru. Pokud ne, převeď ho na něj.",
                "Hotovo. 😎😎"
            };  */

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
            steps[6] = preResult.ToHTML();
            comments[6] = $"Zkontroluj, jestli je zlomek {preResult.ToHTML()} v základním tvaru. Pokud ne, převeď ho na něj.";
            isoMods[6] = $"";

            /// Step 7:
            steps[7] = result.ToHTML();
            comments[7] = "Hotovo. 😎😎";
            isoMods[7] = "";

            return new EFractions_S02E01(steps, comments, isoMods, result, problem);
        }






        //////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /// Old code:
        /*
        static string[] CreateIsolatedSteps(Q A, Q B, Q C, Expression[] steps) {
            string[] isoMods = new string[8];
            // >>> 0 <<< 
            isoMods[0] = $"{B.ToDouble()} = {B.ToHTML()}";

            // >>> 1 <<< 
            BinaryExpression be01 = steps[1] as BinaryExpression;
            BinaryExpression be02 = steps[2] as BinaryExpression;

            isoMods[1] = $"{be01.rightOperand.ToHTML()} = {be02.rightOperand.ToHTML()}";

            // >>> 2 <<< 
            Integer i = C.Denominator as Integer;
            Integer j = B.Denominator as Integer;
            int Cden = i.number;
            int Bden = j.number;

            Q meziKrok = new(new Multiplication(B.Numerator, C.Denominator), new Multiplication(B.Denominator, C.Numerator));
            Q meziKrok02 = meziKrok.DeepCopy();
            meziKrok02.Numerator = new Multiplication(B.Numerator, new Multiplication(Cden / (Cden / Bden), Cden / Bden));

            BinaryExpression finalKrok = steps[3] as BinaryExpression;
            Q finalQ = finalKrok.rightOperand as Q;
            isoMods[2] = $"{be02.rightOperand.ToHTML()} = {meziKrok.ToHTML()} = {meziKrok02.ToHTML()} = {finalQ.ToHTML()}";

            // >>> 3 <<< 
            Q prvoCinitele = finalQ.DeepCopy();
            bool simplestForm = prvoCinitele.IsSimplestForm();
            prvoCinitele.PrimeFactorization();
            isoMods[3] += $"{finalQ.ToHTML()} = {prvoCinitele.ToHTML()} ";


            BinaryExpression step04 = steps[4] as BinaryExpression;
            Q right04 = step04.rightOperand.DeepCopy() as Q;

            if (simplestForm)
                isoMods[3] += "=> Je v základním tvaru.";
            else
                isoMods[3] += $"= {right04.ToHTML()}";

            // >>> 4 <<< 
            Integer xInteger = A.Denominator as Integer;
            int x = xInteger.number;

            Integer yInteger = right04.Denominator as Integer;
            int y = yInteger.number;

            Q left04 = step04.leftOperand.DeepCopy() as Q;
            int LCM = Q.EuclidsLCM(x, y);
            right04.Denominator = new Integer(LCM);
            left04.Denominator = new Integer(LCM);


            Integer aInteger = left04.Numerator as Integer;
            int leftNum = aInteger.number; // -> try creating implicit conversion to int! :) 

            Integer bInteger = right04.Numerator as Integer;
            int rightNum = bInteger.number;

            right04.Numerator = new Multiplication(new Integer(rightNum), new Q(LCM, y));
            left04.Numerator = new Multiplication(new Integer(leftNum), new Q(LCM, x));

            bool plus = steps[0] is Addition;
            string operatorRepr = plus ? "+" : "−";


            isoMods[4] = $"LCM({x}, {y}) = {LCM}<br>";

            int left = LCM / x;
            int right = LCM / y;

            Q vhodnaLeft = new(left, left);
            Q vhodnaRight = new(right, right);

            isoMods[4] += $"Vhodná jednička nalevo: {LCM} : {x} = {left} => {vhodnaLeft.ToHTML()}<br>";
            isoMods[4] += $"Vhodná jednička napravo: {LCM} : {y} = {right} => {vhodnaRight.ToHTML()}<br>";


            isoMods[4] += $"{steps[4].ToHTML()} = ";

            BinaryExpression be = steps[4] as BinaryExpression;
            Q beLeft = be.leftOperand as Q;
            Q beRight = be.rightOperand as Q;

            isoMods[4] += $"{beLeft.ToHTML()} ∙ {vhodnaLeft.ToHTML()} {operatorRepr} {beRight.ToHTML()} ∙ {vhodnaRight.ToHTML()}";

            right04.Numerator = new Multiplication(rightNum, LCM / y);
            left04.Numerator = new Multiplication(leftNum, LCM / x);
            //isoMods[4] += $" = {left04.ToHTML()} {operatorRepr} {right04.ToHTML()}";
            right04.Numerator = new Integer(rightNum * LCM / y);
            left04.Numerator = new Integer(leftNum * LCM / x);
            isoMods[4] += $" = {left04.ToHTML()} {operatorRepr} {right04.ToHTML()}";


            // >>> 5 <<< 
            int l = leftNum * LCM / x;
            int r = rightNum * LCM / y;
            int finalNum = plus ? l + r : l - r;

            Q predPredVysledek = plus ? new(new Addition(l, r), new Integer(LCM)) : new(new Subtraction(l, r), new Integer(LCM));
            Q predVysledek = new(finalNum, LCM);
            isoMods[5] = $"{left04.ToHTML()} {operatorRepr} {right04.ToHTML()} = {predPredVysledek.ToHTML()} = {predVysledek.ToHTML()}";

            // >>> 6 <<< 
            prvoCinitele = predVysledek.DeepCopy();
            simplestForm = prvoCinitele.IsSimplestForm();
            prvoCinitele.PrimeFactorization();
            isoMods[6] = $"{predVysledek.ToHTML()} = ";
            if (finalNum < 0) {
                Minus negativePrvoCinitele = new(prvoCinitele);
                isoMods[6] += negativePrvoCinitele.ToHTML();
            } else {
                isoMods[6] += prvoCinitele.ToHTML();
            }

            if (simplestForm)
                isoMods[6] += " => Je v základním tvaru.";
            else
                isoMods[6] += $" = {steps[7].ToHTML()}";

            return isoMods;
        }

        static Expression[] CreateSteps(Q A, Q B, Q C, bool plus) {
            Expression[] steps = new Expression[8];
            // right operand will always be overwritten, A.DeepCopy will stay there up to step 4
            // This line mainly defines usage of addition or subtraction. Right operands are there to just fill it with something
            BinaryExpression root = plus ? new Addition(A.DeepCopy(), new RealNumber(123456.789)) : new Subtraction(A.DeepCopy(), new RealNumber(123456.789));

            Integer i = C.Numerator as Integer;
            Integer j = C.Denominator as Integer;
            int Cnum = i.number;
            int Cden = j.number;

            Integer k = B.Numerator as Integer;
            Integer l = B.Denominator as Integer;
            int Bnum = k.number;
            int Bden = l.number;

            Integer m = A.Numerator as Integer;
            Integer n = A.Denominator as Integer;
            int Anum = m.number;
            int Aden = n.number;

            // >>> 0 <<<
            BinaryExpression stepZero = root.DeepCopy();
            stepZero.rightOperand = new Division(B.ToDouble(), C.DeepCopy());
            steps[0] = stepZero;

            // >>> 1 <<< 
            BinaryExpression stepOne = root.DeepCopy();
            stepOne.rightOperand = new Division(B.DeepCopy(), C.DeepCopy());
            steps[1] = stepOne;

            // >>> 2 <<< 
            BinaryExpression stepTwo = root.DeepCopy();
            stepTwo.rightOperand = new Multiplication(B.DeepCopy(), new Q(Cden, Cnum));
            steps[2] = stepTwo;

            // >>> 3 <<< 
            BinaryExpression stepThree = root.DeepCopy();
            stepThree.rightOperand = new Q(Bnum * (Cden / Bden), Cnum);
            steps[3] = stepThree;

            // >>> 4 <<<
            BinaryExpression stepFour = root.DeepCopy();
            Q f4 = new(Bnum * (Cden / Bden), Cnum);
            f4.Reduce();
            stepFour.rightOperand = f4;
            steps[4] = stepFour;

            // >>> 5 <<< 
            // priprava:
            Integer ZTden = f4.Denominator as Integer;
            int ZT_Cden = ZTden.number;
            int LCM = Q.EuclidsLCM(Aden, ZT_Cden);
            int levyCinitel = Anum * LCM / Aden;
            int pravyCinitel = Bnum * Cden / Bden * LCM / Cnum; // sick :D
            // priprava done

            BinaryExpression stepFive = root.DeepCopy();
            stepFive.leftOperand = new Q(levyCinitel, LCM);
            stepFive.rightOperand = new Q(pravyCinitel, LCM);
            steps[5] = stepFive;

            // >>> 6 <<<
            //BinaryExpression cinitel = root.DeepCopy();
            //cinitel.leftOperand = new Integer(levyCinitel);
            //cinitel.rightOperand = new Integer(pravyCinitel);
            int cinitel = root is Addition ? levyCinitel + pravyCinitel : levyCinitel - pravyCinitel;
            Q stepSix = new(cinitel, LCM);
            steps[6] = stepSix;

            // >>> 7 <<< 
            Q stepSeven = stepSix.DeepCopy();
            Integer sevenI = stepSeven.leftOperand as Integer;
            int numerator = sevenI.number;
            bool negative = numerator < 0;

            if (negative)
                stepSeven = new Q(new Integer(Math.Abs(numerator)), stepSeven.rightOperand);
            stepSeven.Reduce();

            steps[7] = negative ? new Minus(stepSeven) : stepSeven;
            return steps;
        }

        static string[] CreateComments(Q A, Q B, Q C, Expression[] steps) {
            Fraction AB = new("A", "B");
            Fraction CD = new("C", "D");
            Fraction DC = new("D", "C");

            string sloveso = steps[0] is Addition ? "sečti" : "odečti";
            Integer i = C.Denominator as Integer;
            Integer j = B.Denominator as Integer;
            int Cden = i.number;
            int Bden = j.number;

            Q inverseC = new(C.Denominator, C.Numerator);
            Q tvarVhodneJednicky = new(Cden, Bden);
            Q roznasobenaVhodnaJednicka = new(new Multiplication(Cden / (Cden / Bden), Cden / Bden), B.Denominator);

            BinaryExpression stepThree = steps[3] as BinaryExpression;
            BinaryExpression stepFour = steps[4] as BinaryExpression;
            Q leftFour = stepFour.leftOperand as Q;
            Q rightFour = stepFour.rightOperand as Q;

            string[] comments = new string[8] {
                $"Převeď číslo {B.ToDouble()} na reprezentaci zlomkem.",
                $"Přetoč dělení zlomků {B.ToHTML()} : {C.ToHTML()} na jejich násobení.<br>Použij vzoreček: {AB.ToHTML()} : {CD.ToHTML()} = {AB.ToHTML()} ∙ {DC.ToHTML()}",
                $"V součinu {B.ToHTML()} ∙ {inverseC.ToHTML()} Vykrať jedničku ve tvaru {tvarVhodneJednicky.ToHTML()} = {roznasobenaVhodnaJednicka.ToHTML()}.<br>Poté zapiš součin jako jeden zlomek.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {stepThree.rightOperand.ToHTML()} v zakladním tvaru. Pokud ne, převeď ho na něj.",
                $"Najdi nejmenší společný násobek jmenovatelů {leftFour.Denominator.ToHTML()} a {rightFour.Denominator.ToHTML()}. (NSN nebo LCM jako least common multiple)<br>Zjisti jakým vhodným tvarem jedničky čísla rozšíříš na zlomky o tomto základu.",
                $"Spoj oba zlomky do jednoho a {sloveso} je.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {steps[6].ToHTML()} v základním tvaru. Pokud ne, převeď ho na něj.",
                "Hotovo. 😎😎"
            };

            return comments;
        }*/

        ///////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////

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