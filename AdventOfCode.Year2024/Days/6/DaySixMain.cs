using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DaySix;
public class DaySixMain : AdventOfCodeDay
{
    private char guardIcon = '^';
    private char[] guardIcons = { '^', '>', 'v', '<' };

    private const bool _debugging = false;
    public DaySixMain() : base(Day.Six, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        var startRowLine = linesOfInput.Single(row => row.Contains(guardIcon));

        int row = linesOfInput.IndexOf(startRowLine);
        int col = startRowLine.IndexOf(guardIcon);

        var originalRoute = linesOfInput.Select(line => line).ToList();
        TravelGrid(originalRoute, row, col, out bool dontCare);

        int positionsVisited = string.Concat(originalRoute).Count(character => guardIcons.Contains(character));
        SetResult1(positionsVisited);

        //Now for each position visited
        bool loops = false;
        int loopCounter = 0;
        List<string> testGrid;

        for (int i = 0; i < originalRoute.Count; i++)
        {
            var currentLine = originalRoute[i];
            for (int j = 0; j < currentLine.Length; j++)
            {
                //This is a visited position and we're not at the start position
                if (guardIcons.Contains(currentLine[j])/* && i != row && j != col*/)
                {
                    WriteLine($"Checking Row {i} Col {j} for looping");

                    //Replace the coordinate in the original grid with an obstacle
                    testGrid = linesOfInput.Select(line => line).ToList();
                    testGrid[i] = ReplaceCharAtIndex(testGrid[i], j, '#');

                    TravelGrid(testGrid, row, col, out loops);
                    if (loops)
                    {
                        WriteLine($"Row {i} Col {j} will loop with obstacle (Found {loopCounter})");
                        loopCounter++;
                    }
                    testGrid = new();
                }
            }
        }
        SetResult2(loopCounter);
        await base.Run();
    }

    private void PrintGrid(List<string> grid)
    {
        WriteLine(string.Join("\n", grid));
    }

    private void VisualizeStep(List<string> grid, int row, int col)
    {
        if (_debugging)
        {
            int size = 4;
            var lines = grid.Skip(int.Max(0, row - size)).Take((size * 2) + 1);
            Console.Clear();
            foreach (var line in lines)
            {
                int startIndex = int.Max(0, col - size);
                int length = int.Min((line.Length - startIndex), ((size * 2) + 1));
                Console.WriteLine(line.Substring(startIndex, length));
            }
        }
    }

    private List<string> TravelGrid(List<string> grid, int row, int col, out bool loops)
    {
        //Set start direction of travel
        guardIcon = '^';
        int rowDirection = -1;
        int colDirection = 0;

        //Get the boundaries of the array
        int maxRow = grid.First().Length;
        int maxCol = grid.Count();

        //Set the next available position
        int nextRowPos = row + rowDirection;
        int nextColPos = col + colDirection;

        loops = false;
        bool travelFinished = false;

        HashSet<string> positionsVisited = new();

        //While guard position is inside the grid
        while (!travelFinished)
        {
            //Record current position and direction as a place we've been
            positionsVisited.Add(coodinateAsString(row, col, guardIcon));

            //Check to see if we have a win condition
            string nextCoodinate = coodinateAsString(nextRowPos, nextColPos, guardIcon);
            if (nextRowPos < 0 || nextRowPos >= maxRow || nextColPos < 0 || nextColPos >= maxCol)
            {
                //Escaped grid
                travelFinished = true;
            }
            else if (positionsVisited.Contains(nextCoodinate))
            {
                //Looped Grid
                VisualizeStep(grid, row, col);
                loops = true;
                travelFinished = true;
            }
            else
            {
                //If the next position is an obstacle
                if (grid[nextRowPos][nextColPos] == '#')
                {
                    //Rotate 90
                    switch (guardIcon)
                    {
                        case '^':
                            guardIcon = '>';
                            rowDirection = 0;
                            colDirection = 1;
                            break;
                        case '>':
                            guardIcon = 'v';
                            rowDirection = 1;
                            colDirection = 0;
                            break;
                        case 'v':
                            guardIcon = '<';
                            rowDirection = 0;
                            colDirection = -1;
                            break;
                        case '<':
                            guardIcon = '^';
                            rowDirection = -1;
                            colDirection = 0;
                            break;
                    }
                }
                else
                {
                    //Move
                    grid[row] = ReplaceCharAtIndex(grid[row], col, guardIcon);
                    row = nextRowPos;
                    col = nextColPos;
                    grid[row] = ReplaceCharAtIndex(grid[row], col, guardIcon);
                }

                //Update the next position
                nextRowPos = row + rowDirection;
                nextColPos = col + colDirection;
            }
        }
        //PrintGrid(grid);
        return grid;
    }

    private string ReplaceCharAtIndex(string input, int index, char ch)
    {
        char[] characters = input.ToCharArray();
        characters[index] = ch;
        return new string(characters);
    }

    private string coodinateAsString(int row, int col, char guardIcon)
    {
        return $"{row.ToString().PadLeft(3, '0')}{col.ToString().PadLeft(3, '0')}{guardIcon}";
    }
}
