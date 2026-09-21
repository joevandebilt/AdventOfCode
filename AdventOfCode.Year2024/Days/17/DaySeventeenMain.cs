using AdventOfCode.Shared.Base;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Template;
public class DaySeventeenMain : AdventOfCodeDay
{
    private const bool _debugging = true;
    public DaySeventeenMain() : base(Day.Seventeen, _debugging) { }

    private long A, B, C;
    private int instructionPointer, operandPointer;

    List<int> program = new();
    List<long> outputs = new();

    public override async Task Run()
    {
        instructionPointer = -2;

        var linesOfInput = await LoadFile();
        A = long.Parse(linesOfInput[0].Split(":").Last().Trim());
        B = long.Parse(linesOfInput[1].Split(":").Last().Trim());
        C = long.Parse(linesOfInput[2].Split(":").Last().Trim());
    
        program = linesOfInput.Last().Split(":").Last().Trim().Split(",").Select(int.Parse).ToList();
        while (instructionPointer < program.Count)
        {
            var opcode = GetNextInstruction();
            if (opcode >= 0)
            {
                var operand = GetNextOperand();
                var instruction = Instruction(opcode);
                instruction(operand);
            }
        }

        string output = string.Join(",", outputs);
        WriteLine($"Output: {output}");


        SetResult1(-1);
        SetResult2(-1);
        await base.Run();
    }

    private int GetNextInstruction()
    {
        Write($"Instruction {instructionPointer} -> ");
        instructionPointer += 2;
        if (instructionPointer >= program.Count) 
        {
            WriteLine("-1\tHALT");
            return -1;
        }
        WriteLine($"{instructionPointer}");
        return program[instructionPointer];
    }

    private int GetNextOperand()
    {
        return program[instructionPointer + 1];
    }

    private Action<int> Instruction(int opcode) => opcode switch
    {
        0 => Adv,
        1 => Bxl,
        2 => Bst,
        3 => Jnz,
        4 => Bxc,
        5 => Out,
        6 => Bdv,
        7 => Cdv,
        _ => throw new ArgumentOutOfRangeException("Opcode must be between 0 and 8")
    };

    private long Combo(int value)
    {
        return value switch
        {
            4 => A,
            5 => B,
            6 => C,
            7 => throw new Exception("Clearly for part 2"),
            _ => value,
        };
    }

    //0
    private void Adv(int operand)
    {
        Write($"Adv({operand}) = ");
        A = A / (long)Math.Pow(2, Combo(operand));
        WriteLine($"{A}");
    }

    //1
    private void Bxl(int operand)
    {
        Write($"Bxl({operand}) = ");
        B = (B ^ operand) & 0b111;
        WriteLine($"{B}");
    }

    //2
    private void Bst(int operand)
    {
        Write($"Bxl({operand}) = ");
        B = Combo(operand) % 8;
        WriteLine($"{B}");
    }

    //3
    private void Jnz(int operand)
    {
        Write($"Jnz({operand}) -> ");
        if (A != 0)
        {
            //Jump by 2...
            instructionPointer = operand - 2;
            WriteLine($"Instruction Changed to {instructionPointer}");
        }
        WriteLine($"Instruction unchanged");
    }

    //4
    private void Bxc(int operand)
    {
        Write($"Bxl({operand}) = ");
        B = (B ^ C) & 0b111;
        WriteLine($"{B}");
    }

    //5
    private void Out(int operand)
    {
        var cmb = Combo(operand);
        var mod = cmb % 8;
        
        WriteLine($"Out({operand}) = {cmb} % 8 = {mod}");
        outputs.Add(mod);
    }

    //6
    private void Bdv(int operand)
    {
        Write($"Bdv({operand}) = ");
        B = A / (long)Math.Pow(2, Combo(operand));
        WriteLine($"{B}");
    }

    //7
    private void Cdv(int operand)
    {
        Write($"Cdv({operand}) = ");
        C = A / (long)Math.Pow(2, Combo(operand));
        WriteLine($"{C}");
    }
}
