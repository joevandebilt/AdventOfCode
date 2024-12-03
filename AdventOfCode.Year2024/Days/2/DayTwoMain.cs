using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Template;
public class DayTwoMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayTwoMain() : base(Day.Two, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile();

        List<int[]> reports = new();
        foreach (var line in linesOfInput)
        {
            var reportValues = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            reports.Add(reportValues.Select(x => int.Parse(x)).ToArray());
        }

        int safeReports = 0;
        int failSafeReports = 0;
        foreach (var report in reports)
        {
            var differences = GetDifferences(report);
            if (SafeReport(differences))
            {
                safeReports++;
            }
            else
            {
                for (int i = 0; i < report.Length; i++)
                {
                    //Remove a level and get differences
                    var singleLayerRemoved = report.Where((d, index) => index != i).ToArray();
                    var faultTolerance = GetDifferences(singleLayerRemoved);                    

                    if (SafeReport(faultTolerance))
                    {
                        failSafeReports++;
                        i = differences.Length;
                    }
                }
            }
        }

        SetResult1(safeReports);
        SetResult2(safeReports + failSafeReports);
        await base.Run();
    }

    private int[] GetDifferences(int[] reportValues)
    {
        return reportValues.Take(reportValues.Length - 1).Select((v, i) => reportValues[i + 1] - v).ToArray();
    }

    private bool SafeReport(int[] differences)
    {
        return (differences.All(d => d > 0) || differences.All(d => d < 0)) 
            && differences.All(d => Math.Abs(d) >= 1 && Math.Abs(d) <= 3);
    }
}
