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
}