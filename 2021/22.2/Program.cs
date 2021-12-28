var lines = File.ReadLines("input.txt");

var instructions = lines.Select(Parse);
var processedInstructions = new List<(bool on, Range range)>();

foreach (var instruction in instructions)
{
    var intersections = new List<(bool on, Range range)>();
    foreach ((bool on, Range? range) in processedInstructions)
    {
        Range? intersection = GetIntersection(instruction.Range, range);
        if (intersection != null)
        {
            intersections.Add((!on, intersection));
        }
    }

    processedInstructions.AddRange(intersections);

    if (instruction.On)
    {
        processedInstructions.Add(instruction);
    }
}

long numberOfOnCubes = 0;
foreach ((bool on, Range? range) in processedInstructions)
{
    if (on)
    {
        numberOfOnCubes += CalculateVolume(range);
    }
    else
    {
        numberOfOnCubes -= CalculateVolume(range);
    }
}

Console.WriteLine(numberOfOnCubes);

static (bool On, Range Range) Parse(string line)
{
    int spaceIndex = line.IndexOf(' ');
    bool on = line[..spaceIndex] == "on";
    string[] parts = line[(spaceIndex + 1)..].Split(',');
    (int xFrom, int xTo) = ParseOneDimensionalRange(parts[0]);
    (int yFrom, int yTo) = ParseOneDimensionalRange(parts[1]);
    (int zFrom, int zTo) = ParseOneDimensionalRange(parts[2]);
    return (on, new Range(
        new Coordinate(xFrom, yFrom, zFrom),
        new Coordinate(xTo, yTo, zTo)));
}

static (int from, int to) ParseOneDimensionalRange(string input)
{
    string[] parts = input[2..].Split("..");
    return (int.Parse(parts[0]), int.Parse(parts[1]));
}

static long CalculateVolume(Range range) =>
    (range.To.X - range.From.X + 1) *
    (range.To.Y - range.From.Y + 1) *
    (range.To.Z - range.From.Z + 1);

static Range? GetIntersection(Range first, Range second)
{
    if (first.From.X > second.To.X ||
        second.From.X > first.To.X ||
        first.From.Y > second.To.Y ||
        second.From.Y > first.To.Y ||
        first.From.Z > second.To.Z ||
        second.From.Z > first.To.Z)
    {
        return null;
    }

    return new Range(
        new Coordinate(
            Math.Max(first.From.X, second.From.X),
            Math.Max(first.From.Y, second.From.Y),
            Math.Max(first.From.Z, second.From.Z)),
        new Coordinate(
            Math.Min(first.To.X, second.To.X),
            Math.Min(first.To.Y, second.To.Y),
            Math.Min(first.To.Z, second.To.Z)));
}

internal record Range(Coordinate From, Coordinate To);

internal record Coordinate(long X, long Y, long Z);
