var lines = File.ReadLines("input.txt").ToArray();

var count = 0;
for (var i = 1; i <  lines.Length -1; i++)
{
    for (var j = 1; j < lines[0].Length -1; j++)
    {
        if(lines[i][j] == 'A' &&
           ((lines[i - 1][j - 1] == 'M' && lines[i + 1][j + 1] == 'S') ||
           (lines[i - 1][j - 1] == 'S' && lines[i + 1][j + 1] == 'M')) &&
           ((lines[i - 1][j + 1] == 'M' && lines[i + 1][j - 1] == 'S') ||
            (lines[i - 1][j + 1] == 'S' && lines[i + 1][j - 1] == 'M')))
        {
            count++;
        }
    }
}

Console.WriteLine(count);