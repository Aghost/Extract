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

            foreach (string str in drr.Index) { WriteLine($"{str}"); }

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
                        // TODO Length

                        drr.PrintRecords(startch, endch);
                        break;
                    default: break;
                }
            }
        }
    }
}
