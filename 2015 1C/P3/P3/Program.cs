using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace P3
namespace P3
{
    public static class Constants
    {
        public const long MAX_C = 1;
        public const long MAX_D = 5;
        public const long MAX_V = 30;

    }

    public interface ISolver
    {
        long Solve(int maxCoins, long[] coins, long maxValue);
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        public long Solve(int maxCoins, long[] coins, long maxValue)
        {
            Array.Sort(coins);
            HashSet<long> fillables = new HashSet<long>();

            // Bootstraps fillables from 1 coin
            for (int numCoins = 1; numCoins <= maxCoins; numCoins++)
            {
                long val = numCoins * coins[0];
                if (val <= maxValue)
                {
                    fillables.Add(val);
                }
            }

            // Adds each subsequent coins 
            FindFillables(maxCoins, coins.Skip(1), maxValue, fillables);

            long numNewCoins = 0;
            for (long val = 1; val <= maxValue; val++)
            {
                if (!fillables.Contains(val))
                {
                    FindFillables(maxCoins, new long[] { val }, maxValue, fillables);
                    ++numNewCoins;
                }
            }

            return numNewCoins;
        }

        public static void FindFillables(int maxCoins, IEnumerable<long> coins, long maxValue, ISet<long> fillables)
        {
            // Finds all fillable values by linear combination of existing coins
            foreach (var coin in coins)
            {
                HashSet<long> additions = new HashSet<long>();

                for (int numCoin = 1; numCoin <= maxCoins; numCoin++)
                {
                    foreach (var fill in fillables)
                    {
                        long newCoinVal = numCoin * coin;
                        if (newCoinVal > maxValue)
                        {
                            continue;
                        }
                        additions.Add(newCoinVal);

                        long newValue = fill + newCoinVal;
                        if (newValue <= maxValue && !additions.Contains(newValue))
                        {
                            additions.Add(newValue);
                        }
                    }
                }

                foreach (var addend in additions)
                {
                    fillables.Add(addend);
                }
            }
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {

        public long Solve(int maxCoins, long[] coins, long maxValue)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        #region changeonce

        static string mSmallIn = @"
100
1 3 22
13 19 20
1 2 30
1 14
1 3 22
3 7 13
1 2 5
3 4
1 2 24
4 9
1 3 29
1 6 10
1 1 24
3
1 4 29
20 22 27 29
1 1 6
4
1 3 17
3 5 11
1 4 14
1 3 9 10
1 1 3
2
1 2 27
3 7
1 1 7
1
1 2 29
2 6
1 5 17
9 11 13 15 16
1 1 27
5
1 2 20
1 3
1 1 25
22
1 1 4
3
1 1 8
3
1 1 30
1
1 1 4
4
1 1 14
14
1 1 19
3
1 2 21
4 7
1 4 16
1 2 4 8
1 3 6
1 2 5
1 3 18
2 7 11
1 1 30
30
1 2 11
4 8
1 3 22
13 18 22
1 1 4
1
1 4 7
1 4 6 7
1 1 1
1
1 2 28
9 10
1 5 5
1 2 3 4 5
1 3 13
1 9 10
1 2 5
2 3
1 1 5
5
1 2 27
3 7
1 3 27
13 14 18
1 1 5
1
1 2 11
10 11
1 1 8
4
1 2 16
4 9
1 3 9
2 8 9
1 2 30
11 18
1 1 2
1
1 1 2
1
1 1 30
29
1 5 30
1 2 4 8 15
1 3 11
9 10 11
1 3 26
6 23 25
1 5 16
1 2 4 8 16
1 1 15
5
1 3 28
2 5 12
1 1 22
1
1 3 21
4 5 10
1 2 20
4 8
1 3 23
11 19 23
1 2 29
13 16
1 2 22
2 22
1 3 29
8 18 25
1 3 21
10 14 20
1 2 11
9 11
1 2 24
3 24
1 3 23
9 13 19
1 3 22
5 10 20
1 1 30
2
1 4 17
8 12 15 17
1 3 25
11 18 24
1 3 20
5 10 18
1 1 4
1
1 1 10
10
1 3 16
13 15 16
1 4 25
4 5 13 25
1 1 23
3
1 2 3
1 2
1 4 26
5 22 24 26
1 2 29
1 4
1 2 29
17 24
1 2 25
4 13
1 2 20
14 18
1 5 25
15 17 21 23 24
1 2 10
3 6
1 3 16
2 6 11
1 2 4
3 4
1 2 21
13 21
1 4 22
2 5 11 20
1 2 18
4 9
1 2 26
16 18
1 1 7
1
1 3 12
3 5 11
1 2 26
5 9
1 3 19
2 14 15
1 1 30
1
1 1 12
3
1 1 25
4
1 2 30
19 26
";

        static string mSmallOut = @"
Case #1: 4
Case #2: 4
Case #3: 2
Case #4: 2
Case #5: 3
Case #6: 3
Case #7: 4
Case #8: 5
Case #9: 2
Case #10: 2
Case #11: 2
Case #12: 1
Case #13: 3
Case #14: 2
Case #15: 4
Case #16: 4
Case #17: 5
Case #18: 3
Case #19: 5
Case #20: 2
Case #21: 3
Case #22: 4
Case #23: 2
Case #24: 4
Case #25: 4
Case #26: 3
Case #27: 1
Case #28: 1
Case #29: 2
Case #30: 5
Case #31: 2
Case #32: 4
Case #33: 2
Case #34: 1
Case #35: 0
Case #36: 4
Case #37: 0
Case #38: 3
Case #39: 1
Case #40: 3
Case #41: 3
Case #42: 4
Case #43: 2
Case #44: 4
Case #45: 3
Case #46: 3
Case #47: 2
Case #48: 4
Case #49: 1
Case #50: 1
Case #51: 5
Case #52: 0
Case #53: 4
Case #54: 4
Case #55: 0
Case #56: 4
Case #57: 3
Case #58: 4
Case #59: 2
Case #60: 3
Case #61: 4
Case #62: 4
Case #63: 4
Case #64: 4
Case #65: 4
Case #66: 4
Case #67: 4
Case #68: 4
Case #69: 3
Case #70: 4
Case #71: 3
Case #72: 4
Case #73: 3
Case #74: 2
Case #75: 4
Case #76: 4
Case #77: 2
Case #78: 4
Case #79: 0
Case #80: 4
Case #81: 3
Case #82: 5
Case #83: 3
Case #84: 4
Case #85: 4
Case #86: 2
Case #87: 2
Case #88: 2
Case #89: 4
Case #90: 2
Case #91: 3
Case #92: 4
Case #93: 2
Case #94: 2
Case #95: 4
Case #96: 3
Case #97: 4
Case #98: 3
Case #99: 4
Case #100: 5
";


        static string mSampleIn = @"
3
1 2 3
1 2
1 3 6
1 2 5
2 1 3
3
";

        static string mSampeOut = @"
Case #1: 0
Case #2: 1
Case #3: 1
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            long timeStart = DateTime.Now.Ticks;

            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                var line1 = ReadLine(txtIn).Split(' ').Select(s => long.Parse(s)).ToArray();
                var line2 = ReadLine(txtIn).Split(' ').Select(s => long.Parse(s)).ToArray();

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve((int)line1[0], line2, line1[2]);

                // Prints output
                txtOut.WriteLine("Case #{0}: {1}", i, ans);
            }

            long timeEnd = DateTime.Now.Ticks;
            var elapsed = TimeSpan.FromTicks(timeEnd - timeStart);
            Console.WriteLine("Elapsed time: {0} min {1} sec", elapsed.Minutes, elapsed.Seconds);
        }

