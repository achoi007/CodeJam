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
        private readonly int[] mCh2IntMap = new int[Char.MaxValue];

        public Solver()
        {
            // map from 0-9 a-z to number
            for (int ch = '0'; ch <= '9'; ch++)
            {
                mCh2IntMap[ch] = ch - '0';
            }
            for (int ch = 'a'; ch <= 'z'; ch++)
            {
                mCh2IntMap[ch] = ch - 'a' + 10;
            }
            for (int ch = 'A'; ch <= 'Z'; ch++)
            {
                mCh2IntMap[ch] = ch - 'A' + 10;
            }
        }

        public int Base { get; private set; }

        public BigInteger[] Solution { get; private set; }

        public void Analyze(string[] inputs)
        {
            char maxChar = FindMaxChar(inputs);
            Base = mCh2IntMap[maxChar];
            Solution = Convert(inputs, Base);
        }

        private char FindMaxChar(string[] inputs)
        {
            char[] maxChars = new char[inputs.Length];
            Parallel.For(0, inputs.Length, i =>
            {
                maxChars[i] =  inputs[i].Max();
            });
            return maxChars.Max();
        }

        private BigInteger[] Convert(string[] inputs, int basis)
        {
            BigInteger[] outputs = new BigInteger[inputs.Length];
            Parallel.For(0, inputs.Length, i =>
            {
                BigInteger sum = new BigInteger();
                foreach (var ch in inputs[i])
                {
                    sum = sum * basis + mCh2IntMap[ch];
                }
                outputs[i] = sum;
            });
            return outputs;
        }
    }
}
