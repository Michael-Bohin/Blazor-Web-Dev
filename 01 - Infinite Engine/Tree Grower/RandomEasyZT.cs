using static System.Console;
using InfiniteEngine;
using Q = InfiniteEngine.RationalNumber;
namespace Tree_Grower;

class RandomEasyZT
{
	public readonly List<Q> SetOfEasyZT = new();
	public readonly int count;
	private Random rand = new();
	public RandomEasyZT() {
		foreach(Q q in SetOfRationals.GetAll(1, 10, true))
			SetOfEasyZT.Add(q);
		count = SetOfEasyZT.Count;

		/*WriteLine($"Correctiness check, expected: {count}");
		int counter = 0;
		foreach(Q q in SetOfEasyZT) 
			WriteLine($"Counter: {++counter}, >> {q}");*/
	}

	public Q GetNext() => SetOfEasyZT[rand.Next(0, count)];
}