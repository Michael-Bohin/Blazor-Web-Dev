using System.Collections.Immutable;
using System;

/// I need inheritance -> record over struct
/// I want deep immutablity -> immutable arrays

namespace InfiniteEngine {
    public enum AnswerKind {
        N, Z, Q, R, STRING, YESNO, 
    }

    public abstract record Excercise {
        public ImmutableArray<string> Steps { get; init; }
        public ImmutableArray<string> Comments { get; init; }
        public ImmutableArray<string> IsolatedModifications { get; init; }
        public AnswerKind TypeOfAnswer { get; init; }
        
        public Excercise(string[] s, string[] c, string[] im, AnswerKind ak) {
            Steps = s.ToImmutableArray<string>();
            Comments = c.ToImmutableArray<string>();
            IsolatedModifications = im.ToImmutableArray<string>();
            TypeOfAnswer = ak;
        }

        public Excercise(string[] s, string[] c, AnswerKind ak) {
            Steps = s.ToImmutableArray<string>();
            Comments = c.ToImmutableArray<string>();
            string[] empty = Array.Empty<string>();
            IsolatedModifications = empty.ToImmutableArray<string>();
            TypeOfAnswer = ak;
        }
    }

    public abstract record BriefExcercise : Excercise {
        public BriefExcercise(string[] steps, string[] comments, AnswerKind ak): base(steps, comments, ak) { 
            // empty own constructor
        }
    }
}