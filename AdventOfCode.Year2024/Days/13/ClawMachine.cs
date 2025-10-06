namespace AdventOfCode.Year2024.Days.DayThirteen;

public class ClawMachine
{
    public ButtonConfiguration AButton { get; set; } = null!;
    public ButtonConfiguration BButton { get; set; } = null!;
    public Prize Prize { get; set; } = null!;
    public bool Possible { get; set; } = false;
    public bool Big { get; set; } = false;
    
    public long Cost { get; set; }
}
