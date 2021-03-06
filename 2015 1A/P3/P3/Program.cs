﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace P3
namespace P3
{
    public static class Utils
    {
        public const int MAX_COORD = 1000000;
        public const int MAX_POINTS = 3000;

        /// <summary>
        /// > 0 if counterclockwise
        /// < 0 if clockwise
        /// == 0 if colinear
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CCW(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X);
        }

        public static int DistSq(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }
    }

    public class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public double Angle { get; set; }

        public override int GetHashCode()
        {
            return X * Utils.MAX_COORD + Y;
        }

        public override bool Equals(object obj)
        {
            Point rhs = (Point)obj;
            return X == rhs.X && Y == rhs.Y;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }
    }

    public interface IConvexHull
    {
        IList<Point> Calc(IEnumerable<Point> points, bool isSorted);

        void Sort(Point[] points);
    }

    public class GrahamScan : IConvexHull
    {
        class GSComparer : IComparer<Point>
        {
            private Point mRefPoint;

            public GSComparer(Point refPoint)
            {
                mRefPoint = refPoint;
            }

            public int Compare(Point a, Point b)
            {
                int ccw = Utils.CCW(mRefPoint, a, b);
                if (ccw == 0)
                {
                    return Utils.DistSq(mRefPoint, a) - Utils.DistSq(mRefPoint, b);
                }
                else
                {
                    return -ccw;
                }
            }
        }


        public IList<Point> Calc(IEnumerable<Point> givenPoints, bool isSorted)
        {
            // Sorts points if not sorted already
            Point[] points = givenPoints.ToArray();
            if (!isSorted)
            {
                Sort(points);
            }

            // Creates hull. Automatically adds first 2 points to hull
            Stack<Point> hull = new Stack<Point>();
            hull.Push(points[0]);
            hull.Push(points[1]);
            hull.Push(points[2]);

            Func<Point> belowTop = () => hull.ElementAt(hull.Count - 2);

            for (int i = 3; i < points.Length; i++)
            {
                while (hull.Count >= 3 && Utils.CCW(belowTop(), hull.Peek(), points[i]) < 0)
                {
                    hull.Pop();
                }
                hull.Push(points[i]);
            }

            // Returns hull
            return hull.ToList();
        }

        public void Sort(Point[] points)
        {
            // Finds point with lowest Y coordinate then lowest X coordinate if same Y.
            int minIdx = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].Y < points[minIdx].Y)
                {
                    minIdx = i;
                }
                else if (points[i].Y == points[minIdx].Y && points[i].X < points[minIdx].X)
                {
                    minIdx = i;
                }
            }

            // Makes points[0] the ref point
            var t = points[0];
            points[0] = points[minIdx];
            points[minIdx] = t;

            // Sorts by polar angles points[1 ... ]
            Array.Sort(points, 1, points.Length - 1, new GSComparer(points[0]));
        }
    }


    public interface ISolver
    {
        int[] Solve(Point[] points);
    }

    /// <summary>
    /// Brute force solver that can handle small but not large problems.
    /// </summary>
    public class BruteSolver : ISolver
    {
        IConvexHull mCalc = new GrahamScan();

        public int[] Solve(Point[] points)
        {
            int[] ans = new int[points.Length];

            // Calculates convex hull
            var cvxHull = mCalc.Calc(points, false);
            var cvxHullSet = new HashSet<Point>(cvxHull);
            
            // If point is not in convex hull, calculates how many points need to be removed
            for (int i = 0; i < points.Length; i++)
            {
                if (cvxHullSet.Contains(points[i]))
                {
                    continue;
                }
                ans[i] = -1;    // FINISH
            }

            return ans;
        }
    }

    /// <summary>
    /// Smart solver that can handle even large problems.
    /// </summary>
    public class SmartSolver : ISolver
    {
        public int[] Solve(Point[] points)
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
0 0
10 0
10 10
0 10
5 5
9
0 0
5 0
10 0
0 5
5 5
10 5
0 10
5 10
10 10
";

        static string mSampeOut = @"
Case #1:
0
0
0
0
1
Case #2:
0
0
0
0
3
0
0
0
0
";


        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            long timeStart = DateTime.Now.Ticks;

            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Reads input
                int numPoints = int.Parse(ReadLine(txtIn));
                Point[] points = new Point[numPoints];
                for (int j = 0; j < numPoints; j++)
                {
                    var line = ReadLine(txtIn).Split(' ').Select(s => int.Parse(s)).ToArray();
                    points[j] = new Point() { X = line[0], Y = line[1] };
                }

                // Calls solver to solve
                Console.WriteLine("{0} Doing case #{1}", DateTime.Now.ToLongTimeString(), i);
                var ans = solver.Solve(points);

                // Prints output
                txtOut.WriteLine("Case #{0}:", i);
                foreach (var num in ans)
                {
                    txtOut.WriteLine(num);
                }
            }

            long timeEnd = DateTime.Now.Ticks;
            var elapsed = TimeSpan.FromTicks(timeEnd - timeStart);
            Console.WriteLine("Elapsed time: {0} min {1} sec", elapsed.Minutes, elapsed.Seconds);
        }

        #endregion

        #region fixed

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
            string currCase = "";
            for (int i = 0; i < numLines; i++)
            {
                if (expected[i].StartsWith("Case #"))
                {
                    currCase = expected[i];
                }

                if (expected[i] != actual[i])
                {
                    Warning("{2} @ {3} - Expected: {0} vs Actual {1}", expected[i], actual[i], currCase, i+1);
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
