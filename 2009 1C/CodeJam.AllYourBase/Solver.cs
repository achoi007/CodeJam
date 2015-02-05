using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam.AllYourBase
{
    class Solver
    {
        public ulong Analyze(string input, out uint numBase)
        {
            var chDict = new Dictionary<char, uint>();

            // First ch is always a 1
            chDict.Add(input[0], 1);

            // Next ch that is not the same as first char is a 0
            int currIdx;
            for (currIdx = 1; currIdx < input.Length && input[0] == input[currIdx]; currIdx++)
            {
            }

            if (currIdx != input.Length)
            {
                chDict.Add(input[currIdx], 0);
                ++currIdx;
            }

            // Each ch followed is the next higher up number if it has never been seen before.
            uint currNum = 2;
            while (currIdx < input.Length)
            {
                var ch = input[currIdx];
                if (!chDict.ContainsKey(ch))
                {
                    chDict.Add(ch, currNum);
                    ++currNum;
                }
                ++currIdx;
            }

            // Base is the number of distinct chars
            uint nBase = (uint)chDict.Count;
            if (nBase == 1)
            {
                nBase = 2;
            }

            // Convert string to number
            numBase = nBase;
            return input.Aggregate(0UL, (sum, ch) => sum * nBase + chDict[ch], sum => sum);
        }
    }
}
