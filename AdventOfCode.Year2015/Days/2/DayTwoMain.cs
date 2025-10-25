using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2015.Days.DayOne;

public class DayTwoMain : AdventOfCodeDay
{
    private const bool _debugging = true;

    public DayTwoMain() : base(Day.Two, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var totalPaper = 0;
        var totalRibbon = 0;

        foreach (var line in linesOfInput)
        {
            var numbers = line.Split('x').Select(int.Parse).OrderBy(n => n).ToArray();

            var area = 2 * ((numbers[0] * numbers[1]) + (numbers[1] * numbers[2]) + (numbers[0] * numbers[2]));
            var spare = numbers.Take(2).Aggregate(1, (a, b) => a * b);
            var ribbon = numbers.Take(2).Aggregate((a, b) => (a * 2) + (b * 2));
            var bow = (numbers[0] * numbers[1] * numbers[2]);

            totalPaper += area + spare;
            totalRibbon += bow + ribbon;
        }

        SetResult1(totalPaper);
        SetResult2(totalRibbon);

        await base.Run();
    }
}
