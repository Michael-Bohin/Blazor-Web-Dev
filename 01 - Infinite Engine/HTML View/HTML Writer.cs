using System;
using static System.Console;
using System.Collections.Generic;
using InfiniteEngine;
using System.Text;
using System.IO;

namespace CLI_View_of_Infinite_Engine_library
{
	class Program
	{
		static void Main() {
			WriteLine("Hello World!");
			/*/
            Dificulty[] levels = new Dificulty[5] { Dificulty.MENSI, Dificulty.PRIJIMACKY, Dificulty.VETSI, Dificulty.OBROVSKA, Dificulty.CPU };
            foreach(Dificulty d in levels) {
                Console.WriteLine($"Initiating Writing Dificulty: {d}");
                EGenerator_Fractions_S02E01 egen = new(d);
                List<Excercise> excercises = new();
                for (int i = 0; i < 5; i++)
                    foreach (Excercise e in egen.GetTen())
                        excercises.Add(e);

                HTMLWriter.CreateFile($"InfiniteEngine-Fraction-S0201-v2-Final-100-Collection-{d}.html", excercises.ToArray());
            }
            /**/

			/**/
			Console.WriteLine($"Initiating Writing Fractions S02 A:");
			EGenerator_Fractions_S02_A egen = new();
			using StreamWriter sw = new("stats-log-Fractions-S02-A.txt");
			sw.Write(egen.stats);

			List<Excercise> eList = new();
			eList = egen.GetIllegal(0, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-A-version-0-4-illegal-1.html", eList.ToArray());

			eList = egen.GetIllegal(1, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-A-version-0-4-illegal-2.html", eList.ToArray());

			eList = egen.GetIllegal(2, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-A-version-0-4-illegal-3.html", eList.ToArray());

			eList = egen.GetIllegal(3, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-A-version-0-4-illegal-4.html", eList.ToArray());

			eList = egen.GetLegit(200);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-A-version-0-4-legit.html", eList.ToArray());

			/*

			Console.WriteLine($"Initiating Writing Fractions S02 B:");
			EGenerator_Fractions_S02_B egen = new();
			using StreamWriter sw = new("stats-log-Fractions-S02-B.txt");
			sw.Write(egen.stats);
			List<Excercise> eList = new();

			// 0  vede na nesmysl ktery pada na deleni nulou
			//eList = egen.GetIllegal(0, 50);
			//HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-1.html", eList.ToArray());

			//eList = egen.GetIllegal(1, 50);
			//HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-2.html", eList.ToArray());
			
			WriteLine("init write index 2:");
			eList = egen.GetIllegal(2, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-2.html", eList.ToArray());


			WriteLine("init write index 3:");
			eList = egen.GetIllegal(3, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-3.html", eList.ToArray());

			/*WriteLine("init write index 4:");
			eList = egen.GetIllegal(4, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-4.html", eList.ToArray());

			WriteLine("init write index 5:");
			eList = egen.GetIllegal(5, 50);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-illegal-5.html", eList.ToArray());*/
			/*
			WriteLine("init write legit excercises:");
			eList = egen.GetLegit(200);
			HTMLWriter.CreateFileBriefExcercies2($"InfiniteEngine-Fractions-S02-B-version-0-1-LEGIT.html", eList.ToArray());

			/** /
            EGenerator_Fractions_S02E01 egen = new();
            List<Excercise> excercises = new();

            Fraction A = new(1, 3);
            Fraction B = new(1, 2);
            Fraction C = new(23, 26);
            bool plus = false;
            Excercise e = egen.UnsafeGetExactlyThis(A,B,C,plus);
            excercises.Add(e);

            Excercise[] arrayExcercises = excercises.ToArray();
            HTMLWriter hw = new();
            hw.CreateFile($"InfiniteEngine-Fraction-S0201-DebugConcreteExcercise-01.html", arrayExcercises);
            /**/
			WriteLine("Job done. :)");
		}
	}

	class HTMLWriter
	{
		const string headerTableLess = @"<!doctype html><html lang=""cs""><head><title>💙🔭🧬 Kuchařka řešení - zlomky</title><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no""><link rel=""shortcut icon"" type=""image/x-icon"" href=""favicon.ico""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&display=swap"" rel=""stylesheet""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&family=Indie+Flower&display=swap"" rel=""stylesheet""><link href=""styles.css"" rel=""stylesheet"" type=""text/css""/></head><body><div id=""nadpis""><h1> Kuchařka řešení</h1><h2>Vypočítej a výsledek napiš jako zlomek v základním tvaru</h2></div><div class=""table-container"">";
		const string TableHeader = @"<table class=""table-fill""><thead><tr><th width=""100px"" height=""80px"">Krok</th><th width=""14%"">Výraz</th><th width=""42%""> Popis následující úpravy</th><th>Izolovaná úprava</th></tr></thead><tbody class=""tableBody""> ";
		const string TableFooter = "</tbody></table>";
		const string footerTableLess = "</div></body></html>";

		//const string TableHeaderBrief = @"<table class=""table-fill""><thead><tr><th></th></tr></thead><tbody class=""tableBody""> ";

		public HTMLWriter() {
		}

		public static void CreateFile(string filePath, Excercise[] excercises) {
			StringBuilder sb = new();
			sb.Append(headerTableLess);
			int counter = 0;
			foreach (Excercise e in excercises) {
				counter++;
				sb.Append($"<h2>Příklad číslo: {counter}</h2>");
				sb.Append(TableHeader);
				for (int i = 0; i < e.Steps.Length; i++) {
					sb.Append("<tr><td>" + i + @".</td><td class=""text-blue"">" + e.Steps[i] + "</td>");
					sb.Append(@"<td class=""text-green text-left"">" + e.Comments[i] + @"</td><td class=""text-blue"">" + e.IsolatedModifications[i] + "</td></tr>");
				}
				sb.Append(TableFooter);
			}
			sb.Append(footerTableLess);
			using StreamWriter sw = new(filePath);
			sw.Write(sb.ToString());
		}

		public static void CreateFileBriefExcercies(string filePath, Excercise[] excercises) {
			StringBuilder sb = new();
			sb.Append(headerTableLess);
			int counter = 0;
			foreach (Excercise e in excercises) {
				counter++;
				sb.Append($"<h2>Příklad číslo: {counter}</h2>");
				sb.Append(TableHeader);
				for (int i = 0; i < e.Steps.Length; i++) {
					sb.Append("<tr><td>" + i + @".</td><td class=""text-blue"">" + e.Steps[i] + "</td>");
					sb.Append(@"<td class=""text-green text-left"">" + e.Comments[i] + @"</td><td class=""text-blue"">Tady by byli izolovane upravy</td></tr>");
				}
				sb.Append(TableFooter);
			}
			sb.Append(footerTableLess);
			using StreamWriter sw = new(filePath);
			sw.Write(sb.ToString());
		}

		public static void CreateFileBriefExcercies2(string filePath, Excercise[] excercises) {
			StringBuilder sb = new();
			sb.Append(headerTableLess);
			int counter = 0;
			foreach (Excercise e in excercises) {
				counter++;
				sb.Append($"<h2>Příklad číslo: {counter}</h2>");
				sb.Append(@"<table class=""table-fill""> ");

				sb.Append(@"<tbody class=""tableBody""><tr><td colspan=""2"">");
				for (int i = 0; i < e.Steps.Length - 1; i++)
					sb.Append(e.Steps[i] + " = ");
				sb.Append(e.Steps[^1]);
				sb.Append("</td></tr>");

				for (int i = 0; i < e.Comments.Length; i++)
					sb.Append(@"<tr><td>Krok " + i + @":</td><td class=""text-green text-left"">" + e.Comments[i] + @"</td></tr>");

				sb.Append(TableFooter);
			}
			sb.Append(footerTableLess);
			using StreamWriter sw = new(filePath);
			sw.Write(sb.ToString());
		}
	}
}