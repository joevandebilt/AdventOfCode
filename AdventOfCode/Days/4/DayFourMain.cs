using System.Text.RegularExpressions;

namespace AdventOfCode.Days.DayFour;

public class DayFourMain : AdventOfCodeDay
{
    private const int _day = 4;
    private const bool _debugging = true;

    public DayFourMain() : base(_day, _debugging) { }
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
            scratchCard.WinningNumbers = numberParts.First().Split(' ').Where(f => !string.IsNullOrWhiteSpace(f)).Select(f => int.Parse(f.Trim())).ToList();
            scratchCard.SelectedNumbers = numberParts.Last().Split(' ').Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => int.Parse(l.Trim())).ToList();

            scratchcards.Add(scratchCard);
        }

        //Initialise the madness
        foreach (var originalCard in scratchcards)
        {
            bonusScratchcards.Add(originalCard);

            var cards = bonusScratchcards.Where(s => s.CardNumber == originalCard.CardNumber).ToList();
            foreach (var card in cards)
            {
                var correctPicks = card.CorrectPicks;
                var bonusCards = Enumerable.Range(originalCard.CardNumber+1, correctPicks);

                foreach (var bonusCardId in bonusCards)
                {
                    bonusScratchcards.Add(scratchcards.First(s => s.CardNumber == bonusCardId));
                }
            }
        }

        WriteLine($"This cheeky bugger played {scratchcards.Count} scratchers");

        Part1Result = scratchcards.Sum(s => s.Points);
        Part2Result = bonusScratchcards.Count;
    }
}
