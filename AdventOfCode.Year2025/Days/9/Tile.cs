using AdventOfCode.Shared.Models;

namespace AdventOfCode.Year2025.Days.DayNine
{
    public enum TileColour
    {
        Empty,
        Red,
        Green
    }

    public record Tile : Coordinate
    {
        public Tile()
        {
        }

        public Tile(int row, int column) : base(row, column)
        {
        }

        public TileColour TileColour { get; set; }
    }
}
