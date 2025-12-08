using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DaySeven;
public record Beam : Coordinate
{
    public Beam(int row, int col, long intensity) : base(row, col)
    {
        Intensity = intensity;
    }
    public long Intensity { get; set; }
}
