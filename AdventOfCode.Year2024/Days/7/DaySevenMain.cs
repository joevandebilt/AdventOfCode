using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Year2024.Days._7;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Shared.Template;
public class DaySevenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DaySevenMain() : base(Day.Seven, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        var calibrations = new List<Calibration>();
        foreach (var line in linesOfInput)
        {
            var solutionParts = line.Split(':');
            var inputParts = solutionParts.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            calibrations.Add(new Calibration
            {
                Solution = long.Parse(solutionParts.First()),
                Inputs = inputParts.Select(i => int.Parse(i)),
                Valid = false
            });
        }

        WriteLine($"I got {calibrations.Count} calibrations to process");
        foreach (var calibration in calibrations)
        {
            var operations = new List<string>();
            if (TestOperators(calibration.Solution, calibration.Inputs.First(), calibration.Inputs.Skip(1).Take(calibration.Inputs.Count()), operations))
            {
                operations.Reverse();
                WriteLine($"{calibration.Solution} = {calibration.Inputs.First()} {string.Join(" ", operations)}");
                calibration.Valid = true;
                continue;
            }
        }
        SetResult1(calibrations.Where(c => c.Valid).Sum(c => c.Solution));

        foreach (var calibration in calibrations.Where(c => !c.Valid))
        {
            var operations = new List<string>();
            if (TestOperators(calibration.Solution, calibration.Inputs.First(), calibration.Inputs.Skip(1).Take(calibration.Inputs.Count()), operations, true))
            {
                operations.Reverse();
                WriteLine($"{calibration.Solution} = {calibration.Inputs.First()} {string.Join(" ", operations)}");
                calibration.Valid = true;
                continue;
            }
        }
        SetResult2(calibrations.Where(c => c.Valid).Sum(c => c.Solution));
        await base.Run();
    }

    private bool TestOperators(long solution, long subtotal, IEnumerable<int> inputs, ICollection<string> operations, bool ConcatOperation = false)
    {
        var input = inputs.First();
        if (inputs.Count() == 1)
        {
            //This is the end of the list
            if (subtotal + input == solution)
            {
                operations.Add($"+ {input}");
                return true;
            }
            else if (subtotal * input == solution)
            {
                operations.Add($"* {input}");
                return true;
            }
            else if (ConcatOperation && long.Parse($"{subtotal}{input}") == solution)
            {
                operations.Add($"|| {input}");
                return true;
            }
            else
                return false;
        }
        else
        {
            var testAddSubtotal = subtotal + input;
            var testMulSubtotal = subtotal * input;
            var testConcatSubtotal = long.Parse($"{subtotal}{input}");

            var remainingInputs = inputs.Skip(1).Take(inputs.Count() - 1);
            if (TestOperators(solution, testAddSubtotal, remainingInputs, operations, ConcatOperation))
            {
                operations.Add($"+ {input}");
                return true;
            }
            else if (TestOperators(solution, testMulSubtotal, remainingInputs, operations, ConcatOperation))
            {
                operations.Add($"* {input}");
                return true;
            }
            else if (ConcatOperation && TestOperators(solution, testConcatSubtotal, remainingInputs, operations, ConcatOperation))
            {
                operations.Add($"|| {input}");
                return true;
            }
            return false;
        }
    }
}

// 4 5 6
// 4 * 5 || 6
// 20 || 6
// 206