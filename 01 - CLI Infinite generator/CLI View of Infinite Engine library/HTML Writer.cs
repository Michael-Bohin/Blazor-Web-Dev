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

            EGenerator_Fractions_S01E01 egen = new();

            for (int i = 0; i < 10; i++) {
                HTMLWriter hw = new(egen.GetNext());
                hw.CreateFile($"InfiniteEngine{i}.html");
            }

            WriteLine("Job done. :)");
        }
    }

    class HTMLWriter {
        string header = @"
<!doctype html>
<html lang=""cs"">
<head>
    <title>💚💙🔭🧬 Kuchařka řešení - zlomky</title>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
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

        string footer = @"
            </tbody>
        </table>
    </div>
</body>
</html>
";

        static string GenerateBody(Excercise e) {
            StringBuilder sb = new();
            for(int i = 0; i < e.steps.Length; i++) {
                sb.Append("<tr><td>" + i + @".</td><td class=""text-blue"">" + e.steps[i].ToHTML() + "</td>");
                sb.Append(@"<td class=""text-green"">" + e.comments[i] + @"</td><td class=""text-blue""></td></tr>");
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

        string file;

        public void CreateFile(string filePath) {
            using StreamWriter sw = new(filePath);
            sw.Write(file);
        }
    }
}
