using System;
using Xunit;
using InfiniteEngine;

/*  DeepCopies are great shelters for hidden bugs. 
 *  Foreach leafe class of object tree
 *  Construct identical expected and tested expression. (different instances obviously)
 *  Let tested expression create deepcopy. 
 *  Test that: 
 *  1. Deepcopy and tested expression return same string.
 *  2. After modificationA of tested, both return expected string. 
 *      (tested returns modifiedA, deepcopy stays unchanged)
 *  3. After modificationB of deepcopy, both return expected string. 
 *      (tested returns modifiedA, deepcopy return modifiedB)
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

namespace Expression_DeepCopy { 
    public class SimpleTrees {
        [Fact]
        public void Addition() {
            // Arrange
            BinaryExpression add = new Addition( 5, 99);
            BinaryExpression copy = add.DeepCopy();

            Multiplication changeA = new(2, 3);
            Division changeB = new( 4, 5);

            string expectedOriginal = "(5+99)";
            string expectedModificationA = "(2*3+99)";
            string expectedModificationB = "(5+4:5)";

            // Act 
            string add1 = add.ToString();
            string copy1 = copy.ToString();

            // change add using multiplication:
            add.leftOperand = changeA;
            string add2 = add.ToString();
            string copy2 = copy.ToString();

            // change copy using division:
            copy.rightOperand = changeB;
            string add3 = add.ToString();
            string copy3 = copy.ToString();

            // Assert
            Assert.Equal(expectedOriginal, add1);
            Assert.Equal(expectedOriginal, copy1);

            Assert.Equal(expectedModificationA, add2);
            Assert.Equal(expectedOriginal, copy2);

            Assert.Equal(expectedModificationA, add3);
            Assert.Equal(expectedModificationB, copy3);
        }

        [Fact]
        public void Fraction() {
            // Arrange
            BinaryExpression fraction = new Fraction(5, 99);
            BinaryExpression copy = fraction.DeepCopy();

            Multiplication changeA = new(2, 3);
            Division changeB = new(4, 5);

            string expectedOriginal = "(5)/(99)";
            string expectedModificationA = "(2*3)/(99)";
            string expectedModificationB = "(5)/(4:5)";

            // Act 
            string add1 = fraction.ToString();
            string copy1 = copy.ToString();

            // change add using multiplication:
            fraction.leftOperand = changeA;
            string add2 = fraction.ToString();
            string copy2 = copy.ToString();

            // change copy using division:
            copy.rightOperand = changeB;
            string add3 = fraction.ToString();
            string copy3 = copy.ToString();

            // Assert
            Assert.Equal(expectedOriginal, add1);
            Assert.Equal(expectedOriginal, copy1);

            Assert.Equal(expectedModificationA, add2);
            Assert.Equal(expectedOriginal, copy2);

            Assert.Equal(expectedModificationA, add3);
            Assert.Equal(expectedModificationB, copy3);
        }
    }
    public class ComplexTree {
        private BinaryExpression GetComplexTree() {
            Multiplication m = new(2, 3);
            Variable x = new(4, "x");
            Division d = new(m, x);

            Minus min = new(5.55);
            RealNumber i = new(3.811);
            Addition a = new(min, i);

            Variable y = new(5, "y");
            Subtraction s = new(a, y);

            Fraction result = new(d, s);
            return result;
        }

        [Fact]
        public void MadeExpressionThatIActuallyIntended() {
            // Arrange
            string expected = "(2*3:4x)/((((-5.55)+3.811)-5y))";
            Expression e = GetComplexTree();

            // Act
            string actual = e.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModifyDeepCopies() {
            // Arrange 
            string expectedOriginal = "(2*3:4x)/((((-5.55)+3.811)-5y))";
            BinaryExpression original = GetComplexTree();
            BinaryExpression modFraction = original.DeepCopy();
            BinaryExpression modDivision = original.DeepCopy();
            BinaryExpression modMultiplication = modFraction.DeepCopy();
            BinaryExpression modVariableX = modMultiplication.DeepCopy();
            BinaryExpression modSubtraction = modDivision.DeepCopy();
            BinaryExpression modAddition = modVariableX.DeepCopy();
            BinaryExpression modMinus = original.DeepCopy();
            BinaryExpression modVariableY = modMinus.DeepCopy();

            string actualOriginal1 = $"{original}";
            string actualOriginal2 = $"{modFraction}";
            string actualOriginal3= $"{modDivision}";
            string actualOriginal4 = $"{modMultiplication}";
            string actualOriginal5 = $"{modVariableX}";
            string actualOriginal6 = $"{modSubtraction}";
            string actualOriginal7 = $"{modAddition}";
            string actualOriginal8 = $"{modMinus}";
            string actualOriginal9 = $"{modVariableY}";

            // "(2*3:4x)/((((-5.55)+3.811)-5y))";
            string expectedFraction = "((2*3:4x)/((((-5.55)+3.811)-5y)))/((2*3:4x)/((((-5.55)+3.811)-5y)))";
            string expectedDivison = "((1+1))/((((-5.55)+3.811)-5y))";
            string expectedMultiplication = "(4*5:4x)/((((-5.55)+3.811)-5y))";
            string expectedVariableX = "(2*3:50promenna)/((((-5.55)+3.811)-5y))";
            string expectedSubtraction = "(2*3:4x)/((8-5y))";
            string expectedAddition = "(2*3:4x)/(((9.999+3.811)-5y))";
            string expectedMinus = "(2*3:4x)/((((-1000)+3.811)-5y))";
            string expectedVariableY = "(2*3:4x)/((((-5.55)+3.811)-9*9))";

            // Act 
            Expression left = original.DeepCopy();
            Expression right = original.DeepCopy();
            modFraction = new Fraction(left, right);
           
            modDivision.leftOperand = new Addition(1, 1);
            
            Multiplication m = new(4, 5);
            Variable x = new(4, "x");
            Division d = new( m, x);
            modMultiplication.leftOperand = d;

            Multiplication m2 = new(2, 3);
            Variable promenna = new(50, "promenna");
            Division d2 = new( m2, promenna);
            modVariableX.leftOperand = d2;

            BinaryExpression be5 = modSubtraction.rightOperand as BinaryExpression;
            be5.leftOperand = new Integer(8);

            BinaryExpression be = modAddition.rightOperand as BinaryExpression;
            BinaryExpression be6 = be.leftOperand as BinaryExpression;
            be6.leftOperand = new RealNumber(9.999);

            BinaryExpression be2 = modMinus.rightOperand as BinaryExpression;
            BinaryExpression be3 = be2.leftOperand as BinaryExpression;
            be3.leftOperand = new Minus(1000);

            BinaryExpression be4 = modVariableY.rightOperand as BinaryExpression;
            be4.rightOperand = new Multiplication(9, 9);

            string actualOriginal1AfterModifications = $"{original}";
            string actualFraction = $"{modFraction}";
            string actualDivision = $"{modDivision}";
            string actualMultiplication = $"{modMultiplication}";
            string actualVariableX = $"{modVariableX}";
            string actualSubtraction = $"{modSubtraction}";
            string actualAddition = $"{modAddition}";
            string actualMinus = $"{modMinus}";
            string actualVariableY = $"{modVariableY}";

            // Assert (first initial states after deepcopies were created, than their representation after modifications)
            Assert.Equal(expectedOriginal, actualOriginal1);
            Assert.Equal(expectedOriginal, actualOriginal2);
            Assert.Equal(expectedOriginal, actualOriginal3);

            Assert.Equal(expectedOriginal, actualOriginal4);
            Assert.Equal(expectedOriginal, actualOriginal5);
            Assert.Equal(expectedOriginal, actualOriginal6);

            Assert.Equal(expectedOriginal, actualOriginal7);
            Assert.Equal(expectedOriginal, actualOriginal8);
            Assert.Equal(expectedOriginal, actualOriginal9);

            // after modifications:
            Assert.Equal(expectedOriginal, actualOriginal1AfterModifications);
            Assert.Equal(expectedFraction, actualFraction);
            Assert.Equal(expectedDivison, actualDivision);

            Assert.Equal(expectedMultiplication, actualMultiplication);
            Assert.Equal(expectedVariableX, actualVariableX);
            Assert.Equal(expectedSubtraction, actualSubtraction);

            Assert.Equal(expectedAddition, actualAddition);
            Assert.Equal(expectedMinus, actualMinus);
            Assert.Equal(expectedVariableY, actualVariableY);
        }
        // Arrange
        // Act
        // Assert
    }
}
