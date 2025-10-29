namespace AdventOfCode.Shared.Extensions;
public static class DictionaryExtensions
{
    public static void UpsertEntry<T>(this IDictionary<T, int> dictionary, T key, int count = 1)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] += count;
        else
            dictionary.Add(key, count);
    }
    public static void UpsertEntry<T>(this IDictionary<T, long> dictionary, T key, long count = 1)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key]+=count;
        else
            dictionary.Add(key, count);
    }
}
