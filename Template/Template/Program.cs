using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// namespace Template
namespace $safeprojectname$
{
    public interface ISolver
    {
        int Solve(int p);
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        public int Solve(int p)
        {
            if (p == 2)
            {
                return 15;
            }
            else
            {
                return p * 10;
            }
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {
        public int Solve(int p)
        {
            return p * 10;
        }
    }

    class Program
    {
        #region changeonce

        static string mSampleIn = @"
3
1
junk
2
junk
3
junk
";

        static string mSampeOut = @"
Case #1: 10
Case #2: 20
Case #3: 30
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                var line1 = txtIn.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
                var line2 = txtIn.ReadLine().Split(' ').ToArray();

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(line1[0]);

                // Prints output
                txtOut.WriteLine("Case #{0}: {1}", i, ans);
            }
        }

        #endregion

        #region fixed

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
                txtIn = new StringReader(mSampleIn);
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
                CompareOutputs(mSampeOut, ((StringWriter)txtOut).ToString());
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
