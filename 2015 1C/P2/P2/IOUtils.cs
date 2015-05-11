using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2
{
    public static class IOUtils
    {
        public static string ReadLine(TextReader txtIn)
        {
            string line;

            do
            {
                line = txtIn.ReadLine();
            }
            while (line != null && line.StartsWith("!"));

            return line;
        }

        public static int[] ReadInts(TextReader txtIn)
        {
            return ReadLine(txtIn).Split(' ').Select(s => int.Parse(s)).ToArray();
        }

        public static long[] ReadLongs(TextReader txtIn)
        {
            return ReadLine(txtIn).Split(' ').Select(s => long.Parse(s)).ToArray();
        }

        public static double[] ReadDoubles(TextReader txtIn)
        {
            return ReadLine(txtIn).Split(' ').Select(s => double.Parse(s)).ToArray();
        }

        public static T[,] ReadTable<T>(TextReader txtIn, int numCols, Func<string, T> func, int numRows = -1, char sep = ' ')
        {
            // Gets number of rows from parameter or input
            if (numRows == -1)
            {
                numRows = int.Parse(ReadLine(txtIn));
            }

            T[,] table = new T[numRows, numCols];

            // Reads each row and splits into columns
            for (int row = 0; row < numRows; row++)
            {
                var rec = ReadLine(txtIn).Split(sep).Select(s => func(s)).ToArray();
                Debug.Assert(rec.Length == numCols);
                for (int col = 0; col < numCols; col++)
                {
                    table[row, col] = rec[col];
                }
            }

            return table;
        }
    }
}
