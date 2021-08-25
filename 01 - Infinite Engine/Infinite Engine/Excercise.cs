using System.Collections.Immutable;
using System;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
/// I need inheritance -> record over struct
/// I want deep immutablity -> immutable arrays
/// </summary>
namespace InfiniteEngine {
    using Q = RationalNumber;

    public enum AnswerKind {
        N, Z, Q, R, STRING, YESNO, 
    }
    public abstract record Excercise {
        public ImmutableArray<string> Steps { get; init; }
        public ImmutableArray<string> Comments { get; init; }
        public ImmutableArray<string> IsolatedModifications { get; init; }
        public AnswerKind TypeOfAnswer { get; init; }

        public string Problem { get => Steps[0]; }
        public string Answer { get => Steps[^1]; }
        
        public Excercise(string[] s, string[] c, string[] im, AnswerKind ak) {
            Steps = s.ToImmutableArray<string>();
            Comments = c.ToImmutableArray<string>();
            IsolatedModifications = im.ToImmutableArray<string>();
            TypeOfAnswer = ak;
        }
    }

    public record EFractions_S02E01 : Excercise { 
        public EFractions_S02E01(string[] s, string[] c, string[] im) 
            : base(s, c, im, AnswerKind.Q) { } 
    }

    public record EEquations_S02E01 : Excercise {
        public EEquations_S02E01(string[] s, string[] c, string[] im) 
            : base(s, c, im, AnswerKind.R) { }
    }

    public record ESlovniUloha_S02E01 : Excercise {
        public ESlovniUloha_S02E01(string[] s, string[] c, string[] im) 
            : base(s, c, im, AnswerKind.STRING) { }
    }



}
