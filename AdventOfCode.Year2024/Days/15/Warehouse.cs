namespace AdventOfCode.Year2024.Days.DayFifteen;

public class Warehouse
{
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public Coords Robot { get; set; } = null!;
    public List<Coords> Boxes { get; set; } = new();
    public List<BigBox> BigBoxes { get; set; } = new();
    public List<Coords> Walls { get; set; } = new();

    public List<Coords> NewBoxPositions = new();
    public List<BigBox> NewBigBoxPositions = new();

    public void ApplyMoves()
    {
        foreach (var newPos in NewBoxPositions)
        {
            var id = this.Boxes.FindIndex(b => b.Id == newPos.Id);
            if (id < 0 && Robot.Id == newPos.Id)
            {
                Robot = newPos;
            }
            else
            {
                this.Boxes[id] = newPos;
            }
        }

        foreach (var newPos in NewBigBoxPositions)
        {
            var id = this.BigBoxes.FindIndex(bb => bb.Id == newPos.Id);
            this.BigBoxes[id] = newPos;
        }
        ClearMoves();
    }

    public void ClearMoves()
    {
        NewBoxPositions.Clear();
        NewBigBoxPositions.Clear();
    }
}

public class BigBox
{
    public BigBox() { }
    public BigBox(int row, int col)
    {
        this.Id = int.Parse($"{row}{col}");
        LeftSide = new Coords(row, col);
        RightSide = new Coords(row, col + 1);
    }
    public BigBox(int id, int row, int col)
    {
        this.Id = id;
        LeftSide = new Coords(row, col);
        RightSide = new Coords(row, col + 1);
    }

    public Coords LeftSide { get; set; }
    public Coords RightSide { get; set; }

    public int Id { get; set; }
    public string Reference => $"{LeftSide.Reference}_{RightSide.Reference}";
}
