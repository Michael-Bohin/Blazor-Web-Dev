using System.Collections.Immutable;
using System;

/// I need inheritance -> record over struct
/// I want deep immutablity -> immutable arrays

namespace InfiniteEngine
{
	using Q = RationalNumber;

	public enum AnswerKind
	{
		N, Z, Q, R, STRING, YESNO,
	}

	public abstract record Excercise
	{
		public ImmutableArray<string> Steps {
			get; init;
		}
		public ImmutableArray<string> Comments {
			get; init;
		}
		public ImmutableArray<string> IsolatedModifications {
			get; init;
		}
		public AnswerKind TypeOfAnswer {
			get; init;
		}

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

	public abstract record BriefExcercise : Excercise
	{
		public BriefExcercise(string[] steps, string[] comments, AnswerKind ak) : base(steps, comments, ak) {
			// empty own constructor
		}
	}

	public record EFractions_S02E01 : Excercise
	{

		public EFractions_S02E01(string[] s, string[] c, string[] im, Q r, Expression prob) : base(s, c, im, AnswerKind.Q) {
			Answer = r;
			Problem = prob;
		}

		public readonly Expression Problem;
		public readonly Q Answer;
	}

	public record EFractions_S02 : BriefExcercise
	{
		// public readonly Expression Problem;
		public readonly Q Answer;

		public EFractions_S02(string[] steps, string[] comments, Q answer/*, Expression prob*/) : base(steps, comments, AnswerKind.Q) {
			Answer = answer;
		}
	}
}