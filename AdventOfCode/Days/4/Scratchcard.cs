namespace AdventOfCode.Days.DayFour;
public record Scratchcard
{
    public int CardNumber { get; set; }
    public List<int> WinningNumbers { get; set; } = new List<int>();
    public List<int> SelectedNumbers { get; set; } = new List<int>();

    public int CorrectPicks
    {
        get { return SelectedNumbers.Count(s => WinningNumbers.Contains(s)); }
    }

    public double Points
    {
        get {
            if (CorrectPicks == 0) return 0;
            return Math.Pow(2, Math.Max(0, CorrectPicks-1)); 
        }
    }
}
