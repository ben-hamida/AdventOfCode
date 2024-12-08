using System.Collections.Immutable;

var count = File.ReadLines("input.txt")
    .Index()
    .SelectMany(x => x.Item.Index().Select(y => (Content: y.Item, X: x.Index, Y: y.Index)))
    .Where(item => item.Content != '.')
    .GroupBy(position => position.Content, position => (position.X, position.Y))
    .Select(positions => positions
        .SelectMany((a, i) => positions
            .Skip(i + 1)
            .SelectMany<(int X, int Y), (int X, int Y)>(b => [
                ..Enumerable
                    .Range(0, int.MaxValue)
                    .Select<int, (int X, int Y)>(multiplier => (a.X + (b.X - a.X) * multiplier, a.Y + (b.Y - a.Y) * multiplier))
                    .TakeWhile(result => IsInsideMap(result.X, result.Y)),
                ..Enumerable
                    .Range(0, int.MaxValue)
                    .Select<int, (int X, int Y)>(multiplier => (b.X + (a.X - b.X) * multiplier, b.Y + (a.Y - b.Y) * multiplier))
                    .TakeWhile(result => IsInsideMap(result.X, result.Y))
            ])))
    .Aggregate(ImmutableHashSet.Create<(int X, int Y)>(), (acc, antinodes) => acc.Union(antinodes))
    .Count;

Console.WriteLine(count);
return;

bool IsInsideMap(int x, int y) => x is >= 0 and < 50 && y is >= 0 and < 50;