        #endregion

        #region fixed

        static string[] ReadStrings(TextReader txtIn)
        {
            int numStrs = int.Parse(ReadLine(txtIn));
            string[] strs = new string[numStrs];
            for (int i = 0; i < numStrs; i++)
            {
                strs[i] = ReadLine(txtIn);
            }
            return strs;
        }

        static string ReadLine(TextReader txtIn)
        {
            string line;

            do
            {
                line = txtIn.ReadLine();
            }
            while (line != null && line.StartsWith("!"));

            return line;
        }

        static ISolver CreateSolver(IDictionary<string, string> options)
        {
            ISolver solver = null;

            if (options.ContainsKey("solver"))
            {
                string solverName = options["solver"];
                if (solverName == "brute")
                {
                    solver = new BruteSolver();
                }
                else if (solverName == "smart")
                {
                    solver = new SmartSolver();
                }
            }

            if (solver == null)
            {
                Console.WriteLine("Defaulting to brute solver");
                solver = new BruteSolver();
            }

            return solver;
        }

        static void Main(string[] args)
        {
            // Parses options
            Dictionary<string, string> options;
            args = ParseOptions(args, out options);

            // Gets source of reader and writer
            TextReader txtIn;
            TextWriter txtOut;

            if (options.ContainsKey("test"))
            {
                txtIn = new StringReader(options["test"] == "small" ? mSmallIn : mSampleIn);
                txtIn.ReadLine();   // Skips initial blank line
                txtOut = new StringWriter();
            }
            else if (args.Length != 2)
            {
                Console.WriteLine("Reading from standard input");
                txtIn = Console.In;
                txtOut = Console.Out;
            }
            else
            {
                Console.WriteLine("Reading from file");
                txtIn = File.OpenText(args[0]);

                string outFilename = args[1];
                if (File.Exists(outFilename))
                {
                    File.Delete(outFilename);
                }
                txtOut = new StreamWriter(File.OpenWrite(outFilename));
            }

            // Selects solver
            ISolver solver = CreateSolver(options);

            // Processes text input to generate output
            Process(txtIn, txtOut, solver);
            txtOut.Close();
            txtIn.Close();

            // If in test mode, compares output to sample output
            if (options.ContainsKey("test"))
            {
                Console.WriteLine("Comparing results");
                CompareOutputs(options["test"] == "small" ? mSmallOut : mSampeOut,
                    ((StringWriter)txtOut).ToString());
            }

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();
        }

