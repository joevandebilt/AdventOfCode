using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;

namespace AdventOfCode.Year2025.Days.DayTwelve;

public class DayTwelveMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTwelveMain() : base(Day.Twelve, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var (shapes, regions) = ParseInput(linesOfInput);

        int validRegions = 0;
        int counter = 0;
        foreach (var region in regions)
        {
            WriteLine($"\t\t\t\t\t\t\t\tTesting region {++counter}/{regions.Count} (Found {validRegions} valid regions)");

            if (ValidGrid(region, shapes))
                validRegions++;
            Clear();
        }

        SetResult1(validRegions);
        SetResult2(-1);
        await base.Run();
    }

    private bool ValidGrid(Region region, List<Shape> shapes)
    {
        var shapesRequired = region.Requirements.Select((qty, idx) => new { Shape = shapes.Single(s => s.Id == idx), Quantity = qty }).OrderByDescending(o => o.Shape.Size).ToList();
        var nextRequirement = shapesRequired.FirstOrDefault(s => s.Quantity > 0);
        if (nextRequirement == null)
        {
            //No more shapes to fille
            return true;
        }

        var freeBlocks = CountNonOverlapping3x3Blocks(region.Grid, 3);
        var blocksNeeded = shapesRequired.Sum(sr => sr.Quantity);
        if (freeBlocks > blocksNeeded)
        {
            return true;
        }

        var totalSpaceNeeded = shapesRequired.Sum(sr => sr.Shape.TilesNeedes * sr.Quantity);
        if (totalSpaceNeeded > region.FreeTiles)
        {
            return false;
        }


        var nextShape = nextRequirement.Shape;

        var shapePattern = nextShape.Pattern;
        for (int rotate = 0; rotate < 4; rotate++)
        {
            shapePattern = shapePattern.Rotate90Clockwise();
            PrintShape(shapePattern);

            for (int y = 0; y <= region.Height - shapePattern.GetLength(0); y++)
            {
                for (int x = 0; x <= region.Width - shapePattern.GetLength(1); x++)
                {
                    var workingRegion = region.Clone();

                    if (workingRegion.TryApplyShape(shapePattern, x, y))
                    {
                        PrintRegion(workingRegion);

                        //Shape places, decrement amount needed and recurse
                        workingRegion.Requirements[nextShape.Id]--;

                        var valid = ValidGrid(workingRegion, shapes);
                        if (valid)
                            return true;
                    }
                }
            }
        }
        return false;
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

                int n = line.Length;
                var pattern = new bool[n, n];

                for (int row = 0; row < n; row++)
                {
                    line = linesOfInput[row + i];
                    for (int y = 0; y < n; y++)
                    {
                        pattern[row, y] = (line[y] == '#');
                    }
                }
                i = i + n;

                shapes.Add(new Shape
                {
                    Id = id,
                    Pattern = pattern
                });
            }
            else if (line.Contains(":") && line.Contains("x"))
            {
                //Region Definition
                var regionInfo = line.Split(':');
                var dimensions = regionInfo.First().Split('x');
                var width = int.Parse(dimensions[0]);
                var height = int.Parse(dimensions[1]);

                if (height > width)
                {
                    var tmp = height;
                    height = width;
                    width = tmp;
                }

                var requirement = regionInfo.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var shapeRequirments = requirement.Select(r => int.Parse(r)).ToArray();

                regions.Add(new Region
                {
                    Id = i,
                    Width = width,
                    Height = height,
                    Requirements = shapeRequirments,
                    Grid = new bool[height, width]
                });
            }
        }
        return (shapes, regions);
    }

    private void PrintRegion(Region region)
    {
        ResetCursor();

        int rows = region.Grid.GetLength(0);
        int cols = region.Grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Write(region.Grid[i, j] ? "█" : ".");
            }
            Write(Environment.NewLine);
        }

        Write(Environment.NewLine);

        WriteLine($"{string.Join("\t", region.Requirements.Select((qty, idx) => $"Shape {idx} x {qty}"))}");
    }

    private void PrintShape(bool[,] shapePattern)
    {
        WriteLine("Testing Shape:\r\n");

        int rows = shapePattern.GetLength(0);
        int cols = shapePattern.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Write(shapePattern[i, j] ? "█" : ".");
            }
            Write(Environment.NewLine);
        }
    }

    public int CountNonOverlapping3x3Blocks(bool[,] grid, int blockSize)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        int count = 0;

        // Tracks which cells are already used by a placed block
        bool[,] used = new bool[rows, cols];

        for (int r = 0; r <= rows - blockSize; r++)
        {
            for (int c = 0; c <= cols - blockSize; c++)
            {
                bool canPlace = true;

                // Check the blockSize×blockSize region
                for (int i = 0; i < blockSize && canPlace; i++)
                {
                    for (int j = 0; j < blockSize; j++)
                    {
                        if (grid[r + i, c + j] || used[r + i, c + j])
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }

                if (canPlace)
                {
                    // Mark the region as used
                    for (int i = 0; i < blockSize; i++)
                        for (int j = 0; j < blockSize; j++)
                            used[r + i, c + j] = true;

                    count++;
                }
            }
        }

        return count;
    }
}
