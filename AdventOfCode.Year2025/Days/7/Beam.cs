using AdventOfCode.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2025.Days._7
{
    public record Beam : Coordinate
    {
        public Beam(int row, int col, long intensity) : base(row, col)
        {
            Intensity = intensity;
        }
        public long Intensity { get; set; }
    }
}
