using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Year2024.Days.DayThirteen;
public class DayThirteenMain : AdventOfCodeDay
{
    private const bool _debugging = false;
    public DayThirteenMain() : base(Day.Thirteen, _debugging) { }

    public override async Task Run()
    {
        var linesOfInput = await LoadFile(forceLower: false);
        List<ClawMachine> machines = new();

        var machine = new ClawMachine();
        foreach (var line in linesOfInput)
        {
            if (line.Trim() == string.Empty)
            {
                machines.Add(machine);
                machines.Add(new ClawMachine
                {
                    AButton = machine.AButton,
                    BButton = machine.BButton,
                    Prize = new Prize
                    {
                        X = machine.Prize.X + 10000000000000,
                        Y = machine.Prize.Y + 10000000000000,
                    },
                    Big = true
                });
                machine = new ClawMachine();
                continue;
            }

            var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries).Last().Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (machine.AButton == null)
            {
                //This line is A Button
                machine.AButton = new ButtonConfiguration
                {
                    Identifier = 'A',
                    XMove = int.Parse(parts.First().Split('+').Last()),
                    YMove = int.Parse(parts.Last().Split('+').Last()),
                };
            }
            else if (machine.BButton == null)
            {
                //This line is B Button               
                machine.BButton = new ButtonConfiguration
                {
                    Identifier = 'B',
                    XMove = int.Parse(parts.First().Split('+').Last()),
                    YMove = int.Parse(parts.Last().Split('+').Last()),
                };
            }
            else
            {
                //This line is machine
                machine.Prize = new Prize
                {
                    X = int.Parse(parts.First().Split('=').Last()),
                    Y = int.Parse(parts.Last().Split('=').Last())
                };
            }
        }
        machines.Add(machine);
        machines.Add(new ClawMachine
        {
            AButton = machine.AButton,
            BButton = machine.BButton,
            Prize = new Prize
            {
                X = machine.Prize.X + 10000000000000,
                Y = machine.Prize.Y + 10000000000000,
            },
            Big = true
        });

        foreach (var m in machines)
        {
            long aCount = 0;
            long bCount = 0;

            bCount = (m.AButton.XMove * m.Prize.Y - m.AButton.YMove*m.Prize.X) / (m.AButton.XMove*m.BButton.YMove - m.AButton.YMove*m.BButton.XMove);
            aCount = (m.Prize.X - (bCount * m.BButton.XMove)) / m.AButton.XMove;

            if ((aCount*m.AButton.XMove + bCount*m.BButton.XMove != m.Prize.X) 
                ||
                (aCount * m.AButton.YMove + bCount * m.BButton.YMove != m.Prize.Y))
            {
                m.Possible = false;
            }
            else
            {
                m.Cost = (3 * aCount) + bCount;
                m.Possible = true;
            }                
        }

        foreach (var m in machines)
        {
            if (!m.Possible)
                WriteLine($"Machine X={m.Prize.X}, Y={m.Prize.Y} was Impossible");
            else
                WriteLine($"Machine X={m.Prize.X}, Y={m.Prize.Y} cost {m.Cost}");

        }

        SetResult1(machines.Where(m => m.Possible && !m.Big).Sum(m => m.Cost));
        SetResult2(machines.Where(m => m.Possible && m.Big).Sum(m => m.Cost));
        await base.Run();
    }
}