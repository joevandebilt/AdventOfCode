using AdventOfCode.Year2025.Days.DayTen;

namespace AdventOfCode.Year2025.Days.Day10;

public static class SmithNormalFormSolver
{
    // Extended gcd for two integers (returns (g, s, t) with s*a + t*b = g)
    private static (long g, long s, long t) ExtendedGcd(long a, long b)
    {
        if (b == 0) return (Math.Abs(a), a >= 0 ? 1 : -1, 0);
        long old_r = a, r = b;
        long old_s = 1, s = 0;
        long old_t = 0, t = 1;
        while (r != 0)
        {
            long q = old_r / r;
            (old_r, r) = (r, old_r - q * r);
            (old_s, s) = (s, old_s - q * s);
            (old_t, t) = (t, old_t - q * t);
        }
        long g = old_r;
        // make g positive
        if (g < 0) { g = -g; old_s = -old_s; old_t = -old_t; }
        return (g, old_s, old_t);
    }

    // Helper: deep copy a 2D array
    private static long[,] CloneMat(long[,] A)
    {
        int m = A.GetLength(0), n = A.GetLength(1);
        var C = new long[m, n];
        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                C[i, j] = A[i, j];
        return C;
    }

    // Swap rows
    private static void SwapRows(long[,] M, int r1, int r2)
    {
        int n = M.GetLength(1);
        for (int j = 0; j < n; j++) { var tmp = M[r1, j]; M[r1, j] = M[r2, j]; M[r2, j] = tmp; }
    }

    // Swap cols
    private static void SwapCols(long[,] M, int c1, int c2)
    {
        int m = M.GetLength(0);
        for (int i = 0; i < m; i++) { var tmp = M[i, c1]; M[i, c1] = M[i, c2]; M[i, c2] = tmp; }
    }

    // Add k * row src to row dst (dst += k*src)
    private static void AddRowMultiple(long[,] M, int dst, int src, long k)
    {
        int n = M.GetLength(1);
        for (int j = 0; j < n; j++) M[dst, j] += k * M[src, j];
    }

    // Add k * col src to col dst (dst += k*src)
    private static void AddColMultiple(long[,] M, int dst, int src, long k)
    {
        int m = M.GetLength(0);
        for (int i = 0; i < m; i++) M[i, dst] += k * M[i, src];
    }

    // Produce identity matrix
    private static long[,] Identity(int n)
    {
        var I = new long[n, n];
        for (int i = 0; i < n; i++) I[i, i] = 1;
        return I;
    }

    // Multiply matrices (long)
    private static long[,] Mul(long[,] A, long[,] B)
    {
        int m = A.GetLength(0), p = A.GetLength(1), n = B.GetLength(1);
        var C = new long[m, n];
        for (int i = 0; i < m; i++)
            for (int k = 0; k < p; k++) if (A[i, k] != 0)
                    for (int j = 0; j < n; j++)
                        C[i, j] += A[i, k] * B[k, j];
        return C;
    }

    // Multiply matrix by vector
    private static long[] Mul(long[,] A, long[] v)
    {
        int m = A.GetLength(0), n = A.GetLength(1);
        var r = new long[m];
        for (int i = 0; i < m; i++)
        {
            long sum = 0;
            for (int j = 0; j < n; j++) sum += A[i, j] * v[j];
            r[i] = sum;
        }
        return r;
    }

