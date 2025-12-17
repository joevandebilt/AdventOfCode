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

    public static T[] CloneArray<T>(this T[] matrix)
    {
        var clone = new T[matrix.Length];
        Array.Copy(matrix, clone, matrix.Length);
        return clone;
    }

    public static T[,] CloneArray<T>(this T[,] matrix)
    {
        var clone = new T[matrix.GetLength(0), matrix.GetLength(1)];
        Array.Copy(matrix, clone, matrix.Length);
        return clone;
    }

    public static T[,] Rotate90Clockwise<T>(this T[,] matrix)
    {
        int n = matrix.GetLength(0);

        // Step 1: Transpose the matrix
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                T temp = matrix[i, j];
                matrix[i, j] = matrix[j, i];
                matrix[j, i] = temp;
            }
        }

        // Step 2: Reverse each row
        for (int i = 0; i < n; i++)
        {
            int left = 0;
            int right = n - 1;

            while (left < right)
            {
                T temp = matrix[i, left];
                matrix[i, left] = matrix[i, right];
                matrix[i, right] = temp;

                left++;
                right--;
            }
        }

        return matrix;
    }
}
