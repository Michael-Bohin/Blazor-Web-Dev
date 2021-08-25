using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteEngine;
using Xunit;

namespace RationalNumber_IRationalNumber {
    /// 
    /// class RationalNumber tests 
    /// 
    /// interface IRationalNumber
    /// 
    /// 1.  double ToDouble();
    /// 2.  bool IsSimplestForm();
    /// 3.  bool IsInteger();
    /// 4.  void Reduce();
    /// 5.  Q GetSimplestForm();
    /// 6.  void Expand(int i); // throws InvalidOperationException if i == 0
    /// 7.  Q GetExpandedForm(int i);
    /// 8.  void Inverse();  // throws MathHellException, if numerator is 0
    /// 9.  Q GetInverse();
    /// 10. bool IsEquivalent(Q other); // comparison based on comparing simplest forms of two rational numbers
    /// 11. int[] NumeratorPrimeFactors();
    /// 12. int[] DenominatorPrimeFactors();
    /// 
    /// 
    /// 
    /// in future files:
    /// interface IEquatable<RationalNumber>
    /// 
    /// 1.  bool Equals(Q other);
    /// 2.  override bool Equals(Object obj)
    /// 3.  override int GetHashCode();
    /// 4.  static bool operator ==(Q q1, Q q2);
    /// 5.  static bool opeartor !=(Q q1, Q q2);  
    /// 
    /// interface IComparable<RationalNumber>
    /// 
    /// 1.  bool CompareTo(Q other);
    /// 2.  op_GreaterThan
    /// 3.  op_GreaterThanOrEqual
    /// 4.  op_LessThan
    /// 5.  op_LessThanOrEqual
    using Q = RationalNumber;

    public class Getter_Setter_Tests {
        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        [InlineData(-5, -10)]
        [InlineData(-1, 5)]
        [InlineData(38, -99)]
        public void SimpleGet(int num, int den) {
            // Arrange 
            int expectedNum = num;
            int expectedDen = den;
            Q a = new(num, den);

            // Act
            int actualNum = a.Num;
            int actualDen = a.Den;

            // Assert
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }

        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        [InlineData(-5, -10)]
        [InlineData(-1, 5)]
        [InlineData(38, -99)]
        public void SimpleSet(int num, int den) {
            // Arrange 
            int expectedNum = num;
            int expectedDen = den;
            // setting the value in constructor intentionally to be different than the value passed to setter later
            Q a = new(num * den, den - num);

            // Act
            a.Num = num;
            a.Den = den;
            int actualNum = a.Num;
            int actualDen = a.Den;

            // Assert
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }

        [Fact]
        public void SimplestGet() {
            // Arrange 
            const int num = 55;
            const int den = 66;
            int expectedNum = num;
            int expectedDen = den;
            Q a = new(num, den);

            // Act
            int actualNum = a.Num;
            int actualDen = a.Den;

            // Assert
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }

        [Fact]
        public void SimplestSet() {
            // Arrange 
            const int num = 55;
            const int den = 66;
            int expectedNum = num;
            int expectedDen = den;
            // setting the value in constructor intentionally to be different than the value passed to setter later
            Q a = new(num * den, den - num); 

            // Act
            a.Num = num;
            a.Den = den;
            int actualNum = a.Num;
            int actualDen = a.Den;

            // Assert
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
    }

    public class IsSimplestForm {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 11)]
        [InlineData(-37, 371)]
        [InlineData(-89, 1)]
        [InlineData(1, 1)]
        [InlineData(0, 1)]
        [InlineData(3, 5)]
        [InlineData(7, 11)]
        [InlineData(13, 17)]
        public void IsSimplest(int num, int den) {
            // Arrange 
            Q a = new(num, den);
            bool expected = true;

            // Act
            bool actual = a.IsSimplestForm();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-200, -50)]
        [InlineData(1000, 100)]
        [InlineData(37, 370)]
        [InlineData(-1, -1)]
        [InlineData(1, -1)]
        [InlineData(20, 5)]
        [InlineData(15, 5)]
        [InlineData(110, 11)]
        [InlineData(13, -17)]
        [InlineData(1, -2)]
        [InlineData(0, -2)]
        [InlineData(0, 123456)]
        [InlineData(0, 10)]
        public void IsComposite(int num, int den) {
            // Arrange 
            Q a = new(num, den);
            bool expected = false;

            // Act
            bool actual = a.IsSimplestForm();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class IsInteger {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(-1, 1)]
        [InlineData(-1, -1)]
        [InlineData(1, -1)]
        [InlineData(50, 5)]
        [InlineData(-50, 5)]
        [InlineData(50, -5)]
        [InlineData(-50, -5)]
        [InlineData(1000, 20)]
        [InlineData(-5, 1)]
        [InlineData(-35, 7)]
        [InlineData(-500, -25)]
        [InlineData(-900, 9)]
        public void Integer(int num, int den) {
            // Arrange 
            Q a = new(num, den);
            bool expected = true;

            // Act
            bool actual = a.IsInteger();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(9, 4)]
        [InlineData(10, 100)]
        [InlineData(3, 370)]
        [InlineData(9, 2)]
        [InlineData(2, 9)]
        [InlineData(22, 5)]
        [InlineData(16, 5)]
        [InlineData(115, 11)]
        [InlineData(13, -17)]
        [InlineData(1, -2)]
        [InlineData(1, 123456)]
        [InlineData(1, 10)]
        public void NotInteger(int num, int den) {
            // Arrange 
            Q a = new(num, den);
            bool expected = false;

            // Act
            bool actual = a.IsInteger();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
