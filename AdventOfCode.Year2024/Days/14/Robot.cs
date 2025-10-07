namespace AdventOfCode.Year2024.Days.DayFourteen;

public class Robot
{
    public Coords Position { get; set; } = new();
    public Coords Velocity { get; set; } = new();
}


public class Coords
{
    public Coords() { }
    public Coords(string coodinates)
    {
        var parts = coodinates.Split(',');
        this.X = int.Parse(parts.First());
        this.Y = int.Parse(parts.Last());
    }
    public int X { get; set; }
    public int Y { get; set; }
}