using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteHouseOfPancakes
{
    public interface ISolver
    {
        /// <summary>
        /// Pancakes must be sorted from lowest to highest stack
        /// </summary>
        /// <param name="pancakes"></param>
        /// <returns></returns>
        int Solve(IList<int> pancakes);
    }

    public class BruteSolver : ISolver
    {
        public int Solve(IList<int> pancakes)
        {
            List<int> sortedPancakes = new List<int>(pancakes);
            sortedPancakes.Sort();
            return InternalSolve(sortedPancakes);
        }

        private int InternalSolve(List<int> pancakes, int delta = 0)
        {
            int split = Split(pancakes);
            int eat = Eat(pancakes);
            return delta + (split < eat ? split : eat);
        }

        private int Split(List<int> pancakes)
        {
            int highestPos = pancakes.Count - 1;
            int highest = pancakes[highestPos];

            // No need to split if highest stack is less than 3, since it is always 3 iterations
            if (highest <= 3)
            {
                return int.MaxValue;
            }

            // Splits highest stack into 2 stacks and places the 2 stacks
            int splitSmaller;
            int splitLarger;

            switch (highest)
            {
                case 9:
                    splitSmaller = 3;
                    splitLarger = 6;
                    break;
                default:
                    splitSmaller = highest / 2;
                    splitLarger = highest - splitSmaller;
                    break;
            }

            // Create new pancake list by placing the 2 stacks in proper sorted position and remove
            // highest position
            List<int> cakes = new List<int>(pancakes.Count + 1);
            foreach (var pancake in pancakes.Take(highestPos))
            {
                if (splitSmaller < pancake)
                {
                    cakes.Add(splitSmaller);
                    splitSmaller = int.MaxValue;
                }

                if (splitLarger < pancake)
                {
                    cakes.Add(splitLarger);
                    splitLarger = int.MaxValue;
                }

                cakes.Add(pancake);
            }

            // If smaller stack has not been inserted, insert it at the end
            if (splitSmaller != int.MaxValue)
            {
                cakes.Add(splitSmaller);
            }

            // If larger stack has not been inserted, insert it at the end
            if (splitLarger != int.MaxValue)
            {
                cakes.Add(splitLarger);
            }

            return InternalSolve(cakes, 1);
        }

        private int Eat(List<int> pancakes)
        {
            List<int> cakes = new List<int>(pancakes.Count);
            foreach (var pancake in pancakes)
            {
                if (pancake > 1)
                {
                    cakes.Add(pancake - 1);
                }
            }

            if (cakes.Count == 0)
            {
                return 1;
            }
            else
            {
                return InternalSolve(cakes, 1);
            }
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
                int numDishes = int.Parse(txtIn.ReadLine());
                var pancakeStr = txtIn.ReadLine();
                var pancakes = pancakeStr.Split(' ').Select(s => int.Parse(s)).ToList();
                int duration = solver.Solve(pancakes);
                txtOut.WriteLine("Case #{0}: {1}", i, duration);
            }
        }
    }
}
