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

var sum = plots.Sum(plot => plot.Area * plot.Sides);

Console.WriteLine(sum);

internal record Plot(char Type, ImmutableHashSet<(int X, int Y)> Coordinates)
{
    public int Area => Coordinates.Count;

    public int Sides
    {
        get
        {
            var perimeters = Coordinates
                .SelectMany(c => new ((int X, int Y) Coordinate, Direction Direction, (int X, int Y) Neighbor)[]
                    {
                        (c, Direction.Up, (c.X - 1, c.Y)),
                        (c, Direction.Down, (c.X + 1, c.Y)),
                        (c, Direction.Left, (c.X, c.Y - 1)),
                        (c, Direction.Right, (c.X, c.Y + 1))
                    }
                    .Where(x => !Coordinates.Contains(x.Neighbor)))
                .Select(c => (c.Coordinate.X, c.Coordinate.Y, c.Direction))
                .ToList();

            var count = 0;
            while (perimeters.Count > 0)
            {
                var perimeter = perimeters[0];
                perimeters.RemoveAt(0);
                count++;
                if (perimeter.Direction is Direction.Up or Direction.Down)
                {
                    // search left
                    (int X, int Y, Direction Direction)? left = perimeter;
                    do
                    {
                        left = perimeters
                            .Where(p =>
                                p.Direction == perimeter.Direction &&
                                p.X == perimeter.X &&
                                left.Value.Y - p.Y == 1)
                            .Cast<(int X, int Y, Direction Direction)?>()
                            .SingleOrDefault();

                        if (left is not null)
                        {
                            perimeters.Remove(left.Value);
                        }
                    } while (left is not null);

                    // search right
                    (int X, int Y, Direction Direction)? right = perimeter;
                    do
                    {
                        right = perimeters
                            .Where(p =>
                                p.Direction == perimeter.Direction &&
                                p.X == perimeter.X &&
                                right.Value.Y - p.Y == -1)
                            .Cast<(int X, int Y, Direction Direction)?>()
                            .SingleOrDefault();

                        if (right is not null)
                        {
                            perimeters.Remove(right.Value);
                        }
                    } while (right is not null);
                }
                else
                {
                    // search up
                    (int X, int Y, Direction Direction)? up = perimeter;
                    do
                    {
                        up = perimeters
                            .Where(p =>
                                p.Direction == perimeter.Direction &&
                                p.Y == perimeter.Y &&
                                up.Value.X - p.X == 1)
                            .Cast<(int X, int Y, Direction Direction)?>()
                            .SingleOrDefault();

                        if (up is not null)
                        {
                            perimeters.Remove(up.Value);
                        }
                    } while (up is not null);

                    // search down
                    (int X, int Y, Direction Direction)? down = perimeter;
                    do
                    {
                        down = perimeters
                            .Where(p =>
                                p.Direction == perimeter.Direction &&
                                p.Y == perimeter.Y &&
                                down.Value.X - p.X == -1)
                            .Cast<(int X, int Y, Direction Direction)?>()
                            .SingleOrDefault();

                        if (down is not null)
                        {
                            perimeters.Remove(down.Value);
                        }
                    } while (down is not null);
                }
            }

            return count;
        }
    }
}

internal enum Direction
{
    Up,
    Down,
    Left,
    Right
}