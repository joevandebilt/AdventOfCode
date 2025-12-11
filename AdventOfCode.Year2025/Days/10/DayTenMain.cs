using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Reflection.PortableExecutable;

namespace AdventOfCode.Year2025.Days.DayTen;

public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        var machines = linesOfInput.Select(line => new Machine(line)).ToList();
        for (int i = 0; i < machines.Count; i++)
        {
            var machine = machines[i];
            WriteLine($"Machine {i + 1}/{machines.Count} [{string.Concat(machine.IndicatorLights.Select(light => light ? "#" : "."))}]");
            machine = TryButtons(machine, machine, machine);
            machines[i] = machine;
            WriteLine($"\tTook {machine.PressedCount} button presses to resolve");
        }
        SetResult1(machines.Sum(m => m.PressedCount));

        //Reset the machines
        /*
        machines = linesOfInput.Select(line => new Machine(line)).ToList();
        for (int i = 0; i < machines.Count; i++)
        {
            var machine = machines[i];
            WriteLine($"Machine {i + 1}/{machines.Count} [{string.Concat(machine.IndicatorLights.Select(light => light ? "#" : "."))}]");
            machine = TryButtonsWithJoltage(machine, machine, machine);
            machines[i] = machine;
            WriteLine($"\tTook {machine.PressedCount} button presses to resolve");
        }
        SetResult2(machines.Where(m => m.Ready).Sum(m => m.PressedCount));
        */

        await base.Run();
    }

    public Machine TryButtons(Machine originalMachine, Machine bestSimulation, Machine machine)
    {
        //Get the lights we need to switch off
        var lights = machine.IndicatorLights
            .Select((light, idx) => new { light, idx })
            .Where(x => x.light)
            .Select(x => x.idx)
            .ToList();

        //We are going to try and turn everything off
        for (int i = 0; i < machine.Buttons.Count; i++)
        {
            var button = machine.Buttons[i];
            if (!machine.ButtonsPressed.Contains(i) && button.Wires.Any(wireIdx => lights.Contains(wireIdx)))
            {
                //This button can turn off at least one light we need to turn off
                var newMachineState = machine.Copy();
                newMachineState.PushButton(i);

                if (newMachineState.LightsResolved)
                {
                    //All lights are off
                    bestSimulation = newMachineState;
                    WriteLine($"\tSolved with {bestSimulation.PressedCount}: {string.Join(",", bestSimulation.ButtonsPressed.Select(b => b.ToString()))}");
                    WriteLine($"\t\t{string.Join(",", bestSimulation.ButtonsPressed.Select(b => $"({string.Join(",", bestSimulation.Buttons.ElementAt(b).Wires.Select(w => w.ToString()))})"))}");
                    continue;
                }

                if (newMachineState.PressedCount >= bestSimulation.PressedCount)
                {
                    //No need to continue down this path, we already have a better solution
                    continue;
                }

                var result = TryButtons(originalMachine, bestSimulation, newMachineState);
                if (result.LightsResolved && result.PressedCount < bestSimulation.PressedCount)
                {
                    bestSimulation = result;
                }
            }
        }
        return bestSimulation;
    }

    public Machine TryButtonsWithJoltage(Machine originalMachine, Machine bestSimulation, Machine machine)
    {
        List<Simulation> simulations = new();
        for (int idx = 0; idx < machine.Requirements.Count; idx++)
        {
            if (machine.Requirements[idx] == 0)
            {
                //This requirement is already satisfied
                continue;
            }

            var validButtons = machine.Buttons
                 .Select((button, buttonIdx) => new { button, buttonIdx })
                 .Where(b => b.button.Wires.Contains(idx))
                 .ToList();

            foreach (var vb in validButtons)
            {
                simulations.Add(new Simulation
                {
                    RequirementIndex = idx,
                    ButtonIndex = vb.buttonIdx,
                    PressesRequired = machine.Requirements[idx]
                });
            }
        }

        //Simulation bounds complete
        return TestRequirements(simulations, machine, 0);
    }

    private Machine? TestRequirements(List<Simulation> simulations, Machine machine, int idx)
    {
        //Wait just so I can read the console
        Machine? bestSimulation = null;

        var buttonFunctions = simulations.Where(s => s.RequirementIndex == idx);
        var factorCount = buttonFunctions.Count();
        List<int[]> factorCombinatons = Generate(machine.Requirements[idx], factorCount).ToList();

        foreach (var factors in factorCombinatons)
        {            
            var functions = factors.Select((factor, factorIdx) => new { factor, function = buttonFunctions.ElementAt(factorIdx) })
                .Where(ff => ff.factor > 0)
                .ToList();

            foreach(var func in functions)
            {
                var button = machine.Buttons[func.function.ButtonIndex];
                if (button.Wires.Count == 1)
                {
                    //Quick win to skip unnecessary presses
                    var posIdx = button.Wires.First();
                    var invert = func.factor % 2 == 1;

                    if ((invert && !machine.IndicatorLights[posIdx]) || (!invert && machine.IndicatorLights[posIdx]))
                        continue;
                }

                var newState = machine.Copy();
                for (int press = 0; press < func.factor; press++)
                {
                    newState.PushButton(func.function.ButtonIndex);
                }

                Update($"[{string.Concat(newState.IndicatorLights.Select(light => light ? "#" : "."))}] {{{string.Join(", ", newState.Requirements.Select(r => $"{r}"))}}}");

                if (!newState.Safe || (newState.PressedCount >= bestSimulation?.PressedCount))                    
                    continue;

                if (newState.Ready && newState.PressedCount < bestSimulation?.PressedCount)
                {
                    bestSimulation = newState;
                    WriteLine($"\tSolved with {bestSimulation.PressedCount}: {string.Join(",", bestSimulation.ButtonsPressed.Select(b => b.ToString()))}");
                    WriteLine($"\t\t{string.Join(",", bestSimulation.ButtonsPressed.Select(b => $"({string.Join(",", bestSimulation.Buttons.ElementAt(b).Wires.Select(w => w.ToString()))})"))}");
                    continue;
                }

                int nextIdx = -1;
                if (button.Wires.Any(wireIdx => newState.Requirements[wireIdx] > 0))
                {
                    nextIdx = button.Wires.First(wireIdx => newState.Requirements[wireIdx] > 0);
                }
                else
                {
                    nextIdx = newState.Requirements.Select((req, reqIdx) => new { req, reqIdx })
                        .Where(r => r.req > 0)
                        .Select(r => r.reqIdx)
                        .FirstOrDefault(-1);
                }

                if (nextIdx != -1)
                {
                    var result = TestRequirements(simulations, newState, nextIdx);
                    if (result != null && result.Ready && result.PressedCount < bestSimulation?.PressedCount)
                    {
                        bestSimulation = result;
                    }
                }
            }
        }

        return bestSimulation;
    }



    /// <summary>
    /// Iteratively generates all k-tuples of nonnegative integers summing to n
    /// using the "stars and bars" separator positions approach.
    /// </summary>
    public static IEnumerable<int[]> Generate(int n, int k)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k));
        if (k == 1)
        {
            yield return new[] { n };
            yield break;
        }

        int total = n + k - 1;      // positions
        int bars = k - 1;

        // Initialize the combination of bar positions: [0,1,2,...,bars-1]
        var pos = new int[bars];
        for (int i = 0; i < bars; i++) pos[i] = i;

        while (true)
        {
            // Convert bar positions to tuple counts
            var tuple = new int[k];
            int prev = -1;
            for (int i = 0; i < bars; i++)
            {
                tuple[i] = pos[i] - prev - 1; // stars between bars
                prev = pos[i];
            }
            tuple[k - 1] = total - prev - 1;   // stars after last bar

            yield return tuple;

            // Next combination of bar positions (lexicographic)
            int idx = bars - 1;
            while (idx >= 0 && pos[idx] == total - bars + idx)
                idx--;

            if (idx < 0) yield break;

            pos[idx]++;
            for (int j = idx + 1; j < bars; j++)
                pos[j] = pos[j - 1] + 1;
        }
    }
}