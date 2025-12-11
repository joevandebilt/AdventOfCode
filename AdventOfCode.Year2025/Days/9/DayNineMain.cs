using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayNine;

public class DayNineMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayNineMain() : base(Day.Nine, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var coordinates = new List<Tile>();

        foreach (var line in linesOfInput)
        {
            var parts = line.Split(',');
            var row = int.Parse(parts[0]);
            var column = int.Parse(parts[1]);
            coordinates.Add(new Tile { Column = column, Row = row, TileColour = TileColour.Red });
        }

        var rectangles = coordinates.SelectMany(p1 => coordinates.Where(p2 => p1.Reference != p2.Reference).Select(p2 => new Rectangle(p1, p2)))
            .Distinct()
            .OrderByDescending(r => r.Area)
            .ToHashSet();

        var bigRectangle = rectangles.MaxBy(r => r.Area);
        SetResult1(bigRectangle!.Area);

        //Add green tile lines
        var redCount = coordinates.Count(c => c.TileColour == TileColour.Red);
        for (int i = 0; i < redCount; i++)
        {
            var tile = coordinates[i];

            var nextTile = coordinates[0];
            if (i != redCount - 1)
                nextTile = coordinates[i + 1];

            if (tile.X == nextTile.X)
            {
                for (int y = Math.Min(tile.Y, nextTile.Y) + 1; y < Math.Max(tile.Y, nextTile.Y); y++)
                {
                    coordinates.Add(new Tile { Column = tile.X, Row = y, TileColour = TileColour.Green });
                }
            }
            else if (tile.Y == nextTile.Y)
            {
                for (int x = Math.Min(tile.X, nextTile.X) + 1; x < Math.Max(tile.X, nextTile.X); x++)
                {
                    coordinates.Add(new Tile { Column = x, Row = tile.Y, TileColour = TileColour.Green });
                }
            }
            else
            {
                throw new Exception("Diagonal lines not supported");
            }
        }

        Rectangle? validRectangle = null;
        while (validRectangle == null)
        {
            ResetCursor();
            WriteLine($"Rectangles remaining {rectangles.Count}");

            var rectangle = rectangles.First();
            if (!coordinates.Any(c => rectangle.InRange(c)))
                validRectangle = rectangle;
            else
                rectangles.Remove(rectangle);
        }

        WriteLine($"Best valid rectangle is {validRectangle.P1.Reference}x{validRectangle.P2.Reference} with area {validRectangle.Area}");
        SetResult2(validRectangle.Area);
        await base.Run();
    }
}
