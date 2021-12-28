var inputLines = File.ReadLines("input.txt");

Line[] lines = inputLines.Select(x => x.Split(" -> ")).Select(ParseLine).ToArray();

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

    return MakeDiagonalRange(line.Start, line.Finish).ToArray();
}

static Line ParseLine(string[] parts) => new(
        ParseCoordinate(parts[0].Split(',')),
        ParseCoordinate(parts[1].Split(',')));

static Coordinate ParseCoordinate(string[] parts) => new(int.Parse(parts[0]), int.Parse(parts[1]));

static bool IsHorizontal(Line line) => line.Start.Y == line.Finish.Y;

static bool IsVertical(Line line) => line.Start.X == line.Finish.X;

static IEnumerable<int> MakeRange(int start, int end)
{
    yield return start;

    int i = start;
    while (i != end)
    {
        if (end > start)
        {
            i++;
        }
        else if (end < start)
        {
            i--;
        }

        yield return i;
    }
}


static IEnumerable<Coordinate> MakeDiagonalRange(Coordinate start, Coordinate end)
{
    yield return start;

    int x = start.X;
    int y = start.Y;
    while (x != end.X)
    {
        if (end.X > start.X)
        {
            x++;
        }
        else if (end.X < start.X)
        {
            x--;
        }

        if (end.Y > start.Y)
        {
            y++;
        }
        else if (end.Y < start.Y)
        {
            y--;
        }

        yield return new Coordinate(x, y);
    }
}

record Coordinate(int X, int Y);

record Line(Coordinate Start, Coordinate Finish);
