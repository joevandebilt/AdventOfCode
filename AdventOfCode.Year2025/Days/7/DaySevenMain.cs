using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Year2025.Days._7;

namespace AdventOfCode.Year2025.Days.DaySeven;
public class DaySevenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DaySevenMain() : base(Day.Seven, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        int splits = 0;
        List<Beam> beams = new();

        var startPos = linesOfInput.First().IndexOf('s');
        beams.Add(new Beam(0, startPos, 1));

        for (int row = 0; row < linesOfInput.Count - 1; row++)
        {
            for (int col = 0; col < linesOfInput[row].Length; col++)
            {
                var character = linesOfInput[row][col];
                var characterBelow = linesOfInput[row + 1][col];

                if (character == '|' || character == 's')
                {
                    var beam = beams.Single(b => b.Row == row && b.Column == col);

                    if (characterBelow == '.')
                    {
                        linesOfInput[row + 1] = linesOfInput[row + 1].Remove(col, 1).Insert(col, "|");
                        beam.Row = row + 1;
                        beam.Column = col;
                    }
                    else if (characterBelow == '|')
                    {
                        var beamBelow = beams.Single(b => b.Row == row + 1 && b.Column == col);
                        beamBelow.Intensity += beam.Intensity;
                        beams.Remove(beam);
                    }
                    else if (characterBelow == '^')
                    {
                        //Split logic here
                        linesOfInput[row + 1] = linesOfInput[row + 1].Remove(col - 1, 1).Insert(col - 1, "|");
                        linesOfInput[row + 1] = linesOfInput[row + 1].Remove(col + 1, 1).Insert(col + 1, "|");

                        for (int i = -1; i <= 1; i += 2)
                        {
                            var splitBeam = beams.SingleOrDefault(b => b.Row == row + 1 && b.Column == col + i);
                            if (splitBeam == null)
                            {
                                splitBeam = new Beam(row + 1, col + i, beam.Intensity);
                                beams.Add(splitBeam);
                            }
                            else
                            {
                                splitBeam.Intensity += beam.Intensity;
                            }
                        }
                        beams.Remove(beam);
                        splits++;
                    }
                }
            }
            PrintLaserGrid(linesOfInput);
        }
        PrintLaserGrid(linesOfInput);

        SetResult1(splits);
        SetResult2(beams.Sum(b => b.Intensity));
        await base.Run();
    }

    private void PrintLaserGrid(List<string> grid)
    {
        ResetCursor();
        foreach (var line in grid)
        {
            WriteLine(line);
        }
    }
}
