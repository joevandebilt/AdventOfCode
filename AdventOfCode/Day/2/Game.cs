namespace AdventOfCode.Day.DayTwo;

public record Game
{
    public int Id { get; set; }
    public List<Result> Results { get; set; } = new();

    public int Power
    {
        get
        {
            return MinRed * MinGreen * MinBlue;
        }
    }

    public int MinRed
    {
        get
        {
            return Results.Max(r => r.Red);
        }
    }
    public int MinGreen
    {
        get
        {
            return Results.Max(r => r.Green);
        }
    }

    public int MinBlue
    {
        get
        {
            return Results.Max(r => r.Blue);
        }
    }
}
public record Result
{
    public int Red { get; set; } = 0;
    public int Green { get; set; } = 0;
    public int Blue { get; set; } = 0;
}
