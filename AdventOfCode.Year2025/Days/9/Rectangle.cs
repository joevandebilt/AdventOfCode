using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayNine
{
    public class Rectangle
    {
        public Rectangle(Tile p1, Tile p2)
        {
            if (p2.X < p1.X)
            {
                P2 = p1;
                P1 = p2;
            } 
            else
            {
                P1 = p1;
                P2 = p2;
            }
        }

        public bool Invalid { get; set; } = false;

        public Tile P1 { get; set; }
        public Tile P2 { get; set; }

        public int MaxX => Math.Max(P1.X, P2.X);
        public int MinX => Math.Min(P1.X, P2.X);

        public int MaxY => Math.Max(P1.Y, P2.Y);
        public int MinY => Math.Min(P1.Y, P2.Y);

        public long Area
        {
            get
            {
                long width = (MaxX - MinX) + 1;
                long height = (MaxY - MinY) + 1;
                return width * height;
            }
        }

        public long Perimeter
        {
            get
            {
                long width = Math.Abs(P2.Column - P1.Column);
                long height = Math.Abs(P2.Row - P1.Row);
                return 2 * (width + height);
            }
        }

        public bool InRange(Coordinate P)
        {
            return (P.X > MinX && P.X < MaxX && P.Y > MinY && P.Y < MaxY);
        }
    }
}
