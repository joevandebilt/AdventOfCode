using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayTen;
public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        List<TrailheadLocation> StartLocations = new();
        for (int row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            for (int col = 0; col < line.Length; col++)
            {
                if (line[col] == '0')
                {
                    StartLocations.Add(new TrailheadLocation
                    {
                        Column = col,
                        Row = row,
                        Height = 0,
                        Score = 0
                    });
                }
            }
        }
        WriteLine($"Got {StartLocations.Count} start locations");

        foreach (var start in StartLocations)
        {
            List<Tuple<int, int>> positions = new();
            TraverseDirection(linesOfInput, positions, start.Height, start.Row, start.Column);
            start.Score = positions.GroupBy(p => new { p.Item1, p.Item2 }).Count();
            start.Rating = positions.Count;
        }

        SetResult1(StartLocations.Sum(s => s.Score));
        SetResult2(StartLocations.Sum(s => s.Rating));
        await base.Run();
    }

    public void TraverseDirection(List<string> grid, List<Tuple<int, int>> positions, int height, int row, int col)
    {
        if (height == 9)
        {
            positions.Add(new(row, col));
        }
        
        int nextHeight;        
        for (int rowDirection = -1; rowDirection <= 1; rowDirection++)
        {
            for (int colDirection = -1; colDirection <= 1; colDirection++)
            {
                if (Math.Abs(rowDirection + colDirection) == 1)
                {
                    //Get next position
                    var nextRow = row + rowDirection;
                    var nextCol = col + colDirection;

                    //Inside bounds of grid
                    if (nextRow >= 0 && nextRow < grid.Count && nextCol >= 0 && nextCol < grid[row].Length)
                    {
                        //Character is numeric 
                        if (int.TryParse($"{grid[nextRow][nextCol]}", out nextHeight) && nextHeight == height + 1)
                        {
                            TraverseDirection(grid, positions, nextHeight, nextRow, nextCol);
                        }
                    }
                }
            }
        }
    }
}
