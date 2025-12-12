namespace AdventOfCode.Shared.Extensions;

public static class BitmaskExtensions
{
    public static int[] ToBitmask(this long value, int length)
    {
        int[] bits = new int[length];
        for (int i = 0; i < length; i++)
        {
            int bitIndex = length - 1 - i;
            if (((value >> bitIndex) & 1) == 1)
            {
                bits[i] = 1;
            }
        }
        return bits;
    }

    public static long ToNumber(this int[] bits)
    {
        long mask = 0;
        for (int i = 0; i < bits.Length; i++)
        {
            mask <<= 1; // shift left to make room for next bit
            if (bits[i] == 1)
            {
                mask |= 1; // set lowest bit
            }
        }
        return mask;
    }

    public static int[] ToParityArray(this int[] values)
    {
        int[] bits = new int[values.Length];
        for (int i = 0; i < bits.Length; i++)
        {
            if (values[i] % 2 != 0)
                bits[i] = 1;
        }

        return bits;
    }
}
