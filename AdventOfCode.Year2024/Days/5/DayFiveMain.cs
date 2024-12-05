using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayFive;
public class DayFiveMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayFiveMain() : base(Day.Five, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        List<IList<int>> updates = new();
        List<Tuple<int, int>> rules = new();

        bool rulesFinished = false;
        foreach (var line in linesOfInput)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                rulesFinished = true;
                continue;
            }

            if (!rulesFinished)
            {
                var parts = line.Split('|');
                rules.Add(new(int.Parse(parts.First()), int.Parse(parts.Last())));
            }
            else
            {
                updates.Add(line.Split(',').Select(p => int.Parse(p)).ToList());
            }
        }

        WriteLine($"Parsed {rules.Count} rules & {updates.Count} updates");

        List<int> correctMiddleNumbers = new();
        List<int> incorrectMiddleNumbers = new();
        foreach (var update in updates)
        {
            bool updateValid = true;
            int i = 0;
            while (i < update.Count())
            {
                bool resetRequired = false;

                var page = update.ElementAt(i);
                var relevantRules = rules.Where(r => (r.Item1 == page && update.Contains(r.Item2)) || r.Item2 == page && update.Contains(r.Item1)).ToList();

                WriteLine($"Got {relevantRules.Count} rules relevant to page {page}");
                foreach (var relevantRule in relevantRules)
                {
                    if (relevantRule.Item1 == page)
                    {
                        //Item 2 should have a higher index than i
                        var higherIndex = update.IndexOf(relevantRule.Item2);
                        if (higherIndex < i)
                        {
                            updateValid = false;
                            resetRequired= true;
                            update[i] = relevantRule.Item2;
                            update[higherIndex] = page;
                            break;
                        }
                    }
                    else if (relevantRule.Item2 == page)
                    {
                        //Item 1 should have a lower index than page
                        var lowerIndex = update.IndexOf(relevantRule.Item1);
                        if (lowerIndex > i)
                        {
                            updateValid = false;
                            resetRequired = true;
                            update[i] = relevantRule.Item1;
                            update[lowerIndex] = page;
                            break;
                        }
                    }
                    else
                    {
                        throw new Exception("This rules finder is fucked");
                    }
                }

                if (!resetRequired)
                    i++;
                else
                    i = 0;
            }

            if (updateValid)
            {
                //Unmodified correct update
                correctMiddleNumbers.Add(update.ElementAt(update.Count() / 2));
            }
            else
            {
                //Corrected modified update
                incorrectMiddleNumbers.Add(update.ElementAt(update.Count() / 2));
            }
        }
        SetResult1(correctMiddleNumbers.Sum());
        SetResult2(incorrectMiddleNumbers.Sum());
        await base.Run();
    }
}
