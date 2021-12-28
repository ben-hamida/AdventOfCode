int[] lines = File.ReadLines("input.txt").Select(int.Parse).ToArray();

int result = 0;

for (int i = 1; i < lines.Length; i++)
{
    if (lines[i] > lines[i-1])
    {
        result++;
    }
}

Console.WriteLine(result);