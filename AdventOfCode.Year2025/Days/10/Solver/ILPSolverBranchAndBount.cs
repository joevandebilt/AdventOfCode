
namespace AdventOfCode.Year2025.Days.DayTen.Solver;

public static class ILPSolverBranchAndBound
{
    // Public entry
    public static int SolveMachine(Machine m)
    {
        int dims = m.Requirements.Length;
        int buttons = m.ButtonEffects.Count;

        // Precompute button weights (how many counters each button affects)
        int[] btnWeight = m.ButtonEffects.Select(b => b.Sum()).ToArray();
        int maxButtonWeight = btnWeight.Max();

        // Order buttons by weight descending (heuristic)
        var order = Enumerable.Range(0, buttons)
                              .OrderByDescending(i => btnWeight[i])
                              .ToArray();

        var orderedButtons = order.Select(i => m.ButtonEffects[i]).ToList();

        // Initial greedy upper bound
        int[] startResidual = (int[])m.Requirements.Clone();
        int greedyUB = GreedyUpperBound(orderedButtons, startResidual.ToArray());

        // If greedy failed (returned int.MaxValue), it's unreachable
        if (greedyUB == int.MaxValue)
            throw new Exception("Target unreachable by greedy heuristic (likely unsatisfiable).");

        int best = greedyUB;

        // Prepare arrays for recursion
        int[] residual = (int[])m.Requirements.Clone();

        // Precompute for each counter which buttons (index >= idx) affect it - we will update dynamically
        // But for speed we will check feasibility on the fly.

        // Start branch-and-bound recursion
        Recurse(0, 0);

        if (best == int.MaxValue)
            throw new Exception("No solution found");

        return best;


        // ----- Local functions -----

        // Greedy heuristic: repeatedly pick the button that reduces the number of remaining positive counters the most.
        // Returns total presses found, or int.MaxValue if cannot make progress.
        int GreedyUpperBound(List<int[]> buttonsOrdered, int[] resid)
        {
            int presses = 0;
            int dimsLocal = resid.Length;
            int B = buttonsOrdered.Count;
            int iterationsSafeCap = resid.Sum() * 2 + 1000; // safety

            while (resid.Any(x => x > 0) && iterationsSafeCap-- > 0)
            {
                // Choose button that gives the best immediate improvement:
                // improvement = sum_i min(1, resid[i]) for counters that button affects.
                int bestIdx = -1;
                int bestGain = -1;
                for (int bi = 0; bi < B; bi++)
                {
                    int gain = 0;
                    var btn = buttonsOrdered[bi];
                    for (int i = 0; i < dimsLocal; i++)
                    {
                        if (btn[i] == 1 && resid[i] > 0) gain++;
                    }
                    if (gain > bestGain) { bestGain = gain; bestIdx = bi; }
                }

                if (bestGain <= 0)
                {
                    // No button reduces remaining positive counters -> cannot reach target
                    return int.MaxValue;
                }

                // Press best button once
                var bestBtn = buttonsOrdered[bestIdx];
                for (int i = 0; i < dimsLocal; i++)
                {
                    if (bestBtn[i] == 1 && resid[i] > 0) resid[i]--;
                }

                presses++;
            }

            if (resid.All(x => x == 0)) return presses;
            return int.MaxValue;
        }

        // Main recursive branch-and-bound
        void Recurse(int idx, int currentPresses)
        {
            // Quick prune: if current presses already not better than best:
            if (currentPresses >= best) return;

            int dimsLocal = residual.Length;
            int B = orderedButtons.Count;

            // If all counters satisfied
            if (residual.All(r => r == 0))
            {
                best = Math.Min(best, currentPresses);
                return;
            }

            // If we exhausted buttons
            if (idx >= B)
            {
                return;
            }

            // Feasibility check: for each remaining counter, ensure there's at least one remaining button that affects it
            for (int i = 0; i < dimsLocal; i++)
            {
                if (residual[i] > 0)
                {
                    bool some = false;
                    for (int bi = idx; bi < B; bi++)
                        if (orderedButtons[bi][i] == 1) { some = true; break; }
                    if (!some) return; // can't satisfy counter i
                }
            }

            // Lower bounds:
            int maxResidual = residual.Max(); // need at least this many presses (some counter needs that many increments)
            int totalResidual = residual.Sum();
            int lbByCoverage = (maxButtonWeight == 0) ? int.MaxValue : (int)((totalResidual + maxButtonWeight - 1) / maxButtonWeight);
            int lowerBound = Math.Max(maxResidual, lbByCoverage);

            if (currentPresses + lowerBound >= best) return; // prune


            // Bound maximum presses for this button:
            // For the button at idx, pressing it more than min(residual[i] for affected counters i) would over-increment some affected counter.
            // So x_max = min_{i | btn[i]==1} residual[i]. If button affects none (weight 0), skip it.
            var btnVec = orderedButtons[idx];
            int btnWeightLocal = btnVec.Sum();
            if (btnWeightLocal == 0)
            {
                // This button does nothing - skip it entirely
                Recurse(idx + 1, currentPresses);
                return;
            }

            int xMax = int.MaxValue;
            for (int i = 0; i < dimsLocal; i++)
            {
                if (btnVec[i] == 1)
                    xMax = Math.Min(xMax, residual[i]);
            }
            if (xMax == int.MaxValue) xMax = 0;

            // Also cap by remaining budget before reaching current best
            xMax = Math.Min(xMax, best - 1 - currentPresses);

            // Try larger x first (find good solutions early)
            for (int x = xMax; x >= 0; x--)
            {
                // Apply x presses of button idx
                if (x > 0)
                {
                    for (int i = 0; i < dimsLocal; i++)
                        if (btnVec[i] == 1)
                            residual[i] -= x;
                }

                // Recurse to next button
                Recurse(idx + 1, currentPresses + x);

                // Undo
                if (x > 0)
                {
                    for (int i = 0; i < dimsLocal; i++)
                        if (btnVec[i] == 1)
                            residual[i] += x;
                }

                // Small optimization: if we've found a solution equal to currentPresses (i.e., zero additional cost), we can stop early.
                if (best <= currentPresses) break;
            }
        }
    }
}
