using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DaySixteen;

public class DaySixteenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DaySixteenMain() : base(Day.Sixteen, _debugging) { }

    private IDictionary<string, long> Paths;
    private IList<string> linesOfInput;
    private int maxRow => linesOfInput.Count;
    private int maxCol => linesOfInput[0].Length;
    private long[,,] bestCost; // [row, col, direction]
    private long globalBest;

    public override async Task Run()
    {
        linesOfInput = await LoadFile();
        Paths = new Dictionary<string, long>();

        int startRow = linesOfInput.IndexOf(linesOfInput.First(x => x.Contains("s")));
        int startcol = linesOfInput[startRow].IndexOf("s");

        //Init all cells and directions
        bestCost = new long[maxRow, maxCol, 5]; // 0 = none, 1=U,2=D,3=L,4=R
        for (int r = 0; r < maxRow; r++)
            for (int c = 0; c < maxCol; c++)
                for (int d = 0; d < 5; d++)
                    bestCost[r, c, d] = long.MaxValue;

        globalBest = long.MaxValue;

        FindPath(string.Empty, startRow, startcol, 1000, ' ');

        var bestRoute = Paths.MinBy(p => p.Value);

        Clear();
        PrintMaze(bestRoute.Key);

        SetResult1(globalBest);

        var routes = Paths.Where(p => p.Value == globalBest).SelectMany(p => p.Key.Split('|')).ToList();
        var tiles = routes.Where(r => r.Contains("_")).ToList();
        var uniqueTiles = tiles.Distinct().Order().ToList();

        //+1 for the exit tile which counts!
        SetResult2(uniqueTiles.Count + 1);
        await base.Run();
    }

    private void FindPath(string path, int row, int col, long score, char lastDir)
    {
        //Return if we've already found a better path to the end
        if (score > globalBest)
            return;

        // use per-cell+incoming-direction best-known for stronger pruning
        int dirIdx = DirIndex(lastDir);
        if (score > bestCost[row, col, dirIdx])
            return;
        bestCost[row, col, dirIdx] = score;

        //If we're at the ending log the score and bail
        if (linesOfInput[row][col] == 'e')
        {
            Paths[path] = score;
            globalBest = Math.Min(globalBest, score);
            //PrintMaze(path);
            return;
        }

        var location = $"|{row}_{col}|";
        if (path.Contains(location))
            return;
        else
            path += location;

        // Explore moves preferring to continue in the same direction first
        foreach (var direction in GetOrderedDirections(lastDir))
        {
            int nr = row, nc = col;
            switch (direction)
            {
                case 'U': nr = row - 1; break;
                case 'D': nr = row + 1; break;
                case 'L': nc = col - 1; break;
                case 'R': nc = col + 1; break;
            }

            if (nr < 0 || nr >= maxRow || nc < 0 || nc >= maxCol) continue;
            if (linesOfInput[nr][nc] == '#') continue;

            var cost = MoveCost(lastDir, direction);
            var newScore = score + cost;

            FindPath(path + direction, nr, nc, newScore, direction);
        }
    }

    private int DirIndex(char d)
    {
        return d switch
        {
            ' ' => 0,
            'U' => 1,
            'D' => 2,
            'L' => 3,
            'R' => 4,
            _ => 0,
        };
    }

    private IEnumerable<char> GetOrderedDirections(char lastDir)
    {
        var baseDirs = new List<char> { 'U', 'D', 'L', 'R' };
        if (lastDir == ' ' || !baseDirs.Contains(lastDir))
            return baseDirs;

        // put same direction first
        var ordered = new List<char> { lastDir };
        ordered.AddRange(baseDirs.Where(d => d != lastDir));
        return ordered;
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
