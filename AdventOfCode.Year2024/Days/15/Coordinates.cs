namespace AdventOfCode.Year2024.Days.DayFifteen;

public class Coords
{
    public Coords() { }
    public Coords(int row, int col)
    {
        this.X = col;
        this.Y = row;
    }
    public Coords(string coodinates)
    {
        var parts = coodinates.Split(',');
        this.X = int.Parse(parts.First());
        this.Y = int.Parse(parts.Last());
    }
    public int X { get; set; }
    public int Y { get; set; }
}