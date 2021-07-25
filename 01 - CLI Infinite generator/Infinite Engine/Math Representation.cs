using System;
using System.Collections.Generic;

namespace InfiniteEngine {

    /*     Intention: 
     *     Comments should contain clear explanation of each step of the solution to children
     * therefore, there will always be StepsCount - 1 comments, unless there is a bug or 
     * excercise creator is probably lazy and not responsible.
     *     Each comment should also clearly mention the part of expression that changed - 
     * isolate the change visually and discard not changed part of the expression. 
     *     For web and unity views, it will be important to develop interactive and visual 
     * explanations.
     */
    public abstract class Excercise { 
        public Expression Problem { get => steps[0]; }
        public int StepsCount { get => steps.Count; }
        public Expression Result { get => steps[^1]; }

        public List<Expression> steps;

        public List<string> comments;
    }

    public abstract class Expression {
        public override abstract string ToString(); // override object.ToString so that compiler forces own implementation 
        public abstract Expression DeepCopy();
    }
    public class Person { }
    public abstract class BinaryExpression : Expression {
        public Expression leftOperand;
        public Expression rightOperand;
        protected abstract string SignRepresentation { get; }
        public override string ToString() => $"{leftOperand}{SignRepresentation}{rightOperand}";
        public override BinaryExpression DeepCopy() {
            BinaryExpression other = (BinaryExpression)this.MemberwiseClone();
            other.leftOperand = leftOperand.DeepCopy();
            other.rightOperand = rightOperand.DeepCopy();
            return other;
        }
    }
    public abstract class UnaryExpression : Expression {
        public Expression operand;
        protected abstract string SignRepresentation { get; }
        public override string ToString() => $"{SignRepresentation}{operand}";

        public override UnaryExpression DeepCopy() {
            UnaryExpression other = (UnaryExpression)this.MemberwiseClone();
            other.operand = operand.DeepCopy();
            return other;
        }
    }

    public abstract class Value : Expression { }

    public abstract class Constant : Value {
        public ValueType number;
    }

    public class Addition : BinaryExpression {
        protected override string SignRepresentation => "+";
    }

    public class Subtraction : BinaryExpression {
        protected override string SignRepresentation => "-";
    }

    public class Multiplication : BinaryExpression {
        protected override string SignRepresentation => "-";
    }

    public class Division : BinaryExpression {
        protected override string SignRepresentation => "-";
    }

    /*     Yes, Division and Fraction as part of math is the same thing and so if it only 
     * depended on math only one class would be required. 
     * 
     *     However in the field of mathematics use of Fractions is so central, that specific 
     * definitions set, invented by humans, has evolved that means humans use two names for division
     * Divison and Fraction. Division in Czech textbooks has two different operators: '/' and ':'. 
     * '/' means Fraction and ':' means Division. Both of them are the same arithmetic operation. 
     * 
     *     I will define both classes in math representation too. My point is to improve peoples'
     * user experience learning math. This comes at the cost of writing repetitive code: arithemtic
     * operation division will have two different classes that are describing it Fraction and Division.
     * 
     *     Question/Observation: Wouldn't it be easier for kids to understand the concept if the naming 
     * duality would be removed? 
     */

    public class Fraction : BinaryExpression {
        public Expression numerator;
        public Expression denominator;
        public new Expression rightOperand { get => numerator; } // c s tim naming rule violation => vskutku predtim to byl field ale na redirect na citatele a jemnovatele potrebuju property...
        public new Expression leftOperand { get => denominator; }

        protected override string SignRepresentation => "/";
    }

    public class Minus : UnaryExpression {
        protected override string SignRepresentation => "-";
    }

    public class Integer : Constant {
        public new readonly int number;

        public Integer(int i) {
            number = i;
        }

        public override string ToString() => number.ToString();
        
        public override Integer DeepCopy() => (Integer) this.MemberwiseClone();
    }

    public class RealNumber : Constant {
        public new readonly double number;

        public RealNumber(double d) {
            number = d;
        }

        public override string ToString() => number.ToString();

        public override RealNumber DeepCopy() => (RealNumber)this.MemberwiseClone();
    }

    public class Variable : Value {
        public readonly string variableName;
        public readonly Constant constant;

        public Variable(Constant c, string s) {
            variableName = s;
            constant = c;
        }

        public override string ToString() => $"{constant}{variableName}";

        // I dislike the current implementation of DeepCopy, I will think through options and rewrite it. 
        public override Variable DeepCopy() {
            // using Microsoft's docs recomended approach:
            // https://docs.microsoft.com/en-us/dotnet/api/system.string.copy?view=net-5.0
            string otherName = variableName[..];
            if (constant is Integer i) {
                Integer otherConstant = new Integer(i.number);
                return new Variable(otherConstant, otherName);
            } else if( constant is RealNumber r) {
                RealNumber otherConstant = new(r.number);
                return new Variable(otherConstant, otherName);
            }
            throw new Exception("DeepCopy method of Variable class encountered unknown Constant's child.");
        }
    }
}