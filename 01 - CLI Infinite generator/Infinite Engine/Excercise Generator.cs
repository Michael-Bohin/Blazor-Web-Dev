using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {

    public abstract class ExcerciseGenerator {
        protected Random rand = new Random();
        public ExcerciseGenerator() { }

        // generates random permutation of integers from 0 to ( count - 1 )
        // 0 returns 0
        // 1 returns [ 0, 1] or [ 1, 0] 
        // 2 returns [ 0, 1, 2] or [ 0, 2, 1] or [ 1, 0, 2] or .....
        // meaning Length of the array will always be count, but maximum number will be count - 1
        // Note: unit test even distribution against large numbers
        protected int[] GetRandomPermutation(int count) {
            if (count < 1)
                throw new InvalidOperationException("GetRandomPermuation can't use negative boundary. :D ");
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
                result[i] = i;

            for (int i = 0; i < count - 1; i++) {
                // choose number index greater or equal than current index to swap elements with 
                int target = rand.Next(i, count);

                int temp = result[i];
                result[i] = result[target];
                result[target] = temp;
            }
            return result;
        }

        protected bool CoinFlip() => rand.Next(0, 2) == 0;

        public abstract Excercise GetNext();
    }

    public enum Dificulty {
        MENSI, PRIJIMACKY, VETSI, OBROVSKA, CPU
    }

    public class EGenerator_Fractions_S02E01 : ExcerciseGenerator {
        Dificulty level;
        public EGenerator_Fractions_S02E01() : base() { }
        public EGenerator_Fractions_S02E01(Dificulty level) : base() {
            this.level = level;
            //static bool fun(Fraction f) => f.ToDouble() > 1;
            //Predicate<Fraction> thatAreGreaterThanOne = fun;
            /*mnozinaA.RemoveAll( (Fraction f) => { 
                return f.ToDouble() > 1 ; 
            });*/
            //mnozinaA.RemoveAll(thatAreGreaterThanOne);
            FractionsInSimplestForm fsf = new();
            int lowerA = 0; int upperA = 0; int lowerC = 0; int upperC = 0;

            if (Dificulty.MENSI == level)
                mnozinaB = mnozinaBmini;

            switch (level) {
                case Dificulty.MENSI:
                    lowerA = 1; upperA = 4;
                    lowerC = 5; upperC = 10; break;

                case Dificulty.PRIJIMACKY:
                    lowerA = 1; upperA = 5;
                    lowerC = 6; upperC = 30; break;

                case Dificulty.VETSI:
                    lowerA = 1; upperA = 10;
                    lowerC = 11; upperC = 60; break;

                case Dificulty.OBROVSKA:
                    lowerA = 10; upperA = 50;
                    lowerC = 51; upperC = 200; break;


                case Dificulty.CPU:
                    lowerA = 50; upperA = 500;
                    lowerC = 10_000; upperC = 50_001; break;

                default:
                    break;
            }

            mnozinaA = fsf.GetAll(lowerA, upperA, mnozinaB);
            List<Fraction> neomezenaMnozinaC = new();
            
            if(level != Dificulty.CPU) // just generate all options, not nessecasry to pick randomly some of them 
                neomezenaMnozinaC = fsf.GetAll(lowerC, upperC);
            else  // but do not generate all for CPU ranges
                for (int i = 0; i < 2000; i++)
                    neomezenaMnozinaC.Add(new Fraction(rand.Next(lowerC, upperC), rand.Next(lowerC, upperC)));
            
            foreach (Fraction possibleC in neomezenaMnozinaC) {
                Integer C = possibleC.Denominator as Integer;
                int Cden = C.number;
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

        readonly List<Fraction> mnozinaA = new();
        readonly List<Fraction> mnozinaB = new() {
            new Fraction(1, 10),
            new Fraction(1, 5),
            new Fraction(1, 4),
            new Fraction(3, 10),
            new Fraction(2, 5),
            new Fraction(1, 2),
            new Fraction(3, 5),
            new Fraction(3, 4),
            new Fraction(4, 5),
            new Fraction(9, 10)
        };
        readonly List<Fraction> mnozinaBmini = new() {
            new Fraction(1, 10),
            new Fraction(1, 5),
            new Fraction(1, 4),
            new Fraction(1, 2)
        };

        readonly List<Fraction> neomezenaMnozinaC = new();
        readonly List<Fraction> mnozinaC_Poloviny = new();
        readonly List<Fraction> mnozinaC_Ctvrtiny = new();
        readonly List<Fraction> mnozinaC_Petiny = new();
        readonly List<Fraction> mnozinaC_Desetiny = new();

        // this can brake some pedagogic logic, since user can state fractions without any limit, if called directly
        // becasue you can call itt directly but through the Unsafe prefix
        public Excercise UnsafeGetExactlyThis(Fraction A, Fraction B, Fraction C, bool plus) => GetExactlyThis(A, B, C, plus);
        
        private Excercise GetExactlyThis(Fraction A, Fraction B, Fraction C, bool plus) {
            Expression[] steps = CreateSteps(A, B, C, plus);
            string[] comments = CreateComments(A, B, C, steps);
            string[] isolatedModifications = CreateIsolatedSteps(A, B, C, steps);
            EFractions_S02E01 result = new(steps, comments, isolatedModifications);
            return result;
        }

        private string[] CreateIsolatedSteps(Fraction A, Fraction B, Fraction C, Expression[] steps) {
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

            Fraction meziKrok = new Fraction( new Multiplication( B.Numerator, C.Denominator), new Multiplication( B.Denominator, C.Numerator) );
            Fraction meziKrok02 = meziKrok.DeepCopy();
            meziKrok02.Numerator = new Multiplication(B.Numerator, new Multiplication(Cden / (Cden / Bden), Cden / Bden));

            BinaryExpression finalKrok = steps[3] as BinaryExpression;
            Fraction finalFraction = finalKrok.rightOperand as Fraction;
            isoMods[2] = $"{be02.rightOperand.ToHTML()} = {meziKrok.ToHTML()} = {meziKrok02.ToHTML()} = {finalFraction.ToHTML()}";

            // >>> 3 <<< 
            Fraction prvoCinitele = finalFraction.DeepCopy();
            bool simplestForm = prvoCinitele.IsSimplestForm();
            prvoCinitele.PrimeFactorization();
            isoMods[3] += $"{finalFraction.ToHTML()} = {prvoCinitele.ToHTML()} ";


            BinaryExpression step04 = steps[4] as BinaryExpression;
            Fraction right04 = step04.rightOperand.DeepCopy() as Fraction;

            if(simplestForm)
                isoMods[3] += "=> Je v základním tvaru.";
            else 
                isoMods[3] += $"= {right04.ToHTML()}";

            // >>> 4 <<< 
            Integer xInteger = A.Denominator as Integer;
            int x = xInteger.number;

            Integer yInteger = right04.Denominator as Integer;
            int y = yInteger.number;

            Fraction left04 = step04.leftOperand.DeepCopy() as Fraction;
            int LCM = Fraction.EuclidsLCM(x, y);
            right04.Denominator = new Integer(LCM);
            left04.Denominator = new Integer(LCM);


            Integer aInteger = left04.Numerator as Integer;
            int leftNum = aInteger.number; // -> try creating implicit conversion to int! :) 

            Integer bInteger = right04.Numerator as Integer;
            int rightNum = bInteger.number;

            right04.Numerator = new Multiplication( new Integer(rightNum), new Fraction(LCM, y) );
            left04.Numerator = new Multiplication( new Integer(leftNum), new Fraction(LCM, x) );

            bool plus = steps[0] is Addition;
            string operatorRepr = plus ? "+" : "−";


            isoMods[4] = $"LCM({x}, {y}) = {LCM}<br>{step04.ToHTML()} = {left04.ToHTML()} {operatorRepr} {right04.ToHTML()}";
            right04.Numerator = new Multiplication(rightNum, LCM/ y);
            left04.Numerator = new Multiplication(leftNum, LCM/ x);
            isoMods[4] += $" = {left04.ToHTML()} {operatorRepr} {right04.ToHTML()}";
            right04.Numerator = new Integer(rightNum * LCM / y);
            left04.Numerator = new Integer(leftNum * LCM / x);
            isoMods[4] += $" = {left04.ToHTML()} {operatorRepr} {right04.ToHTML()}";




            // >>> 5 <<< 
            int l = leftNum * LCM / x;
            int r = rightNum * LCM / y;
            int finalNum = plus ? l + r : l - r;
            
            Fraction predPredVysledek = plus ? new( new Addition(l, r), new Integer(LCM)) : new(new Subtraction(l, r), new Integer(LCM));
            Fraction predVysledek = new( finalNum, LCM);
            isoMods[5] = $"{left04.ToHTML()} {operatorRepr} {right04.ToHTML()} = {predPredVysledek.ToHTML()} = {predVysledek.ToHTML()}";

            // >>> 6 <<< 
            prvoCinitele = predVysledek.DeepCopy();
            simplestForm = prvoCinitele.IsSimplestForm();
            prvoCinitele.PrimeFactorization();
            isoMods[6] = $"{predVysledek.ToHTML()} = ";
            if (finalNum < 0) {
                Minus negativePrvoCinitele = new Minus(prvoCinitele);
                isoMods[6] += negativePrvoCinitele.ToHTML();
            } else {
                isoMods[6] += prvoCinitele.ToHTML();
            }

            if (simplestForm)
                isoMods[6] += " => Je v základním tvaru.";
            else
                isoMods[6] += $" = {steps[7].ToHTML()}";

            // >>> 7 <<< 
            //isoMods[7] = $"77";

            return isoMods;
        }

        
        private Expression[] CreateSteps(Fraction A, Fraction B, Fraction C, bool plus) {
            Expression[] steps = new Expression[8];
            // right operand will always be overwritten, A.DeepCopy will stay there up to step 4
            // This line mainly defines usage of addition or subtraction. Right operands are there to just fill it with something
            BinaryExpression root = plus ? new Addition( A.DeepCopy(), new RealNumber(123456.789) ) : new Subtraction( A.DeepCopy() , new RealNumber(123456.789));

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
            stepZero.rightOperand = new Division( B.ToDouble(), C.DeepCopy() );
            steps[0] = stepZero;

            // >>> 1 <<< 
            BinaryExpression stepOne = root.DeepCopy();
            stepOne.rightOperand = new Division(B.DeepCopy(), C.DeepCopy());
            steps[1] = stepOne;

            // >>> 2 <<< 
            BinaryExpression stepTwo = root.DeepCopy();
            stepTwo.rightOperand = new Multiplication( B.DeepCopy(), new Fraction(Cden, Cnum) );
            steps[2] = stepTwo;

            // >>> 3 <<< 
            BinaryExpression stepThree = root.DeepCopy();
            stepThree.rightOperand = new Fraction( Bnum * (Cden / Bden), Cnum);
            steps[3] = stepThree;

            // >>> 4 <<<
            BinaryExpression stepFour = root.DeepCopy();
            Fraction f4 = new( Bnum * (Cden / Bden), Cnum );
            f4.Reduce();
            stepFour.rightOperand = f4;
            steps[4] = stepFour;

            // >>> 5 <<< 
            // priprava:
            Integer ZTden = f4.Denominator as Integer;
            int ZT_Cden = ZTden.number;
            int LCM = Fraction.EuclidsLCM(Aden, ZT_Cden);
            int levyCinitel = Anum * LCM / Aden;
            int pravyCinitel = Bnum * Cden / Bden * LCM / Cnum; // sick :D
            // priprava done

            BinaryExpression stepFive = root.DeepCopy();
            stepFive.leftOperand = new Fraction( levyCinitel, LCM);
            stepFive.rightOperand = new Fraction( pravyCinitel, LCM);
            steps[5] = stepFive;

            // >>> 6 <<<
            //BinaryExpression cinitel = root.DeepCopy();
            //cinitel.leftOperand = new Integer(levyCinitel);
            //cinitel.rightOperand = new Integer(pravyCinitel);
            int cinitel = root is Addition ? levyCinitel + pravyCinitel : levyCinitel - pravyCinitel;
            Fraction stepSix = new( cinitel, LCM );
            steps[6] = stepSix;

            // >>> 7 <<< 
            Fraction stepSeven = stepSix.DeepCopy();
            Integer sevenI = stepSeven.leftOperand as Integer;
            int numerator = sevenI.number;
            bool negative = numerator < 0;

            if(negative) 
                stepSeven = new Fraction(new Integer(Math.Abs(numerator)), stepSeven.rightOperand);
            stepSeven.Reduce();

            steps[7] = negative ? new Minus(stepSeven) : stepSeven;
            return steps;
        }

        private string[] CreateComments(Fraction A, Fraction B, Fraction C, Expression[] steps) {
            Fraction AB = new("A", "B");
            Fraction CD = new("C", "D");
            Fraction DC = new("D", "C");
            Fraction FF = new("AD", "BC");

            string sloveso = steps[0] is Addition ? "sečti" : "odečti";
            Integer i = C.Denominator as Integer;
            Integer j = B.Denominator as Integer;
            int Cden = i.number;
            int Bden = j.number;

            Fraction inverseC = new(C.Denominator, C.Numerator);
            Fraction tvarVhodneJednicky = new(Cden, Bden);
            Fraction roznasobenaVhodnaJednicka = new(new Multiplication(  Cden / (Cden / Bden), Cden / Bden), B.Denominator);

            BinaryExpression stepThree = steps[3] as BinaryExpression;
            BinaryExpression stepFour = steps[4] as BinaryExpression;
            Fraction leftFour = stepFour.leftOperand as Fraction;
            Fraction rightFour = stepFour.rightOperand as Fraction;

            string[] comments = new string[8] {
                $"Převeď číslo {B.ToDouble()} na reprezentaci zlomkem.",
                $"Přetoč dělení zlomků {B.ToHTML()} : {C.ToHTML()} na jejich násobení.<br>Použij vzoreček: {AB.ToHTML()} : {CD.ToHTML()} = {AB.ToHTML()} ∙ {DC.ToHTML()}",
                $"V součinu {B.ToHTML()} ∙ {inverseC.ToHTML()} Vykrať jedničku ve tvaru {tvarVhodneJednicky.ToHTML()} = {roznasobenaVhodnaJednicka.ToHTML()}.<br>Poté zapiš součin jako jeden zlomek.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {stepThree.rightOperand.ToHTML()} v zakladním tvaru. Pokud ne, převeď ho na něj.",
                $"Najdi nejmenší společný násobek jmenovatelů {leftFour.Denominator.ToHTML()} a {rightFour.Denominator.ToHTML()}. (NSN nebo LCM jako least common multiple)<br>Potom oba zlomky rozšiř na zlomky o tomto základu.",
                $"Spoj oba zlomky do jednoho a {sloveso} je.",
                $"Rozkladem na prvočinitele zkontroluj, jestli je zlomek {steps[6].ToHTML()} v základním tvaru. Pokud ne, převeď ho na něj.",
                "Hotovo. 😎😎"
            };

            return comments;
        }

        public override Excercise GetNext() {
            (Fraction A, Fraction B, Fraction C) = ChooseAnyCSG();
            return GetExactlyThis(A, B, C, CoinFlip());
        } 

        private (Fraction, Fraction, Fraction) ChooseAnyCSG() {
            Fraction A = ChooseAnyFraction(mnozinaA);
            Fraction B = ChooseAnyFraction(mnozinaB);

            Integer denominator = B.Denominator as Integer;
            int den = denominator.number;
            List<Fraction> filtrovanaMnozinaC = new();
            foreach(Fraction f in neomezenaMnozinaC) 
                if (f.Denominator is Integer i && i.number % den == 0)
                    filtrovanaMnozinaC.Add(f);
            Fraction C = ChooseAnyFraction(filtrovanaMnozinaC);

            return (A, B, C);
        }

        private Fraction ChooseAnyFraction(List<Fraction> from) => from[ rand.Next(0, from.Count) ].DeepCopy(); // here the deepcopy is extremely important
        public (Excercise, Excercise) GetTwo() { throw new NotImplementedException(); }
        public Excercise[] GetSix() { throw new NotImplementedException(); }

        public Excercise[] GetEight() {
            Fraction[] acka = new Fraction[8];
            for (int i = 0; i < 8; i++)
                acka[i] = ChooseAnyFraction(mnozinaA);

            Fraction[] becka = new Fraction[8];
            becka[0] = new Fraction(1, 2);
            becka[1] = new Fraction(1, 2);
            becka[2] = new Fraction(1, 4);
            becka[3] = new Fraction(3, 4);

            Fraction[] moznePetiny = new Fraction[4] { new Fraction(1, 5), new Fraction(2, 5), new Fraction(3, 5), new Fraction(4, 5) };
            Fraction[] mozneDesetiny = new Fraction[] { new Fraction(1, 10), new Fraction(3, 10), new Fraction(9, 10) };

            int[] permPetin = GetRandomPermutation(4);
            int[] permDesetin = GetRandomPermutation(3);

            becka[4] = moznePetiny[permPetin[0]];
            becka[5] = moznePetiny[permPetin[1]];

            becka[6] = mozneDesetiny[permDesetin[0]];
            becka[7] = mozneDesetiny[permDesetin[1]];

            int[] shuffleBecka = GetRandomPermutation(8);

            Fraction[] KonecnaBecka = new Fraction[8];
            for (int i = 0; i < 8; i++)
                KonecnaBecka[i] = becka[shuffleBecka[i]];

            Fraction[] cecka = new Fraction[8];

            for(int i = 0; i < 8; i++) {
                Integer Bden = KonecnaBecka[i].Denominator as Integer;
                int BDEN = Bden.number;
                if(BDEN == 2) {
                    cecka[i] = ChooseAnyFraction(mnozinaC_Poloviny);
                } else if(BDEN == 4) {
                    cecka[i] = ChooseAnyFraction(mnozinaC_Ctvrtiny);
                } else if (BDEN == 5) {
                    cecka[i] = ChooseAnyFraction(mnozinaC_Petiny);
                } else if (BDEN == 10) {
                    cecka[i] = ChooseAnyFraction(mnozinaC_Desetiny);
                }
            }

            Excercise[] result = new Excercise[8];
            for (int i = 0; i < 8; i++) {
                result[i] = GetExactlyThis(acka[i], KonecnaBecka[i], cecka[i], CoinFlip());
                // je-li vysledek prilis velky proti omezenim obtiznosti, ponechej becko a vytoc nove A a C a podivej se jestli tentokrat je jiz vse ok
                Integer Bden = KonecnaBecka[i].Denominator as Integer;
                int BDEN = Bden.number;
                while ( ! LevelIsOk(result[i])) {
                    acka[i] = ChooseAnyFraction(mnozinaA);
                    if (BDEN == 2) {
                        cecka[i] = ChooseAnyFraction(mnozinaC_Poloviny);
                    } else if (BDEN == 4) {
                        cecka[i] = ChooseAnyFraction(mnozinaC_Ctvrtiny);
                    } else if (BDEN == 5) {
                        cecka[i] = ChooseAnyFraction(mnozinaC_Petiny);
                    } else if (BDEN == 10) {
                        cecka[i] = ChooseAnyFraction(mnozinaC_Desetiny);
                    }
                    result[i] = GetExactlyThis(acka[i], KonecnaBecka[i], cecka[i], CoinFlip());
                }
            }
            return result;
        }

        private bool LevelIsOk(Excercise e) {
            // this depends on the level and it restrictions
            if (level == Dificulty.CPU)
                return true; // no resetrictions on CPU level.. :) 

            Fraction result = e.Result as Fraction;
            if (e.Result is Minus m) 
                result = m.operand as Fraction;
            
            Integer Inum = result.Numerator as Integer;
            int num = Inum.number;

            Integer Iden = result.Denominator as Integer;
            int den = Iden.number;

            if (level == Dificulty.MENSI)
                return num < 21 && den < 21;
            else if (level == Dificulty.PRIJIMACKY)
                return num < 41 && den < 41;
            else if (level == Dificulty.VETSI)
                return num < 250 && den < 250 && ( 50 < num || 50 < den);
            return num < 10_000 && den < 10_000;
            /*static bool omezeniMensi(int num, int den) => num < 21 && den < 21;
            static bool omezeniPrijimacky(int num, int den) => num < 41 && den < 41;
            static bool omezeniVetsi(int num, int den) => num < 250 && den < 250 && (num < 50 || den < 50);
            static bool omezeniObrovska(int num, int den) => num < 10_000 && den < 10_000;*/
            //Predicate<Fraction> thatAreGreaterThanOne = fun
            //mnozinaA.RemoveAll(thatAreGreaterThanOne);
        }

        public Excercise[] GetAll() {
            List<Excercise> result = new();
            foreach(Fraction A in mnozinaA) 
                foreach(Fraction B in mnozinaB) {
                    Integer denominator = B.Denominator as Integer;
                    int den = denominator.number;
                    List<Fraction> filtrovanaMnozinaC = new();
                    foreach (Fraction f in neomezenaMnozinaC)
                        if (f.Denominator is Integer i && i.number % den == 0)
                            filtrovanaMnozinaC.Add(f);
                    foreach (Fraction C in filtrovanaMnozinaC) 
                        result.Add( GetExactlyThis(A, B, C, CoinFlip() ) );
                }
            return result.ToArray();
        }
    }
}



/* Deprecated generator! s02e01 is its better version */
/*public class EGenerator_Fractions_S02E00 : ExcerciseGenerator {
    /*  [# Possible combinations]
     *  [24]    1. Let a, c, d be different primes from: { 2, 3, 5, 7 }
     *  [6]     2. Let b be not prime from: { 4, 6, 8, 9, 10, 12 }
     *  [1]     3. Let e = c*b
     *  [2]     4. Let operator be either + or -
     *  [1]     5. Form the expression in step 4 as described on handwritten paper
     *  [1]     6. For step 3 leave out variable b
     *  [4]     7. Let f extend right member in the form f/f from: { 2/2, 3/3, 5/5, 7/7 }
     *  [1]     8. Generating variables done. Finnish all steps as described on handwritten paper.
     *  [1]     9. Define all comments 
     *  [1]     10.Define all isolated modifications
     *  [∏] = 24 * 6 * 2 * 4 = 1152 combinations
     *//*

    static readonly int[] primes = new int[4] { 2, 3, 5, 7 };
    static readonly int[] composites = new int[6] { 4, 6, 8, 9, 10, 12 };
    static readonly int[] easyEnoughDenominators = new int[4] { 2, 4, 5, 10 }; // such that 1/item makes easy enough conversion for 9th graders

    public EGenerator_Fractions_S02E00() : base() { }

    public override Excercise GetNext() {
        Expression[] steps = new Expression[7];
        // string[] comments = new string[6];


        int a, b, c, d, e, f;

        // >>> 1 <<<
        int[] randPerm = GetRandomPermutation(3);
        a = primes[randPerm[0]];
        c = primes[randPerm[1]];
        d = primes[randPerm[2]];

        // >>> 2 <<< 
        b = composites[rand.Next(0, 6)];

        // >>> 3 <<< 
        e = c * b;

        // >>> 4 <<<
        bool plus = CoinFlip();

        // >>> generate variable f 
        f = easyEnoughDenominators[rand.Next(0, 4)];

        // >>> step 0
        Fraction s0_left = new(a, c);
        double s0_divLeft = 1.0 / (double)f;
        Fraction s0_divRight = new(e, d * f);

        Division s0_right = new(s0_divLeft, s0_divRight);
        steps[0] = plus ? new Addition(s0_left, s0_right) : new Subtraction(s0_left, s0_right);

        // >>> step 1
        Fraction s1_left = new(a, c);
        Fraction s1_divLeft = new(1, f);
        Fraction s1_divRight = new(e, d * f);

        Division s1_right = new(s1_divLeft, s1_divRight);
        steps[1] = plus ? new Addition(s1_left, s1_right) : new Subtraction(s1_left, s1_right);

        // >>> step 2
        Fraction s2_left = new(a, c);
        Fraction s2_mulLeft = new(1, f);
        Fraction s2_mulRight = new(d * f, e);

        Multiplication s2_right = new(s2_mulLeft, s2_mulRight);
        steps[2] = plus ? new Addition(s2_left, s2_right) : new Subtraction(s2_left, s2_right);

        // >>> step 3
        Fraction s3_left = new(a, c);
        Fraction s3_right = new(d, e);

        steps[3] = plus ? new Addition(s3_left, s3_right) : new Subtraction(s3_left, s3_right);

        // >>> step 4
        Fraction s4_left = new(a * b, c * b);
        Fraction s4_right = new(d, e);

        steps[4] = plus ? new Addition(s4_left, s4_right) : new Subtraction(s4_left, s4_right);

        // >>> step 5
        int numerator = plus ? a * b + d : a * b - d;
        Fraction s5_result = new(numerator, e);
        steps[5] = s5_result;

        // >>> step 6
        Fraction result = s5_result.DeepCopy();
        if (s5_result.NumAndDenAreIntegers())  // they are
            result.Reduce();

        steps[6] = result;

        // Now fill in all comments (popis nasledujici upravy)

        Fraction ac = new(a, c);
        Fraction bb = new(b, b);
        Fraction ff = new(f, f);

        Fraction AB = new("A", "B");
        Fraction CD = new("C", "D");
        Fraction DC = new("D", "C");
        Fraction FF = new("AD", "BC");
        /* Fraction AD = new( "AD", D');
         Fraction BC *//*

        string[] comments = new string[] {
            "Převeď desetinné číslo na zlomek.",
            $"Použij vzoreček na převod dělení dvou zlomků na jejich součin. {AB.ToHTML()}:{CD.ToHTML()} = {AB.ToHTML()}∙{DC.ToHTML()} = {FF.ToHTML()}",
            $"Vyjádři číslo {d*f} jako součin {f} a potom vykrať {ff.ToHTML()}",
            $"Rozšiř {ac.ToHTML()} výrazem {bb.ToHTML()}, aby jsi rozdíl dvou zlomků mohl sloučit do jednoho.",
            "Sluč rozdíl dvou zlomků se stejným jmenovatelem do jednoho a dopočítej výsledek.",
            "Převeď zlomek na jeho základní tvar. Rozlož čitatele i jmenovatele na prvočinitele. Koukni se, jestli můžeme nějaké vykrátit. (Bacha, zlomek již v základním tvaru být může.)",
            "Hotovo 😎😎"
        };
        // ! do html dodat nějak formatování zlomků
        // po otestovani aktulanich veci dodej Expression[][] isolatedModifications
        Expression[][] isolatedModifications = new Expression[2][];
        EFractions_S02E00 nextExcercise = new(steps, comments, isolatedModifications);
        return nextExcercise;
    }
}*/