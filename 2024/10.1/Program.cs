var lines = File.ReadLines("input.txt").ToArray();

var coordinates = lines
    .Index()
    .SelectMany(x => x.Item
        .Index()
        .Select(y => (X: x.Index, Y: y.Index, Height: int.Parse(y.Item.ToString()))))
    .ToArray();

var map = coordinates.ToDictionary(a => (a.X, a.Y), a => a.Height);
var searches = new Queue<(int X, int Y, int Height)>(coordinates.Where(c => c.Height == 0));
var score = 0;

while (searches.TryDequeue(out var search))
{
    var neighbors = new (int X, int Y)[]
        {
            (search.X - 1, search.Y),
            (search.X + 1, search.Y),
            (search.X, search.Y - 1),
            (search.X, search.Y + 1)
        }
        .Where(c =>
            c.X >= 0 && c.X < lines.Length &&
            c.Y >= 0 && c.Y < lines[0].Length &&
            map[(c.X, c.Y)] == search.Height + 1)
        .ToList();

    if (search.Height == 8)
    {
        score += neighbors.Count;
    }
    else
    {
        neighbors.ForEach(n => searches.Enqueue((n.X, n.Y, search.Height + 1)));
    }
}

Console.WriteLine(score);