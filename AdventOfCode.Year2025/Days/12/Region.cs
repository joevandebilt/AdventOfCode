using AdventOfCode.Shared.Extensions;

namespace AdventOfCode.Year2025.Days.DayTwelve;

public class Region
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Tiles => Width * Height;
    public int FreeTiles => Grid.Cast<bool>().Count(x => !x);

    public int[] Requirements { get; set; } = null!;

    public bool[,] Grid { get; set; }

    public Region Clone()
    {

        return new Region
        {
            Id = Id,
            Width = Width,
            Height = Height,
            Requirements = Requirements.CloneArray(),
            Grid = Grid.CloneArray()
        };
    }

    public bool TryApplyShape(bool[,] shape, int offsetX, int offsetY)
    {
        bool[,] outcome = Grid.CloneArray();        

        for (int y = 0; y < shape.GetLength(0); y++)
        {
            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (!shape[y, x])
                    continue;

                int targetX = x + offsetX;
                int targetY = y + offsetY;

                if (outcome[targetY, targetX] == true)
                    return false;
                else
                    outcome[targetY, targetX] = true;
            }
        }

        Grid = outcome;
        return true;
    }
}
