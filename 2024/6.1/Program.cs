var lines = File.ReadLines("input.txt").ToArray();

var map = lines
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (Content: y.Item, X: x.Index, Y: y.Index)))
    .ToArray();

var obstaclePositions = map
    .Where(x => x.Content == '#')
    .Select(x => (x.X, x.Y))
    .ToHashSet();
var guardPosition = map.First(x => x.Content == '^');
var guard = (guardPosition.X, guardPosition.Y, Direction: Direction.Up);

var visited = new HashSet<(int X, int Y)>();

while (IsInsideMap(guard.X, guard.Y))
{
    visited.Add((guard.X, guard.Y));
    var (x, y, _) = guard;
    guard = guard.Direction switch
    {
        Direction.Up when HasObstacle(x - 1, y) => guard with { Direction = Direction.Right },
        Direction.Down when HasObstacle(x + 1, y) => guard with { Direction = Direction.Left },
        Direction.Left when HasObstacle(x, y - 1) => guard with { Direction = Direction.Up },
        Direction.Right when HasObstacle(x, y + 1) => guard with { Direction = Direction.Down },
        Direction.Up => guard with { X = guard.X - 1 },
        Direction.Down => guard with { X = guard.X + 1 },
        Direction.Left => guard with { Y = guard.Y - 1 },
        Direction.Right => guard with { Y = guard.Y + 1 },
        _ => throw new InvalidOperationException()
    };
}

Console.WriteLine(visited.Count);
return;

bool IsInsideMap(int x, int y) => x >= 0 && x < lines.Length && y >= 0 && y < lines[0].Length;

bool HasObstacle(int x, int y) => obstaclePositions.Contains((x, y));

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}