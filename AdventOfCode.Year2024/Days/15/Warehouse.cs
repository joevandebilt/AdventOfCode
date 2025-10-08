namespace AdventOfCode.Year2024.Days.DayFifteen;

public class Warehouse
{
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public Coords Robot { get; set; } = null!;
    public List<Coords> Boxes { get; set; } = new();
    public List<Coords> Walls { get; set; } = new();
}
