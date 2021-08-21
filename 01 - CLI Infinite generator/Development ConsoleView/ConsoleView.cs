using System;
using static System.Console;
using System.Collections.Generic;
using InfiniteEngine;

namespace Development_ConsoleView {
    class ConsoleView {
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
            FractionsInSimplestForm fsf = new();
            /* WriteLine("What inclisuve lower and upper bound shall I set for numerator and denominator?");

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
             }*/
            List<Fraction> mnozinaB = new();
            mnozinaB.Add( new Fraction(1, 10));
            mnozinaB.Add(new Fraction(1, 5));
            mnozinaB.Add(new Fraction(1, 4));
            mnozinaB.Add(new Fraction(3, 10));
            mnozinaB.Add(new Fraction(2, 5));
            mnozinaB.Add(new Fraction(1, 2));
            mnozinaB.Add(new Fraction(3, 5));
            mnozinaB.Add(new Fraction(3, 4));
            mnozinaB.Add(new Fraction(4, 5 ));
            mnozinaB.Add(new Fraction(9, 10));

            List<Fraction> mnozinaA = fsf.GetAll(1, 10, mnozinaB);

            WriteLine("Vypis mnoziny A:");
            int counter = 0;
            foreach (Fraction f in mnozinaA) {
                counter++;
                WriteLine(counter + " >> " + f.ToString());
            }

            WriteLine("Vypis mnoziny B:");
            counter = 0;
            foreach (Fraction f in mnozinaB) {
                counter++;
                WriteLine(counter + " >> " + f.ToString());
            }
            List<Fraction> neomezenaMnozinaC = fsf.GetAll(11, 40);

            Integer F, Iden;
            int Bden, Cden;

            counter = 0;
            foreach ( Fraction f in mnozinaB) {
                WriteLine("Vypis mnoziny C pri volbe zlomku: " + f.ToString());
                
                F = f.Denominator as Integer;
                Bden = F.number;

                foreach(Fraction candidate in neomezenaMnozinaC) {
                    Iden = candidate.Denominator as Integer;
                    Cden = Iden.number;
                    if(Cden % Bden == 0) {
                        counter++;
                        WriteLine(counter + " >> " + candidate.ToString());
                    }
                }
            }
        }

        static void PrintMenu() {
            WriteLine(@"
                Option '1' : Enumerate CSG.AllFractionsInSimplestForm(int from, int to)
            ");
        }
    }

    
}
