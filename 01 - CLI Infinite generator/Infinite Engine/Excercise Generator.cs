using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {

    public abstract class ExcerciseGenerator {
        protected Random rand;

        public ExcerciseGenerator() {
            rand = new Random();
        }

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

    /* Deprecated generator! s02e01 is its better version */
    public class EGenerator_Fractions_S02E00 : ExcerciseGenerator {
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
         */

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
             Fraction BC */

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
    }

    public class EGenerator_Fractions_S02E01 : ExcerciseGenerator {
        public EGenerator_Fractions_S02E01() : base() {
            FractionsInSimplestForm fsf = new();
            mnozinaB = new() {
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

            mnozinaA = fsf.GetAll(1, 5, mnozinaB);
            static bool fun(Fraction f) => f.ToDouble() > 1;
            Predicate<Fraction> thatAreGreaterThanOne = fun;
            /*mnozinaA.RemoveAll( (Fraction f) => { 
                return f.ToDouble() > 1 ; 
            });*/
            mnozinaA.RemoveAll(thatAreGreaterThanOne);

            neomezenaMnozinaC = fsf.GetAll(10, 30);

            foreach(Fraction possibleC in neomezenaMnozinaC) {
                Integer C = possibleC.Denominator as Integer;
                int Cden = C.number;
                if(Cden % 2 == 0) {
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
        readonly List<Fraction> mnozinaB = new();
        readonly List<Fraction> neomezenaMnozinaC = new();
        readonly List<Fraction> mnozinaC_Poloviny = new();
        readonly List<Fraction> mnozinaC_Ctvrtiny = new();
        readonly List<Fraction> mnozinaC_Petiny = new();
        readonly List<Fraction> mnozinaC_Desetiny = new();

        // this can brake some pedagogic logic, since user can state fractions without any limit, if called directly
        // becasue you can call itt directly but through the Unsafe prefix
        public Excercise UnsafeGetExactlyThis(Fraction A, Fraction B, Fraction C) => GetExactlyThis(A, B, C);
        
        private Excercise GetExactlyThis(Fraction A, Fraction B, Fraction C) {
            Expression[] steps = CreateSteps(A, B, C);
            string[] comments = CreateComments(A, B, C);
            Expression[][] isolatedModifications = new Expression[2][];

            EFractions_S02E01 result = new(steps, comments, isolatedModifications);
            return result;
        }

        private Expression[] CreateSteps(Fraction A, Fraction B, Fraction C) {
            Expression[] steps = new Expression[8];
            bool plus = CoinFlip();
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
            stepSeven.Reduce();
            steps[7] = stepSeven;
            return steps;
        }

        public string[] CreateComments(Fraction A, Fraction B, Fraction C) {
            string[] comments = new string[8] {
                "Převeď B na reprezentaci zlomkem.",
                "Přetoč dělení zlomků B : C na jejich násobení.",
                "V součinu B * (C.den/C.num) Vykrať jedničku ve tvaru C.den/B.den a poté zapiš součin jako jeden zlomek.",
                "Není-li pravý člen v základním tvaru zlomku, převeď ho na něj.",
                "Rozšiř oba zlomky na jmenovatel rovný nejmenšímu společnmu násobku jejich jmenovatelů.",
                "Spoj oba zlomky do jednoho a sečti / odečti je.",
                "Není-li výsledek v základním tvaru zlomku, převeď ho na něj.",
                "Hotovo. 😎😎"
            };
            return comments;
        }

        public override Excercise GetNext() {
            (Fraction A, Fraction B, Fraction C) = ChooseAnyCSG();
            return GetExactlyThis(A, B, C);
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

        public (Excercise, Excercise) GetTwo() {
            throw new NotImplementedException();
        }

        public Excercise[] GetSix() {
            throw new NotImplementedException();
        }

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
            for (int i = 0; i < 8; i++)
                result[i] = GetExactlyThis(acka[i], KonecnaBecka[i], cecka[i]);
            return result;
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
                        result.Add( GetExactlyThis(A, B, C) );
                }
            return result.ToArray();
        }
    }
}
