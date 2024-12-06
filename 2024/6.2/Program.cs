var lines = File.ReadLines("input.txt").ToArray();

var map = lines
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (Content: y.Item, X: x.Index, Y: y.Index)))
    .ToArray();

var initialObstacles = map
    .Where(position => position.Content == '#')
    .Select(position => (position.X, position.Y))
    .ToHashSet();

var guardPosition = map.First(x => x.Content == '^');

var count = map
    .Where(position => position.Content == '.')
    .Count(position => IsLoop(
        (guardPosition.X, guardPosition.Y, Direction: Direction.Up),
        [..initialObstacles, (position.X, position.Y)]));

Console.WriteLine(count);
return;

bool IsLoop((int X, int Y, Direction Direction) initialState, HashSet<(int X, int Y)> obstacles)
{
    var previousStates = new HashSet<(int X, int Y, Direction Direction)>();

    while (IsInsideMap(initialState.X, initialState.Y))
    {
        if (!previousStates.Add(initialState))
        {
            return true;
        }

        var (x, y, _) = initialState;
        initialState = initialState.Direction switch
        {
            Direction.Up when obstacles.Contains((x - 1, y)) => initialState with { Direction = Direction.Right },
            Direction.Down when obstacles.Contains((x + 1, y)) => initialState with { Direction = Direction.Left },
            Direction.Left when obstacles.Contains((x, y - 1)) => initialState with { Direction = Direction.Up },
            Direction.Right when obstacles.Contains((x, y + 1)) => initialState with { Direction = Direction.Down },
            Direction.Up => initialState with { X = initialState.X - 1 },
            Direction.Down => initialState with { X = initialState.X + 1 },
            Direction.Left => initialState with { Y = initialState.Y - 1 },
            Direction.Right => initialState with { Y = initialState.Y + 1 },
            _ => throw new InvalidOperationException()
        };
    }

    return false;
}

bool IsInsideMap(int x, int y) => x >= 0 && x < lines.Length && y >= 0 && y < lines[0].Length;


public enum Direction
{
    Up,
    Right,
    Down,
    Left
}