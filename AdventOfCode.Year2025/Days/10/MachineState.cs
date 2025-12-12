namespace AdventOfCode.Year2025.Days.DayTen;

public record MachineState
{
    public long Lights { get; set; }
    public int[] Parity { get; set; }
    public int[] Voltage { get; set; }
    public int Presses { get; set; }

    public override string ToString()
    {
        return $"[{string.Concat(Parity)}] {{{string.Concat(Voltage)}}} {Presses}";
    }
}