        static void Warning(string fmt, params object[] p)
        {
            var origColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(fmt, p);
            Console.ForegroundColor = origColor;
        }

        /// <summary>
        /// Compares expected output to actual output and generates warnings on diffs
        /// </summary>
        /// <param name="expectedStr"></param>
        /// <param name="actualStr"></param>
        static void CompareOutputs(string expectedStr, string actualStr)
        {
            // Separates expected and actual output into lines
            var lineSep = new[] { '\r', '\n' };
            var expected = expectedStr.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);
            var actual = actualStr.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);

            // Checks output line count
            if (expected.Length != actual.Length)
            {
                Warning("Diff in num of lines - expected {0} vs actual {1}", expected.Length, actual.Length);
            }
            int numLines = Math.Min(expected.Length, actual.Length);

            // Performs line by line comparison
            bool OK = true;
            for (int i = 0; i < numLines; i++)
            {
                if (expected[i] != actual[i])
                {
                    Warning("Expected: {0} vs Actual {1}", expected[i], actual[i]);
                    OK = false;
                }
            }

            if (OK)
            {
                Console.WriteLine("All tests passed");
            }
        }

        private static string[] ParseOptions(string[] args, out Dictionary<string, string> options)
        {
            int argPos;
            options = new Dictionary<string, string>();

            for (argPos = 0; argPos < args.Length; argPos++)
            {
                // Done if not an option parameter
                if (!args[argPos].StartsWith("-"))
                {
                    break;
                }

                // Extracts option name and optional value and stores into options dictionary
                string optName = args[argPos].Substring(1).ToLower();
                string optVal = "1";
                switch (optName)
                {
                    case "solver":
                    case "test":
                        ++argPos;
                        optVal = args[argPos];
                        break;
                    default:
                        break;
                }
                options.Add(optName, optVal);
            }

            return args.Skip(argPos).ToArray();
        }

        #endregion
    }
}
