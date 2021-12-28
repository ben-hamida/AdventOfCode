string[] lines = File.ReadLines("input.txt").ToArray();

string template = lines[0];

var rules = lines.Skip(2).Select(ParseRule).ToArray();

List<char> current = template.ToList();

for (int step = 0; step < 10; step++)
{
    int index = 0;
    while (index < current.Count - 1)
    {
        char first = current[index];
        char second = current[index + 1];
        char insertion = GetInsertion(first, second);
        current.Insert(index + 1,insertion);
        index += 2;
    }
}

int[] counts = current.Distinct().ToArray().Select(c => current.Count(x => x == c)).ToArray();

Console.WriteLine(counts.Max() - counts.Min());

static (char first, char second, char insertion) ParseRule(string line)
{
    string[] parts = line.Split(" -> ");
    return (parts[0][0], parts[0][1], parts[1][0]);
}

char GetInsertion(char first, char second) => rules.Single(r=> r.first == first && r.second == second).insertion;
