using System.Collections.Immutable;

const int size = 71;
const int numberOfBytes = 1024;

var byteCoordinates = File
    .ReadAllLines("input.txt")
    .Take(numberOfBytes)
    .Select(line => line.Split(','))
    .Select(parts => (X: int.Parse(parts[0]), Y: int.Parse(parts[1])))
    .ToHashSet();

var endCoordinate = (X: size - 1, Y: size - 1);

var queue = new PriorityQueue<Search, int>(
    [(new Search([(0,0)]), GetDistance(endCoordinate, (0, 0)))]);
var minimumSteps = new Dictionary<(int X, int Y), int>();
List<Search> foundPaths = [];
while (queue.TryDequeue(out var search, out _))
{
    var coordinate = search.Path.Last();
    if (byteCoordinates.Contains(coordinate) || coordinate.X < 0 || coordinate.Y < 0 || coordinate.X >= size || coordinate.Y >= size)
    {
        continue;
    }

    if (coordinate == endCoordinate)
    {
        foundPaths.Add(search);
    }

    if (minimumSteps.TryGetValue(coordinate, out var previousMinSteps) &&
        previousMinSteps <= search.NumberOfSteps)
    {
        continue;
    }

    minimumSteps[coordinate] = search.NumberOfSteps;

    queue.EnqueueRange(
        new[]
        {
            coordinate with{ X = coordinate.X + 1},
            coordinate with{ X = coordinate.X - 1},
            coordinate with{ Y = coordinate.Y + 1},
            coordinate with{ Y = coordinate.Y - 1},
        }
            .Select(c => (new Search(search.Path.Add(c)), GetDistance(endCoordinate, c)))
    );
}

// PrintMap(foundPath!, byteCoordinates);
Console.WriteLine(foundPaths.MinBy(s => s.NumberOfSteps)?.NumberOfSteps);
return;

static int GetDistance((int X, int Y) a, (int X, int Y) b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
internal record Search(ImmutableList<(int X, int Y)> Path)
{
    public int NumberOfSteps => Path.Count - 1;
}
