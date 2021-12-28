string[] lines = File.ReadLines("input.txt").ToArray();

HashSet<Cucumber> cucumbers = new(ParseCucumbers(lines), new PositionComparer());

int steps = 1;
while (MoveCucumbers(cucumbers))
{
    steps++;
}

Console.WriteLine(steps);

static bool MoveCucumbers(ISet<Cucumber> cucumbers) =>
    MoveCucumbersInDirection(cucumbers, Direction.East) |
    MoveCucumbersInDirection(cucumbers, Direction.South);

static bool MoveCucumbersInDirection(ISet<Cucumber> cucumbers, Direction direction)
{
    List<Cucumber> movedCucumbers = new();
    foreach (Cucumber cucumber in cucumbers.Where(IsFacing(direction)).ToList())
    {
        var cucumberInNewPosition = Move(cucumber);
        if (cucumbers.Add(cucumberInNewPosition))
        {
            movedCucumbers.Add(cucumber);
        }
    }

    foreach (Cucumber movedCucumber in movedCucumbers)
    {
        cucumbers.Remove(movedCucumber);
    }

    return movedCucumbers.Any();
}

static Cucumber Move(Cucumber cucumber) => cucumber.Direction == Direction.East
    ? cucumber with { Y = cucumber.Y == 138 ? 0 : cucumber.Y + 1 }
    : cucumber with { X = cucumber.X == 136 ? 0 : cucumber.X + 1 };

static Func<Cucumber, bool> IsFacing(Direction direction) => cucumber => cucumber.Direction == direction;

static IEnumerable<Cucumber> ParseCucumbers(string[] lines)
{
    for (int x = 0; x < 137; x++)
    {
        for (int y = 0; y < 139; y++)
        {
            Direction? direction = ParseDirection(lines[x][y]);
            if (direction.HasValue)
            {
                yield return new Cucumber(x, y, direction.Value);
            }
        }
    }
}

static Direction? ParseDirection(char c) => c switch
{
    '>' => Direction.East,
    'v' => Direction.South,
    _ => null
};

internal record Cucumber(int X, int Y, Direction Direction);

internal enum Direction { East, South }

internal class PositionComparer : IEqualityComparer<Cucumber>
{
    public bool Equals(Cucumber? x, Cucumber? y) => x!.X == y!.X && x.Y == y.Y;

    public int GetHashCode(Cucumber obj) => HashCode.Combine(obj.X, obj.Y);
}