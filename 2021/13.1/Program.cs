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

(FoldingAxis foldingAxis, int position) = instructions.First();

IEnumerable<(int x, int y)> result = Fold(dots, foldingAxis, position);

Console.WriteLine(result.Count());

static IEnumerable<(int x, int y)> Fold(IEnumerable<(int x, int y)> dots, FoldingAxis axis, int line)
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