namespace AdventOfCode.Shared.Extensions;

public static class ArrayExtensions
{
    public static int[] Subtract(this int[] a, int[] b)
    {
        if (a.Length != b.Length) throw new ArgumentException("Arrays must have equal length");
        return a.Select((a, i) => a - b[i]).ToArray();
    }

    public static bool ArrayEquals(this int[] a, int[] b)
    {
        if (a.Length != b.Length) throw new ArgumentException("Arrays must have equal length to be compared");
        for (int i = 0; i < a.Length; i++)
        {
            if (b[i] != a[i]) return false;
        }
        return true;
    }

    public static bool IsMultiple(this int[] target, int[] candidate, out int factor)
    {
        // Check if target is an integer multiple of candidate
        factor = -1;
        for (int i = 0; i < target.Length; i++)
        {
            if (candidate[i] == 0 && target[i] == 0) continue;
            if (candidate[i] == 0) return false;

            int ratio = target[i] / candidate[i];
            if (target[i] % candidate[i] != 0) return false;

            if (factor == -1) factor = ratio;
            else if (factor != ratio) return false;
        }
        return factor > 0;
    }

    public static bool IsMultiple(this int[] target, int[] candidate)
    {
        return IsMultiple(target, candidate, out int _);
    }
}
