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

        long LCMTime { get; }

        long HaircutsPerLCM { get; }
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

        static long lcm(IEnumerable<long> nums)
        {
            return nums.Aggregate(1L, (s, n) => lcm(s, n));
        }

        public long Reduce(long pos, long[] barberTimes)
        {
            LCMTime = lcm(barberTimes);

            // How many haircuts can be done in lcm time
            HaircutsPerLCM = barberTimes.Sum(b => LCMTime / b);

            
            pos %= HaircutsPerLCM;
            if (pos == 0)
            {
                pos = HaircutsPerLCM;
            }
            return pos;
        }


        public long LCMTime
        {
            get;
            private set;
        }

        public long HaircutsPerLCM
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        IPositionReducer mReducer = new LCMPosReducer();

        public long Solve(long pos, long[] barberTimes)
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
                        // number.  Returns it
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
            long numBarbers = barberTimes.Length;

            // More barbers than position, done
            if (pos <= numBarbers)
            {
                return pos;
            }

            // Binary searches for time T s.t. Sum(haircuts, < lowerT) <= pos <= Sum(haircuts, > upperT) or pos == Sum(haircuts, mid)
            long lower = 0;
            long upper = pos * 100000 / barberTimes.Length + 1;  // each barber takes at most 100,000 seconds per haircut
            long found = -1;
            while (lower < upper)
            {
                long mid = (upper + lower) / 2;
                long haircuts = barberTimes.Sum(b => mid / b);
                if (haircuts > pos)
                {
                    upper = mid - 1;
                }
                else if (haircuts < pos)
                {
                    lower = mid + 1;
                }
                else
                {
                    found = mid;
                    break;
                }
            }

            if (found == -1)
            {
                found = lower;
            }

            long numCust = barberTimes.Sum(b => (found - 1) / b);
            for (int i = 0; i < numBarbers; i++)
            {
                    // If i-th barber is done, add to customer count
                    if (found % barberTimes[i] == 0)
                    {
                        numCust++;
                        // If customer count is same as position, we know the barber
                        // number.  Returns it
                        if (numCust == pos)
                        {
                            return i + 1;   // Barber array starts at 0, barber number starts at 1
                        }
                    }                
            }

            // Not reached
            return -1;
        }
    }


    class Program
    {
        #region changeonce

        static string mSampleIn = @"
100
2 4
10 5
3 12
7 7 7
3 12
4 2 1
5 1000000000
25 25 25 25 25
2 2
1 1
5 5
1 1 1 1 1
5 448257424
1 2 3 4 5
5 915742169
18 7 21 11 25
5 714724053
10 10 10 10 10
5 846031768
5 5 5 5 5
5 268362882
10 10 10 10 10
5 507616061
18 18 18 18 18
5 867527160
5 5 5 5 5
5 14292117
7 7 7 7 7
5 113197432
12 12 12 12 12
5 434802157
15 15 15 15 15
5 875208637
12 12 12 12 12
5 598204019
23 23 23 23 23
5 930580339
15 15 15 15 15
5 548399485
6 6 6 6 6
5 850830825
21 21 21 21 21
5 68635284
11 11 11 11 11
5 218118272
17 17 17 17 17
5 975015708
9 9 9 9 9
5 568217814
15 15 15 15 15
5 91214437
12 12 12 12 12
5 567536380
18 18 18 18 18
5 686402411
12 12 12 12 12
5 861220169
16 16 16 16 16
5 98744025
22 22 22 22 22
5 725932411
18 18 18 18 18
5 736789750
15 15 15 15 15
5 893661536
18 18 18 18 18
5 276728745
1 2 3 4 5
5 735092037
1 2 3 4 5
5 970782493
1 2 3 4 5
5 960105183
1 2 3 4 5
5 936094217
1 2 3 4 5
5 491661373
1 2 3 4 5
5 388340171
1 2 3 4 5
5 929323319
1 2 3 4 5
5 534319488
1 2 3 4 5
5 935754473
1 2 3 4 5
5 48051188
1 2 3 4 5
5 732211233
1 2 3 4 5
5 652956242
1 2 3 4 5
5 599818572
1 2 3 4 5
5 181647206
1 2 3 4 5
5 246093995
1 2 3 4 5
5 448257424
1 2 3 4 5
5 949115035
1 2 3 4 5
5 631832946
1 2 3 4 5
5 457274500
1 2 3 4 5
5 681288758
1 2 3 4 5
5 816033640
1 2 3 4 5
5 395488494
1 2 3 4 5
5 917991491
1 2 3 4 5
5 415665899
1 2 3 4 5
5 111039833
3 3 21 3 8
5 131782772
7 14 1 5 8
5 273543505
15 20 8 12 3
5 171594523
20 7 13 17 5
5 838725204
4 12 10 4 15
5 928336683
19 1 25 16 5
5 474571493
17 17 8 15 7
5 970734369
20 7 24 7 3
5 951913332
1 13 7 7 23
5 655189487
12 11 16 2 2
5 43325616
17 10 21 10 20
5 338108004
10 15 4 14 14
5 398824749
17 2 8 20 8
5 718625295
14 9 1 20 24
5 807907739
19 15 11 9 20
5 655921600
13 20 23 24 9
5 299856520
21 17 2 23 1
5 831136075
15 10 8 7 6
5 891369626
9 21 17 14 5
5 351000218
9 21 19 10 15
5 154351384
1 22 12 9 14
5 734737029
14 10 20 15 7
5 34925476
24 9 15 14 3
5 709270299
7 5 22 19 14
5 607263928
23 4 24 13 6
5 180925441
6 20 7 12 25
5 905209519
12 15 5 19 12
5 605449653
14 4 11 22 25
5 419481524
10 8 24 7 23
5 164940752
25 9 24 6 2
5 915742169
18 7 21 11 25
5 739811016
11 20 10 18 23
5 536085944
14 13 6 2 16
5 606081776
11 18 14 9 16
5 82652731
14 23 8 22 2
5 274743668
6 24 15 14 21
5 754779150
19 20 12 25 14
5 455025787
3 23 17 9 1
5 140758232
11 12 21 15 3
5 483932340
14 13 14 24 24
5 494171035
21 5 8 22 20
5 229887443
13 22 19 7 2
";

        static string mSampeOut = @"
Case #1: 1
Case #2: 3
Case #3: 2
Case #4: 5
Case #5: 2
Case #6: 5
Case #7: 1
Case #8: 4
Case #9: 3
Case #10: 3
Case #11: 2
Case #12: 1
Case #13: 5
Case #14: 2
Case #15: 2
Case #16: 2
Case #17: 2
Case #18: 4
Case #19: 4
Case #20: 5
Case #21: 5
Case #22: 4
Case #23: 2
Case #24: 3
Case #25: 4
Case #26: 2
Case #27: 5
Case #28: 1
Case #29: 4
Case #30: 5
Case #31: 1
Case #32: 5
Case #33: 1
Case #34: 1
Case #35: 1
Case #36: 1
Case #37: 4
Case #38: 1
Case #39: 1
Case #40: 1
Case #41: 2
Case #42: 1
Case #43: 2
Case #44: 1
Case #45: 1
Case #46: 2
Case #47: 2
Case #48: 2
Case #49: 5
Case #50: 1
Case #51: 1
Case #52: 1
Case #53: 3
Case #54: 1
Case #55: 3
Case #56: 3
Case #57: 2
Case #58: 2
Case #59: 1
Case #60: 3
Case #61: 1
Case #62: 2
Case #63: 2
Case #64: 2
Case #65: 2
Case #66: 3
Case #67: 1
Case #68: 4
Case #69: 4
Case #70: 1
Case #71: 2
Case #72: 4
Case #73: 4
Case #74: 4
Case #75: 5
Case #76: 5
Case #77: 2
Case #78: 1
Case #79: 1
Case #80: 5
Case #81: 5
Case #82: 2
Case #83: 3
Case #84: 3
Case #85: 2
Case #86: 2
Case #87: 4
Case #88: 5
Case #89: 4
Case #90: 1
Case #91: 4
Case #92: 2
Case #93: 2
Case #94: 1
Case #95: 2
Case #96: 1
Case #97: 1
Case #98: 3
Case #99: 2
Case #100: 1
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
