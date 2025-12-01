using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Numerics;

namespace AdventOfCode.Shared.Template;
public class DayOneMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayOneMain() : base(Day.One, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        int dialShown = 50;
        int hits = 0;
        int clicks = 0;

        bool freeMove = false;
        foreach (var line in linesOfInput)
        {
            //Process each line here
            int invert = 1;
            if (line.StartsWith('l'))
                invert = -1;

            var numberPart = int.Parse(line[1..]);
            clicks += numberPart / 100;
            
            var shift = numberPart % 100;

            dialShown += shift * invert;
            
            if (dialShown == 0)
            {
                hits++;
                clicks++;
                freeMove = true;
            }
            else if (dialShown == 100)
            {
                hits++;
                clicks++;
                dialShown = 0;
                freeMove = true;
            }
            else if (dialShown > 100)
            {
                dialShown -= 100;
                if (!freeMove)
                    clicks++;
                else
                    freeMove = false;
            }
            else if (dialShown < 0)
            {
                dialShown += 100;
                if (!freeMove)
                    clicks++;
                else
                    freeMove = false;
            }
            else if (freeMove)
            {
                freeMove = false;
            }

                WriteLine($"{line} -> {dialShown}");
        }


        SetResult1(hits);
        SetResult2(clicks);
        await base.Run();
    }
}
