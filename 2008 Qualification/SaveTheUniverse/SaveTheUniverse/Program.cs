using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace SaveTheUniverse
namespace SaveTheUniverse
{
    public static class Consts
    {
        public const int NUM_ENGINES = 10;
        public const int NUM_QUERIES = 100;
    };

    public interface ISolver
    {
        int Solve(string[] searchEngines, string[] queries);
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        public int Solve(string[] searchEngines, string[] queries)
        {
            int numSwitch = 0;

            // Loop Invariant
            // queries[0 .. i-1] is not in validEngines
            HashSet<string> validEngines = new HashSet<string>(searchEngines);
            for (int i = 0; i < queries.Length; i++)
            {
                validEngines.Remove(queries[i]);
                if (validEngines.Count == 0)
                {
                    ++numSwitch;
                    validEngines = new HashSet<string>(searchEngines);
                    --i;
                }
            }
            
            return numSwitch;
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {
        public int Solve(string[] searchEngines, string[] queries)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        #region changeonce

        static string mSmallIn = @"
1
1
1 junk
";

        static string mSmallOut = @"
Case #1: 10
";


        static string mSampleIn = @"
2
5
Yeehaw
NSM
Dont Ask
B9
Googol
10
Yeehaw
Yeehaw
Googol
B9
Googol
NSM
B9
NSM
Dont Ask
Googol
5
Yeehaw
NSM
Dont Ask
B9
Googol
7
Googol
Dont Ask
NSM
NSM
Yeehaw
Yeehaw
Googol
";

        static string mSampeOut = @"
Case #1: 1
Case #2: 0
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            long timeStart = DateTime.Now.Ticks;

            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                string[] engines = ReadStrings(txtIn);
                string[] queries = ReadStrings(txtIn);

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(engines, queries);

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
