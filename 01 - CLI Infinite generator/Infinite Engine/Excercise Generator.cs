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

            for(int i = 0; i < count; i++) 
                result[i] = i;
            
            for(int i = 0; i < count; i++) {
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

    public class EGenerator_Fractions_S01E01 : ExcerciseGenerator {
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

        public EGenerator_Fractions_S01E01(): base() { }

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
            Fraction s0_divRight = new(e, d*f);

            Division s0_right = new(s0_divLeft, s0_divRight);
            steps[0] = plus ? new Addition(s0_left, s0_right) : new Subtraction(s0_left, s0_right);

            // >>> step 1
            Fraction s1_left = new(a, c);
            Fraction s1_divLeft = new(1, f);
            Fraction s1_divRight = new(e, d*f);
            
            Division s1_right = new( s1_divLeft, s1_divRight);
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
            Fraction s4_left = new( a*b, c*b );
            Fraction s4_right = new(d, e);

            steps[4] = plus ? new Addition(s4_left, s4_right) : new Subtraction(s4_left, s4_right);

            // >>> step 5
            int numerator = plus ? a * b + d : a * b - d;
            Fraction s5_result = new(numerator , e);
            steps[5] = s5_result;

            // >>> step 6
            Fraction result = s5_result.DeepCopy();
            if(s5_result.NumAndDenAreIntegers())  // they are
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

            EFractions_S01E01 nextExcercise = new( steps, comments, isolatedModifications);
            return nextExcercise;
            /*
            // >>> 5 <<<
            Fraction left_04 = new ( a*b, e);
            Fraction right_04 = new ( d, e);
            steps[4] = plus ? new Addition(left_04, right_04) : new Subtraction(left_04, right_04);

            // >>> 6 <<<
            Fraction left_03 = new(a, c);
            Fraction right_03 = right_04.DeepCopy();
            steps[3] = plus ? new Addition(left_03, right_03) : new Subtraction(left_03, right_03);

            // >>> 7 <<< 
            Fraction left_02 = left_03.DeepCopy();

            int extendWith = primes[rand.Next(0, 4)];
            Fraction mLeft = new(1, extendWith);
            Fraction mRight = new(extendWith * d, e);
            Multiplication right_02 = new(mLeft, mRight);
            steps[2] = plus ? new Addition(left_02, right_02) : new Subtraction(left_02, right_02);

            // >>> 8 <<< 
            /*      a. define steps[1] : swap right multiplication to be division 
             *      b. define steps[0] : swap fraction to be real number
             *      c. define steps[5] : from steps[4]
             */
            /*
            // a: 

            Fraction left_01 = left_02.DeepCopy();

            Fraction dLeft = mLeft.DeepCopy();
            Expression denominator = (mRight.Denominator).DeepCopy();
            Expression numerator = (mRight.Numerator).DeepCopy();
            Fraction dRight = new Fraction(denominator, numerator);
            Division right_01 = new Division(dLeft, dRight);
            steps[1] = plus ? new Addition(left_01, right_01) : new Subtraction(left_01, right_01);
            // b:
            // >>> 9 <<<
            */

        }
    }
}
