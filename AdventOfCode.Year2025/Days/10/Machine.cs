using AdventOfCode.Shared.Extensions;

namespace AdventOfCode.Year2025.Days.DayTen;

public record Machine

{
    public Machine() { }
    public Machine(int id, string input)
    {
        this.Id = id;
        var parts = input.Split(' ');
        foreach (var part in parts)
        {
            if (part.StartsWith('['))
            {
                var lights = part.Trim('[', ']');
                LightsCount = lights.Length;

                long bitmask = 0;
                foreach (char c in lights)
                {
                    bitmask <<= 1; // shift left to make room for next bit
                    if (c == '#')
                    {
                        bitmask |= 1; // set lowest bit to 1
                    }
                }
                TargetLights = bitmask;
            }
            else if (part.StartsWith('('))
            {
                var numLights = this.LightsCount;

                long mask = 0;
                var idxs = part.Trim('(', ')').Split(',').Select(p => int.Parse(p)).ToList();
                foreach (var idx in idxs)
                {
                    mask |= 1L << (numLights - 1 - idx);
                }
                Buttons.Add(mask);
            }
            else if (part.StartsWith('{'))
            {
                var requirements = part.Trim('{', '}').Split(',');
                Requirements = requirements.Select(r => int.Parse(r)).ToArray();
            }
        }
    }

    public int Id { get; init; }

    public int LightsCount { get; set; }

    public long TargetLights { get; init; }

    public long Joltage { get; set; }

    public List<long> Buttons { get; set; } = new();
    public List<(long mask, int[] voltages)> ButtonVoltages => Buttons.Select(b => (b, b.ToBitmask(LightsCount))).ToList();

    public List<int[]> ButtonEffects => Buttons.Select(b => b.ToBitmask(LightsCount)).ToList();

    public int[] Requirements { get; init; }
    public int[] RequirementsParity => Requirements.ToArray().ToParityArray();

}