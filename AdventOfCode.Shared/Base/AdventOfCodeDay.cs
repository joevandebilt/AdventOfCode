using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Base;

public partial class AdventOfCodeDay
{
    private DateTime init;
    private DateTime part1TimeResult;
    private DateTime part2TimeResult;

    private double part1Result;
    private double part2Result;

    private readonly int _dayOfAdvent;
    private readonly bool _debugMode;

    public int DayOfAdvent => _dayOfAdvent;

    public AdventOfCodeDay(Day dayOfAdvent, bool debugMode)
    {
        _dayOfAdvent = (int)dayOfAdvent;
        _debugMode = debugMode;
        init = DateTime.Now;

        Console.CursorVisible = false;
    }

    public virtual async Task Run()
    {
        WriteLine($"Day {_dayOfAdvent} Completed");
        await Task.CompletedTask;
    }
    public async Task PrintResult()
    {
        Console.WriteLine($"Day {_dayOfAdvent} Part 1 Result:\t{part1Result} ({part1TimeResult.Subtract(init).TotalMilliseconds}ms)");
        Console.WriteLine($"Day {_dayOfAdvent} Part 2 Result:\t{part2Result} ({part2TimeResult.Subtract(init).TotalMilliseconds}ms)");
        await Task.CompletedTask;
    }

    protected void SetResult1(double result)
    {
        part1Result = result;
        part1TimeResult = DateTime.Now;
    }
    protected void SetResult2(double result)
    {
        part2Result = result;
        part2TimeResult = DateTime.Now;
    }

    protected string GetCurrentFilePath()
    {
        return Path.Combine(Environment.CurrentDirectory, "Days", _dayOfAdvent.ToString());
    }

    protected async Task<List<string>> LoadFile(bool forceLower = true)
    {
        List<string> linesOfInput = new();
        string currentDir = GetCurrentFilePath();
        string inputPath = Path.Combine(currentDir, "Input.txt");

        if (!File.Exists(inputPath))
            throw new ArgumentException($"input.txt not found for day {_dayOfAdvent}");

        using (var reader = new StreamReader(inputPath))
        {
            string? line = string.Empty;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                linesOfInput.Add(forceLower ? line.ToLower() : line);
            }
        }
        return linesOfInput;
    }

    protected void WriteLine(string line)
    {
        if (_debugMode)
            Console.WriteLine(line);
    }

    protected void Write(string line)
    {
        if (_debugMode)
            Console.Write(line);
    }

    protected void Clear()
    {
        if (_debugMode)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();
        }
    }
    protected void ResetCursor()
    {
        if (_debugMode)
            Console.SetCursorPosition(0, 0);
    }

}
