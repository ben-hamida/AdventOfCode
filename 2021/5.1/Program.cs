var inputLines = File.ReadLines("input.txt");

Line[] lines = inputLines.Select(x => x.Split(" -> ")).Select(ParseLine).Where(IsNonDiagonal).ToArray();

Coordinate[][] lineSegments = lines.Select(GetAllCoordinates).ToArray();

List<Coordinate> overlaps = new();

for (int i = 0; i < lines.Length - 1; i++)
{
    for (int j = i + 1; j < lines.Length; j++)
    {
        var intersectingCoordinates = lineSegments[i].Intersect(lineSegments[j]);
        overlaps.AddRange(intersectingCoordinates);
    } 
}

int distinctNumberOfOverlaps = overlaps.Distinct().Count();

Console.WriteLine(distinctNumberOfOverlaps);

static Coordinate[] GetAllCoordinates(Line line)
{
    if (IsHorizontal(line))
    {
        return MakeRange(line.Start.X, line.Finish.X).Select(x => new Coordinate(x, line.Start.Y)).ToArray();
    }

    if (IsVertical(line))
    {
        return MakeRange(line.Start.Y, line.Finish.Y).Select(y => new Coordinate(line.Start.X, y)).ToArray();
    }

    return Array.Empty<Coordinate>();
}

static Line ParseLine(string[] parts) => new(
        ParseCoordinate(parts[0].Split(',')),
        ParseCoordinate(parts[1].Split(',')));

static Coordinate ParseCoordinate(string[] parts) => new(int.Parse(parts[0]), int.Parse(parts[1]));

static bool IsNonDiagonal(Line line) => IsHorizontal(line) || IsVertical(line);

static bool IsHorizontal(Line line) => line.Start.Y == line.Finish.Y;

static bool IsVertical(Line line) => line.Start.X == line.Finish.X;

static IEnumerable<int> MakeRange(int start, int end)
{
    yield return start;

    int i = start;
    while (i != end)
    {
        i += Math.Sign(end - start);
        yield return i;
    }
}

internal record Coordinate(int X, int Y);

internal record Line(Coordinate Start, Coordinate Finish);
