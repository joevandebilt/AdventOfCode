using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using System.ComponentModel;

namespace AdventOfCode.Year2024.Days.DayTwelve;
public class DayTwelveMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTwelveMain() : base(Day.Twelve, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);
        Dictionary<char, GardenZone> zones = new();
        for (int row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            for (int col = 0; col < line.Length; col++)
            {
                var character = line[col];
                GardenZone zone = null!;
                if (zones.ContainsKey(character))
                {
                    zone = zones[character];
                    zone.Area++;
                }
                else
                {
                    zone = new GardenZone
                    {
                        Identifier = character,
                        Area = 1,
                        Perimeter = 0
                    };
                    zones.Add(character, zone);
                }

                var surrounding = GetCompassDirections(linesOfInput, row, col);
                var fenceNeeded = surrounding.Count(s => !s.Equals(character));
                zone.Perimeter += fenceNeeded;

                zones[character] = zone;
            }
        }

        WriteLine(string.Join("\r\n", zones.Select(z => $"{z.Value.Identifier}: {z.Value.Area} x {z.Value.Perimeter} = {z.Value.Price}")));
        
        SetResult1(zones.Sum(z => z.Value.Price));
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
}
