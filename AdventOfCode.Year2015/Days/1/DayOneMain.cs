using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2015.Days.DayOne;

public class DayOneMain : AdventOfCodeDay
{
    private const bool _debugging = true;

    public DayOneMain() : base(Day.One, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        int floor = 0;
        int position = -1;

        foreach (var line in linesOfInput)
        {
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '(')
                    floor++;
                else if (c == ')')
                    floor--;

                if (floor == -1 && position == -1)
                    position = i + 1;
            }
        }

        SetResult1(floor);
        SetResult2(position);

        await base.Run();
    }
}
