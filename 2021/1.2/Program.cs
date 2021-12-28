int[] lines = File.ReadLines("input.txt").Select(int.Parse).ToArray();

var transformed = new List<int>();

for (int i = 1; i < lines.Length - 1; i++)
{
    transformed.Add(lines[i - 1] + lines[i] + lines[i + 1]);
}

int result = 0;

for (int i = 1; i < transformed.Count; i++)
{
    if (transformed[i] > transformed[i-1])
    {
        result++;
    }
}

Console.WriteLine(result);