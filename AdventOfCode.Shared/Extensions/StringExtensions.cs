namespace AdventOfCode.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceCharAtIndex(this string input, int index, char ch)
        {
            char[] characters = input.ToCharArray();
            characters[index] = ch;
            return new string(characters);
        }
    }
}
