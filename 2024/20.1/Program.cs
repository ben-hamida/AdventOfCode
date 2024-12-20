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
    .SelectMany(cheatStart => new ((int X, int Y) Middle, (int X, int Y) End)[]
        {
            (cheatStart with { X = cheatStart.X - 1 }, cheatStart with { X = cheatStart.X - 2 }),
            (cheatStart with { X = cheatStart.X + 1 }, cheatStart with { X = cheatStart.X + 2 }),
            (cheatStart with { Y = cheatStart.Y - 1 }, cheatStart with { Y = cheatStart.Y - 2 }),
            (cheatStart with { Y = cheatStart.Y + 1 }, cheatStart with { Y = cheatStart.Y + 2 }),
        }
        .Where(cheat =>
            times.ContainsKey(cheat.End) &&
            walls.Contains(cheat.Middle) &&
            times[cheat.End] > times[cheatStart])
        .Select(cheat => times[cheat.End] - times[cheatStart] - 2))
    .Count(t => t >= 100);

Console.WriteLine(count);