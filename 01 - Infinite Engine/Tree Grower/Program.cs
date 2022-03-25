using static System.Console;
using InfiniteEngine;
using Tree_Grower;
using Q = InfiniteEngine.RationalNumber;

// first create 2D matrices with atomic tree, so that I can easily call them...
// then use atomic tree class to chain GEV operation 4 times once with each: add, sub, div, mul

List<Q> easyFractions = SetOfRationals.GetAll(1, 10, true);

List<List<List<Addition>>> addEasyFractions = new();
List<List<List<Subtraction>>> subEasyFractions = new();
List<List<List<Multiplication>>> mulEasyFractions = new();
List<List<List<Division>>> divEasyFractions = new();

// omg..

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
			// add it to appropriate coords...
			if (leftIsSimplestEasyZT(add) || rightIsSimplestEasyZT(add))
				addEasyFractions[num][den].Add(add);

		} else if(atomicTree is Subtraction sub && NoTorturer.LittleIntegers(sub)) {
			if (leftIsSimplestEasyZT(sub) || rightIsSimplestEasyZT(sub))
				subEasyFractions[num][den].Add(sub);
		} else if(atomicTree is Multiplication mul && NoTorturer.LittleIntegers(mul)) {
			if (leftIsSimplestEasyZT(mul) || rightIsSimplestEasyZT(mul))
				mulEasyFractions[num][den].Add(mul);
		} else if(atomicTree is Division div && NoTorturer.LittleIntegers(div)) {
			if (leftIsSimplestEasyZT(div) || rightIsSimplestEasyZT(div))
				divEasyFractions[num][den].Add(div);
		} else {
			if(atomicTree is not Addition && atomicTree is not Subtraction && atomicTree is not Multiplication && atomicTree is not Division) {
				WriteLine("Critical error, met impossible class");
				throw new Exception("Critical bug! Your logic is flawed.");
			}
		}
	}
}

// first job done, atomic trees are sitting inside their 3D matrix of atomic expression trees..
// Assert correctness:

// counter must be equal to 239662, and sums foreach 2D coordinate must match the numbers from statistics.txt
int counter = 0;
using StreamWriter sw = new($"atomic trees - tree grower tableau - code simplification all are growable using EasyZT either to left or right.txt");

for (int i = 0; i < 11;i++) {
	for(int j = 0; j < 11;j++) {
		int addCount = addEasyFractions[i][j].Count;
		int subCount = subEasyFractions[i][j].Count;
		int mulCount = mulEasyFractions[i][j].Count;
		int divCount = divEasyFractions[i][j].Count;
		if(addCount != 0) {
			int sum = addCount + subCount + mulCount + divCount;
			counter +=  sum;
			WriteLine($"{addCount} + {subCount} + {mulCount} + {divCount} = {sum}");
			foreach(Addition add in addEasyFractions[i][j])
				
					sw.WriteLine(add);
			foreach (Subtraction sub in subEasyFractions[i][j])
				
					sw.WriteLine(sub);
			foreach (Multiplication mul in mulEasyFractions[i][j])
				
					sw.WriteLine(mul);
			foreach (Division div in divEasyFractions[i][j])
				
					sw.WriteLine(div);

			if(addCount == 0 && subCount == 0 && mulCount == 0 && divCount == 0) {
				// if the runtime survives this condition, I will not need to consider returns while growing for now.. :) 
				// if the error gets thrown I need to figure it out... 
				throw new Exception("Some list of binary ops got 0 count after cutting all non EasyZT leaves...");
			}
		}
	}
}
WriteLine($"Sum check: {counter}");
sw.Dispose();


/// correct, 

bool leftIsSimplestEasyZT(BinaryExpression be) {
	Q? left = be.leftOperand as Q;
	if(left == null) 
		throw new Exception("Critical error! Your logic is flawed. Rethink.");

	return left.IsSimplestForm() && left.Num < 11 && left.Den < 11;
}

bool rightIsSimplestEasyZT(BinaryExpression be) {
	Q? right = be.leftOperand as Q;
	if (right == null)
		throw new Exception("Critical error! Your logic is flawed. Rethink.");

	return right.IsSimplestForm() && right.Num < 11 && right.Den < 11;
}




List<BinaryExpression> sedmDesetin = new();

foreach(Addition add in addEasyFractions[7][10]) {
	if(leftIsSimplestEasyZT(add) || rightIsSimplestEasyZT(add))
		sedmDesetin.Add(add);
}

foreach(Subtraction sub in subEasyFractions[7][10]) {
	if (leftIsSimplestEasyZT(sub) || rightIsSimplestEasyZT(sub))
		sedmDesetin.Add(sub);
}

foreach(Multiplication mul in mulEasyFractions[7][10]) {
	if (leftIsSimplestEasyZT(mul) || rightIsSimplestEasyZT(mul))
		sedmDesetin.Add(mul);
}

foreach(Division div in divEasyFractions[7][10]) {
	if (leftIsSimplestEasyZT(div) || rightIsSimplestEasyZT(div))
		sedmDesetin.Add(div);
}


WriteLine(sedmDesetin.Count);

using StreamWriter resultSw = new($"sedm desetin S01E02.txt");
foreach(BinaryExpression be in sedmDesetin) {
	resultSw.WriteLine(be);
}
resultSw.Dispose();


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

WriteLine("Initiating final task");
int finalCounter = 0;
using StreamWriter stream = new($"sedm desetin S01E03.txt");
foreach (BinaryExpression be in sedmDesetin) {
	finalCounter++;
	LeafeId li = RandomStateSpaceCrawler.FetchRandomLeafe(be, addEasyFractions, subEasyFractions, mulEasyFractions, divEasyFractions);
	BinaryExpression exercise = ExerciseBuilder.Construct(be, li, addEasyFractions, subEasyFractions, mulEasyFractions, divEasyFractions);
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
	static public LeafeId FetchRandomLeafe(BinaryExpression root, List<List<List<Addition>>> addFractions, List<List<List<Subtraction>>> subFractions, List<List<List<Multiplication>>> mulFractions, List<List<List<Division>>> divFractions) {

		return new LeafeId(true, true, true, 0, 0, 0);
	}

}


class ExerciseBuilder
{
	static public BinaryExpression Construct(BinaryExpression root, LeafeId li, List<List<List<Addition>>> addFractions, List<List<List<Subtraction>>> subFractions, List<List<List<Multiplication>>> mulFractions, List<List<List<Division>>> divFractions) {
	
		return root;
	}

}

































































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