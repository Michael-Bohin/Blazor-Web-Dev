using static System.Console;
using InfiniteEngine;
using Tree_Grower;
using Q = InfiniteEngine.RationalNumber;
using System;

// first create 2D matrices with atomic tree, so that I can easily call them...
// then use atomic tree class to chain GEV operation 4 times once with each: add, sub, div, mul

List<Q> easyFractions = SetOfRationals.GetAll(1, 10, true);

List<List<List<Addition>>> addEasyFractions = new();
List<List<List<Subtraction>>> subEasyFractions = new();
List<List<List<Multiplication>>> mulEasyFractions = new();
List<List<List<Division>>> divEasyFractions = new();


for(int i  =0 ; i < 11; i++) {
	addEasyFractions.Add(new List<List<Addition>>());
	subEasyFractions.Add(new List<List<Subtraction>>());
	mulEasyFractions.Add(new List<List<Multiplication>>());
	divEasyFractions.Add(new List<List<Division>>());
	for (int j = 0; j < 11; j++) {
		addEasyFractions[i].Add(new List<Addition>());
		subEasyFractions[i].Add(new List<Subtraction>());
		mulEasyFractions[i].Add(new List<Multiplication>());
		divEasyFractions[i].Add(new List<Division>());
	}
}

// so now we have empty lists actual 3D matrix of atomic tree additions, subtractions, multiplications, divisons....
// the reasons for doing this suicide is that I can adress them by their value and choose randomly
// say I will need an atomic tree with value 3/2 and I want it to be addition. 
// these are sitting inside a list at adress: addEasyFractions[3][2]
// now I dont know ahead of time, how many there will be, so I will need to check count of that list and call random to get random atomic tree that is addition and equals 3/2....

// First I will fill the 3d matrix:
NoTorturer nt = new();

foreach(Q easyQ in easyFractions) {
	int num = easyQ.Num;
	int den =  easyQ.Den;
	Q root = new(num, den);
	TreeGrower tg = new(root);
	List<Expression> forest = tg.GrowTree();

	foreach(Expression atomicTree in forest) {
		if(atomicTree is Addition add && NoTorturer.LittleIntegers(add)) {
			addEasyFractions[num][den].Add(add);
		} else if(atomicTree is Subtraction sub && NoTorturer.LittleIntegers(sub)) {
			subEasyFractions[num][den].Add(sub);
		} else if(atomicTree is Multiplication mul && NoTorturer.LittleIntegers(mul)) {
			mulEasyFractions[num][den].Add(mul);
		} else if(atomicTree is Division div && NoTorturer.LittleIntegers(div)) {
			divEasyFractions[num][den].Add(div);
		} else {
			if(atomicTree is not Addition && atomicTree is not Subtraction && atomicTree is not Multiplication && atomicTree is not Division) {
				WriteLine("Critical error, met impossible class");
				throw new Exception("Critical bug! Your logic is flawed.");
			}
		}
	}
}

Random rand = new();

bool nestTwice = true;


foreach(Q easyZT in easyFractions) {
	int easyNum = easyZT.Num;
	int easyDen = easyZT.Den;

	int addCount = addEasyFractions[easyNum][easyDen].Count;
	int subCount = subEasyFractions[easyNum][easyDen].Count;
	int mulCount = mulEasyFractions[easyNum][easyDen].Count;
	int divCount = divEasyFractions[easyNum][easyDen].Count;

	WriteLine($">>>>> {addCount} , {subCount} , {mulCount} , {divCount} <<<<< ");
	using StreamWriter stream = new($"GEV call ONCE {easyNum} {easyDen} count-10_000x.txt");
	NestGrowExpressionValue nestGEV = new();

	for (int repeat = 0; repeat < 10000; repeat++) {
		BinaryExpression be;
		int pickOp = rand.Next(0, 4);
		if (pickOp == 0) {
			be = addEasyFractions[easyNum][easyDen][rand.Next(0, addCount)];
		} else if (pickOp == 1) {
			be = subEasyFractions[easyNum][easyDen][rand.Next(0, subCount)];
		} else if (pickOp == 2) {
			be = mulEasyFractions[easyNum][easyDen][rand.Next(0, mulCount)];
		} else {
			be = divEasyFractions[easyNum][easyDen][rand.Next(0, divCount)];
		}
		BinaryExpression grownTree = nestGEV.GetRandomLeaf(be);

		/*if (nestTwice) {
			BinaryExpression nextAtomicChild;
			pickOp = rand.Next(0, 4);
			Q q = new(-1,-1);
			if (grownTree.leftOperand is Q r) 
				q = r.DeepCopy();
			if (grownTree.rightOperand is Q s) 
				q = s.DeepCopy();
			
			if (pickOp == 0) {
				nextAtomicChild = addEasyFractions[q.Num][q.Den][rand.Next(0, addEasyFractions[q.Num][q.Den].Count)];
			} else if (pickOp == 1) {
				nextAtomicChild = subEasyFractions[q.Num][q.Den][rand.Next(0, subEasyFractions[q.Num][q.Den].Count)];
			} else if (pickOp == 2) {
				nextAtomicChild = mulEasyFractions[q.Num][q.Den][rand.Next(0, mulEasyFractions[q.Num][q.Den].Count)];
			} else {
				nextAtomicChild = divEasyFractions[q.Num][q.Den][rand.Next(0, divEasyFractions[q.Num][q.Den].Count)];
			}

		}*/

		stream.WriteLine($"{repeat} >> {grownTree}");
	}

	stream.Dispose();
	stream.Close();
}
















































































































































































