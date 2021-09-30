using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Extract.Core.Types;

namespace Extract.Core
{
    public class DataRepository
    {
        private Stopwatch sw = new();

        public string Root { get; private set; }
        public string[] Index { get; private set; }
        public char[][] Data { get; private set; }
        public List<DataRecord> Records { get; set; }
        public List<DataRecord> Buffer { get; set; }

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
            foreach(DataRecord dr in Records.Where(r => r.StartChar == startc && r.EndChar == endc)) {
                PrintData(dr);
            }
        }

        public void PrintRecords(char startc, char endc, int len) {
            foreach(DataRecord dr in Records.Where(r => r.StartChar == startc && r.EndChar == endc && r.Length == len)) {
                PrintData(dr);
            }
        }

        public void PrintAllRecords() {
            foreach(DataRecord dr in Records) {
                PrintData(dr);
            }
        }

        public void PrintData(DataRecord datarecord) {
            Console.Write($"filename: {Index[datarecord.FileIndex]}\n[");
            for (int i = 0; i < datarecord.Length; i++) {
                Console.Write(this.Data[datarecord.FileIndex][datarecord.StartIndex + i]);
            }

            Console.WriteLine($"]\n{datarecord.FileIndex} {datarecord.StartIndex} {datarecord.Length} {datarecord.StartChar} {datarecord.EndChar}");
        }

        public void ToBuffer(char startc, char endc, int len, int findex = -1) {
            Buffer = new();

            if (findex != -1) {
                foreach(DataRecord dr in Records.Where(r => r.FileIndex == findex && r.StartChar == startc && r.EndChar == endc && r.Length == len)) {
                    Buffer.Add(dr);
                }
            } else {
                foreach(DataRecord dr in Records.Where(r => r.StartChar == startc && r.EndChar == endc && r.Length == len)) {
                    Buffer.Add(dr);
                }
            }
        }

        public void PrintBuffer() {
            foreach (DataRecord dr in Buffer) {
                PrintData(dr);
            }
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

                        if (s != le && le - s > 1) {
                            results.Add(new DataRecord{ FileIndex = i, StartIndex = s, Length = le - s, StartChar = data[i][s], EndChar = data[i][s + le - s - 1] });
                        }
                    }
                }

                for (;s < len; s++) {
                    if (data[i][s] == ' ')
                        results.Add(new DataRecord{ FileIndex = i, StartIndex = s + 1, Length = len - 1 - s, StartChar = data[i][len - s], EndChar = data[i][len - 1]});
                }
            }

            return results;
        }

    }
}
