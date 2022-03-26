using static System.Console;
using InfiniteEngine;
using Q = InfiniteEngine.RationalNumber;
namespace Tree_Grower;

class NestGrowExpressionValue
{
	readonly RandomEasyZT easyZT = new();
	readonly Random rand = new();

	public BinaryExpression GetRandomLeaf(BinaryExpression rootAtomicTree) {
		// 1. pick if you want to nest left or right child
		// 2. pick if you want nest add, sub, mul or div 
		// 3. pick which easyZT will be one of the children
		// 4. get numerator and denominator
		// 5. create the binary expr child, by counting the other not picked child (which is now determined by: op, firstChild, rootVal (num/den) )
		// 6. substitute the created binary expr with coresponding leaf of rootAtomicTree 

		bool secondChildContainsHugeNumbers = true;
		bool left = true;
		int opType = -1;
		Q firstChild = new(-1,-1);
		Q secondChild = new(-1,-1);

		while(secondChildContainsHugeNumbers) {
			left = rand.Next(0, 2) == 0;
			opType = rand.Next(0, 4);
			firstChild = easyZT.GetNext().DeepCopy();
			Q? childRootQ = left ? rootAtomicTree.leftOperand as Q : rootAtomicTree.rightOperand as Q; // dangerous, but written to work like this:
			if (childRootQ == null) {
				return new Addition(1, 1);
			}

			if (opType == 0) { // calc for additon
				secondChild = childRootQ - firstChild;
			} else if (opType == 1) { // calc for subtraction
				secondChild = childRootQ + firstChild;
			} else if (opType == 2) { // calc for multiplication
				secondChild = childRootQ / firstChild;
			} else { // calc for division
				secondChild = childRootQ * firstChild;
			}

			secondChildContainsHugeNumbers = ContainsHugeNumbers(secondChild);
		}

		// WriteLine($"second child: {secondChild}");

		return GrowExpressionValue(rootAtomicTree.DeepCopy(), left, opType, firstChild, secondChild);
	}

	BinaryExpression GrowExpressionValue(BinaryExpression root, bool left, int opType, Q firstChild, Q secondChild) {
		if (opType == 0) {
			Addition grownValue = new(firstChild, secondChild);
			if (left) {
				root.leftOperand = grownValue;
			} else {
				root.rightOperand = grownValue;
			}
		} else if (opType == 1) {
			Subtraction grownValue = new(secondChild, firstChild);
			if (left) {
				root.leftOperand = grownValue;
			} else {
				root.rightOperand = grownValue;
			}
		} else if (opType == 2) {
			Multiplication grownValue = new(secondChild, firstChild);
			if (left) {
				root.leftOperand = grownValue;
			} else {
				root.rightOperand = grownValue;
			}
		} else {
			Division grownValue = new(secondChild, firstChild);
			if (left) {
				root.leftOperand = grownValue;
			} else {
				root.rightOperand = grownValue;
			}
		}
		return root;
	}

	bool ContainsHugeNumbers(Q q) => q.Num < -50 || q.Num > 50 || q.Den < -50 || q.Den > 50 ;
}


// int childRootNum = childRootQ.Num;
// int childRootDen = childRootQ.Den;



/*
 
 
	BinaryExpression GrowAddition(BinaryExpression rootAtomicTree, bool left, Q firstChild, Q childRootQ) {
		Q secondChild = childRootQ - firstChild;
		Addition grownValue = new(firstChild, secondChild);
		if(left) {
			rootAtomicTree.leftOperand = grownValue;
		} else {
			rootAtomicTree.rightOperand = grownValue;
		}
		return rootAtomicTree;
	}

	BinaryExpression GrowSubtraction(BinaryExpression rootAtomicTree, bool left, Q firstChild, Q childRootQ) {
		Q secondChild = childRootQ + firstChild;
		Subtraction grownValue = new(secondChild, firstChild);
		if (left) {
			rootAtomicTree.leftOperand = grownValue;
		} else {
			rootAtomicTree.rightOperand = grownValue;
		}
		return rootAtomicTree;
	}

	BinaryExpression GrowMultiplication(BinaryExpression rootAtomicTree, bool left, Q firstChild, Q childRootQ) {
		Q secondChild = childRootQ / firstChild;
		Multiplication grownValue = new(secondChild, firstChild);
		if (left) {
			rootAtomicTree.leftOperand = grownValue;
		} else {
			rootAtomicTree.rightOperand = grownValue;
		}
		return rootAtomicTree;
	}

	BinaryExpression GrowDivision(BinaryExpression rootAtomicTree, bool left, Q firstChild, Q childRootQ) {
		Q secondChild = childRootQ * firstChild;
		Division grownValue = new(secondChild, firstChild);
		if (left) {
			rootAtomicTree.leftOperand = grownValue;
		} else {
			rootAtomicTree.rightOperand = grownValue;
		}
		return rootAtomicTree;
	}
 
 
 */