    // Compute Smith Normal Form. Returns (D, U, V) so that U*A*V = D.
    // A is m x n.
    public static (long[,] D, long[,] U, long[,] V) SNF(long[,] Aorig)
    {
        var A = CloneMat(Aorig);
        int m = A.GetLength(0), n = A.GetLength(1);
        var U = Identity(m);
        var V = Identity(n);

        int k = 0; // pivot index
        while (k < m && k < n)
        {
            // Find smallest non-zero absolute value element in submatrix A[k.., k..]
            int p = -1, q = -1;
            long minAbs = 0;
            for (int i = k; i < m; i++)
                for (int j = k; j < n; j++)
                {
                    long val = A[i, j];
                    if (val != 0)
                    {
                        long aval = Math.Abs(val);
                        if (p == -1 || aval < minAbs) { minAbs = aval; p = i; q = j; }
                    }
                }

            if (p == -1)
            {
                // all zeros in submatrix => done
                break;
            }

            // Move A[p,q] to A[k,k]
            if (p != k) { SwapRows(A, p, k); SwapRows(U, p, k); }
            if (q != k) { SwapCols(A, q, k); SwapCols(V, q, k); }

            // Use extended Euclid process to make A[k,k] divide all entries in its row and column
            bool changed = true;
            while (changed)
            {
                changed = false;

                // For each row i > k, reduce A[i,k] by row operations using A[k,k]
                for (int i = k + 1; i < m; i++)
                {
                    if (A[i, k] != 0)
                    {
                        var (g, s, t) = ExtendedGcd(A[k, k], A[i, k]);
                        // create unimodular row operations:
                        // [ s  t ] [A[k,k]]   => [g]
                        // [-r  u ] [A[i,k]]      [...]
                        // We'll use row ops to replace rows k and i.
                        // Construct transform matrix R on rows applied on left; implement by row ops on A and U.
                        // But easier: perform integer combination to replace rows:
                        // let a = A[k,k], b = A[i,k]
                        long a = A[k, k], b = A[i, k];
                        // the combination: new row k = s*row k + t*row i
                        // new row i = (-b/g)*row k + (a/g)*row i  (these coefficients are integers)
                        long s1 = s, t1 = t;
                        long mul1 = -(b / g);
                        long mul2 = a / g;

                        // Apply to A's rows
                        // tempRowK = s1 * rowK + t1 * rowI
                        var tempRowK = new long[n];
                        var tempRowI = new long[n];
                        for (int j = 0; j < n; j++)
                        {
                            tempRowK[j] = s1 * A[k, j] + t1 * A[i, j];
                            tempRowI[j] = mul1 * A[k, j] + mul2 * A[i, j];
                        }
                        for (int j = 0; j < n; j++) { A[k, j] = tempRowK[j]; A[i, j] = tempRowI[j]; }

                        // Apply same to U (row operations on left)
                        var tempURowK = new long[m];
                        var tempURowI = new long[m];
                        for (int j = 0; j < m; j++)
                        {
                            tempURowK[j] = s1 * U[k, j] + t1 * U[i, j];
                            tempURowI[j] = mul1 * U[k, j] + mul2 * U[i, j];
                        }
                        for (int j = 0; j < m; j++) { U[k, j] = tempURowK[j]; U[i, j] = tempURowI[j]; }

                        changed = true;
                    }
                }

                // For each column j > k, reduce A[k,j] by column operations using A[k,k]
                for (int j = k + 1; j < n; j++)
                {
                    if (A[k, j] != 0)
                    {
                        var (g, s, t) = ExtendedGcd(A[k, k], A[k, j]);
                        long a = A[k, k], b = A[k, j];
                        long s1 = s, t1 = t;
                        long mul1 = -(b / g);
                        long mul2 = a / g;

                        // Apply to columns of A
                        var tempColK = new long[m];
                        var tempColJ = new long[m];
                        for (int i2 = 0; i2 < m; i2++)
                        {
                            tempColK[i2] = s1 * A[i2, k] + t1 * A[i2, j];
                            tempColJ[i2] = mul1 * A[i2, k] + mul2 * A[i2, j];
                        }
                        for (int i2 = 0; i2 < m; i2++) { A[i2, k] = tempColK[i2]; A[i2, j] = tempColJ[i2]; }

                        // Apply same to V (column operations on right)
                        var tempVColK = new long[n];
                        var tempVColJ = new long[n];
                        for (int j2 = 0; j2 < n; j2++)
                        {
                            tempVColK[j2] = s1 * V[k, j2] + t1 * V[j, j2];
                            tempVColJ[j2] = mul1 * V[k, j2] + mul2 * V[j, j2];
                        }
                        for (int j2 = 0; j2 < n; j2++) { V[k, j2] = tempVColK[j2]; V[j, j2] = tempVColJ[j2]; }

                        changed = true;
                    }
                }
            } // while changed

            // Now eliminate all nonzero entries in column k (below and above) and row k (right and left)
            for (int i = 0; i < m; i++)
                if (i != k && A[i, k] != 0)
                {
                    long quim = A[i, k] / A[k, k];
                    AddRowMultiple(A, i, k, -quim);
                    AddRowMultiple(U, i, k, -quim);
                    if (A[i, k] != 0)
                    {
                        // if still nonzero, perform one more elimination by swapping
                        // swap rows i and k and restart
                        SwapRows(A, i, k); SwapRows(U, i, k);
                        goto continueOuter;
                    }
                }

            for (int j = 0; j < n; j++)
                if (j != k && A[k, j] != 0)
                {
                    long quim = A[k, j] / A[k, k];
                    AddColMultiple(A, j, k, -quim);
                    AddColMultiple(V, j, k, -quim);
                    if (A[k, j] != 0)
                    {
                        SwapCols(A, j, k); SwapCols(V, j, k);
                        goto continueOuter;
                    }
                }

            // Ensure diagonal divides next diagonal (d_k divides d_{k+1}, ...). If not, fix by combining columns/rows.
            // This simple algorithm will rely on the earlier Euclid adjustments to produce divisibility in practice for small matrices.
            k++;
        continueOuter:
            continue;
        }

        // Make D matrix (copy of A but only diagonal up to min(m,n))
        var D = new long[m, n];
        for (int i = 0; i < Math.Min(m, n); i++) D[i, i] = A[i, i];
        return (D, U, V);
    }

