using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Djijkstra
{
    public interface ISolver
    {
        bool Reducible(string s, int numStr, out Tuple<int, int> splits);
    }

    public interface IReducer
    {
        void Init(int[] nums);

        /// <summary>
        /// Calculates nums[start] * nums[start+1] * ... * nums[end-1] * nums[end]
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        int Product(int startPos, int endPos);
    }

    public class CacheReducer : IReducer
    {
        int[,] mProducts;

        public void Init(int[] nums)
        {
            mProducts = new int[nums.Length, nums.Length];
            Parallel.For(0, nums.Length, i =>
            {
                int curr = nums[i];
                mProducts[i, i] = curr;

                for (int j = i + 1; j < nums.Length; j++)
                {
                    curr = Solver.Multiply(null, curr, nums[j]);
                    mProducts[i, j] = curr;
                }
            });
        }

        public int Product(int startPos, int endPos)
        {
            Debug.Assert(startPos <= endPos);
            return mProducts[startPos, endPos];
        }
    }


    public static class Solver
    {
        private static int[,] Table;
        public const int I = 2;
        public const int J = 3;
        public const int K = 4;

        static Solver()
        {
            Table = new int[5, 5] {
                { 0, 0,  0,  0,  0},
                { 0, 1,  I,  J,  K},
                { 0, I, -1,  K, -J},
                { 0, J, -K, -1,  I},
                { 0, K,  J, -I, -1}
            };
        }

        public static int Multiply(this ISolver solver, int a, int b)
        {
            bool sameSign = Math.Sign(a) == Math.Sign(b);
            int product = Table[Math.Abs(a), Math.Abs(b)];
            return sameSign ? product : -product;
        }

        public static int[] Convert(this ISolver solver, string s)
        {
            return s.Select(c => c - 'i' + Solver.I).ToArray();
        }
    }

    public class BruteSolver : ISolver
    {
        public bool Reducible(string s, int numStr, out Tuple<int, int> splits)
        {
            int[] nums = CreateNums(s, numStr);
            
            // Creates reducer to cache computations
            IReducer reducer = new CacheReducer();
            reducer.Init(nums);

            // Finds i such that Product(0, i) == 'i'
            // Then finds j such that Product(i+1, j) == 'j'
            // Then checks Product(j+1, nums.Length-1) == 'k'
            int maxI = nums.Length - 3;
            int maxJ = nums.Length - 2;
            int maxK = nums.Length - 1;

            int currI = 1;
            for (int i = 0; i <= maxI; i++)
            {
                currI = this.Multiply(currI, nums[i]);

                // Skips unless Product(0, i) == 'i'
                if (currI != Solver.I)
                {
                    continue;
                }

                int currJ = 1;
                for (int j = i+1; j <= maxJ; j++)
                {
                    currJ = this.Multiply(currJ, nums[j]);

                    // Skips unless Product(i+1, j) == 'j'
                    if (currJ != Solver.J)
                    {
                        continue;
                    }

                    // Checks Product(j+1, maxK) == 'k'
                    int currK = reducer.Product(j + 1, maxK);
                    if (currK != Solver.K)
                    {
                        continue;
                    }

                    // Finds answer
                    splits = Tuple.Create(i, j);
                    return true;
                }
            }

            splits = null;
            return false;
        }

        private int[] CreateNums(string s, int numStr)
        {
            string fullStr = string.Join("", Enumerable.Repeat(s, numStr));
            return this.Convert(fullStr);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            TextReader txtIn;
            TextWriter txtOut;

            if (args.Length != 2)
            {
                txtIn = Console.In;
                txtOut = Console.Out;
            }
            else
            {
                txtIn = File.OpenText(args[0]);
                txtOut = new StreamWriter(File.OpenWrite(args[1]));
            }

            Process(txtIn, txtOut, new BruteSolver());
            txtOut.Close();
            txtIn.Close();
        }

        static void Process(TextReader txtIn, TextWriter txtOut, ISolver solver)
        {
            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Read input
                var line = txtIn.ReadLine().Split(' ');
                int repeat = int.Parse(line[1]);

                var str = txtIn.ReadLine();
                Tuple<int, int> split;
                Console.WriteLine("Doing case #{0}", i);
                bool reducible = solver.Reducible(str, repeat, out split);

                txtOut.WriteLine("Case #{0}: {1}", i, reducible ? "YES" : "NO");
                Console.WriteLine("Split: {0}", split);
            }
        }
    }
}
