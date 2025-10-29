using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Year2022.Days.DayNine;
public class DayNineMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayNineMain() : base(Day.Nine, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: true);

        List<Tuple<int, int>> Knots = new();
        for (int i = 0; i < 10; i++)
        {
            Knots.Add(new(0, 0));
        }

        Dictionary<string, int> positions = new();

        foreach (var line in linesOfInput)
        {
            var Head = Knots.First();
            var parts = line.Split(' ');
            var direction = parts.First();
            var steps = int.Parse(parts.Last());

            for (int i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case "u":
                        Head = new(Head.Item1, Head.Item2 - 1);
                        break;
                    case "d":
                        Head = new(Head.Item1, Head.Item2 + 1);
                        break;
                    case "l":
                        Head = new(Head.Item1 - 1, Head.Item2);
                        break;
                    case "r":
                        Head = new(Head.Item1 + 1, Head.Item2);
                        break;
                }
                Knots[0] = Head;

                //Update position of tail
                for (int j = 1; j < Knots.Count; j++)
                {
                    Knots[j] = MoveTail(Knots[j - 1], Knots[j]);
                }

                var posKey = $"First_{Knots[1].Item1}:{Knots[1].Item2}";
                positions.UpsertEntry(posKey);

                posKey = $"Last_{Knots.Last().Item1}:{Knots.Last().Item2}";
                positions.UpsertEntry(posKey);
            }
        }

        SetResult1(positions.Where(p => p.Key.StartsWith("First_")).Count());
        SetResult2(positions.Where(p => p.Key.StartsWith("Last_")).Count());
        await base.Run();
    }

    private Tuple<int, int> MoveTail(Tuple<int, int> Head, Tuple<int, int> Tail)
    {
        //Get Difference 
        var xChange = Head.Item2 - Tail.Item2;
        var yChange = Head.Item1 - Tail.Item1;

        //If the difference in position is 1 or 0 then it's touching
        if (Math.Abs(xChange) > 1 || Math.Abs(yChange) > 1)
        {
            var xMove = Math.Clamp(xChange, -1, 1);
            var yMove = Math.Clamp(yChange, -1, 1);
            Tail = new(Tail.Item1 + yMove, Tail.Item2 + xMove);
        }
        return Tail;
    }
}
