using System;

namespace InfiniteEngine {
    public enum Dificulty {
        MENSI, PRIJIMACKY, VETSI, OBROVSKA, CPU
    }

    public abstract class ExcerciseGenerator {
        protected Random rand;
        public readonly Dificulty level;
        public ExcerciseGenerator(Dificulty l) {
            rand = new();
            level = l;
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

        // ensure reasonable distribution of different variables in excercises
        public abstract Excercise GetOne();
        public abstract Excercise[] GetTen();
        public abstract Excercise[] UnsafeGetAll(); // beware may blow up memory, only use to get estimate on number of different problem per dificulty 
    }
}