using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025.Days.DayFive
{
    public record NumberRange
    {
        public NumberRange(long lower, long upper)
        {
            if (lower > upper)
            {
                var temp = lower;
                lower = upper;
                upper = temp;
            }

            this.Lower = lower;
            this.Upper = upper;
        }
        public long Lower { get; set; }
        public long Upper { get; set; }

        internal bool InRange(long i)
        {
            return i >= Lower && i <= Upper;
        }

        internal long Size()
        {
            return Upper - Lower + 1;
        }

        internal bool Overlaps(NumberRange other)
        {
            return this.InRange(other.Lower) || this.InRange(other.Upper) || other.InRange(this.Lower) || other.InRange(this.Upper);
        }
    }
}
