using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Year2025.Days.Day10;
using AdventOfCode.Year2025.Days.Day10.Solver;
using System.Data;

namespace AdventOfCode.Year2025.Days.DayTen;

public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayTenMain() : base(Day.Ten, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var machines = linesOfInput.Select((line, idx) => new Machine(idx, line)).ToList();

        long totalPresses = 0;
        foreach (var machine in machines)
        {
            WriteLine($"Solving Machine {machine.Id + 1}/{machines.Count} [{Convert.ToString(machine.TargetLights, 2).PadLeft(machine.LightsCount, '0')}]");
            int presses = Solve(machine);
            if (presses >= 0)
            {
                WriteLine($"\tSolved in {presses} presses");
                totalPresses += presses;
            }
            else
            {
                WriteLine($"\tNo solution found");
            }
        }
        SetResult1(totalPresses);


        totalPresses = 0;
        foreach (var machine in machines)
        {
            WriteLine($"Doing Joltage Solving Machine {machine.Id + 1}/{machines.Count} [{Convert.ToString(machine.TargetLights, 2).PadLeft(machine.LightsCount, '0')}]");
            var solver = new BifurcateSolver(machine);
            var presses = solver.Solve(machine.Requirements);
            if (presses >= 0)
            {
                WriteLine($"\tSolved in {presses} presses");
                totalPresses += presses;
            }
            else
            {
                WriteLine($"\tNo solution found");
            }
        }
        SetResult2(totalPresses);
    }

    private int Solve(Machine machine)
    {
        var visited = new HashSet<long>();
        var queue = new Queue<(long state, int presses)>();

        queue.Enqueue((0, 0)); // start with all lights off
        visited.Add(0);

        while (queue.Count > 0)
        {
            var (state, presses) = queue.Dequeue();

            if (state == machine.TargetLights)
                return presses; // found solution

            foreach (var button in machine.Buttons)
            {
                long next = state ^ button; // toggle lights
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue((next, presses + 1));
                }
            }
        }
        return -1; // no solution
    }
}