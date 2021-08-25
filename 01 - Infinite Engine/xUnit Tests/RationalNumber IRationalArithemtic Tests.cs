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

        [Theory]
        [InlineData(13, 33, 79, 189, 1688, 2079)]
        [InlineData(31, 12, 22, 51, 205, 68)]
        [InlineData(45, 23, 30, 71, 3885, 1633)]
        public void LargeNumbers(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a + b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
    }

    public class Subtract {
        [Theory]
        [InlineData(5, 3, 1, 4, 17, 12)]
        [InlineData(7, 9, 13, 11, -40, 99)]
        public void Small(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a - b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
        
        [Theory]
        [InlineData(40, 39, 86, 127, 1726, 4953)]
        [InlineData(47, 29, 93, 157, 4682, 4553)]
        public void LargeNumbers(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a - b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
    }

    public class Multiply {
        [Theory]
        [InlineData(5, 7, 11, 13, 55, 91)]
        [InlineData(1, 2, 20, 3, 10, 3)]
        public void Small(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a * b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
        
        [Theory]
        [InlineData(4, 5, 130, 77, 104, 77)]
        [InlineData(3, 10, 130, 189, 13, 63)]

        public void LargeNumbers(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a * b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
    }

    public class Division {
        [Theory]
        [InlineData( 5, 7, 1, 2, 10, 7)]
        [InlineData( 7, 3, 8, 2, 7, 12)]
        public void Small(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a / b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
        
        [Theory]
        [InlineData( 3, 5, 162, 145, 29, 54)]
        [InlineData( 3, 4, 105, 92, 23, 35)]
        public void LargeNumbers(int Anum, int Aden, int Bnum, int Bden, int Cnum, int Cden) {
            // Arrange 
            Q a = new(Anum, Aden);
            Q b = new(Bnum, Bden);
            int expectedNum = Cnum;
            int expectedDen = Cden;

            // Act
            Q c = a / b;

            // Assert
            int actualNum = c.Num;
            int actualDen = c.Den;
            Assert.Equal(expectedNum, actualNum);
            Assert.Equal(expectedDen, actualDen);
        }
    }
}
