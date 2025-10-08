using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayFifteen;
public class DayFifteenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayFifteenMain() : base(Day.Fifteen, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);

        var warehouse = new Warehouse();
        var instructions = new List<Direction>();

        for (var row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith('#'))
            {
                //Warehouse config
                warehouse.Height++;

                if (warehouse.Width == 0)
                {
                    warehouse.Width = line.Length;
                }

                for (var col = 0; col < line.Length; col++)
                {
                    switch (line[col])
                    {
                        case 'O':
                            warehouse.Boxes.Add(new Coords(row, col));
                            break;
                        case '@':
                            warehouse.Robot = new Coords(row, col);
                            break;
                        case '#':
                            warehouse.Walls.Add(new Coords(row, col));
                            break;
                        default:
                            continue;
                    }
                }
            }
            else
            {
                //Instruction time, oh yeah
                for (var col = 0; col < line.Length; col++)
                {
                    switch (line[col])
                    {
                        case '<':
                            instructions.Add(Direction.Left);
                            break;
                        case '>':
                            instructions.Add(Direction.Right);
                            break;
                        case 'v':
                            instructions.Add(Direction.Down);
                            break;
                        case '^':
                            instructions.Add(Direction.Up);
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        foreach (var instruction in instructions)
        {
            TryMove(warehouse.Robot, instruction, warehouse);
            //PrintWarehouse(warehouse);
        }

        var gps = warehouse.Boxes.Sum(b => (b.Y * 100) + b.X);
        SetResult1(gps);
        SetResult2(-1);
        await base.Run();
    }


    private bool TryMove(Coords origin, Direction direction, Warehouse warehouse)
    {
        var x = origin.X;
        var y = origin.Y;

        switch (direction)
        {
            case Direction.Up:
                y--;
                break;
            case Direction.Down:
                y++;
                break;
            case Direction.Left:
                x--;
                break;
            case Direction.Right:
                x++;
                break;
        }

        var wall = warehouse.Walls.FirstOrDefault(w => w.X == x && w.Y == y);
        var box = warehouse.Boxes.FirstOrDefault(b => b.X == x && b.Y == y);

        if (wall != null)
        {
            //You can't move into a wall
            return false;
        }
        else if (box != null && !TryMove(box, direction, warehouse))
        {
            return false;
        }

        //Success
        origin.X = x;
        origin.Y = y;
        return true;
    }

    private void PrintWarehouse(Warehouse warehouse)
    {
        Clear();
        for (var row = 0; row < warehouse.Height; row++)
        {
            for (var col = 0; col < warehouse.Width; col++)
            {
                var wall = warehouse.Walls.FirstOrDefault(w => w.X == col && w.Y == row);
                var box = warehouse.Boxes.FirstOrDefault(b => b.X == col && b.Y == row);

                if (wall != null)
                {
                    Write("#");
                }
                else if (box != null)
                {
                    Write("O");
                }
                else if (warehouse.Robot.X == col && warehouse.Robot.Y == row)
                {
                    Write("@");
                }
                else
                {
                    Write(".");
                }
            }
            Write("\r\n");
        }
    }
}
