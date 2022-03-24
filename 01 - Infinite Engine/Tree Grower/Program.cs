using static System.Console;
using InfiniteEngine;
using Q = InfiniteEngine.RationalNumber;


WriteLine("Hello, World!");
List<Q> fractions = SetOfRationals.GetAll( 1, 10, true);
int counter = 0;
foreach(Q q in fractions) 
	WriteLine($"{++counter}: {q}");


using StreamWriter statsSW = new StreamWriter("statistics.txt");

int statsTotal = 0;
int littleTotal = 0;
int notLitleTotal = 0;

foreach(Q q in fractions) {
	Q root = new(q.Num, q.Den);
	TreeGrower tg = new(root);
	List<Expression> forest = tg.GrowTree();

	WriteLine($"In total there are {forest.Count} grown expression trees.");
	using StreamWriter sw = new($"forest state space - {root.Num}__{root.Den}.txt");
	foreach (Expression tree in forest)
		sw.WriteLine(tree);
	sw.Dispose();

	WriteLine("We don't want to torture children, so...");
	List<Expression> littleIntegers = new();
	List<Expression> notLittleIntegers = new();

	foreach (Expression tree in forest) {
		if (tree is BinaryExpression be) { // given the code I have written it alwas is
			if (NoTorturer.LittleIntegers(be)) {
				littleIntegers.Add(be);
			} else {
				notLittleIntegers.Add(be);
			}
		} else {
			WriteLine("Not binary expression error!");
			throw new Exception("Not binary expression error!");
		}
	}

	WriteLine($"Forest with little integers has {littleIntegers.Count} trees.");
	WriteLine($"Forest with NOT little integers has {notLittleIntegers.Count} trees.");

	using StreamWriter sw2 = new($"forest with little trees - {root.Num}__{root.Den}.txt");
	foreach (Expression tree in littleIntegers)
		sw2.WriteLine(tree);
	sw2.Dispose();

	using StreamWriter sw3 = new($"forest with NOT little trees - {root.Num}__{root.Den}.txt");
	foreach (Expression tree in notLittleIntegers)
		sw3.WriteLine(tree);
	sw3.Dispose();

	statsSW.WriteLine($"{root} {littleIntegers.Count} / {notLittleIntegers.Count}");
	statsTotal++;
	littleTotal += littleIntegers.Count;
	notLitleTotal += notLittleIntegers.Count;
}

statsSW.WriteLine($"Total roots used: {statsTotal}, total little trees grown: {littleTotal}, total NOT little trees grown: {notLitleTotal}, avg little: {littleTotal/ statsTotal}, avg NOT little: {notLitleTotal / statsTotal}");

statsSW.Dispose();

class TreeGrower
{
	readonly Q root;
	public TreeGrower(Q root) {
		this.root = root;
	}

	public List<Expression> GrowTree() {
		List<Expression> grownTrees = new();
		List<BinaryExpression> addSub = GrowAddSubOperator();
		foreach(BinaryExpression a in addSub)
			grownTrees.Add(a);

		List<BinaryExpression> mulDiv = GrowMulDivOperator();
		foreach(BinaryExpression a in mulDiv)
			grownTrees.Add(a);
		return grownTrees;
	}

	private List<BinaryExpression> GrowAddSubOperator() {
		List<BinaryExpression> addSub = new();
		List<Q> allEasyFractions = SetOfRationals.GetAll(1, 50);
		int adds = 0;
		int subs = 0;
		foreach (Q leftChild in allEasyFractions) {
			if(leftChild < root) {
				adds++;
				Q rightChild = root - leftChild;
				addSub.Add(new Addition(leftChild, rightChild));
			}
			if (leftChild > root) {
				subs++;
				Q rightChild =  leftChild - root;
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

		if(a < -100 || a > 100)
			return false;

		if (b < -100 || b > 100)
			return false;

		if (c < -100 || c > 100)
			return false;

		if (d < -100 || d > 100)
			return false;
		return true;
	}
}