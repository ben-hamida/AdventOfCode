var lines = File.ReadLines("input.txt");

var instructions = lines.Select(Parse).ToArray();

HashSet<(int x, int y, int z)> onCubes = new();

foreach (var instruction in instructions.Take(20))
{
    for (int x = instruction.Range.From.X; x <= instruction.Range.To.X; x++)
    {
        for (int y = instruction.Range.From.Y; y <= instruction.Range.To.Y; y++)
        {
            for (int z = instruction.Range.From.Z; z <= instruction.Range.To.Z; z++)
            {
                if (instruction.On)
                {
                    onCubes.Add((x, y, z));
                }
                else
                {
                    onCubes.Remove((x, y, z));
                }
            }
        }
    }
}

Console.WriteLine(onCubes.Count);

static (bool On, Range Range) Parse(string line)
{
    int spaceIndex = line.IndexOf(' ');
    bool on = line.Substring(0, spaceIndex) == "on";
    string[] parts = line.Substring(spaceIndex + 1).Split(',');
    var xRange = ParseOneDimensionalRange(parts[0]);
    var yRange = ParseOneDimensionalRange(parts[1]);
    var zRange = ParseOneDimensionalRange(parts[2]);
    return (on, new Range(
        new Coordinate(xRange.from, yRange.from, zRange.from),
        new Coordinate(xRange.to, yRange.to, zRange.to)));
}

static (int from, int to) ParseOneDimensionalRange(string input)
{
    string[] parts = input[2..].Split("..");
    return (int.Parse(parts[0]), int.Parse(parts[1]));
}

internal sealed record Coordinate(int X, int Y, int Z);

internal sealed record Range(Coordinate From, Coordinate To);