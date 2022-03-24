using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using Q = RationalNumber;

    public class SetOfRationals : ICombinatoricStructureGenerator<Q> {
        readonly Random rand;

        readonly List<Q> easilyFractionableMini = new() {
            new Q(1, 10),
            new Q(1, 5),
            new Q(1, 4),
            new Q(1, 2)
        };

        public SetOfRationals() {
            rand = new();
        }

        public List<Q> GetAll() => GetAll(1, 50, true);
        public Q GetRandomOne() => GetRandomOne(1, 50, true);

        public static List<Q> GetEasilyFractionable() {  
			List<Q> easilyFractionable = new() {
				new Q(1, 10),
				new Q(1, 5),
				new Q(1, 4),
				new Q(3, 10),
				new Q(2, 5),
				new Q(1, 2),
				new Q(3, 5),
				new Q(3, 4),
				new Q(4, 5),
				new Q(9, 10)
			};	

			return easilyFractionable;
		}
        public List<Q> GetEasilyFractionableMini() => easilyFractionableMini;


		// there are several overloads of get all method.
		// first get all with arity 2 returns trully all, both in simplest form and not in simplest form 
		// getall with arity 3 returnes either fractions in simplest form ot the not in simplest form 
		// getall with arity 4 is the same, plus you have the right to forbid some Qs (by adding the to the List<Q> except, fourth parameter) 
		public static List<Q> GetAll(int low, int high) {
			List<Q> result = new();
			for (int numerator = low; numerator <= high; numerator++)
				for (int denominator = low; denominator <= high; denominator++) {
					if (denominator == 1)
						continue; // ignore whole numbers, despite making it through the simplest form definition
					Q f = new(numerator, denominator);
					result.Add(f);
				}
			return result;
		}


		public static List<Q> GetAll(int low, int high, bool simplestForm) {
            List<Q> result = new();
            for (int numerator = low; numerator <= high; numerator++)
                for (int denominator = low; denominator <= high; denominator++) {
                    if (denominator == 1)
                        continue; // ignore whole numbers, despite making it through the simplest form definition
                    Q f = new(numerator, denominator);
                    if (f.IsSimplestForm() == simplestForm)
                        result.Add(f);
                }
            return result;
        }

        public static List<Q> GetAll(int low, int high, bool simplestForm, List<Q> except) {
            List<Q> notFiltered = GetAll(low, high, simplestForm);
            List<Q> filtered = new();
            foreach (Q f in notFiltered)
                if (!except.Contains(f))
                    filtered.Add(f);
            return filtered;
        }

        public Q GetRandomOne(int lowerBound, int upperBound, bool simplestForm) {
            List<Q> candidates = GetAll(lowerBound, upperBound, simplestForm);
            return candidates[rand.Next(0, candidates.Count)]; // usereturning deep copy as means for GC to collect candidates sooner. Note: Does this actually help or not? 
        }

		public static List<Q> GetEasyMediumZTSet() {
			List<Q> result = new();
			for(int i = 1; i < 10; i++)
				for(int j = 10; j < 16; j++) {
					Q test = new(i, j);
					if(test.IsSimplestForm())
						result.Add(test);
				}

			for(int i = 10; i < 16; i++)
				for(int j = 2; j < 10; j++) {
					Q test = new(i, j);
					if(test.IsSimplestForm())
						result.Add(test);
				}
			return result;
		}
    }
}