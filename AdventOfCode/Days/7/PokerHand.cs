using AdventOfCode.Enums;

namespace AdventOfCode.Days.DaySeven;
public record PokerHand
{
    private Dictionary<char, int> cardStrength = new Dictionary<char, int>()
    {
        { '2', 1  },
        { '3', 2  },
        { '4', 3  },
        { '5', 4  },
        { '6', 5  },
        { '7', 6  },
        { '8', 7  },
        { '9', 8  },
        { 'T', 9  },
        { 'J', 10 },
        { 'Q', 11 },
        { 'K', 12 },
        { 'A', 13 }
    };

    public string Hand { get; set; } = null!;
    public int Bid { get; set; }
    public int Rank { get; set; }
    public virtual long Score
    {
        get
        {
            var parts = new string[]
            {
                $"{(int)Strength}",
                $"{cardStrength[Hand[0]]}".PadLeft(2, '0'),
                $"{cardStrength[Hand[1]]}".PadLeft(2, '0'),
                $"{cardStrength[Hand[2]]}".PadLeft(2, '0'),
                $"{cardStrength[Hand[3]]}".PadLeft(2, '0'),
                $"{cardStrength[Hand[4]]}".PadLeft(2, '0'),
            };
            string totalScore = string.Concat(parts);
            return long.Parse(totalScore);
        }
    }

    public PokerHandStrength Strength
    {
        get
        {
            int distinctCards = Hand.Distinct().Count();
            switch (distinctCards)
            {
                case 1:
                    return PokerHandStrength.FiveOfAKind;
                case 5:
                    return PokerHandStrength.HighCard;
                case 4:
                    return PokerHandStrength.OnePair;
                case 3:
                    var cardCount = Hand.GroupBy(c => c).Max(g => Hand.Count(h => h == g.Key));
                    switch (cardCount)
                    {
                        case 3:
                            return PokerHandStrength.ThreeOfAKind;
                        case 2:
                            return PokerHandStrength.TwoPair;
                        default:
                            throw new InvalidDataException("Could not determine Strenght of Hand");
                    }
                case 2:
                    switch (Hand.Count(h => h == Hand[0]))
                    {
                        case 1:
                        case 4:
                            return PokerHandStrength.FourOfAKind;
                        case 2:
                        case 3:
                            return PokerHandStrength.FullHouse;
                        default:
                            throw new InvalidDataException("Could not determine Strenght of Hand");
                    }
                default:
                    throw new InvalidDataException("Could not determine Strenght of Hand");
            }

        }
    }

}
