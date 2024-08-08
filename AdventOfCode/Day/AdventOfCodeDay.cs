
namespace AdventOfCode.Day;
public abstract class AdventOfCodeDay
{
    protected int Part1Result;
    protected int Part2Result;

    private readonly int _dayOfAdvent;
    private readonly bool _debugMode;

    public AdventOfCodeDay(int day, bool debugMode)
    {
        _dayOfAdvent = day;
        _debugMode = debugMode;
    }

    public virtual Task Run()
    {
        Console.WriteLine("You should override this base method you dingus");
        throw new NotImplementedException("Override this method");
    }
    public Task PrintResult()
    {
        Console.WriteLine($"Day {_dayOfAdvent} Part 1 Result:\t{Part1Result}");
        Console.WriteLine($"Day {_dayOfAdvent} Part 2 Result:\t{Part2Result}");
        return Task.CompletedTask;
    }

    protected string GetCurrentFilePath()
    {
        return Path.Combine(Environment.CurrentDirectory, "Day", _dayOfAdvent.ToString());
    }

    protected async Task<List<string>> LoadFile()
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
                linesOfInput.Add(line.ToLower());
            }
        }
        return linesOfInput;
    }

    protected void WriteLine(string line)
    {
        if (_debugMode)
            Console.WriteLine(line);
    }

}
