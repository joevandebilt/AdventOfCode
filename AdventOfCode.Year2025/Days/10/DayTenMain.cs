using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayTen;

public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    private List<Machine> ValidMachines = new();

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        var machines = linesOfInput.Select((line, idx) => new Machine(idx, line)).ToList();
        for (int i = 0; i < machines.Count; i++)
        {
            var machine = machines[i];
            WriteLine($"Machine {i + 1}/{machines.Count} [{string.Concat(machine.IndicatorLights.Select(light => light ? "#" : "."))}]");
            machine = TryButtons(machine, machine, machine);
            machines[i] = machine;
            WriteLine($"\tTook {machine.PressedCount} button presses to resolve");
        }
        SetResult1(machines.Sum(m => m.PressedCount));

        WriteLine("\n\n\t\tPart 2\n\n");

        //Reset the machines
        long presses = 0;
        machines = linesOfInput.Select((line, idx) => new Machine(idx, line)).ToList();
        for (int i = 0; i < machines.Count; i++)
        {
            //https://www.reddit.com/r/adventofcode/comments/1pk87hl/2025_day_10_part_2_bifurcate_your_way_to_victory/
            var machine = machines[i];
            WriteLine($"Machine {i + 1}/{machines.Count} [{string.Concat(machine.IndicatorLights.Select(light => light ? "#" : "."))}] {{{string.Join(", ", machine.Requirements)}}}");

            var validMachinesForThis = ValidMachines
                .Where(vm => vm.Id == machine.Id)
                .ToList();

            int best = int.MaxValue;
            foreach (var vm in validMachinesForThis)
            {
                var result = SimulateMachine(machine.Requirements.ToArray(), vm.Buttons.Where((b, idx) => vm.ButtonsPressed.Contains(idx)).ToList());
                if (result < best)
                    best = result;
            }
            presses += best;
        }
        SetResult2(presses);

        await base.Run();
    }

    public int SimulateMachine(int[] targets, List<Button> buttons)
    {
        var oddIdx = targets.Select((req, idx) => new { req, idx })
            .Where(x => x.req % 2 != 0)
            .Select(x => x.idx)
            .ToList();

        var balanceButtons = buttons
            .Where(b => b.Wires.All(wireIdx => oddIdx.Contains(wireIdx)))
            .ToList();

        if (!oddIdx.All(oi => balanceButtons.SelectMany(bb => bb.Wires).Contains(oi)))
        {
            //This won't balance the odd numbers
            return int.MaxValue;
        }

        int balancePresses = 0;
        while (oddIdx.Count > 0)
        {
            var idx = oddIdx.First();

            var balanceButton = balanceButtons
                .Where(b => b.Wires.Contains(idx));

            if (balanceButton.Count() == 1)
            {
                balanceButton.First().Wires.ForEach(wireIdx => targets[wireIdx]--);
                balancePresses++;

                oddIdx = targets.Select((req, idx) => new { req, idx })
                    .Where(x => x.req % 2 != 0)
                    .Select(x => x.idx)
                    .ToList();

                continue;
            }
            else
            {
                //Not sure what to do here, cry?
                return int.MaxValue;
            }
        }

        //Now check if we can resolve the rest
        var presses = SimulateJoltagePresses(targets.ToArray(), 1, buttons) + balancePresses;

        return int.MaxValue;
    }

    public int SimulateJoltagePresses(int[] target, int factor, List<Button> buttons)
    {
        if (target.All(t => t % 2 == 0))
        {
            return SimulateJoltagePresses(target.Select(t => t / 2).ToArray(), factor + 1, buttons);
        }
        else if (target.Any(t => t % 2 > 0))
        {
            return factor * SimulateMachine(target, buttons);
        }
        else
        {
            int bestButtons = BestButtons(target, 0, buttons);
            return factor * bestButtons;
        }
    }

    public int BestButtons(int[] target, int presses, List<Button> buttons)
    {
        if (buttons.Count == 0)
        {
            //No buttons left
            if (target.All(t => t == 0))
            {
                return presses;
            }
            else
            {
                return int.MaxValue;
            }
        }
        else if (target.Any(t => t < 0))
        {
            return int.MaxValue;
        }

        var newButtons = buttons.Select(b => b).ToList();
        foreach (var button in buttons)
        {
            newButtons.Remove(button);

            //Test On
            var testOnTarget = target.ToArray();
            foreach (var wireIdx in button.Wires)
            {
                testOnTarget[wireIdx]--;
            }
            var resultOn = BestButtons(testOnTarget, presses + 1, newButtons);

            //Test Off
            var testOffTarget = target.ToArray();
            var resultOff = BestButtons(testOffTarget, presses, newButtons);

            presses = Math.Min(resultOn, resultOff);
        }
        return presses;
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
                    if (!ValidMachines.Any(vm => vm.Key == bestSimulation.Key))
                    {
                        WriteLine($"\tSolved with {bestSimulation.PressedCount}: {string.Join(",", bestSimulation.ButtonsPressed.Select(b => b.ToString()))}");
                        WriteLine($"\t\t{string.Join(",", bestSimulation.ButtonsPressed.Select(b => $"({string.Join(",", bestSimulation.Buttons.ElementAt(b).Wires.Select(w => w.ToString()))})"))}");
                        ValidMachines.Add(bestSimulation);
                    }
                    continue;
                }

                //if (newMachineState.PressedCount >= bestSimulation.PressedCount)
                //{
                //    //No need to continue down this path, we already have a better solution
                //    continue;
                //}

                var result = TryButtons(originalMachine, bestSimulation, newMachineState);
                if (result.LightsResolved && result.PressedCount < bestSimulation.PressedCount)
                {
                    bestSimulation = result;
                }
            }
        }
        return bestSimulation;
    }
}