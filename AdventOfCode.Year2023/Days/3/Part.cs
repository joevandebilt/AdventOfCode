namespace AdventOfCode.Year2023.Days.DayThree
{
    public record Part
    {
        public string Key { get; set; } = string.Empty;
        public int PartNumber { get; set; }
        public Dictionary<char, KeyValuePair<int, int>> Positions { get; set; } = new();

        public bool Success
        {
            get
            {
                return Positions.Keys.Count > 0;
            }
        }
    }
}
