using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MyAdjacencyList;
using static System.Console;

namespace CS_B02
{
    class Program
    {
        static AdjacencyList ReadAdjacencyList(string filepath, out int source, out int target)
        {
            using (var sr = new StreamReader(filepath))
            {
                // Allocations
                var list = new AdjacencyList();
                var s1 = sr.ReadLine().Split(' ');
                list.Vertices = int.Parse(s1[0]);
                source = int.Parse(s1[1]) - 1;
                target = int.Parse(s1[2]) - 1;
                list.List = new LinkedList<int>[list.Vertices];

                // Read matrix
                for (int i = 0; i < list.Vertices; i++)
                {
                    var s = sr.ReadLine().Split(' ');

                    var tmp = new LinkedList<int>();
                    foreach (string e in s)
                    {
                        // catch the error if line content is not integers
                        if (int.TryParse(e, out int tmp2) == false)
                            if (tmp2 == 0)
                                continue;
                        tmp.AddLast(--tmp2);
                    }

                    list.List[i] = tmp;
                }

                return list;
            }
        }

        static int[] FindTheWayBFS(AdjacencyList list, int source, int target)
        {
            // Allocations
            var q = new Queue<int>();
            var checkpoint = new bool[list.Vertices];
            var pre = Enumerable.Repeat(-1, list.Vertices).ToArray();

            // Traverse and trace
            q.Enqueue(source);
            checkpoint[source] = true;
            while (q.Count != 0)
            {
                source = q.Dequeue();
                foreach (var item in list.List[source])
                {
                    if (checkpoint[item])
                        continue;
                    checkpoint[item] = true;
                    pre[item] = source;
                    q.Enqueue(item);
                }
            }
            return pre;
        }

        static void Print(int[] pre, int source, int target)
        {
            if (pre[target] == -1)
                Write($"There is no way from {source} to {target}.");
            else
            {
                List<int> theWay = new List<int>();

                theWay.Add(target);
                int i = target;
                while (i != source)
                {
                    theWay.Add(pre[i]);
                    i = pre[i];
                }

                theWay.Reverse();
                var sb = new StringBuilder();
                sb.Append(theWay.Count);
                sb.AppendLine();
                Write("The way: ");
                foreach (var item in theWay)
                {
                    Write($"{item + 1} ");
                    sb.Append(item + 1); sb.Append(' ');
                }

                WriteResult(sb);
            }
        }

        static void WriteResult(StringBuilder sb)
        {
            using (var sw = new StreamWriter("..//..//timduongdibfs.out"))
            {
                sw.Write(sb.ToString());
            }
        }

        static void Main(string[] args)
        {
            var list = ReadAdjacencyList("..//..//timduongdibfs.inp", out int source, out int target);
            WriteLine(list);
            WriteLine($"Source peak: {source + 1}");
            WriteLine($"Target peak: {target + 1}");


            Print(FindTheWayBFS(list, source, target), source, target);
        }
    }
}
