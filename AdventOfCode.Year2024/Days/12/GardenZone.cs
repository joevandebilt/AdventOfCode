namespace AdventOfCode.Year2024.Days.DayTwelve;

public class GardenZone
{
    public char Identifier { get; set; }
    public Tuple<int, int>? Coordinate { get; set; }
}

public class Region
{
    public string Id { get; set; } = string.Empty;
    public char Identifier { get; set; }
    public int Perimeter { get; set; } = 0;
    public int Corners { get; set; } = 0;
    public int Area => Zones.Count();
    public long Price { get { return Area * Perimeter; } }
    public List<GardenZone> Zones { get; set; } = new();
}
