using AdventOfCode.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared.Models
{
    public record Coordinate
    {
        public Coordinate() { }

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public int X => Column;
        public int Y => Row;

        public string Reference => $"{X}_{Y}";

        public bool MoveDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => MoveUp(),
                Direction.North => MoveUp(),
                Direction.Down => MoveDown(),
                Direction.South => MoveDown(),
                Direction.Left => MoveLeft(),
                Direction.West => MoveLeft(),
                Direction.Right => MoveRight(),
                Direction.East => MoveRight(),
                _ => false,
            };
        }
        public bool MoveUp()
        {
            if (Row == 0)
                return false;

            Row--;
            return true;
        }
        public bool MoveDown()
        {
            Row++;
            return true;
        }
        public bool MoveLeft()
        {
            if (Column == 0)
                return false;

            Column--;
            return true;
        }
        public bool MoveRight()
        {
            Column++;
            return true;
        }
    }
}
