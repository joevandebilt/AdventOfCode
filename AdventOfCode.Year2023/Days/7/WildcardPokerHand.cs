namespace AdventOfCode.Year2023.Days.DaySeven;
public record WildcardPokerHand : PokerHand
{
    private Dictionary<char, int> cardStrength = new Dictionary<char, int>()
    {
        { 'J', 1 },
        { '2', 2  },
        { '3', 3  },
        { '4', 4  },
        { '5', 5  },
        { '6', 6  },
        { '7', 7  },
        { '8', 8  },
        { '9', 9  },
        { 'T', 10 },
        { 'Q', 11 },
        { 'K', 12 },
        { 'A', 13 }
    };

    public WildcardPokerHand(string hand, int bid)
    {
        var bestHand = hand;
        if (hand.Contains('J'))
        {
            //Do wildcard logic
            var possibleWildcardhands = new List<PokerHand>();
            var possibleWildcardVals = hand.GroupBy(h => h);
            foreach (var wildcardVal in possibleWildcardVals)
            {
                if (wildcardVal.Key != 'J')
                {
                    possibleWildcardhands.Add(new PokerHand
                    {
                        Hand = hand.Replace('J', wildcardVal.Key)
                    });
                }
            }
            
            if (possibleWildcardhands.Any())
            {
                bestHand = possibleWildcardhands.OrderByDescending(h => h.Score).First().Hand;
            }
        }
        this.OriginalHand = hand;
        this.Hand = bestHand;
        this.Bid = bid;
    }

    public string OriginalHand { get; set; }

    public override long Score
    {
        get
        {
            var parts = new string[]
            {
                $"{(int)Strength}",
                $"{cardStrength[OriginalHand[0]]}".PadLeft(2, '0'),
                $"{cardStrength[OriginalHand[1]]}".PadLeft(2, '0'),
                $"{cardStrength[OriginalHand[2]]}".PadLeft(2, '0'),
                $"{cardStrength[OriginalHand[3]]}".PadLeft(2, '0'),
                $"{cardStrength[OriginalHand[4]]}".PadLeft(2, '0'),
            };
            string totalScore = string.Concat(parts);
            return long.Parse(totalScore);
        }
    }
}