    // Solve A x = b (A: m x n, b: m). Returns minimal sum(x) and the x vector.
    // Throws if no integer solution.
    public static (long minPresses, long[] x) SolveWithSNF(Machine machine)
    {
        long[,] Aorig = new long[machine.LightsCount, machine.Buttons.Count]; 

        void PutCol(int col, int[] ones)
        {
            for (int i = 0; i < machine.LightsCount; i++)
            {
                Aorig[i, col] = 0;
            }

            foreach (var r in ones)
            {
                Aorig[r, col] = 1;
            }
        }

        for(int i =0; i<machine.ButtonEffects.Count; i++)
        {
            var cols = machine.ButtonEffects[i].Select((b, idx) => new { b, idx }).Where(x => x.b == 1).Select(x => x.idx).ToArray();
            PutCol(i, cols);
        }

        var borig = machine.Requirements.Select(r => (long)r).ToArray();

        int m = Aorig.GetLength(0), n = Aorig.GetLength(1);
        if (borig.Length != m) throw new ArgumentException("b length mismatch");

        var (D, U, V) = SNF(Aorig);
        // c = U * b
        var c = Mul(U, borig);

        // Determine rank r = number of non-zero diagonal entries
        int r = 0;
        for (int i = 0; i < Math.Min(m, n); i++) if (D[i, i] != 0) r++; else break;

        // Check solvability on the first r rows: D[i,i] must divide c[i]
        for (int i = 0; i < r; i++)
        {
            long di = D[i, i];
            long ci = c[i];
            if (ci % di != 0) throw new Exception("No integer solution (incompatible).");
        }
        // For rows i >= r (up to m-1), require c[i] == 0 else impossible
        for (int i = r; i < m; i++)
            if (c[i] != 0) throw new Exception("No integer solution (incompatible).");

        // Construct a particular z (length n) with:
        // z_i = c_i / d_i for i < r
        var zPart = new long[n];
        for (int i = 0; i < r; i++) zPart[i] = c[i] / D[i, i];
        for (int i = r; i < n; i++) zPart[i] = 0; // free variables initially 0

        // General solution: z = zPart + sum_{j=r..n-1} t_j * e_j (free integer params)
        // x = V * z  => x = V * zPart + sum t_j * (V column j)
        var xPart = Mul(V, zPart); // V * zPart
                                   // Precompute free column vectors (as long[] of length n for x coords)
        int freeCount = n - r;
        var freeCols = new long[freeCount][];
        for (int j = 0; j < freeCount; j++)
        {
            int colIdx = r + j;
            var col = new long[n];
            for (int i = 0; i < n; i++) col[i] = V[i, colIdx]; // note: V is n x n, rows index 0..n-1
            freeCols[j] = col;
        }

        // If there are no free variables, we have a unique z and thus unique x:
        if (freeCount == 0)
        {
            var xUnique = xPart;
            // check nonnegativity
            if (xUnique.Any(val => val < 0)) throw new Exception("No nonnegative solution exists.");
            long sum = xUnique.Sum();
            return (sum, xUnique);
        }

        // We now minimize sum(x) = sum(xPart + sum t_j * freeCols[j]) = const + w^T t
        // where w_j = sum_i freeCols[j][i]
        var w = new long[freeCount];
        for (int j = 0; j < freeCount; j++) w[j] = freeCols[j].Sum();

        // Nonnegativity constraints: xPart[i] + sum_j freeCols[j][i] * t_j >= 0  for each i=0..n-1
        // We'll enumerate integer vectors t = (t_0..t_{freeCount-1}) within bounds derived from these inequalities.

        // Derive naive bounds for each t_j individually by considering worst-case when other t_k = 0
        var lo = new long[freeCount];
        var hi = new long[freeCount];
        const long INF = (long)1e12;
        for (int j = 0; j < freeCount; j++)
        {
            long low = -INF, high = INF;
            for (int i = 0; i < n; i++)
            {
                long a = freeCols[j][i];
                long rhs = -xPart[i];
                if (a == 0) continue;
                if (a > 0)
                {
                    // a * t_j >= rhs  => t_j >= ceil(rhs / a)
                    long bound = DivCeil(rhs, a);
                    if (bound > low) low = bound;
                }
                else // a < 0
                {
                    // a * t_j >= rhs => t_j <= floor(rhs / a) because dividing by negative flips
                    long bound = FloorDiv(rhs, a);
                    if (bound < high) high = bound;
                }
            }
            // clamp to finite if INF
            if (low < -1000000) low = -1000000;
            if (high > 1000000) high = 1000000;
            lo[j] = low; hi[j] = high;
        }

        // Branch-and-bound enumerator across freeCount dims with pruning
        long bestSum = long.MaxValue;
        long[] bestX = null;

        // To improve pruning, evaluate a lower bound on objective for partial assignments.
        // We'll do DFS enumerating t_j in some order; choose order by absolute weight descending (impact on objective).
        var order = Enumerable.Range(0, freeCount).OrderByDescending(j => Math.Abs(w[j])).ToArray();

        // Precompute column vectors for easier access
        var freeColsOrdered = order.Select(j => freeCols[j]).ToArray();
        var wOrdered = order.Select(j => w[j]).ToArray();
        var loOrdered = order.Select(j => lo[j]).ToArray();
        var hiOrdered = order.Select(j => hi[j]).ToArray();

        // current partial t assignment
        var t = new long[freeCount];

        // current x = xPart + sum assigned t_j * freeColsOrdered[j]
        var xCur = (long[])xPart.Clone();

        // recursive DFS
        void Dfs(int idx, long objSoFar)
        {
            if (objSoFar >= bestSum) return; // prune by current objective lower bound
            if (idx == freeCount)
            {
                // all assigned, check nonnegativity (should hold) and compute sum
                if (xCur.Any(v => v < 0)) return;
                long total = xCur.Sum();
                if (total < bestSum)
                {
                    bestSum = total;
                    bestX = (long[])xCur.Clone();
                }
                return;
            }

            // compute heuristic lower bound on additional objective contributions from remaining t's
            // simplest: minimal possible addition by choosing t_j within lo..hi: if wOrdered[j] >=0 pick lo else pick hi
            long lbAdd = 0;
            for (int j = idx; j < freeCount; j++)
            {
                long pick = (wOrdered[j] >= 0) ? loOrdered[j] : hiOrdered[j];
                lbAdd += wOrdered[j] * pick;
            }
            if (objSoFar + lbAdd >= bestSum) return;

            // Enumerate t_idx within a small range: to keep it efficient, limit enumeration by feasible range
            long start = loOrdered[idx], end = hiOrdered[idx];

            // Heuristic: iterate near zero first (smaller magnitude) to find good solutions quickly
            var seq = EnumerateCentered(start, end);

            foreach (var val in seq)
            {
                // apply val
                t[idx] = val;
                // update xCur: xCur += val * freeColsOrdered[idx]
                var col = freeColsOrdered[idx];
                for (int i = 0; i < n; i++) xCur[i] += val * col[i];
                long newObj = objSoFar + wOrdered[idx] * val;

                // quick feasibility check partial: if any xCur[i] < 0 and remaining columns cannot fix it, prune
                bool feasible = true;
                for (int i = 0; i < n; i++)
                {
                    if (xCur[i] < 0)
                    {
                        // try to see if remaining free cols can increase xCur[i]
                        long maxPossibleIncrease = 0;
                        for (int j = idx + 1; j < freeCount; j++)
                        {
                            long coef = freeColsOrdered[j][i];
                            if (coef > 0)
                            {
                                // maximum increase if t_j = hiOrdered[j]
                                maxPossibleIncrease += coef * hiOrdered[j];
                            }
                            else if (coef < 0)
                            {
                                // minimum decrease if t_j = loOrdered[j], but that doesn't help increase
                            }
                        }
                        if (xCur[i] + maxPossibleIncrease < 0) { feasible = false; break; }
                    }
                }

                if (feasible)
                {
                    Dfs(idx + 1, newObj);
                }

                // undo
                for (int i = 0; i < n; i++) xCur[i] -= val * col[i];

                // small early exit if bestSum already minimal possible (0)
                if (bestSum == 0) return;
            }
        }

        // start DFS
        Dfs(0, xPart.Sum());

        if (bestX == null) throw new Exception("No nonnegative solution found");

        return (bestSum, bestX);


        // ----- helper local functions -----

        static long DivCeil(long a, long b)
        {
            if (b == 0) throw new ArgumentException();
            if (b > 0)
            {
                if (a >= 0) return (a + b - 1) / b;
                else return a / b;
            }
            else
            {
                // b < 0
                if (a >= 0) return (a + b + 1) / b;
                else return a / b;
            }
        }
        static long FloorDiv(long a, long b)
        {
            if (b == 0) throw new ArgumentException();
            if (b > 0)
            {
                if (a >= 0) return a / b;
                else return (a - (b - 1)) / b;
            }
            else
            {
                if (a >= 0) return a / b;
                else return a / b;
            }
        }

        // produce an enumeration of integers in [lo..hi] but centered near 0 first: 0, 1, -1, 2, -2, ...
        static IEnumerable<long> EnumerateCentered(long lo, long hi)
        {
            // clamp to safe small window if huge
            long L = Math.Max(lo, -1000);
            long H = Math.Min(hi, 1000);
            // yield 0 if in range
            if (L <= 0 && 0 <= H) yield return 0;
            for (long d = 1; ; d++)
            {
                bool any = false;
                long v1 = d;
                if (L <= v1 && v1 <= H) { yield return v1; any = true; }
                long v2 = -d;
                if (L <= v2 && v2 <= H) { yield return v2; any = true; }
                if (!any && (v1 > H && v2 < L)) break;
                if (d > Math.Max(Math.Abs(L), Math.Abs(H)) + 2) break;
            }
        }
    }
}
