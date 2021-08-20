using System;
using static System.Console;
using System.Collections.Generic;
using InfiniteEngine;

namespace Development_ConsoleView {
    class Program {
        static void Main() {
            WriteLine("What shall the console do?");
            PrintMenu();

            int option = Int32.Parse(ReadLine().Trim());
            switch(option) {
                case 1 : 
                    EnumerateAllFractions(); break;
                default:
                    WriteLine("Sorry didn't find any match. Bye."); break;
            }
        }

        private static void EnumerateAllFractions() {
            WriteLine("What inclisuve lower and upper bound shall I set for numerator and denominator?");

            string bounds = ReadLine().Trim();
            string[] parsedBounds = bounds.Split();
            if(parsedBounds.Length != 2) {
                WriteLine("Sorry expected to get two integeres, exiting run.");
                return;
            }

            int boundA = Int32.Parse(parsedBounds[0]);
            int boundB = Int32.Parse(parsedBounds[1]);

            int lowerBound = Math.Min(boundA, boundB);
            int higherBound = Math.Max(boundA, boundB);

            FractionsInSimplestForm fsf = new();
            List<Fraction> result = fsf.GetAll(lowerBound, higherBound);

            int counter = 0;
            foreach(Fraction f in result) {
                counter++;
                WriteLine(counter + " >> " + f.ToString());
            }
        }

        static void PrintMenu() {
            WriteLine(@"
                Option '1' : Enumerate CSG.AllFractionsInSimplestForm(int from, int to)
            ");
        }
    }

    
}
