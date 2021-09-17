using System;
using System.Collections.Generic;

namespace InfiniteEngine {
    using Q = RationalNumber;

    public record EFractions_S02E01 : Excercise {

        public EFractions_S02E01(string[] s, string[] c, string[] im, Q r, Expression prob) : base(s, c, im, AnswerKind.Q) {
            Answer = r;
            Problem = prob;
        }

        public readonly Expression Problem;
        public readonly Q Answer;
    }

    public record EFractions_S02_A : BriefExcercise {
       // public readonly Expression Problem;
        public readonly Q Answer;

        public EFractions_S02_A(string[] steps, string[] comments, Q answer/*, Expression prob*/) : base(steps, comments, AnswerKind.Q) {
            Answer = answer;
            //Problem = prob;
        }
    }
}