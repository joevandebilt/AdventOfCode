
using AdventOfCode.Enums;

namespace AdventOfCode.Days.DayOne;

public class DayOneMain : AdventOfCodeDay
{
    private const bool _debugging = false;

    public DayOneMain() : base(Day.One, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        List<int> p1calibrations = new();
        List<int> p2calibrations = new(); 
        foreach (var line in linesOfInput)
        {
            Dictionary<int, int> indexedValues = new();

            int firstIndex, lastIndex;
            foreach (var numberEnum in Enum.GetValues<Day>())
            {
                string numberName = numberEnum.ToString();
                int numberValue = (int)numberEnum;

                firstIndex = line.IndexOf(numberName.ToLower());
                if (firstIndex > -1)
                    indexedValues.Add(firstIndex, numberValue);

                lastIndex = line.LastIndexOf(numberName);
                if (lastIndex > -1 && lastIndex != firstIndex)
                    indexedValues.Add(lastIndex, numberValue);
            }

            var firstNumericChar = line.FirstOrDefault(l => Char.IsNumber(l));
            var lastNumericChar = line.LastOrDefault(l => Char.IsNumber(l));

            firstIndex = line.IndexOf(firstNumericChar);
            lastIndex = line.LastIndexOf(lastNumericChar);

            indexedValues.Add(firstIndex, int.Parse($"{firstNumericChar}"));
            if (firstIndex != lastIndex)
                indexedValues.Add(lastIndex, int.Parse($"{lastNumericChar}"));

            var orderedValues = indexedValues.OrderBy(key => key.Key);
            p1calibrations.Add(int.Parse($"{firstNumericChar}{lastNumericChar}"));
            p2calibrations.Add(int.Parse($"{orderedValues.First().Value}{orderedValues.Last().Value}"));
        }
        SetResult1(p1calibrations.Sum());
        SetResult2(p2calibrations.Sum());

        await base.Run();
    }
}
