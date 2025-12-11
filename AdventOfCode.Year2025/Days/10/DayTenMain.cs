using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2025.Days.DayTen;

public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var machines = linesOfInput.Select(line => new Machine(line)).ToList();

        for (int i = 0; i < machines.Count; i++)
        {
            var machine = machines[i];
            WriteLine($"Machine {i}/{machines.Count} [{string.Concat(machine.IndicatorLights.Select(light => light ? "#" : "."))}]");
            machine = TryButtons(machine, machine, machine);
            machines[i] = machine;
            WriteLine($"\tTook {machine.PressedCount} button presses to resolve");
        }

        SetResult1(machines.Sum(m => m.PressedCount));
        SetResult2(-1);
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

        if (lights.Count == 0)
        {
            //All lights are off
            WriteLine($"\tSolved with {machine.PressedCount}: {string.Join(",", machine.ButtonsPressed.Select(b => b.ToString()))}");
            WriteLine($"\t\t{string.Join(",", machine.ButtonsPressed.Select(b => $"({string.Join(",", machine.Buttons.ElementAt(b).Wires.Select(w => w.ToString()))})"))}");
            return machine;
        }

        //We are going to try and turn everything off
        for (int i = 0; i < machine.Buttons.Count; i++)
        {
            var button = machine.Buttons[i];
            if (!machine.ButtonsPressed.Contains(i) && button.Wires.Any(wireIdx => lights.Contains(wireIdx)))
            {
                //This button can turn off at least one light we need to turn off
                var newMachineState = machine.Copy();
                newMachineState.PushButton(i);

                if (bestSimulation.Ready && newMachineState.PressedCount >= bestSimulation.PressedCount)
                {
                    //No need to continue down this path, we already have a better solution
                    continue;
                }

                var result = TryButtons(originalMachine, bestSimulation, newMachineState);
                if ((result.Ready && !machine.Ready) || (result.Ready && machine.Ready && result.PressedCount < machine.PressedCount))
                {
                    bestSimulation = result;
                }
            }
        }
        return bestSimulation;
    }
}
