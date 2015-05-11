using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace P2
namespace P2
{
    public static class Constants
    {
        public const long MAX_K = 100;
        public const long MAX_L = 100;
        public const long MAX_S = 100;
    }

    public interface ISolver
    {
        double Solve(string keys, long numTimes, string targetWord);
    }

    public static class Utils
    {
        public static bool CanType(string keys, string word)
        {
            bool[] hasKeys = new bool[Char.MaxValue];

            foreach (var ch in keys)
            {
                hasKeys[ch] = true;
            }

            foreach (var ch in word)
            {
                if (!hasKeys[ch])
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        public double Solve(string keys, long numTimes, string targetWord)
        {
            if (!Utils.CanType(keys, targetWord))
            {
                return 0;
            }

            var outcomes = GenOutcomes(keys, numTimes).Select(w => CountBananas(w, targetWord)).ToArray();
            var maxBananas = outcomes.Max();
            var avgBananas = outcomes.Average();
            // Console.WriteLine("Max = {0} Avg = {1}", maxBananas, avgBananas);
            return maxBananas - avgBananas;
        }

        public IEnumerable<string> GenOutcomes(string keys, long numTimes)
        {
            long n = keys.Length;
            long maxCombo = (long)Math.Pow(n, numTimes);
            char[] word = new char[numTimes];

            for (long i = 0; i < maxCombo; i++)
            {
                long wordNum = i;
                for (long j = 0; j < numTimes; j++)
                {
                    word[j] = keys[(int)(wordNum % n)];
                    wordNum /= n;
                }

                string s = new string(word);
                // Console.WriteLine(s);
                yield return s;
            }
        }

        public long CountBananas(string outcome, string targetWord)
        {
            long match = 0;
            int targetLen = targetWord.Length;

            for (int i = 0; i <= outcome.Length - targetLen; i++)
            {
                if (outcome.Substring(i, targetLen) == targetWord)
                {
                    ++match;
                }
            }

            return match;
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {
        public double Solve(string keys, long numTimes, string targetWord)
        {
            if (!Utils.CanType(keys, targetWord))
            {
                return 0;
            }

            int numKeys = keys.Length;

            var charDict = keys.GroupBy(k => k).Select(g => new { Key = g.Key, Count = g.Count() })
                .ToDictionary(nvp => nvp.Key, nvp => nvp.Count);

            long maxBananas = CalcMaxBananas(targetWord, numTimes);

            return 0;   // FINISH
        }

        public int CalcLongestPrefixIdx(string s)
        {
            for (int i = 1; i < s.Length; i++)
            {
                if (s.StartsWith(s.Substring(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        public long CalcMaxBananas(string s, long numTimes)
        {
            var prefix = CalcLongestPrefixIdx(s);

            if (s.Length > numTimes)
            {
                return 0;
            }

            if (prefix == -1)
            {
                return numTimes / s.Length;
            }

            long left = numTimes - s.Length;
            long suffixLen = s.Length - prefix + 1;
            return 1 + left / suffixLen;
        }

        public double CalcWordProbability(IDictionary<char, int> charDict, int numKeys, string word)
        {
            double p = 1;

            foreach (var ch in word)
            {
                int count;
                if (!charDict.TryGetValue(ch, out count))
                {
                    return 0;
                }

                p *= (double)count / numKeys;
            }

            return p;
        }
    }

    class Program
    {
        #region changeonce

        static string mSmallIn = @"
100
5 2 4
YLSRO
VN
7 7 7
YIKPUGV
IUPIYPP
1 1 1
A
A
2 1 2
PG
G
6 7 7
YCIRTE
RRIECIE
4 6 7
PLFW
PFFPFF
7 2 3
LLLUNLL
LL
2 2 7
KQ
QK
4 6 6
LEED
DLLDEE
7 2 6
FFFFFEF
FF
1 1 1
F
F
1 3 3
S
ASS
7 1 7
GGGGRGS
G
6 2 2
GOOGLE
GO
2 1 1
KG
K
5 4 4
IIKNT
IKTK
2 3 4
AA
AAA
6 3 3
QPBFYU
QFP
7 2 4
QWQQQQI
QQ
7 1 2
FFHFOFF
F
3 1 5
AIV
I
7 4 7
RQBAGVN
AQVA
4 6 7
BRJB
JJBRBJ
7 1 5
KZKZZUK
K
7 7 7
YYYYYYY
YYYYYYY
5 6 6
IUPMU
UMMIUM
7 2 5
CCCCEPC
CC
6 6 6
NRLLPI
LLNINR
2 2 6
KN
NK
7 3 3
RRRHRRR
RRR
6 3 3
BKMVOD
MBK
7 3 3
CZZZZZZ
ZZZ
5 6 7
JANMM
MMMJMM
2 1 2
AB
B
7 1 1
QKBYAUG
Y
3 1 7
ODC
D
7 7 7
RRRRRRR
RRRRRRR
4 2 6
JYQZ
ZQ
7 1 6
UUUUMOX
U
2 6 7
QQ
QQQQQQ
4 7 7
HBUA
HBHABBU
7 2 3
AAJAAUA
AA
7 7 7
FFFFFFF
FFFFFFF
5 1 2
OSFXA
H
1 1 1
G
G
1 2 2
Y
GK
7 2 2
PIFBLAM
PP
3 1 6
RPT
T
3 2 5
VOD
DO
1 1 1
S
A
5 4 6
ZPMFS
PSSF
1 3 5
V
VVV
4 1 2
ANTU
A
2 2 5
ZV
YB
6 1 3
BYTCNO
C
7 5 7
JZNWQKX
JKQZX
7 7 7
SSSSSSS
SSSSSSS
7 7 7
FFFFFFF
FFFFFFF
7 7 7
DDDDDDD
DDDDDDD
4 6 7
ZPLD
LPZPPZ
2 5 7
CD
CCCCC
4 1 7
BMPW
I
1 6 6
I
LOTRPG
1 1 2
B
B
7 1 5
JJKJJGJ
J
7 1 4
LNQDIQD
D
2 2 4
ZG
FE
7 4 7
XNOWNBJ
NXNX
7 3 7
RRRRRVR
RRR
7 3 6
ZXXYWLJ
XYW
5 2 2
NMLDH
HN
7 1 2
MLMMMSM
M
2 7 7
WB
BBBBBBB
7 2 2
DHUGGXX
UG
3 3 6
FRI
RII
7 7 7
BBBBBBB
BBBBBBB
7 3 3
EPKBAMQ
KEM
4 7 7
HTVO
TTOOTTO
6 3 7
PPMGVL
MPM
4 1 7
XLSL
L
6 2 6
KNCPDJ
JK
7 3 7
MYFKNLU
UNU
6 2 4
CLSFZE
LL
1 4 4
A
AAAA
7 1 1
KDCDDDD
D
6 4 7
EMOFQF
MHPF
7 6 6
BANANAS
MONKEY
5 3 7
MLAYU
MLM
3 2 5
BNO
FK
3 2 5
ZQE
EE
7 7 7
QFRXOYW
WWWYXWW
6 3 3
JWBBEM
QHZ
3 5 6
CLE
ITVNX
6 6 6
IWJPIQ
IIQQPP
2 4 5
OD
ODOD
4 2 7
WLCJ
JJ
7 3 6
PZPPZXY
PZP
4 7 7
ASJD
ASDJASD
7 2 5
DNONNNN
NN
5 2 7
MOORY
OO
";

        static string mSmallOut = @"
Case #1: 0.0000000
Case #2: 0.9999988
Case #3: 0.0000000
Case #4: 1.0000000
Case #5: 0.9999964
Case #6: 0.9995117
Case #7: 0.9795918
Case #8: 1.5000000
Case #9: 0.9990234
Case #10: 1.3265306
Case #11: 0.0000000
Case #12: 0.0000000
Case #13: 2.0000000
Case #14: 0.8888889
Case #15: 0.5000000
Case #16: 0.9968000
Case #17: 0.0000000
Case #18: 0.9953704
Case #19: 1.4693878
Case #20: 0.5714286
Case #21: 3.3333333
Case #22: 1.9983340
Case #23: 0.9980469
Case #24: 2.8571429
Case #25: 0.0000000
Case #26: 0.9997440
Case #27: 1.9591837
Case #28: 0.9999143
Case #29: 1.7500000
Case #30: 0.3702624
Case #31: 0.9953704
Case #32: 0.3702624
Case #33: 0.9959040
Case #34: 1.0000000
Case #35: 0.8571429
Case #36: 4.6666667
Case #37: 0.0000000
Case #38: 2.6875000
Case #39: 2.5714286
Case #40: 0.0000000
Case #41: 0.9999390
Case #42: 0.9795918
Case #43: 0.0000000
Case #44: 0.0000000
Case #45: 0.0000000
Case #46: 0.0000000
Case #47: 0.9795918
Case #48: 4.0000000
Case #49: 1.5555556
Case #50: 0.0000000
Case #51: 0.9952000
Case #52: 0.0000000
Case #53: 1.5000000
Case #54: 0.0000000
Case #55: 2.5000000
Case #56: 0.9998215
Case #57: 0.0000000
Case #58: 0.0000000
Case #59: 0.0000000
Case #60: 0.9995117
Case #61: 2.9062500
Case #62: 0.0000000
Case #63: 0.0000000
Case #64: 0.0000000
Case #65: 1.4285714
Case #66: 2.8571429
Case #67: 0.0000000
Case #68: 1.9933361
Case #69: 1.8513120
Case #70: 1.9766764
Case #71: 0.9600000
Case #72: 0.5714286
Case #73: 0.9921875
Case #74: 0.9591837
Case #75: 1.8518519
Case #76: 0.0000000
Case #77: 0.9970845
Case #78: 0.9999390
Case #79: 2.9537037
Case #80: 3.5000000
Case #81: 2.8611111
Case #82: 2.9854227
Case #83: 2.9166667
Case #84: 0.0000000
Case #85: 0.2857143
Case #86: 0.0000000
Case #87: 0.0000000
Case #88: 2.9600000
Case #89: 0.0000000
Case #90: 3.5555556
Case #91: 0.9999988
Case #92: 0.0000000
Case #93: 0.0000000
Case #94: 0.9999143
Case #95: 0.8750000
Case #96: 5.6250000
Case #97: 1.7900875
Case #98: 0.9999390
Case #99: 1.9591837
Case #100: 5.0400000
";


        static string mSampleIn = @"
4
7 6 6
BANANAS
MONKEY
2 3 4
AA
AAA
2 1 2
AB
B
6 2 2
GOOGLE
GO
";

        static string mSampeOut = @"
Case #1: 0.0000000
Case #2: 0.0000000
Case #3: 1.0000000
Case #4: 0.8888889
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            long timeStart = DateTime.Now.Ticks;

            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                var line1 = ReadLine(txtIn).Split(' ').Select(s => long.Parse(s)).ToArray();
                var keys = ReadLine(txtIn);
                var targetWord = ReadLine(txtIn);

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(keys, line1[2], targetWord);

                // Prints output
                txtOut.WriteLine("Case #{0}: {1:F7}", i, ans);
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
