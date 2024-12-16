namespace AdventOfCode.Year2024.Days.DayTwelve;

public class GardenZone
{
    public char Identifier { get; set; }
    public int Area { get; set; } = 0;
    public int Perimeter { get; set; } = 0;
    public long Price { get { return Area * Perimeter; } }
}
