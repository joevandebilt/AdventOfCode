namespace AdventOfCode.Year2025.Days.DayTwelve;

public class Region
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public IDictionary<int, int> Requirements { get; set; } = null!;
}
