using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2023.Days.DaySix;
public class Day6 : AdventOfCodeDay
{
    private const bool _debugging = true;
    public Day6() : base(Day.Six, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var boatRaces = new List<BoatRace>();
        
        if (linesOfInput.Count != 2) throw new DataMisalignedException("Expected only 2 lines of input");
        var boatRaceTimes = linesOfInput.First().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var boatRaceDistances = linesOfInput.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (boatRaceTimes.Length != boatRaceDistances.Length) throw new DataMisalignedException("Expected same number of times and distances");
        for (int i = 1; i < boatRaceTimes.Length; i++)
        {
            boatRaces.Add(new BoatRace
            {
                Time = int.Parse(boatRaceTimes[i]),
                Distance = int.Parse(boatRaceDistances[i])
            });
        }
        SetResult1(boatRaces.Select(br => br.BestTimes().Count).Aggregate(1, (x,y) => x*y));

        var bigRace = new BoatRace
        {
            Time = long.Parse(string.Concat(boatRaces.Select(br => br.Time.ToString()))),
            Distance = long.Parse(string.Concat(boatRaces.Select(br => br.Distance.ToString())))
        };        
        SetResult2(bigRace.BestTimes().Count);
        await base.Run();
    }
}
