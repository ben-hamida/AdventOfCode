var lines = File.ReadLines("input.txt").ToArray();

var coordinates = lines
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (X: x.Index, Y: y.Index, Height: int.Parse(y.Item.ToString()))))
    .ToArray();

var map = coordinates.ToDictionary(a => (a.X, a.Y), a => a.Height);

var searches = new Queue<Search>(coordinates
    .Where(c => c.Height == 0)
    .Select(c => new Search(new TrailHead(c.X, c.Y), (c.X, c.Y), 0)));

var foundTrails = new HashSet<(TrailHead Head, TrailEnd End)>();

while (searches.TryDequeue(out var search))
{
    Console.WriteLine("Searching: " + search);
    var neighbors = new (int X, int Y)[]
        {
            (search.CurrentPosition.X - 1, search.CurrentPosition.Y),
            (search.CurrentPosition.X + 1, search.CurrentPosition.Y),
            (search.CurrentPosition.X, search.CurrentPosition.Y - 1),
            (search.CurrentPosition.X, search.CurrentPosition.Y + 1)
        }
        .Where(c =>
            c.X >= 0 && c.X < lines.Length &&
            c.Y >= 0 && c.Y < lines[0].Length &&
            map[(c.X, c.Y)] == search.CurrentHeight + 1)
        .ToList();

    foreach (var neighbor in neighbors)
    {
        Console.WriteLine("Neighbor: " + neighbor + " Height: " + map[neighbor]);
    }

    if (search.CurrentHeight == 8)
    {
        neighbors.ForEach(n => foundTrails.Add((search.TrailHead, new TrailEnd(n.X, n.Y))));
    }
    else
    {
        neighbors.ForEach(n => searches.Enqueue(search with
        {
            CurrentPosition = (n.X, n.Y),
            CurrentHeight = search.CurrentHeight + 1
        }));
    }
}

Console.WriteLine(foundTrails.Count);

internal record TrailHead(int X, int Y);
internal record TrailEnd(int X, int Y);
internal record Search(TrailHead TrailHead, (int X, int Y) CurrentPosition, int CurrentHeight);