
namespace AdventOfCode.Year2025.Days.DayTwelve;

public class Shape
{
    public int Id { get; set; }
    public bool[,] Pattern { get; set; }
    public int Size => Pattern.Length;

    public int Height => Pattern.GetLength(0);

    public int Width => Pattern.GetLength(1);
}
