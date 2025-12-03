using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Collections.Generic;

namespace AdventOfCode.Year2025.Days.DayTwo;

public class DayTwoMain : AdventOfCodeDay
{
    private const bool _debugging = false;
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

        for (long id = start; id <= end; id++)
        {
            var idString = id.ToString();
            var half = idString.Length / 2;

            for (int chunk = 1; chunk <= half; chunk++)
            {
                //Only consider chunk sizes that evenly divide the string length
                if (idString.Length % chunk == 0)
                {
                    //Split the string into chunks of the current size and get the unique entries
                    var chunks = idString.Chunk(chunk).Select(x => new string(x)).ToList();
                    var uniqueEntries = chunks.Distinct().ToList();

                    //Pattern is repeating if the number of unique entries is 1
                    if (uniqueEntries.Count == 1)
                    {
                        //Ensure we aren't recapturing one we already have, for example 2222 is 2-2-2-2 and 22-22
                        if (!repeatingSequence.Contains(id))
                            repeatingSequence.Add(id);

                        //If the chunk size is exactly half the string length, we have a repeating half
                        if (chunk == half && idString.Length % 2 == 0)
                            repeatingHalves.Add(id);
                    }
                }
            }
        }

        return (repeatingHalves, repeatingSequence);
    }
}
