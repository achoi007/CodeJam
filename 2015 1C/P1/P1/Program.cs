using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace P1
namespace P1
{
    public static class Constants
    {
        public const long MAX_ROW = 1;
        public const long MAX_COL = 10;
    }

    public interface ISolver
    {
        long Solve(long row, long col, long width);
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {

        public long Solve(long row, long col, long width)
        {
            long n = row * (col / width);

            if (col % width == 0)
            {
                n += width - 1;
            }
            else
            {
                n += width;
            }

            return n;
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {
        public long Solve(long row, long col, long width)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        #region changeonce

        static string mSmallIn = @"
55
1 4 2
1 7 7
1 9 9
1 5 1
1 10 3
1 4 3
1 3 1
1 7 3
1 8 8
1 10 9
1 9 8
1 10 4
1 9 7
1 10 5
1 8 4
1 2 1
1 7 5
1 9 6
1 1 1
1 8 2
1 10 1
1 6 5
1 7 2
1 10 2
1 8 6
1 5 2
1 8 5
1 7 6
1 10 8
1 6 2
1 5 4
1 3 2
1 4 1
1 6 1
1 7 1
1 3 3
1 9 2
1 5 3
1 9 1
1 8 1
1 6 3
1 5 5
1 9 5
1 4 4
1 6 4
1 9 3
1 10 6
1 2 2
1 8 7
1 7 4
1 9 4
1 10 7
1 8 3
1 10 10
1 6 6
";

        static string mSmallOut = @"
Case #1: 3
Case #2: 7
Case #3: 9
Case #4: 5
Case #5: 6
Case #6: 4
Case #7: 3
Case #8: 5
Case #9: 8
Case #10: 10
Case #11: 9
Case #12: 6
Case #13: 8
Case #14: 6
Case #15: 5
Case #16: 2
Case #17: 6
Case #18: 7
Case #19: 1
Case #20: 5
Case #21: 10
Case #22: 6
Case #23: 5
Case #24: 6
Case #25: 7
Case #26: 4
Case #27: 6
Case #28: 7
Case #29: 9
Case #30: 4
Case #31: 5
Case #32: 3
Case #33: 4
Case #34: 6
Case #35: 7
Case #36: 3
Case #37: 6
Case #38: 4
Case #39: 9
Case #40: 8
Case #41: 4
Case #42: 5
Case #43: 6
Case #44: 4
Case #45: 5
Case #46: 5
Case #47: 7
Case #48: 2
Case #49: 8
Case #50: 5
Case #51: 6
Case #52: 8
Case #53: 5
Case #54: 10
Case #55: 6
";


        static string mSampleIn = @"
7
1 4 2
1 7 7
2 5 1
1 21 3
1 20 3
1 22 3
1 23 3
";

        static string mSampeOut = @"
Case #1: 3
Case #2: 7
Case #3: 10
Case #4: 9
Case #5: 9
Case #6: 10
Case #7: 10
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            long timeStart = DateTime.Now.Ticks;

            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                var line1 = ReadLine(txtIn).Split(' ').Select(s => long.Parse(s)).ToArray();

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(line1[0], line1[1], line1[2]);

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
