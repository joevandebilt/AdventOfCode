using AdventOfCode.Enums;

namespace AdventOfCode.Days.DaySeven;
public class Day7 : AdventOfCodeDay
{
    private const bool _debugging = false;
    public Day7() : base(Day.Seven, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var hands = new List<PokerHand>();
        foreach (var line in linesOfInput)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);            
            int bid = int.Parse(parts.Last());

            hands.Add(new PokerHand
            {
                Bid = bid,
                Hand = parts[0].ToUpper()
            });
        }

        hands = hands.OrderBy(h => h.Score).ToList();
        for (int i = 0; i < hands.Count; i++) {
            hands[i].Rank = i+1;
        }
        SetResult1(hands.Sum(h => h.Bid*h.Rank));

        //Part 2
        var wildcards = hands.Select(h => 
            new WildcardPokerHand(h.Hand, h.Bid)
        ).ToList();

        wildcards = wildcards.OrderBy(h => h.Score).ToList();
        for (int i = 0; i < wildcards.Count; i++)
        {
            wildcards[i].Rank = i + 1;
        }
        SetResult2(wildcards.Sum(h => h.Bid * h.Rank));
        
        await base.Run();
    }
}
