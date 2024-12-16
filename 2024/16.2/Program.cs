using System.Collections.Immutable;

var map = File
    .ReadAllLines("input.txt")
    .Index()
    .SelectMany(x => x.Item.Index().Select(y => (X: x.Index, Y: y.Index, Value: y.Item)))
    .ToArray();

var reindeer = map
    .Where(c => c.Value == 'S')
    .Select(c => (c.X, c.Y, Direction: Direction.Right))
    .Single();

var end = map
    .Where(c => c.Value == 'E')
    .Select(c => (c.X, c.Y))
    .Single();

var walls = map
    .Where(c => c.Value == '#')
    .Select(c => (c.X, c.Y))
    .ToHashSet();

var paths = new PriorityQueue<(int X, int Y, Direction Direction, ImmutableList<(int X, int Y)> Path), long>(
    [((reindeer.X, reindeer.Y, reindeer.Direction, []), 0)]);
var visited = new Dictionary<(int X, int Y, Direction Direction), long>();
var winningPaths = new List<ImmutableList<(int X, int Y)>>();
var winningScore = long.MaxValue;

while (paths.TryDequeue(out var state, out var score))
{
    if (walls.Contains((state.X, state.Y)))
    {
        continue;
    }

    if (visited.TryGetValue((state.X, state.Y, state.Direction), out var previousScore) && previousScore < score)
    {
        continue;
    }

    state.Path = state.Path.Add((state.X, state.Y));

    visited[(state.X, state.Y, state.Direction)] = score;
    if (state.X == end.X && state.Y == end.Y)
    {
        if (score < winningScore)
        {
            winningPaths = [state.Path];
            winningScore = score;
        }
        else if (score == winningScore)
        {
            winningPaths.Add(state.Path);
        }

        continue;
    }

    paths.EnqueueRange(
        state.Direction switch
        {
            Direction.Left =>
            [
                (state with { Y = state.Y - 1 }, score + 1),
                (state with { Direction = Direction.Up }, score + 1000),
                (state with { Direction = Direction.Down }, score + 1000)
            ],
            Direction.Up =>
            [
                (state with { X = state.X - 1 }, score + 1),
                (state with { Direction = Direction.Left }, score + 1000),
                (state with { Direction = Direction.Right }, score + 1000)
            ],
            Direction.Right =>
            [
                (state with { Y = state.Y + 1 }, score + 1),
                (state with { Direction = Direction.Up }, score + 1000),
                (state with { Direction = Direction.Down }, score + 1000)
            ],
            Direction.Down =>
            [
                (state with { X = state.X + 1 }, score + 1),
                (state with { Direction = Direction.Left }, score + 1000),
                (state with { Direction = Direction.Right }, score + 1000)
            ],
            _ => throw new ArgumentOutOfRangeException()
        });
}

Console.WriteLine(winningPaths.SelectMany(p => p).Distinct().Count());


internal enum Direction
{
    Left,
    Up,
    Right,
    Down
}