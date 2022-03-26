using static System.Console;
using InfiniteEngine;
using Q = InfiniteEngine.RationalNumber;
namespace Tree_Grower;

class TreeGrower
{
	readonly Q root;
	public TreeGrower(Q root) {
		this.root = root;
	}

	public List<Expression> GrowTree() {
		List<Expression> grownTrees = new();
		List<BinaryExpression> addSub = GrowAddSubOperator();
		foreach (BinaryExpression a in addSub)
			grownTrees.Add(a);

		List<BinaryExpression> mulDiv = GrowMulDivOperator();
		foreach (BinaryExpression a in mulDiv)
			grownTrees.Add(a);
		return grownTrees;
	}

	private List<BinaryExpression> GrowAddSubOperator() {
		List<BinaryExpression> addSub = new();
		List<Q> allEasyFractions = SetOfRationals.GetAll(1, 50);
		int adds = 0;
		int subs = 0;
		foreach (Q leftChild in allEasyFractions) {
			if (leftChild < root) {
				adds++;
				Q rightChild = root - leftChild;
				addSub.Add(new Addition(leftChild, rightChild));
			}
			if (leftChild > root) {
				subs++;
				Q rightChild = leftChild - root;
				addSub.Add(new Subtraction(leftChild, rightChild));
			}
		}
		WriteLine($"There are {adds} grown additions.");
		WriteLine($"There are {subs} grown subtractions.");
		return addSub;
	}

	private List<BinaryExpression> GrowMulDivOperator() {
		List<BinaryExpression> mulDiv = new();
		List<Q> allEasyFractions = SetOfRationals.GetAll(1, 50);
		int muls = 0;
		int divs = 0;
		foreach (Q leftChild in allEasyFractions) {
			muls++;
			Q rightChild = root / leftChild;
			mulDiv.Add(new Multiplication(leftChild, rightChild));

			divs++;
			Q divLeft = root * leftChild;
			rightChild = root * leftChild;
			mulDiv.Add(new Division(divLeft, leftChild));
		}
		WriteLine($"There are {muls} grown multiplications.");
		WriteLine($"There are {divs} grown divisions.");
		return mulDiv;
	}
}


class NoTorturer
{
	static public bool LittleIntegers(BinaryExpression be) {
		// dangerous code, I have written the parameter to be like this, in general input may be in form that cast as to null
		Q left = be.leftOperand as Q;
		Q right = be.rightOperand as Q;
		int a = left.Num;
		int b = left.Den;
		int c = right.Num;
		int d = right.Den;

		if (a < -50 || a > 50)
			return false;

		if (b < -50 || b > 50)
			return false;

		if (c < -50 || c > 50)
			return false;

		if (d < -50 || d > 50)
			return false;
		return true;
	}
}

