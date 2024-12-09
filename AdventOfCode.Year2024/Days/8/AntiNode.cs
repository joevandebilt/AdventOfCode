namespace AdventOfCode.Year2024.Days.DayEight;
public class AntiNode : Antenna
{
    public int RowDirection { get; set; }
    public int ColDirection { get; set; }
}

public class Antenna
{
    public char Identifier { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}