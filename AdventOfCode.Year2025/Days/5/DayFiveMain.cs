using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayFive;
public class DayFiveMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFiveMain() : base(Day.Five, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        
        List<long> ingredients = new();
        List<NumberRange> ranges = new();

        foreach (var line in linesOfInput)
        {
            if (line.Contains('-'))
            {
                var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var lower = long.Parse(parts[0]);
                var upper = long.Parse(parts[1]);
                ranges.Add(new NumberRange(lower, upper));
            }
            else if (line.Trim().Length > 0)
            {
                ingredients.Add(long.Parse(line));
            }
        }

        var orderedRange = ranges.OrderBy(r => r.Lower).ToList();
        for (int i = 0; i < orderedRange.Count - 1; i++)
        {
            var current = orderedRange[i];
            var next = orderedRange[i + 1];
            if (current.Overlaps(next))
            {
                var merged = new NumberRange(Math.Min(current.Lower, next.Lower), Math.Max(current.Upper, next.Upper));
                orderedRange[i] = merged;
                orderedRange.RemoveAt(i + 1);
                i--;
            }
        }

        SetResult1(ingredients.Count(i => ranges.Any(r => r.InRange(i))));
        SetResult2(orderedRange.Sum(r => r.Size()));
        await base.Run();
    }
}
