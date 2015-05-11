using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3
// namespace P3
{
    public static class TimeUtils
    {
        public static int Parse(string hhmmss)
        {
            var nums = hhmmss.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s));
            return nums.Aggregate(0, (s, n) => s * 60 + n);
        }
    }
}
