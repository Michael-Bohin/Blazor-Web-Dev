using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
    class MathAlgorithms {
        // contains general math algorithms, that are usefull across different topics of math
        
        // find greatest common divisor of two integers:
        public static int EuclidsGCD(int a, int b) {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0)
                return b;
            if (b == 0)
                return a;

            int t;
            while(a % b != 0) {
                t = b;
                b = a % b;
                a = t;
            }
            return b;
        }

        // least common multiple -> multiply the two numbers and divide the result by GCD
        public static int EuclidsLCM(int a, int b) => a * b / EuclidsGCD(a, b);
    }
}
