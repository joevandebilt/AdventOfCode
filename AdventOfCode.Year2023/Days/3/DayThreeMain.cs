using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023.Days.DayThree;

public class DayThreeMain : AdventOfCodeDay
{
    private const bool _debugging = false;

    public DayThreeMain() : base(Day.Three, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        List<Part> partNumbers = new();
        for (int lineNumber = 0; lineNumber < linesOfInput.Count; lineNumber++)
        {
            var line = linesOfInput[lineNumber];

            //Scan for numbers
            var symbolMatches = Regex.Matches(line, "\\d+");
            foreach (Match match in symbolMatches)
            {
                var partNumber = match.Value;
                int partNumberIndex = match.Index;

                var part = new Part
                {
                    Key = $"{lineNumber}:{partNumberIndex}",
                    PartNumber = int.Parse(partNumber)
                };

                int start = Math.Max(0, partNumberIndex - 1);
                int end = Math.Min(line.Length, partNumberIndex + partNumber.Length + 1);

                //Scan line
                List<string> lines = new();

                //Scan Line Above
                if (lineNumber > 0)
                {
                    var lineAbove = lineNumber - 1;
                    ScanLine(part, linesOfInput[lineAbove], lineAbove, start, end);
                }

                //Add current line
                ScanLine(part, line, lineNumber, start, end);

                //Scan Line Below
                if (lineNumber < linesOfInput.Count - 1)
                {
                    var lineBelow = lineNumber + 1;
                    ScanLine(part, linesOfInput[lineBelow], lineBelow, start, end);
                }

                //Scan for symbols, add to parts if valid
                partNumbers.Add(part);
            }
        }
        SetResult1(partNumbers.Where(p => p.Success).Sum(p => p.PartNumber));

        int gearPower = 0;
        var itemsToCheck = partNumbers.Where(p => p.Positions.ContainsKey('*')).ToList();
        var coodinateList = itemsToCheck.Select(i => i.Positions['*']).Distinct().ToList();
        if (coodinateList.Any())
        {
            foreach (var coodinate in coodinateList)
            {
                var gearParts = itemsToCheck.Where(p => p.Positions.ContainsValue(coodinate)).ToList();
                if (gearParts.Count == 2)
                {
                    gearPower = gearPower + (gearParts.First().PartNumber * gearParts.Last().PartNumber);
                }
            }
        }
        SetResult2(gearPower);

        await base.Run();
    }

    private Part ScanLine(Part part, string line, int lineNumber, int start, int end)
    {
        var substring = line.Substring(start, end - start);
        WriteLine(substring);

        var matches = Regex.Matches(substring, "[^.\\w]");
        foreach (Match symbolMatch in matches)
        {
            part.Positions.Add(char.Parse(symbolMatch.Value), new(lineNumber, start + symbolMatch.Index));
        }
        return part;
    }
}
