using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteEngine {
    public abstract record Excercise {
        public Expression[] Steps { get; init; }
        public string[] Comments { get; init; }                       // description of next modification : popis následující úpravy
        public Expression[][] IsolatedModifications { get; init; }

        public Expression Problem { get => Steps[0]; }
        public int StepsCount { get => Steps.Length - 1; }
        public Expression Result { get => Steps[^1]; }

        public Excercise( Expression[] s, string[] c, Expression[][] im) {
            Steps = s; Comments = c; IsolatedModifications = im;
        }
    }
    
    record EFractions_S02E00 : Excercise { public EFractions_S02E00(Expression[] s, string[] c, Expression[][] im) : base(s, c, im) { } }
    record EFractions_S02E01 : Excercise { public EFractions_S02E01(Expression[] s, string[] c, Expression[][] im) : base(s, c, im) { } }
}
