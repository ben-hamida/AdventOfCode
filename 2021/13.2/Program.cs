string[] lines = File.ReadLines("input.txt").ToArray();

(int x, int y)[] dots = lines
    .TakeWhile(IsNotEmpty)
    .Select(ParseCoordinate)
    .ToArray();

IEnumerable<(FoldingAxis axis, int position)> instructions = lines
    .Reverse()
    .TakeWhile(IsNotEmpty)
    .Reverse()
    .Select(ParseFoldingInstruction);

HashSet<(int x, int y)> result = new(dots);
foreach ((FoldingAxis axis, int position) in instructions)
{
    result = Fold(result, axis, position);
}

Print(result);

static void Print(IReadOnlySet<(int x, int y)> dots)
{
    int maxX = dots.MaxBy(c => c.x).x;
    int maxY = dots.MaxBy(c => c.y).y;
    int minX = dots.MinBy(c => c.x).x;
    int minY = dots.MinBy(c => c.y).y;

    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            Console.Write(dots.Contains((x, y)) ? "#" : ".");
        }

        Console.WriteLine();
    }
}

static HashSet<(int x, int y)> Fold(IEnumerable<(int x, int y)> dots, FoldingAxis axis, int line)
{
    HashSet<(int x, int y)> result = new();

    foreach ((int x, int y) dot in dots)
    {
        if (axis == FoldingAxis.X)
        {
            if (dot.x < line)
            {
                result.Add(dot);
                continue;
            }

            int distance = dot.x - line;
            result.Add((line - distance, dot.y));
        }
        else if (axis == FoldingAxis.Y)
        {
            if (dot.y < line)
            {
                result.Add(dot);
                continue;
            }

            int distance = dot.y - line;
            result.Add((dot.x, line - distance));
        }
    }

    return result;
}

(int x, int y) ParseCoordinate(string input)
{
    string[] parts = input.Split(',');
    return (int.Parse(parts[0]), int.Parse(parts[1]));
}

(FoldingAxis axis, int position) ParseFoldingInstruction(string line)
{
    string[] parts = line.Split(' ').Last().Split('=');
    return (Enum.Parse<FoldingAxis>(parts[0], true), int.Parse(parts[1]));
}

bool IsNotEmpty(string line) => !string.IsNullOrWhiteSpace(line);

internal enum FoldingAxis{ X, Y }