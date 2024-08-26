using AdventOfCode.Enums;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days.DayFour;

public class DayFourMain : AdventOfCodeDay
{
    private const bool _debugging = false;

    public DayFourMain() : base(Day.Four, _debugging) { }
    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        List<Scratchcard> scratchcards = new(); 
        List<Scratchcard> bonusScratchcards = new();

        foreach (var line in linesOfInput)
        {
            var scratchCard = new Scratchcard();
            var scratchParts = line.Split(':');
            if (scratchParts.Length != 2) throw new ArgumentOutOfRangeException("Scratchcard should be made of 2 parts");
            
            var identifierPart = scratchParts.First();
            scratchCard.CardNumber = int.Parse(Regex.Match(identifierPart, "\\d+").Value);

            var numberParts = scratchParts.Last().Split('|');
            if (numberParts.Length != 2) throw new ArgumentOutOfRangeException("Numbers should be made of 2 parts");

            scratchCard.WinningNumbers = SplitNumbers(numberParts.First());
            scratchCard.SelectedNumbers = SplitNumbers(numberParts.Last());

            scratchcards.Add(scratchCard);
        }

        //Initialise the madness
        foreach (var originalCard in scratchcards)
        {
            bonusScratchcards.Add(originalCard);

            var cardIterations = bonusScratchcards.Count(s => s.CardNumber == originalCard.CardNumber);
            var correctPicks = originalCard.CorrectPicks;
            var bonusCardIds = Enumerable.Range(originalCard.CardNumber + 1, correctPicks);

            foreach (var bonusCardId in bonusCardIds) 
            {
                var cardToClone = scratchcards.Single(s => s.CardNumber == bonusCardId);
                var bonusCards = Enumerable.Range(0, cardIterations).Select(n => cardToClone with { }).ToList();
                bonusScratchcards.AddRange(bonusCards);
            }
        }

        WriteLine($"This cheeky bugger played {scratchcards.Count} scratchers");

        SetResult1(scratchcards.Sum(s => s.Points));
        SetResult2(bonusScratchcards.Count);

        await base.Run();
    }

    private List<int> SplitNumbers(string input)
    {
        return input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => int.Parse(l.Trim()))
            .ToList();
    }
}
