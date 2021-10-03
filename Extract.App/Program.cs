using System;
using Extract.Core;

using static System.Console;

namespace Extract.App
{
    class Program
    {
        static void Main(string[] args)
        {
            DataRepository drr = new("../../../0_LIBRARY/0_UNSORTED");
            //DataRepository drr = new("../../../0_LIBRARY");

            WriteLine("(type 'help' to list cmds)");

            string userinput = "";
            while(userinput != "exit") {
                Write("/>: ");
                userinput = ReadLine();

                switch (userinput) {
                    case "search":
                        Write($"startchar: ");
                        char startch = ReadKey().KeyChar;
                        WriteLine();
                        Write($"endchar: ");
                        char endch = ReadKey().KeyChar;
                        WriteLine();
                        Write($"length: ");
                        if (int.TryParse(ReadLine(), out int tmp)) {
                            drr.PrintRecords(startch, endch, tmp);
                        } else {
                            drr.PrintRecords(startch, endch);
                        }

                        break;
                    case "tobuffer":
                        Write($"startchar: ");
                        char startch2 = ReadKey().KeyChar;
                        WriteLine();
                        Write($"endchar: ");
                        char endch2 = ReadKey().KeyChar;
                        WriteLine();
                        Write($"length: ");
                        if (int.TryParse(ReadLine(), out int tmp2)) {
                            drr.ToBuffer(startch2, endch2, tmp2);
                            WriteLine("check ?buffer");
                        } else {
                            WriteLine("argument error");
                        }
                        break;
                    case "?records":
                        if (AskQ($"Are you sure? (records: {drr.Records.Count})")) { drr.PrintAllRecords(); }
                        break;
                    case "?buffer":
                        if (AskQ($"Are you sure? ")) { drr.PrintBuffer(); }
                        break;
                    case "index":
                        if (AskQ($"Are you sure? (files: {drr.Index.Length})")) { foreach (string str in drr.Index) { WriteLine($"{str}"); } }
                        break;
                    case "help":
                        WriteLine("search | tobuffer | ?records | ?buffer | index | help | exit");
                        break;
                    default:
                        break;
                }
            }
        }

        static bool AskQ(string question) {
            WriteLine($"{question}(y/n)");
            return ReadLine() == "y";
        }
    }
}
