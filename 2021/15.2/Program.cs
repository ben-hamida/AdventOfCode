
string[] lines = File.ReadLines("input.txt").ToArray();

int[,] originalMap = new int[100, 100];
for (int x = 0; x < 100; x++)
{
    for (int y = 0; y < 100; y++)
    {
        originalMap[x, y] = lines[x][y] - 48;
    }
}

int[,] riskMap = new int[500, 500];
for (int i = 0; i < 5; i++)
{
    for (int j = 0; j < 5; j++)
    {
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                int value = originalMap[x, y] + i + j;
                riskMap[i * 100 + x, j * 100 + y] = value <= 9 ? value : value - 9;
            }
        }
    }
}

int leastRisk = 0;

PriorityQueue<Search, int> queue = new();
queue.Enqueue(new Search((0, 0), 0, riskMap, new int?[500, 500]), 0);

while (queue.TryDequeue(out Search? search, out int risk))
{
    if (search.HasReachedEnd())
    {
        leastRisk = risk;
        break;
    }

    if (search.MoveDown(out Search newSearch))
    {
        queue.Enqueue(newSearch, newSearch.Risk);
    }

    if (search.MoveUp(out newSearch))
    {
        queue.Enqueue(newSearch, newSearch.Risk);
    }

    if (search.MoveLeft(out newSearch))
    {
        queue.Enqueue(newSearch, newSearch.Risk);
    }

    if (search.MoveRight(out newSearch))
    {
        queue.Enqueue(newSearch, newSearch.Risk);
    }
}

Console.WriteLine(leastRisk);

internal sealed class Search
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

    public bool HasReachedEnd() => Position.x == 499 && Position.y == 499;

    private bool Move(int x, int y, out Search search)
    {
        if (x is < 0 or > 499 || y is < 0 or > 499)
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
