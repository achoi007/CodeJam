using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// namespace P2
namespace P2
{
    public interface ISolver
    {
        long Solve(long pos, long[] barberTimes);
    }

    public interface IPositionReducer
    {
        /// <summary>
        /// P is large, reduce P to P' so that
        ///     P = P' + k * Sum(haircuts within LCM(barber times))
        /// P' will have the same barber number as P
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="barberTimes"></param>
        /// <returns></returns>
        long Reduce(long pos, long[] barberTimes);
    }

    public class LCMPosReducer : IPositionReducer
    {
        static long gcd(long a, long b)
        {
            if (b > a)
            {
                long t = b;
                b = a;
                a = t;
            }

            while (b != 0)
            {
                long t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return a * b / gcd(a, b);
        }

        static long lcm(long[] nums)
        {
            return nums.Aggregate(1L, (s, n) => lcm(s, n));
        }

        public long Reduce(long pos, long[] barberTimes)
        {
            long lcmTime = lcm(barberTimes);

            // How many haircuts can be done in lcm time
            long haircutsPerLcmTime = barberTimes.Sum(b => lcmTime / b);

            
            pos %= haircutsPerLcmTime;
            if (pos == 0)
            {
                pos = haircutsPerLcmTime;
            }
            return pos;
        }
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        IPositionReducer mReducer = new LCMPosReducer();

        public virtual long Solve(long pos, long[] barberTimes)
        {
            pos = mReducer.Reduce(pos, barberTimes);

            long numBarbers = barberTimes.Length;

            // More barbers than position, done
            if (pos <= numBarbers)
            {
                return pos;
            }

            // At t=0, number of customers already with haircut + number of customers having
            // haircut done is number of barbers
            long numCust = numBarbers;

            // Start at t=1
            long now = 1;
            while (true)
            {
                for (int i = 0; i < numBarbers; i++)
                {
                    // If i-th barber is done, add to customer count
                    if (now % barberTimes[i] == 0)
                    {
                        numCust++;
                        // If customer count is same as position, we know the barber
                        // number.  Return it
                        if (numCust == pos)
                        {
                            return i + 1;   // Barber array starts at 0, barber number starts at 1
                        }
                    }
                }
                ++now;
            }
        }
    }

    public class SmartSolver : ISolver
    {
        public long Solve(long pos, long[] barberTimes)
        {
            throw new NotImplementedException();
        }
    }


    class Program
    {
        #region changeonce

        static string mSampleIn = @"
16
2 4
10 5
3 12
7 7 7
3 8
4 2 1
3 12
4 2 1
2 2
1 1
3 21
1 2 4
3 22
1 2 4
3 23
1 2 4
3 24
1 2 4
3 25
1 2 4
3 26
1 2 4
3 27
1 2 4
3 28
1 2 4
3 29
1 2 4
3 30
1 2 4
3 31
1 2 4
";

        static string mSampeOut = @"
Case #1: 1
Case #2: 3
Case #3: 1
Case #4: 2
Case #5: 2
Case #6: 1
Case #7: 1
Case #8: 2
Case #9: 3
Case #10: 1
Case #11: 1
Case #12: 2
Case #13: 1
Case #14: 1
Case #15: 2
Case #16: 3
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                var line1 = txtIn.ReadLine().Split(' ').Select(s => long.Parse(s)).ToArray();
                var line2 = txtIn.ReadLine().Split(' ').Select(s => long.Parse(s)).ToArray();

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(line1[1], line2);

                // Prlongs output
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
            // Separates expected and actual output longo lines
            var lineSep = new[] { '\r', '\n' };
            var expected = expectedStr.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);
            var actual = actualStr.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);

            // Checks output line count
            if (expected.Length != actual.Length)
            {
                Warning("Diff in num of lines - expected {0} vs actual {1}", expected.Length, actual.Length);
            }
            long numLines = Math.Min(expected.Length, actual.Length);

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

                // Extracts option name and optional value and stores longo options dictionary
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
