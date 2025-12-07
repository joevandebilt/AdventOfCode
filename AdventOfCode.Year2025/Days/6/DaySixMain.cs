using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DaySix;
public class DaySixMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DaySixMain() : base(Day.Six, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        IDictionary<int, List<long>> numbers = new Dictionary<int, List<long>>();
        long part1Total = 0;
        long part2Total = 0;

        foreach (var line in linesOfInput)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (long.TryParse(part, out var number))
                {
                    if (!numbers.ContainsKey(i))
                        numbers[i] = new List<long>();

                    numbers[i].Add(number);
                }
                else
                {
                    var leftToRight = numbers[i];
                    switch (part)
                    {
                        case "*":
                            part1Total += leftToRight.Aggregate((a, b) => { return a * b; });
                            break;
                        case "+":
                            part1Total += leftToRight.Sum();
                            break;
                        default:
                            throw new ArgumentException("That shouldn't bloody happen");
                    }
                }
            }
        }
        numbers.Clear();

        List<long> values = new();
        var lineLength = linesOfInput?.MaxBy(l => l.Length)?.Length;
        string numberString = string.Empty, operation = string.Empty;
        for (int col = 0; col < lineLength; col++)
        {            
            for (int row = 0; row < linesOfInput?.Count; row++)
            {
                var character = linesOfInput[row][col];
                switch (character)
                {
                    case '*':
                    case '+':
                        operation = character.ToString();
                        break;
                    case char n when char.IsDigit(n):
                        numberString += n;
                        break;
                    default:
                        break;
                }
            }
            if (string.IsNullOrEmpty(numberString))
            {
                part2Total += AddUpColumn(operation, values);
                values.Clear();
            }
            else
            {
                values.Add(long.Parse(numberString));
                numberString = string.Empty;
            }
        }
        part2Total += AddUpColumn(operation, values);

        SetResult1(part1Total);
        SetResult2(part2Total);
        await base.Run();
    }

    private long AddUpColumn(string operation, List<long> values)
    {
        return operation switch {
            "*" => values.Aggregate((a, b) => { return a * b; }),
            "+" => values.Sum(),
            _ => throw new ArgumentException("That shouldn't bloody happen")
        };
    }
}