// first job done, atomic trees are sitting inside their 3D matrix of atomic expression trees..
// Assert correctness:

// counter must be equal to 239662, and sums foreach 2D coordinate must match the numbers from statistics.txt


/*int counter = 0;
using StreamWriter sw = new($"atomic trees - tree grower tableau - code simplification all are growable using EasyZT either to left or right.txt");

for (int i = 0; i < 11;i++) {
	for(int j = 0; j < 11;j++) {
		int _addCount = addEasyFractions[i][j].Count;
		int _subCount = subEasyFractions[i][j].Count;
		int _mulCount = mulEasyFractions[i][j].Count;
		int _divCount = divEasyFractions[i][j].Count;
		if ( _addCount != 0) {
			int sum = _addCount + _subCount + _mulCount + _divCount;
			counter +=  sum;
			WriteLine($"{_addCount} + {_subCount} + {_mulCount} + {_divCount} = {sum}");
			foreach(Addition add in addEasyFractions[i][j])
				
					sw.WriteLine(add);
			foreach (Subtraction sub in subEasyFractions[i][j])
				
					sw.WriteLine(sub);
			foreach (Multiplication mul in mulEasyFractions[i][j])
				
					sw.WriteLine(mul);
			foreach (Division div in divEasyFractions[i][j])
				
					sw.WriteLine(div);

			if(_addCount == 0 && _subCount == 0 && _mulCount == 0 && _divCount == 0) {
				// if the runtime survives this condition, I will not need to consider returns while growing for now.. :) 
				// if the error gets thrown I need to figure it out... 
				throw new Exception("Some list of binary ops got 0 count after cutting all non EasyZT leaves...");
			}
		}
	}
}
WriteLine($"Sum check: {counter}");
sw.Dispose();*/



/*

List<BinaryExpression> sedmDesetin = new();

foreach (Addition add in addEasyFractions[7][10]) {
	// if(leftIsSimplestEasyZT(add) || rightIsSimplestEasyZT(add))
	sedmDesetin.Add(add);
}

foreach (Subtraction sub in subEasyFractions[7][10]) {
	// if (leftIsSimplestEasyZT(sub) || rightIsSimplestEasyZT(sub))
	sedmDesetin.Add(sub);
}

foreach (Multiplication mul in mulEasyFractions[7][10]) {
	// if (leftIsSimplestEasyZT(mul) || rightIsSimplestEasyZT(mul))
	sedmDesetin.Add(mul);
}

foreach (Division div in divEasyFractions[7][10]) {
	// if (leftIsSimplestEasyZT(div) || rightIsSimplestEasyZT(div))
	sedmDesetin.Add(div);
}

WriteLine(sedmDesetin.Count);

using StreamWriter resultSw = new($"sedm desetin S01E02.txt");
foreach (BinaryExpression be in sedmDesetin) {
	resultSw.WriteLine(be);
}
resultSw.Dispose();
*/

/*
/// correct, 

bool leftIsSimplestEasyZT(BinaryExpression be) {
	Q? left = be.leftOperand as Q;
	if(left == null) 
		throw new Exception("Critical error! Your logic is flawed. Rethink.");

	return left.IsSimplestForm() && left.Num < 11 && left.Den < 11 && left.Den != 1;
}

bool rightIsSimplestEasyZT(BinaryExpression be) {
	Q? right = be.leftOperand as Q;
	if (right == null)
		throw new Exception("Critical error! Your logic is flawed. Rethink.");

	return right.IsSimplestForm() && right.Num < 11 && right.Den < 11 && right.Den != 1;
} 
 
*/


