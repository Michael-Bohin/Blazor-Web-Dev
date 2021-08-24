using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteEngine;
using Xunit;

/// interface IRationalArithmetic
/// 1.  Q Add(Q addend);
/// 2.  Q Subtract(Q subtrahend);
/// 3.  Q Multiply(Q multiplier);
/// 4.  Q Divide(Q divisor);
/// 5.  op_Addition
/// 6.  op_UnaryPlus
/// 7.  op_Subtraction
/// 8.  op_UnaryMinus
/// 9.  op_Multiplication
/// 10. op_UnaryMultiplciation
/// 11. op_Division
/// 12. op_UnaryDivision

namespace RationNumber_IRationalArithemtic {
    using Q = RationalNumber;
    public class Add {
        [Theory]
        [InlineData(5, 6, 2, 6)]
        [InlineData(1, 3, 1, 3)]
        public void SameDenominator(int Anum, int Aden, int Bnum, int Bden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Anum + Bnum;
            int expectedDen = Aden;

            // Act
            Q c = a + b;

            // Assert
            int actualCNum = c.Num;
            int actualCDen = c.Den;
            Assert.Equal(expectedNum, actualCNum);
            Assert.Equal(expectedDen, actualCDen);
        }

        [Theory]
        [InlineData(11, 41, 13, 2)]
        [InlineData(3, 4, 2, 11)]
        public void DifferentDenominator(int Anum, int Aden, int Bnum, int Bden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Anum * Bden + Aden * Bnum;
            int expectedDen = Aden * Bden;

            // Act
            Q c = a + b;

            // Assert
            int actualCNum = c.Num;
            int actualCDen = c.Den;
            Assert.Equal(expectedNum, actualCNum);
            Assert.Equal(expectedDen, actualCDen);
        }
    }
}
