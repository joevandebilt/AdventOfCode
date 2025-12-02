using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode.Year2024.Days.DaySixteen;

public class DaySixteenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DaySixteenMain() : base(Day.Sixteen, _debugging) { }

    private IDictionary<string, long> Paths;
    private IList<string> linesOfInput;
    private int maxRow => linesOfInput.Count;
    private int maxCol => linesOfInput[0].Length;

    private long bestScore()
    {
        if (Paths.Count == 0)
            return long.MaxValue;
        else
            return Paths.MinBy(p => p.Value).Value;
    }

    public override async Task Run()
    {
        linesOfInput = await LoadFile();
        Paths = new Dictionary<string, long>();
        int startRow = 0, startcol = 0;

        startRow = linesOfInput.IndexOf(linesOfInput.First(x => x.Contains("s")));
        startcol = linesOfInput[startRow].IndexOf("s");

        FindPath(string.Empty, startRow, startcol, 1000);

        var bestRoute = Paths.MinBy(p => p.Value);

        Clear();
        PrintMaze(bestRoute.Key);

        SetResult1(bestRoute.Value);
        SetResult2(-1);
        await base.Run();
    }

    private void FindPath(string path, int row, int col, long score)
    {
        if (score > bestScore())
            return;

        char lastPos = ' ', opposite = ' ';
        if (!string.IsNullOrWhiteSpace(path))
        {
            lastPos = path.Last();
        }

        //If we're at the ending log the score and bail
        if (linesOfInput[row][col] == 'e')
        {
            Paths.Add(path, score);
            PrintMaze(path);
            return;
        }

        var location = $"|{row}_{col}|";
        if (path.Contains(location))
            return;
        else
            path += location;

        //Up
        if (row - 1 >= 0 && linesOfInput[row - 1][col] != '#')
        {
            var cost = MoveCost(lastPos, 'U');
            FindPath(path + 'U', row - 1, col, score + cost);
        }

        //Down
        if (row + 1 < maxRow && linesOfInput[row + 1][col] != '#')
        {
            var cost = MoveCost(lastPos, 'D');
            FindPath(path + 'D', row + 1, col, score + cost);
        }

        //Left
        if (col - 1 >= 0 && linesOfInput[row][col - 1] != '#')
        {
            var cost = MoveCost(lastPos, 'L');
            FindPath(path + 'L', row, col - 1, score + cost);
        }

        //Right
        if (col + 1 < maxCol && linesOfInput[row][col + 1] != '#')
        {
            var cost = MoveCost(lastPos, 'R');
            FindPath(path + 'R', row, col + 1, score + cost);
        }
    }

    private int MoveCost(char lastDir, char direction)
    {
        if (lastDir != ' ' && lastDir != direction)
            return 1001;
        return 1;
    }

    private void PrintMaze(string path)
    {
        Clear();
        for (int r = 0; r < maxRow; r++)
        {
            for (int c = 0; c < maxCol; c++)
            {
                var location = $"|{r}_{c}|";
                if (path.Contains(location))
                    Write("*");
                else
                    Write(linesOfInput[r][c].ToString());
            }
            WriteLine("");
        }
    }
}
