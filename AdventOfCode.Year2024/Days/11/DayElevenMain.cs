using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;

namespace AdventOfCode.Year2024.Days.DayEleven;
public class DayElevenMain : AdventOfCodeDay
{
    private const bool _debugging = true;

    public DayElevenMain() : base(Day.Eleven, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        Dictionary<long, long> stones = new();

        foreach (var line in linesOfInput)
        {
            foreach (var stone in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                stones.Add(long.Parse(stone), 1);
            }
        }

        int blink = 0;
        while (blink < 75)
        {
            if (blink == 25)
                SetResult1(stones.Sum(sc => sc.Value));

            Dictionary<long, long> newStones = new();

            foreach (var stone in stones)
            {
                if (stone.Key == 0)
                {
                    newStones.UpsertEntry(1, stone.Value);
                }
                else if (new string(stone.Key.ToString()) is var stoneText && stoneText.Length % 2 == 0)
                {
                    var leftPart = long.Parse(stoneText.Substring(0, (stoneText.Length / 2)));
                    var rightPart = long.Parse(stoneText.Substring(stoneText.Length / 2));

                    newStones.UpsertEntry(leftPart, stone.Value);
                    newStones.UpsertEntry(rightPart, stone.Value);
                }
                else
                {
                    newStones.UpsertEntry(stone.Key * 2024, stone.Value);
                }
            }

            blink++;
            stones = newStones;
        }
        SetResult2(stones.Sum(sc => sc.Value));

        await base.Run();
    }
}
