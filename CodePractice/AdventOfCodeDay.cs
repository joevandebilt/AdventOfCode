using System.Reflection;

namespace AdventOfCode
{
    public abstract class AdventOfCodeDay
    {
        protected int Part1Result;
        protected int Part2Result;

        private readonly int _day;
        protected readonly Dictionary<int, string> _numberWords = new()
        {
            { 0, "Zero" },
            { 1, "One" },
            { 2, "Two" },
            { 3, "Three" },
            { 4, "Four" },
            { 5, "Five" },
            { 6, "Six" },
            { 7, "Seven" },
            { 8, "Eight" },
            { 9, "Nine" }
        };

        protected Dictionary<string, int> _wordNumbers
        {
            get
            {
                return _numberWords.ToDictionary(key => key.Value, value => value.Key);
            }
        }

        public AdventOfCodeDay(int day)
        {
            _day = day;
        }

        public virtual Task Run()
        {
            Console.WriteLine("You should override this base method you dingus");
            throw new NotImplementedException("Override this method");
        }
        public Task PrintResult()
        {
            Console.WriteLine($"Day {_day} Part 1 Result:\t{Part1Result}");
            Console.WriteLine($"Day {_day} Part 2 Result:\t{Part2Result}");
            return Task.CompletedTask;
        }

        protected string GetCurrentFilePath()
        {
            return Path.Combine(Environment.CurrentDirectory, "AdventOfCode", $"{_day}. Day{_numberWords[_day]}");
        }

        protected async Task<List<string>> LoadFile()
        {
            List<string> linesOfInput = new();
            string currentDir = GetCurrentFilePath();
            using (var reader = new StreamReader($"{currentDir}\\Day{_numberWords[_day]}Input.txt"))
            {
                string? line = string.Empty;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    linesOfInput.Add(line.ToLower());
                }
            }

            Console.WriteLine($"Got {linesOfInput.Count} lines of input");

            return linesOfInput;
        }

    }
}
