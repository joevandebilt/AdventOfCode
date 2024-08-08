namespace AdventOfCode.DayOne
{
    public class DayOneMain : AdventOfCodeDay
    {
        public DayOneMain() : base(1) { }
        public override async Task Run()
        {
            var linesOfInput = await LoadFile();
            List<int> p1calibrations = new();
            List<int> p2calibrations = new(); 
            foreach (var line in linesOfInput)
            {
                Dictionary<int, int> indexedValues = new();

                int firstIndex, lastIndex;
                foreach (var item in _wordNumbers)
                {
                    firstIndex = line.IndexOf(item.Key.ToLower());
                    if (firstIndex > -1)
                        indexedValues.Add(firstIndex, item.Value);

                    lastIndex = line.LastIndexOf(item.Key.ToLower());
                    if (lastIndex > -1 && lastIndex != firstIndex)
                        indexedValues.Add(lastIndex, item.Value);
                }

                var firstNumericChar = line.FirstOrDefault(l => Char.IsNumber(l));
                var lastNumericChar = line.LastOrDefault(l => Char.IsNumber(l));

                firstIndex = line.IndexOf(firstNumericChar);
                lastIndex = line.LastIndexOf(lastNumericChar);

                indexedValues.Add(firstIndex, int.Parse($"{firstNumericChar}"));
                if (firstIndex != lastIndex)
                    indexedValues.Add(lastIndex, int.Parse($"{lastNumericChar}"));

                var orderedValues = indexedValues.OrderBy(key => key.Key);
                p1calibrations.Add(int.Parse($"{firstNumericChar}{lastNumericChar}"));
                p2calibrations.Add(int.Parse($"{orderedValues.First().Value}{orderedValues.Last().Value}"));
            }
            Part1Result = p1calibrations.Sum();
            Part2Result = p2calibrations.Sum();
        }
    }
}
