var lines = File.ReadAllLines("input.txt");

var registers = new Registers(
    A: long.Parse(lines[0][11..]),
    B: long.Parse(lines[1][11..]),
    C: long.Parse(lines[2][11..]));

var program = lines[4][9..].Split(',').Select(int.Parse).ToArray();
var outputs = new List<string>();
long instructionPointer = 0;
while (instructionPointer < program.Length)
{
    var instruction = ParseInstruction(opcode: program[instructionPointer]);
    var operand = program[instructionPointer + 1];
    switch (instruction)
    {
        case Jnz jnz:
            var nextInstructionPointer = jnz.GetNextInstructionPointer(operand, registers);
            if (nextInstructionPointer is not null)
            {
                instructionPointer = nextInstructionPointer.Value;
                continue;
            }
            break;
        case Out @out: outputs.Add(@out.GetOutput(operand, registers)); break;
        case IRegisterInstruction registerInstruction:
            registers = registerInstruction.Execute(operand, registers);
            break;
    }

    instructionPointer += 2;
}

Console.WriteLine(string.Join(',', outputs));
return;

static IInstruction ParseInstruction(int opcode) =>
    opcode switch
    {
        0 => new Adv(),
        1 => new Bxl(),
        2 => new Bst(),
        3 => new Jnz(),
        4 => new Bxc(),
        5 => new Out(),
        6 => new Bdv(),
        7 => new Cdv(),
        _ => throw new InvalidOperationException($"Unknown opcode {opcode}")
    };

internal record Registers(long A, long B, long C);

internal interface IInstruction;

internal interface IRegisterInstruction : IInstruction
{
    Registers Execute(int operand, Registers registers);
}

internal static class ComboOperand
{
    public static long CalculateValue(int operand, Registers registers) =>
        operand switch
        {
            >= 0 and <= 3 => operand,
            4 => registers.A,
            5 => registers.B,
            6 => registers.C,
            _ => throw new InvalidOperationException($"Unknown operand {operand}")
        };
}

internal class Adv : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) =>
        registers with
        {
            A = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}

internal class Bxl : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) => registers with { B = registers.B ^ operand };
}

internal class Bst : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) =>
        registers with { B = ComboOperand.CalculateValue(operand, registers) % 8 };
}

internal class Jnz : IInstruction
{
    public long? GetNextInstructionPointer(int operand, Registers registers) =>
        registers.A == 0 ? null : operand;
}

internal class Bxc : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) =>
        registers with { B = registers.B ^ registers.C };
}

internal class Out : IInstruction
{
    public string GetOutput(int operand, Registers registers) =>
        (ComboOperand.CalculateValue(operand, registers) % 8).ToString();
}

internal class Bdv : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) =>
        registers with
        {
            B = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}

internal class Cdv : IRegisterInstruction
{
    public Registers Execute(int operand, Registers registers) =>
        registers with
        {
            C = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}