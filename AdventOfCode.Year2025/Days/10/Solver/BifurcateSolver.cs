using AdventOfCode.Year2025.Days.DayTen;

namespace AdventOfCode.Year2025.Days.DayTen.Solver;

public class BifurcateSolver
{
    private readonly int mButtons;
    private readonly int nCounters;
    private readonly int[][] buttonEffects; // buttonEffects[b][i] = 0/1
                                            // For each parity mask (0..(1<<nCounters)-1) store list of pairs (sumVec, cost)
    private readonly Dictionary<int, List<(int[] sumVec, int cost)>> parityGroups;
    private readonly Dictionary<string, long> memo; // key = serialized target -> min presses

    private const long INF = (long)1_000_000_000_000;

    public BifurcateSolver(Machine machine)
    {
        this.buttonEffects = machine.ButtonEffects.ToArray();
        this.mButtons = machine.ButtonEffects.Count;
        this.nCounters = machine.LightsCount;
        this.parityGroups = new Dictionary<int, List<(int[] sumVec, int cost)>>();
        this.memo = new Dictionary<string, long>();

        PrecomputeParitySubsets();
    }

    // Precompute all 2^m subsets: parity mask, sum vector, and cost (popcount).
    private void PrecomputeParitySubsets()
    {
        int maxMask = 1 << mButtons;
        for (int mask = 0; mask < maxMask; mask++)
        {
            int parity = 0;
            var sumVec = new int[nCounters];
            int cost = 0;
            for (int b = 0; b < mButtons; b++)
            {
                if (((mask >> b) & 1) != 0)
                {
                    cost++;
                    var bev = buttonEffects[b];
                    for (int i = 0; i < nCounters; i++)
                    {
                        sumVec[i] += bev[i];
                    }
                }
            }
            // compute parity bits from sumVec mod 2
            for (int i = 0; i < nCounters; i++)
                if ((sumVec[i] & 1) != 0) parity |= (1 << i);

            if (!parityGroups.TryGetValue(parity, out var list))
            {
                list = new List<(int[] sumVec, int cost)>();
                parityGroups[parity] = list;
            }
            // store a clone of sumVec
            list.Add((sumVec.ToArray(), cost));
        }

        // For each parity group, reduce dominated entries:
        // Keep only (sumVec, cost) pairs not dominated by another (componentwise <= and cost <=).
        foreach (var key in parityGroups.Keys.ToList())
        {
            var list = parityGroups[key];
            // For identical sumVec keep minimal cost
            var dict = new Dictionary<string, int>();
            foreach (var (sv, c) in list)
            {
                string k = SerializeVec(sv);
                if (!dict.TryGetValue(k, out var cur) || c < cur) dict[k] = c;
            }
            var condensed = dict.Select(kv => (Vec: DeserializeVec(kv.Key), Cost: kv.Value)).ToList();

            // Now apply dominance: remove any entry dominated by another
            var kept = new List<(int[] sumVec, int cost)>();
            for (int i = 0; i < condensed.Count; i++)
            {
                bool dominated = false;
                var (vi, ci) = condensed[i];
                for (int j = 0; j < condensed.Count; j++)
                {
                    if (i == j) continue;
                    var (vj, cj) = condensed[j];
                    // j dominates i if vj[k] <= vi[k] for all k AND cj <= ci, and at least one strict
                    bool allLe = true;
                    bool someStrict = false;
                    for (int k = 0; k < nCounters; k++)
                    {
                        if (vj[k] > vi[k]) { allLe = false; break; }
                        if (vj[k] < vi[k]) someStrict = true;
                    }
                    if (allLe && cj <= ci && someStrict)
                    {
                        dominated = true;
                        break;
                    }
                }
                if (!dominated) kept.Add((vi, ci));
            }
            parityGroups[key] = kept;
        }
    }

    // Public solve entry. Returns minimal presses or throws if impossible.
    public long Solve(int[] target)
    {
        if (target.Length != nCounters) throw new ArgumentException("target length mismatch");
        var tcopy = target.ToArray();
        // quick infeasibility: if any target < 0 -> impossible
        if (tcopy.Any(x => x < 0)) throw new ArgumentException("target negative");

        long ans = SolveRecursive(tcopy);
        if (ans >= INF) throw new Exception("No solution found");
        return ans;
    }

    // Recursively solve
    private long SolveRecursive(int[] target)
    {
        // base
        if (AllZero(target)) return 0;
        if (target.Any(x => x < 0)) return INF;

        string key = SerializeVec(target);
        if (memo.TryGetValue(key, out var val)) return val;

        // parity mask of target
        int parity = 0;
        for (int i = 0; i < nCounters; i++) if ((target[i] & 1) != 0) parity |= (1 << i);

        // if no parity subsets give this parity -> impossible
        if (!parityGroups.TryGetValue(parity, out var candidates))
        {
            memo[key] = INF;
            return INF;
        }

        long best = INF;

        // iterate candidates
        foreach (var (sumVec, cost) in candidates)
        {
            bool ok = true;
            var rem = new int[nCounters];
            for (int i = 0; i < nCounters; i++)
            {
                rem[i] = target[i] - sumVec[i];
                if (rem[i] < 0) { ok = false; break; } // cannot subtract more than target
                if ((rem[i] & 1) != 0) { ok = false; break; } // parity mismatch (shouldn't happen if parity computed correctly)
            }
            if (!ok) continue;

            // half the remainder (all even)
            var half = new int[nCounters];
            for (int i = 0; i < nCounters; i++) half[i] = rem[i] >> 1;

            long sub = SolveRecursive(half);
            if (sub >= INF) continue;

            long total = 2 * sub + cost;
            if (total < best) best = total;
        }

        memo[key] = best;
        return best;
    }

    // helpers
    private static bool AllZero(int[] v)
    {
        for (int i = 0; i < v.Length; i++) if (v[i] != 0) return false;
        return true;
    }

    private static string SerializeVec(int[] v) => string.Join(",", v);
    private static int[] DeserializeVec(string s) => s.Split(',').Select(int.Parse).ToArray();
}
