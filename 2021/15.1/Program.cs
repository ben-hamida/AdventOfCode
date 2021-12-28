string[] lines = File.ReadLines("input.txt").ToArray();

const int dim = 100;

int[,] riskMap = new int[dim, dim];
for (int x = 0; x < dim; x++)
{
    for (int y = 0; y < dim; y++)
    {
        riskMap[x, y] = lines[x][y] - 48;
    }
}

List<Search> searches = new() { new Search((0, 0), 0, riskMap, new int?[dim, dim]) };
int leastRisk = 9 * 2 * dim;

while (true)
{
    List<Search> newSearches = new();

    foreach (Search search in searches)
    {
        if (search.HasReachedEnd())
        {
            leastRisk = Math.Min(search.Risk, leastRisk);
            continue;
        }

        if (search.Risk > leastRisk)
        {
            continue;
        }

        if (search.MoveDown(out Search newSearch))
        {
            newSearches.Add(newSearch);
        }

        if (search.MoveUp(out newSearch))
        {
            newSearches.Add(newSearch);
        }

        if (search.MoveLeft(out newSearch))
        {
            newSearches.Add(newSearch);
        }

        if (search.MoveRight(out newSearch))
        {
            newSearches.Add(newSearch);
        }     
    }

    searches = newSearches;
    if (!searches.Any())
    {
        break;
    }
}

Console.WriteLine(leastRisk);

internal class Search
{
    private readonly int[,] _riskMap;
    private readonly int?[,] _minimumRisks;

    public Search(
        (int, int) position,
        int risk,
        int[,] riskMap,
        int?[,] minimumRisks)
    {
        Position = position;
        Risk = risk;
        _riskMap = riskMap;
        _minimumRisks = minimumRisks;
    }

    public (int x, int y) Position { get; }

    public int Risk { get; }

    public bool MoveDown(out Search search) => Move(Position.x, Position.y + 1, out search);

    public bool MoveUp(out Search search) => Move(Position.x, Position.y - 1, out search);

    public bool MoveLeft(out Search search) => Move(Position.x - 1, Position.y, out search);

    public bool MoveRight(out Search search) => Move(Position.x + 1, Position.y, out search);

    public bool HasReachedEnd() => Position.x == 99 && Position.y == 99;

    private bool Move(int x, int y, out Search search)
    {
        if (x is < 0 or >= 100 || y is < 0 or >= 100)
        {
            search = this;
            return false;
        }

        int risk = Risk + _riskMap[x, y];
        int? minRisk = _minimumRisks[x, y];
        if(minRisk <= risk)
        {
            search = this;
            return false;
        }

        _minimumRisks[x, y] = risk;
        search = new Search((x, y), risk, _riskMap, _minimumRisks);
        return true;
    }
}
