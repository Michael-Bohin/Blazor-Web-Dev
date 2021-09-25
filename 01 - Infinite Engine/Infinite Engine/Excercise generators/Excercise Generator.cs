using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteEngine {
	using Q = RationalNumber;
    public enum Dificulty {
        MENSI, PRIJIMACKY, VETSI, OBROVSKA, CPU
    }

	public enum Op
	{
		Add, Sub, Mul, Div
	}

	//public record Zadani { }

	/*
	Excercise Generator abstraction thought process:
	
	Each exercise generator should: 

	1. Answer following questions: 
		a. What is the set of legit excercises from pedagogic point of view? 
		b. How do you solve each individual excercis? "Solution cookbook" 
		(perhaps Recipe fits bettter?)

	2. Offer IExcerciseGenerator API for all users, so that they can easily 
		call finnished solution recipes and use them. 

	- Methods Get Illegal , Get Pedagogic set are primarily meant for developint 
		phase, when creating the excericse. To see the entire state space. 
	- Methods GetLegit(int count), GetOne and GetTen are meant for finall in production use. 
		- GetOne selects any of legit excercises and returns it along with the solution cookbook. 
		- GetTen does the same, but is meant to use if it is reasonable to not give users uniform 
			distribution of legit excercises.
		- GetLegit method is mainly meant to get large number of excercises or all at the same time. 


	- The primarily goal of the class ExcerciseGenerator is to implement as much 
		shared code for all EGens as possible so that it minimizes each next excercise requires 
		of dev time to be added to the library. 

		Any code that may be shared acros all generators should be here. 
	 */

	public interface IExcerciseGenerator  {
		List<Excercise> GetIllegal(int type, int count);
		List<Excercise> GetLegit(int count);
		// List<Excercise> GetPedagogicSet(List<Zadani> zList, int count); // enables easier exploring of options 
		Excercise GetOne();
		Excercise[] GetTen(); // enables defining not uniform distribution of excercises
	}

    public abstract class ExcerciseGenerator<Zadani> : IExcerciseGenerator  {
		protected abstract Excercise Construct(Zadani z);

        protected Random rand;
        public readonly Dificulty level;

		protected readonly List<Zadani>[] illegal;
		public readonly int illegalSetsCount;
		public int[] illegalCounter;
		protected readonly List<Zadani> legit = new();
		protected readonly string[] xtiny = new string[] { "nula", "jedniny", "poloviny", "třetiny", "čtvrtiny", "pětiny", "šestiny", "sedminy", "osminy", "devítiny", "desetiny", "jedenáctiny", "dvanáctiny", "třináctiny", "čtrnáctiny", "patnáctiny", "šestnáctiny", "sedmnáctiny", "osmnáctiny", "devatenáctiny", "dvacetiny" };
		protected (Op, Op)[] addSubCombinations = new (Op, Op)[] { (Op.Add, Op.Add), (Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) };

		protected string Repr(Op o) { 
			if(o == Op.Add) return "+";
			if(o == Op.Sub) return "-";
			if(o == Op.Mul) return "*";
			return ":";
		}

		protected static bool IsEasyZt(Q a) {
			int cit = a.Num;
			int jm = a.Den;
			return -11 < cit && cit < 11 && 1 < jm && jm < 11;
		}

		public string stats;

        public ExcerciseGenerator() {
            rand = new();
            level = Dificulty.PRIJIMACKY;
        }

        public ExcerciseGenerator(Dificulty l) {
            rand = new();
            level = l;
        }

		public ExcerciseGenerator(int illegalSetsCount) {
			illegal = new List<Zadani>[illegalSetsCount];
			for(int i = 0; i < illegal.Length; i++)
				illegal[i] = new();

			illegalCounter = new int[illegalSetsCount];
			this.illegalSetsCount = illegalSetsCount;
			rand = new();
            level = Dificulty.PRIJIMACKY;
		}

		protected void CreateStatsLog(params int[] list) {
			StringBuilder sb = new();
			int total = legit.Count;
			for(int i = 0; i < illegal.Length; i++)
				total += illegalCounter[i];

			sb.Append($"Total possible: {total} --> {(double)total / total * 100}%\n");
			for(int i = 0; i < illegal.Length; i++)
				sb.Append($"illegal {i} count: {illegalCounter[i]} --> {(double)illegalCounter[i] / total * 100}%\n");

			sb.Append($"legit count: {legit.Count} --> {(double)legit.Count / total * 100}%\n");
			sb.Append(AssertCardinality(list));
			stats = sb.ToString();
		}

		protected static string AssertCardinality(params int[] list) {
			string message = "\nPro kontrolu aritmeticky by melo existovat celkem ";
			for (int i = 0; i < list.Length-1; i++) 
				message += $"{list[i]} *  ";
			message += $"{list[^1]} = ";
			int product = 1;
			for (int i = 0; i < list.Length; i++) 
				product *= list[i];
			message += $"{product} moznosti.\n";
			return message;
		}

		protected static List<int> GetRange(int from, int to) { // inclusive from, to
			List<int> result = new();
			for(int i = from; i <= to; i++)
				result.Add(i);
			return result;
		}

		public List<Excercise> GetIllegal(int type, int count) { 
			if(type < 0 || illegal.Length <= type)
				throw new IndexOutOfRangeException($"Illegal set with index: {type} does not exist!!");
			return GetPedagogicSet(illegal[type], count); 
		}
		public List<Excercise> GetLegit(int count) { 
			if(count < 0)
				throw new IndexOutOfRangeException($"Returning negative number of legit excercises does not make any sense at all.");
			return GetPedagogicSet(legit, count); 
		}

		protected List<Excercise> ConstructExcercises(List<Zadani> seznamZadani) {
			List<Excercise> result = new();
			foreach (Zadani z in seznamZadani)
				result.Add(Construct(z));
			return result;
		}
		
		public List<Excercise> GetPedagogicSet(List<Zadani> zList, int count) {
			List<Zadani> subList = new();
			if (count < zList.Count)
				for (int i = 0; i < count; i++)
					subList.Add(zList[rand.Next(zList.Count)]);
			else
				for (int i = 0; i < zList.Count; i++)
					subList.Add(zList[i]);
			return ConstructExcercises(subList);
		}

        protected int[] GetRandomPermutation(int count) {
            if (count < 1)
                throw new InvalidOperationException("GetRandomPermuation can't use negative boundary. :D ");
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
                result[i] = i;

            for (int i = 0; i < count - 1; i++) {
                // choose number index greater or equal than current index to swap elements with 
                int target = rand.Next(i, count);

                int temp = result[i];
                result[i] = result[target];
                result[target] = temp;
            }
            return result;
        }

        protected bool CoinFlip() => rand.Next(0, 2) == 0;

		public Excercise GetOne() {
			int pick = rand.Next(legit.Count);
			Zadani z = legit[pick];
			return Construct(z);
		}

		// ensure reasonable distribution of different variables in excercises
		// by default choose random, if you do not intend uniform distribution
		// of excercise, override the method inside child
		public virtual Excercise[] GetTen() {
			Excercise[] result = new Excercise[10];
			int[] randPerm = GetRandomPermutation(legit.Count);
			for (int i = 0; i < 10; i++) {
				Zadani z = legit[randPerm[i]];
				result[i] = Construct(z);
			}
			return result;
		}

		protected string XtinyCesky(int jmenovatel) => jmenovatel < 21 ? xtiny[jmenovatel] : $"zlomek se jmenovatelem {jmenovatel}";
    }
}