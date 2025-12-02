using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Collections.Generic;

namespace AdventOfCode.Year2025.Days.DayTwo;

public class DayTwoMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTwoMain() : base(Day.Two, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var part1Ids = new List<long>();
        var part2Ids = new List<long>();

        foreach (var line in linesOfInput)
        {
            //Process each line here
            foreach (var productRange in line.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var productIds = productRange.Split('-').Select(x => long.Parse(x));
                var (part1, part2) = ScanValidRange(productIds.First(), productIds.Last());

                part1Ids.AddRange(part1);
                part2Ids.AddRange(part2);
            }
        }

        SetResult1(part1Ids.Sum());
        SetResult2(part2Ids.Sum());
        await base.Run();
    }

    private (IEnumerable<long>, IEnumerable<long>) ScanValidRange(long start, long end)
    {
        List<long> repeatingHalves = new();
        List<long> repeatingSequence = new();

        //An invalid Id must repeat itself so must start at twice it's length
        for (long i = start; i <= end; i++)
        {
            var idString = i.ToString();
            var half = idString.Length / 2;

            for (int j = 1; j <= half; j++)
            {
                if (idString.Length % j != 0)
                    continue;

                var chunks = idString.Chunk(j).Select(x => new string(x)).ToList();
                var uniqueEntries = chunks.Distinct().ToList();
                if (uniqueEntries.Count == 1)
                {
                    if (!repeatingSequence.Contains(i))
                        repeatingSequence.Add(i);                    
                    
                    if (j == half && idString.Length % 2 == 0)
                        repeatingHalves.Add(i);
                }
            }

        }

        return (repeatingHalves, repeatingSequence);
    }
}
