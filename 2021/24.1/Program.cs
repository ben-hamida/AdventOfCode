// Assuming 14 iterations where each iteration starts after an input:
// Formula:
// if (w = z(i-1) % 26 + b) -> z = z(i-1)/a
// else                     -> z = (z(i-1)/a) * 26 + w + c
// where z(i-1) is z at the previous iteration,
// 'a' is constant at line 4 from the input (div z a)
// 'b' is constant at line 5 from the input (add x b)
// 'c' is constant at line 15 from the input (add y c), 3 from next input
// x and y don't matter!

string[] lines = File.ReadLines("input.txt").ToArray();
Constants[] constants = Parse(lines).ToArray();
long[] maxZ = CalculateMaxZ(constants);
int[] startingNumber = Enumerable.Repeat(1, 14).ToArray();
long? result = Search(0, 0, startingNumber);
Console.WriteLine(result);

long? Search(int index, long z, int[] currentNumber)
{
    if (index == 14)
    {
        return z == 0 ? long.Parse(string.Join("", currentNumber)) : null;
    }

    if (z > maxZ[index])
    {
        return null;
    }

    for (int i = 9; i >= -1; i--)
    {
        currentNumber[index] = i;
        long nextZ = GetNextZ(z, constants[index], currentNumber[index]);
        long? resultOfSearchAtNextPosition = Search(index + 1, nextZ, currentNumber);
        if (resultOfSearchAtNextPosition != null)
        {
            return resultOfSearchAtNextPosition;
        }
    }

    return null;
}

static long GetNextZ(long z, Constants constants, int w) =>
    w == z % 26 + constants.B
        ? z / constants.A
        : z / constants.A * 26 + w + constants.C;

static IEnumerable<Constants> Parse(string[] lines)
{
    for (int i = 0; i < 14; i++)
    {
        yield return new Constants(
            int.Parse(lines[i * 18 + 4][6..]),
            int.Parse(lines[i * 18 + 5][6..]),
            int.Parse(lines[i * 18 + 15][6..]));
    }
}

static long[] CalculateMaxZ(IReadOnlyCollection<Constants> constants) => Enumerable.Range(0, constants.Count)
    .Select(index => constants
        .Skip(index)
        .Select(constant => (long)constant.A)
        .Aggregate((x, y) => x * y))
    .ToArray();

internal record Constants(int A, int B, int C);