using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayFour;
public class DayFourMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFourMain() : base(Day.Four, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        List<Coordinate> coordinates = new();
        int changes = int.MaxValue;

        while (changes > 0)
        {
            PrintGrid(linesOfInput);
            var freeBoxes = TraverseGrid(linesOfInput);
            changes = freeBoxes.Count;
            foreach (var box in freeBoxes)
            {
                // Mark the box as unoccupied
                var line = linesOfInput[box.Row];
                var chars = line.ToCharArray();
                chars[box.Column] = '.';
                line = new string(chars);
                linesOfInput[box.Row] = line;
            }
         
            if (coordinates.Count == 0)
            {
                SetResult1(freeBoxes.Count);
            }
            coordinates.AddRange(freeBoxes);
            
        }
        SetResult2(coordinates.Count);
        await base.Run();
    }

    private void PrintGrid(List<string> linesOfInput)
    {
        Clear();
        foreach (var line in linesOfInput)
        {
            WriteLine(line);
        }
    }

    private List<Coordinate> TraverseGrid(IList<string> linesOfInput)
    {
        List<Coordinate> coordinates = new();
        for (int row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            for (int column = 0; column < line.Length; column++)
            {
                if (line[column] == '@')
                {
                    //Scan directions
                    int usedSpace = 0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0)
                                continue;

                            int scanRow = row + x;
                            int scanColumn = column + y;
                            if (scanRow < 0 || scanRow >= linesOfInput.Count || scanColumn < 0 || scanColumn >= linesOfInput[scanRow].Length)
                                continue;

                            if (linesOfInput[scanRow][scanColumn] == '@')
                            {
                                usedSpace++;
                            }
                        }
                    }

                    if (usedSpace < 4)
                    {
                        coordinates.Add(new Coordinate { Row = row, Column = column });
                    }
                }
            }
        }
        return coordinates;
    }
}
