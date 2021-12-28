var connections = new (string from, string to)[]
{
    ("vn", "DD"),
    ("qm", "DD"),
    ("MV", "xy"),
    ("end", "xy"),
    ("KG", "end"),
    ("end", "kw"),
    ("qm", "xy"),
    ("start", "vn"),
    ("MV", "vn"),
    ("vn", "ko"),
    ("lj", "KG"),
    ("DD", "xy"),
    ("lj", "kh"),
    ("lj", "MV"),
    ("ko", "MV"),
    ("kw", "qm"),
    ("qm", "MV"),
    ("lj", "kw"),
    ("VH", "lj"),
    ("ko", "qm"),
    ("ko", "start"),
    ("MV", "start"),
    ("DD", "ko")
};

int numberOfPaths = 0;

ExplorePath("start", new(), false);

Console.WriteLine(numberOfPaths);

void ExplorePath(string currentCave, HashSet<string> visitedSmallCaves, bool hasVisitedSmallCaveTwice)
{
    if (currentCave == "end")
    {
        numberOfPaths++;
        return;
    }

    if (visitedSmallCaves.Contains(currentCave))
    {
        if (hasVisitedSmallCaveTwice || currentCave == "start")
        {
            return;
        }

        hasVisitedSmallCaveTwice = true;
    }

    if (currentCave.All(char.IsLower))
    {
        visitedSmallCaves.Add(currentCave);
    }

    string[] connectingCaves = connections
        .Where(c => c.from == currentCave || c.to == currentCave)
        .Select(c => c.from == currentCave ? c.to : c.from)
        .ToArray();
    
    foreach (var cave in connectingCaves)
    {
        ExplorePath(cave, new(visitedSmallCaves), hasVisitedSmallCaveTwice);
    }
}
