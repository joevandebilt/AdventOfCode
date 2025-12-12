namespace AdventOfCode.Year2025.Days.DayEleven;

public record Device
{
    public Device(string input)
    {
        var parts = input.Split(":", StringSplitOptions.TrimEntries);
        Name = parts[0];
        if (parts.Length > 1)
        {
            OutputConnections = parts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }

    public string Name { get; set; }
    public List<string> OutputConnections { get; set; } = new();
    public List<Device> Outputs { get; set; } = new();
}
