var map = File
    .ReadLines("input.txt")
    .Index()
    .SelectMany(x => x.Item.Index().Select(y => (X: x.Index, Y: y.Index, Value: y.Item)))
    .ToArray();

var walls = map
    .Where(tile => tile.Value == '#')
    .Select(tile => (tile.X, tile.Y))
    .ToHashSet();

var start = map
    .Where(tile => tile.Value == 'S')
    .Select(tile => (tile.X, tile.Y))
    .Single();

var end = map
    .Where(tile => tile.Value == 'E')
    .Select(tile => (tile.X, tile.Y))
    .Single();

const int minSaveTime = 100;

var times = new Dictionary<(int X, int Y), int> { { start, 0 } };
var currentTile = start;
var time = 0;
while (currentTile != end)
{
    time++;
    currentTile = new[]
    {
        (currentTile.X - 1, currentTile.Y),
        (currentTile.X + 1, currentTile.Y),
        (currentTile.X, currentTile.Y - 1),
        (currentTile.X, currentTile.Y + 1)
    }.Single(tile => !walls.Contains(tile) && times.TryAdd(tile, time));
}

var count = times.Keys
    .SelectMany(cheatStart => GetCheats(cheatStart, walls, times)
        .Select(cheat => times[cheat.End] - times[cheatStart] - cheat.CheatTime))
    .Count(t => t >= minSaveTime);

Console.WriteLine(count);
return;

static IEnumerable<((int X, int Y) End, int CheatTime)> GetCheats(
    (int X, int Y) start,
    HashSet<(int, int)> walls,
    IDictionary<(int, int), int> times)
{
    var queue = new PriorityQueue<((int X, int Y) Coordinate, int cheatTime), int>([((start, 0), 0)]);
    var foundCheatPaths = new Dictionary<(int X, int Y), int>();
    var visitedTiles = new Dictionary<(int, int), int>();
    var startTileTime = times[start];
    while (queue.TryDequeue(out var item, out _))
    {
        var (coordinate, cheatTime) = item;

        if (!walls.Contains(coordinate) && !times.ContainsKey(coordinate))
        {
            // Out of bounds
            continue;
        }

        if (visitedTiles.TryGetValue(coordinate, out var visitedTileCheatTime) && visitedTileCheatTime <= cheatTime)
        {
            // Already visited this tile with a lower or equal cheat time
            continue;
        }

        visitedTiles[coordinate] = cheatTime;

        // Check if this is a non-wall tile and that it's a tile that is later in the track
        if (times.TryGetValue(coordinate, out var endTileTime) && endTileTime > startTileTime)
        {
            // Store lowest cheat time needed to reach this track tile
            if (!foundCheatPaths.TryGetValue(coordinate, out var previousCheatTime) || cheatTime < previousCheatTime)
            {
                foundCheatPaths[coordinate] = cheatTime;
            }
        }

        if (cheatTime == 20)
        {
            continue;
        }

        queue.EnqueueRange(
            new[]
                {
                    coordinate with { X = coordinate.X - 1 },
                    coordinate with { X = coordinate.X + 1 },
                    coordinate with { Y = coordinate.Y - 1 },
                    coordinate with { Y = coordinate.Y + 1 },
                }
                .Select(c => ((c, cheatTime + 1), -(cheatTime + 1))));
    }

    return foundCheatPaths.Select(kvp => (kvp.Key, kvp.Value));
}