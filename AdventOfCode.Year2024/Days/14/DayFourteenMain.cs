using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayFourteen;
public class DayFourteen : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayFourteen() : base(Day.Fourteen, _debugging) { }

    public override async Task Run()
    {
        var Robots = new List<Robot>();
        var linesOfInput = await LoadFile();
        foreach (var line in linesOfInput)
        {
            var robotSpecs = line.Split(' ');
            var position = robotSpecs.First().Split('=').Last();
            var velocity = robotSpecs.Last().Split('=').Last();

            Robots.Add(new Robot
            {
                Position = new(position),
                Velocity = new(velocity)
            });
        }

        var width = 101;
        var height = 103;

        var safetyRow = height / 2;
        var safetyCol = width / 2;

        var seconds = 100;

        RunPositions(seconds, width, height, Robots);
        var quads = CheckQuads(Robots, safetyRow, safetyCol);
        var minSafety = SafetyFactor(quads);
        SetResult1(minSafety);


        while (seconds < 10000)
        {
            seconds++;
            RunPositions(1, width, height, Robots);
            quads = CheckQuads(Robots, safetyRow, safetyCol);
            var safety = SafetyFactor(quads);
            if (safety < minSafety)
            {
                WriteLine($"Printing at {seconds} Seconds");
                PrintGrid(width, height, Robots);                
                minSafety = safety;
                SetResult2(seconds);
            }
        }

        await base.Run();
    }

    private void RunPositions(int timeLimit, int width, int height, List<Robot> Robots)
    {
        var safetyCol = width / 2;
        for (int seconds = 0; seconds < timeLimit; seconds++)
        {
            foreach (var robot in Robots)
            {
                robot.Position.X += robot.Velocity.X;
                robot.Position.Y += robot.Velocity.Y;

                if (robot.Position.X < 0)
                    robot.Position.X += width;

                if (robot.Position.X >= width)
                    robot.Position.X -= width;

                if (robot.Position.Y < 0)
                    robot.Position.Y += height;

                if (robot.Position.Y >= height)
                    robot.Position.Y -= height;
            }
        }
    }

    private long SafetyFactor(int[] quads)
    {
        var safetyFactor = quads.Aggregate((a, b) => a * b);
        return safetyFactor;
    }

    private int[] CheckQuads(List<Robot> Robots, int safetyRow, int safetyCol)
    {
        var quads = new int[4];
        foreach (var robot in Robots)
        {
            if (robot.Position.X < safetyCol && robot.Position.Y < safetyRow)
                quads[0]++;
            else if (robot.Position.X > safetyCol && robot.Position.Y < safetyRow)
                quads[1]++;
            else if (robot.Position.X < safetyCol && robot.Position.Y > safetyRow)
                quads[2]++;
            else if (robot.Position.X > safetyCol && robot.Position.Y > safetyRow)
                quads[3]++;
            else
                continue;
        }
        return quads;
    }

    private void PrintGrid(int width, int height, List<Robot> Robots)
    {
        Clear();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (Robots.Any(r => r.Position.X == col && r.Position.Y == row))
                    Write("X");
                else 
                    Write(".");
            }
            Write(Environment.NewLine);
        }
    }
}
