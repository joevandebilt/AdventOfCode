using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using Google.OrTools.LinearSolver;
using System.Data;
using System.Diagnostics;

namespace AdventOfCode.Year2025.Days.DayTen;

public class DayTenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
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
            var watch = Stopwatch.StartNew();
            WriteLine($"Doing Joltage Solving Machine {machine.Id + 1}/{machines.Count} [{Convert.ToString(machine.TargetLights, 2).PadLeft(machine.LightsCount, '0')}]");
            var presses = SolveWithJoltage(machine);
            if (presses >= 0)
            {
                Write($"\tSolved in {presses} presses");
                totalPresses += presses;
            }
            else
            {
                Write($"\tNo solution found");
            }
            watch.Stop();
            Write($" ({watch.ElapsedMilliseconds}ms)\n");
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

    public long SolveWithJoltage(Machine machine)
    {
        var buttonEffects = machine.ButtonEffects.ToArray();
        var target = machine.Requirements;

        int mButtons = buttonEffects.Length;
        int nCounters = target.Length;

        var solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
        if (solver == null) throw new Exception("Could not create solver.");

        // Variables x_j >= 0 integer
        var x = new Variable[mButtons];
        for (int j = 0; j < mButtons; j++)
            x[j] = solver.MakeIntVar(0, double.PositiveInfinity, $"x[{j}]");

        // Constraints: sum_j A[i,j] * x[j] == target[i]
        for (int i = 0; i < nCounters; i++)
        {
            var ct = solver.MakeConstraint(target[i], target[i]);
            for (int j = 0; j < mButtons; j++)
                if (buttonEffects[j][i] != 0)
                    ct.SetCoefficient(x[j], buttonEffects[j][i]);
        }

        // Objective: minimize sum_j x_j
        var objective = solver.Objective();
        for (int j = 0; j < mButtons; j++)
            objective.SetCoefficient(x[j], 1);
        objective.SetMinimization();

        // Solve
        var status = solver.Solve();

        if (status != Solver.ResultStatus.OPTIMAL && status != Solver.ResultStatus.FEASIBLE)
            throw new Exception("Solver did not find a solution.");

        // Return total presses
        long total = 0;
        for (int j = 0; j < mButtons; j++)
            total += (long)Math.Round(x[j].SolutionValue());

        return total;
    }
}