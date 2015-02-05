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
            // Read input lines
            string[] inputs = File.ReadAllLines(args[0]);

            // Generate solution
            var solver = new Solver();
            solver.Analyze(inputs);
            var solution = solver.Solution;

            // Print out solution
            var solnTexts = solution.Select((n, i) =>
            {
                return string.Format("Case #{0}: {1}", i, n);
            });
            File.WriteAllLines(args[1], solnTexts);
        }
    }
}
