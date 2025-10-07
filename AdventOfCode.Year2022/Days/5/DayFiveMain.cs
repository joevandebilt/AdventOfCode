using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Data;

namespace AdventOfCode.Year2022.Days.DayFive;
public class DayFiveMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFiveMain() : base(Day.Five, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);

        var instructions = new List<MoveInstruction>();
        List<char>[] lanes = null!;

        int startline = 0;
        int columns;

        while (lanes == null)
        {
            var line = linesOfInput[startline];
            if (line.Trim().StartsWith('1'))
            {
                columns = (int)Char.GetNumericValue(line.Trim().Last());
                lanes = new List<char>[columns];

                var readLine = startline - 1;
                while (readLine >= 0)
                {
                    line = linesOfInput[readLine];

                    var charIndex = 1;
                    for (int col = 0; col < columns; col++)
                    {
                        if (lanes[col] == null)
                            lanes[col] = new List<char>();

                        var readChar = line[charIndex];
                        if (readChar != ' ' && readChar != '[' && readChar != ']')
                        {
                            lanes[col].Add(readChar);
                        }
                        charIndex += 4;
                    }
                    readLine--;
                }

                for (int row = startline + 2; row < linesOfInput.Count; row++)
                {
                    try
                    {
                        line = linesOfInput[row];
                        if (line.Trim() != "")
                        {
                            line = line.Replace("move ", "");
                            line = line.Replace("from ", "");
                            line = line.Replace("to ", "");

                            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            instructions.Add(new MoveInstruction
                            {
                                Quantity = int.Parse(parts[0]),
                                SourceLane = int.Parse(parts[1]),
                                DestinationLane = int.Parse(parts[2]),
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"I broke on line {row}");
                        throw ex;
                    }
                }
            }
            else
                startline++;
        }
        PrintLanes(lanes);

        foreach (var instruction in instructions)
        {
            for (int i = 0; i < instruction.Quantity; i++)
            {
                var take = lanes[instruction.SourceLane - 1].Last();
                lanes[instruction.DestinationLane-1].Add(take);
                lanes[instruction.SourceLane - 1].RemoveAt(lanes[instruction.SourceLane - 1].Count - 1);
            }
            PrintLanes(lanes);
        }

        var output = string.Concat(lanes.Select(lane => lane.Last()));

        Console.WriteLine();
        //SetResult1(0);
        //SetResult2(0);
        await base.Run();
    }

    private void PrintLanes(List<char>[] lanes)
    {
        Console.Clear();
        int maxHeight = lanes.Max(l => l.Count);
        for (int row = maxHeight - 1; row >= 0; row--)
        {
            for (int lane = 0; lane < lanes.Length; lane++)
            {
                Console.Write(' ');
                if (lanes[lane].Count < row + 1)
                    Console.Write("   ");
                else
                    Console.Write($"[{lanes[lane][row]}]");
            }
            Console.Write(Environment.NewLine);
        }
        var cols = Enumerable.Range(1, lanes.Length);
        Console.WriteLine($"  {string.Join("   ", cols)} ");
    }
}
