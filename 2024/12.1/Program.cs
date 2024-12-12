using System.Collections.Immutable;

var plots = File.ReadAllLines("input.txt")
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (X: x.Index, Y: y.Index, Type: y.Item)))
    .ToArray()
    .Select(c => new Plot(c.Type, [(c.X, c.Y)])).ToList();

while (true)
{
    var newPlots = new List<Plot>();
    var mergedPlots = new HashSet<Plot>(ReferenceEqualityComparer.Instance);

    foreach (var plot in plots)
    {
        if (mergedPlots.Contains(plot))
        {
            continue;
        }

        var neighboringPlotsOfSameType = plots
            .Where(p => p.Type == plot.Type)
            .Where(p => p.Coordinates
                .Any(c1 => plot.Coordinates
                    .Any(c2 =>
                        (c1.Y == c2.Y && Math.Abs(c1.X - c2.X) == 1) ||
                        (c1.X == c2.X && Math.Abs(c1.Y - c2.Y) == 1))))
            .ToArray();

        newPlots.Add(plot with
        {
            Coordinates = plot.Coordinates.Union(neighboringPlotsOfSameType.SelectMany(p => p.Coordinates))
        });

        foreach (var p in neighboringPlotsOfSameType)
        {
            mergedPlots.Add(p);
        }
    }

    if (newPlots.Count == plots.Count)
    {
        break;
    }

    plots = newPlots;
}

var sum = plots.Sum(plot => plot.Area * plot.Perimeter);

Console.WriteLine(sum);

internal record Plot(char Type, ImmutableHashSet<(int X, int Y)> Coordinates)
{
    public int Area => Coordinates.Count;

    public int Perimeter => Coordinates
        .SelectMany(
            c => new (int X, int Y)[]
            {
                (c.X - 1, c.Y),
                (c.X + 1, c.Y),
                (c.X, c.Y - 1),
                (c.X, c.Y + 1)
            }.Where(neighbor => !Coordinates.Contains(neighbor)))
        .Count();
}