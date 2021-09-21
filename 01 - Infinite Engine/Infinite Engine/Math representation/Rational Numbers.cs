using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using M = MathAlgorithms;
    using Q = RationalNumber;
    // using EQ = Educational_RationalNumber;
    // implement N, Z, and R in future! :) 

    /// I can see several approaches to doing Rational arithemtic 
    /// Beacuse of kids, all will need to have its reprezentation in code 
    /// For now, the one leading to the result in reduced form will be in place. 
    /// Different approaches: (Addition and subtraction)
    /// A: ( most steps, but guaranteed smallest product of multiplication )
    ///     1. Reduce operands 
    ///     2. Find the least common multiple of denominators 
    ///     3. Expand operands to LCM denominator 
    ///     4. Add/Subtract numerators after joining the Ration numbers 
    ///     5. Reduce result to simplest form 
    /// 
    /// B: 
    ///     1. Use equation: (ad+-cb)/bd and dont reduce anything
    ///     
    /// C:  
    ///     1. B + reduce operands before and result after 
    ///     
    /// D:  1. Find the least common multiple of denominators 
    ///     2. Expand operands to LCM denominator 
    ///     3. Add/Subtract numerators after joining the Ration numbers 
    ///     4. don't reduce anything 

    /// IComparable<RationalNumber>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=net-5.0
    /// 
    /// defines: 
    /// bool CompareTo(Q other); -> negative: this < other, zero: this == equal, positive: this > other. 
    /// op_GreaterThan, op_GreaterThanOrEqual
    /// op_LessThan, op_LessThanOrEqual
    /// 
    /// IEquatable<RationalNumber>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1?view=net-5.0
    /// 
    /// define: 
    /// bool Equals(Q other);
    /// override bool Equals(Object obj)
    /// override int GetHashCode();
    /// static bool operator ==(Q q1, Q q2);
    /// static bool opeartor !=(Q q1, Q q2);
    ///
    /// math note:
    /// In this code I will consider two different expansions of same simplest form different. 
    /// bool IsEquivalent() covers the other comparison semantic where 2/4 is equal to 1/2

    /// RationalNumber:
    // Fraction with the constraint of holding some integer as numerator and denominator
    // Denominator can not be zero. Therefore, it is the set Q. 
    // With usefull methods, that are frequently used in primary school math. 
    // Because it is Q, unlike Fractions (which can hold nested infix expression),
    // it inherits from value as real number or integer does.
    // Simplest form definition:
    // 1. denominator is from natural numbers
    // 2. the only divisor of num and den is 1.

    interface IRationalNumber {
        double ToDouble();

        bool IsSimplestForm();
        bool IsInteger();

        void Reduce();
        Q GetSimplestForm();

        void Expand(int i); // expand the rational number, throws InvalidOperationException if i == 0
        Q GetExpandedForm(int i);

        Q Copy();

        void Inverse();  // throws MathHellException, if numerator is 0
        Q GetInverse();

        bool IsEquivalent(Q other); // comparison based on comparing simplest forms of two rational numbers

        int[] NumeratorPrimeFactors();
        int[] DenominatorPrimeFactors();
    }

    interface IRationalArithmetic {
        Q Add(Q addend);
        Q Subtract(Q subtrahend);
        Q Multiply(Q multiplier);
        Q Divide(Q divisor);
	}

	interface ITeacherAtomicOperations
	{
		// To be implemented! First need to carefully think through what are all the use cases. 
	}

	public class RationalNumber : Value, IRationalNumber, IRationalArithmetic, 
        IEquatable<RationalNumber>, IComparable<RationalNumber>
        /// IFormattable : Implement in future, if sending it to different view like HTML, WinForms, Unity etc. requires significantly different formatting
        /// IConvertible : Maybe implement in future 
        {
        int _num;
        public int Num { get => _num; set => _num = value; }

        int _den;
        public int Den {
            get => _den;
            set {
                if (value == 0)
                    throw new MathHellException(mathHellAlert);
                _den = value;
            }
        }
        const string mathHellAlert = "Math hell alert! 😈🙀 Rational number can not have denominator equal to zero.";

        public RationalNumber(int numerator, int denominator) {
            if(denominator == 0)
                throw new MathHellException(mathHellAlert);
            _num = numerator;
            _den = denominator;
        }

        public RationalNumber(int integer)
        {
            _num = integer;
            _den = 1;
        }

        /// inherited abstract methods overrides ///
        //public override string ToString() => $"{_num} / {_den}";
        public override string ToString() => ToHTML();
        public override Q DeepCopy() => (Q)MemberwiseClone();
        public override string ToHTML() => @"<div class=""frac""><span>" + _num.ToString() + @"</span><span class=""symbol"">/</span><span class=""bottom"">" + _den.ToString() + @"</span></div>";

        /// interface IRationalNumber ///
        public double ToDouble() => (double)_num / (double)_den;

        public bool IsSimplestForm() => !(_den < 0) && M.EuclidsGCD(Math.Abs(_num), _den) == 1;

        public bool IsInteger() => Math.Abs(_num) % Math.Abs(_den) == 0;
        
        public void Reduce() {
            if (_den < 0) { // if denominator is negative , multiply both num and den by -1
                _den = Math.Abs(_den);
                _num *= -1;
            }
                
            if (IsSimplestForm())
                return;
            int GCD = M.EuclidsGCD(_num, _den);
            _num /= GCD;
            _den /= GCD;
        }

        public Q GetSimplestForm() {
            Q other = new(_num, _den);
            other.Reduce();
            return other;
        }

        const string invalidExpandException = "Expanding rational number using 0, not only doesn't preserve same simplest form, but also leads to Math Hell Exception!";
        public void Expand(int i) {
            if (i == 0)
                throw new InvalidOperationException(invalidExpandException);
            _num *= i;
            _den *= i;
        }

        public Q GetExpandedForm(int i) {
            if (i == 0)
                throw new InvalidOperationException(invalidExpandException);
            return new(_num * i, _den * i);
        }

        public Q Copy() => new(_num, _den);

        public void Inverse() {
            if (_num == 0)
                throw new InvalidOperationException(invalidExpandException);
            int temp = _num;
            _num = _den;
            _den = temp;
        }

        public Q GetInverse() {
            if (_num == 0)
                throw new MathHellException(mathHellAlert);
            Q other = Copy();
            other.Inverse();
            return other;
        }

        public bool IsEquivalent(Q other) {
            Q a = Copy();
            Q b = other.Copy();
            a.Reduce();
            b.Reduce();
            return a.Num == b.Num && a.Den == b.Den;
        }

        // ! treats numerator as absolute value
        public int[] NumeratorPrimeFactors() => GetPrimeFactors(_num);
        public int[] DenominatorPrimeFactors() => GetPrimeFactors(_den);

        static int[] GetPrimeFactors(int number) {
            List<int> primeFactors = new();
            if (number == 0)
                return primeFactors.ToArray();

            number = Math.Abs(number);

            if (number == 1) {
                primeFactors.Add(1);
                return primeFactors.ToArray();
            }

            int divisor = 2;
            while (number > 1) {
                if (number % divisor == 0) {
                    primeFactors.Add(divisor);
                    number /= divisor;
                } else {
                    divisor++; // can be sped up by only using primes
                }
            }
            return primeFactors.ToArray();
        }

        public Fraction GetPrimeFactorization() {
            int[] numPF = NumeratorPrimeFactors();
            int[] denPF = DenominatorPrimeFactors();
            Expression num = Fraction.CreatePrimeProduct(new List<int>(numPF));
            Expression den = Fraction.CreatePrimeProduct(new List<int>(denPF));
            return new Fraction(num, den);
        }

        /// interface IRationalArithmetic ///

        public Q Add(Q addend) {
            // a/b + c/d = (ad+bc)/bd -> can be shortcuted using finding LCM of simplest forms 
            (Q a, Q b) = PrepareAddSub(addend);
            a.Num += b.Num;
            a.Reduce();
            return a;
        }

        (Q a, Q b) PrepareAddSub(Q rightOperand) {
            Q a = GetSimplestForm();
            Q b = rightOperand.GetSimplestForm();

            int LCM = M.EuclidsLCM(a.Den, b.Den);
            if(LCM != a.Den)
                a.Expand(LCM/ a.Den);

            if (LCM != b.Den)
                b.Expand(LCM /b.Den);

            return (a, b);
        }

        public Q Subtract(Q subtrahend) {
            (Q a, Q b) = PrepareAddSub(subtrahend);
            a.Num -= b.Num;
            a.Reduce();
            return a;
        }

        public Q Multiply(Q multiplier) {
            Q a = GetSimplestForm();
            Q b = multiplier.GetSimplestForm();

            a.Num *= b.Num;
            a.Den *= b.Den;
            a.Reduce();
            return a;
        }

        public Q Divide(Q divisor) {
            if (divisor.Num == 0)
                throw new MathHellException(mathHellAlert);

            return Multiply( divisor.GetInverse() );
        }

        public static Q operator -(Q a) => new (-a._num, a._den);
        public static Q operator +(Q a, Q b) => a.Add(b);
        public static Q operator -(Q a, Q b) => a.Subtract(b);
        public static Q operator *(Q a, Q b) => a.Multiply(b);
        public static Q operator /(Q a, Q b) {
            if (b.Num == 0) 
                throw new DivideByZeroException();
            return a.Divide(b);
        }

        /// interface IEquatable<RationalNumber> ///

        // public bool Equals(Q other) => other != null && _num == other.Num && _den == other.Den;
        
        // public override bool Equals(Object obj) => obj != null && obj is Q rationalObj && Equals(rationalObj);

        // avoid inverse fractions returning same product 
        public override int GetHashCode() => _num.GetHashCode() * _den.GetHashCode() + _num.GetHashCode();

        public bool Equals(Q other) {
            if (other == null)
                return false;

            return _num == other.Num && _den == other.Den;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            Q fractionObj = obj as Q;
            if (fractionObj == null)
                return false;
            else
                return Equals(fractionObj);
        }

        public static bool operator ==(Q person1, Q person2) {
            if (((object)person1) == null || ((object)person2) == null)
                return Object.Equals(person1, person2);

            return person1.Equals(person2);
        }

        public static bool operator !=(Q person1, Q person2) {
            if (((object)person1) == null || ((object)person2) == null)
                return !Object.Equals(person1, person2);

            return !(person1.Equals(person2));
        }

        /// interface IComparable<RationalNumber> /// 

        public int CompareTo(Q other) {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // reduce both to simplest form 
            // find least common multiple of both denominators
            // extend both fraction to it 
            // compare only numerators :) 
            Q a = GetSimplestForm();
            Q b = other.GetSimplestForm();
            int LCM = M.EuclidsLCM(a.Den, b.Den);
            if (LCM != a.Den)
                a.Expand(LCM / a.Den);
            if (LCM != b.Den)
                b.Expand(LCM / b.Den);

            return a.Num.CompareTo(b.Num);
        }

        public static bool operator >(Q operand1, Q operand2) => operand1.CompareTo(operand2) > 0;
        public static bool operator <(Q operand1, Q operand2) => operand1.CompareTo(operand2) < 0;
        public static bool operator >=(Q operand1, Q operand2) => operand1.CompareTo(operand2) >= 0;
        public static bool operator <=(Q operand1, Q operand2) => operand1.CompareTo(operand2) <= 0;

		/* ! To be properly implemented ! -> also think out all possible usages -> from teaching point of view, what are all the atomic opeartions? 
		 * First define interface: ITeacher_RationalAtomicOperations
		 * public void Operate(Op op, Q rightOperand) {
			if(op == Op.Mul || op == Op.Div)
				throw new NotImplementedException();

			if( _den != rightOperand.Den) {
				// expand to same denominator 

			}
		}*/
    }
    /*/
    // Educational child of Q class will consist of same 
    // opeartions as its parent and foreach of these methods 
    // will also consist of educational method which will do the 
    // same operation and on top will return "kucharka reseni"
    // which will exhaustively describe each step

    interface IEducational_RationalNumber { }
    interface IEducational_RationalArithmetic { }
    class Educational_RationalNumber : RationalNumber { }
    /**/
}

/// https://docs.microsoft.com/en-us/dotnet/api/system.iformattable?view=net-5.0
/// Study IFormattable in depth and if usefull implement it in future
///
/// https://github.com/microsoft/referencesource/blob/master/mscorlib/system/double.cs
/// IArithmetic<Double> template at the end 