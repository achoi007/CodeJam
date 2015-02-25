using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam.RopeIntranet
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input and output filenames
            string inputFilename, outputFilename;
            if (args.Length != 2)
            {
                Console.Write("Input filename: ");
                inputFilename = Console.ReadLine();
                Console.Write("Output filename: ");
                outputFilename = Console.ReadLine();
            }
            else
            {
                inputFilename = args[0];
                outputFilename = args[1];
            }

            // Open input and output files and solve
            using (var input = File.OpenText(inputFilename))
            {
                using (var output = new StreamWriter(File.OpenWrite(outputFilename)))
                {
                    Solve(input, output);
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static void Solve(TextReader input, TextWriter output)
        {
            int numCases = int.Parse(input.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                var ropes = ReadRopes(input);
                var ans = ComputeCrossings(ropes);
                output.WriteLine("Case #{0}: {1}", i, ans);
            }
        }

        static Tuple<int, int>[] ReadRopes(TextReader input)
        {
            int numRopes = int.Parse(input.ReadLine());
            Tuple<int, int>[] ropes = new Tuple<int, int>[numRopes];
            char[] sep = new char[] { ' ' };
            for (int i = 0; i < numRopes; i++)
            {
                var cols = input.ReadLine().Split(sep, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s)).ToArray();
                ropes[i] = Tuple.Create(cols[0], cols[1]);
            }
            return ropes;
        }

        static int ComputeCrossings(Tuple<int, int>[] ropes)
        {
            int ropesLen = ropes.Length;
            int[] sums = new int[ropesLen];
            Parallel.For(0, ropes.Length, i =>
            {
                var rope = ropes[i];
                for (int j = 0; j < ropesLen; j++)
                {
                    int diff1 = rope.Item1 - ropes[j].Item1;
                    int diff2 = rope.Item2 - ropes[j].Item2;

                    if (diff1 == 0 || diff2 == 0)
                    {
                        continue;
                    }
                    else if (Math.Sign(diff1) != Math.Sign(diff2))
                    {
                        sums[i]++;
                    }
                }
            });

            return sums.Sum() / 2;
        }
    }
}
