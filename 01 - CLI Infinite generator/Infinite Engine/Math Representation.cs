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

    public abstract class Constant : Value { }

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

    public class Minus : UnaryExpression {
        protected override string SignRepresentation => "-";
    }

    public class Integer : Constant {
        public readonly int number;

        public Integer(int i) {
            number = i;
        }

        public override string ToString() => number.ToString();
        
        public override Integer DeepCopy() => (Integer) this.MemberwiseClone();
    }

    public class RealNumber : Constant {
        public readonly double number;

        public RealNumber(double d) {
            number = d;
        }

        public override string ToString() => number.ToString();

        public override RealNumber DeepCopy() => (RealNumber)this.MemberwiseClone();
    }

    public class Fraction : Value { }

    public class Variable : Value { }

}