using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using System.Text;

namespace AdventOfCode.Year2023.Days.DayTen;
public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    private List<Tile> _tileDictionary = new()
    {
        new ('|', Direction.North, Direction.South),
        new('-', Direction.East, Direction.West),
        new('L', Direction.North, Direction.East),
        new('J', Direction.North, Direction.West),
        new('7', Direction.South, Direction.West),
        new('F', Direction.South, Direction.East)
    };

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);

        //Work out starting position
        int row, col;
        Tuple<int, int> startingPosition;
        int startRow = linesOfInput.IndexOf(linesOfInput.Single(line => line.Contains('S')));
        int startCol = linesOfInput[startRow].IndexOf('S');

        startingPosition = new(startRow, startCol);

        //Work out start tile
        var possibilities = _tileDictionary.Select(td => td).ToList();
        foreach (Direction possibleDirection in Enum.GetValues(typeof(Direction)))
        {
            (row, col) = DirectionOfTravel(possibleDirection);
            int checkRow = startRow + row;
            int checkCol = startCol + col;

            //Out of bounds
            if (checkRow < 0 || checkRow >= linesOfInput.Count || checkCol < 0 || checkCol >= linesOfInput.First().Length)
                continue;

            var tileId = linesOfInput[checkRow][checkCol];
            if (tileId == '.')
                continue;

            var tile = _tileDictionary.Single(td => td.Identifier == tileId);

            var inverse = inverseDirection(possibleDirection);
            if (tile.DirectionOne == inverse || tile.DirectionTwo == inverse)
            {
                possibilities = possibilities.Where(p => p.DirectionOne == possibleDirection || p.DirectionTwo == possibleDirection).ToList();
            }
        }

        if (possibilities.Count != 1)
        {
            throw new Exception("YOU FUCKED UP");
        }
        var startTile = possibilities.Single();
        linesOfInput[startRow] = linesOfInput[startRow].ReplaceCharAtIndex(startCol, startTile.Identifier);

        //Path the animal
        int stepsToMaxDistance = 0;
        double maxDistanceFromStart = 0;

        row = startRow;
        col = startCol;
        Direction directionOfTravel = startTile.DirectionOne;

        bool journeyFinished = false;
        List<Tuple<int, int>> mazeCoodinates = new();
        while (!journeyFinished)
        {
            //Record where we stepped
            mazeCoodinates.Add(new(row, col));

            var (rowDirection, colDirection) = DirectionOfTravel(directionOfTravel);
            row = row + rowDirection;
            col = col + colDirection;

            if (row == startRow && col == startCol)
            {
                journeyFinished = true;
            }
            else
            {
                var nextTile = _tileDictionary.Single(td => td.Identifier == linesOfInput[row][col]);
                if (nextTile.DirectionOne == inverseDirection(directionOfTravel))
                {
                    directionOfTravel = nextTile.DirectionTwo;
                }
                else if (nextTile.DirectionTwo == inverseDirection(directionOfTravel))
                {
                    directionOfTravel = nextTile.DirectionOne;
                }
                else
                {
                    throw new Exception("I DONE GOOFED WITH MAPPING");
                }
            }
        }
        WriteLine($"I took {mazeCoodinates.Count} steps");
        WriteLine($"The furthest position from start is {maxDistanceFromStart} which will take {stepsToMaxDistance} inside the loop");
        SetResult1(mazeCoodinates.Count / 2);


        List<string> map = new();
        int nestedTiles = 0;
        for (row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];

            StringBuilder mapLine = new StringBuilder();
            for (col = 0; col < line.Length; col++)
            {
                char c = line[col];
                if (mazeCoodinates.Any(coodinate => coodinate.Item1 == row && coodinate.Item2 == col))
                {
                    mapLine.Append(c);
                }
                else
                {
                    var tilesNorth = mazeCoodinates.Count(cood => cood.Item1 < row && cood.Item2 == col);
                    var tilesEast = mazeCoodinates.Count(cood => cood.Item1 == row && cood.Item2 > col);
                    var tilesSouth = mazeCoodinates.Count(cood => cood.Item1 > row && cood.Item2 == col);
                    var tilesWest = mazeCoodinates.Count(cood => cood.Item1 == row && cood.Item2 < col);

                    var validNorth = (tilesNorth > 0) && (tilesNorth % 2 == 1);
                    var validEast = (tilesEast > 0) && (tilesEast % 2 == 1);
                    var validSouth = (tilesSouth > 0) && (tilesSouth % 2 == 1);
                    var validWest = (tilesWest > 0) && (tilesWest % 2 == 1);

                    if (validNorth && validEast && validSouth && validWest)
                    {
                        mapLine.Append('I');
                        nestedTiles++;
                    }
                    else
                    {
                        mapLine.Append('O');
                    }
                }
            }
            map.Add(mapLine.ToString());
        }
        WriteLine(string.Join("\n", map));
        SetResult2(nestedTiles);
        await base.Run();

    }

    private (int, int) DirectionOfTravel(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return (-1, 0);
            case Direction.East:
                return (0, 1);
            case Direction.South:
                return (1, 0);
            case Direction.West:
                return (0, -1);
            default:
                return (0, 0);
        }
    }

    private Direction inverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;
            case Direction.South:
                return Direction.North;
            case Direction.West:
                return Direction.East;
            case Direction.East:
                return Direction.West;
            default:
                return Direction.North;
        }
    }
}
