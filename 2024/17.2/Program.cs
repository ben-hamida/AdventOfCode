var lines = File.ReadAllLines("input.txt");

var registers = new Registers(
    A: 0,
    B: long.Parse(lines[1][11..]),
    C: long.Parse(lines[2][11..]));

var program = lines[4][9..].Split(',').Select(long.Parse).ToArray();

// We need 16 iterations to get 16 printouts
// On the last iteration A has to be 0, let's search backwards
var queue = new PriorityQueue<long, int>([(0, 16)]); // a, iteration
long? foundA = null;
while (queue.TryDequeue(out var a, out var iteration))
{
    if (Run(program, registers with { A = a }).SequenceEqual(program))
    {
        foundA = a;
        break;
    }

    if (iteration == 0)
    {
        continue;
    }

    // Normally A and B registers matter, but in this case they are always set based on previous A
    queue.EnqueueRange(
        Enumerable
            .Range(0, 8)
            .Select(remainder => a * 8 + remainder)
            .Where(aPrevious => Run(program, registers with { A = aPrevious }).First() == program[iteration - 1])
            .Select(aPrevious => (aPrevious, iteration - 1)));
}

Console.WriteLine(foundA);
return;

static IEnumerable<long> Run(long[] program, Registers registers)
{
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
            case Out @out: yield return @out.GetOutput(operand, registers); break;
            case IRegisterInstruction registerInstruction:
                registers = registerInstruction.Execute(operand, registers);
                break;
        }

        instructionPointer += 2;
    }
}

static IInstruction ParseInstruction(long opcode) =>
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
    Registers Execute(long operand, Registers registers);
}

internal static class ComboOperand
{
    public static long CalculateValue(long operand, Registers registers) =>
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
    public Registers Execute(long operand, Registers registers) =>
        registers with
        {
            A = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}

internal class Bxl : IRegisterInstruction
{
    public Registers Execute(long operand, Registers registers) => registers with { B = registers.B ^ operand };
}

internal class Bst : IRegisterInstruction
{
    public Registers Execute(long operand, Registers registers) =>
        registers with { B = ComboOperand.CalculateValue(operand, registers) % 8 };
}

internal class Jnz : IInstruction
{
    public long? GetNextInstructionPointer(long operand, Registers registers) => registers.A == 0 ? null : operand;
}

internal class Bxc : IRegisterInstruction
{
    public Registers Execute(long operand, Registers registers) => registers with { B = registers.B ^ registers.C };
}

internal class Out : IInstruction
{
    public long GetOutput(long operand, Registers registers) => (ComboOperand.CalculateValue(operand, registers) % 8);
}

internal class Bdv : IRegisterInstruction
{
    public Registers Execute(long operand, Registers registers) =>
        registers with
        {
            B = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}

internal class Cdv : IRegisterInstruction
{
    public Registers Execute(long operand, Registers registers) =>
        registers with
        {
            C = (long)(registers.A / (Math.Pow(2, ComboOperand.CalculateValue(operand, registers))))
        };
}