using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Days.DayThree;
public class DayThreeMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayThreeMain() : base(Day.Three, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        List<Tuple<int, int>> mulOperation = new();
        string line = string.Concat(linesOfInput);
        var matches = Regex.Matches(line, @"mul\((\d{1,3}),(\d{1,3})\)");
        foreach (Match match in matches)
        {
            mulOperation.Add(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
        }

        long result = mulOperation.Sum(x => x.Item1 * x.Item2);
        SetResult1(result);

        mulOperation.Clear();
        
        //Remove DONT blocks
        var cleanLine = Regex.Replace(line, @"don't\(\)(.+?)do\(\)", string.Empty);
        var cleanMatches = Regex.Matches(cleanLine, @"mul\((\d{1,3}),(\d{1,3})\)");
        foreach (Match match in cleanMatches)
        {
            mulOperation.Add(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
            
        }
        result = mulOperation.Sum(x => x.Item1 * x.Item2);
        SetResult2(result);

        await base.Run();
    }
}
