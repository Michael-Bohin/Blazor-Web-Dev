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

			EGenerator_Fractions_S02_E egen = new();
			Process(egen, "E", 5, 2_000, "2000x LargeFile");

			WriteLine("Job done. :)");
		}

		static void Process(ExcerciseGenerator<Zadani_Fractions_S02_E> egen, string episode, int illegalCount, int legitCount, string version) {
			
			WriteLine($"Initiating Writing Fractions S02 {episode}:");
			using StreamWriter sw = new($"stats-log-Fractions-S02-{episode}.txt");
			sw.Write(egen.stats);
			sw.Dispose();

			List<Excercise> eList = new();
			/*for(int i = 0; i < egen.illegalSetsCount; i++) {
				eList = egen.GetIllegal(i, illegalCount);
				HTMLWriter.CreateFileBriefExcercise($"InfiniteEngine-Fractions-S02-{episode}-{version}-illegal-{i+1}.html", eList.ToArray());
			}*/

			eList = egen.GetLegit(legitCount);
			HTMLWriter.CreateFileBriefExcercise($"Fractions S02 {version}-{episode}.html", eList.ToArray());
		}
	}

	class HTMLWriter
	{	// blazor time not necessary part: <!doctype html><html lang=""cs""><head><title>💙🔭🧬 Kuchařka řešení - zlomky</title><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no""><link rel=""shortcut icon"" type=""image/x-icon"" href=""favicon.ico""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&display=swap"" rel=""stylesheet""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&family=Indie+Flower&display=swap"" rel=""stylesheet""><link href=""styles.css"" rel=""stylesheet"" type=""text/css""/></head><body>
		const string headerTableLess = @"<!doctype html><html lang=""cs""><head><title>💙🔭🧬 Kuchařka řešení - zlomky</title><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no""><link rel=""shortcut icon"" type=""image/x-icon"" href=""favicon.ico""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&display=swap"" rel=""stylesheet""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&family=Indie+Flower&display=swap"" rel=""stylesheet""><link href=""styles.css"" rel=""stylesheet"" type=""text/css""/></head><body><div id=""nadpis""><h1> Kuchařka řešení</h1><h2>Vypočítej a výsledek napiš jako zlomek v základním tvaru</h2></div><div class=""table-container"">";
		const string TableHeader = @"<table class=""table-fill""><thead><tr><th width=""100px"" height=""80px"">Krok</th><th width=""14%"">Výraz</th><th width=""42%""> Popis následující úpravy</th><th>Izolovaná úprava</th></tr></thead><tbody class=""tableBody""> ";
		const string TableFooter = "</tbody></table>";
		const string footerTableLess = "</div></body></html>"; // </body></html>

		public HTMLWriter() { }

		public static void CreateFileDetailedExcercise(string filePath, Excercise[] excercises) {
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

		public static void CreateFileBriefExcercise(string filePath, Excercise[] excercises) {
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