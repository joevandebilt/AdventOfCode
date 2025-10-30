using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Security.Cryptography;

namespace AdventOfCode.Year2022.Days.DayTen;
public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: true);


        int cycles = 0;
        int xRegister = 1;

        Dictionary<int, int> signalStrength = new();
        string pixels = string.Empty;

        foreach (var line in linesOfInput)
        {
            var hertz = line.StartsWith("noop") ? 1 : 2;
            for (int i = 0; i < hertz; i++)
            {
                //Sprite in position
                if (Math.Abs(xRegister - (cycles % 40)) <= 1)
                {
                    pixels += "█";
                }
                else
                {
                    pixels += " ";
                }

                cycles++;
                if ((cycles - 20) % 40 == 0)
                {
                    signalStrength.Add(cycles, xRegister);
                }
            }

            if (hertz == 2)
            {
                var xChange = int.Parse(line.Split(' ').Last());
                xRegister += xChange;
            }
        }

        SetResult1(signalStrength.Sum(ss => ss.Key * ss.Value));

        PrintScreen(pixels);
        SetResult2(-1);
        await base.Run();
    }

    private void PrintScreen(string pixels)
    {
        var lines = pixels.Chunk(40);
        foreach (var line in lines)
        {
            WriteLine(new string(line));
        }
    }
}
