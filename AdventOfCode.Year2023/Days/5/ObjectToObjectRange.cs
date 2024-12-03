namespace AdventOfCode.Year2023.Days.DayFive;
public class ObjectToObjectRange
{
    public long SourceStart { get; set; }
    public long DestinationStart { get; set; }
    public long RangeLength { get; set; }

    public bool SourceInRange(long input) => (SourceStart <= input && input < (SourceStart + RangeLength));
    public bool DestinationInRange(long input) => (DestinationStart <= input && input < (DestinationStart + RangeLength));
}
