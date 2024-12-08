using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode.Year2024.Days.DayEight;
public class DayEightMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    char[] uniqueFrequencies;
    public DayEightMain() : base(Day.Eight, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);
        List<AntiNode> Antinodes = new();

        //Map the grid and get unique frequencies
        uniqueFrequencies = string.Concat(linesOfInput).Distinct().Order().ToArray();
        var gridMap = linesOfInput.SelectMany((line, row) => line.Select((character, col) => new Antenna
        {
            Identifier = character,
            Row = row,
            Column = col
        })).ToList();

        //For each Frequency
        foreach (var frequency in uniqueFrequencies.Where(c => c != '.'))
        {
            //Get all antenna locations
            var antennaLocations = gridMap.Where(g => g.Identifier == frequency).ToList();
            foreach (var antennaLocation in antennaLocations)
            {
                //Compare to other atennas to find direction and viability for antinode
                foreach (var secondaryAtenanna in antennaLocations
                    .Where(a => a.Row != antennaLocation.Row && a.Column != antennaLocation.Column))
                {
                    var rowDirection = (antennaLocation.Row - secondaryAtenanna.Row);
                    var colDirection = (antennaLocation.Column - secondaryAtenanna.Column);

                    int proposedRow = antennaLocation.Row + rowDirection;
                    int proposedCol = antennaLocation.Column + colDirection;

                    if (proposedRow >= 0 && proposedRow < linesOfInput.Count
                        && proposedCol >= 0 && proposedCol < linesOfInput.First().Length
                        && !Antinodes.Any(a => a.Identifier == frequency
                            && a.Row == proposedRow
                            && a.Column == proposedCol)
                        )
                    {
                        Antinodes.Add(new AntiNode
                        {
                            Identifier = frequency,
                            Row = proposedRow,
                            Column = proposedCol
                        });
                    }
                }
            }
        }
        WriteLine($"There are {Antinodes.Count} antinodes possible on the map");

        //Print the maps for the debugger
        PrintMaps(linesOfInput, Antinodes, gridMap);

        SetResult1(Antinodes.Select(a => new { a.Row, a.Column }).Distinct().Count());
        SetResult2(-1);
        await base.Run();
    }

    private void PrintMaps(List<string> input, List<AntiNode> antiNodes, List<Antenna> antennas)
    {
        List<string> mapGrid = new();
        for (int row = 0; row < input.Count; row++)
        {
            StringBuilder mapLine = new StringBuilder();
            var line = input[row];
            for (int col = 0; col < line.Length; col++)
            {
                if (antiNodes.Any(a => a.Row == row && a.Column == col))
                {
                    mapLine.Append('#');
                }
                else
                {
                    mapLine.Append(line[col]);
                }
            }
            mapGrid.Add(mapLine.ToString());
        }
        WriteLine(string.Join("\n", mapGrid));
    }
}
