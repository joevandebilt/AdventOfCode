namespace AdventOfCode.Year2024.Days.DayThirteen;

public class ButtonConfiguration
{
    public char Identifier { get; set; }
    public int XMove { get; set; }
    public int YMove { get; set; }
    public int Cost => Identifier == 'A' ? 3 : 1;
}
