using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayTwelve;
public class DayTwelveMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTwelveMain() : base(Day.Twelve, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);
        List<GardenZone> Zones = new();
        List<Region> Regions = new();
        for (int row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            for (int col = 0; col < line.Length; col++)
            {
                var character = line[col];

                Zones.Add(new GardenZone
                {
                    Identifier = character,
                    Coordinate = new(row, col)
                });
            }
        }

        var ZoneTypes = Zones.Select(z => z.Identifier).Distinct().ToList();
        foreach (var zoneType in ZoneTypes)
        {
            //Get a region
            while (Zones.Count(z => z.Identifier == zoneType) > 0)
            {
                var region = new Region
                {
                    Id = $"{zoneType}_{Regions.Count(r => r.Identifier == zoneType).ToString().PadLeft(2, '0')}",
                    Identifier = zoneType,
                };

                var nextPoint = Zones.FirstOrDefault(z => z.Identifier == zoneType);
                while (nextPoint != null)
                {
                    region.Zones.Add(nextPoint);
                    Zones.Remove(nextPoint);

                    var surrounding = GetCompassDirections(linesOfInput, nextPoint.Coordinate.Item1, nextPoint.Coordinate.Item2);
                    var fenceNeeded = surrounding.Count(s => !s.Equals(zoneType));
                    region.Perimeter += fenceNeeded;

                    nextPoint = Zones.FirstOrDefault(z => z.Identifier == zoneType && region.Zones.Any(rz => IsNeighbour(z, rz)));
                }
                Regions.Add(region);
            }
        }

        foreach (var region in Regions)
        {
            WriteLine($"{region.Id}: Region of {region.Identifier} with price {region.Area} x {region.Perimeter} = {region.Price}");
        }

        //Debug
        //var region = Regions.FirstOrDefault(r => r.Id == "H_12")

        WriteLine($"Total Area {Regions.Sum(r => r.Area)}");

        SetResult1(Regions.Sum(z => z.Price));
        SetResult2(-1);
        await base.Run();
    }

    private string GetCompassDirections(List<string> grid, int row, int col)
    {
        List<char> characters = new();
        for (int rowDirection = -1; rowDirection <= 1; rowDirection++)
        {
            for (int colDirection = -1; colDirection <= 1; colDirection++)
            {
                if (Math.Abs(rowDirection + colDirection) == 1)
                {
                    var nextRow = row + rowDirection;
                    var nextCol = col + colDirection;

                    if (nextRow >= 0 && nextRow < grid.Count && nextCol >= 0 && nextCol < grid[row].Length)
                    {
                        characters.Add(grid[nextRow][nextCol]);
                    }
                    else
                    {
                        //Out of bounds requires fence parts
                        characters.Add('.');
                    }
                }
            }
        }
        return new string(characters.ToArray());
    }

    private bool IsNeighbour(GardenZone z1, GardenZone z2)
    {
        return IsNeighbour(z1.Coordinate.Item1, z1.Coordinate.Item2, z2);
    }

    private bool IsNeighbour(int row, int col, GardenZone zone)
    {
        if (zone.Coordinate == null)
            return false;

        var zoneRow = zone.Coordinate.Item1;
        var zoneCol = zone.Coordinate.Item2;

        var rowChange = Math.Abs(zoneRow - row);
        var colChange = Math.Abs(zoneCol - col);

        var up = rowChange == -1 && colChange == 0;
        var down = rowChange == 1 && colChange == 0;
        var left = colChange == -1 && rowChange == 0;
        var right = colChange == 1 && rowChange == 0;

        if ((up || down) && (left || right))
            return false;

        var neighbour = (up || down || left || right);
        return neighbour;
    }
}
