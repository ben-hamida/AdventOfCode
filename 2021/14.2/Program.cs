string[] lines = File.ReadLines("input.txt").ToArray();

string template = lines[0];

Dictionary<(char first, char second), char> rules = lines
    .Skip(2)
    .Select(ParseRule)
    .ToDictionary(x => (x.first, x.second), x => x.insertion);

Dictionary<(char first, char second), long> pairCounts = rules
    .ToDictionary(x => x.Key, _ => 0L);

Dictionary<char, long> characterCounts = pairCounts.Keys
    .Select(x => x.first)
    .Concat(pairCounts.Keys
        .Select(x => x.second))
    .Distinct()
    .ToDictionary(x => x, _ => 0L);

// Initialize counts
for (int i = 0; i < template.Length - 1; i++)
{
    pairCounts[(template[i], template[i + 1])]++;
    characterCounts[template[i]]++;
}

characterCounts[template[^1]]++;

for (int step = 0; step < 40; step++)
{
    Dictionary<(char first, char second), long> newPairCounts =rules.ToDictionary(x => x.Key, _ => 0L);
    foreach (var pair in pairCounts.Keys)
    {
        char insertion = rules[pair];
        var firstNewPair = (pair.first, insertion);
        var secondNewPair = (insertion, pair.second);
        newPairCounts[firstNewPair] += pairCounts[pair];
        newPairCounts[secondNewPair] += pairCounts[pair];
        characterCounts[insertion] += pairCounts[pair];
    }

    pairCounts = newPairCounts;
}

Console.WriteLine(characterCounts.Values.Max() - characterCounts.Values.Min());

static (char first, char second, char insertion) ParseRule(string line)
{
    string[] parts = line.Split(" -> ");
    return (parts[0][0], parts[0][1], parts[1][0]);
}