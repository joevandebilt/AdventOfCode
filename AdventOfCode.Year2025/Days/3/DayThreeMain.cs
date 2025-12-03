using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Collections.Generic;

namespace AdventOfCode.Year2025.Days.DayThree;

public class DayThreeMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayThreeMain() : base(Day.Three, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        var joltages2Combo = new List<long>();
        var joltages12Combo = new List<long>();

        foreach (var line in linesOfInput)
        {
            //Turn each character in the line into an integer
            var batteries = line.Select(c => int.Parse(c.ToString()));
           
            joltages2Combo.Add(GetBatteries(batteries, 2));
            joltages12Combo.Add(GetBatteries(batteries, 12));

        }
        SetResult1(joltages2Combo.Sum());
        SetResult2(joltages12Combo.Sum());
        await base.Run();
    }

    private long GetBatteries(IEnumerable<int> batteries, int batteriesAllowed)
    {
        var combinations = new List<long>();
        var bank = new List<KeyValuePair<int, int>>();

        int idx = -1;
        int joltage = 0;
        for (int i = 1; i <= batteriesAllowed; i++)
        {
            (joltage, idx) = CalculateCombinations(batteries, idx, batteriesAllowed - i);
            bank.Add(new KeyValuePair<int, int>(joltage, idx));
        }
        var combination = long.Parse(string.Concat(bank.Select(b => b.Key.ToString())));
        WriteLine($"Line: {string.Concat(batteries.Select(b=>b.ToString()))} -> Combination: {combination}");
        return combination;
    }

    private (int, int) CalculateCombinations(IEnumerable<int> joltageRatings, int currentIndex, int offset)
    {
        var highest = -1;
        var idx = 0;
        var max = joltageRatings.Count() - offset;

        for (int i = currentIndex + 1; i < max; i++)
        {
            var joltage = joltageRatings.ElementAt(i);
            if (joltage > highest)
            {
                highest = joltage;
                idx = i;
            }
        }
        return (highest, idx);
    }
}
