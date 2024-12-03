namespace AdventOfCode.Year2023.Days.DaySix;

public record BoatRace
{
    public long Time { get; set; }
    public long Distance { get; set; }

    public List<long> BestTimes()
    {
        var bestTimes = new List<long>();
        for (long i = 0; i<=Time; i++)
        {
            var velocity = i;
            var timeRemaining = this.Time - i;
            var distanceTravelled = velocity * timeRemaining;
            if (distanceTravelled > Distance) bestTimes.Add(i);
        }
        return bestTimes;
    }
}
