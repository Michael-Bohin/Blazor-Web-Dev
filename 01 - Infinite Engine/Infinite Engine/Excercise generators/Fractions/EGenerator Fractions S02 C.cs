using System.Collections.Generic;

namespace InfiniteEngine
{
	using Q = RationalNumber;
	using M = MathAlgorithms;

	public record Zadani_Fractions_S02_C : Zadani
	{
		public readonly int A;
		public readonly Q B, C, D, E;
		public readonly Op opA, opB;

		public Zadani_Fractions_S02_C(int A, Q B, Q C, Q D, Q E, Op opA, Op opB) {
			this.A = A; this.B = B; this.C = C; this.D = D; this.E = E; this.opA = opA; this.opB = opB;
		}
	}

	public class EGenerator_Fractions_S02_C : ExcerciseGenerator <Zadani_Fractions_S02_C>
	{
		/// 
		/// Jaká je množina pedagogicky legit zadání?
		/// 
		public EGenerator_Fractions_S02_C() : base(4) {
			List<int> moznaA = new();
			for(int i = 2; i < 11; i++)
				moznaA.Add(i);
			List<Q> moznaB = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaC = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaD = SetOfRationals.GetAll(1, 9, true);
			List<Q> moznaE = SetOfRationals.GetAll(1, 9, true);

			(Op, Op)[] operatorCombinations = new (Op, Op)[] { (Op.Add, Op.Add), (Op.Sub , Op.Add), (Op.Add , Op.Sub), (Op.Sub , Op.Sub) };

			aritmetickaKontrola = $"\nPro kontrolu aritmeticky by melo existovat celkem {moznaA.Count} * {moznaB.Count} * {moznaC.Count} * {moznaD.Count} * {moznaE.Count} * {operatorCombinations.Length} = {moznaA.Count * moznaB.Count * moznaC.Count * moznaD.Count * moznaE.Count * operatorCombinations.Length} moznosti.\n";
			
			foreach (int A in moznaA)
				foreach (Q B in moznaB)
					foreach (Q C in moznaC)
						foreach (Q D in moznaD)
							foreach(Q E in moznaE)
								foreach((Op x, Op y) in operatorCombinations)
									Consider(A, B, C, D, E, x, y);
			CreateStatsLog();
		}

		void Consider(int A, Q B, Q C, Q D, Q E, Op x, Op y) {
			// masivni constraint: (A-B) * C == 1
			// D y E je ruzne od nuly
			// D.Den != E.Den
			// Vysledek nalezi do EasyZT

			if( !(  ((Q)A-B) * C == (Q)1 )  ) // massive constraint
				ProcessZadani(A, B, C, D, E, x, y , 0);
			else if( !( (D.Operate(E, y)).Num != 0 )  ) // keep this despite the next constraint also filtering out this option -> in order to test construct doesnt fall on exception
				ProcessZadani(A, B, C, D, E, x, y , 1);
			else if( !( D.Den != E.Den ) ) 
				ProcessZadani(A, B, C, D, E, x, y , 2);
			else if( ! VysledekNaleziDoMnozinyEasyZlomky(D, E, y) )
				ProcessZadani(A, B, C, D, E, x, y , 3);
			else
				legit.Add( GetZadani(A, B, C, D, E, x, y) );
		}

		void ProcessZadani(int A, Q B, Q C, Q D, Q E, Op x, Op y, int i) {
			illegalCounter[i]++;
			if(illegalCounter[i] < 1000)
				illegal[i].Add( GetZadani(A, B, C, D, E, x, y) );
		}

		static Zadani_Fractions_S02_C GetZadani(int A, Q B, Q C, Q D, Q E, Op x, Op y) => new (A, B.Copy(), C.Copy(), D.Copy(), E.Copy(), x, y);

		static bool VysledekNaleziDoMnozinyEasyZlomky(Q D, Q E, Op y) {
			Q right = y == Op.Add ? D + E : D - E;
			right.Inverse();
			right.Reduce(); // in case of negative Qs, this is nessecary
			return IsEasyZt(right);
		}

		/// 
		/// Kuchařka řešení: Jak se zadání řeší?
		/// 

		protected override Excercise Construct(Zadani_Fractions_S02_C z) { 
			int A = z.A;
			Q B = z.B.Copy();
			Q C = z.C.Copy();
			Q D = z.D.Copy();
			Q E = z.E.Copy();
			Op opX = z.opA;
			Op opY = z.opB;

			string[] steps = new string[7];
			string[] comments = new string[7];
			int rightLCD = M.EuclidsLCM(D.Den, E.Den);

			string x = Repr(opX);
			string y = Repr(opY);

			// Step 1:
			steps[0] = $"( {A} {x} {B} ) ∙ {C} : ( {D} {y} {E} )";
			comments[0] = $"V levé závorce rozšiř číslo {A} na {XtinyCesky(B.Den)}.<br>V pravé závorce rozšiř {XtinyCesky(D.Den)} a {XtinyCesky(E.Den)} na {XtinyCesky(rightLCD)}.";

			// Step 2:
			Q F = new (A * B.Den, B.Den);
			D.ExpandToLCD( E );

			steps[1] = $"( {F} {x} {B} ) ∙ {C} : ( {D} {y} {E} )";
			comments[1] = "Sečti/odečti zlomky v závorkách.";

			// Step 3:
			F = F.Operate(B, opX);
			D = D.Operate(E, opY);
			steps[2] = $"{F} ∙ {C} : {D}";
			comments[2] = $"Vykrať {F.Num} a {F.Den}.";

			// Step 4:
			F.Num = 1; F.Den = 1; C.Num = 1; C.Den = 1;
			steps[3] = $"{F} ∙ {C} : {D}";
			comments[3] = $"Dělení dvou zlomků převeď na násobení.";

			// Step 5:
			// make illegal cases not cause runtime math hell excpetion
			if(D.Num == 0) {
				comments[4] = $">> RUNTIME MATH HELL EXCEPTION AVOIDED<<";
				return new EFractions_S02(steps, comments, D);	
			}
			D.Inverse();
			
			steps[4] = $"{F} ∙ {D}";
			comments[4] = $"Vynásob jedničku.";

			// Step 6:
			steps[5] = $"{D}";
			comments[5] = $"Výsledný zlomek převeď na jeho základní tvar.";

			// Step 7:
			D.Reduce();
			steps[6] = $"{D}";
			comments[6] = "Hotovo! 😎😎";
			return new EFractions_S02(steps, comments, D);	
		}
	}
}
