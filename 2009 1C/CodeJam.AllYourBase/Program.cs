using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam.AllYourBase
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read inputs
            string[] inputs;
            var reader = new Reader();
            if (args.Length == 2)
            {
                inputs = reader.Read(args[0]);
            }
            else
            {
                Console.WriteLine("Please enter input.");
                inputs = reader.Read(Console.In);
            }

            // Generate solution parallelly
            var solver = new Solver();
            ulong[] solutions = new ulong[inputs.Length];
            Parallel.For(0, solutions.Length, i =>
            {
                uint numBase;
                solutions[i] = solver.Analyze(inputs[i], out numBase);
            });

            // Print out solution
            var tw = (args.Length == 2) ? new StreamWriter(File.OpenWrite(args[1])) : Console.Out;
            for (int i = 0; i < solutions.Length; i++)
            {
                tw.WriteLine("Case #{0}: {1}", i + 1, solutions[i]);
            }
            if (tw != Console.Out)
            {
                tw.Close();
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
