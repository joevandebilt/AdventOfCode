using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayOne;

public class DayOneMain : AdventOfCodeDay
{
    private const bool _debugging = false;

    public DayOneMain() : base(Day.One, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        
        SetResult1(-1);
        SetResult2(-1);

        await base.Run();
    }
}
