using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Text;
namespace AdventOfCode.Year2024.Days.DayFifteen;
public class DayFifteenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFifteenMain() : base(Day.Fifteen, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);

        var warehouse = new Warehouse();
        var bigWarehouse = new Warehouse();
        var instructions = new List<Direction>();

        int id = 0;

        for (var row = 0; row < linesOfInput.Count; row++)
        {
            var line = linesOfInput[row];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith('#'))
            {
                //Warehouse config
                warehouse.Height++;
                bigWarehouse.Height++;

                if (warehouse.Width == 0)
                {
                    warehouse.Width = line.Length;
                    bigWarehouse.Width = line.Length * 2;
                }

                for (var col = 0; col < line.Length; col++)
                {
                    switch (line[col])
                    {
                        case 'O':
                            warehouse.Boxes.Add(new Coords(id++, row, col));
                            bigWarehouse.BigBoxes.Add(new BigBox(id++, row, col * 2));
                            break;
                        case '@':
                            warehouse.Robot = new Coords(id++, row, col);
                            bigWarehouse.Robot = new Coords(id++, row, col * 2);
                            break;
                        case '#':
                            warehouse.Walls.Add(new Coords(id++, row, col));
                            bigWarehouse.Walls.Add(new Coords(id++, row, col * 2));
                            bigWarehouse.Walls.Add(new Coords(id++, row, col * 2 + 1));
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
            //PrintWarehouse(warehouse);
            if (TryMove(warehouse.Robot, instruction, warehouse))
                warehouse.ApplyMoves();
            else
                warehouse.ClearMoves();
        }
        PrintWarehouse(warehouse);

        var gps = warehouse.Boxes.Sum(b => (b.Y * 100) + b.X);
        SetResult1(gps);

        Clear();
        foreach (var instruction in instructions)
        {
            PrintWarehouse(bigWarehouse);
            if (TryMove(bigWarehouse.Robot, instruction, bigWarehouse))
                bigWarehouse.ApplyMoves();
            else
                bigWarehouse.ClearMoves();
        }
        PrintWarehouse(bigWarehouse);

        gps = bigWarehouse.BigBoxes.Sum(b => (b.LeftSide.Y * 100) + b.LeftSide.X);
        SetResult2(gps);
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
        var bigBox = warehouse.BigBoxes.FirstOrDefault(b => (b.LeftSide.X == x && b.LeftSide.Y == y) || (b.RightSide.X == x && b.RightSide.Y == y));

        if (wall != null)
        {
            //You can't move into a wall
            return false;
        }
        else if (box != null && !TryMove(box, direction, warehouse))
        {
            //Can't move box
            return false;
        }
        else if (bigBox != null && !TryMove(bigBox, direction, warehouse))
        {
            return false;
        }

        //Success
        warehouse.NewBoxPositions.Add(new Coords(origin.Id, y, x));
        return true;
    }

    private bool TryMove(BigBox origin, Direction direction, Warehouse warehouse)
    {
        BigBox newPos = new(origin.Id, origin.LeftSide.Y, origin.LeftSide.X);

        switch (direction)
        {
            case Direction.Up:
                newPos.LeftSide.Y--;
                newPos.RightSide.Y--;
                break;
            case Direction.Down:
                newPos.LeftSide.Y++;
                newPos.RightSide.Y++;
                break;
            case Direction.Left:
                newPos.LeftSide.X--;
                newPos.RightSide.X--;
                break;
            case Direction.Right:
                newPos.LeftSide.X++;
                newPos.RightSide.X++;
                break;
        }

        var leftSideWall = warehouse.Walls.FirstOrDefault(w => w.X == newPos.LeftSide.X && w.Y == newPos.LeftSide.Y);
        var rightSideWall = warehouse.Walls.FirstOrDefault(w => w.X == newPos.RightSide.X && w.Y == newPos.RightSide.Y);

        var directCollision = warehouse.BigBoxes.FirstOrDefault(bb => bb.Reference == newPos.Reference);
        var leftCollision = warehouse.BigBoxes.FirstOrDefault(bb => bb.RightSide.Reference == newPos.LeftSide.Reference && direction != Direction.Right);
        var rightCollision = warehouse.BigBoxes.FirstOrDefault(bb => bb.LeftSide.Reference == newPos.RightSide.Reference && direction != Direction.Left);

        if (leftSideWall != null || rightSideWall != null)
        {
            return false;
        }
        else
        {
            if (directCollision != null)
            {
                if (!TryMove(directCollision, direction, warehouse))
                {
                    return false;
                }
            }
            else if (leftCollision != null && rightCollision == null)
            {
                if (!TryMove(leftCollision, direction, warehouse))
                {
                    return false;
                }
            }
            else if (leftCollision == null && rightCollision != null)
            {
                if (!TryMove(rightCollision, direction, warehouse))
                {
                    return false;
                }
            }
            else if (leftCollision != null && rightCollision != null && leftCollision.Id == rightCollision.Id)
            {
                if (!TryMove(rightCollision, direction, warehouse))
                {
                    return false;
                }
            }
            else if (leftCollision != null && rightCollision != null && leftCollision.Id != rightCollision.Id)
            {
                var leftMoveSuccess = TryMove(leftCollision, direction, warehouse);
                var rightMoveSuccess = TryMove(rightCollision, direction, warehouse);
                if (!leftMoveSuccess || !rightMoveSuccess)
                {
                    return false;
                }
            }
        }

        warehouse.NewBigBoxPositions.Add(newPos);
        return true;
    }

    private void PrintWarehouse(Warehouse warehouse)
    {
        if (_debugging)
        {
            StringBuilder sb = new StringBuilder();
            for (var row = 0; row < warehouse.Height; row++)
            {
                for (var col = 0; col < warehouse.Width; col++)
                {
                    var wall = warehouse.Walls.FirstOrDefault(w => w.X == col && w.Y == row);
                    var box = warehouse.Boxes.FirstOrDefault(b => b.X == col && b.Y == row);
                    var leftSide = warehouse.BigBoxes.FirstOrDefault(b => b.LeftSide.X == col && b.LeftSide.Y == row);
                    var rightSide = warehouse.BigBoxes.FirstOrDefault(b => b.RightSide.X == col && b.RightSide.Y == row);

                    if (wall != null)
                    {
                        sb.Append("#");
                    }
                    else if (box != null)
                    {
                        sb.Append("O");
                    }
                    else if (warehouse.Robot.X == col && warehouse.Robot.Y == row)
                    {
                        sb.Append("@");
                    }
                    else if (leftSide != null)
                    {
                        sb.Append("[");
                    }
                    else if (rightSide != null)
                    {
                        sb.Append("]");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }
                sb.Append("\r\n");
            }
            ResetCursor();
            Write(sb.ToString());
        }
    }
}
