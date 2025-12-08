using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayEight;

public class Circuit
{
    public List<Vector> JunctionBoxes { get; set; } = new();

    public bool Contains(Vector vec)
    {
        return JunctionBoxes.Any(v => v.Reference == vec.Reference);
    }
}
