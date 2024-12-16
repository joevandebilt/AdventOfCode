using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Text;

namespace AdventOfCode.Year2024.Days.DayNine;
public class DayNineMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayNineMain() : base(Day.Nine, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();
        var diskMap = linesOfInput.First();

        int id = 0;
        bool freeSpace = false;
        List<string> entries = new();
        List<DiskMapEntry> diskMapEntries = new();
        foreach (var fileMap in diskMap)
        {
            int length = int.Parse($"{fileMap}");
            for (int i = 0; i < length; i++)
            {
                if (!freeSpace)
                    entries.Add($"{id}");
                else
                    entries.Add(".");
            }

            if (!freeSpace)
                id++;

            freeSpace = !freeSpace;
        }
        WriteLine($"{string.Concat(entries)}");

        //Defrag
        var defragPassOne = entries.Select(e => e).ToList();
        for (int i = 0; i < defragPassOne.Count; i++)
        {
            if (defragPassOne[i] == ".")
            {
                //find the rightmost non-empty entry to fill
                var rightmost = defragPassOne.Where((e, index) => index > i && e != ".").LastOrDefault();
                if (rightmost != null)
                {
                    var freespaceIndex = defragPassOne.LastIndexOf(rightmost);
                    defragPassOne[i] = rightmost;
                    defragPassOne[freespaceIndex] = ".";
                }
            }
        }
        WriteLine(string.Concat(defragPassOne));

        var sums = defragPassOne.Select((e, i) => e == "." ? 0 : long.Parse(e.ToString()) * i);
        SetResult1(sums.Sum());

        var defragPassTwo = entries.Select(e => e).ToList();
        for (int i = defragPassTwo.Count - 1; i >= 0; i--)
        {
            if (defragPassTwo[i] != ".")
            {
                //get the file id and length
                var fileId = defragPassTwo[i];
                var fileLength = defragPassTwo.Count(e => e == fileId);

                var map = string.Concat(defragPassTwo.Select(e => e == "." ? "." : "0"));
                var blockNeeded = ".".PadRight(fileLength, '.');
                var freeIndex = map.IndexOf(blockNeeded);
                if (freeIndex > 0 && freeIndex < i)
                {
                    for (int j = 0; j < fileLength; j++)
                    {
                        defragPassTwo[freeIndex + j] = fileId;
                        defragPassTwo[i - j] = ".";
                    }
                }
            }
        }
        WriteLine(string.Concat(defragPassTwo));
        sums = defragPassTwo.Select((e, i) => e == "." ? 0 : long.Parse(e.ToString()) * i);
        SetResult2(sums.Sum());
        await base.Run();
    }

    private string ConvertToFileMap(List<DiskMapEntry> diskMap)
    {
        StringBuilder mapAsLine = new StringBuilder();
        foreach (var map in diskMap)
        {
            if (map.FreeSpace)
            {
                mapAsLine.Append(".".PadRight(map.Length, '.'));
            }
            else
            {
                mapAsLine.Append(map.Id.ToString().PadRight(map.Length, map.Id.ToString()[0]));
            }
        }
        WriteLine(mapAsLine.ToString());
        return mapAsLine.ToString();
    }
}
