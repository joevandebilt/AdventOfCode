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

        List<int> leftList = new();
        List<int> rightList = new();
        foreach (var line in linesOfInput)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            leftList.Add(int.Parse(parts.First()));
            rightList.Add(int.Parse(parts.Last()));
        }
        leftList.Sort();
        rightList.Sort();

        List<int> differences = new();
        for (int i = 0; i < leftList.Count; i++)
        {
            differences.Add(Math.Abs(leftList[i] - rightList[i]));
        }
        SetResult1(differences.Sum());

        List<int> similarity = new();
        var groupedRightList = rightList.GroupBy(x => x).ToList();
        foreach (int entry in leftList)
        {
            var record = groupedRightList.SingleOrDefault(x => x.Key == entry);
            if (record != null)
            {
                int count = record.Count();
                similarity.Add(entry * count);
            }
        }
        
        SetResult2(similarity.Sum());

        await base.Run();
    }
}
