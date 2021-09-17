﻿using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using Q = RationalNumber;

    public class SetOfRationals : ICombinatoricStructureGenerator<Q> {
        readonly Random rand;

        readonly List<Q> easilyFractionable = new() {
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

        public List<Q> GetEasilyFractionable() => easilyFractionable;
        public List<Q> GetEasilyFractionableMini() => easilyFractionableMini;

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
    }
}