using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandingOvation
{
    public class Solver
    {
        public int Solve(int maxShyness, string countPerShynessStr)
        {
            int[] countPerShyness = countPerShynessStr.Select(c => c - '0').ToArray();
            int needed = 0;
            int sum = 0;

            for (int i = 0; i < countPerShyness.Length; i++)
            {
                int deltaNeeded = 0;
                if (sum < i)
                {
                    deltaNeeded = i - sum;
                }
                sum += countPerShyness[i] + deltaNeeded;
                needed += deltaNeeded;
            }

            return needed;
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

            Process(txtIn, txtOut);
            txtOut.Close();
            txtIn.Close();
        }

        static void Process(TextReader txtIn, TextWriter txtOut)
        {
            int numCases = int.Parse(txtIn.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                // Read input
                var line = txtIn.ReadLine().Split(' ');
                int maxShyness = int.Parse(line[0]);
                string countPerShyness = line[1];

                // Generate result
                Solver solver = new Solver();
                int pplNeeded = solver.Solve(maxShyness, countPerShyness);
                txtOut.WriteLine("Case #{0}: {1}", i, pplNeeded);
            }
        }
    }
}
