using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteEngine {
    public enum Dificulty {
        MENSI, PRIJIMACKY, VETSI, OBROVSKA, CPU
    }

	public enum Op
	{
		Add, Sub, Mul, Div
	}

	public record Zadani { }

	/*
	Excercise Generator abstraction thought process:
	
	Each exercise generator should: 

	1. Define child of class Zadani: what input variables for every excercise are there? 
	2. In constructor:
		a) How many illegal sets of Zadani from pedagogic point of view are there? 
			-> call base constructor with int equal to this answer 
		b) Instantiate all combinatoric sets of possible variables 
		c) For each possible zadani consider its pedagogic legitimacy.
			That means assign it ither to some illegal list or legit list. 
		d) create the kontrolaAritmetiky string, that does the 'podvojne ucetnictvi' of possible combinations
		e) call CreateStatsLog of ExcerciseGenerator parent.
	3. Give implementation to abstract method Consider -> rule out all not legit Zadani's
	4. Give implementation to abstract method Construct -> Define Kuchařka řešení: the heart of the entire idea.
	 */

	public interface IExcerciseGenerator<T>  {
		List<Excercise> GetIllegal(int type, int count);
		List<Excercise> GetLegit(int count);
		List<Excercise> GetPedagogicSet(List<T> zList, int count); // enables easier exploring of options 
		Excercise GetOne();
		Excercise[] GetTen(); // enables defining not uniform distribution of excercises
	}

    public abstract class ExcerciseGenerator<T> : IExcerciseGenerator<T> where T : Zadani  {
		protected abstract void Consider(T z);
		protected abstract Excercise Construct(T z);


        protected Random rand;
        public readonly Dificulty level;

		protected readonly List<T>[] illegal;
		public int[] illegalCounter;
		protected readonly List<T> legit = new();
		protected readonly string[] xtiny = new string[] { "nula", "jedniny", "poloviny", "třetiny", "čtvrtiny", "pětiny", "šestiny", "sedminy", "osminy", "devítiny", "desetiny", "jedenáctiny", "dvanáctiny", "třináctiny", "čtrnáctiny", "patnáctiny", "šestnáctiny", "sedmnáctiny", "osmnáctiny", "devatenáctiny", "dvacetiny" };
		public string stats;
		protected string aritmetickaKontrola;

        public ExcerciseGenerator() {
            rand = new();
            level = Dificulty.PRIJIMACKY;
        }

        public ExcerciseGenerator(Dificulty l) {
            rand = new();
            level = l;
        }

		public ExcerciseGenerator(int illegalSetsCount) {
			illegal = new List<T>[illegalSetsCount];
			for(int i = 0; i < illegal.Length; i++)
				illegal[i] = new();

			illegalCounter = new int[illegalSetsCount];
			rand = new();
            level = Dificulty.PRIJIMACKY;
		}

		protected void CreateStatsLog() {
			StringBuilder sb = new();
			int total = legit.Count;
			for(int i = 0; i < illegal.Length; i++)
				total += illegalCounter[i];

			sb.Append($"Total possible: {total} --> {(double)total / total * 100}%\n");
			for(int i = 0; i < illegal.Length; i++)
				sb.Append($"illegal {i} count: {illegalCounter[i]} --> {(double)illegalCounter[i] / total * 100}%\n");
			
			sb.Append($"legit count: {legit.Count} --> {(double)legit.Count / total * 100}%\n");
			sb.Append(aritmetickaKontrola);

			stats = sb.ToString();
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

		protected List<Excercise> ConstructExcercises(List<T> seznamZadani) {
			List<Excercise> result = new();
			foreach (T z in seznamZadani)
				result.Add(Construct(z));
			return result;
		}
		
		public List<Excercise> GetPedagogicSet(List<T> zList, int count) {
			List<T> subList = new();
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
			T z = legit[pick];
			return Construct(z);
		}

		// ensure reasonable distribution of different variables in excercises
		// by default choose random, if you do not intend uniform distribution
		// of excercise, override the method inside child
		public virtual Excercise[] GetTen() {
			Excercise[] result = new Excercise[10];
			int[] randPerm = GetRandomPermutation(legit.Count);
			for (int i = 0; i < 10; i++) {
				T z = legit[randPerm[i]];
				result[i] = Construct(z);
			}
			return result;
		}

		protected string XtinyCesky(int jmenovatel) => jmenovatel < 21 ? xtiny[jmenovatel] : $"zlomek se jmenovatelem {jmenovatel}";
    }
}