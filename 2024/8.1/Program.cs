using System.Collections.Immutable;

var count = File.ReadLines("input.txt")
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (Content: y.Item, X: x.Index, Y: y.Index)))
    .Where(item => item.Content != '.')
    .GroupBy(position => position.Content, position => (position.X, position.Y))
    .Select(positions => positions
        .SelectMany((first, i) => positions
            .Skip(i + 1)
            .SelectMany(second => new []
            {
                (first.X + (second.X - first.X) * 2, first.Y + (second.Y - first.Y) * 2),
                (second.X + (first.X - second.X) * 2, second.Y + (first.Y - second.Y) * 2)
            })))
    .Aggregate(ImmutableHashSet.Create<(int X, int Y)>(), (acc, antinodes) => acc.Union(antinodes))
    .Count(c => c is { X: >= 0 and < 50, Y: >= 0 and < 50 });

Console.WriteLine(count);