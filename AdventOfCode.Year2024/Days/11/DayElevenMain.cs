using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;
using System.Diagnostics;

namespace AdventOfCode.Year2024.Days.DayEleven;
public class DayElevenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DayElevenMain() : base(Day.Eleven, _debugging) { }

    public override async Task Run()
    {
        var pythonExe = @"C:\Users\JoevandeBilt\AppData\Local\Microsoft\WindowsApps\python.exe";
        var scriptPath = Path.Combine(GetCurrentFilePath(), "DayEleven.py");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = scriptPath,
            WorkingDirectory = GetCurrentFilePath(),
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process pythonScript = Process.Start(startInfo)!)
        {
            string result = pythonScript!.StandardOutput.ReadToEnd();
            var parts = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            SetResult1(long.Parse(parts.First()));
            SetResult2(long.Parse(parts.Last()));
        }


        await base.Run();
    }
}
