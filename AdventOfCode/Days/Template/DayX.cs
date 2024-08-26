using AdventOfCode.Enums;

namespace AdventOfCode.Days.Template;
public class Day0 : AdventOfCodeDay
{
    private const bool _debugging = false;
    public Day0() : base(Day.Zero, _debugging) { }

    public override async Task Run()
    {
        //var linesOfInput = await LoadFile();

        SetResult1(-1);
        SetResult2(-1);
        await base.Run();
    }
}
