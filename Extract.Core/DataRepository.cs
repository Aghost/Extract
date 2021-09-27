using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Extract.Core.Types;

namespace Extract.Core
{
    public class DataRepository
    {
        public string Root { get; private set; }
        public string[] Index { get; private set; }
        public char[][] Data { get; private set; }
        public List<DataRecord> Records { get; set; }

        public DataRepository(string Root) {
            this.Root = Root;
            this.Index = CreateRootIndex(Root);
            this.Data = LoadData(Index);

            Records = CreateRecordIndex(Data, Index);
            Console.WriteLine("Load DataRepository --- Done");
            Console.WriteLine($"Records: {Records.Count}");
        }

        private static string[] CreateRootIndex(string dir, string ext = "*") {
            return Directory.GetFiles(dir, $"*.{ext}", SearchOption.AllDirectories);
        }

        private static char[][] LoadData(string[] index) {
            char[][] dataset = new char[index.Length][];

            Parallel.For(0, index.Length, i => {
                dataset[i] = String.Join("\n", File.ReadAllLines(index[i])).ToCharArray();
            });

            return dataset;
        }

        public void PrintRecords(char startc, char endc) {
            Console.Write("PrintRecords?(y) :");
            if (Console.ReadLine() == "y") {
                foreach(DataRecord dr in Records.Where(r => r.StartChar == startc && r.EndChar == endc)) {
                    PrintData(dr);
                }

            }
        }

        public void PrintData(DataRecord datarecord) {
            Console.WriteLine($"{datarecord.FileIndex} {datarecord.StartIndex} {datarecord.Length} {datarecord.StartChar} {datarecord.EndChar}");
            for (int i = 0; i < datarecord.Length; i++) {
                Console.Write(this.Data[datarecord.FileIndex][datarecord.StartIndex + i]);
            }
            Console.WriteLine();
        }

        private static List<DataRecord> CreateRecordIndex(char[][] data, string[] fileindex) {
            List<DataRecord> results = new();

            for (int i = 0; i < data.Length; i++) {
                int len = data[i].Length;
                int s = 0, ls = 0, le = 0;  // start, line/wordstart, line/wordend

                for (; le < len; le++) {
                    if (data[i][le] == ' ' || data[i][le] == '\n' || data[i][le] == '\t') {
                        s = ls;
                        ls = le + 1;

                        if (s != le) {
                            results.Add(new DataRecord{ FileIndex = i, StartIndex = s, Length = le - s, StartChar = data[i][s], EndChar = data[i][s + le - s - 1] });
                        }
                    }
                }

                int laststart = 0;

                for (;s < len; s++) {
                    if (data[i][s] == ' ')
                        laststart = s;
                    if (s == le - 1) {
                        results.Add(new DataRecord{ FileIndex = i, StartIndex = s, Length = len - laststart - 1, StartChar = data[i][len - laststart - 1], EndChar = data[i][len - 1]});
                    }
                }
            }

            return results;
        }

    }
}
