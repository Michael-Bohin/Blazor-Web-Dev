using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
    /*public abstract class CombinatoricStructureGenerator {
        public abstract List<Expression> GetAll();
        public abstract Expression GetRandomOne();
    }*/

    interface ICombinatoricStructureGenerator<T> {
        public List<T> GetAll();
        public T GetRandomOne();
    }

    public class FractionsInSimplestForm : ICombinatoricStructureGenerator<Fraction> {
        readonly Random rand;
        public FractionsInSimplestForm() {
            rand = new();
        }

        public List<Fraction> GetAll() => GetAll(1, 50);
        public Fraction GetRandomOne() => GetRandomOne(1, 50);

        public List<Fraction> GetAll(int lowerBound, int upperBound) {
            List<Fraction> result = new();
            for(int numerator = lowerBound; numerator <= upperBound; numerator++)
                for (int denominator = lowerBound; denominator <= upperBound; denominator++) {
                    if (denominator == 1)
                        continue; // ignore whole numbers, despite making it through the simplest form definition
                    Fraction f = new(numerator, denominator);
                    if (f.IsSimplestForm())
                        result.Add(f);
                }
            return result;
        }

        // !!! repeat equality conditions and implement itacordingly : implement equality operator and hashcode operator 
        public List<Fraction> GetAll(int lowerBound, int upperBound, List<Fraction> except) {
            List<Fraction> notFiltered = GetAll(lowerBound, upperBound);
            List<Fraction> filtered = new();
            foreach( Fraction f in notFiltered)
                if (!except.Contains(f)) // !! implement equality comparison !!
                    filtered.Add(f);
            return filtered;
        }

        public Fraction GetRandomOne(int lowerBound, int upperBound) {
            List<Fraction> candidates = GetAll(lowerBound, upperBound);
            return candidates[rand.Next(0, candidates.Count)].DeepCopy(); // usereturning deep copy as means for GC to collect candidates sooner. Note: Does this actually help or not? 
        }
    }
}
