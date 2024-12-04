var lines = File.ReadLines("input.txt").ToArray();

var count = 0;
for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[0].Length; j++)
    {
        if (lines[i][j] != 'X')
        {
            continue;
        }

        try
        {
            if (lines[i + 1][j] == 'M' && lines[i + 2][j] == 'A' && lines[i + 3][j] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i - 1][j] == 'M' && lines[i - 2][j] == 'A' && lines[i - 3][j] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i][j + 1] == 'M' && lines[i][j + 2] == 'A' && lines[i][j + 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i][j - 1] == 'M' && lines[i][j - 2] == 'A' && lines[i][j - 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i + 1][j + 1] == 'M' && lines[i + 2][j + 2] == 'A' && lines[i + 3][j + 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i - 1][j - 1] == 'M' && lines[i - 2][j - 2] == 'A' && lines[i - 3][j - 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i - 1][j + 1] == 'M' && lines[i - 2][j + 2] == 'A' && lines[i - 3][j + 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (lines[i + 1][j - 1] == 'M' && lines[i + 2][j - 2] == 'A' && lines[i + 3][j - 3] == 'S')
            {
                count++;
            }
        }
        catch (IndexOutOfRangeException)
        {
        }
    }
}

Console.WriteLine(count);