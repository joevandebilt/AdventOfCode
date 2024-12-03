using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2023.Days.DayNine;
public class Day9 : AdventOfCodeDay
{
    private const bool _debugging = false;
    public Day9() : base(Day.Nine, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var extrapolatedValues = new List<IList<int>>();
        foreach (var line in linesOfInput)
        {
            var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            extrapolatedValues.Add(ExtrapolateNextValue(numbers));
        }
        SetResult1(extrapolatedValues.Select(e => e.Last()).Sum());
        SetResult2(extrapolatedValues.Select(e => e.First()).Sum());
        await base.Run();
    }

    private IList<int> ExtrapolateNextValue(IList<int> values)
    {
        if (values.Distinct().Count() == 1)
        {
            var stableIncrement = values.First();
            values.Insert(0, stableIncrement);
            values.Add(stableIncrement);
            return values;
        }
        else
        {
            var nextLayer = new List<int>();
            for (int i = 0; i < values.Count() - 1; i++)
            {
                nextLayer.Add(values.ElementAt(i + 1) - values.ElementAt(i));
            }
            if (nextLayer.Count() != values.Count() - 1) throw new ArithmeticException("Next Layer Should be 1 order smaller dickhead");

            var extrapolatedLayer = ExtrapolateNextValue(nextLayer);
            values.Insert(0, values.First() - extrapolatedLayer.First());
            values.Add(values.Last() + extrapolatedLayer.Last());
            return values;
        }
    }
}
