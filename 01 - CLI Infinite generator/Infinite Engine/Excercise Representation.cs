using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
    public abstract class Excercise {
        public Expression[] steps;
        public string[] comments;                           // description of next modification : popis následující úpravy
        public Expression[][] isolatedModifications;

        public Expression Problem { get => steps[0]; }
        public int StepsCount { get => steps.Length - 1; }
        public Expression Result { get => steps[^1]; }

        public Excercise( Expression[] s, string[] c, Expression[][] im) {
            steps = s; comments = c; isolatedModifications = im;
        }
    }

    class EFractions_S01E01 : Excercise { 
        public EFractions_S01E01(Expression[] s, string[] c, Expression[][] im) : base(s, c, im) { }
    }
}