/*RandomEasyZT easyZT = new();
WriteLine(">>>> easyZT set:");
foreach(Q q in easyZT.SetOfEasyZT)
	WriteLine(q);
WriteLine(easyZT.SetOfEasyZT.Count);
WriteLine(">>>> easyZT set:");*/


/// Pseudocode of the 3D generator:
/// For the simplicity 1st GEV call will always be addition
/// 2nd call will always be multiplication 
/// 3rd will always be addition 
/// this way i can also forget about the subtraction 0 count at 9/2, all lists contain at least one binary op :)
/// 

/// 0. foreach atomicke tablo rovne sedm desetin:
/// 1. Figue out 1st leftright, 1st index
/// 2. 2nd left  right, 2nd index
/// 3. 3rd left right, 3rd index 
/// 4. The exercise has been drawn unambiguously -> Create the binary expression and writeline it into resultint file

/*
WriteLine("Initiating final task");
int finalCounter = 0;
using StreamWriter stream = new($"sedm desetin S01E03.txt");
foreach (BinaryExpression be in sedmDesetin) {
	finalCounter++;
	LeafeId li = RandomStateSpaceCrawler.FetchRandomLeafe(be, addEasyFractions,  mulEasyFractions);
	BinaryExpression exercise = ExerciseBuilder.Construct(be, li, addEasyFractions,  mulEasyFractions);
	stream.WriteLine(exercise);
}

WriteLine($"final foreach iterated {finalCounter} times.");

stream.Dispose();
stream.Close();


record LeafeId
{
	public LeafeId(bool firstL, bool secondL, bool thirdL, int firstIn, int secondIn, int thirdIn) {
		firstLeft = firstL;
		secondLeft = secondL;
		thirdLeft = thirdL;

		firstIndex = firstIn;
		secondIndex	= secondIn;
		thirdIndex = thirdIn;
	}
	public readonly bool firstLeft;
	public readonly bool secondLeft;
	public readonly bool thirdLeft;

	public readonly int firstIndex;
	public readonly int secondIndex;
	public readonly int thirdIndex;	
}


class RandomStateSpaceCrawler
{
	readonly static Random rand = new();
	static public LeafeId FetchRandomLeafe(BinaryExpression root, List<List<List<Addition>>> addFractions, List<List<List<Multiplication>>> mulFractions) {
		bool leftA, leftB, leftC;
		int idA, idB, idC;
		
		Q leftRootChild = root.leftOperand as Q;
		Q rightRootChild = root.rightOperand as Q;

		leftA = leftIsSimplestEasyZT(root);
		if(leftA) {
			int numA = leftRootChild.Num;
			int denA = leftRootChild.Den;
			int firstCount = addFractions[numA][denA].Count; // dangerous
			WriteLine("root: " + root + "left " + firstCount + " num/den: " + numA + "  " + denA);

			if(firstCount == 0) {
				WriteLine("no options! " + root);
				return new LeafeId(true, true, true, -1, 0, 0);
			}

			idA = rand.Next(0, firstCount);

		} else {
			int numA = rightRootChild.Num;
			int denA = rightRootChild.Den;
			int firstCount = addFractions[numA][denA].Count;
			WriteLine("right " +  firstCount);

			if (firstCount == 0) {
				WriteLine("no options! " + root);
				return new LeafeId(true, true, true, -1, 0, 0);
			}
			idA = rand.Next(0, firstCount);
		}

		return new LeafeId(leftA, true, true, idA, 0, 0);
	}


	static bool leftIsSimplestEasyZT(BinaryExpression be) {
		Q? left = be.leftOperand as Q;
		if (left == null)
			throw new Exception("Critical error! Your logic is flawed. Rethink.");

		return left.IsSimplestForm() && left.Num < 11 && left.Den < 11 && left.Den != 1;
	}
}


class ExerciseBuilder
{
	static public BinaryExpression Construct(BinaryExpression root, LeafeId li, List<List<List<Addition>>> addFractions,  List<List<List<Multiplication>>> mulFractions) {
		if(li.firstIndex == -1)
			return root;

		Q left = root.leftOperand as Q;
		Q right = root.rightOperand as Q;

		int numA, denA;
		if (li.firstLeft) {
			numA = left.Num;
			denA = left.Den;
			root.leftOperand = addFractions[numA][denA][li.firstIndex];
		} else {
			numA = right.Num;
			denA = right.Den;
			root.rightOperand = addFractions[numA][denA][li.firstIndex];
		}
		
		return root;
	}

}
*/
/*
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
*/