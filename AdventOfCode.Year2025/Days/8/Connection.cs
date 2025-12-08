using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayEight;

public record Connection
{
    public Vector JunctionA { get; set; }
    public Vector JunctionB { get; set; }
    public Connection(Vector a, Vector b)
    {
        if (a.X < b.X)
        {
            JunctionA = a;
            JunctionB = b;
        }
        else
        {
            JunctionA = b;
            JunctionB = a;
        }
    }
    public double Distance => JunctionA.Distance(JunctionB);

    public bool Simulated { get; set; } = true;
}
