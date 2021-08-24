using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using InfiniteEngine;

/*      Unit tests of ToString method assert that all leafes of the object tree of Expression 
 *  do return the expected string representation. Therefor foreach leafe of the Expression
 *  desecndant there will be test class that will constuct different variations and than 
 *  check that ToString return expected result. 
 *  
 *      Currently tested leafe classes are: 
 *      1. Addition 
 *      2. Multiplication 
 *      3. Subtraction 
 *      4. Division 
 *      5. Minus (Unary expr)
 *      6. Integer
 *      7. RealNumber 
 *      8. Fraction 
 *      9. Variable  
 */

namespace Expression_ToString {
    public class Addition_Tests {
        [Theory]
        [InlineData( 4,  5  )] 
        [InlineData( 50, 100)]
        [InlineData( 0, 500)]
        public void Simple( int b, int c) {
            // Arrange 
            string expected = $"({b}+{c})";
            Addition a = new( b, c );

            // Act
            string actual = a.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RealNumbers() {
            // Arrange 
            string expected = "((2.55+2.66)+(2.77+2.88))";
            Addition a = new( 2.55, 2.66);
            Addition b = new( 2.77, 2.88);
            Addition c = new( a, b);

            // Act
            string actual = c.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsI() {
            // Arrange
            string expected = "(5∙8+(4)/(2))";
            Multiplication m = new(5, 8);
            Fraction f = new(4, 2);
            Addition a = new(m, f);

            // Act
            string actual = a.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsII() {
            // Arrange
            string expected = "(5∙8+4:2)";
            Multiplication m = new(5, 8);
            Division d = new(4, 2);
            Addition a = new(m, d);

            // Act
            string actual = a.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }
    }


    public class Subtraction_Tests {
        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        public void Simple(int a, int b) {
            // Arrange 
            string expected = $"({a}−{b})";
            Subtraction s = new(a, b);

            // Act
            string actual = s.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RealNumbers() {
            // Arrange 
            string expected = "((2.55−2.66)−(2.77−2.88))";
            Subtraction a = new(2.55, 2.66);
            Subtraction b = new(2.77, 2.88);
            Subtraction c = new(a, b);

            // Act
            string actual = c.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsI() {
            // Arrange
            string expected = "(5∙8−(4)/(2))";
            Multiplication m = new(5, 8);
            Fraction f = new(4, 2);
            Subtraction s = new(m, f);

            // Act
            string actual = s.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsII() {
            // Arrange
            string expected = "(5∙8−4:2)";
            Multiplication m = new(5, 8);
            Division d = new(4, 2);
            Subtraction s = new(m, d);

            // Act
            string actual = s.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }
    }

    public class Multiplication_Tests {
        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        public void Simple(int a, int b) {
            // Arrange 
            string expected = $"{a}∙{b}";
            Multiplication m = new(a, b);

            // Act
            string actual = m.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RealNumbers() {
            // Arrange 
            string expected = "2.55∙2.66∙2.77∙2.88";
            Multiplication a = new(2.55, 2.66);
            Multiplication b = new(2.77, 2.88);
            Multiplication c = new(a, b);

            // Act
            string actual = c.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsI() {
            // Arrange
            string expected = "5∙8∙(4)/(2)";
            Multiplication a = new(5, 8);
            Fraction f = new(4, 2);
            Multiplication m = new(a, f);

            // Act
            string actual = m.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsII() {
            // Arrange
            string expected = "5∙8∙4:2";
            Multiplication a = new(5, 8);
            Division d = new(4, 2);
            Multiplication m = new(a, d);

            // Act
            string actual = m.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }
    }

    public class Division_Tests {
        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        public void Simple(int a, int b) {
            // Arrange 
            string expected = $"{a}:{b}";
            Division d = new(a, b);

            // Act
            string actual = d.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RealNumbers() {
            // Arrange 
            string expected = "2.55:2.66:2.77:2.88";
            Division a = new(2.55, 2.66);
            Division b = new(2.77, 2.88);
            Division c = new(a, b);

            // Act
            string actual = c.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsI() {
            // Arrange
            string expected = "5:8:(4)/(2)";
            Division a = new(5, 8);
            Fraction f = new(4, 2);
            Division d = new(a, f);

            // Act
            string actual = d.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsII() {
            // Arrange
            string expected = "(5+8):(4−2)";
            Addition a = new(5, 8);
            Subtraction s = new(4, 2);
            Division d = new(a, s);

            // Act
            string actual = d.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }
    }

    public class Fraction_Tests {
        [Theory]
        [InlineData(4, 5)]
        [InlineData(50, 100)]
        [InlineData(0, 500)]
        public void Simple(int a, int b) {
            // Arrange 
            string expected = $"({a})/({b})";
            Fraction f = new(a, b);

            // Act
            string actual = f.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RealNumbers() {
            // Arrange 
            string expected = "((2.55)/(2.66))/((2.77)/(2.88))";
            Fraction a = new(2.55, 2.66);
            Fraction b = new(2.77, 2.88);
            Fraction c = new(a, b);

            // Act
            string actual = c.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsI() {
            // Arrange
            string expected = "(5∙8)/((4+2))";
            Multiplication m = new(5, 8);
            Addition a= new(4, 2);
            Fraction f = new(m, a);

            // Act
            string actual = f.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OtherExpressionsII() {
            // Arrange
            string expected = "(5∙8)/(4:2)";
            Multiplication m = new(5, 8);
            Division d = new(4, 2);
            Fraction f = new(m, d);

            // Act
            string actual = f.ToString();

            // Assert 
            Assert.Equal(expected, actual);
        }
    }

    public class Integer_Tests {
        [Theory]
        [InlineData(4)]
        [InlineData(50)]
        [InlineData(0)]
        public void Simple(int a) {
            // Arrange 
            string expected = $"{a}";
            Integer i = new(a);

            // Act
            string actual = i.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class RealNumber_Tests {
        [Theory]
        [InlineData(4.123)]
        [InlineData(50.456)]
        [InlineData(0.789)]
        public void Simple(double a) {
            // Arrange 
            string expected = $"{a}";
            RealNumber r = new(a);

            // Act
            string actual = r.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class Variable_Tests {
        [Theory]
        [InlineData(4.123, "x")]
        [InlineData(50.456, "y")]
        [InlineData(0.789, "someLongerVariableName")]
        public void SimpleRealNumbers(double a, string s) {
            // Arrange 
            string expected = $"{a}{s}";
            Variable v = new(a, s);

            // Act
            string actual = v.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, "x")]
        [InlineData(50, "y")]
        [InlineData(0, "someLongerVariableName")]
        public void SimpleIntegers(int a, string s) {
            // Arrange 
            string expected = $"{a}{s}";
            Variable v = new(a, s);

            // Act
            string actual = v.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, "x")]
        [InlineData(50, "y")]
        [InlineData(0, "someLongerVariableName")]
        public void SimpleConstant(int a, string s) {
            // Arrange 
            string expected = $"{a}{s}";
            Integer i = new(a);
            Variable v = new(i, s);

            // Act
            string actual = v.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class Minus_Tests {
        [Theory]
        [InlineData(4)]
        [InlineData(50)]
        [InlineData(0)]
        public void SimpleInt(int a) {
            // Arrange 
            string expected = $"(− {a})";
            Minus m = new(a);

            // Act
            string actual = m.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(4.123)]
        [InlineData(50.456)]
        [InlineData(0.789)]
        public void SimpleReal(double a) {
            // Arrange 
            string expected = $"(− {a})";
            Minus m = new(a);

            // Act
            string actual = m.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4.123)]
        [InlineData(50.456)]
        [InlineData(0.789)]
        public void Expression(double a) {
            // Arrange 
            string expected = $"(− {a})";
            Minus m = new(a);

            // Act
            string actual = m.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

}
