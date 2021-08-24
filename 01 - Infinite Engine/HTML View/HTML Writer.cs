using System;
using static System.Console;
using System.Collections.Generic;
using InfiniteEngine;
using System.Text;
using System.IO;

namespace CLI_View_of_Infinite_Engine_library {
    class Program {
        static void Main() {
            WriteLine("Hello World!");
            /* Uncomment to get eight /
            EGenerator_Fractions_S02E01 egen = new();
            Excercise[] excercises = egen.GetEight();

            HTMLWriter hw = new();
            hw.CreateFile($"InfiniteEngine_Fraction_S0201_GetEight.html", excercises);
            /**/

            /* Uncomment to get all ! Dangerous creates 112 MB file /
            EGenerator_Fractions_S02E01 egen = new();
            Excercise[] excercises = egen.GetAll();

            HTMLWriter hw = new();
            hw.CreateFile($"InfiniteEngine_Fraction_S0201_GetAll.html", excercises);
            /**/

            /**/
            Dificulty[] levels = new Dificulty[5] { Dificulty.MENSI, Dificulty.PRIJIMACKY, Dificulty.VETSI, Dificulty.OBROVSKA, Dificulty.CPU };
            foreach(Dificulty d in levels) {
                Console.WriteLine($"Initiating Writin Dificulty: {d}");
                EGenerator_Fractions_S02E01 egen = new(d);
                List<Excercise> excercises = new();
                for (int i = 0; i < 25; i++)
                    foreach (Excercise e in egen.GetEight())
                        excercises.Add(e);

                Excercise[] arrayExcercises = excercises.ToArray();

                HTMLWriter hw = new();
                hw.CreateFile($"InfiniteEngine-Fraction-S0201-GetRandom200-Level-{d}.html", arrayExcercises);
            }
            /**/

            /*/
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

    class HTMLWriter {
        string header = @"
<!doctype html>
<html lang=""cs"">
<head>
    <title>💙🔭🧬 Kuchařka řešení - zlomky</title>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
    <link rel=""shortcut icon"" type=""image/x-icon"" href=""favicon.ico"">
    <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
    <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
    <link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&display=swap"" rel=""stylesheet"">

    <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
    <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
    <link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&family=Indie+Flower&display=swap"" rel=""stylesheet"">
    <link href=""styles.css"" rel=""stylesheet"" type=""text/css""/>

</head>
<body>
    <div id=""nadpis"">
        <h1> Kuchařka řešení</h1>
        <h2>Vypočítej a výsledek napiš jako zlomek v základním tvaru</h2>
    </div>
    
    <div id=""table-container"">
        <table class=""table-fill"">
            <thead>
                <tr>
                    <th width=""150px"" height=""80px"">Krok</th>
                    <th>Výraz</th>
                    <th width=""32%""> Popis následující úpravy</th>
                    <th>Izolovaná úprava</th>
                </tr>
            </thead>
            <tbody id=""tableBody""> 
";

        string headerTableLess = @"<!doctype html><html lang=""cs""><head><title>💙🔭🧬 Kuchařka řešení - zlomky</title><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no""><link rel=""shortcut icon"" type=""image/x-icon"" href=""favicon.ico""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&display=swap"" rel=""stylesheet""><link rel=""preconnect"" href=""https://fonts.googleapis.com""><link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin><link href=""https://fonts.googleapis.com/css2?family=Caveat:wght@500&family=Indie+Flower&display=swap"" rel=""stylesheet""><link href=""styles.css"" rel=""stylesheet"" type=""text/css""/></head><body><div id=""nadpis""><h1> Kuchařka řešení</h1><h2>Vypočítej a výsledek napiš jako zlomek v základním tvaru</h2></div><div class=""table-container"">";

        // id=""table-container""
        string TableHeader = @"<table class=""table-fill""><thead><tr><th width=""100px"" height=""80px"">Krok</th><th width=""14%"">Výraz</th><th width=""42%""> Popis následující úpravy</th><th>Izolovaná úprava</th></tr></thead><tbody class=""tableBody""> ";

        string TableFooter = "</tbody></table>";

        string footerTableLess = "</div></body></html>";

        string footer = @"
            </tbody>
        </table>
    </div>
</body>
</html>
";

        static string GenerateBody(Excercise e) {
            StringBuilder sb = new();
            for(int i = 0; i < e.Steps.Length; i++) {
                sb.Append("<tr><td>" + i + @".</td><td class=""text-blue"">" + e.Steps[i].ToHTML() + "</td>");
                sb.Append(@"<td class=""text-green text-left"">" + e.Comments[i] + @"</td><td class=""text-blue""></td></tr>");
            }
            return sb.ToString();
        }

        public HTMLWriter(Excercise e) {
            StringBuilder sb = new();
            sb.Append(header);
            sb.Append(GenerateBody(e));
            sb.Append(footer);
            file = sb.ToString();
        }

        public HTMLWriter() { }

        string file;

        public void CreateFile(string filePath) {
            using StreamWriter sw = new(filePath);
            sw.Write(file);
        }

        public void CreateFile(string filePath, Excercise[] excercises) {
            StringBuilder sb = new();
            sb.Append(headerTableLess);
            int counter = 0;
            foreach (Excercise e in excercises) {
                counter++;
                sb.Append($"<h2>Příklad číslo: {counter}</h2>");
                sb.Append(TableHeader);
                for (int i = 0; i < e.Steps.Length; i++) {
                    sb.Append("<tr><td>" + i + @".</td><td class=""text-blue"">" + e.Steps[i].ToHTML() + "</td>");
                    sb.Append(@"<td class=""text-green text-left"">" + e.Comments[i] + @"</td><td class=""text-blue"">" + e.IsolatedModifications[i] + "</td></tr>");
                }
                sb.Append(TableFooter);
            }
            sb.Append(footerTableLess);
            using StreamWriter sw = new(filePath);
            sw.Write( sb.ToString() );
        }
    }
}
