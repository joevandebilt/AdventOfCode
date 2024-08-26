using AdventOfCode.Enums;

namespace AdventOfCode.Days.DayFive;
public class Day5 : AdventOfCodeDay
{
    private const int _day = 5;
    private const bool _debugging = false;
    public Day5() : base(_day, _debugging) { }

    private List<Map> Maps = new();
    public override async Task Run()
    {
        var inputLines = await LoadFile();

        List<long> Seeds = new();
        Map currentMap = null!;
        for (int i = 0; i < inputLines.Count; i++)
        {
            var line = inputLines[i];
            if (i == 0)
            {
                Seeds = line.Split(':').Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(l => long.Parse(l.Trim())).ToList();
            }
            else
            {
                if (line.Trim() == string.Empty)
                {
                    if (currentMap != null) Maps.Add(currentMap);
                    currentMap = new Map();

                    i++;
                    line = inputLines[i];

                    var mapDescription = line.Split(' ').First();
                    var mapComponents = mapDescription.Split('-');
                    currentMap.SourceType = (MapType)Enum.Parse(typeof(MapType), mapComponents.First(), true);
                    currentMap.DestinationType = (MapType)Enum.Parse(typeof(MapType), mapComponents.Last(), true);
                }
                else
                {
                    var mapParts = line.Split(' ');
                    var mapRange = new ObjectToObjectRange
                    {
                        DestinationStart = long.Parse(mapParts[0].Trim()),
                        SourceStart = long.Parse(mapParts[1].Trim()),
                        RangeLength = long.Parse(mapParts[2].Trim())
                    };
                    currentMap.Ranges.Add(mapRange);
                }
            }
        }
        Maps.Add(currentMap);
        currentMap = null!;

        WriteLine($"Loaded {Maps.Count} Maps");

        long lowestLocation = long.MaxValue;
        foreach (var seed in Seeds)
        {
            var location = TraverseTree(seed, MapType.Seed);
            WriteLine($"Seed {seed} goes to location {location}");
            if (location < lowestLocation) lowestLocation = location;
            if (location == 389056265)
            {
                WriteLine($"Break Seed {seed}");
            }
        }
        Part1Result = lowestLocation;

        var seedMap = new Map
        {
            DestinationType = MapType.Seed,
            SourceType = MapType.None

        };
        for (int i = 0; i < Seeds.Count; i = i + 2)
        {
            seedMap.Ranges.Add(new ObjectToObjectRange
            {
                DestinationStart = Seeds[i],
                RangeLength = Seeds[i + 1]
            });
        }
        Maps.Add(seedMap);

        var locationMap = Maps.First(map => map.DestinationType == MapType.Location);
        var lowRange = locationMap.Ranges.OrderBy(locationMap => locationMap.DestinationStart).First();

        long interval = 100000;
        long loc = lowRange.DestinationStart;
        while (Part2Result == 0)
        {
            WriteLine($"Testing Location {loc}");
            long seedLocation = InTraverseTree(loc, MapType.Location);
            if (seedLocation != long.MaxValue)
            {
                if (interval == 1) Part2Result = loc; //Win condition

                //Valid seedmap
                WriteLine($"Location {loc} maps to Seed {seedLocation}");
                
                //Go back to last step and shorten the step
                loc = loc - interval;
                interval = interval / 10;                
                WriteLine($"New interval is {interval}");
            }
            else
            {
                loc = loc + interval;
            }
        }
    }

    private long TraverseTree(long input, MapType sourceType)
    {
        WriteLine($"\n\nGot {sourceType} with value {input}");
        var map = Maps.SingleOrDefault(m => m.SourceType == sourceType);
        if (map != null)
        {
            WriteLine($"\tGot {map.SourceType}-to-{map.DestinationType} map");
            var range = map.Ranges.SingleOrDefault(r => r.SourceInRange(input));
            if (range != null)
            {
                var diff = input - range.SourceStart;
                var nextInput = range.DestinationStart + diff;
                WriteLine($"\t\tFound destination {map.DestinationType} with start value of {range.DestinationStart}");
                return TraverseTree(nextInput, map.DestinationType);
            }
            else
            {
                WriteLine($"\t\tNo Match {map.DestinationType} will use same value of {input}");
                return TraverseTree(input, map.DestinationType);
            }
        }

        //End of tree
        return input;
    }
    private long InTraverseTree(long input, MapType destinationType)
    {
        if (destinationType == MapType.None)
            return input;  //end of the road

        WriteLine($"\n\nGot {destinationType} with value {input}");
        var map = Maps.SingleOrDefault(m => m.DestinationType == destinationType);

        if (map != null)
        {
            WriteLine($"\tGot {map.SourceType}-to-{map.DestinationType} map");
            var testRanges = map.Ranges.Where(r => r.DestinationInRange(input)).ToList();
            var range = map.Ranges.SingleOrDefault(r => r.DestinationInRange(input));
            if (range != null)
            {
                var diff = input - range.DestinationStart;
                var nextInput = diff + range.SourceStart;
                WriteLine($"\t\tFound Source range {map.SourceType} with start value of {range.SourceStart}");
                return InTraverseTree(nextInput, map.SourceType);
            }
            else
            {
                WriteLine($"\t\tNo Match {map.SourceType} not a valid map");
                return long.MaxValue;
            }
        }

        //End of tree
        return input;
    }
}
