using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayFour;
public class DayFourMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFourMain() : base(Day.Four, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        string searchWord = "xmas";
        int xmasCount = ScanForWord(linesOfInput, searchWord);
        SetResult1(xmasCount);

        int masCount = 0;
        searchWord = "mas";
        for (var row = 1; row < linesOfInput.Count - 1; row++)
        {
            for (var col = 1; col < linesOfInput[row].Length - 1; col++)
            {
                if (linesOfInput[row][col] == 'a')
                {
                    //m=109 a=97 s=115 == 321
                    char[] characters = {
                        linesOfInput[row - 1][col - 1],
                        linesOfInput[row - 1][col + 1],
                        'a',
                        linesOfInput[row + 1][col + 1],
                        linesOfInput[row + 1][col - 1],
                    };

                    var northwest = (int)linesOfInput[row - 1][col - 1];
                    var northeast = (int)linesOfInput[row - 1][col + 1];
                    var southeast = (int)linesOfInput[row + 1][col + 1];
                    var southwest = (int)linesOfInput[row + 1][col - 1];

                    if (northwest + southeast == 224 && northeast + southwest == 224)
                    {
                        WriteLine($"{characters[0]}.{characters[1]}");
                        WriteLine($".{characters[2]}.");
                        WriteLine($"{characters[3]}.{characters[4]}");
                        WriteLine($"Pass\n");
                        masCount++;
                    }
                    else
                    {
                        WriteLine($"Fail\n");
                    }
                }
            }
        }


        SetResult2(masCount);
        await base.Run();
    }

    private int ScanForWord(List<string> lines, string searchWord)
    {
        int counter = 0;
        for (var row = 0; row < lines.Count; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                //This could be the start of a word
                if (lines[row][col] == 'x')
                {
                    //Can we look north
                    if (North(lines, row, col, searchWord))
                    {
                        if (lines[row - 1][col] == 'm' && lines[row - 2][col] == 'a' && lines[row - 3][col] == 's')
                            counter++;
                    }

                    //Can we look South
                    if (South(lines, row, col, searchWord))
                    {
                        if (lines[row + 1][col] == 'm' && lines[row + 2][col] == 'a' && lines[row + 3][col] == 's')
                            counter++;
                    }

                    //Can we look West
                    if (West(lines, row, col, searchWord))
                    {
                        if (lines[row][col - 1] == 'm' && lines[row][col - 2] == 'a' && lines[row][col - 3] == 's')
                            counter++;
                    }

                    //Can we look East
                    if (East(lines, row, col, searchWord))
                    {
                        if (lines[row][col + 1] == 'm' && lines[row][col + 2] == 'a' && lines[row][col + 3] == 's')
                            counter++;
                    }

                    //Can we look Northeast
                    if (North(lines, row, col, searchWord) && East(lines, row, col, searchWord))
                    {
                        if (lines[row - 1][col + 1] == 'm' && lines[row - 2][col + 2] == 'a' && lines[row - 3][col + 3] == 's')
                            counter++;
                    }

                    //Can we look SouthEast
                    if (South(lines, row, col, searchWord) && East(lines, row, col, searchWord))
                    {
                        if (lines[row + 1][col + 1] == 'm' && lines[row + 2][col + 2] == 'a' && lines[row + 3][col + 3] == 's')
                            counter++;
                    }

                    //Can we look SouthWest
                    if (South(lines, row, col, searchWord) && West(lines, row, col, searchWord))
                    {
                        if (lines[row + 1][col - 1] == 'm' && lines[row + 2][col - 2] == 'a' && lines[row + 3][col - 3] == 's')
                            counter++;
                    }

                    //Can we look Northwest
                    if (North(lines, row, col, searchWord) && West(lines, row, col, searchWord))
                    {
                        if (lines[row - 1][col - 1] == 'm' && lines[row - 2][col - 2] == 'a' && lines[row - 3][col - 3] == 's')
                            counter++;
                    }
                }
            }
        }
        return counter;
    }

    private bool North(List<string> lines, int row, int col, string searchWord)
    {
        return (row >= searchWord.Length - 1);
    }
    private bool East(List<string> lines, int row, int col, string searchWord)
    {
        return (col <= lines[row].Length - searchWord.Length);
    }
    private bool South(List<string> lines, int row, int col, string searchWord)
    {
        return (row <= lines.Count - searchWord.Length);
    }
    private bool West(List<string> lines, int row, int col, string searchWord)
    {
        return (col >= searchWord.Length - 1);
    }
}
