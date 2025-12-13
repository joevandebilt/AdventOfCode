using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayTwelve;
public class DayTwelveMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTwelveMain() : base(Day.Twelve, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var (shapes, regions) = ParseInput(linesOfInput);        

        SetResult1(-1);
        SetResult2(-1);
        await base.Run();
    }

    private (List<Shape>, List<Region>) ParseInput(List<string> linesOfInput)
    {
        List<Shape> shapes = new();
        List<Region> regions = new();
        for (var i = 0; i < linesOfInput.Count; i++)
        {
            var line = linesOfInput[i];
            if (line.Contains(":") && !line.Contains("x"))
            {
                //Shape definition
                var id = int.Parse(line.Trim(':'));
                line = linesOfInput[++i];

                List<bool[]> shapeLines = new();
                while (!string.IsNullOrWhiteSpace(line))
                {
                    shapeLines.Add(line.Select(l => l == '#').ToArray());
                    line = linesOfInput[++i];
                }
                shapes.Add(new Shape
                {
                    Id = id,
                    Pattern = shapeLines.ToArray()
                });
            }
            else if (line.Contains(":") && line.Contains("x"))
            {
                //Region Definition
                var regionInfo = line.Split(':');
                var dimensions = regionInfo.First().Split('x');
                var width = int.Parse(dimensions[0]);
                var height = int.Parse(dimensions[1]);

                var requirement = regionInfo.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var shapeRequirments = requirement.Select((r, idx) => new { r, idx }).ToDictionary(x => x.idx, x => int.Parse(x.r));

                regions.Add(new Region
                {
                    Id = i,
                    Width = width,
                    Height = height,
                    Requirements = shapeRequirments
                });
            }
        }
        return (shapes, regions);
    }
}
