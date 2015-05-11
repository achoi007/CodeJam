using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3
{
    public class Point
    {
        const int MAX_COORD = 10000;

        public int X { get; set; }

        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            Point rhs = (Point)obj;
            return X == rhs.X && Y == rhs.Y;
        }

        public override int GetHashCode()
        {
            return X * MAX_COORD + Y;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point() { X = a.X + b.X, Y = a.Y + b.Y };
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point() { X = a.X - b.X, Y = a.Y - b.Y };
        }

        // Dot product of 2 "vectors"
        public static int operator *(Point a, Point b)
        {
            return a.X * b.X + a.Y + b.Y;
        }

        // Cross product of 2 "vectors"
        public static int operator ^(Point a, Point b)
        {
            return a.X * b.Y - b.X * a.Y;
        }
    }

    public static class LinAlg
    {
        public static int DistSq(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static double Norm(Point a)
        {
            return Math.Sqrt(a.X * a.X + a.Y * a.Y);
        }

        /// <summary>
        /// Determines if (a, b, c) are clockwise or counterclockwise
        /// > 0 if counter-clockwise
        /// 0 if co-linear
        /// < 0 if clockwise
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CCW(Point a, Point b, Point c)
        {
            Point v1 = b - a;
            Point v2 = c - a;
            return v1 ^ v2;
        }
    }
}
